using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ncl
{
    public static class App
    {
        private static readonly string LOG4NET_XML_FILE = "ncl-log4net.xml";

        public static readonly ILog Logger = log4net.LogManager.GetLogger(typeof(App));

        public static void Init()
        {
            // 파일이 존재하면 파일로 설정
            if (File.Exists(LOG4NET_XML_FILE))
                XmlConfigurator.Configure(new System.IO.FileInfo(LOG4NET_XML_FILE));
            else // 파일이 없으면 App.Config로 설정
                XmlConfigurator.Configure();
        }

        public static bool Exists(string ID, out Mutex mutex)
        {
            bool createNew = false;

            mutex = new Mutex(true, ID, out createNew);
            
            if (!createNew)
            {
                MsgBox.Error("Application already started!");

                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        WinApi.SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
            }

            return !createNew;            
        }

        /// get exe application names
        /// http://stackoverflow.com/questions/14829689/how-to-get-exe-application-name-and-version-in-c-sharp-compact-framework
        public static string Name
        {
            get { 
                string exeName = ExeName;
                return exeName.Substring(0, exeName.IndexOf('.')); 
            }
        }
        public static string ExeName
        {
            get { return  AppDomain.CurrentDomain.FriendlyName; }
        }
        public static string Version
        {
            get { return Application.ProductVersion; }
        }
        public static string FullName
        {
            get { return Name + " ver-" + Version; }
        }
        public static string Path
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
        public static string IniFileName
        {
            get { return Path + Name + ".ini"; }
        }
    }
}
