using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace ncl
{
    public class RadioGroup : GroupBox
    {
        #region field

        private int _SelectedIndex = -1;
        private int _DownedIndex = -1;
        private int _HoveredIndex = -1;
        private int _Column = 1;
        private string[] _Items = new string[0];
        private Rectangle[] _Rects;
        #endregion

        #region constructor

        public RadioGroup()
        {
            DoubleBuffered = true;
        }
        #endregion

        #region property

        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Items
        {
            get
            {
                return string.Join("\r\n", _Items);
            }
            set
            {
                string[] sep = { "\r\n" };
                _Items = value.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                CalcLayout();
                Invalidate();
            }
        }

        public int Column
        {
            get
            {
                return _Column;
            }
            set
            {
                if (value < 1) _Column = 1;
                else _Column = value;

                CalcLayout();
                Invalidate();
            }
        }

        [Browsable(false)]
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                int n;
                if (Count == 0) n = -1;
                else if (value < 0) n = -1;
                else if (value >= Count) n = Count - 1;
                else n = value;

                if (n != _SelectedIndex)
                {
                    _SelectedIndex = n;
                    CalcLayout();
                    Invalidate();
                }
            }
        }

        [Browsable(false)]
        public int Count { get { return _Items.Length; } }

        [Browsable(false)]
        public string this[int index]
        {
            get
            {
                if (Count > 0 && index > -1 && index < Count) return _Items[index];
                else return "";
            }
            set
            {
                if (Count > 0 && index > -1 && index < Count) _Items[index] = value;
            }
        }

        #endregion

        #region method

        private void CalcLayout()
        {
            _Rects = new Rectangle[Count];

            Rectangle cr = ClientRectangle;
            cr.Y = cr.Y + 10;
            cr.Inflate(-10, -10);

            int nRow = Count / Column;
            if (nRow == 0) return;
            if (Count % Column > 0) nRow++;

            int h = (cr.Bottom - cr.Top) / nRow;
            int w = (cr.Right - cr.Left) / Column;
            cr.Height = h;
            cr.Width = w;

            for (int j = 0; j < nRow; j++)
            {
                for (int i = 0; i < Column; i++)
                {
                    int n = i + Column * j;
                    if (n >= Count) break;

                    _Rects[n] = cr;
                    _Rects[n].Offset(w * i, h * j);
                }
            }
        }
        #endregion

        #region override GroupBox

        protected override void OnSizeChanged(EventArgs e)
        {
            CalcLayout();

            base.OnSizeChanged(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            _DownedIndex = -1;

            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < Count; i++)
                    if (_Rects[i].Contains(e.Location))
                    {
                        _DownedIndex = i;
                        Invalidate();
                        break;
                    }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < Count; i++)
                    if (_Rects[i].Contains(e.Location))
                    {
                        if (_DownedIndex == i) SelectedIndex = i;
                        _DownedIndex = -1;
                        Invalidate();
                        break;
                    }
            }

            _DownedIndex = -1;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            for (int i = 0; i < Count; i++)
                if (_Rects[i].Contains(e.Location))
                {
                    _HoveredIndex = i;
                    Invalidate();
                    break;
                }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            _HoveredIndex = -1;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            System.Windows.Forms.VisualStyles.RadioButtonState state;

            TextFormatFlags flags = TextFormatFlags.VerticalCenter;

            for (int i = 0; i < Count; i++)
            {
                if (i == _SelectedIndex)
                {
                    if (Enabled)
                    {
                        if (i == _DownedIndex) state = RadioButtonState.CheckedPressed;
                        else if (i == _HoveredIndex) state = RadioButtonState.CheckedHot;
                        else state = RadioButtonState.CheckedNormal;
                    }
                    else
                        state = RadioButtonState.CheckedDisabled;
                }
                else
                {
                    if (Enabled)
                    {
                        if (i == _DownedIndex) state = RadioButtonState.UncheckedPressed;
                        else if (i == _HoveredIndex) state = RadioButtonState.UncheckedHot;
                        else state = RadioButtonState.UncheckedNormal;
                    }
                    else
                        state = RadioButtonState.UncheckedDisabled;
                }

                Rectangle r = _Rects[i];
                r.X = r.X + 18;

                Point p = _Rects[i].Location;
                p.Y = p.Y + r.Height / 2 - 6;

                RadioButtonRenderer.DrawRadioButton(e.Graphics, p, r, _Items[i], this.Font, flags, this.Focused, state);
            }
        }
        #endregion
    }
}


 