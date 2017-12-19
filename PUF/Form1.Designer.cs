namespace PUF
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.btnCom = new System.Windows.Forms.Button();
            this.cbbCOM = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labZero = new System.Windows.Forms.Label();
            this.labOne = new System.Windows.Forms.Label();
            this.btnPUF = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart4 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtRef_12 = new System.Windows.Forms.TextBox();
            this.refGridView1 = new System.Windows.Forms.DataGridView();
            this.refGridView2 = new System.Windows.Forms.DataGridView();
            this.refGridView3 = new System.Windows.Forms.DataGridView();
            this.refGridView4 = new System.Windows.Forms.DataGridView();
            this.txtRef_22 = new System.Windows.Forms.TextBox();
            this.txtRef_32 = new System.Windows.Forms.TextBox();
            this.txtRef_42 = new System.Windows.Forms.TextBox();
            this.txtRef_11 = new System.Windows.Forms.TextBox();
            this.txtRef_13 = new System.Windows.Forms.TextBox();
            this.txtRef_21 = new System.Windows.Forms.TextBox();
            this.txtRef_23 = new System.Windows.Forms.TextBox();
            this.txtRef_31 = new System.Windows.Forms.TextBox();
            this.txtRef_33 = new System.Windows.Forms.TextBox();
            this.txtRef_41 = new System.Windows.Forms.TextBox();
            this.txtRef_43 = new System.Windows.Forms.TextBox();
            this.btnRef_11 = new System.Windows.Forms.Button();
            this.btnRef_12 = new System.Windows.Forms.Button();
            this.btnRef_13 = new System.Windows.Forms.Button();
            this.btnRef_14 = new System.Windows.Forms.Button();
            this.btnRef_21 = new System.Windows.Forms.Button();
            this.btnRef_22 = new System.Windows.Forms.Button();
            this.btnRef_23 = new System.Windows.Forms.Button();
            this.btnRef_24 = new System.Windows.Forms.Button();
            this.btnRef_31 = new System.Windows.Forms.Button();
            this.btnRef_32 = new System.Windows.Forms.Button();
            this.btnRef_33 = new System.Windows.Forms.Button();
            this.btnRef_34 = new System.Windows.Forms.Button();
            this.btnRef_41 = new System.Windows.Forms.Button();
            this.btnRef_43 = new System.Windows.Forms.Button();
            this.btnRef_42 = new System.Windows.Forms.Button();
            this.btnRef_44 = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refGridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCom
            // 
            this.btnCom.Font = new System.Drawing.Font("PMingLiU", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCom.Location = new System.Drawing.Point(240, 2);
            this.btnCom.Name = "btnCom";
            this.btnCom.Size = new System.Drawing.Size(150, 30);
            this.btnCom.TabIndex = 0;
            this.btnCom.Text = "Open";
            this.btnCom.UseVisualStyleBackColor = true;
            this.btnCom.Click += new System.EventHandler(this.btnCom_Click);
            // 
            // cbbCOM
            // 
            this.cbbCOM.Font = new System.Drawing.Font("PMingLiU", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbbCOM.FormattingEnabled = true;
            this.cbbCOM.Location = new System.Drawing.Point(110, 2);
            this.cbbCOM.Name = "cbbCOM";
            this.cbbCOM.Size = new System.Drawing.Size(120, 29);
            this.cbbCOM.TabIndex = 4;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(10, 40);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 10;
            this.dataGridView1.Size = new System.Drawing.Size(643, 643);
            this.dataGridView1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(10, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 22);
            this.label1.TabIndex = 9;
            this.label1.Text = "COM Port";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(43)))), ((int)(((byte)(134)))));
            this.label3.Location = new System.Drawing.Point(662, 360);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 50);
            this.label3.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(253)))), ((int)(((byte)(8)))));
            this.label4.Location = new System.Drawing.Point(662, 460);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 50);
            this.label4.TabIndex = 12;
            // 
            // labZero
            // 
            this.labZero.Font = new System.Drawing.Font("PMingLiU", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labZero.Location = new System.Drawing.Point(712, 460);
            this.labZero.Name = "labZero";
            this.labZero.Size = new System.Drawing.Size(200, 54);
            this.labZero.TabIndex = 13;
            this.labZero.Text = "0:";
            this.labZero.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labOne
            // 
            this.labOne.Font = new System.Drawing.Font("PMingLiU", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labOne.Location = new System.Drawing.Point(712, 360);
            this.labOne.Name = "labOne";
            this.labOne.Size = new System.Drawing.Size(200, 56);
            this.labOne.TabIndex = 14;
            this.labOne.Text = "1:";
            this.labOne.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPUF
            // 
            this.btnPUF.Enabled = false;
            this.btnPUF.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPUF.Location = new System.Drawing.Point(662, 60);
            this.btnPUF.Name = "btnPUF";
            this.btnPUF.Size = new System.Drawing.Size(200, 60);
            this.btnPUF.TabIndex = 17;
            this.btnPUF.Text = "PUF";
            this.btnPUF.UseVisualStyleBackColor = true;
            this.btnPUF.Click += new System.EventHandler(this.btnPUF_Click);
            // 
            // chart1
            // 
            chartArea5.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea5);
            this.chart1.Location = new System.Drawing.Point(350, 850);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(1900, 300);
            this.chart1.TabIndex = 18;
            this.chart1.Text = "chart1";
            // 
            // chart2
            // 
            chartArea6.Name = "ChartArea2";
            this.chart2.ChartAreas.Add(chartArea6);
            this.chart2.Location = new System.Drawing.Point(350, 1300);
            this.chart2.Name = "chart2";
            this.chart2.Size = new System.Drawing.Size(1900, 300);
            this.chart2.TabIndex = 19;
            this.chart2.Text = "chart2";
            // 
            // chart3
            // 
            chartArea7.Name = "ChartArea3";
            this.chart3.ChartAreas.Add(chartArea7);
            this.chart3.Location = new System.Drawing.Point(350, 1750);
            this.chart3.Name = "chart3";
            this.chart3.Size = new System.Drawing.Size(1900, 300);
            this.chart3.TabIndex = 20;
            this.chart3.Text = "chart3";
            // 
            // chart4
            // 
            chartArea8.Name = "ChartArea4";
            this.chart4.ChartAreas.Add(chartArea8);
            this.chart4.Location = new System.Drawing.Point(350, 2200);
            this.chart4.Name = "chart4";
            this.chart4.Size = new System.Drawing.Size(1900, 300);
            this.chart4.TabIndex = 21;
            this.chart4.Text = "chart4";
            // 
            // txtRef_12
            // 
            this.txtRef_12.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_12.Location = new System.Drawing.Point(275, 800);
            this.txtRef_12.Name = "txtRef_12";
            this.txtRef_12.Size = new System.Drawing.Size(200, 46);
            this.txtRef_12.TabIndex = 22;
            this.txtRef_12.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_12_KeyDown);
            // 
            // refGridView1
            // 
            this.refGridView1.AllowUserToAddRows = false;
            this.refGridView1.AllowUserToDeleteRows = false;
            this.refGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.refGridView1.Location = new System.Drawing.Point(10, 850);
            this.refGridView1.Name = "refGridView1";
            this.refGridView1.ReadOnly = true;
            this.refGridView1.RowTemplate.Height = 10;
            this.refGridView1.Size = new System.Drawing.Size(323, 323);
            this.refGridView1.TabIndex = 23;
            // 
            // refGridView2
            // 
            this.refGridView2.AllowUserToAddRows = false;
            this.refGridView2.AllowUserToDeleteRows = false;
            this.refGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.refGridView2.Location = new System.Drawing.Point(10, 1300);
            this.refGridView2.Name = "refGridView2";
            this.refGridView2.ReadOnly = true;
            this.refGridView2.RowTemplate.Height = 10;
            this.refGridView2.Size = new System.Drawing.Size(323, 323);
            this.refGridView2.TabIndex = 24;
            // 
            // refGridView3
            // 
            this.refGridView3.AllowUserToAddRows = false;
            this.refGridView3.AllowUserToDeleteRows = false;
            this.refGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.refGridView3.Location = new System.Drawing.Point(10, 1750);
            this.refGridView3.Name = "refGridView3";
            this.refGridView3.ReadOnly = true;
            this.refGridView3.RowTemplate.Height = 10;
            this.refGridView3.Size = new System.Drawing.Size(323, 323);
            this.refGridView3.TabIndex = 25;
            // 
            // refGridView4
            // 
            this.refGridView4.AllowUserToAddRows = false;
            this.refGridView4.AllowUserToDeleteRows = false;
            this.refGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.refGridView4.Location = new System.Drawing.Point(10, 2200);
            this.refGridView4.Name = "refGridView4";
            this.refGridView4.ReadOnly = true;
            this.refGridView4.RowTemplate.Height = 10;
            this.refGridView4.Size = new System.Drawing.Size(323, 323);
            this.refGridView4.TabIndex = 26;
            // 
            // txtRef_22
            // 
            this.txtRef_22.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_22.Location = new System.Drawing.Point(275, 1250);
            this.txtRef_22.Name = "txtRef_22";
            this.txtRef_22.Size = new System.Drawing.Size(200, 46);
            this.txtRef_22.TabIndex = 27;
            this.txtRef_22.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_22_KeyDown);
            // 
            // txtRef_32
            // 
            this.txtRef_32.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_32.Location = new System.Drawing.Point(275, 1700);
            this.txtRef_32.Name = "txtRef_32";
            this.txtRef_32.Size = new System.Drawing.Size(200, 46);
            this.txtRef_32.TabIndex = 28;
            this.txtRef_32.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_32_KeyDown);
            // 
            // txtRef_42
            // 
            this.txtRef_42.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_42.Location = new System.Drawing.Point(275, 2150);
            this.txtRef_42.Name = "txtRef_42";
            this.txtRef_42.Size = new System.Drawing.Size(200, 46);
            this.txtRef_42.TabIndex = 29;
            this.txtRef_42.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_42_KeyDown);
            // 
            // txtRef_11
            // 
            this.txtRef_11.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_11.Location = new System.Drawing.Point(40, 800);
            this.txtRef_11.Name = "txtRef_11";
            this.txtRef_11.Size = new System.Drawing.Size(200, 46);
            this.txtRef_11.TabIndex = 30;
            this.txtRef_11.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_11_KeyDown);
            // 
            // txtRef_13
            // 
            this.txtRef_13.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_13.Location = new System.Drawing.Point(515, 800);
            this.txtRef_13.Name = "txtRef_13";
            this.txtRef_13.Size = new System.Drawing.Size(200, 46);
            this.txtRef_13.TabIndex = 31;
            this.txtRef_13.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_13_KeyDown);
            // 
            // txtRef_21
            // 
            this.txtRef_21.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_21.Location = new System.Drawing.Point(40, 1250);
            this.txtRef_21.Name = "txtRef_21";
            this.txtRef_21.Size = new System.Drawing.Size(200, 46);
            this.txtRef_21.TabIndex = 40;
            this.txtRef_21.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_21_KeyDown);
            // 
            // txtRef_23
            // 
            this.txtRef_23.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_23.Location = new System.Drawing.Point(515, 1250);
            this.txtRef_23.Name = "txtRef_23";
            this.txtRef_23.Size = new System.Drawing.Size(200, 46);
            this.txtRef_23.TabIndex = 41;
            this.txtRef_23.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_23_KeyDown);
            // 
            // txtRef_31
            // 
            this.txtRef_31.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_31.Location = new System.Drawing.Point(40, 1700);
            this.txtRef_31.Name = "txtRef_31";
            this.txtRef_31.Size = new System.Drawing.Size(200, 46);
            this.txtRef_31.TabIndex = 46;
            this.txtRef_31.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_31_KeyDown);
            // 
            // txtRef_33
            // 
            this.txtRef_33.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_33.Location = new System.Drawing.Point(515, 1700);
            this.txtRef_33.Name = "txtRef_33";
            this.txtRef_33.Size = new System.Drawing.Size(200, 46);
            this.txtRef_33.TabIndex = 47;
            this.txtRef_33.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_33_KeyDown);
            // 
            // txtRef_41
            // 
            this.txtRef_41.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_41.Location = new System.Drawing.Point(40, 2150);
            this.txtRef_41.Name = "txtRef_41";
            this.txtRef_41.Size = new System.Drawing.Size(200, 46);
            this.txtRef_41.TabIndex = 52;
            this.txtRef_41.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_41_KeyDown);
            // 
            // txtRef_43
            // 
            this.txtRef_43.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRef_43.Location = new System.Drawing.Point(515, 2150);
            this.txtRef_43.Name = "txtRef_43";
            this.txtRef_43.Size = new System.Drawing.Size(200, 46);
            this.txtRef_43.TabIndex = 53;
            this.txtRef_43.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRef_43_KeyDown);
            // 
            // btnRef_11
            // 
            this.btnRef_11.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_11.Location = new System.Drawing.Point(10, 780);
            this.btnRef_11.Name = "btnRef_11";
            this.btnRef_11.Size = new System.Drawing.Size(25, 40);
            this.btnRef_11.TabIndex = 54;
            this.btnRef_11.Text = "0";
            this.btnRef_11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_11.UseCompatibleTextRendering = true;
            this.btnRef_11.UseVisualStyleBackColor = true;
            this.btnRef_11.Click += new System.EventHandler(this.btnRef_11_Click);
            // 
            // btnRef_12
            // 
            this.btnRef_12.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_12.Location = new System.Drawing.Point(245, 780);
            this.btnRef_12.Name = "btnRef_12";
            this.btnRef_12.Size = new System.Drawing.Size(25, 40);
            this.btnRef_12.TabIndex = 55;
            this.btnRef_12.Text = "1";
            this.btnRef_12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_12.UseCompatibleTextRendering = true;
            this.btnRef_12.UseVisualStyleBackColor = true;
            this.btnRef_12.Click += new System.EventHandler(this.btnRef_12_Click);
            // 
            // btnRef_13
            // 
            this.btnRef_13.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_13.Location = new System.Drawing.Point(480, 780);
            this.btnRef_13.Name = "btnRef_13";
            this.btnRef_13.Size = new System.Drawing.Size(25, 40);
            this.btnRef_13.TabIndex = 56;
            this.btnRef_13.Text = "0";
            this.btnRef_13.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_13.UseCompatibleTextRendering = true;
            this.btnRef_13.UseVisualStyleBackColor = true;
            this.btnRef_13.Click += new System.EventHandler(this.btnRef_13_Click);
            // 
            // btnRef_14
            // 
            this.btnRef_14.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_14.Location = new System.Drawing.Point(720, 780);
            this.btnRef_14.Name = "btnRef_14";
            this.btnRef_14.Size = new System.Drawing.Size(25, 40);
            this.btnRef_14.TabIndex = 57;
            this.btnRef_14.Text = "1";
            this.btnRef_14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_14.UseCompatibleTextRendering = true;
            this.btnRef_14.UseVisualStyleBackColor = true;
            this.btnRef_14.Click += new System.EventHandler(this.btnRef_14_Click);
            // 
            // btnRef_21
            // 
            this.btnRef_21.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_21.Location = new System.Drawing.Point(10, 1230);
            this.btnRef_21.Name = "btnRef_21";
            this.btnRef_21.Size = new System.Drawing.Size(25, 40);
            this.btnRef_21.TabIndex = 58;
            this.btnRef_21.Text = "0";
            this.btnRef_21.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_21.UseCompatibleTextRendering = true;
            this.btnRef_21.UseVisualStyleBackColor = true;
            this.btnRef_21.Click += new System.EventHandler(this.btnRef_21_Click);
            // 
            // btnRef_22
            // 
            this.btnRef_22.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_22.Location = new System.Drawing.Point(245, 1230);
            this.btnRef_22.Name = "btnRef_22";
            this.btnRef_22.Size = new System.Drawing.Size(25, 40);
            this.btnRef_22.TabIndex = 59;
            this.btnRef_22.Text = "1";
            this.btnRef_22.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_22.UseCompatibleTextRendering = true;
            this.btnRef_22.UseVisualStyleBackColor = true;
            this.btnRef_22.Click += new System.EventHandler(this.btnRef_22_Click);
            // 
            // btnRef_23
            // 
            this.btnRef_23.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_23.Location = new System.Drawing.Point(480, 1230);
            this.btnRef_23.Name = "btnRef_23";
            this.btnRef_23.Size = new System.Drawing.Size(25, 40);
            this.btnRef_23.TabIndex = 60;
            this.btnRef_23.Text = "0";
            this.btnRef_23.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_23.UseCompatibleTextRendering = true;
            this.btnRef_23.UseVisualStyleBackColor = true;
            this.btnRef_23.Click += new System.EventHandler(this.btnRef_23_Click);
            // 
            // btnRef_24
            // 
            this.btnRef_24.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_24.Location = new System.Drawing.Point(720, 1230);
            this.btnRef_24.Name = "btnRef_24";
            this.btnRef_24.Size = new System.Drawing.Size(25, 40);
            this.btnRef_24.TabIndex = 61;
            this.btnRef_24.Text = "1";
            this.btnRef_24.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_24.UseCompatibleTextRendering = true;
            this.btnRef_24.UseVisualStyleBackColor = true;
            this.btnRef_24.Click += new System.EventHandler(this.btnRef_24_Click);
            // 
            // btnRef_31
            // 
            this.btnRef_31.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_31.Location = new System.Drawing.Point(10, 1680);
            this.btnRef_31.Name = "btnRef_31";
            this.btnRef_31.Size = new System.Drawing.Size(25, 40);
            this.btnRef_31.TabIndex = 62;
            this.btnRef_31.Text = "0";
            this.btnRef_31.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_31.UseCompatibleTextRendering = true;
            this.btnRef_31.UseVisualStyleBackColor = true;
            this.btnRef_31.Click += new System.EventHandler(this.btnRef_31_Click);
            // 
            // btnRef_32
            // 
            this.btnRef_32.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_32.Location = new System.Drawing.Point(245, 1680);
            this.btnRef_32.Name = "btnRef_32";
            this.btnRef_32.Size = new System.Drawing.Size(25, 40);
            this.btnRef_32.TabIndex = 63;
            this.btnRef_32.Text = "1";
            this.btnRef_32.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_32.UseCompatibleTextRendering = true;
            this.btnRef_32.UseVisualStyleBackColor = true;
            this.btnRef_32.Click += new System.EventHandler(this.btnRef_32_Click);
            // 
            // btnRef_33
            // 
            this.btnRef_33.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_33.Location = new System.Drawing.Point(480, 1680);
            this.btnRef_33.Name = "btnRef_33";
            this.btnRef_33.Size = new System.Drawing.Size(25, 40);
            this.btnRef_33.TabIndex = 64;
            this.btnRef_33.Text = "0";
            this.btnRef_33.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_33.UseCompatibleTextRendering = true;
            this.btnRef_33.UseVisualStyleBackColor = true;
            this.btnRef_33.Click += new System.EventHandler(this.btnRef_33_Click);
            // 
            // btnRef_34
            // 
            this.btnRef_34.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_34.Location = new System.Drawing.Point(720, 1680);
            this.btnRef_34.Name = "btnRef_34";
            this.btnRef_34.Size = new System.Drawing.Size(25, 40);
            this.btnRef_34.TabIndex = 65;
            this.btnRef_34.Text = "1";
            this.btnRef_34.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_34.UseCompatibleTextRendering = true;
            this.btnRef_34.UseVisualStyleBackColor = true;
            this.btnRef_34.Click += new System.EventHandler(this.btnRef_34_Click);
            // 
            // btnRef_41
            // 
            this.btnRef_41.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_41.Location = new System.Drawing.Point(10, 2130);
            this.btnRef_41.Name = "btnRef_41";
            this.btnRef_41.Size = new System.Drawing.Size(25, 40);
            this.btnRef_41.TabIndex = 66;
            this.btnRef_41.Text = "0";
            this.btnRef_41.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_41.UseCompatibleTextRendering = true;
            this.btnRef_41.UseVisualStyleBackColor = true;
            this.btnRef_41.Click += new System.EventHandler(this.btnRef_41_Click);
            // 
            // btnRef_43
            // 
            this.btnRef_43.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_43.Location = new System.Drawing.Point(480, 2130);
            this.btnRef_43.Name = "btnRef_43";
            this.btnRef_43.Size = new System.Drawing.Size(25, 40);
            this.btnRef_43.TabIndex = 67;
            this.btnRef_43.Text = "0";
            this.btnRef_43.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_43.UseCompatibleTextRendering = true;
            this.btnRef_43.UseVisualStyleBackColor = true;
            this.btnRef_43.Click += new System.EventHandler(this.btnRef_43_Click);
            // 
            // btnRef_42
            // 
            this.btnRef_42.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_42.Location = new System.Drawing.Point(245, 2130);
            this.btnRef_42.Name = "btnRef_42";
            this.btnRef_42.Size = new System.Drawing.Size(25, 40);
            this.btnRef_42.TabIndex = 68;
            this.btnRef_42.Text = "1";
            this.btnRef_42.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_42.UseCompatibleTextRendering = true;
            this.btnRef_42.UseVisualStyleBackColor = true;
            this.btnRef_42.Click += new System.EventHandler(this.btnRef_42_Click);
            // 
            // btnRef_44
            // 
            this.btnRef_44.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRef_44.Location = new System.Drawing.Point(720, 2130);
            this.btnRef_44.Name = "btnRef_44";
            this.btnRef_44.Size = new System.Drawing.Size(25, 40);
            this.btnRef_44.TabIndex = 69;
            this.btnRef_44.Text = "1";
            this.btnRef_44.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRef_44.UseCompatibleTextRendering = true;
            this.btnRef_44.UseVisualStyleBackColor = true;
            this.btnRef_44.Click += new System.EventHandler(this.btnRef_44_Click);
            // 
            // btnRead
            // 
            this.btnRead.Enabled = false;
            this.btnRead.Font = new System.Drawing.Font("PMingLiU", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRead.Location = new System.Drawing.Point(662, 150);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(200, 60);
            this.btnRead.TabIndex = 70;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1035, 812);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnRef_44);
            this.Controls.Add(this.btnRef_42);
            this.Controls.Add(this.btnRef_43);
            this.Controls.Add(this.btnRef_41);
            this.Controls.Add(this.btnRef_34);
            this.Controls.Add(this.btnRef_33);
            this.Controls.Add(this.btnRef_32);
            this.Controls.Add(this.btnRef_31);
            this.Controls.Add(this.btnRef_24);
            this.Controls.Add(this.btnRef_23);
            this.Controls.Add(this.btnRef_22);
            this.Controls.Add(this.btnRef_21);
            this.Controls.Add(this.btnRef_14);
            this.Controls.Add(this.btnRef_13);
            this.Controls.Add(this.btnRef_12);
            this.Controls.Add(this.btnRef_11);
            this.Controls.Add(this.txtRef_43);
            this.Controls.Add(this.txtRef_41);
            this.Controls.Add(this.txtRef_33);
            this.Controls.Add(this.txtRef_31);
            this.Controls.Add(this.txtRef_23);
            this.Controls.Add(this.txtRef_21);
            this.Controls.Add(this.txtRef_13);
            this.Controls.Add(this.txtRef_11);
            this.Controls.Add(this.txtRef_42);
            this.Controls.Add(this.txtRef_32);
            this.Controls.Add(this.txtRef_22);
            this.Controls.Add(this.refGridView4);
            this.Controls.Add(this.refGridView3);
            this.Controls.Add(this.refGridView2);
            this.Controls.Add(this.refGridView1);
            this.Controls.Add(this.txtRef_12);
            this.Controls.Add(this.chart4);
            this.Controls.Add(this.chart3);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.btnPUF);
            this.Controls.Add(this.labOne);
            this.Controls.Add(this.labZero);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cbbCOM);
            this.Controls.Add(this.btnCom);
            this.Name = "Form1";
            this.Text = "PUF Demo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refGridView4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCom;
        private System.Windows.Forms.ComboBox cbbCOM;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labZero;
        private System.Windows.Forms.Label labOne;
        private System.Windows.Forms.Button btnPUF;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart4;
        private System.Windows.Forms.TextBox txtRef_12;
        private System.Windows.Forms.DataGridView refGridView1;
        private System.Windows.Forms.DataGridView refGridView2;
        private System.Windows.Forms.DataGridView refGridView3;
        private System.Windows.Forms.DataGridView refGridView4;
        private System.Windows.Forms.TextBox txtRef_22;
        private System.Windows.Forms.TextBox txtRef_32;
        private System.Windows.Forms.TextBox txtRef_42;
        private System.Windows.Forms.TextBox txtRef_11;
        private System.Windows.Forms.TextBox txtRef_13;
        private System.Windows.Forms.TextBox txtRef_21;
        private System.Windows.Forms.TextBox txtRef_23;
        private System.Windows.Forms.TextBox txtRef_31;
        private System.Windows.Forms.TextBox txtRef_33;
        private System.Windows.Forms.TextBox txtRef_41;
        private System.Windows.Forms.TextBox txtRef_43;
        private System.Windows.Forms.Button btnRef_11;
        private System.Windows.Forms.Button btnRef_12;
        private System.Windows.Forms.Button btnRef_13;
        private System.Windows.Forms.Button btnRef_14;
        private System.Windows.Forms.Button btnRef_21;
        private System.Windows.Forms.Button btnRef_22;
        private System.Windows.Forms.Button btnRef_23;
        private System.Windows.Forms.Button btnRef_24;
        private System.Windows.Forms.Button btnRef_31;
        private System.Windows.Forms.Button btnRef_32;
        private System.Windows.Forms.Button btnRef_33;
        private System.Windows.Forms.Button btnRef_34;
        private System.Windows.Forms.Button btnRef_41;
        private System.Windows.Forms.Button btnRef_43;
        private System.Windows.Forms.Button btnRef_42;
        private System.Windows.Forms.Button btnRef_44;
        private System.Windows.Forms.Button btnRead;
    }
}

