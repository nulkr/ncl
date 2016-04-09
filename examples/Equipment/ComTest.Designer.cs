namespace exEquipment
{
    partial class ComTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComTest));
            this.SuspendLayout();
            // 
            // seqMonitoring
            // 
            this.seqMonitoring.OnWork += new System.ComponentModel.DoWorkEventHandler(this.seqMonitoring_OnWork);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(306, 6);
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnMonitoring
            // 
            this.btnMonitoring.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMonitoring.BackgroundImage")));
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
            // 
            // btnConnected
            // 
            this.btnConnected.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnConnected.BackgroundImage")));
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
            // 
            // listBox
            // 
            this.listBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.listBox.Size = new System.Drawing.Size(432, 522);
            // 
            // propGrid
            // 
            this.propGrid.Location = new System.Drawing.Point(146, 0);
            // 
            // cbInput
            // 
            this.cbInput.Size = new System.Drawing.Size(134, 23);
            // 
            // ComTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.ClientSize = new System.Drawing.Size(432, 526);
            this.Name = "ComTest";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
