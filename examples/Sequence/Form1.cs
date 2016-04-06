using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using j2k;

namespace j2kTestSequence
{
    public partial class Form1 : TestForm
    {
        public Form2 F;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 999000000;
            seq1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            seq1.Abort();
        }

        private void seq1_OnCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (seq1.Status)
            {
                case SeqStatus.Error:
                    listBox1.Items.Add(seq1.Status.ToString() + " : Error Code = " + seq1.ErrorCode);
                    break;
                case SeqStatus.Aborted:
                case SeqStatus.Exception:
                case SeqStatus.Paused:
                    listBox1.Items.Add(seq1.Status.ToString() + " : Current SeqNo = " + seq1.SeqNo);
                    break;
                default:
                    listBox1.Items.Add(seq1.Status.ToString());
                    break;
            }
        }

        private void seq1_OnProgress(object sender, ProgressChangedEventArgs e)
        {
            if (seq1.SeqNo >= 0 && seq1.SeqNo <= progressBar1.Maximum)
                progressBar1.Value = seq1.SeqNo;

            label1.Text = seq1.SeqNo.ToString();
        }

        private void seq1_OnSequence(object sender, DoWorkEventArgs e)
        {
            // WARNING : Only Sequence routine, NOT UI update

            var seq = sender as Sequence;
            {
                switch (seq.SeqNo)
                {
                    case 1:
                        seq.Next();
                        break;
                    case 100000:
                        seq.Jump(10);
                        break;
                    case 1000000:
                        seq.Finish();
                        break;
                    default:
                        seq.Next();
                        break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            seq1.Error(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            seq1.Pause();
        }

        private void button4_Click(object sender,
            Args e)
        {
            seq1.Resume();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            F = new Form2();
            F.Owner = this;
            F.TopLevel = false;
            this.Controls.Add(F);
            F.Show();
            F.label1.Text = "1234";
        }

        private void stateButton1_Click(object sender, EventArgs e)
        {
        }
    }
}
