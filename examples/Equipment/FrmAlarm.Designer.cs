namespace j2kTestEquipment
{
    partial class FrmAlarm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblText = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBuzzerOff = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbAlarms = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Consolas", 21F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(632, 60);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "TITLE";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblText
            // 
            this.lblText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblText.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.lblText.Location = new System.Drawing.Point(0, 60);
            this.lblText.Name = "lblText";
            this.lblText.Padding = new System.Windows.Forms.Padding(20);
            this.lblText.Size = new System.Drawing.Size(632, 460);
            this.lblText.TabIndex = 2;
            this.lblText.Text = "TEXT";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnBuzzerOff);
            this.panel1.Controls.Add(this.btnHide);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(492, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 460);
            this.panel1.TabIndex = 4;
            // 
            // btnBuzzerOff
            // 
            this.btnBuzzerOff.Location = new System.Drawing.Point(10, 119);
            this.btnBuzzerOff.Name = "btnBuzzerOff";
            this.btnBuzzerOff.Size = new System.Drawing.Size(120, 60);
            this.btnBuzzerOff.TabIndex = 3;
            this.btnBuzzerOff.Text = "Buzzer Off";
            this.btnBuzzerOff.UseVisualStyleBackColor = true;
            this.btnBuzzerOff.Click += new System.EventHandler(this.btnBuzzerOff_Click);
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHide.Location = new System.Drawing.Point(10, 389);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(120, 60);
            this.btnHide.TabIndex = 2;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(10, 10);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 60);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbAlarms);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 420);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(492, 100);
            this.panel2.TabIndex = 6;
            // 
            // lbAlarms
            // 
            this.lbAlarms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAlarms.FormattingEnabled = true;
            this.lbAlarms.ItemHeight = 14;
            this.lbAlarms.Location = new System.Drawing.Point(3, 3);
            this.lbAlarms.Name = "lbAlarms";
            this.lbAlarms.ScrollAlwaysVisible = true;
            this.lbAlarms.Size = new System.Drawing.Size(486, 94);
            this.lbAlarms.TabIndex = 6;
            this.lbAlarms.SelectedIndexChanged += new System.EventHandler(this.lbAlarms_SelectedIndexChanged);
            // 
            // AlarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 520);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "AlarmForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alarm Information";
            this.TopMost = true;
            this.VisibleChanged += new System.EventHandler(this.AlarmForm_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnBuzzerOff;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lbAlarms;
    }
}