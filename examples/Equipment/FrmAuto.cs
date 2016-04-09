using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ncl;

namespace exEquipment
{
    public partial class FrmAuto : Form
    {
        public FrmAuto()
        {
            InitializeComponent();
        }

        private void stateButton3_Click(object sender, EventArgs e)
        {
            EQ.Alarms.AlarmOccur(1);
            EQ.Alarms.AlarmOccur(12);
            EQ.Alarms.AlarmOccur(1);
            EQ.Alarms.AlarmOccur(12);
            EQ.Alarms.AlarmOccur(11);
        }

    }
}
