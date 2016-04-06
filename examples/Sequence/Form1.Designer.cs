namespace j2kTestSequence
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.stateButton1 = new j2k.StateButton();
            this.stateLabel5 = new j2k.StateLabel();
            this.stateLabel6 = new j2k.StateLabel();
            this.stateLabel7 = new j2k.StateLabel();
            this.stateLabel8 = new j2k.StateLabel();
            this.stateLabel3 = new j2k.StateLabel();
            this.stateLabel4 = new j2k.StateLabel();
            this.stateLabel2 = new j2k.StateLabel();
            this.stateLabel1 = new j2k.StateLabel();
            this.radioGroup1 = new j2k.RadioGroup();
            this.seq1 = new j2k.Sequence();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 50);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(19, 84);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 50);
            this.button2.TabIndex = 1;
            this.button2.Text = "Abort";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grip;
            this.progressBar1.Location = new System.Drawing.Point(161, 17);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(250, 30);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 2;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(161, 62);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(250, 244);
            this.listBox1.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(19, 156);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 50);
            this.button3.TabIndex = 4;
            this.button3.Text = "Error";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(19, 302);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 50);
            this.button4.TabIndex = 6;
            this.button4.Text = "Resume";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(19, 230);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(120, 50);
            this.button5.TabIndex = 5;
            this.button5.Text = "Suspend";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.Location = new System.Drawing.Point(0, 0);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(104, 24);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.Location = new System.Drawing.Point(0, 0);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(104, 24);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.Location = new System.Drawing.Point(0, 0);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(104, 24);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "radioButton3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.Location = new System.Drawing.Point(0, 0);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(104, 24);
            this.radioButton4.TabIndex = 0;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "radioButton4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(181, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "label1";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.comboBox1.Location = new System.Drawing.Point(60, 383);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton7);
            this.groupBox1.Controls.Add(this.radioButton6);
            this.groupBox1.Controls.Add(this.radioButton5);
            this.groupBox1.Location = new System.Drawing.Point(183, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 174);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(9, 20);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(92, 16);
            this.radioButton5.TabIndex = 0;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "radioButton5";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(19, 50);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(92, 16);
            this.radioButton6.TabIndex = 1;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "radioButton6";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(54, 124);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(92, 16);
            this.radioButton7.TabIndex = 2;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "radioButton7";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // stateButton1
            // 
            this.stateButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stateButton1.BackgroundImage")));
            this.stateButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stateButton1.ColorSet = j2k.StateColorSet.Custom;
            this.stateButton1.Location = new System.Drawing.Point(235, 590);
            this.stateButton1.Name = "stateButton1";
            this.stateButton1.Padding = new System.Windows.Forms.Padding(10);
            this.stateButton1.Size = new System.Drawing.Size(119, 53);
            this.stateButton1.State = false;
            this.stateButton1.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.stateButton1.StateProp.ChangeImage = false;
            this.stateButton1.StateProp.ColorSet = j2k.StateColorSet.Custom;
            this.stateButton1.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.stateButton1.StateProp.OffColor1 = System.Drawing.SystemColors.Control;
            this.stateButton1.StateProp.OffColor2 = System.Drawing.Color.Gray;
            this.stateButton1.StateProp.OnColor1 = System.Drawing.Color.White;
            this.stateButton1.StateProp.OnColor2 = System.Drawing.Color.Lime;
            this.stateButton1.StateProp.State = false;
            this.stateButton1.TabIndex = 21;
            this.stateButton1.Text = "stateButton1";
            this.stateButton1.UseVisualStyleBackColor = true;
            this.stateButton1.Click += new System.EventHandler(this.stateButton1_Click);
            // 
            // stateLabel5
            // 
            this.stateLabel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stateLabel5.ColorSet = j2k.StateColorSet.Red;
            this.stateLabel5.Location = new System.Drawing.Point(275, 502);
            this.stateLabel5.Name = "stateLabel5";
            this.stateLabel5.Size = new System.Drawing.Size(79, 45);
            this.stateLabel5.State = true;
            this.stateLabel5.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.stateLabel5.StateProp.ChangeImage = false;
            this.stateLabel5.StateProp.ColorSet = j2k.StateColorSet.Red;
            this.stateLabel5.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.stateLabel5.StateProp.OffColor1 = System.Drawing.Color.White;
            this.stateLabel5.StateProp.OffColor2 = System.Drawing.SystemColors.Control;
            this.stateLabel5.StateProp.OnColor1 = System.Drawing.Color.MistyRose;
            this.stateLabel5.StateProp.OnColor2 = System.Drawing.Color.Red;
            this.stateLabel5.StateProp.State = true;
            this.stateLabel5.TabIndex = 20;
            this.stateLabel5.Text = "1234567890";
            this.stateLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.stateLabel5.Value = 1234567890D;
            this.stateLabel5.ValueFormat = "{0:0.###}";
            // 
            // stateLabel6
            // 
            this.stateLabel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stateLabel6.ColorSet = j2k.StateColorSet.Custom;
            this.stateLabel6.Location = new System.Drawing.Point(190, 502);
            this.stateLabel6.Name = "stateLabel6";
            this.stateLabel6.Size = new System.Drawing.Size(79, 45);
            this.stateLabel6.State = false;
            this.stateLabel6.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.stateLabel6.StateProp.ChangeImage = false;
            this.stateLabel6.StateProp.ColorSet = j2k.StateColorSet.Custom;
            this.stateLabel6.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.stateLabel6.StateProp.OffColor1 = System.Drawing.Color.White;
            this.stateLabel6.StateProp.OffColor2 = System.Drawing.SystemColors.Control;
            this.stateLabel6.StateProp.OnColor1 = System.Drawing.Color.Lavender;
            this.stateLabel6.StateProp.OnColor2 = System.Drawing.Color.DodgerBlue;
            this.stateLabel6.StateProp.State = false;
            this.stateLabel6.TabIndex = 19;
            this.stateLabel6.Text = "1234567890";
            this.stateLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.stateLabel6.Value = 1234567890D;
            this.stateLabel6.ValueFormat = "{0:0.###}";
            // 
            // stateLabel7
            // 
            this.stateLabel7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stateLabel7.ColorSet = j2k.StateColorSet.Green;
            this.stateLabel7.Location = new System.Drawing.Point(102, 502);
            this.stateLabel7.Name = "stateLabel7";
            this.stateLabel7.Size = new System.Drawing.Size(79, 45);
            this.stateLabel7.State = true;
            this.stateLabel7.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.stateLabel7.StateProp.ChangeImage = false;
            this.stateLabel7.StateProp.ColorSet = j2k.StateColorSet.Green;
            this.stateLabel7.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.stateLabel7.StateProp.OffColor1 = System.Drawing.Color.White;
            this.stateLabel7.StateProp.OffColor2 = System.Drawing.SystemColors.Control;
            this.stateLabel7.StateProp.OnColor1 = System.Drawing.Color.Lime;
            this.stateLabel7.StateProp.OnColor2 = System.Drawing.Color.DarkGreen;
            this.stateLabel7.StateProp.State = true;
            this.stateLabel7.TabIndex = 18;
            this.stateLabel7.Text = "1234567890";
            this.stateLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.stateLabel7.Value = 1234567890D;
            this.stateLabel7.ValueFormat = "{0:0.###}";
            // 
            // stateLabel8
            // 
            this.stateLabel8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stateLabel8.ColorSet = j2k.StateColorSet.Pink;
            this.stateLabel8.Location = new System.Drawing.Point(17, 502);
            this.stateLabel8.Name = "stateLabel8";
            this.stateLabel8.Size = new System.Drawing.Size(79, 45);
            this.stateLabel8.State = true;
            this.stateLabel8.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.stateLabel8.StateProp.ChangeImage = false;
            this.stateLabel8.StateProp.ColorSet = j2k.StateColorSet.Pink;
            this.stateLabel8.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.stateLabel8.StateProp.OffColor1 = System.Drawing.Color.White;
            this.stateLabel8.StateProp.OffColor2 = System.Drawing.Color.Gray;
            this.stateLabel8.StateProp.OnColor1 = System.Drawing.Color.White;
            this.stateLabel8.StateProp.OnColor2 = System.Drawing.Color.Pink;
            this.stateLabel8.StateProp.State = true;
            this.stateLabel8.TabIndex = 17;
            this.stateLabel8.Text = "0";
            this.stateLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.stateLabel8.Value = 0D;
            this.stateLabel8.ValueFormat = "{0:0.###}";
            // 
            // stateLabel3
            // 
            this.stateLabel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stateLabel3.ColorSet = j2k.StateColorSet.Orange;
            this.stateLabel3.Location = new System.Drawing.Point(275, 446);
            this.stateLabel3.Name = "stateLabel3";
            this.stateLabel3.Size = new System.Drawing.Size(79, 45);
            this.stateLabel3.State = true;
            this.stateLabel3.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.stateLabel3.StateProp.ChangeImage = false;
            this.stateLabel3.StateProp.ColorSet = j2k.StateColorSet.Orange;
            this.stateLabel3.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.stateLabel3.StateProp.OffColor1 = System.Drawing.Color.White;
            this.stateLabel3.StateProp.OffColor2 = System.Drawing.Color.Gray;
            this.stateLabel3.StateProp.OnColor1 = System.Drawing.Color.Moccasin;
            this.stateLabel3.StateProp.OnColor2 = System.Drawing.Color.DarkOrange;
            this.stateLabel3.StateProp.State = true;
            this.stateLabel3.TabIndex = 16;
            this.stateLabel3.Text = "0";
            this.stateLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.stateLabel3.Value = 0D;
            this.stateLabel3.ValueFormat = "{0:0.###}";
            // 
            // stateLabel4
            // 
            this.stateLabel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stateLabel4.ColorSet = j2k.StateColorSet.Yellow;
            this.stateLabel4.Location = new System.Drawing.Point(190, 446);
            this.stateLabel4.Name = "stateLabel4";
            this.stateLabel4.Size = new System.Drawing.Size(79, 45);
            this.stateLabel4.State = true;
            this.stateLabel4.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.stateLabel4.StateProp.ChangeImage = false;
            this.stateLabel4.StateProp.ColorSet = j2k.StateColorSet.Yellow;
            this.stateLabel4.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.stateLabel4.StateProp.OffColor1 = System.Drawing.Color.White;
            this.stateLabel4.StateProp.OffColor2 = System.Drawing.Color.Gray;
            this.stateLabel4.StateProp.OnColor1 = System.Drawing.Color.White;
            this.stateLabel4.StateProp.OnColor2 = System.Drawing.Color.Yellow;
            this.stateLabel4.StateProp.State = true;
            this.stateLabel4.TabIndex = 15;
            this.stateLabel4.Text = "0";
            this.stateLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.stateLabel4.Value = 0D;
            this.stateLabel4.ValueFormat = "{0:0.###}";
            // 
            // stateLabel2
            // 
            this.stateLabel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stateLabel2.ColorSet = j2k.StateColorSet.Lime;
            this.stateLabel2.Location = new System.Drawing.Point(102, 446);
            this.stateLabel2.Name = "stateLabel2";
            this.stateLabel2.Size = new System.Drawing.Size(79, 45);
            this.stateLabel2.State = true;
            this.stateLabel2.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.stateLabel2.StateProp.ChangeImage = false;
            this.stateLabel2.StateProp.ColorSet = j2k.StateColorSet.Lime;
            this.stateLabel2.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.stateLabel2.StateProp.OffColor1 = System.Drawing.Color.White;
            this.stateLabel2.StateProp.OffColor2 = System.Drawing.Color.Gray;
            this.stateLabel2.StateProp.OnColor1 = System.Drawing.Color.White;
            this.stateLabel2.StateProp.OnColor2 = System.Drawing.Color.Lime;
            this.stateLabel2.StateProp.State = true;
            this.stateLabel2.TabIndex = 14;
            this.stateLabel2.Text = "0";
            this.stateLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.stateLabel2.Value = 0D;
            this.stateLabel2.ValueFormat = "{0:0.###}";
            // 
            // stateLabel1
            // 
            this.stateLabel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stateLabel1.ColorSet = j2k.StateColorSet.Aqua;
            this.stateLabel1.Location = new System.Drawing.Point(17, 446);
            this.stateLabel1.Name = "stateLabel1";
            this.stateLabel1.Size = new System.Drawing.Size(79, 45);
            this.stateLabel1.State = true;
            this.stateLabel1.StateProp.BorderColor = System.Drawing.Color.Gray;
            this.stateLabel1.StateProp.ChangeImage = false;
            this.stateLabel1.StateProp.ColorSet = j2k.StateColorSet.Aqua;
            this.stateLabel1.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.stateLabel1.StateProp.OffColor1 = System.Drawing.Color.White;
            this.stateLabel1.StateProp.OffColor2 = System.Drawing.Color.Gray;
            this.stateLabel1.StateProp.OnColor1 = System.Drawing.Color.White;
            this.stateLabel1.StateProp.OnColor2 = System.Drawing.Color.Aqua;
            this.stateLabel1.StateProp.State = true;
            this.stateLabel1.TabIndex = 13;
            this.stateLabel1.Text = "0";
            this.stateLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.stateLabel1.Value = 0D;
            this.stateLabel1.ValueFormat = "{0:0.###}";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Column = 3;
            this.radioGroup1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroup1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioGroup1.Items = "333\r\ndd\r\nd\r\nd11ddd\r\n가나다라";
            this.radioGroup1.Location = new System.Drawing.Point(12, 302);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Padding = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.radioGroup1.SelectedIndex = -1;
            this.radioGroup1.Size = new System.Drawing.Size(399, 137);
            this.radioGroup1.TabIndex = 10;
            this.radioGroup1.TabStop = false;
            this.radioGroup1.Text = "radioGroup1";
            // 
            // seq1
            // 
            this.seq1.Interval = 1;
            this.seq1.WorkerReportsProgress = false;
            this.seq1.OnSequence += new System.ComponentModel.DoWorkEventHandler(this.seq1_OnSequence);
            this.seq1.OnProgress += new System.ComponentModel.ProgressChangedEventHandler(this.seq1_OnProgress);
            this.seq1.OnCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.seq1_OnCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 675);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.stateButton1);
            this.Controls.Add(this.stateLabel5);
            this.Controls.Add(this.stateLabel6);
            this.Controls.Add(this.stateLabel7);
            this.Controls.Add(this.stateLabel8);
            this.Controls.Add(this.stateLabel3);
            this.Controls.Add(this.stateLabel4);
            this.Controls.Add(this.stateLabel2);
            this.Controls.Add(this.stateLabel1);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private j2k.Sequence seq1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private j2k.RadioGroup radioGroup1;
        private j2k.StateLabel stateLabel1;
        private j2k.StateLabel stateLabel2;
        private j2k.StateLabel stateLabel3;
        private j2k.StateLabel stateLabel4;
        private j2k.StateLabel stateLabel5;
        private j2k.StateLabel stateLabel6;
        private j2k.StateLabel stateLabel7;
        private j2k.StateLabel stateLabel8;
        private j2k.StateButton stateButton1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
    }
}

