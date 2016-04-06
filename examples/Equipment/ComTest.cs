using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using j2k;

namespace j2kTestEquipment
{
    public partial class ComTest : j2k.ComPort
    {
        public ComTest()
        {
            InitializeComponent();
        }
        public ComTest(string comName, string iniFileName):base(comName, iniFileName)
        {
            InitializeComponent();
        }

        public override void Send(string s)
        {
            base.Send(s);
        }

        public override void AddToListBox(string s)
        {
            base.AddToListBox(s);
        }

        private void seqMonitoring_OnWork(object sender, DoWorkEventArgs e)
        {
            // TODO :
        }

        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // TODO :
        }

        private void serialPort_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            // TODO :
        }
    }
}
