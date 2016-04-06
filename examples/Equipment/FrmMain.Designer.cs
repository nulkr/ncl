namespace j2kTestEquipment
{
    partial class FrmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.panMain = new System.Windows.Forms.Panel();
            this.panTop = new System.Windows.Forms.Panel();
            this.btnCommTest = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.panBottom = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnIO = new System.Windows.Forms.Button();
            this.btnMotor = new System.Windows.Forms.Button();
            this.btnAuto = new System.Windows.Forms.Button();
            this.btnRecipe = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panTop.SuspendLayout();
            this.panBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panMain
            // 
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panMain.Location = new System.Drawing.Point(0, 0);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1272, 990);
            this.panMain.TabIndex = 2;
            // 
            // panTop
            // 
            this.panTop.Controls.Add(this.label2);
            this.panTop.Controls.Add(this.lblTitle);
            this.panTop.Controls.Add(this.pictureBox1);
            this.panTop.Controls.Add(this.btnCommTest);
            this.panTop.Controls.Add(this.btnLog);
            this.panTop.Controls.Add(this.btnLoad);
            this.panTop.Controls.Add(this.btnSave);
            this.panTop.Controls.Add(this.btnQuit);
            this.panTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTop.Location = new System.Drawing.Point(0, 0);
            this.panTop.Name = "panTop";
            this.panTop.Padding = new System.Windows.Forms.Padding(5);
            this.panTop.Size = new System.Drawing.Size(1272, 80);
            this.panTop.TabIndex = 4;
            // 
            // btnCommTest
            // 
            this.btnCommTest.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCommTest.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnCommTest.Location = new System.Drawing.Point(667, 5);
            this.btnCommTest.Margin = new System.Windows.Forms.Padding(33);
            this.btnCommTest.Name = "btnCommTest";
            this.btnCommTest.Size = new System.Drawing.Size(120, 70);
            this.btnCommTest.TabIndex = 10;
            this.btnCommTest.Text = "COMM TEST";
            this.btnCommTest.UseVisualStyleBackColor = true;
            this.btnCommTest.Click += new System.EventHandler(this.btnCommTest_Click);
            // 
            // btnLog
            // 
            this.btnLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLog.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnLog.Location = new System.Drawing.Point(787, 5);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(120, 70);
            this.btnLog.TabIndex = 9;
            this.btnLog.Text = "Log";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnLoad.Location = new System.Drawing.Point(907, 5);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(120, 70);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnSave.Location = new System.Drawing.Point(1027, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 70);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnQuit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnQuit.Location = new System.Drawing.Point(1147, 5);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(120, 70);
            this.btnQuit.TabIndex = 6;
            this.btnQuit.Text = "QUIT";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // panBottom
            // 
            this.panBottom.Controls.Add(this.button2);
            this.panBottom.Controls.Add(this.btnStop);
            this.panBottom.Controls.Add(this.btnStart);
            this.panBottom.Controls.Add(this.btnIO);
            this.panBottom.Controls.Add(this.btnMotor);
            this.panBottom.Controls.Add(this.btnAuto);
            this.panBottom.Controls.Add(this.btnRecipe);
            this.panBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panBottom.Location = new System.Drawing.Point(0, 910);
            this.panBottom.Name = "panBottom";
            this.panBottom.Padding = new System.Windows.Forms.Padding(5);
            this.panBottom.Size = new System.Drawing.Size(1272, 80);
            this.panBottom.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Left;
            this.button2.Location = new System.Drawing.Point(245, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 70);
            this.button2.TabIndex = 13;
            this.button2.Text = "LOG TEST";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnStop
            // 
            this.btnStop.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnStop.Location = new System.Drawing.Point(125, 5);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(120, 70);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnStart.Location = new System.Drawing.Point(5, 5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 70);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnIO
            // 
            this.btnIO.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnIO.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnIO.Location = new System.Drawing.Point(787, 5);
            this.btnIO.Name = "btnIO";
            this.btnIO.Size = new System.Drawing.Size(120, 70);
            this.btnIO.TabIndex = 10;
            this.btnIO.Text = "IO";
            this.btnIO.UseVisualStyleBackColor = true;
            // 
            // btnMotor
            // 
            this.btnMotor.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMotor.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnMotor.Location = new System.Drawing.Point(907, 5);
            this.btnMotor.Name = "btnMotor";
            this.btnMotor.Size = new System.Drawing.Size(120, 70);
            this.btnMotor.TabIndex = 9;
            this.btnMotor.Text = "Motor";
            this.btnMotor.UseVisualStyleBackColor = true;
            this.btnMotor.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAuto
            // 
            this.btnAuto.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAuto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnAuto.Location = new System.Drawing.Point(1027, 5);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(120, 70);
            this.btnAuto.TabIndex = 8;
            this.btnAuto.Text = "Auto";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // btnRecipe
            // 
            this.btnRecipe.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRecipe.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnRecipe.Location = new System.Drawing.Point(1147, 5);
            this.btnRecipe.Name = "btnRecipe";
            this.btnRecipe.Size = new System.Drawing.Size(120, 70);
            this.btnRecipe.TabIndex = 7;
            this.btnRecipe.Text = "Recipe";
            this.btnRecipe.UseVisualStyleBackColor = true;
            this.btnRecipe.Click += new System.EventHandler(this.btnRecipe_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(5, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 70);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Consolas", 21F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(75, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(592, 70);
            this.lblTitle.TabIndex = 12;
            this.lblTitle.Text = "Title of Current";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(75, 55);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.label2.Size = new System.Drawing.Size(592, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Recipe File Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 990);
            this.Controls.Add(this.panBottom);
            this.Controls.Add(this.panTop);
            this.Controls.Add(this.panMain);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.panTop.ResumeLayout(false);
            this.panBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Panel panTop;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Panel panBottom;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Button btnRecipe;
        private System.Windows.Forms.Button btnMotor;
        private System.Windows.Forms.Button btnIO;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Button btnCommTest;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label2;
    }
}

