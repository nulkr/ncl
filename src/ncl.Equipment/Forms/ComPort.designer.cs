namespace ncl
{
    partial class ComPort
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComPort));
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.listBox = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnMonitoring = new ncl.StateButton();
            this.btnConnected = new ncl.StateButton();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbInput = new System.Windows.Forms.ComboBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.seqMonitoring = new ncl.Sequence();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.BackColor = System.Drawing.Color.MidnightBlue;
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listBox.ForeColor = System.Drawing.SystemColors.Info;
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 14;
            this.listBox.Location = new System.Drawing.Point(0, 0);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(421, 526);
            this.listBox.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSettings);
            this.panel1.Controls.Add(this.btnMonitoring);
            this.panel1.Controls.Add(this.btnConnected);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 444);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(6);
            this.panel1.Size = new System.Drawing.Size(421, 82);
            this.panel1.TabIndex = 5;
            // 
            // btnSettings
            // 
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSettings.Location = new System.Drawing.Point(295, 6);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(120, 70);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "Show Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnMonitoring
            // 
            this.btnMonitoring.BackColor = System.Drawing.SystemColors.Control;
            this.btnMonitoring.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMonitoring.BackgroundImage")));
            this.btnMonitoring.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMonitoring.ColorSet = ncl.StateColorSet.Green;
            this.btnMonitoring.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMonitoring.Location = new System.Drawing.Point(126, 6);
            this.btnMonitoring.Name = "btnMonitoring";
            this.btnMonitoring.Padding = new System.Windows.Forms.Padding(10);
            this.btnMonitoring.Size = new System.Drawing.Size(120, 70);
            this.btnMonitoring.State = false;
            this.btnMonitoring.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.btnMonitoring.StateProp.ChangeImage = false;
            this.btnMonitoring.StateProp.ColorSet = ncl.StateColorSet.Green;
            this.btnMonitoring.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.btnMonitoring.StateProp.OffColor1 = System.Drawing.Color.White;
            this.btnMonitoring.StateProp.OffColor2 = System.Drawing.SystemColors.Control;
            this.btnMonitoring.StateProp.OnColor1 = System.Drawing.Color.Lime;
            this.btnMonitoring.StateProp.OnColor2 = System.Drawing.Color.DarkGreen;
            this.btnMonitoring.StateProp.State = false;
            this.btnMonitoring.StateProp.UseGradient = true;
            this.btnMonitoring.TabIndex = 6;
            this.btnMonitoring.Text = "Monitoring";
            this.btnMonitoring.UseVisualStyleBackColor = false;
            this.btnMonitoring.Click += new System.EventHandler(this.btnMonitoring_Click);
            // 
            // btnConnected
            // 
            this.btnConnected.BackColor = System.Drawing.SystemColors.Control;
            this.btnConnected.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnConnected.BackgroundImage")));
            this.btnConnected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnConnected.ColorSet = ncl.StateColorSet.Green;
            this.btnConnected.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnConnected.Location = new System.Drawing.Point(6, 6);
            this.btnConnected.Name = "btnConnected";
            this.btnConnected.Padding = new System.Windows.Forms.Padding(10);
            this.btnConnected.Size = new System.Drawing.Size(120, 70);
            this.btnConnected.State = false;
            this.btnConnected.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.btnConnected.StateProp.ChangeImage = false;
            this.btnConnected.StateProp.ColorSet = ncl.StateColorSet.Green;
            this.btnConnected.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.btnConnected.StateProp.OffColor1 = System.Drawing.Color.White;
            this.btnConnected.StateProp.OffColor2 = System.Drawing.SystemColors.Control;
            this.btnConnected.StateProp.OnColor1 = System.Drawing.Color.Lime;
            this.btnConnected.StateProp.OnColor2 = System.Drawing.Color.DarkGreen;
            this.btnConnected.StateProp.State = false;
            this.btnConnected.StateProp.UseGradient = true;
            this.btnConnected.TabIndex = 5;
            this.btnConnected.Text = "Connected";
            this.btnConnected.UseVisualStyleBackColor = false;
            this.btnConnected.Click += new System.EventHandler(this.btnConnected_Click);
            // 
            // propGrid
            // 
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Right;
            this.propGrid.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.propGrid.Location = new System.Drawing.Point(135, 0);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(286, 444);
            this.propGrid.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbInput);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 402);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(6);
            this.panel2.Size = new System.Drawing.Size(135, 42);
            this.panel2.TabIndex = 13;
            // 
            // cbInput
            // 
            this.cbInput.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbInput.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbInput.FormattingEnabled = true;
            this.cbInput.ItemHeight = 18;
            this.cbInput.Location = new System.Drawing.Point(6, 6);
            this.cbInput.Name = "cbInput";
            this.cbInput.Size = new System.Drawing.Size(123, 26);
            this.cbInput.TabIndex = 11;
            this.cbInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyUp);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(132, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 402);
            this.splitter1.TabIndex = 14;
            this.splitter1.TabStop = false;
            // 
            // seqMonitoring
            // 
            this.seqMonitoring.ProgressInterval = 100;
            this.seqMonitoring.SeqNo = 0;
            this.seqMonitoring.WorkerReportsProgress = false;
            // 
            // ComPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 526);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.propGrid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listBox);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ComPort";
            this.Text = "COM Port";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ComPort_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        protected System.IO.Ports.SerialPort serialPort;
        protected Sequence seqMonitoring;
        protected System.Windows.Forms.Button btnSettings;
        protected StateButton btnMonitoring;
        protected StateButton btnConnected;
        protected System.Windows.Forms.ListBox listBox;
        protected System.Windows.Forms.PropertyGrid propGrid;
        protected System.Windows.Forms.ComboBox cbInput;
    }
}