using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using j2k;
using j2k.Shapes;

namespace j2kTestShapes
{
    public partial class Form1 : Form, IMessageFilter 
    {
        Random _Rand;

        public Form1()
        {
            InitializeComponent();

            // http://stackoverflow.com/questions/7852824/usercontrol-how-to-add-mousewheel-listener
            // http://www.hanbit.co.kr/network/view.html?bi_id=333 
            Application.AddMessageFilter(this);

            comboBox1.SelectedIndex = 3;

            _Rand = new Random(DateTime.Now.Second);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x20a)
            {
                // WM_MOUSEWHEEL, find the control at screen position m.LParam
                System.Drawing.Point pos = new System.Drawing.Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                IntPtr hWnd = WinApi.WindowFromPoint(pos);
                if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
                {
                    WinApi.SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                    return true;
                }
            }
            return false;
        }

        private Color RandColor()
        {
            unchecked
            {
                if (chkAlphaBlending.Checked)
                    return Color.FromArgb(_Rand.Next(int.MaxValue));
                else
                    return Color.FromArgb(_Rand.Next(0x1000000) | (int)0xFF000000);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int rnd_w = shapeView1.ClientSize.Width;
            int rnd_h = shapeView1.ClientSize.Height;

            switch (comboBox1.SelectedIndex)
            {
                case 0: // draw R Line
                shapeData1.Add(new Line(50, 200, 50, 50));
                shapeData1.Add(new Line(55, 200, 200, 100));
                shapeData1.Add(new Line(200, 95, 55, 95));
                shapeData1.Add(new Line(55, 90, 200, 50));
                break;

                case 1: // points
                for (int i = 0; i < edtCount.AsInt; i++)
                    {
                        j2k.Shapes.Point p = new j2k.Shapes.Point(_Rand.Next(rnd_w), _Rand.Next(rnd_h));
                        //p.Pen.Color = RandColor();
                        shapeData1.Add(p);
                    }
                    break;

                case 2: // line
                    for (int i = 0; i < edtCount.AsInt; i++)
                    {
                        Line l = new Line(_Rand.Next(rnd_w), _Rand.Next(rnd_h), _Rand.Next(rnd_w), _Rand.Next(rnd_h));
                        //l.Pen.Color = RandColor();
                        shapeData1.Add(l);
                    }
                    break;

                case 3:// polyline
                    Polyline pl = new Polyline();

                    for (int i = 0; i < _Rand.Next(edtCount.AsInt) + 2; i++)
                        pl.AddPoint(0, _Rand.Next(rnd_w), _Rand.Next(rnd_h));

                    //pl.Pen.Color = RandColor();
                    shapeData1.Add(pl);

                    break;

                case 4: // circle
                    for (int i = 0; i < edtCount.AsInt; i++)
                    {
                        Circle cir = new Circle(_Rand.Next(rnd_w), _Rand.Next(rnd_h), _Rand.Next(rnd_h / 3) + 5);
                        //cir.Pen.Color = RandColor();
                        shapeData1.Add(cir);
                    }
                    break;

                case 5: // arc
                    for (int i = 0; i < edtCount.AsInt; i++)
                    {
                        Arc arc = new Arc(_Rand.Next(rnd_w), _Rand.Next(rnd_h), _Rand.Next(rnd_h / 3) + 5, _Rand.Next(360), _Rand.Next(50) + 5);
                        //arc.Pen.Color = RandColor();
                        shapeData1.Add(arc);
                    }
                    break;

                default:
                    return;
            }

            // set property
            foreach (Shape o in shapeData1)
            {
                o.ShowArrow = chkShowArrow.Checked;
                o.Selected = chkSelected.Checked;
            }

            // redraw
            shapeView1.DrawData();
            shapeView1.Invalidate();
        }

        private void chkShowArrow_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Shape o in shapeData1)
            {
                o.ShowArrow = chkShowArrow.Checked;
            }

            shapeView1.DrawData();
            shapeView1.Invalidate();
        }

        private void chkSelected_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Shape o in shapeData1)
                o.Selected = chkSelected.Checked;

            shapeView1.DrawData();
            shapeView1.Invalidate();
        }

        private void btnInvertDir_Click(object sender, EventArgs e)
        {
            foreach (Shape o in shapeData1)
                o.InvertDir();

            shapeView1.DrawData();
            shapeView1.Invalidate();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            shapeData1.Clear();

            shapeView1.DrawData();
            shapeView1.Invalidate();
        }

        private void btnZoomFit_Click(object sender, EventArgs e)
        {
            shapeView1.ZoomFit(30);
        }

        
    }
}
