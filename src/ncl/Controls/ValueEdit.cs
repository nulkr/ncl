using System;
using System.Windows.Forms;

namespace ncl
{
    public class ValueEdit : TextBox
    {
        #region field

        private string _ValueFormat;

        #endregion field

        #region property

        public string ValueFormat
        {
            get { return _ValueFormat; }
            set
            {
                if (_ValueFormat != value)
                {
                    _ValueFormat = value;
                    UpdateText(Value);
                }
            }
        }

        public double Value
        {
            get
            {
                try
                {
                    return Convert.ToDouble(Text);
                }
                catch
                {
                    return 0;
                }
            }
            set { UpdateText(value); }
        }

        [System.ComponentModel.Browsable(false)]
        public int AsInt
        {
            get { return (int)Math.Round(Value); }
        }

        #endregion property

        #region constructor

        public ValueEdit()
        {
            _ValueFormat = "{0:0.###}";    // { index[,alignment][ :formatString] }

            Text = "0";
        }

        #endregion constructor

        #region method

        private void UpdateText(double value)
        {
            Text = String.Format(_ValueFormat, value);
        }

        #endregion method
    }
}