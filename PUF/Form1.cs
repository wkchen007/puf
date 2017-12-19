using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private delegate void Display();
        //讀取電路thread
        private SerialPort serialPort1 = new SerialPort();
        private Boolean receiving;
        private Thread receiveThread;
        private List<string> ExternalRead = new List<string>();
        private string actionClick = "";
        private int readCount = 0;
        //圖表變數
        private Color cZero = Color.FromArgb(52, 43, 134), //藍色
              cOne = Color.FromArgb(249, 253, 8); //黃色
        private DataGridViewTextBoxColumn[] Col = new DataGridViewTextBoxColumn[64];
        private int[] zeroCount = new int[BLOCK_SIZE];
        private int[] oneCount = new int[BLOCK_SIZE];
        private int[][] bitArray = new int[BLOCK_SIZE][];
        private Chart[] charts;
        private Series[] bitSeries = new Series[BLOCK_SIZE];
        private int[] xValues = new int[CHART_LENGTH / CHART_UNIT];
        private int[][] yValues = new int[BLOCK_SIZE][];
        private int[][] chTotal = new int[BLOCK_SIZE][]; //暫存直方圖的數值陣列，防止yValues超出陣列上限
        //PUFRead方法的變數
        private Boolean PUFReadCheck = false;
        private int PUFReadComplete = 4105;
        //Read方法的變數
        private Boolean ExtReadCheck = false;
        private int ExtReadComplete = 2055;
        //bit reference變數
        private int bitTh1 = 124, bitTh2 = 128, bitTh3 = 133;
        private DataGridView[] refGrids;
        private int[][] refBitTh = new int[BLOCK_SIZE][];
        public Form1()
        {
            InitializeComponent();
            //大圖初始化
            for (int i = 0; i < bitArray.Length; i++)
                bitArray[i] = new int[1024];
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
                chTotal[i] = new int[1025 / CHART_UNIT]; //訊號最高值:1025
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
                Axis ax = charts[i].ChartAreas[0].AxisX;
                ax.Interval = CHART_UNIT;
                ax.IntervalOffset = 0;
                ax.Minimum = 0;
                ax.Maximum = CHART_LENGTH;
                charts[i].Series.Add(bitSeries[i]);
                bitSeries[i].ChartType = SeriesChartType.Column;
                bitSeries[i].Name = "bitSeries" + i;
                bitSeries[i].IsValueShownAsLabel = true;

                //set chart data
                bitSeries[i].Points.Clear();
                bitSeries[i].Points.DataBindXY(xValues, yValues[i]);
                //set bitSeries axis x offset
                foreach (DataPoint dp in bitSeries[i].Points) dp.XValue += ax.Interval / 2;
            }
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
                btnPUF.Enabled = true;
                btnRead.Enabled = true;
                btnCom.Text = "Close";
                receiving = true;
                receiveThread = new Thread(DoReceive);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            else
            {
                receiving = false;
                serialPort1.Close();
                btnPUF.Enabled = false;
                btnRead.Enabled = false;
                btnCom.Text = "Open";
            }
        }
        private void btnPUF_Click(object sender, EventArgs e)
        {
            ExternalRead.Clear();
            PUFReadCheck = false;
            for (int i = 0; i < bitArray.Length; i++)
            {
                Array.Clear(bitArray[i], 0, bitArray[i].Length);
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
            for (int i = 0; i < bitArray.Length; i++)
            {
                Array.Clear(bitArray[i], 0, bitArray[i].Length);
                Array.Clear(chTotal[i], 0, chTotal[i].Length);
                Array.Clear(yValues[i], 0, yValues[i].Length);
            }
            readCount = 0;
            actionClick = "btnRead_Click";
            zeroCount[0] = 0; zeroCount[1] = 0; zeroCount[2] = 0; zeroCount[3] = 0;
            oneCount[0] = 0; oneCount[1] = 0; oneCount[2] = 0; oneCount[3] = 0;
            serialPort1.Write("r");
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

                                int[] numbers;
                                for (int i = 1; i < 1025; i++)
                                {
                                    //Console.Write(ExternalRead[i]);
                                    numbers = Regex.Matches(ExternalRead[i], "(-?[0-9]+)").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
                                    bitArray[0][i - 1] = numbers[0];
                                    bitArray[1][i - 1] = numbers[1];
                                    bitArray[2][i - 1] = numbers[2];
                                    bitArray[3][i - 1] = numbers[3];
                                }
                                //Console.WriteLine(readCount);
                                for (int i = 0; i < bitArray.Length; i++)
                                {
                                    for (int j = 0; j < bitArray[i].Length; j++)
                                    {
                                        chTotal[i][bitArray[i][j] / CHART_UNIT]++;
                                    }
                                }
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

                                int[] numbers;
                                for (int i = 1; i < 1025; i++)
                                {
                                    //Console.Write(ExternalRead[i]);
                                    numbers = Regex.Matches(ExternalRead[i], "(-?[0-9]+)").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
                                    bitArray[0][i - 1] = numbers[0];
                                    bitArray[1][i - 1] = numbers[1];
                                    bitArray[2][i - 1] = numbers[2];
                                    bitArray[3][i - 1] = numbers[3];
                                }
                                //Console.WriteLine(readCount);
                                for (int i = 0; i < bitArray.Length; i++)
                                {
                                    for (int j = 0; j < bitArray[i].Length; j++)
                                    {
                                        chTotal[i][bitArray[i][j] / CHART_UNIT]++;
                                    }
                                }
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
            displayMapArray();
        }

        private void displayMapArray()
        {
            int[] offsetX = new int[] { 0, 32, 0, 32 };
            int[] offsetY = new int[] { 0, 0, 32, 32 };
            for (int i = 0; i < bitArray.Length; i++)
            {
                for (int j = 0; j < bitArray[i].Length; j++)
                {
                    if (bitArray[i][j] < refBitTh[i][1])
                    {
                        if (bitArray[i][j] < refBitTh[i][0])
                        {
                            refGrids[i].Rows[j / 32].Cells[j % 32].Style.BackColor = Color.FromArgb(52, 43, 134);
                            dataGridView1.Rows[offsetY[i] + j / 32].Cells[offsetX[i] + j % 32].Style.BackColor = Color.FromArgb(52, 43, 134);
                            zeroCount[i]++;
                        }
                        else
                        {
                            refGrids[i].Rows[j / 32].Cells[j % 32].Style.BackColor = Color.FromArgb(249, 253, 8);
                            dataGridView1.Rows[offsetY[i] + j / 32].Cells[offsetX[i] + j % 32].Style.BackColor = Color.FromArgb(249, 253, 8);
                            oneCount[i]++;
                        }
                    }
                    else
                    {
                        if (bitArray[i][j] < refBitTh[i][2])
                        {
                            refGrids[i].Rows[j / 32].Cells[j % 32].Style.BackColor = Color.FromArgb(52, 43, 134);
                            dataGridView1.Rows[offsetY[i] + j / 32].Cells[offsetX[i] + j % 32].Style.BackColor = Color.FromArgb(52, 43, 134);
                            zeroCount[i]++;
                        }
                        else
                        {
                            refGrids[i].Rows[j / 32].Cells[j % 32].Style.BackColor = Color.FromArgb(249, 253, 8);
                            dataGridView1.Rows[offsetY[i] + j / 32].Cells[offsetX[i] + j % 32].Style.BackColor = Color.FromArgb(249, 253, 8);
                            oneCount[i]++;
                        }
                    }
                }
            }
            double zeroPercent = Math.Round((zeroCount[0] + zeroCount[1] + zeroCount[2] + zeroCount[3]) / 4096.0, 4);
            double onePercent = 1 - zeroPercent;
            labZero.Text = "0: " + zeroPercent * 100 + " %";
            labOne.Text = "1: " + onePercent * 100 + " %";
            btnPUF.Text = "PUF";
            btnRead.Text = "Read";
        }

        private void txtRef_11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 0;
                refBitTh[i][0] = int.Parse(txtRef_11.Text); refBitTh[i][1] = int.Parse(txtRef_12.Text); refBitTh[i][2] = int.Parse(txtRef_13.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 0, 0);
            }
        }
        private void txtRef_12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 0;
                refBitTh[i][0] = int.Parse(txtRef_11.Text); refBitTh[i][1] = int.Parse(txtRef_12.Text); refBitTh[i][2] = int.Parse(txtRef_13.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 0, 0);
            }
        }
        private void txtRef_13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 0;
                refBitTh[i][0] = int.Parse(txtRef_11.Text); refBitTh[i][1] = int.Parse(txtRef_12.Text); refBitTh[i][2] = int.Parse(txtRef_13.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 0, 0);
            }
        }
        private void txtRef_21_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 1;
                refBitTh[i][0] = int.Parse(txtRef_21.Text); refBitTh[i][1] = int.Parse(txtRef_22.Text); refBitTh[i][2] = int.Parse(txtRef_23.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 32, 0);
            }
        }
        private void txtRef_22_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 1;
                refBitTh[i][0] = int.Parse(txtRef_21.Text); refBitTh[i][1] = int.Parse(txtRef_22.Text); refBitTh[i][2] = int.Parse(txtRef_23.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 32, 0);
            }
        }
        private void txtRef_23_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 1;
                refBitTh[i][0] = int.Parse(txtRef_21.Text); refBitTh[i][1] = int.Parse(txtRef_22.Text); refBitTh[i][2] = int.Parse(txtRef_23.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 32, 0);
            }
        }
        private void txtRef_31_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 2;
                refBitTh[i][0] = int.Parse(txtRef_31.Text); refBitTh[i][1] = int.Parse(txtRef_32.Text); refBitTh[i][2] = int.Parse(txtRef_33.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 0, 32);
            }
        }
        private void txtRef_32_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 2;
                refBitTh[i][0] = int.Parse(txtRef_31.Text); refBitTh[i][1] = int.Parse(txtRef_32.Text); refBitTh[i][2] = int.Parse(txtRef_33.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 0, 32);
            }
        }
        private void txtRef_33_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 2;
                refBitTh[i][0] = int.Parse(txtRef_31.Text); refBitTh[i][1] = int.Parse(txtRef_32.Text); refBitTh[i][2] = int.Parse(txtRef_33.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 0, 32);
            }
        }
        private void txtRef_41_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 3;
                refBitTh[i][0] = int.Parse(txtRef_41.Text); refBitTh[i][1] = int.Parse(txtRef_42.Text); refBitTh[i][2] = int.Parse(txtRef_43.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 32, 32);
            }
        }
        private void txtRef_42_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 3;
                refBitTh[i][0] = int.Parse(txtRef_41.Text); refBitTh[i][1] = int.Parse(txtRef_42.Text); refBitTh[i][2] = int.Parse(txtRef_43.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 32, 32);
            }
        }
        private void txtRef_43_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int i = 3;
                refBitTh[i][0] = int.Parse(txtRef_41.Text); refBitTh[i][1] = int.Parse(txtRef_42.Text); refBitTh[i][2] = int.Parse(txtRef_43.Text);
                zeroCount[i] = 0; oneCount[i] = 0;
                displayMapArray(i, 32, 32);
            }
        }
        private void btnRef_11_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_1();
        }
        private void btnRef_12_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_1();
        }
        private void btnRef_13_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_1();
        }
        private void btnRef_14_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_1();
        }
        private void btnRef_1()
        {
            Color[] color = new Color[4];
            color[0] = btnRef_11.Text == "0" ? cZero : cOne;
            color[1] = btnRef_12.Text == "0" ? cZero : cOne;
            color[2] = btnRef_13.Text == "0" ? cZero : cOne;
            color[3] = btnRef_14.Text == "0" ? cZero : cOne;
            int i = 0;
            refBitTh[i][0] = int.Parse(txtRef_11.Text); refBitTh[i][1] = int.Parse(txtRef_12.Text); refBitTh[i][2] = int.Parse(txtRef_13.Text);
            zeroCount[i] = 0; oneCount[i] = 0;
            displayMapArray(i, 0, 0, color);
        }
        private void btnRef_21_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_2();
        }
        private void btnRef_22_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_2();
        }

        private void btnRef_23_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_2();
        }

        private void btnRef_24_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_2();
        }
        private void btnRef_2()
        {
            Color[] color = new Color[4];
            color[0] = btnRef_21.Text == "0" ? cZero : cOne;
            color[1] = btnRef_22.Text == "0" ? cZero : cOne;
            color[2] = btnRef_23.Text == "0" ? cZero : cOne;
            color[3] = btnRef_24.Text == "0" ? cZero : cOne;
            int i = 1;
            refBitTh[i][0] = int.Parse(txtRef_21.Text); refBitTh[i][1] = int.Parse(txtRef_22.Text); refBitTh[i][2] = int.Parse(txtRef_23.Text);
            zeroCount[i] = 0; oneCount[i] = 0;
            displayMapArray(i, 32, 0, color);
        }
        private void btnRef_31_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_3();
        }
        private void btnRef_32_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_3();
        }

        private void btnRef_33_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_3();
        }
        private void btnRef_34_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_3();
        }
        private void btnRef_3()
        {
            Color[] color = new Color[4];
            color[0] = btnRef_31.Text == "0" ? cZero : cOne;
            color[1] = btnRef_32.Text == "0" ? cZero : cOne;
            color[2] = btnRef_33.Text == "0" ? cZero : cOne;
            color[3] = btnRef_34.Text == "0" ? cZero : cOne;
            int i = 2;
            refBitTh[i][0] = int.Parse(txtRef_31.Text); refBitTh[i][1] = int.Parse(txtRef_32.Text); refBitTh[i][2] = int.Parse(txtRef_33.Text);
            zeroCount[i] = 0; oneCount[i] = 0;
            displayMapArray(i, 0, 32, color);
        }
        private void btnRef_41_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_4();
        }
        private void btnRef_42_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_4();
        }
        private void btnRef_43_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_4();
        }
        private void btnRef_44_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "1")
                btn.Text = "0";
            else
                btn.Text = "1";
            btnRef_4();
        }
        private void btnRef_4()
        {
            Color[] color = new Color[4];
            color[0] = btnRef_41.Text == "0" ? cZero : cOne;
            color[1] = btnRef_42.Text == "0" ? cZero : cOne;
            color[2] = btnRef_43.Text == "0" ? cZero : cOne;
            color[3] = btnRef_44.Text == "0" ? cZero : cOne;
            int i = 3;
            refBitTh[i][0] = int.Parse(txtRef_41.Text); refBitTh[i][1] = int.Parse(txtRef_42.Text); refBitTh[i][2] = int.Parse(txtRef_43.Text);
            zeroCount[i] = 0; oneCount[i] = 0;
            displayMapArray(i, 32, 32, color);
        }
        private void displayMapArray(int index, int offsetX, int offsetY)
        {
            for (int j = 0; j < bitArray[index].Length; j++)
            {
                if (bitArray[index][j] < refBitTh[index][1])
                {
                    if (bitArray[index][j] < refBitTh[index][0])
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = Color.FromArgb(52, 43, 134);
                        dataGridView1.Rows[offsetY + j / 32].Cells[offsetX + j % 32].Style.BackColor = Color.FromArgb(52, 43, 134);
                        zeroCount[index]++;
                    }
                    else
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = Color.FromArgb(249, 253, 8);
                        dataGridView1.Rows[offsetY + j / 32].Cells[offsetX + j % 32].Style.BackColor = Color.FromArgb(249, 253, 8);
                        oneCount[index]++;
                    }
                }
                else
                {
                    if (bitArray[index][j] < refBitTh[index][2])
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = Color.FromArgb(52, 43, 134);
                        dataGridView1.Rows[offsetY + j / 32].Cells[offsetX + j % 32].Style.BackColor = Color.FromArgb(52, 43, 134);
                        zeroCount[index]++;
                    }
                    else
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = Color.FromArgb(249, 253, 8);
                        dataGridView1.Rows[offsetY + j / 32].Cells[offsetX + j % 32].Style.BackColor = Color.FromArgb(249, 253, 8);
                        oneCount[index]++;
                    }
                }
            }
            double zeroPercent = Math.Round((zeroCount[0] + zeroCount[1] + zeroCount[2] + zeroCount[3]) / 4096.0, 4);
            double onePercent = 1 - zeroPercent;
            labZero.Text = "0: " + zeroPercent * 100 + " %";
            labOne.Text = "1: " + onePercent * 100 + " %";
            btnPUF.Text = "PUF";
            btnRead.Text = "Read";
        }

        private void displayMapArray(int index, int offsetX, int offsetY, Color[] color)
        {
            for (int j = 0; j < bitArray[index].Length; j++)
            {
                if (bitArray[index][j] < refBitTh[index][1])
                {
                    if (bitArray[index][j] < refBitTh[index][0])
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = color[0];
                        dataGridView1.Rows[offsetY + j / 32].Cells[offsetX + j % 32].Style.BackColor = color[0];
                        zeroCount[index]++;
                    }
                    else
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = color[1];
                        dataGridView1.Rows[offsetY + j / 32].Cells[offsetX + j % 32].Style.BackColor = color[1];
                        oneCount[index]++;
                    }
                }
                else
                {
                    if (bitArray[index][j] < refBitTh[index][2])
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = color[2];
                        dataGridView1.Rows[offsetY + j / 32].Cells[offsetX + j % 32].Style.BackColor = color[2];
                        zeroCount[index]++;
                    }
                    else
                    {
                        refGrids[index].Rows[j / 32].Cells[j % 32].Style.BackColor = color[3];
                        dataGridView1.Rows[offsetY + j / 32].Cells[offsetX + j % 32].Style.BackColor = color[3];
                        oneCount[index]++;
                    }
                }
            }
            double zeroPercent = Math.Round((zeroCount[0] + zeroCount[1] + zeroCount[2] + zeroCount[3]) / 4096.0, 4);
            double onePercent = 1 - zeroPercent;
            labZero.Text = "0: " + zeroPercent * 100 + " %";
            labOne.Text = "1: " + onePercent * 100 + " %";
            btnPUF.Text = "PUF";
            btnRead.Text = "Read";
        }
    }
}
