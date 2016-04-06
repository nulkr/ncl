using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace ncl
{
    public class StateButton : Button
    {
        #region field

        private StateProp _StateProp;
        #endregion

        #region property

        public bool State
        {
            get { return _StateProp.State; }
            set { _StateProp.State = value; }
        }
        public StateColorSet ColorSet
        {
            get { return _StateProp.ColorSet; }
            set { StateProp.ColorSet = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public StateProp StateProp
        {
            get { return _StateProp; }
            set { _StateProp = value; }
        }
        #endregion

        #region constructor

        public StateButton() 
        {
            Padding = new Padding(10);

            _StateProp = new StateProp(this);
        }
        #endregion
        
        #region override Button

        protected override void OnClick(EventArgs e)
        {
            State = !State;

            base.OnClick(e);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            _StateProp.Initialize();
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            if (Created) StateProp.UpdateBitmap();

            base.OnPaddingChanged(e);
        }

        #endregion
    }
}
