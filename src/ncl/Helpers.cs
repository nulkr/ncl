using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections.Generic;

namespace ncl
{
    public static class ControlHelper
    {
        /// 불필요한 UI Drawing 을 수행하지 않도록 한다
        /// http://stackoverflow.com/questions/778095/windows-forms-using-backgroundimage-slows-down-drawing-of-the-forms-controls
        public static void SuspendDrawing(this Control ctrl)
        {
            WinApi.SendMessage(ctrl.Handle, WinApi.WM_SETREDRAW, 0, 0);
        }
        public static void ResumeDrawing(this Control ctrl, bool redraw = true)
        {
            WinApi.SendMessage(ctrl.Handle, WinApi.WM_SETREDRAW, 1, 0);
            if (redraw) ctrl.Refresh();
        }

        /// generic invoke
        /// http://www.devpia.com/Maeul/Contents/Detail.aspx?BoardID=18&MAEULNO=8&no=1723&page=8
        public static void InvokeIfNeeded(this Control ctrl, Action action)
        {
            if (ctrl.InvokeRequired)
                ctrl.Invoke(action);
            else
                action();
        }
        public static void InvokeIfNeeded<T>(this Control ctrl, Action<T> action, T args)
        {
            if (ctrl.InvokeRequired)
                ctrl.Invoke(action, args);
            else
                action(args);
        }

        /// Hide / Close child forms
        /// 
        public static void HideChildForms(this Control ctrl)
        {
            if (ctrl != null)
            foreach (Control f in ctrl.Controls)
            {
                if (f is Form)
                {
                    (f as Form).Hide();
                }
            }
        }
        public static void CloseChildForms(this Control ctrl)
        {
            if (ctrl != null)
            foreach (Control f in ctrl.Controls)
            {
                if (f is Form)
                {
                    (f as Form).Close();
                }
            }
        }
    }

    public static class FormHelper
    {
        /// Enable/Disable child controls
        /// 
        public static void DisableControls(this Form form)
        {
            form.SuspendDrawing();

            foreach (Control ctrl in form.Controls)
            {
                ctrl.Enabled = false;
            }

            form.ResumeDrawing();
        }
        public static void EanbleControls(this Form form, int authority)
        {
            form.SuspendDrawing();

            foreach (Control ctrl in form.Controls)
            {
                if (ctrl.ForeColor.Equals(Color.Blue) || ctrl.ForeColor.Equals(Color.Navy))
                    ctrl.Enabled = authority > 0;
                else if (ctrl.ForeColor == Color.Red || ctrl.ForeColor == Color.Maroon)
                    ctrl.Enabled = authority > 1;
                else if (ctrl.ForeColor == Color.Purple)
                    ctrl.Enabled = authority >= 2;
                else
                    ctrl.Enabled = true;
            }

            form.ResumeDrawing();
        }

        /// Show Fom in parent control and Close/Hide previous insided forms
        /// <param name="form"></param>
        /// <param name="parent"></param>
        /// <param name="ClosePreFroms"></param>
        public static void ShowInside(this Form form, Control parent)
        {
            form.TopLevel = false;
            parent.Controls.Add(form);
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Show();
        }
    }
}
