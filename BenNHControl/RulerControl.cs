using BenNHControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BenNHControl
{
    public partial class RulerControl : UserControl
    {
        private double _monitorDPI = 100;
        /// <summary>
        /// 用于控制放大倍数
        /// </summary>
        private double _multiple = 1;

        /// <summary>
        /// 每多少个像素1格
        /// </summary>
        public double MonitorDPI
        {
            get
            {
                return _monitorDPI;
            }
            set
            {
                _monitorDPI = value;
            }
        }
        /// <summary>
        /// X轴偏移位置
        /// </summary>
        private float offSetX = 0;

        /// <summary>
        /// Y轴偏移位置
        /// </summary>
        private float offSetY = 0;

        /// <summary>
        /// 开始位置X
        /// </summary>
        public double XStart
        {
            get;
            set;
        }

        /// <summary>
        /// 开始位置Y
        /// </summary>
        public double YStart
        {
            get;
            set;
        }

        /// <summary>
        /// 用户设置的原始图
        /// </summary>
        private Image _initImg = null;



        /// <summary>
        /// 图片
        /// </summary>
        public Image Image
        {
            get
            {
                return Pic.Image;
            }
            set
            {
                Pic.Image = value;
                if (_initImg == null && value != null)
                {
                    _initImg = PicHelper.GetNewPic(value, value.Width, value.Height);
                }
            }
        }
        private Font font = new Font("宋体", 9); //刻度值显示字体
        public RulerControl()
        {
            InitializeComponent();
        }

        private void Pic_MouseWheel(object sender, MouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                return;
            }
            Image img = Pic.Image;
            if (img != null)
            {
                if (e.Delta >= 0)
                {
                    if (_multiple * 2 > 4)
                        return;
                    _multiple *= 2;
                }
                else
                {
                    if (Pic.Width < this.Width - 50)
                        return;

                    _multiple *= 0.5;
                    if (Pic.Width <= (this.Width - 50))
                    {
                        XStart = 0;
                        YStart = 0;
                    }
                }
                DrawRect();
                AdapterDpi();
                ReDrawX();
                ReDrawY();
            }
        }

        private void RulerControl_Paint(object sender, PaintEventArgs e)
        {
            P2.Height = 50;
            P2.Width = this.Width - 50;
            P1.Height = this.Height - 50;
            P1.Width = 50;
            P2.Location = new Point(50, 0);
            P1.Location = new Point(0, 50);
            PContainer.Location = new Point(50, 50);
        }

        private void RulerControl_Resize(object sender, EventArgs e)
        {
            P2.Height = 50;
            P2.Width = this.Width - 50;
            P1.Height = this.Height - 50;
            P1.Width = 50;
            P2.Location = new Point(50, 0);
            P1.Location = new Point(0, 50);
            PContainer.Location = new Point(50, 50);
        }

        /// <summary>
        /// 重画Y轴
        /// </summary>
        private void ReDrawY()
        {
            if (P1.BackgroundImage != null)
            {
                P1.BackgroundImage.Dispose();
            }
            Bitmap bmpY = new Bitmap(P1.Width, P1.Height);
            using (Graphics g = Graphics.FromImage(bmpY))
            {
                int originLocation = bmpY.Width - 1;
                int startY = (int)Math.Ceiling(YStart);
                offSetY = (float)(MonitorDPI * _multiple * (startY - YStart));
                for (int i = startY; i <= Math.Ceiling(P1.Height / (MonitorDPI * _multiple) + YStart); i++)
                {
                    float y = (float)(MonitorDPI * _multiple * (i - YStart)) + offSetY;
                    if (y >= 0)
                    {
                        PointF start = new PointF(originLocation, y);
                        PointF end = new PointF(originLocation - 3, y);
                        if (i % 5 == 0)
                        {
                            end = new PointF(originLocation - 6, y);
                        }
                        if (i % 10 == 0 && i != 0)
                        {
                            end = new PointF(originLocation - 12, y);
                            g.DrawString((i * MonitorDPI).ToString(), font, Brushes.Black, new PointF(originLocation - 30, y - 5));
                        }
                        g.DrawLine(Pens.Black, start, end);
                    }
                }
                g.DrawLine(Pens.Black, new PointF(originLocation, 0), new PointF(originLocation, bmpY.Height));
                P1.BackgroundImage = bmpY;
            }
        }

        /// <summary>
        /// 重画X轴
        /// </summary>
        private void ReDrawX()
        {
            if (P2.BackgroundImage != null)
            {
                P2.BackgroundImage.Dispose();
            }
            Bitmap bmpX = new Bitmap(P2.Width, P2.Height);
            using (Graphics g = Graphics.FromImage(bmpX))
            {
                int originLocation = bmpX.Height - 1;
                int startX = (int)Math.Ceiling(XStart);
                offSetX = (float)(MonitorDPI * _multiple * (startX - XStart));
                for (int i = startX; i <= Math.Ceiling(P2.Width / (MonitorDPI * _multiple) + XStart); i++)
                {
                    float x = (float)(MonitorDPI * _multiple * (i - XStart)) + offSetX;
                    if (x >= 0)
                    {
                        PointF start = new PointF(x, originLocation);
                        PointF end = new PointF(x, originLocation - 3);
                        if (i % 5 == 0)
                        {
                            end = new PointF(x, originLocation - 6);
                        }
                        if (i % 10 == 0 && i != 0)
                        {
                            end = new PointF(x, originLocation - 12);
                            g.DrawString((i * MonitorDPI).ToString(), font, Brushes.Black, new PointF(x - 5, originLocation - 30));
                        }
                        g.DrawLine(Pens.Black, start, end);
                    }
                }
                g.DrawLine(Pens.Black, new PointF(0, originLocation), new PointF(bmpX.Width, originLocation));
                P2.BackgroundImage = bmpX;
            }
        }

        private void RulerControl_Load(object sender, EventArgs e)
        {
            P2.Height = 50;
            P2.Width = this.Width - 50;
            P1.Height = this.Height - 50;
            P1.Width = 50;
            P2.Location = new Point(50, 0);
            P1.Location = new Point(0, 50);
            PContainer.Location = new Point(50, 50);
            ReDrawX();
            ReDrawY();
            Pic.MouseWheel += new MouseEventHandler(Pic_MouseWheel);
        }

        private void Pic_MouseEnter(object sender, EventArgs e)
        {
            Pic.Focus();
        }

        private void PContainer_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                XStart = e.NewValue / (MonitorDPI * _multiple);
                ReDrawX();
            }
            else if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                YStart = e.NewValue / (MonitorDPI * _multiple);
                ReDrawY();
            }
        }


        #region 画图片选定区域
        bool MouseIsDown = false;
        Rectangle MouseRect = Rectangle.Empty;

        private void Pic_MouseDown(object sender, MouseEventArgs e)
        {
            MouseIsDown = true;
            MouseRect = new Rectangle(e.X, e.Y, 0, 0);
            Rectangle rec = new Rectangle(0, 0,
                Math.Min(PContainer.ClientRectangle.Width, Pic.ClientRectangle.Width),
                Math.Min(PContainer.ClientRectangle.Height, Pic.ClientRectangle.Height));
            Cursor.Clip = PContainer.RectangleToScreen(rec);
        }

        private void Pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseIsDown)
            {
                MouseRect.Width = e.X - MouseRect.Left;
                MouseRect.Height = e.Y - MouseRect.Top;
                if (MouseRect.Width > 0 && MouseRect.Height > 0)
                {
                    Rectangle rect = Pic.RectangleToScreen(MouseRect);
                    ControlPaint.DrawReversibleFrame(rect, Color.Black, FrameStyle.Dashed);
                }
                Pic.Refresh();
            }

        }

        private void Pic_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseIsDown)
            {
                MouseRect.Width = e.X - MouseRect.Left;
                MouseRect.Height = e.Y - MouseRect.Top;
                using (Graphics g = Graphics.FromImage(Pic.Image))
                {
                    g.DrawRectangle(new Pen(Color.Red), MouseRect);
                }
                Pic.Refresh();
                MouseIsDown = false;
                if ((int)(MouseRect.Width * _multiple) > 0 && (int)(MouseRect.Height * _multiple) > 0)
                {
                    list.Add(new Rectangle((int)(MouseRect.X / _multiple),
                        (int)(MouseRect.Y / _multiple),
                        (int)(MouseRect.Width / _multiple),
                        (int)(MouseRect.Height / _multiple)
                        ));
                }
                MouseRect = Rectangle.Empty;
                Cursor.Clip = Rectangle.Empty;
            }
        }
        #endregion
        public List<Rectangle> list = new List<Rectangle>();

        private void Pic_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs ev = e as MouseEventArgs;
            List<Rectangle> temp = new List<Rectangle>();
            foreach (Rectangle item in list)
            {
                if (ev.X > item.X * _multiple && ev.X < item.X * _multiple + item.Width * _multiple &&
                    ev.Y > item.Y * _multiple && ev.Y < item.Y * _multiple + item.Height * _multiple)
                {
                    temp.Add(item);
                }
            }
            foreach (Rectangle item in temp)
            {
                list.Remove(item);
            }

            DrawRect();
        }

        /// <summary>
        /// 把list中的框画到图片中
        /// </summary>
        private void DrawRect()
        {
            if (Pic.Image != _initImg)
                Pic.Image.Dispose();
            Pic.Image = PicHelper.GetNewPic(_initImg, (int)(_initImg.Width * _multiple), (int)(_initImg.Height * _multiple));
            using (Graphics g = Graphics.FromImage(Image))
            {
                foreach (Rectangle item in list)
                {
                    g.DrawRectangle(new Pen(Color.Red), new Rectangle((int)(item.X * _multiple),
                        (int)(item.Y * _multiple),
                        (int)(item.Width * _multiple),
                        (int)(item.Height * _multiple)
                        ));
                }
            }
            Pic.Refresh();
        }

        private void AdapterDpi()
        {
            if (MonitorDPI * _multiple < 10)
            {
                MonitorDPI *= 2;
            }
            if (MonitorDPI * _multiple > 20)
            {
                MonitorDPI /= 2;
            }
        }



    }
}
