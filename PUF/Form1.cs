using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PUF
{
    public partial class Form1 : Form
    {
        private const int BLOCK_SIZE = 4;
        private const int CHART_UNIT = 5, CHART_LENGTH = 1025;
        private Color BLUE = Color.FromArgb(52, 43, 134), YELLOW = Color.FromArgb(249, 253, 8);
        private string folder_path = "PUF_result";
        private delegate void Display();
        private string deviceName = "";
        //讀取電路thread
        private SerialPort serialPort1 = new SerialPort();
        private Boolean receiving;
        private Thread receiveThread;
        private List<string> ExternalRead = new List<string>();
        private string actionClick = "";
        private int readCount = 0;
        private int[][] ADC = new int[BLOCK_SIZE][]; //RRAM讀取電壓
        private List<int[][]> ADC_List = new List<int[][]>();
        private int[][] ADC_Bit = new int[BLOCK_SIZE][]; //電壓轉換bit結果
        private List<int[][]> ADC_Bit_List = new List<int[][]>();
        private Boolean UniCheck = false;
        private int[][][] UniArr;
        //圖表變數
        private DataGridViewTextBoxColumn[] Col = new DataGridViewTextBoxColumn[64];
        private int[] zeroCount = new int[BLOCK_SIZE];
        private int[] oneCount = new int[BLOCK_SIZE];
        private Chart[] charts;
        private Series[] bitSeries = new Series[BLOCK_SIZE];
        private int[] xValues = new int[CHART_LENGTH / CHART_UNIT];
        private int[][] yValues = new int[BLOCK_SIZE][];
        private int[][] chTotal = new int[BLOCK_SIZE][]; //暫存直方圖的數值陣列，防止yValues超出陣列上限
        private Color cZero, cOne;
        private Color[][] cBit = new Color[BLOCK_SIZE][];
        //PUFRead方法的變數
        private Boolean PUFReadCheck = false;
        private int PUFReadComplete = 4105;
        //Read方法的變數
        private Boolean ExtReadCheck = false;
        private int ExtReadComplete = 2055;
        //bit reference變數
        private int bitTh1 = 70, bitTh2 = 70, bitTh3 = 70;
        private DataGridView[] refGrids;
        private int[][] refBitTh = new int[BLOCK_SIZE][];
        public Form1()
        {
            InitializeComponent();
            btnPUF.Visible = false; //為了展示，暫時隱藏按鈕。
            //大圖初始化
            for (int i = 0; i < ADC.Length; i++)
            {
                ADC[i] = new int[1024];
                ADC_Bit[i] = new int[1024];
            }

            for (int i = 0; i < Col.Length; i++)
            {
                this.Col[i] = new DataGridViewTextBoxColumn();
                this.Col[i].Width = 10;
                this.Col[i].ReadOnly = true;
                this.Col[i].Resizable = DataGridViewTriState.False;
            }
            dataGridView1.Columns.AddRange(Col);
            dataGridView1.RowCount = 64;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Enabled = false;
            cZero = YELLOW; cOne = BLUE;
            for (int i = 0; i < cBit.Length; i++)
                cBit[i] = new Color[] { cOne, cOne, cZero, cZero };
            //小圖初始化
            refGrids = new DataGridView[] { refGridView1, refGridView2, refGridView3, refGridView4 };
            for (int i = 0; i < refGrids.Length; i++)
            {
                DataGridViewTextBoxColumn[] c = new DataGridViewTextBoxColumn[32];
                for (int j = 0; j < c.Length; j++)
                {
                    c[j] = new DataGridViewTextBoxColumn();
                    c[j].Width = 10;
                    c[j].ReadOnly = true;
                    c[j].Resizable = DataGridViewTriState.False;
                }
                refGrids[i].Columns.AddRange(c);
                refGrids[i].RowCount = 32;
                refGrids[i].ColumnHeadersVisible = false;
                refGrids[i].RowHeadersVisible = false;
                refGrids[i].Enabled = false;
            }
            //reference初始化
            for (int i = 0; i < refBitTh.Length; i++)
                refBitTh[i] = new int[] { bitTh1, bitTh2, bitTh3 };
            txtRef_11.Text = bitTh1.ToString(); txtRef_12.Text = bitTh2.ToString(); txtRef_13.Text = bitTh3.ToString();
            txtRef_21.Text = bitTh1.ToString(); txtRef_22.Text = bitTh2.ToString(); txtRef_23.Text = bitTh3.ToString();
            txtRef_31.Text = bitTh1.ToString(); txtRef_32.Text = bitTh2.ToString(); txtRef_33.Text = bitTh3.ToString();
            txtRef_41.Text = bitTh1.ToString(); txtRef_42.Text = bitTh2.ToString(); txtRef_43.Text = bitTh3.ToString();
            //統計表初始化
            for (int i = 0; i < xValues.Length; i++)
                xValues[i] = CHART_UNIT * i;
            for (int i = 0; i < yValues.Length; i++)
            {
                yValues[i] = new int[CHART_LENGTH / CHART_UNIT];
                chTotal[i] = new int[1025 / CHART_UNIT]; //電壓最高值:1025
            }
            charts = new Chart[] { chart1, chart2, chart3, chart4 };
            for (int i = 0; i < charts.Length; i++)
            {
                charts[i].ChartAreas[0].AxisX.ScaleView.Size = 400;
                charts[i].SetBounds(270, charts[i].Location.Y, charts[i].Size.Width, charts[i].Size.Height);
            }
            for (int i = 0; i < bitSeries.Length; i++)
            {
                bitSeries[i] = new Series();
                charts[i].ChartAreas[0].AxisY.Maximum = 400;
                charts[i].Series.Add(bitSeries[i]);
                bitSeries[i].ChartType = SeriesChartType.Column;
                bitSeries[i].Name = "bitSeries" + i;
                bitSeries[i].IsValueShownAsLabel = true;
            }
            setChartData();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            for (int i = 0; i < refGrids.Length; i++)
                refGrids[i].ClearSelection();

            string[] serialPorts = SerialPort.GetPortNames();
            foreach (string serialPort in serialPorts)
            {
                cbbCOM.Items.Add(serialPort);
                if (cbbCOM.Items.Count > 0)
                {
                    cbbCOM.SelectedIndex = 0;
                }
            }
        }
        private void btnCom_Click(object sender, EventArgs e)
        {
            Button btnCom = (Button)sender;
            if (btnCom.Text == "Open")
            {
                serialPort1.PortName = (string)cbbCOM.SelectedItem;
                serialPort1.BaudRate = 115200;
                serialPort1.DataBits = 8;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Open();
                serialPort1.DiscardInBuffer();
                //btnPUF.Enabled = true;
                btnRead.Enabled = true;
                btnCom.Text = "Close";
                receiving = true;
                receiveThread = new Thread(DoReceive);
                receiveThread.IsBackground = true;
                receiveThread.Start();
                deviceName = serialPort1.PortName;
            }
            else
            {
                receiving = false;
                serialPort1.Close();
                //btnPUF.Enabled = false;
                btnRead.Enabled = false;
                btnCom.Text = "Open";
                deviceName = "";
            }
        }
        private void btnPUF_Click(object sender, EventArgs e)
        {
            ExternalRead.Clear();
            PUFReadCheck = false;
            for (int i = 0; i < ADC.Length; i++)
            {
                Array.Clear(ADC[i], 0, ADC[i].Length);
                Array.Clear(ADC_Bit[i], 0, ADC_Bit[i].Length);
                Array.Clear(chTotal[i], 0, chTotal[i].Length);
                Array.Clear(yValues[i], 0, yValues[i].Length);
            }
            readCount = 0;
            actionClick = "btnPUF_Click";
            zeroCount[0] = 0; zeroCount[1] = 0; zeroCount[2] = 0; zeroCount[3] = 0;
            oneCount[0] = 0; oneCount[1] = 0; oneCount[2] = 0; oneCount[3] = 0;
            serialPort1.Write("z");
            serialPort1.Write("o");
            serialPort1.Write("r");
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            ExternalRead.Clear();
            ExtReadCheck = false;
            for (int i = 0; i < ADC.Length; i++)
            {
                Array.Clear(ADC[i], 0, ADC[i].Length);
                Array.Clear(ADC_Bit[i], 0, ADC_Bit[i].Length);
                Array.Clear(chTotal[i], 0, chTotal[i].Length);
                Array.Clear(yValues[i], 0, yValues[i].Length);
            }
            readCount = 0;
            actionClick = "btnRead_Click";
            zeroCount[0] = 0; zeroCount[1] = 0; zeroCount[2] = 0; zeroCount[3] = 0;
            oneCount[0] = 0; oneCount[1] = 0; oneCount[2] = 0; oneCount[3] = 0;
            serialPort1.Write("r");
        }
        private void bitParse()
        {
            int[] numbers;
            int[][] temp = new int[][] { new int[1024], new int[1024], new int[1024], new int[1024] };
            for (int i = 1; i < 1025; i++)
            {
                //Console.Write(ExternalRead[i]);
                numbers = Regex.Matches(ExternalRead[i], "(-?[0-9]+)").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
                ADC[0][i - 1] = numbers[0];
                ADC[1][i - 1] = numbers[1];
                ADC[2][i - 1] = numbers[2];
                ADC[3][i - 1] = numbers[3];
                temp[0][i - 1] = numbers[0];
                temp[1][i - 1] = numbers[1];
                temp[2][i - 1] = numbers[2];
                temp[3][i - 1] = numbers[3];
            }
            ADC_List.Add(temp);
            //Console.WriteLine(readCount);
            for (int i = 0; i < ADC.Length; i++)
            {
                for (int j = 0; j < ADC[i].Length; j++)
                {
                    chTotal[i][ADC[i][j] / CHART_UNIT]++;
                }
            }
        }
        private void DoReceive()
        {
            string temp = "";
            while (receiving)
            {
                if (serialPort1.BytesToRead > 0)
                {
                    temp = serialPort1.ReadLine();
                    if (actionClick == "btnPUF_Click")
                    {
                        this.Invoke((MethodInvoker)delegate () { btnPUF.Text = "Read...(" + readCount * 100 / PUFReadComplete + "%)"; });
                        readCount++;

                        if (temp.Contains("External read") && PUFReadCheck == false)
                        {
                            PUFReadCheck = true;
                            ExternalRead.Add(temp);
                            //Console.Write(temp);
                        }
                        else if (PUFReadCheck == true)
                        {
                            this.Invoke((MethodInvoker)delegate () { btnPUF.Text = "Read...(" + readCount * 100 / PUFReadComplete + "%)"; });
                            readCount++;
                            ExternalRead.Add(temp);
                            //Console.Write(temp);

                            if (temp.Contains("End"))
                            {
                                PUFReadCheck = false;
                                bitParse();
                                actionClick = "";
                                this.Invoke((MethodInvoker)delegate () { displayChart(); });
                            }
                        }
                    }
                    else if (actionClick == "btnRead_Click")
                    {
                        this.Invoke((MethodInvoker)delegate () { btnRead.Text = "Read...(" + readCount * 100 / ExtReadComplete + "%)"; });
                        readCount++;

                        if (temp.Contains("External read") && ExtReadCheck == false)
                        {
                            ExtReadCheck = true;
                            ExternalRead.Add(temp);
                            //Console.Write(temp);
                        }
                        else if (ExtReadCheck == true)
                        {
                            this.Invoke((MethodInvoker)delegate () { btnRead.Text = "Read...(" + readCount * 100 / ExtReadComplete + "%)"; });
                            readCount++;
                            ExternalRead.Add(temp);
                            //Console.Write(temp);

                            if (temp.Contains("End"))
                            {
                                ExtReadCheck = false;
                                bitParse();
                                actionClick = "";
                                this.Invoke((MethodInvoker)delegate () { displayChart(); });
                            }
                        }
                    }
                }
            }
        }
        private void displayChart()
        {
            for (int i = 0; i < yValues.Length; i++)
                Array.Copy(chTotal[i], 0, yValues[i], 0, yValues[i].Length);
            setChartData();
            displayBitMap();
        }
        private void displayBitMap()
        {
            int[] offsetX = new int[] { 0, 32, 0, 32 };
            int[] offsetY = new int[] { 0, 0, 32, 32 };
            int[][] temp = new int[][] { new int[1024], new int[1024], new int[1024], new int[1024] };
            for (int i = 0; i < ADC.Length; i++)
            {
                for (int j = 0; j < ADC[i].Length; j++)
                {
                    if (ADC[i][j] < refBitTh[i][1])
                    {
                        if (ADC[i][j] < refBitTh[i][0])
                        {
                            refGrids[i].Rows[j / 32].Cells[j % 32].Style.BackColor = cBit[i][0];
                            dataGridView1.Rows[offsetY[i] + j / 32].Cells[offsetX[i] + j % 32].Style.BackColor = cBit[i][0];
                            countBit(i, j, 0);
                        }
                        else
                        {
                            refGrids[i].Rows[j / 32].Cells[j % 32].Style.BackColor = cBit[i][1];
                            dataGridView1.Rows[offsetY[i] + j / 32].Cells[offsetX[i] + j % 32].Style.BackColor = cBit[i][1];
                            countBit(i, j, 1);
                        }
                    }
                    else
                    {
                        if (ADC[i][j] < refBitTh[i][2])
                        {
                            refGrids[i].Rows[j / 32].Cells[j % 32].Style.BackColor = cBit[i][2];
                            dataGridView1.Rows[offsetY[i] + j / 32].Cells[offsetX[i] + j % 32].Style.BackColor = cBit[i][2];
                            countBit(i, j, 2);
                        }
                        else
                        {
                            refGrids[i].Rows[j / 32].Cells[j % 32].Style.BackColor = cBit[i][3];
                            dataGridView1.Rows[offsetY[i] + j / 32].Cells[offsetX[i] + j % 32].Style.BackColor = cBit[i][3];
                            countBit(i, j, 3);
                        }
                    }
                }
            }
            ADC_Bit[0].CopyTo(temp[0], 0); ADC_Bit[1].CopyTo(temp[1], 0); ADC_Bit[2].CopyTo(temp[2], 0); ADC_Bit[3].CopyTo(temp[3], 0);
            ADC_Bit_List.Add(temp);
            setBitMetric();
        }
        private void setChartData()
        {
            for (int i = 0; i < bitSeries.Length; i++)
            {
                Axis ax = charts[i].ChartAreas[0].AxisX;
                ax.Interval = CHART_UNIT;
                ax.IntervalOffset = 0;
                ax.Minimum = 0;
                ax.Maximum = CHART_LENGTH;

                //set chart data
                bitSeries[i].Points.Clear();
                bitSeries[i].Points.DataBindXY(xValues, yValues[i]);
                //set bitSeries axis x offset
                foreach (DataPoint dp in bitSeries[i].Points) dp.XValue += ax.Interval / 2;
            }
        }
        public int HammingDistance(string num_1, string num_2)
        {
            int ans = 0;
            for (int i = 0; i < num_1.Length; i++)
                if (num_1[i] != num_2[i])
                    ans++;
            return ans;
        }
        private void setBitMetric()
        {
            setRandomness();
            setUniqueness();
            setRobustness();
            btnPUF.Text = "PUF";
            btnRead.Text = "Read";
        }
        private void setRandomness()
        {
            double zeroPercent = Math.Round((zeroCount[0] + zeroCount[1] + zeroCount[2] + zeroCount[3]) / 4096.0, 4);
            double onePercent = 1 - zeroPercent;
            labZero.Text = "0: " + zeroPercent * 100 + " %";
            labOne.Text = "1: " + onePercent * 100 + " %";
        }
        private void setUniqueness()
        {
            if (UniCheck)
            {
                //寫入log
                string[] lines = new string[BLOCK_SIZE];
                for (int i = 0; i < lines.Length; i++)
                {
                    string str = string.Join(",", ADC_Bit[i]);
                    lines[i] = str;
                }
                File.WriteAllLines(folder_path + string.Format(@"\{0}_{1}.txt", deviceName, DateTime.Now.ToString("yyyyMMddHHmmss")), lines);
                //取最近一次log
                DirectoryInfo dir = new DirectoryInfo(folder_path);
                var prefixes = dir.GetFiles("COM*.txt").GroupBy(x => x.Name.Split('_')[0]).Select(y => new { Prefix = y.Key, Count = y.Count() }).ToArray();
                string[] fileCompare = new string[prefixes.Length];
                for (int i = 0; i < prefixes.Length; i++)
                {
                    Console.WriteLine("Prefix: {0}, Count: {1}", prefixes[i].Prefix, prefixes[i].Count);
                    long[] times = dir.GetFiles(string.Format("{0}*.txt", prefixes[i].Prefix)).Select(delegate (FileInfo f)
                    {
                        string t = (f.Name.Split('.')[0]).Split('_')[1];
                        return long.Parse(t);
                    }).ToArray();
                    Array.Sort(times); //時間排序小到大
                    /*
                    foreach (long time in times)
                        Console.WriteLine(time);
                    Console.WriteLine("Max:" + times[times.Length - 1]);
                    */
                    fileCompare[i] = folder_path + string.Format(@"\{0}_{1}.txt", prefixes[i].Prefix, times[times.Length - 1]);
                }
                //讀取log
                UniArr = new int[prefixes.Length][][];
                for (int i = 0; i < fileCompare.Length; i++)
                {
                    UniArr[i] = new int[BLOCK_SIZE][];
                    string[][] temp = new string[BLOCK_SIZE][];
                    using (StreamReader sr = new StreamReader(fileCompare[i]))
                    {
                        for (int r = 0; r < BLOCK_SIZE; r++)
                        {
                            temp[r] = sr.ReadLine().Split(',');
                        }
                    }
                    for (int j = 0; j < temp.Length; j++)
                    {
                        int[] intArray = Array.ConvertAll(temp[j], delegate (string s) { return int.Parse(s); });
                        UniArr[i][j] = intArray;
                    }
                }
                //公式比較
                for (int i = 0; i < UniArr.Length; i++)
                {
                    for (int j = 0; j < UniArr[i].Length; j++)
                    {
                        Console.WriteLine(string.Join(",", UniArr[i][j]));
                    }
                    Console.WriteLine();
                }
            }
        }
        private void setRobustness()
        {
            if (ADC_Bit_List.Count >= 2)
            {
                double total = 0;
                string num_1 = string.Join("", Array.ConvertAll(ADC_Bit_List[0], delegate (int[] x) { return string.Join("", x); }));
                for (int i = 1; i < ADC_Bit_List.Count; i++)
                {
                    string num_2 = string.Join("", Array.ConvertAll(ADC_Bit_List[i], delegate (int[] x) { return string.Join("", x); }));
                    double ans = Math.Round(HammingDistance(num_1, num_2) / 4096.0, 4);
                    //Console.Write(0 + " " + i + " " + ans + "\n");
                    total += ans;
                }
                double HD_intra = Math.Round(total / (ADC_Bit_List.Count - 1), 4);
                //Console.Write("Robust: " + HD_intra * 100 + " %" + "\n");
                labRob.Text = HD_intra * 100 + " %";
            }
        }
        private void countBit(int index1, int index2, int span)
        {
            if (cBit[index1][span] == cZero)
            {
                zeroCount[index1]++;
                ADC_Bit[index1][index2] = 0;
            }
            else
            {
                oneCount[index1]++;
                ADC_Bit[index1][index2] = 1;
            }
        }
        private void changeBlock(int index, int offX, int offY)
        {
            //Change randomness metric.
            for (int j = 0; j < ADC[index].Length; j++)
            {
                if (ADC[index][j] < refBitTh[index][1])
                {
                    if (ADC[index][j] < refBitTh[index][0])
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = cBit[index][0];
                        dataGridView1.Rows[offY + j / 32].Cells[offX + j % 32].Style.BackColor = cBit[index][0];
                        countBit(index, j, 0);
                    }
                    else
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = cBit[index][1];
                        dataGridView1.Rows[offY + j / 32].Cells[offX + j % 32].Style.BackColor = cBit[index][1];
                        countBit(index, j, 1);
                    }
                }
                else
                {
                    if (ADC[index][j] < refBitTh[index][2])
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = cBit[index][2];
                        dataGridView1.Rows[offY + j / 32].Cells[offX + j % 32].Style.BackColor = cBit[index][2];
                        countBit(index, j, 2);
                    }
                    else
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = cBit[index][3];
                        dataGridView1.Rows[offY + j / 32].Cells[offX + j % 32].Style.BackColor = cBit[index][3];
                        countBit(index, j, 3);
                    }
                }
            }
            //Change robustness metric.
            if (ADC_List.Count >= 2)
            {
                int[] offsetX = new int[] { 0, 32, 0, 32 };
                int[] offsetY = new int[] { 0, 0, 32, 32 };
                for (int k = 0; k < ADC_List.Count; k++)
                {
                    int[][] ADC1 = ADC_List[k];
                    int[][] temp = new int[][] { new int[1024], new int[1024], new int[1024], new int[1024] };
                    for (int i = 0; i < ADC1.Length; i++)
                    {
                        for (int j = 0; j < ADC1[i].Length; j++)
                        {
                            if (ADC1[i][j] < refBitTh[i][1])
                            {
                                if (ADC1[i][j] < refBitTh[i][0])
                                {
                                    if (cBit[i][0] == cZero)
                                        ADC_Bit[i][j] = 0;
                                    else
                                        ADC_Bit[i][j] = 1;
                                }
                                else
                                {
                                    if (cBit[i][1] == cZero)
                                        ADC_Bit[i][j] = 0;
                                    else
                                        ADC_Bit[i][j] = 1;
                                }
                            }
                            else
                            {
                                if (ADC1[i][j] < refBitTh[i][2])
                                {
                                    if (cBit[i][2] == cZero)
                                        ADC_Bit[i][j] = 0;
                                    else
                                        ADC_Bit[i][j] = 1;
                                }
                                else
                                {
                                    if (cBit[i][3] == cZero)
                                        ADC_Bit[i][j] = 0;
                                    else
                                        ADC_Bit[i][j] = 1;
                                }
                            }
                        }
                    }
                    ADC_Bit[0].CopyTo(temp[0], 0); ADC_Bit[1].CopyTo(temp[1], 0); ADC_Bit[2].CopyTo(temp[2], 0); ADC_Bit[3].CopyTo(temp[3], 0);
                    ADC_Bit_List.Add(temp);
                    //Console.Write("Rob" + k + string.Join("", ADC_Bit_List[k][3]) + "\n");
                }
            }
            setBitMetric();
        }

        private void txtRef_1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 0;
                refBitTh[i][0] = int.Parse(txtRef_11.Text); refBitTh[i][1] = int.Parse(txtRef_12.Text); refBitTh[i][2] = int.Parse(txtRef_13.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                ADC_Bit_List.Clear();
                changeBlock(i, 0, 0);
            }
        }
        private void txtRef_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 1;
                refBitTh[i][0] = int.Parse(txtRef_21.Text); refBitTh[i][1] = int.Parse(txtRef_22.Text); refBitTh[i][2] = int.Parse(txtRef_23.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                ADC_Bit_List.Clear();
                changeBlock(i, 32, 0);
            }
        }
        private void txtRef_3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 2;
                refBitTh[i][0] = int.Parse(txtRef_31.Text); refBitTh[i][1] = int.Parse(txtRef_32.Text); refBitTh[i][2] = int.Parse(txtRef_33.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                ADC_Bit_List.Clear();
                changeBlock(i, 0, 32);
            }
        }
        private void txtRef_4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 3;
                refBitTh[i][0] = int.Parse(txtRef_41.Text); refBitTh[i][1] = int.Parse(txtRef_42.Text); refBitTh[i][2] = int.Parse(txtRef_43.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                ADC_Bit_List.Clear();
                changeBlock(i, 32, 32);
            }
        }
        private void labUniOn_Click(object sender, EventArgs e)
        {
            if (labUniOn.ForeColor == Color.Gray)
            {
                if (!Directory.Exists(folder_path))
                {
                    try
                    {
                        DirectoryInfo di = Directory.CreateDirectory(folder_path);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The process failed: {0}", ex.ToString());
                    }
                }
                UniCheck = true;
                labUniOn.ForeColor = Color.Black;
            }
            else
            {
                UniCheck = false;
                labUniOn.ForeColor = Color.Gray;
            }
        }
        private void btnRef_1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Text = btn.Text == "1" ? "0" : "1";
            btnRef_1();
        }
        private void btnRef_1()
        {
            //Console.Write("Ref " + string.Join("", ADC_Bit_List[0][3]) + "\n");
            int i = 0;
            cBit[i][0] = btnRef_11.Text == "0" ? cZero : cOne;
            cBit[i][1] = btnRef_12.Text == "0" ? cZero : cOne;
            cBit[i][2] = btnRef_13.Text == "0" ? cZero : cOne;
            cBit[i][3] = btnRef_14.Text == "0" ? cZero : cOne;
            refBitTh[i][0] = int.Parse(txtRef_11.Text); refBitTh[i][1] = int.Parse(txtRef_12.Text); refBitTh[i][2] = int.Parse(txtRef_13.Text);
            zeroCount[i] = 0; oneCount[i] = 0;
            ADC_Bit_List.Clear();
            changeBlock(i, 0, 0);
        }
        private void btnRef_2_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Text = btn.Text == "1" ? "0" : "1";
            btnRef_2();
        }
        private void btnRef_2()
        {
            int i = 1;
            cBit[i][0] = btnRef_21.Text == "0" ? cZero : cOne;
            cBit[i][1] = btnRef_22.Text == "0" ? cZero : cOne;
            cBit[i][2] = btnRef_23.Text == "0" ? cZero : cOne;
            cBit[i][3] = btnRef_24.Text == "0" ? cZero : cOne;
            refBitTh[i][0] = int.Parse(txtRef_21.Text); refBitTh[i][1] = int.Parse(txtRef_22.Text); refBitTh[i][2] = int.Parse(txtRef_23.Text);
            zeroCount[i] = 0; oneCount[i] = 0;
            ADC_Bit_List.Clear();
            changeBlock(i, 32, 0);
        }
        private void btnRef_3_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Text = btn.Text == "1" ? "0" : "1";
            btnRef_3();
        }
        private void btnRef_3()
        {
            int i = 2;
            cBit[i][0] = btnRef_31.Text == "0" ? cZero : cOne;
            cBit[i][1] = btnRef_32.Text == "0" ? cZero : cOne;
            cBit[i][2] = btnRef_33.Text == "0" ? cZero : cOne;
            cBit[i][3] = btnRef_34.Text == "0" ? cZero : cOne;
            refBitTh[i][0] = int.Parse(txtRef_31.Text); refBitTh[i][1] = int.Parse(txtRef_32.Text); refBitTh[i][2] = int.Parse(txtRef_33.Text);
            zeroCount[i] = 0; oneCount[i] = 0;
            ADC_Bit_List.Clear();
            changeBlock(i, 0, 32);
        }
        private void btnRef_4_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Text = btn.Text == "1" ? "0" : "1";
            btnRef_4();
        }
        private void btnRef_4()
        {
            int i = 3;
            cBit[i][0] = btnRef_41.Text == "0" ? cZero : cOne;
            cBit[i][1] = btnRef_42.Text == "0" ? cZero : cOne;
            cBit[i][2] = btnRef_43.Text == "0" ? cZero : cOne;
            cBit[i][3] = btnRef_44.Text == "0" ? cZero : cOne;
            refBitTh[i][0] = int.Parse(txtRef_41.Text); refBitTh[i][1] = int.Parse(txtRef_42.Text); refBitTh[i][2] = int.Parse(txtRef_43.Text);
            zeroCount[i] = 0; oneCount[i] = 0;
            ADC_Bit_List.Clear();
            changeBlock(i, 32, 32);
        }
    }
}
