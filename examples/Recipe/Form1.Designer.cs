namespace j2kTestRecipe
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnPmacIO = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSaveCSV = new System.Windows.Forms.Button();
            this.btnLoadCSV = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnPmacVar = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnOutputIO = new System.Windows.Forms.Button();
            this.btnInputIO = new System.Windows.Forms.Button();
            this.btnFixed = new System.Windows.Forms.Button();
            this.btnParam = new System.Windows.Forms.Button();
            this.pgIO = new System.Windows.Forms.PropertyGrid();
            this.tabPageControl = new System.Windows.Forms.TabPage();
            this.btnAssignToControl = new System.Windows.Forms.Button();
            this.btnAssignFromControl = new System.Windows.Forms.Button();
            this.edt_opt_Lamp_ConfigRun = new j2k.ValueEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.edt_txt_Glass_Desc = new j2k.ValueEdit();
            this.edt_x_FlatPoint_1 = new j2k.ValueEdit();
            this.sbtn_opt_Door_Lock_Use = new j2k.StateButton();
            this.chk_opt_Align12_Only = new System.Windows.Forms.CheckBox();
            this.cb_opt_Show_Warning = new System.Windows.Forms.ComboBox();
            this.valueEdit3 = new j2k.ValueEdit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPageControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPageControl);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnApply);
            this.tabPage1.Controls.Add(this.btnRestore);
            this.tabPage1.Controls.Add(this.btnBackup);
            this.tabPage1.Controls.Add(this.btnPmacIO);
            this.tabPage1.Controls.Add(this.btnSave);
            this.tabPage1.Controls.Add(this.btnLoad);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnSaveCSV);
            this.tabPage1.Controls.Add(this.btnLoadCSV);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.btnClear);
            this.tabPage1.Controls.Add(this.btnPmacVar);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnRestore
            // 
            resources.ApplyResources(this.btnRestore, "btnRestore");
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnBackup
            // 
            resources.ApplyResources(this.btnBackup, "btnBackup");
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnPmacIO
            // 
            resources.ApplyResources(this.btnPmacIO, "btnPmacIO");
            this.btnPmacIO.Name = "btnPmacIO";
            this.btnPmacIO.UseVisualStyleBackColor = true;
            this.btnPmacIO.Click += new System.EventHandler(this.btnPmacIO_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnSaveCSV
            // 
            resources.ApplyResources(this.btnSaveCSV, "btnSaveCSV");
            this.btnSaveCSV.Name = "btnSaveCSV";
            this.btnSaveCSV.UseVisualStyleBackColor = true;
            this.btnSaveCSV.Click += new System.EventHandler(this.btnSaveCSV_Click);
            // 
            // btnLoadCSV
            // 
            resources.ApplyResources(this.btnLoadCSV, "btnLoadCSV");
            this.btnLoadCSV.Name = "btnLoadCSV";
            this.btnLoadCSV.UseVisualStyleBackColor = true;
            this.btnLoadCSV.Click += new System.EventHandler(this.btnLoadCSV_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnPmacVar
            // 
            resources.ApplyResources(this.btnPmacVar, "btnPmacVar");
            this.btnPmacVar.Name = "btnPmacVar";
            this.btnPmacVar.UseVisualStyleBackColor = true;
            this.btnPmacVar.Click += new System.EventHandler(this.btnPmacVar_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnOutputIO);
            this.tabPage2.Controls.Add(this.btnInputIO);
            this.tabPage2.Controls.Add(this.btnFixed);
            this.tabPage2.Controls.Add(this.btnParam);
            this.tabPage2.Controls.Add(this.pgIO);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnOutputIO
            // 
            resources.ApplyResources(this.btnOutputIO, "btnOutputIO");
            this.btnOutputIO.Name = "btnOutputIO";
            this.btnOutputIO.UseVisualStyleBackColor = true;
            this.btnOutputIO.Click += new System.EventHandler(this.btnOutputIO_Click);
            // 
            // btnInputIO
            // 
            resources.ApplyResources(this.btnInputIO, "btnInputIO");
            this.btnInputIO.Name = "btnInputIO";
            this.btnInputIO.UseVisualStyleBackColor = true;
            this.btnInputIO.Click += new System.EventHandler(this.btnInputIO_Click);
            // 
            // btnFixed
            // 
            resources.ApplyResources(this.btnFixed, "btnFixed");
            this.btnFixed.Name = "btnFixed";
            this.btnFixed.UseVisualStyleBackColor = true;
            this.btnFixed.Click += new System.EventHandler(this.btnFixed_Click);
            // 
            // btnParam
            // 
            resources.ApplyResources(this.btnParam, "btnParam");
            this.btnParam.Name = "btnParam";
            this.btnParam.UseVisualStyleBackColor = true;
            this.btnParam.Click += new System.EventHandler(this.button1_Click);
            // 
            // pgIO
            // 
            resources.ApplyResources(this.pgIO, "pgIO");
            this.pgIO.Name = "pgIO";
            // 
            // tabPageControl
            // 
            this.tabPageControl.Controls.Add(this.btnAssignToControl);
            this.tabPageControl.Controls.Add(this.btnAssignFromControl);
            this.tabPageControl.Controls.Add(this.edt_opt_Lamp_ConfigRun);
            this.tabPageControl.Controls.Add(this.label5);
            this.tabPageControl.Controls.Add(this.label6);
            this.tabPageControl.Controls.Add(this.label7);
            this.tabPageControl.Controls.Add(this.label4);
            this.tabPageControl.Controls.Add(this.label3);
            this.tabPageControl.Controls.Add(this.label2);
            this.tabPageControl.Controls.Add(this.edt_txt_Glass_Desc);
            this.tabPageControl.Controls.Add(this.edt_x_FlatPoint_1);
            this.tabPageControl.Controls.Add(this.sbtn_opt_Door_Lock_Use);
            this.tabPageControl.Controls.Add(this.chk_opt_Align12_Only);
            this.tabPageControl.Controls.Add(this.cb_opt_Show_Warning);
            resources.ApplyResources(this.tabPageControl, "tabPageControl");
            this.tabPageControl.Name = "tabPageControl";
            this.tabPageControl.UseVisualStyleBackColor = true;
            // 
            // btnAssignToControl
            // 
            resources.ApplyResources(this.btnAssignToControl, "btnAssignToControl");
            this.btnAssignToControl.Name = "btnAssignToControl";
            this.btnAssignToControl.UseVisualStyleBackColor = true;
            this.btnAssignToControl.Click += new System.EventHandler(this.btnAssignToControl_Click);
            // 
            // btnAssignFromControl
            // 
            resources.ApplyResources(this.btnAssignFromControl, "btnAssignFromControl");
            this.btnAssignFromControl.Name = "btnAssignFromControl";
            this.btnAssignFromControl.UseVisualStyleBackColor = true;
            this.btnAssignFromControl.Click += new System.EventHandler(this.btnAssignFromControl_Click);
            // 
            // edt_opt_Lamp_ConfigRun
            // 
            resources.ApplyResources(this.edt_opt_Lamp_ConfigRun, "edt_opt_Lamp_ConfigRun");
            this.edt_opt_Lamp_ConfigRun.Name = "edt_opt_Lamp_ConfigRun";
            this.edt_opt_Lamp_ConfigRun.Value = 0D;
            this.edt_opt_Lamp_ConfigRun.ValueFormat = "{0:0.###}";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // edt_txt_Glass_Desc
            // 
            resources.ApplyResources(this.edt_txt_Glass_Desc, "edt_txt_Glass_Desc");
            this.edt_txt_Glass_Desc.Name = "edt_txt_Glass_Desc";
            this.edt_txt_Glass_Desc.Value = 0D;
            this.edt_txt_Glass_Desc.ValueFormat = "{0:0.###}";
            // 
            // edt_x_FlatPoint_1
            // 
            resources.ApplyResources(this.edt_x_FlatPoint_1, "edt_x_FlatPoint_1");
            this.edt_x_FlatPoint_1.Name = "edt_x_FlatPoint_1";
            this.edt_x_FlatPoint_1.Value = 0D;
            this.edt_x_FlatPoint_1.ValueFormat = "{0:0.###}";
            // 
            // sbtn_opt_Door_Lock_Use
            // 
            this.sbtn_opt_Door_Lock_Use.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.sbtn_opt_Door_Lock_Use, "sbtn_opt_Door_Lock_Use");
            this.sbtn_opt_Door_Lock_Use.Name = "sbtn_opt_Door_Lock_Use";
            this.sbtn_opt_Door_Lock_Use.State = false;
            this.sbtn_opt_Door_Lock_Use.StateProp.BorderColor = System.Drawing.Color.Black;
            this.sbtn_opt_Door_Lock_Use.StateProp.ChangeImage = false;
            this.sbtn_opt_Door_Lock_Use.StateProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.sbtn_opt_Door_Lock_Use.StateProp.OffColor1 = System.Drawing.SystemColors.ButtonFace;
            this.sbtn_opt_Door_Lock_Use.StateProp.OffColor2 = System.Drawing.SystemColors.ButtonShadow;
            this.sbtn_opt_Door_Lock_Use.StateProp.OnColor1 = System.Drawing.SystemColors.ButtonFace;
            this.sbtn_opt_Door_Lock_Use.StateProp.OnColor2 = System.Drawing.Color.Lime;
            this.sbtn_opt_Door_Lock_Use.StateProp.State = false;
            this.sbtn_opt_Door_Lock_Use.StateProp.UseGradient = true;
            this.sbtn_opt_Door_Lock_Use.UseVisualStyleBackColor = false;
            // 
            // chk_opt_Align12_Only
            // 
            resources.ApplyResources(this.chk_opt_Align12_Only, "chk_opt_Align12_Only");
            this.chk_opt_Align12_Only.Name = "chk_opt_Align12_Only";
            this.chk_opt_Align12_Only.UseVisualStyleBackColor = true;
            // 
            // cb_opt_Show_Warning
            // 
            this.cb_opt_Show_Warning.FormattingEnabled = true;
            this.cb_opt_Show_Warning.Items.AddRange(new object[] {
            resources.GetString("cb_opt_Show_Warning.Items"),
            resources.GetString("cb_opt_Show_Warning.Items1"),
            resources.GetString("cb_opt_Show_Warning.Items2"),
            resources.GetString("cb_opt_Show_Warning.Items3"),
            resources.GetString("cb_opt_Show_Warning.Items4"),
            resources.GetString("cb_opt_Show_Warning.Items5")});
            resources.ApplyResources(this.cb_opt_Show_Warning, "cb_opt_Show_Warning");
            this.cb_opt_Show_Warning.Name = "cb_opt_Show_Warning";
            // 
            // valueEdit3
            // 
            resources.ApplyResources(this.valueEdit3, "valueEdit3");
            this.valueEdit3.Name = "valueEdit3";
            this.valueEdit3.Value = 0D;
            this.valueEdit3.ValueFormat = "{0:0.###}";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPageControl.ResumeLayout(false);
            this.tabPageControl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnPmacVar;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnSaveCSV;
        private System.Windows.Forms.Button btnLoadCSV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.PropertyGrid pgIO;
        private System.Windows.Forms.Button btnParam;
        private System.Windows.Forms.Button btnPmacIO;
        private System.Windows.Forms.Button btnFixed;
        private System.Windows.Forms.Button btnInputIO;
        private System.Windows.Forms.Button btnOutputIO;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.TabPage tabPageControl;
        private j2k.StateButton sbtn_opt_Door_Lock_Use;
        private System.Windows.Forms.CheckBox chk_opt_Align12_Only;
        private System.Windows.Forms.ComboBox cb_opt_Show_Warning;
        private j2k.ValueEdit edt_txt_Glass_Desc;
        private j2k.ValueEdit edt_x_FlatPoint_1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private j2k.ValueEdit valueEdit3;
        private j2k.ValueEdit edt_opt_Lamp_ConfigRun;
        private System.Windows.Forms.Button btnAssignToControl;
        private System.Windows.Forms.Button btnAssignFromControl;
    }
}

