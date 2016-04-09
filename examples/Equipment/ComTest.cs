using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ncl;

namespace exEquipment
{
    public partial class ComTest : ncl.ComPort
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

        private void btnSettings_Click(object sender, EventArgs e)
        {
            IniFile ini = new IniFile(App.Path + "1.ini");
            ini.Read("TEST COM", serialPort);
            IniFile ini2 = new IniFile(App.Path + "2.ini");
            ini2.Write("TEST COM", serialPort);
        }
    }
}
