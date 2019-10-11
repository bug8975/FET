using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Monitor_HCCS.Common;
using ToastNotifications;

namespace Monitor_HCCS.View
{
    public partial class PictureForm : DevExpress.XtraEditors.XtraForm
    {

        //定义委托调用Toast
        delegate void ShowToastCallback_InPicture(string s1, string s2, int time);

        #region 属性 && 构造器
        public string PictureName { get; set; }
        public string InfoSite { get; set; }
        public int PointNum { get; set; }
        public float Rate { get; set; }
        public float Origin { get; set; }

        public PictureForm()
        {
            InitializeComponent();
        }
        #endregion

        #region 控件事件
        //加载
        private void PictureForm_Load(object sender, EventArgs e)
        {
            string imageFileName = PictureName + "img.jpg";
            FileStream fs;
            try
            {
                fs = new FileStream(imageFileName, FileMode.Open, FileAccess.Read);
                int byteLength = (int)fs.Length;
                byte[] fileBytes = new byte[byteLength];
                fs.Read(fileBytes, 0, byteLength);
                //文件流关閉,文件解除锁定
                fs.Close();
                pictureBox1.Image = Image.FromStream(new MemoryStream(fileBytes));
            }
            catch (Exception)
            {

            }
            
            
            ucLabel1.UcText = "经度： ";
            ucLabel2.UcText = "纬度： ";
            ucLabel5.UcText = "海拔： ";
            ucLabel3.UcText = "测线名称： " + PictureName;


            Task.Run(new Action(async () =>
            {
                ucLabel4.UcText = "地点： " + await getInfoSite(PictureName);
                PointNum = await getPointCount(PictureName);
                string sql = "select gears from info where name = '" + PictureName + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql, null);
                if (dt.Rows.Count == 0)
                    return;

                string gears = dt.Rows[0].ItemArray[0].ToString();
                if (gears.Contains("wt"))
                    gears = gears.Split(new char[] { 'w', 't', 'a' })[2];
                else if (gears.Contains("g"))
                    gears = gears.Split(new char[] { 'g', 'p' })[1];
                Rate = Percentage.GetPerc(PictureName, PointNum, gears);
                Origin = (1 - Rate) * pictureBox1.Height;
            }));

