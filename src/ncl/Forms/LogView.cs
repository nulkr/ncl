using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net.Appender;
using System.IO;

namespace ncl
{
    /// log4net의 log data를 control에 display 하려면 IAppender만 구현하고 Repository를 연결해준다
    /// http://weblogs.asp.net/psteele/live-capture-of-log4net-logging
    public partial class LogView : Form, IAppender
    {
        public int MaxLines = 1024;

        public LogView(string loadingFile = "", int maxLines = 102)
        {
            InitializeComponent();

            MaxLines = maxLines;

            // loading previous log file
            if (loadingFile != "" && File.Exists(loadingFile))
            {
                string[] lines = File.ReadAllLines(loadingFile, Encoding.UTF8);

                for (int i = Utils.EnsureRange(lines.Length - MaxLines, 0, int.MaxValue); i < lines.Length; i++)
                    listBox.Items.Add(lines[i]);
            }

            // Repository를 연결
            ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).Root.AddAppender(this);
        }

        public void DoAppend(log4net.Core.LoggingEvent e)
        {
            listBox.InvokeIfNeeded(() =>
            {
                string s = DateTime.Now.ToString("yy-MM-dd|HH:mm:ss|fff|") + string.Format("{0}|{1}\r\n", e.Level.Name, e.MessageObject.ToString());

                // TODO : 깜빡임이 발생하지만 TextBox로는 답이 없음 ListBox는?????
                if (listBox.Items.Count >= MaxLines)
                    listBox.Items.RemoveAt(0);

                listBox.Items.Add(s);
            });
        }

        // scroll to end
        private void LogView_Shown(object sender, EventArgs e)
        {
            listBox.SelectedIndex = listBox.Items.Count - 1;
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            //e.DrawBackground();

            e.Graphics.DrawString(listBox.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Black), e.Bounds.X, e.Bounds.Y);
        }
    }
}
