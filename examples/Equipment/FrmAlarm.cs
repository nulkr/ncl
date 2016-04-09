using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ncl;
using ncl.Equipment;

namespace exEquipment
{
    public partial class FrmAlarm : Form
    {
        public FrmAlarm()
        {
            InitializeComponent();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            EQ.Alarms.ClearBuffers();
            Hide();
        }

        private void lbAlarms_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = lbAlarms.Items[lbAlarms.SelectedIndex].ToString();
            string[] words = s.Split(' ');

            int code = 0;
            if (words.Length > 0 && int.TryParse(words[0], out code) && code != 0)
            {
                lblTitle.Text = s;
                lblText.Text = EQ.Alarms[code].Text;
            }
        }
        public void ShowAlarm()
        {
            this.InvokeIfNeeded(() => {
                
            });
        }

        private void AlarmForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible) return;

            lbAlarms.Items.Clear();

            if (!EQ.Alarms.AlarmExists)
            {
                lblTitle.Text = "NO ALARM";
                lblText.Text = "";
                return;
            }
            else
            {
                for (int i = 0; i < Alarms.BUFFER_COUNT; i++)
                    if (EQ.Alarms.Buffers[i] != 0)
                    {
                        int code = EQ.Alarms.Buffers[i];
                        lbAlarms.Items.Add(string.Format("{0:0000} {1}", code, EQ.Alarms[code].Title));
                    }
                    else
                        break;

                lbAlarms.SelectedIndex = 0;

                Show();
            }

        }

        private void btnBuzzerOff_Click(object sender, EventArgs e)
        {
            // TODO : EQ.IOController.SetBitY....
        }                    

    }
}
