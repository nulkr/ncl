using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace ncl
{
    public class StateLabel : Label
    {
        #region field

        private StateProp _StateProp;
        private string _ValueFormat;
        private int _ValueSubIndex0, _ValueSubIndex1;
        #endregion

        #region constructor

        public StateLabel() 
        {
            _StateProp = new StateProp(this);

            _ValueFormat = "{0:0.###}"; // { index[,alignment][:formatString] }
            _ValueSubIndex0 = 0;        // index of {
            _ValueSubIndex1 = 0;        // right index of }

            Text = "0";
            BorderStyle = BorderStyle.None;
            TextAlign = ContentAlignment.MiddleCenter;
            AutoSize = false;
        }
        #endregion

        #region property

        public bool State
        {
            get { return _StateProp.State; }
            set { _StateProp.State = value; }
        }
        public StateColorSet ColorSet
        {
            get { return _StateProp.ColorSet;  }
            set { StateProp.ColorSet = value; }
        }
        public string ValueFormat
        {
            get { return _ValueFormat; }
            set
            {
                if (_ValueFormat != value)
                {
                    _ValueFormat = value;
                    _ValueSubIndex0 = value.IndexOf('{');
                    _ValueSubIndex1 = value.Length - value.IndexOf('}') - 1;
                    UpdateText(Value);
                }
            }
        }

        public double Value
        {
            get
            {
                string tmps = Text;
                try
                {
                    double res;
                    if (_ValueSubIndex0 == 0 && _ValueSubIndex1 == 0)
                    {
                        if (!Double.TryParse(Text, out res)) return 0;
                    }
                    else
                    {
                        string s = Text.Substring(_ValueSubIndex0, Text.Length - _ValueSubIndex0 - _ValueSubIndex1);
                        if (!Double.TryParse(s, out res))
                            return 0;
                    }
                    return res;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            set { UpdateText(value); }
        }

        [Browsable(false)]
        public int ValueInt
        {
            get { return (int)Math.Round(Value); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public StateProp StateProp
        {
            get { return _StateProp; }
            set { _StateProp = value; }
        }
        #endregion

        #region override Label

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            _StateProp.Initialize();
        }
        #endregion

        #region method

        private void UpdateText(double value)
        {
            Text = String.Format(_ValueFormat, value);
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            if (Created) StateProp.UpdateBitmap();

            base.OnPaddingChanged(e);
        }
        #endregion
    }
}
