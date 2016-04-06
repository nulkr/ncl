using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace j2kTestSequence
{
    public class TestForm : Form
    {
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            j2k.MsgBox.Show("ddd");
        }
    }
}
