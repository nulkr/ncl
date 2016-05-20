using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

namespace ncl
{
    public static class ControlHelper
    {
        /// <summary>
        /// Enable Double Buffering
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="setting"></param>
        public static void DoubleBuffered(this Control ctrl, bool setting)
        {
            Type dgvType = ctrl.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(ctrl, setting, null);
        }

        public static int AsInt(this Control ctrl)
        {
            int n = 0;
            int.TryParse(ctrl.Text, out n);
            return n;
        }

        public static double AsDouble(this Control ctrl)
        {
            double d = 0;
            double.TryParse(ctrl.Text, out d);
            return d;
        }

        public static void SetValue(this Control ctrl, object o)
        {
            ctrl.Text = o.ToString();
        }

        public static void SetValue(this Control ctrl, double value, string fmt)
        {
            ctrl.Text = value.ToString(fmt);
        }

        /// <summary>
        /// 불필요한 UI Drawing 을 수행하지 않도록 한다
        /// http://stackoverflow.com/questions/778095/windows-forms-using-backgroundimage-slows-down-drawing-of-the-forms-controls
        /// </summary>
        /// <param name="ctrl"></param>
        public static void SuspendDrawing(this Control ctrl)
        {
            WinApi.SendMessage(ctrl.Handle, WinApi.WM_SETREDRAW, 0, 0);
        }

        public static void ResumeDrawing(this Control ctrl, bool redraw = true)
        {
            WinApi.SendMessage(ctrl.Handle, WinApi.WM_SETREDRAW, 1, 0);
            if (redraw) ctrl.Refresh();
        }

        /// <summary>
        /// generic invoke
        /// http://www.devpia.com/Maeul/Contents/Detail.aspx?BoardID=18&MAEULNO=8&no=1723&page=8</summary>
        /// <param name="ctrl"></param>
        /// <param name="action"></param>
        public static void InvokeIfNeeded(this Control ctrl, Action action)
        {
            if (ctrl.InvokeRequired)
                ctrl.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// generic invoke
        /// http://www.devpia.com/Maeul/Contents/Detail.aspx?BoardID=18&MAEULNO=8&no=1723&page=8</summary>
        /// <param name="ctrl"></param>
        /// <param name="action"></param>
        public static void InvokeIfNeeded<T>(this Control ctrl, Action<T> action, T args)
        {
            if (ctrl.InvokeRequired)
                ctrl.Invoke(action, args);
            else
                action(args);
        }

        /// <summary>
        /// Hide child forms
        /// </summary>
        /// <param name="ctrl"></param>
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

        /// <summary>
        /// Close child forms
        /// </summary>
        /// <param name="ctrl"></param>
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

        /// <summary>
        /// Double Buffering All DataGridView in control 
        /// </summary>
        /// <param name="ctrl"></param>
        public static void DoubleBufferingAllDataGrid(this Control ctrl)
        {
            foreach (var c in ctrl.Controls)
                if (c is DataGridView)
                    ((DataGridView)c).DoubleBuffered(true);
                else if (c is Control)
                    DoubleBufferingAllDataGrid((Control)c);
        }

        /// <summary>
        /// Disable child controls
        /// </summary>
        /// <param name="form"></param>
        public static void DisableControls(this Control ctrl)
        {
            ctrl.SuspendDrawing();

            foreach (Control c in ctrl.Controls)
            {
                c.Enabled = false;
            }

            ctrl.ResumeDrawing();
        }

        /// <summary>
        /// Enable child controls
        /// </summary>
        /// <param name="form"></param>
        public static void EanbleControls(this Control ctrl, int authority)
        {
            ctrl.SuspendDrawing();

            foreach (Control c in ctrl.Controls)
            {
                if (c.ForeColor.Equals(Color.Blue) || c.ForeColor.Equals(Color.Navy))
                    c.Enabled = authority > 0;
                else if (c.ForeColor == Color.Red || c.ForeColor == Color.Maroon)
                    c.Enabled = authority > 1;
                else if (c.ForeColor == Color.Purple)
                    c.Enabled = authority >= 2;
                else
                    c.Enabled = true;
            }

            ctrl.ResumeDrawing();
        }
    }

    public static class FormHelper
    {
       /// <summary>
        /// Show Fom in parent control and Close/Hide previous insided forms
        /// </summary>
        /// <param name="form"></param>
        /// <param name="parent"></param>
        public static void ShowInside(this Form form, Control parent)
        {
            form.TopLevel = false;
            parent.Controls.Add(form);
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Show();
        }
    }

    public static class ComparableHelper
    {
        public static bool In<T>(this T self, params T[] args) where T : IComparable
        {
            foreach (T a in args)
                if (self.Equals(a)) return true;

            return false;
        }

        public static bool InRange<T>(this T self, T min, T max) where T : IComparable
        {
            return (self.CompareTo(min) >= 0 && self.CompareTo(max) <= 0);
        }

        public static T EnsureRange<T>(this T self, T min, T max) where T : IComparable
        {
            if (self.CompareTo(min) < 0) return min;
            else if (self.CompareTo(max) > 0) return max;
            return self;
        }
    }

    public static class DoubleHelper
    {
        public static bool IsZero(this double self, double ES = 10E-7)
        {
            return (self >= -ES && self <= ES);
        }

        public static bool IsSame(this double self, double value, double ES = 10E-7)
        {
            return (self - value).IsZero(ES);
        }
    }
}
