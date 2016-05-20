using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ncl
{
    public enum StateColorSet { Custom, Lime, Aqua, Yellow, Orange, Pink, Green, Blue, Red }

    /// State 에 따라 FillRect를 그려주고 ImageIndex를 바꾼다
    /// State = true => Deginer 에서 ImageIndex 지정.
    /// State = false => ImageIndex = 0
    [TypeConverter(typeof(StatePropTypeConverter))]
    public class StateProp
    {
        #region constant

        private readonly Color[,] ColorSetArray = {
            {Color.White,   Color.Gray,             Color.White,        Color.Lime},
            {Color.White,   Color.Gray,             Color.White,        Color.Lime},
            {Color.White,   Color.Gray,             Color.White,        Color.Aqua},
            {Color.White,   Color.Gray,             Color.White,        Color.Yellow},
            {Color.White,   Color.Gray,             Color.Moccasin,     Color.DarkOrange},
            {Color.White,   Color.Gray,             Color.White,        Color.Pink},
            {Color.White,   SystemColors.Control,   Color.Lime,         Color.DarkGreen},
            {Color.White,   SystemColors.Control,   Color.Lavender,     Color.DodgerBlue}, // Start
            {Color.White,   SystemColors.Control,   Color.MistyRose,    Color.Red}}; // Red

        #endregion constant

        #region private field

        private int _OnImageIndex = 0;
        private bool _State = false;
        private bool _UseGradient = true;
        private bool _ChangeImage = false;
        private LinearGradientMode _GradientMode = LinearGradientMode.ForwardDiagonal;
        private Color _OnColor1 = Color.White;
        private Color _OnColor2 = Color.Lime;
        private Color _OffColor1 = SystemColors.Control;
        private Color _OffColor2 = Color.Gray;
        private Color _BorderColor = Color.Gray;
        private Bitmap _OnBitmap;
        private Bitmap _OffBitmap;
        private Control _Owner;
        private StateColorSet _ColorSet = StateColorSet.Custom;
        private Brush _Brush1;
        private Brush _Brush2;

        #endregion private field

        #region private method

        private void DoSizeChanged(object sender, EventArgs e)
        {
            UpdateBitmap();
        }

        private void UpdateState()
        {
            _Owner.SuspendDrawing();

            if (_ChangeImage && _OnImageIndex > 0)
            {
                _Owner.BackgroundImage = null;

                if (_State)
                    _Owner.GetType().GetProperty("ImageIndex").SetValue(_Owner, _OnImageIndex, null);
                else
                    _Owner.GetType().GetProperty("ImageIndex").SetValue(_Owner, 0, null);
            }
            else
            {
                if (_State)
                    _Owner.BackgroundImage = _OnBitmap;
                else
                    _Owner.BackgroundImage = _OffBitmap;
            }
            _Owner.ResumeDrawing();
        }

        #endregion private method

        #region constructor

        public StateProp(Control ctrlOwner)
        {
            _Owner = ctrlOwner;
            _Owner.BackgroundImageLayout = ImageLayout.Center;
            _Owner.SizeChanged += DoSizeChanged;
        }

        #endregion constructor

        #region properties

        public bool State
        {
            get { return _State; }
            set
            {
                if (value != _State)
                {
                    _State = value;
                    UpdateState();
                }
            }
        }

        public bool UseGradient
        {
            get { return _UseGradient; }
            set
            {
                if (value != _UseGradient)
                {
                    _UseGradient = value;
                    UpdateBitmap();
                }
            }
        }

        public bool ChangeImage
        {
            get { return _ChangeImage; }
            set
            {
                if (value != _ChangeImage)
                {
                    _ChangeImage = value;
                    UpdateState();
                }
            }
        }

        public bool ChangeOnClick { get; set; }

        public int OnImageIndex 
        { 
            get { return _OnImageIndex; } 
            set 
            {
                if (value != _OnImageIndex)
                {
                    _OnImageIndex = value;
                    UpdateState();
                }
            } 
        }

        public LinearGradientMode GradientMode
        {
            get { return _GradientMode; }
            set
            {
                if (value != _GradientMode)
                {
                    _GradientMode = value;
                    UpdateBitmap();
                }
            }
        }

        public Color OnColor1
        {
            get { return _OnColor1; }
            set
            {
                if (value != _OnColor1)
                {
                    _ColorSet = StateColorSet.Custom;
                    _OnColor1 = value;
                    UpdateBitmap();
                }
            }
        }

        public Color OnColor2
        {
            get { return _OnColor2; }
            set
            {
                if (value != _OnColor2)
                {
                    _ColorSet = StateColorSet.Custom;
                    _OnColor2 = value;
                    UpdateBitmap();
                }
            }
        }

        public Color OffColor1
        {
            get { return _OffColor1; }
            set
            {
                if (value != _OffColor1)
                {
                    _ColorSet = StateColorSet.Custom;
                    _OffColor1 = value;
                    UpdateBitmap();
                }
            }
        }

        public Color OffColor2
        {
            get { return _OffColor2; }
            set
            {
                if (value != _OffColor2)
                {
                    _ColorSet = StateColorSet.Custom;
                    _OffColor2 = value;
                    UpdateBitmap();
                }
            }
        }

        public Color BorderColor
        {
            get { return _BorderColor; }
            set
            {
                if (value != _BorderColor)
                {
                    _BorderColor = value;
                    UpdateBitmap();
                }
            }
        }

        public StateColorSet ColorSet
        {
            get { return _ColorSet; }
            set
            {
                _ColorSet = value;
                if (value == StateColorSet.Custom) return;

                int n = Convert.ToInt32(_ColorSet);
                _OffColor1 = ColorSetArray[n, 0];
                _OffColor2 = ColorSetArray[n, 1];
                _OnColor1 = ColorSetArray[n, 2];
                _OnColor2 = ColorSetArray[n, 3];
                UpdateBitmap();
            }
        }

        #endregion properties

        #region public method

        /// runtime 시에 property를 체크하므로 객체 생성이 완료된 후에 이 작업을 해주어야 한다
        public void Initialize()
        {
            if (_Owner is ButtonBase)
            {
                ButtonBase b = _Owner as ButtonBase;
                if (b.ImageList != null && b.ImageList.Images.Count > 1)
                    _OnImageIndex = b.ImageIndex;
            }
            else if (_Owner is Label)
            {
                Label l = _Owner as Label;
                if (l.ImageList != null && l.ImageList.Images.Count > 1)
                    _OnImageIndex = l.ImageIndex;
            }
        }

        public void UpdateBitmap()
        {
            int w = Math.Max(_Owner.Size.Width - _Owner.Padding.Size.Width, 1);
            int h = Math.Max(_Owner.Size.Height - _Owner.Padding.Size.Height, 1);

            // size == 0 이면 LinearGradientBrush 생성시 에러 발생함
            if (w <= 1 || h <= 1) return;

            _OnBitmap = new Bitmap(w, h);
            _OffBitmap = new Bitmap(w, h);

            Rectangle r = new Rectangle(0, 0, w - 1, h - 1);

            Pen p = new Pen(_BorderColor);

            if (_UseGradient)
            {
                _Brush1 = new LinearGradientBrush(r, _OnColor1, _OnColor2, _GradientMode);
                _Brush2 = new LinearGradientBrush(r, _OffColor1, _OffColor2, _GradientMode);
            }
            else
            {
                _Brush1 = new SolidBrush(_OnColor1);
                _Brush2 = new SolidBrush(_OffColor1);
            }

            Graphics g1 = Graphics.FromImage(_OnBitmap);
            Graphics g2 = Graphics.FromImage(_OffBitmap);

            g1.FillRectangle(_Brush1, r);
            g2.FillRectangle(_Brush2, r);

            g1.DrawRectangle(p, r);
            g2.DrawRectangle(p, r);

            UpdateState();
        }

        #endregion public method
    }

    /// http://www.codeproject.com/Articles/9667/Creating-Custom-Controls-Providing-Design-Time-Sup
    internal class StatePropTypeConverter : TypeConverter
    {
        /// allows us to display the + symbol near the property name
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(StateProp));
        }
    }
}