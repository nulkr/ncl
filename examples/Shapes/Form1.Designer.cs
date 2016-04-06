namespace j2kTestShapes
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnZoomFit = new System.Windows.Forms.Button();
            this.chkAlphaBlending = new System.Windows.Forms.CheckBox();
            this.edtCount = new j2k.ValueEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnInvertDir = new System.Windows.Forms.Button();
            this.chkSelected = new System.Windows.Forms.CheckBox();
            this.chkShowArrow = new System.Windows.Forms.CheckBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.shapeView1 = new j2k.Shapes.ShapeView();
            this.shapeData1 = new j2k.Shapes.ShapeData();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.btnZoomFit);
            this.panel1.Controls.Add(this.chkAlphaBlending);
            this.panel1.Controls.Add(this.edtCount);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.btnInvertDir);
            this.panel1.Controls.Add(this.chkSelected);
            this.panel1.Controls.Add(this.chkShowArrow);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(738, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(162, 606);
            this.panel1.TabIndex = 1;
            // 
            // btnZoomFit
            // 
            this.btnZoomFit.Location = new System.Drawing.Point(16, 182);
            this.btnZoomFit.Name = "btnZoomFit";
            this.btnZoomFit.Size = new System.Drawing.Size(134, 37);
            this.btnZoomFit.TabIndex = 8;
            this.btnZoomFit.Text = "Zoom Fit";
            this.btnZoomFit.UseVisualStyleBackColor = true;
            this.btnZoomFit.Click += new System.EventHandler(this.btnZoomFit_Click);
            // 
            // chkAlphaBlending
            // 
            this.chkAlphaBlending.AutoSize = true;
            this.chkAlphaBlending.Location = new System.Drawing.Point(15, 65);
            this.chkAlphaBlending.Name = "chkAlphaBlending";
            this.chkAlphaBlending.Size = new System.Drawing.Size(109, 16);
            this.chkAlphaBlending.TabIndex = 7;
            this.chkAlphaBlending.Text = "Alpha Blending";
            this.chkAlphaBlending.UseVisualStyleBackColor = true;
            // 
            // edtCount
            // 
            this.edtCount.Location = new System.Drawing.Point(57, 38);
            this.edtCount.Name = "edtCount";
            this.edtCount.Size = new System.Drawing.Size(91, 21);
            this.edtCount.TabIndex = 6;
            this.edtCount.Text = "10";
            this.edtCount.Value = 10D;
            this.edtCount.ValueFormat = "{0:0.}";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Count";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(14, 130);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(134, 37);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Draw R",
            "Point",
            "Line",
            "Polyline",
            "Circle",
            "Arc"});
            this.comboBox1.Location = new System.Drawing.Point(14, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(134, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // btnInvertDir
            // 
            this.btnInvertDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInvertDir.Location = new System.Drawing.Point(14, 513);
            this.btnInvertDir.Name = "btnInvertDir";
            this.btnInvertDir.Size = new System.Drawing.Size(134, 37);
            this.btnInvertDir.TabIndex = 3;
            this.btnInvertDir.Text = "Invert Dir";
            this.btnInvertDir.UseVisualStyleBackColor = true;
            this.btnInvertDir.Click += new System.EventHandler(this.btnInvertDir_Click);
            // 
            // chkSelected
            // 
            this.chkSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSelected.AutoSize = true;
            this.chkSelected.Location = new System.Drawing.Point(15, 578);
            this.chkSelected.Name = "chkSelected";
            this.chkSelected.Size = new System.Drawing.Size(73, 16);
            this.chkSelected.TabIndex = 2;
            this.chkSelected.Text = "Selected";
            this.chkSelected.UseVisualStyleBackColor = true;
            this.chkSelected.CheckedChanged += new System.EventHandler(this.chkSelected_CheckedChanged);
            // 
            // chkShowArrow
            // 
            this.chkShowArrow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowArrow.AutoSize = true;
            this.chkShowArrow.Location = new System.Drawing.Point(15, 556);
            this.chkShowArrow.Name = "chkShowArrow";
            this.chkShowArrow.Size = new System.Drawing.Size(93, 16);
            this.chkShowArrow.TabIndex = 1;
            this.chkShowArrow.Text = "Show Arrow";
            this.chkShowArrow.UseVisualStyleBackColor = true;
            this.chkShowArrow.CheckedChanged += new System.EventHandler(this.chkShowArrow_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(14, 87);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(134, 37);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // shapeView1
            // 
            this.shapeView1.BackColor = System.Drawing.Color.White;
            this.shapeView1.CursorColor = System.Drawing.Color.Orange;
            this.shapeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shapeView1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.shapeView1.Location = new System.Drawing.Point(0, 0);
            this.shapeView1.MouseMode = ((j2k.Shapes.MouseMode)(((j2k.Shapes.MouseMode.ZoomRect | j2k.Shapes.MouseMode.ZoomWheel) 
            | j2k.Shapes.MouseMode.Panning)));
            this.shapeView1.Name = "shapeView1";
            this.shapeView1.ShapeData = this.shapeData1;
            this.shapeView1.Size = new System.Drawing.Size(738, 606);
            this.shapeView1.TabIndex = 2;
            this.shapeView1.Text = "shapeView1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 606);
            this.Controls.Add(this.shapeView1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAdd;
        private j2k.Shapes.ShapeData shapeData1;
        private System.Windows.Forms.CheckBox chkSelected;
        private System.Windows.Forms.CheckBox chkShowArrow;
        private System.Windows.Forms.Button btnInvertDir;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label1;
        private j2k.ValueEdit edtCount;
        private System.Windows.Forms.CheckBox chkAlphaBlending;
        private j2k.Shapes.ShapeView shapeView1;
        private System.Windows.Forms.Button btnZoomFit;
    }
}