            Task.Run(new Action(async () =>
            {
                ucLabel6.UcText = "点距： " + await getDistance(PictureName) + "米";
            }));
        }
        //返回
        private void ComeBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //截图
        private void CutScreen_Click(object sender, EventArgs e)
        {
            //截取全屏图像
            string ScreenShotImage = PictureName + "_SSImage.jpg";
            Size size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Bitmap bm = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, size);
            bm.Save(ScreenShotImage, ImageFormat.Jpeg);
            bm.Dispose();
            ShowToast_InPicture("提示", "截图保存成功", 2);
        }
        #endregion

        //获得测线地点
        private async Task<string> getInfoSite(string name)
        {
            try
            {
                string sql = "select site from info where name = '" + name + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql);
                return dt.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return null;
            }
        }

        //获得测线点距
        private async Task<string> getDistance(string name)
        {
            try
            {
                string sql = "select distance from info where name = '" + name + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql);
                return dt.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return null;
            }
        }

        //获得测量的点数
        private async Task<int> getPointCount(string name)
        {
            try
            {
                string sql1 = "select gears from info where name = '" + PictureName + "'";
                DataTable dt1 = SQLiteHelper.ExecuteDataTable(sql1, null);
                if (dt1.Rows.Count == 0)
                    return 0;
                string dbname = dt1.Rows[0].ItemArray[0].ToString();
                string sql2 = "select count(Id) from '" + dbname + "' where info_name = '" + name + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql2);
                return Convert.ToInt32(dt.Rows[0].ItemArray[0]);
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return 0;
            }
        }

        //绘制刻度和数字
        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Pen pen = new Pen(Color.Black, 1);
            Pen TransPen = new Pen(Color.White, 0.3f);
            SolidBrush brush = new SolidBrush(Color.Black);

            StringFormat strF = new StringFormat(StringFormatFlags.DirectionVertical);
            //e.Graphics.TranslateTransform(100.0F, 100.0F);
            //e.Graphics.RotateTransform(270.0F);
            //坐标系数
            //原点
            float ox = pictureBox1.Location.X;
            float oy = pictureBox1.Location.Y + pictureBox1.Height;

            //绘制主坐标轴
            graphics.DrawLine(pen, ox - 1, Origin + 1, ox - 1, pictureBox1.Height);
            graphics.DrawLine(pen, ox, pictureBox1.Height, pictureBox1.Width + ox, pictureBox1.Height);

            //刻度线  &&  数字标签
            int depth = Convert.ToInt32(XmlHelper.getValue(PictureName, "depth"));
            float AXiesLength = pictureBox1.Width;
            float AXiesAverage = AXiesLength / 10;

            float AYiesLength = pictureBox1.Height;
            float AYiesAverage = AYiesLength * Rate / (PointNum - 1);

            //Y轴上上的刻度
            for (int i = 0; i < PointNum; i++)
            {
                graphics.DrawLine(pen, ox, AYiesAverage * i + Origin + 0.5f, ox - 5, AYiesAverage * i + Origin + 0.5f);
                //graphics.DrawString((i + 1).ToString(), new Font("微软雅黑", 10), brush, 25f, AYiesLength - AYiesAverage * i + 10, new StringFormat(StringFormatFlags.DirectionRightToLeft));
                //graphics.DrawString((i + 1).ToString(), new Font("宋体", 12), brush, 5, AYiesLength - AYiesAverage * i, strF);
            }

            //X轴上的刻度
            for (int i = 0; i < 11; i++)
            {
                graphics.DrawLine(pen, ox + AXiesAverage * i, AYiesLength, ox + AXiesAverage * i, AYiesLength + 5);
                //graphics.DrawString("-" + (depth / 10 * (i+1)), new Font("微软雅黑", 14), brush, 5f + AXiesAverage * (i+1), AYiesLength + 5);
                //graphics.DrawString((depth / 10 * i).ToString(), new Font("宋体", 10), brush, 15f + AXiesAverage * i, AYiesLength + 5, strF);
            }




            Matrix matrix = new Matrix();
            matrix.RotateAt(270, new PointF(ox, oy));
            graphics.Transform = matrix;
            //Y轴上的数字
            for (int i = 0; i < PointNum; i++)
            {
                if (PointNum > 30  && PointNum <= 60)
                {
                    if ((i % 2) == 1)
                        continue;
                }

                if (PointNum > 60 && PointNum <= 110)
                {
                    if ((i % 5) != 0)
                        continue;
                }

                if (PointNum > 110)
                {
                    if ((i % 10) != 0)
                        continue;
                }

                if (PointNum > 200)
                {
                    if ((i % 20) != 0)
                        continue;
                }

                graphics.DrawString((i + 1).ToString(), new Font("微软雅黑", 16), brush, ox - 30f + AYiesAverage * i, oy - 35f);
            }

            //X轴上的数字
            for (int i = 0; i < 11; i++)
            {
                if(i == 0)
                {
                    graphics.DrawString("" + (depth / 10 * i), new Font("微软雅黑", 16), brush, ox - 55f, oy + AXiesAverage * i);
                    continue;
                }
                    
                graphics.DrawString("-" + (depth / 10 * i), new Font("微软雅黑", 16), brush, ox - 65f, oy + AXiesAverage * i);
            }

            Matrix matrix2 = new Matrix();
            matrix2.RotateAt(0, new PointF(ox,oy));
            graphics.Transform = matrix2;
        }

        //绘制图片分割白线
        private void pictureEdit1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Pen TransPen = new Pen(Color.White, 0.001f);

            float AXiesLength = pictureBox1.Width;
            float AXiesAverage = AXiesLength / 10;

            float AYiesLength = pictureBox1.Height;
            float AYiesAverage = AYiesLength * Rate / (PointNum - 1);

            //Y轴上的分割线
            for (int i = 0; i < PointNum; i++)
            {
                graphics.DrawLine(TransPen, 0f, AYiesAverage * (i + 1) + Origin, AXiesLength, AYiesAverage * (i + 1) + Origin);
            }

            //X轴上的分割线
            for (int i = 0; i < 11; i++)
            {
                graphics.DrawLine(TransPen, AXiesAverage * (i + 1), AYiesLength, AXiesAverage * (i + 1), Origin);
            }

            //用label遮住上部空白
            labelControl1.Height = Convert.ToInt32(Origin);
        }

        //Toast服务
        public void ShowToast_InPicture(string title, string body, int time)
        {
            if (this.InvokeRequired)
            {
                while (this.IsHandleCreated == false)
                {
                    if (this.Disposing || this.IsDisposed)
                        return;
                }
                ShowToastCallback_InPicture st = new ShowToastCallback_InPicture(ShowToast_InPicture);
                this.BeginInvoke(st, new object[] { title, body, time });
            }
            else
            {
                new Notification(title, body, time, ToastNotifications.FormAnimator.AnimationMethod.Slide, ToastNotifications.FormAnimator.AnimationDirection.Up).Show();
            }
        }
        #region 闪屏
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        #endregion
    }
}
