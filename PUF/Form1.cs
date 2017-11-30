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
        private SerialPort serialPort1 = new SerialPort();
        private DataGridViewTextBoxColumn[] Col = new DataGridViewTextBoxColumn[64];
        private int zeroCount = 0, oneCount = 0;
        private delegate void Display();
        private int[][] bitArray = new int[4][];

        private Chart[] charts;
        private Series[] bitSeries = new Series[4];
        private int unit = 5, chMax = 1025;
        private int[] xValues;
        private int[][] yValues = new int[4][];

        private Boolean receiving;
        private Thread receiveThread;

        private List<string> ExternalRead = new List<string>();
        private Boolean ExternalReadCheck = false, pClick = false;
        private int pCount = 0, pComplete = 4105, bitTh = 124;
        private int[][] pTotal = new int[4][];
        public Form1()
        {
            InitializeComponent();
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
            txtInput.Text = bitTh.ToString();

            for (int i = 0; i < bitArray.Length; i++)
                bitArray[i] = new int[1024];
            for (int i = 0; i < pTotal.Length; i++)
                pTotal[i] = new int[1025 / unit];

            charts = new Chart[] { chart1, chart2, chart3, chart4 };
            xValues = new int[chMax / unit];
            for (int i = 0; i < charts.Length; i++)
            {
                charts[i].ChartAreas[0].AxisX.ScaleView.Size = 500;
                charts[i].SetBounds(-80, charts[i].Location.Y, charts[i].Size.Width, charts[i].Size.Height);
            }
            for (int i = 0; i < xValues.Length; i++)
                xValues[i] = unit * i;
            for (int i = 0; i < yValues.Length; i++)
                yValues[i] = new int[chMax / unit];
            for (int i = 0; i < bitSeries.Length; i++)
            {
                bitSeries[i] = new Series();
                charts[i].ChartAreas[0].AxisY.Maximum = 400;
                Axis ax = charts[i].ChartAreas[0].AxisX;
                ax.Interval = unit;
                ax.IntervalOffset = 0;
                ax.Minimum = 0;
                ax.Maximum = chMax;
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
                btnCom.Text = "Open";
            }
        }

        private void btnPUF_Click(object sender, EventArgs e)
        {

            ExternalRead.Clear();
            ExternalReadCheck = false;
            for (int i = 0; i < bitArray.Length; i++)
            {
                Array.Clear(bitArray[i], 0, bitArray[i].Length);
                Array.Clear(pTotal[i], 0, pTotal[i].Length);
            }

            for (int i = 0; i < yValues.Length; i++)
                Array.Clear(yValues[i], 0, yValues[i].Length);
            pCount = 0;
            pClick = true;
            zeroCount = 0;
            oneCount = 0;
            serialPort1.Write("z");
            serialPort1.Write("o");
            serialPort1.Write("r");
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                bitTh = int.Parse(txtInput.Text);
                zeroCount = 0;
                oneCount = 0;
                displayMapArray();
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
                    if (pClick)
                    {
                        this.Invoke((MethodInvoker)delegate () { btnPUF.Text = "Read...(" + pCount * 100 / pComplete + "%)"; });
                        pCount++;
                    }
                    if (temp.Contains("External read") && ExternalReadCheck == false)
                    {
                        ExternalReadCheck = true;
                        ExternalRead.Add(temp);
                        //Console.Write(temp);
                    }
                    else if (ExternalReadCheck == true)
                    {
                        this.Invoke((MethodInvoker)delegate () { btnPUF.Text = "Read...(" + pCount * 100 / pComplete + "%)"; });
                        pCount++;
                        ExternalRead.Add(temp);
                        //Console.Write(temp);

                        if (temp.Contains("End"))
                        {
                            ExternalReadCheck = false;

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
                            //Console.WriteLine(pCount);
                            for (int i = 0; i < bitArray.Length; i++)
                            {
                                for (int j = 0; j < bitArray[i].Length; j++)
                                {
                                    pTotal[i][bitArray[i][j] / unit]++;
                                }
                            }
                            this.Invoke((MethodInvoker)delegate () { displayChart(); });
                        }
                    }
                }
            }
        }
        private void displayChart()
        {
            for (int i = 0; i < pTotal.Length; i++)
                Array.Copy(pTotal[i], 0, yValues[i], 0, yValues[i].Length);
            for (int i = 0; i < bitSeries.Length; i++)
            {
                Axis ax = charts[i].ChartAreas[0].AxisX;
                ax.Interval = unit;
                ax.IntervalOffset = 0;
                ax.Minimum = 0;
                ax.Maximum = chMax;

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
                    if (bitArray[i][j] <= bitTh)
                    {
                        dataGridView1.Rows[offsetY[i] + j / 32].Cells[offsetX[i] + j % 32].Style.BackColor = Color.FromArgb(52, 43, 134);
                        zeroCount++;
                    }
                    else
                    {
                        dataGridView1.Rows[offsetY[i] + j / 32].Cells[offsetX[i] + j % 32].Style.BackColor = Color.FromArgb(249, 253, 8);
                        oneCount++;
                    }
                }
            }
            double zeroPercent = Math.Round(zeroCount / 4096.0, 4);
            double onePercent = 1 - zeroPercent;
            labZero.Text = "0: " + zeroPercent * 100 + " %";
            labOne.Text = "1: " + onePercent * 100 + " %";
            btnPUF.Text = "PUF";
        }
    }
}
