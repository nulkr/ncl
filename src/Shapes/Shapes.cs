using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace ncl
{
    namespace Shapes
    {
        using Contour = List<PointD>;
        using ContourList = List<List<PointD>>;
        using DispContour = List<PointF>;
        using DispContourList = List<List<PointF>>;
        using PointI = System.Drawing.Point;
        using RectI = System.Drawing.Rectangle;

        /// <summary>
        /// Flag Bit 은 아래 링크 처럼 쓰자
        /// http://stackoverflow.com/questions/40211/how-to-compare-flags-in-c
        /// </summary>
        [Flags]
        public enum MouseMode
        {
            None        = 0x0000,
            ZoomRect    = 0x0001,
            ZoomWheel   = 0x0002,
            Panning     = 0x0004,
        };

        public class ShapeData : System.ComponentModel.Component
        {
            internal List<Shape> _List;
            internal RectangleF _BoundsRect;

            public ShapeData()
            {
                _List = new List<Shape>();
                _BoundsRect = new RectangleF(0, 0, 0, 0);
            }

            [Browsable(false)]
            public int Count { get { return _List.Count; } }
            [Browsable(false)]
            public RectangleF BoundsRect { get { return _BoundsRect; } }

            public IEnumerator GetEnumerator()
            {
                foreach (Shape o in _List)
                {
                    yield return o;
                }
            }

            public void ReadBinary(BinaryReader reader)
            {
                Clear();

                int nCnt = reader.Read();
                for (int i = 0; i < nCnt; i++)
                {
                    //
                    // http://stackoverflow.com/questions/223952/create-an-instance-of-a-class-from-a-string
                    // http://stackoverflow.com/questions/493490/converting-a-string-to-a-class-name
                    //
                    string sClassName = reader.ReadString();

                    Shape shape = (Shape)Activator.CreateInstance(Type.GetType(sClassName));
                    
                    shape.ReadBinary(reader);
                }
            }
            public void WriteBinary(BinaryWriter writer)
            {
                writer.Write(_List.Count);

                foreach (Shape shape in _List)
                {
                    shape.WriteBinary(writer);
                }
            }

            /// <summary>
            /// Boundary 업데이트 되는 Clear
            /// </summary>
            public void Clear()
            {
                _List.Clear();
                _BoundsRect = new RectangleF(0, 0, 0, 0);
            }
            /// <summary>
            /// Boundary 가 업데이트 되는 Add
            /// </summary>
            /// <param name="aShape"> 이함수를 쓰기전에 모든 데이터를 넣어서 올바른 Boundary를 갖게 하자</param>
            public void Add(Shape aShape)
            {
                _List.Add(aShape);

                if (_List.Count == 1)
                {
                    _BoundsRect = aShape.Boundary;
                }
                else
                {
                    if (BoundsRect.X > aShape.Boundary.X) _BoundsRect.X = aShape.Boundary.X;
                    if (BoundsRect.Y > aShape.Boundary.Y) _BoundsRect.Y = aShape.Boundary.Y;
                    if (BoundsRect.Right < aShape.Boundary.Right) _BoundsRect.Width = aShape.Boundary.Right - _BoundsRect.X;
                    if (BoundsRect.Bottom < aShape.Boundary.Bottom) _BoundsRect.Height = aShape.Boundary.Bottom - _BoundsRect.Y;
                }
            }
            /// <summary>
            /// Boundary 갱신, 몽땅 다 검색
            /// </summary>
            public void UpdateBoundary()
            {
                double x1 = Double.MaxValue;
                double y1 = Double.MaxValue;
                double x2 = Double.MinValue;
                double y2 = Double.MinValue;

                foreach (Shape o in _List)
                {
                    if (o.Boundary.X < x1) x1 = o.Boundary.X;
                    if (o.Boundary.Right > x2) x2 = o.Boundary.Right;
                    if (o.Boundary.Y < y1) y1 = o.Boundary.Y;
                    if (o.Boundary.Bottom > y2) y2 = o.Boundary.Bottom;
                }

                _BoundsRect = RectangleF.FromLTRB((float)x1, (float)y1, (float)x2, (float)y2);
            }
        }

        public class ShapeView : System.Windows.Forms.Control
        {
            // tripple buffering
            Bitmap _BmpBuffer1;
            Bitmap _BmpBuffer2;
            Graphics _GraphicsBuffer1;
            Graphics _GraphicsBuffer2;

            // panning offset
            int _PanX, _PanY;
            double _OldOffsetX, _OldOffsetY;
            MouseMode _RunningMode;

            // mouse clicked positino queue
            List<PointI> _ClickedBuffer;

            DrawingInfo _DrawingInfo;

            [Browsable(false)]
            public DrawingInfo DrawingInfo { get { return _DrawingInfo; } }
            public ShapeData ShapeData { get; set; }
            public MouseMode MouseMode { get; set; }
            public Color CursorColor { get; set; }

            public ShapeView()
                : base()
            {
                ShapeData = null;

                this.MouseMode = MouseMode.ZoomRect | MouseMode.ZoomWheel | MouseMode.Panning;
                this.CursorColor = Color.Orange;

                _ClickedBuffer = new List<PointI>();
                _DrawingInfo = new DrawingInfo(this);

                // for manual tripple buffering
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
                SetStyle(ControlStyles.DoubleBuffer, false);

                _PanX = 0;
                _PanY = 0;
                _RunningMode = MouseMode.None;
            }

            [DefaultValue(typeof(Color), "LightGreen")]
            public override Color BackColor
            {
                get
                {
                    return base.BackColor;
                }
                set
                {
                    base.BackColor = value;
                    DrawData();
                    Invalidate();
                }
            }

            protected override void OnMouseWheel(MouseEventArgs e)
            {
                base.OnMouseWheel(e);

                if (MouseModeHasFlags(MouseMode.ZoomWheel))
                {
                    if (e.Delta > 0)
                        Zoom(1.2f * _DrawingInfo.PixelPerUnit, e.X, e.Y);
                    else if (e.Delta < 0 && _DrawingInfo.PixelPerUnit > 0.003)
                        Zoom(0.8f * _DrawingInfo.PixelPerUnit, e.X, e.Y);
                }
            }
            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);

                if (MouseModeHasFlags(MouseMode.Panning) && e.Button == MouseButtons.Right)
                {
                    _OldOffsetX = _DrawingInfo.OffsetX;
                    _OldOffsetY = _DrawingInfo.OffsetY;

                    _ClickedBuffer.Clear();
                    _ClickedBuffer.Add(new PointI(e.X, e.Y));

                    _PanX = 0;
                    _PanY = 0;

                    this.Cursor = Cursors.NoMove2D;

                    _RunningMode = MouseMode.Panning;
                }
            }
            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);

                if (_RunningMode == MouseMode.Panning)
                {
                    _PanX = e.X - _ClickedBuffer[0].X;
                    _PanY = e.Y - _ClickedBuffer[0].Y;

                    Invalidate();
                }
                else if (_RunningMode == MouseMode.ZoomRect)
                {
                    if (_ClickedBuffer.Count == 1)
                        _ClickedBuffer.Add(new PointI(e.X, e.Y));
                    else
                        _ClickedBuffer[1] = new PointI(e.X, e.Y);
                    Invalidate();
                }
            }
            protected override void OnMouseUp(MouseEventArgs e)
            {
                base.OnMouseUp(e);

                if (_RunningMode == MouseMode.Panning)
                {
                    _DrawingInfo.OffsetX =  e.X - _ClickedBuffer[0].X + _OldOffsetX;
                    _DrawingInfo.OffsetY = -e.Y + _ClickedBuffer[0].Y + _OldOffsetY;
                    _PanX = 0;
                    _PanY = 0;

                    _ClickedBuffer.Clear();

                    this.Cursor = Cursors.Default;

                    _RunningMode = MouseMode.None;

                    DrawData();
                    Invalidate();
                }
                else if (MouseModeHasFlags(MouseMode.ZoomRect) && e.Button == MouseButtons.Left)
                {
                    // 1st click
                    if (_RunningMode == MouseMode.None)
                    {
                        _ClickedBuffer.Add(new PointI(e.X, e.Y));
                        _RunningMode = MouseMode.ZoomRect;
                        
                        return;
                    }
                    
                    // 2nd click
                    if (_RunningMode == MouseMode.ZoomRect)
                    {
                        _RunningMode = MouseMode.None;

                        ZoomRect(_ClickedBuffer[0].X, _ClickedBuffer[0].Y, e.X, e.Y);
                        
                        _ClickedBuffer.Clear();
                    }
                }
            }
            protected override void OnPaintBackground(PaintEventArgs e)
            {
                // do nothing, prevent flickering
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                // panning 시에는 빈공간을 채워야 한다
                if (_RunningMode == MouseMode.Panning)
                {
                    using(Bitmap bmp = new Bitmap(_BmpBuffer1))
                    {
                        Utils.DrawAlpha(bmp, _BmpBuffer1, _PanX, _PanY, 0.5f);
                        _GraphicsBuffer2.DrawImageUnscaled(bmp, ClientRectangle.X, ClientRectangle.Y);
                    }
                }
                else
                    _GraphicsBuffer2.DrawImageUnscaled(_BmpBuffer1, 0, 0);

                // zoom-rect 시에는 영역에 반투명 효과를 준다
                if (_RunningMode == MouseMode.ZoomRect)
                {
                    using (Brush brush = new SolidBrush(Color.FromArgb(50, CursorColor)))
                    using (Pen pen = new Pen(CursorColor))
                    {
                        RectI rect = Utils.CreateRect_From2Point(_ClickedBuffer[0], _ClickedBuffer[1]);

                        _GraphicsBuffer2.FillRectangle(brush, rect);

                        pen.DashStyle = DashStyle.Dash;
                        _GraphicsBuffer2.DrawRectangle(pen, rect);
                    }
                }

                e.Graphics.DrawImageUnscaled(_BmpBuffer2, 0, 0);
            }
            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);


                _BmpBuffer1 = new Bitmap(ClientSize.Width, ClientSize.Height, PixelFormat.Format32bppPArgb);
                _GraphicsBuffer1 = Graphics.FromImage(_BmpBuffer1);
                _GraphicsBuffer1.SmoothingMode = SmoothingMode.AntiAlias;
                
                _BmpBuffer2 = new Bitmap(ClientSize.Width, ClientSize.Height, PixelFormat.Format32bppPArgb);
                _GraphicsBuffer2 = Graphics.FromImage(_BmpBuffer2);
                _GraphicsBuffer2.SmoothingMode = SmoothingMode.AntiAlias;

                DrawData();
                Invalidate();
            }

            public void DrawData()
            {
                if (_GraphicsBuffer1 != null)
                    _GraphicsBuffer1.Clear(BackColor);

                if (ShapeData == null)
                    return;

                foreach (Shape o in ShapeData)
                    try
                    {
                        o.Draw(_GraphicsBuffer1, _DrawingInfo);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Error(o.GetType().ToString() + " : " + ex.Message);
                    }
            }

            /// <summary>
            /// 특정 점을 고정된 위치로 두고 Zooming
            /// </summary>
            /// <param name="aScale">Pixel/Unit Scale factor</param>
            /// <param name="aPixelX">특정 Window 점 X [pixel]</param>
            /// <param name="aPixelY">특정 Window 점 Y [pixel]</param>
            public void Zoom(double aScale, float aPixelX, float aPixelY)
            {
                // get old real of mouse x,y
                double rx, ry;
                _DrawingInfo.ToReal(aPixelX, aPixelY, out rx, out ry);

                _DrawingInfo.PixelPerUnit = aScale;

                // calc new win of mouse x,y
                float wx, wy;
                _DrawingInfo.ToWin(rx, ry, out wx, out wy);

                // apply offset
                _DrawingInfo.OffsetX -= wx - aPixelX;
                _DrawingInfo.OffsetY += wy - aPixelY;

                DrawData();
                Invalidate();
            }
            /// <summary>
            /// 윈도우상의 aPixelMargin 만큼 여유를 두고 화면에 꽉차도록 Zooming
            /// </summary>
            /// <param name="aPixelMargin"></param>
            public void ZoomFit(int aPixelMargin = 10)
            {
                if (ShapeData == null || ShapeData.Count == 0)
                {
                    // TODO : 
                    // Zoom Area
                }
                else
                {
                    float cw = ClientSize.Width - aPixelMargin * 2;
                    float ch = ClientSize.Height - aPixelMargin * 2;

                    // 컴파일러 경고 CS1690 발생하므로 지역변수에 복사하여 쓴다
                    // https://msdn.microsoft.com/ko-kr/library/x524dkh4.aspx
                    ShapeData.UpdateBoundary();
                    RectangleF boundary = ShapeData.BoundsRect;

                    // calc scale
                    if (boundary.Width < Geometry.Epsilon || boundary.Height < Geometry.Epsilon)
                        _DrawingInfo.PixelPerUnit = 1.0;
                    else if (boundary.Width < Geometry.Epsilon)
                        _DrawingInfo.PixelPerUnit = ch / boundary.Height;
                    else if (boundary.Height < Geometry.Epsilon)
                        _DrawingInfo.PixelPerUnit = cw / boundary.Width;
                    else
                        _DrawingInfo.PixelPerUnit = Math.Min(cw / boundary.Width, ch / boundary.Height);

                    // calc offset
                    float wx, wy;

                    _DrawingInfo.OffsetX = 0;
                    _DrawingInfo.OffsetY = 0;

                    _DrawingInfo.ToWin(boundary.X + boundary.Width / 2, boundary.Y + boundary.Height / 2, out wx, out wy);

                    _DrawingInfo.OffsetX = -wx + ClientSize.Width / 2;
                    _DrawingInfo.OffsetY = wy - ClientSize.Height / 2;
                }

                DrawData();
                Invalidate();
            }
            /// <summary>
            /// aWinRect의 영역이 화면에 꽉차도록 Zooming
            /// </summary>
            /// <param name="aWinRect"></param>
            /// <param name="aPixelMargin"></param>
            public void ZoomRect(RectI aWinRect, int aPixelMargin = 5)
            {
                if (aWinRect.Width < Geometry.Epsilon || aWinRect.Height < Geometry.Epsilon)
                    return;

                // calc real rect size
                double rw = aWinRect.Width / _DrawingInfo.PixelPerUnit;
                double rh = aWinRect.Height / _DrawingInfo.PixelPerUnit;

                // calc real rect pos
                // 기준점은 좌하단 (좌하단이 0,0이므로)
                double rx, ry;
                _DrawingInfo.ToReal(aWinRect.X, aWinRect.Y + aWinRect.Height, out rx, out ry);

                _DrawingInfo.PixelPerUnit = Math.Min((ClientSize.Width - aPixelMargin * 2) / rw, (ClientSize.Height - aPixelMargin * 2) / rh);

                _DrawingInfo.OffsetX = -rx * _DrawingInfo.PixelPerUnit;
                _DrawingInfo.OffsetY = -ry * _DrawingInfo.PixelPerUnit;

                DrawData();
                Invalidate();
            }
            public void ZoomRect(int x1, int y1, int x2, int y2, int aPixelMargin = 5)
            {
                int x = Math.Min(x1, x2);
                int y = Math.Min(y1, y2);

                ZoomRect(Utils.CreateRect_From2Point(x1, y1, x2, y2), aPixelMargin);
            }

            public bool MouseModeHasFlags(MouseMode aMode)
            {
                return (this.MouseMode & aMode) == aMode;
            }
        }

        public struct PointD
        {
            public double X, Y;

            public PointD(double x, double y)
            {
                X = x;
                Y = y;
            }
            public PointD(PointD pt)
            {
                X = pt.X;
                Y = pt.Y;
            }
            public PointD(PointF pt)
            {
                X = pt.X;
                Y = pt.Y;
            }
            public PointD(PointI pt)
            {
                X = pt.X;
                Y = pt.Y;
            }

            public static PointD operator -(PointD p1, PointD p2)
            {
                return new PointD(p1.X - p2.X, p1.Y - p2.Y);
            }
            public static PointD operator +(PointD p1, PointD p2)
            {
                return new PointD(p1.X + p2.X, p1.Y + p2.Y);
            }
            public static bool operator !=(PointD left, PointF right)
            {
                return Math.Abs(left.X - right.X) >= Geometry.Epsilon || Math.Abs(left.Y - right.Y) >= Geometry.Epsilon;
            }
            public static bool operator ==(PointD left, PointF right)
            {
                return Math.Abs(left.X - right.X) < Geometry.Epsilon && Math.Abs(left.Y - right.Y) < Geometry.Epsilon;
            }

            public override bool Equals(object obj)
            {
                if (obj is PointD)
                    return Math.Abs(X - ((PointD)obj).X) < Geometry.Epsilon && Math.Abs(Y - ((PointD)obj).Y) < Geometry.Epsilon;
                else
                    return false;
            }
            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }
            public override string ToString()
            {
                return String.Format("{0:0.#####},{0:0.#####}", X, Y);
            }
        }

        public class DrawingInfo
        {
            private System.Windows.Forms.Control _OwnerControl;

            public double PixelPerUnit = 1.0;
            public double OffsetX = 0.0; // [pixel]
            public double OffsetY = 0.0; // [pixel]
            public Pen Pen;
            public Pen HandlePen;

            public DrawingInfo(System.Windows.Forms.Control owner)
            {
                _OwnerControl = owner;

                this.Pen = new Pen(Color.Blue, 0.8f);
                this.HandlePen = new Pen(Color.Violet, 1.5f);
            }

            public void ToReal(float Wx, float Wy, out double X, out double Y)
            {
                X = (Wx - OffsetX) / PixelPerUnit;
                Y = - (Wy + OffsetY - _OwnerControl.ClientSize.Height) / PixelPerUnit;
            }
            public void ToReal(PointF aWinPt, out double X, out double Y)
            {
                ToReal(aWinPt.X, aWinPt.Y, out X, out Y);
            }
            public PointD ToReal(PointF aWinPoint)
            {
                double x, y;
                ToReal(aWinPoint, out x, out  y);
                return new PointD(x, y);
            }
            public void ToWin(double Rx, double Ry, out float X, out float Y)
            {
                X = (float)(Rx * PixelPerUnit + OffsetX);
                Y = (float)(-Ry * PixelPerUnit - OffsetY + _OwnerControl.ClientSize.Height);
            }
            public void ToWin(PointD aRealPt, out float X, out float Y)
            {
                ToWin(aRealPt.X, aRealPt.Y, out X, out Y);
            }
            public void ToWin(PointF aRealPt, out float X, out float Y)
            {
                ToWin(aRealPt.X, aRealPt.Y, out X, out Y);
            }
            public PointF ToWin(PointD aPoint)
            {
                float x, y;
                ToWin(aPoint, out x, out  y);
                return new PointF(x, y);
            }
        }

        public static class Geometry
        {
            internal const double Epsilon = 0.00001;
            internal const float HANDLE_SIZE = 3.0f;

            public static PointD GetCirclePoint(double Cx, double Cy, double R, double aAngle)
            {
                double a = Utils.ToRadian(aAngle);
                double x = Cx + R * Math.Cos(a);
                double y = Cy + R * Math.Sin(a);

                return new PointD(x, y);
            }
        }

        public class Layer
        {
        }

        public abstract class Shape
        {
            protected ContourList _Contours = new ContourList();
            protected DispContourList _DispContours = new DispContourList();
            protected RectangleF _Boundary;

            public bool Selected { get; set; }
            public bool ShowArrow { get; set; }

            [Browsable(false)]
            public RectangleF Boundary { get { return _Boundary; } }
            
            public PointD this[int nIndex0, int nIndex1]
            {
                get { return _Contours[nIndex0][nIndex1]; }
                set 
                { 
                    _Contours[nIndex0][nIndex1] = value;

                    if (value.X < Boundary.X) _Boundary.X = (float)value.X;
                    if (value.Y < Boundary.Y) _Boundary.Y = (float)value.Y;
                    if (value.X > Boundary.Right) _Boundary.Width = (float)value.X - _Boundary.X;
                    if (value.Y > Boundary.Bottom) _Boundary.Height = (float)value.Y - _Boundary.Y;
                }
            }

            public Shape(int nContourCount = 1)
            {
                for (int i = 0; i < nContourCount; i++)
                {
                    _Contours.Add(new Contour());
                    _DispContours.Add(new DispContour());
                }

                _Boundary = new RectangleF(0, 0, 0, 0);
            }

            protected virtual void UpdateCtrlPoints()
            {
                double x1 = Double.MaxValue;
                double y1 = Double.MaxValue;
                double x2 = Double.MinValue;
                double y2 = Double.MinValue;

                foreach (Contour c in _Contours)
                {
                    foreach (PointD p in c)
                    {
                        if (p.X < x1) x1 = p.X;
                        if (p.X > x2) x2 = p.X;
                        if (p.Y < y1) y1 = p.Y;
                        if (p.Y > y2) y2 = p.Y;
                    }
                }

                _Boundary = RectangleF.FromLTRB((float)x1, (float)y1, (float)x2, (float)y2);
            }

            internal virtual void ReadBinary(BinaryReader reader)
            {

            }
            internal virtual void WriteBinary(BinaryWriter writer)
            {
                writer.Write(GetType().ToString());

                writer.Write(_Contours.Count);
                foreach (Contour c in _Contours)
                {
                    writer.Write(c.Count);
                    foreach (PointD p in c)
                    {
                        writer.Write(p.X);
                        writer.Write(p.Y);
                    }
                }
            }

            public virtual void PreDraw(Graphics aGraphic, DrawingInfo aInfo)
            {
                // calc display contours
                for (int i = 0; i < _Contours.Count; i++)
                    for (int j = 0; j < _Contours[i].Count; j++)
                    {
                        _DispContours[i][j] = aInfo.ToWin(_Contours[i][j]);
                    }

                if (ShowArrow)
                {
                    aInfo.Pen.StartCap = LineCap.RoundAnchor;
                    aInfo.Pen.CustomEndCap = new AdjustableArrowCap(5, 5);
                }
                else
                {
                    aInfo.Pen.StartCap = LineCap.NoAnchor;
                    aInfo.Pen.EndCap = LineCap.NoAnchor;
                }

                if (Selected)
                {
                    // draw handle
                    foreach (DispContour c in _DispContours)
                        foreach (PointF pt in c)
                            aGraphic.DrawRectangle(aInfo.HandlePen, (float)pt.X - Geometry.HANDLE_SIZE, (float)pt.Y - Geometry.HANDLE_SIZE, (float)Geometry.HANDLE_SIZE * 2, (float)Geometry.HANDLE_SIZE * 2);

                    aInfo.Pen.DashStyle = DashStyle.Dash;
                }
                else
                {
                    aInfo.Pen.DashStyle = DashStyle.Solid;
                }
            }
            public virtual void Draw(Graphics aGraphic, DrawingInfo aInfo)
            {
                PreDraw(aGraphic, aInfo);

                // draw polyline
                foreach (DispContour c in _DispContours)
                    aGraphic.DrawLines(aInfo.Pen, c.ToArray());
            }
            public virtual void InvertDir()
            {
                foreach (Contour c in _Contours)
                    c.Reverse();
                foreach (DispContour c in _DispContours)
                    c.Reverse();
            }

            public void AddContour()
            {
                _Contours.Add(new Contour());
                _DispContours.Add(new DispContour());
            }
            public void AddPoint(int nContourIndex, double x, double y)
            {
                _Contours[nContourIndex].Add(new PointD(x, y));
                _DispContours[nContourIndex].Add(new PointF((float)x, (float)y));

                UpdateCtrlPoints();
            }
            public void AddPoint(int nContourIndex, PointD pt)
            {
                _Contours[nContourIndex].Add(pt);
                _DispContours[nContourIndex].Add(new PointF((float)pt.X, (float)pt.Y));

                UpdateCtrlPoints();
            }
            public void AddPoint(int nContourIndex, PointD[] pts)
            {
                _Contours[nContourIndex].AddRange(pts);
                foreach (PointD p in pts)
                    _DispContours[nContourIndex].Add(new PointF((float)p.X, (float)p.Y));

                UpdateCtrlPoints();
            }
        }

        public class Point : Shape
        {
            public Point(double x = 0, double y = 0) 
                : base(1)
            {
                AddPoint(0, x, y);
            }
            public Point(PointD pt)
                : base(1)
            {
                AddPoint(0, pt);
            }

            public override void Draw(Graphics aGraphic, DrawingInfo aInfo)
            {
                PreDraw(aGraphic, aInfo);

                aGraphic.DrawEllipse(aInfo.Pen, _DispContours[0][0].X - 1.0f, _DispContours[0][0].Y - 1.0f, 2.0f, 2.0f);
            }
        }

        public class Line : Shape 
        {
            public Line(double x1 = 0.0, double y1 = 0.0, double x2 = 0.0, double y2 = 0.0) 
                : base(1)
            {
                AddPoint(0, x1, y1);
                AddPoint(0, x2, y2);
            }
            public Line(PointD pt1, PointD pt2)
                : base(1)
            {
                AddPoint(0, pt1);
                AddPoint(0, pt2);
            }
        }

        public class Polyline : Shape
        {
            public Polyline()
                : base(1)
            {
            }
            public Polyline(double x, double y)
                : base(1)
            {
                AddPoint(0, x, y);
            }
            public Polyline(PointD pt)
                : base(1)
            {
                AddPoint(0, pt);
            }
            public Polyline(PointD[] pts)
                : base(1)
            {
                AddPoint(0, pts);
            }
        }

        /// <summary>
        /// Control Point is Center, Left, Bottom, Right, Top
        /// </summary>
        public class Circle : Shape
        {
            private double _Radius = 1.0;

            public Circle(double x = 0.0, double y = 0.0, double r = 1.0)
                : base(1)
            {
                _Radius = r;

                AddPoint(0, x, y);
                AddPoint(0, x - r, y);
                AddPoint(0, x, y - r);
                AddPoint(0, x + r, y);
                AddPoint(0, x, y + r);
            }
            public Circle(PointD ptCenter, double r = 1.0)
                : base(1)
            {
                _Radius = r;

                AddPoint(0, ptCenter.X, ptCenter.Y);
                AddPoint(0, ptCenter.X - r, ptCenter.Y);
                AddPoint(0, ptCenter.X, ptCenter.Y - r);
                AddPoint(0, ptCenter.X + r, ptCenter.Y);
                AddPoint(0, ptCenter.X, ptCenter.Y + r);
            }

            public PointD PtCenter
            {
                get { return _Contours[0][0]; }
                set 
                { 
                    _Contours[0][0] = value;
                    UpdateCtrlPoints();
                }
            }
            public double CenterX
            {
                get { return _Contours[0][0].X; }
                set { PtCenter = new PointD(value, CenterY); }
            }
            public double CenterY
            {
                get { return _Contours[0][0].Y; }
                set { PtCenter = new PointD(CenterX, value); }
            }
            public double Radius
            {
                get { return _Radius; }
                set { _Radius = value; UpdateCtrlPoints(); }
            }

            protected override void UpdateCtrlPoints()
            {
                if (_Contours[0].Count < 5) return;

                _Contours[0][1] = new PointD(CenterX - Radius, CenterY); // left
                _Contours[0][2] = new PointD(CenterX, CenterY - Radius); // bottom
                _Contours[0][3] = new PointD(CenterX + Radius, CenterY); // right
                _Contours[0][4] = new PointD(CenterX, CenterY + Radius); // top

                 base.UpdateCtrlPoints();
            }

            internal override void ReadBinary(BinaryReader reader)
            {
                base.ReadBinary(reader);

                Radius = reader.ReadSingle();
            }
            internal override void WriteBinary(BinaryWriter writer)
            {
                base.WriteBinary(writer);

                writer.Write(Radius);
            }

            public override void Draw(Graphics aGraphic, DrawingInfo aInfo)
            {
                PreDraw(aGraphic, aInfo);

                float x1, y1, x2, y2;
                float wh = (float)(_Radius * 2 * aInfo.PixelPerUnit);

                aInfo.ToWin(CenterX - Radius, CenterY - Radius, out x1, out y1);
                aInfo.ToWin(CenterX + Radius, CenterY + Radius, out x2, out y2);

                aGraphic.DrawEllipse(aInfo.Pen, Math.Min(x1, x2), Math.Min(y1, y2), wh, wh);
            }
            public override void InvertDir()
            {
                // Do nothing;
            }
        }

        /// <summary>
        /// Control Point is Center, StartArcPoint, MiddleArcPoint, EndArcPoint
        /// </summary>
        public class Arc : Shape
        {
            private double _Radius = 1.0f;
            private double _StartAngle = 0.0f;
            private double _SweepAngle = 45.0f;

            public Arc(double x = 0.0, double y = 0.0, double r = 1.0, double aStartAngle = 0.0, double aSweepAngle = 45.0)
                : base(1)
            {
                _Radius = r;
                _StartAngle = aStartAngle;
                _SweepAngle = aSweepAngle;

                AddPoint(0, x, y);
                AddPoint(0, Geometry.GetCirclePoint(x, y, r, aStartAngle));
                AddPoint(0, Geometry.GetCirclePoint(x, y, r, aStartAngle + aSweepAngle / 2.0));
                AddPoint(0, Geometry.GetCirclePoint(x, y, r, aStartAngle + aSweepAngle));
            }

            public PointD PtCenter
            {
                get { return _Contours[0][0]; }
                set { _Contours[0][0] = value; UpdateCtrlPoints(); }
            }
            public double CenterX
            {
                get { return _Contours[0][0].X; }
                set { PtCenter = new PointD(value, CenterY); }
            }
            public double CenterY
            {
                get { return _Contours[0][0].Y; }
                set { PtCenter = new PointD(CenterX, value); }
            }
            public double Radius
            {
                get { return _Radius; }
                set { _Radius = value; UpdateCtrlPoints(); }
            }
            public double StartAngle
            {
                get { return _StartAngle; }
                set { _StartAngle = value; UpdateCtrlPoints(); }
            }
            public double SweepAngle
            {
                get { return _SweepAngle; }
                set { _SweepAngle = value; UpdateCtrlPoints(); }
            }

            protected override void UpdateCtrlPoints()
            {
                if (_Contours[0].Count < 4) return;

                // Start Arc Point
                _Contours[0][1] = Geometry.GetCirclePoint(CenterX, CenterY, Radius, StartAngle);

                // Middle Arc Point
                _Contours[0][2] = Geometry.GetCirclePoint(CenterX, CenterY, Radius, StartAngle + SweepAngle / 2.0f);

                // End Arc Point
                _Contours[0][3] = Geometry.GetCirclePoint(CenterX, CenterY, Radius, StartAngle + SweepAngle);

                // calc boundary rect
                double x1 = Double.MaxValue;
                double y1 = Double.MaxValue;
                double x2 = Double.MinValue;
                double y2 = Double.MinValue;

                PointD p;

                // compare start point
                p = Geometry.GetCirclePoint(CenterX, CenterY, Radius, StartAngle);
                if (p.X < x1) x1 = p.X;
                if (p.X > x2) x2 = p.X;
                if (p.Y < y1) y1 = p.Y;
                if (p.Y > y2) y2 = p.Y;

                // compare end point
                p = Geometry.GetCirclePoint(CenterX, CenterY, Radius, StartAngle + SweepAngle);
                if (p.X < x1) x1 = p.X;
                if (p.X > x2) x2 = p.X;
                if (p.Y < y1) y1 = p.Y;
                if (p.Y > y2) y2 = p.Y;


                if (SweepAngle >= 0)
                    for (double a = StartAngle + 10 ; a < StartAngle + SweepAngle; a += 10)
                    {
                        p = Geometry.GetCirclePoint(CenterX, CenterY, Radius, a);
                        if (p.X < x1) x1 = p.X;
                        if (p.X > x2) x2 = p.X;
                        if (p.Y < y1) y1 = p.Y;
                        if (p.Y > y2) y2 = p.Y;
                    }
                else
                    for (double a = StartAngle - 10; a > StartAngle + SweepAngle; a -= 10)
                    {
                        p = Geometry.GetCirclePoint(CenterX, CenterY, Radius, a);
                        if (p.X < x1) x1 = p.X;
                        if (p.X > x2) x2 = p.X;
                        if (p.Y < y1) y1 = p.Y;
                        if (p.Y > y2) y2 = p.Y;
                    }

                _Boundary = RectangleF.FromLTRB((float)x1, (float)y1, (float)x2, (float)y2);
            }
            
            internal override void ReadBinary(BinaryReader reader)
            {
                base.ReadBinary(reader);

                _Radius = reader.ReadSingle();
                _StartAngle = reader.ReadSingle();
                _SweepAngle = reader.ReadSingle();

                UpdateCtrlPoints();
            }
            internal override void WriteBinary(BinaryWriter writer)
            {
                base.WriteBinary(writer);

                writer.Write(Radius);
                writer.Write(StartAngle);
                writer.Write(SweepAngle);
            }

            public override void Draw(Graphics aGraphic, DrawingInfo aInfo)
            {
                PreDraw(aGraphic, aInfo);

                float x1, y1, x2, y2;
                float wh = (float)(_Radius * 2 * aInfo.PixelPerUnit);

                aInfo.ToWin(CenterX - Radius, CenterY - Radius, out x1, out y1);
                aInfo.ToWin(CenterX + Radius, CenterY + Radius, out x2, out y2);

                if (wh <= 1.0)
                    aGraphic.DrawLine(aInfo.Pen, x1, y1, x2, y2);
                else
                    // invert Y -> angle changed
                    aGraphic.DrawArc(aInfo.Pen, Math.Min(x1, x2), Math.Min(y1, y2), wh, wh, -(float)StartAngle, -(float)SweepAngle);
            }
            public override void InvertDir()
            {
                StartAngle += SweepAngle;
                SweepAngle = -SweepAngle;
            }

            public bool IsCCW() { return SweepAngle >= 0; }
        }

        public class Spline : Shape
        {
            public Spline()
                : base(1)
            {
            }
            public Spline(double x, double y)
                : base(1)
            {
                AddPoint(0, x, y);
            }
            public Spline(PointD pt)
                : base(1)
            {
                AddPoint(0, pt);
            }
            public Spline(PointD[] pts)
                : base(1)
            {
                AddPoint(0, pts);
            }
        }

        public class Curve : Shape
        {
            internal const int SEGMENT_COUNT = 32;

            public Curve(int nContourCount = 1)
                : base(nContourCount)
            {
            }
            protected virtual void UpdateContour()
            {
            }
        }     

        public class Rectangle : Shape
        {
            public Rectangle(float l, float t, float r, float b)
                : base(1)
            {
            }
        }
    }
}