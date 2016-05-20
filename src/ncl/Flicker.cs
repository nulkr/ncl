using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace ncl
{
    public static class Flicker
    {
        private static System.Threading.Timer _FlickerTimer = new System.Threading.Timer(DoFlickerTimer, null, 0, 500);
        private static bool _IsOn = false;

        public static bool IsOn { get { return _IsOn; } }
        
        public static void Restart(int period = 500) { _FlickerTimer.Change(0, period); }
        public static void Stop() { _FlickerTimer.Change(Timeout.Infinite, Timeout.Infinite); }
        
        private static void DoFlickerTimer(object sender) { _IsOn = !_IsOn; }
    }

    public static class FlickerHelper
    {
        /// <summary>
        /// Flickering Control.BackColor by state
        /// </summary>
        public static void SetColor(this Control self, bool state, Color onColor, Color offColor)
        {
            self.BackColor = state && Flicker.IsOn ? onColor : offColor;
        }
        /// <summary>
        /// Flickering Control.BackColor by state
        /// </summary>
        public static void SetColor(this Control self, bool state, Color onColor1, Color onColor2, Color offColor)
        {
            if (state) self.BackColor = Flicker.IsOn ? onColor1 : onColor2;
            else       self.BackColor = offColor;
        }
        public static void SetColor(this Control self, Color flickerColor1, Color flickerColor2)
        {
            self.BackColor = Flicker.IsOn ? flickerColor1 : flickerColor2;
        }
        /// <summary>
        /// Flickering Control.ImageIndex by state
        /// </summary>
        public static void SetImgIndex(this Button self, bool state, Color onColor, Color offColor)
        {
            self.BackColor = state && Flicker.IsOn ? onColor : offColor;
        }
        /// <summary>
        /// Flickering Control.ImageIndex by state
        /// </summary>
        public static void SetImgIndex(this Button self, bool state, int onIndex, int offIndex)
        {
            self.ImageIndex = state && Flicker.IsOn ? onIndex : offIndex;
        }
        /// <summary>
        /// Flickering Control.ImageIndex by state
        /// </summary>
        public static void SetImgIndex(this Button self, bool state, int onIndex1, int onIndex2, int offIndex)
        {
            if (state) self.ImageIndex = Flicker.IsOn ? onIndex1 : onIndex2;
            else       self.ImageIndex = offIndex;
        }
    }

}
