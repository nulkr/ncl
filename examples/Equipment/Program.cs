using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ncl;
using System.Threading;
using System.Diagnostics;

namespace exEquipment
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //bool createdNew = true;
            //using (Mutex mutex = new Mutex(true, "exEquipment_123456789_fqgqgwqgw", out createdNew))
            //{
            //    if (createdNew)
            //    {
            //        Application.EnableVisualStyles();
            //        Application.SetCompatibleTextRenderingDefault(false);
            //        Application.Run(EQ.MainForm = new FrmMain());
            //    }
            //    else
            //    {
            //        Process current = Process.GetCurrentProcess();
            //        foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            //        {
            //            if (process.Id != current.Id)
            //            {
            //                WinApi.SetForegroundWindow(process.MainWindowHandle);
            //                break;
            //            }
            //        }
            //    }
            //}
            Mutex mutex;
            if (!App.Exists("exEquipment", out mutex))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(EQ.MainForm = new FrmMain());
            }
            mutex.Close();
        }
    }
}
