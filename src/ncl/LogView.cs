using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using log4net.Appender;

namespace ncl
{
    public partial class LogView : Form, IAppender
    {
        public int MaxLines = 1024;

        public static void Execute(string loadingFile = "", int maxLines = 102)
        {
            string winCaption = App.Name + " Log View";
 
            IntPtr h = WinApi.FindWindow(null, winCaption);

            if ((long)h > 0)
            {
                WinApi.ShowWindow(h, WinApi.SW_SHOWNORMAL);
            }
            else
            {
                LogView v = (new LogView(loadingFile, maxLines));
                v.Text = winCaption;
                v.Show();
            }
        }

        public LogView(string loadingFile = "", int maxLines = 102)
        {
            InitializeComponent();

            // log4net의 log data를 control에 display 하려면 IAppender만 구현하고 Repository를 연결해준다
            // http://weblogs.asp.net/psteele/live-capture-of-log4net-logging
            
            // Repository를 연결
            ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).Root.AddAppender(this);

            MaxLines = maxLines;

            // loading previous log file
            if (loadingFile != "" && File.Exists(loadingFile))
            {
                string[] lines = File.ReadAllLines(loadingFile, Encoding.UTF8);

                for (int i = (lines.Length - MaxLines).EnsureRange(0, int.MaxValue); i < lines.Length; i++)
                    listBox.Items.Add(lines[i]);
            }
            else 
            {
                string filename = null;

                IAppender[] appenders = ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).GetAppenders();

                // Check each appender this logger has
                foreach (IAppender appender in appenders)
                {
                    Type t = appender.GetType();
                    // Get the file name from the first FileAppender found and return
                    if (t.Equals(typeof(FileAppender)) || t.Equals(typeof(RollingFileAppender)))
                    {
                        filename = ((FileAppender)appender).File;

                        string[] lines = File.ReadAllLines(filename, Encoding.UTF8);

                        for (int i = (lines.Length - MaxLines).EnsureRange(0, int.MaxValue); i < lines.Length; i++)
                            listBox.Items.Add(lines[i]);
                    }
                }

            }
        }

        public void DoAppend(log4net.Core.LoggingEvent e)
        {
            listBox.InvokeIfNeeded(() =>
            {
                //string s = e.TimeStamp.ToString("yy-MM-dd|HH:mm:ss|fff|") + string.Format("{0}|{1}\r\n", e.Level.Name, e.MessageObject.ToString());
                string s = e.TimeStamp.ToString("yy-MM-dd|HH:mm:ss|") + e.MessageObject.ToString();

                // TODO : 깜빡임이 발생하지만 TextBox로는 답이 없음 ListBox는?????
                if (listBox.Items.Count >= MaxLines)
                    listBox.Items.RemoveAt(0);

                listBox.Items.Add(s);

                listBox.SelectedIndex = listBox.Items.Count - 1;    
            });
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            //e.DrawBackground();

           // e.Graphics.DrawString(listBox.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Black), e.Bounds.X, e.Bounds.Y);
        }

        private void LogView_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible) return;

            listBox.SelectedIndex = listBox.Items.Count - 1;
        }
    }
}