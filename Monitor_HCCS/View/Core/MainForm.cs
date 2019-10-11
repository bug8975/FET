using System;
using System.Windows.Forms;
using System.Threading;
using Monitor_HCCS.Common;
using Monitor_HCCS.View.Dialog;
using System.IO;
using System.Runtime.InteropServices;
using BenNHControl;
using DevExpress.XtraSplashScreen;
using Monitor_HCCS.View.Core;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Generic;

namespace Monitor_HCCS.View
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {

        public object lockObj = new object();
        public bool formSwitchFlag = false;
        public string DestPath { get; set; }

        //定义一个用于保存静态变量的实例
        private static MainForm instance = null;
        //定义一个保证线程同步的标识
        private static readonly object locker = new object();
        public static MainForm GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new MainForm();
                }
                return instance;
            }
            return instance;
        }
        public HomeForm homeForm = null;
        public DataForm dataForm = null;
        public USB usb = null;
        public Delete delete = null;
        public SetupForm setupForm = null;
        //当前窗体
        private System.Windows.Forms.Form currentForm;

        private MainForm()
        {
            InitializeComponent();

            this.IsMdiContainer = true;

            //实例化子窗体
            homeForm = HomeForm.GetInstance();
            dataForm = DataForm.GetInstance();
            usb = USB.GetInstance();
            delete = Delete.GetInstance();
            setupForm = SetupForm.GetInstance();

            homeForm.Click_InMain += simpleButton1_Click;
            dataForm.Click_InData += simpleButton2_Click;

            #region 添加子窗体
            homeForm.MdiParent = this;
            dataForm.MdiParent = this;
            usb.MdiParent = this;
            delete.MdiParent = this;
            setupForm.MdiParent = this;

            //pnlCenter.BeginInit();
            //pnlCenter.Controls.Add(homeForm);
            //pnlCenter.Controls.Add(dataForm);
            //pnlCenter.Controls.Add(usb);
            //pnlCenter.Controls.Add(delete);
            //pnlCenter.Controls.Add(setupForm);
            //pnlCenter.EndInit();
            #endregion

            ////计时服务
            System.Timers.Timer clock = new System.Timers.Timer(1000);
            clock.Elapsed += Timer_Elapsed;
            clock.AutoReset = true;
            clock.Enabled = true;
        }

        #region 控件初始化
        /// <summary>
        /// 解决闪烁问题
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        /// <summary>
        /// 按钮初始化
        /// </summary>
        /// <returns></returns>
        private bool initButton()
        {
            try
            {
                watch15.Start();
                this.simpleButton1.BackgroundImage = global::Monitor_HCCS.Properties.Resources.InsertPicture;
                this.simpleButton2.BackgroundImage = global::Monitor_HCCS.Properties.Resources.LinePicture;
                this.simpleButton3.BackgroundImage = global::Monitor_HCCS.Properties.Resources.DeletePicture;
                this.simpleButton4.BackgroundImage = global::Monitor_HCCS.Properties.Resources.OutputPicture;
                this.simpleButton5.BackgroundImage = global::Monitor_HCCS.Properties.Resources.SetupPicture;
                watch15.Stop();
                Console.WriteLine("\r\n1.1所有按钮的图片初始化耗时：" + watch15.Elapsed.TotalMilliseconds);
                watch15.Reset();
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="frm"></param>
        public void ShowForm(DevExpress.XtraEditors.PanelControl panel, System.Windows.Forms.Form frm)
        {
            try
            {
                watch10.Start();

                watch11.Start();                
                if (this.currentForm != null)
                    if (this.currentForm == frm)
                        return;

                if (this.ActiveMdiChild != null)
                    this.ActiveMdiChild.Hide();

                this.currentForm = frm;
                watch11.Stop();

                
                watch12.Start();
                frm.TopLevel = false;
                //frm.MdiParent = this;
                watch12.Stop();

                watch13.Start();
                panel.BeginInit();
                panel.Controls.Clear();
                watch13.Stop();

                watch14.Start();
                panel.Controls.Add(frm);            
                frm.Dock = System.Windows.Forms.DockStyle.Fill;
                //panel.EndInit();
                watch14.Stop();

                watch10.Stop();
                Console.WriteLine("2.1.1、隐藏子页面耗时：" + watch11.Elapsed.TotalMilliseconds);
                Console.WriteLine("2.1.2、子页面设置panel为父容器耗时：" + watch12.Elapsed.TotalMilliseconds);
                Console.WriteLine("2.1.3、panel清除子控件耗时：" + watch13.Elapsed.TotalMilliseconds);
                Console.WriteLine("2.1.4、panel添加子页面和子页面布局耗时：" + watch14.Elapsed.TotalMilliseconds);

                //Console.WriteLine("2.1主窗体切换子页面耗时：" + watch10.Elapsed.TotalMilliseconds);
                watch10.Reset();
                watch11.Reset();
                watch12.Reset();
                watch13.Reset();
                watch14.Reset();
            }
            catch (System.Exception ex)
            {
                Log4netHelper.Error(ex);
            }
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action<object>(delegate(object arg)
                    {
                        frm.Show();
                        //frm.Focus();
                    }), new object[] { null });
                }
                else
                {
                    frm.Show();
                    //frm.Focus();
                }
            });

            
        }
        #endregion

        //测量界面
        private void simpleButton1_Click(object sender, EventArgs e)
        {

            try
            {
                watch9.Start();
                watch1.Start();
                this.initButton();
                this.simpleButton1.BackgroundImage = global::Monitor_HCCS.Properties.Resources.Insert3D;
                watch1.Stop();
                formSwitchFlag = true;
                //dataForm.ShowMessage();     //显示等待窗体
                watch8.Start();
                this.ShowForm(pnlCenter, dataForm);
                watch8.Stop();

                //dataForm.HideMessage();     //关闭等待窗体
                formSwitchFlag = false;
                watch7.Start();
                dataForm.ReFlush();
                watch7.Stop();
                watch9.Stop();
                Console.WriteLine("1.切换图片运行时间：" + watch1.Elapsed.TotalMilliseconds);
                Console.WriteLine("2.测量界面show方法运行时间：" + watch8.Elapsed.TotalMilliseconds);
                Console.WriteLine("3.测量界面ReFlush方法运行时间：" + watch7.Elapsed.TotalMilliseconds);
                Console.WriteLine("总测量界面整体运行时间：" + watch9.Elapsed.TotalMilliseconds);
                //builder.Append("测量界面整体运行时间：" + watch9.Elapsed.TotalMilliseconds + "\r\n");
                string s = "****************测量界面整体运行时间：" + watch9.Elapsed.TotalMilliseconds + "***************";

                watch1.Reset();
                watch9.Reset();
                watch7.Reset();
                watch8.Reset();



            }
            catch (System.Exception ex)
            {
                Log4netHelper.Error(ex);
            }
            finally
            {
                //Monitor.Exit(this.lockObj);
            }

            
        }
        //测线界面
        private void simpleButton2_Click(object sender, EventArgs e)
        {

            try
            {
                watch3.Start();
                watch4.Start();
                this.initButton();
                this.simpleButton2.BackgroundImage = global::Monitor_HCCS.Properties.Resources.Line3D;
                watch4.Stop();
                formSwitchFlag = true;
                watch1.Start();
                this.ShowForm(pnlCenter, homeForm);
                watch1.Stop();
                watch2.Start();
                homeForm.ReFlush();
                watch2.Stop();
                watch3.Stop();
                Console.WriteLine("1.切换图片运行时间：" + watch4.Elapsed.TotalMilliseconds);
                Console.WriteLine("2.测线界面show方法运行时间：" + watch1.Elapsed.TotalMilliseconds);
                Console.WriteLine("3.测线界面ReFlush方法运行时间：" + watch2.Elapsed.TotalMilliseconds);
                Console.WriteLine("总测线界面整体运行时间：" + watch3.Elapsed.TotalMilliseconds);
                //builder.Append("测线界面show方法运行时间：" + watch1.Elapsed.TotalMilliseconds + "\r\n").Append("测线界面ReFlush方法运行时间：" + watch2.Elapsed.TotalMilliseconds + "\r\n");
                string s = "测线界面整体运行时间：" + watch3.Elapsed.TotalMilliseconds;

                watch4.Reset();
                watch1.Reset();
                watch2.Reset();
                watch3.Reset();
                formSwitchFlag = false;



            }
            catch (System.Exception ex)
            {
                Log4netHelper.Error(ex);
            }
            finally
            {
                //Monitor.Exit(this.lockObj);
            }

            
        }
        //删除界面
        private void simpleButton3_Click(object sender, EventArgs e)
        {

            try
            {
                watch5.Start();
                watch1.Start();
                this.initButton();
                this.simpleButton3.BackgroundImage = global::Monitor_HCCS.Properties.Resources.Delete3D;
                watch1.Stop();
                formSwitchFlag = true;
                watch3.Start();
                this.ShowForm(pnlCenter, delete);
                watch3.Stop();
                formSwitchFlag = false;

                watch4.Start();
                delete.ReFlush();
                watch4.Stop();
                watch5.Stop();
                Console.WriteLine("1.切换图片运行时间：" + watch1.Elapsed.TotalMilliseconds);
                Console.WriteLine("2.删除界面show方法运行时间：" + watch3.Elapsed.TotalMilliseconds);
                Console.WriteLine("3.删除界面ReFlush方法运行时间：" + watch4.Elapsed.TotalMilliseconds);
                Console.WriteLine("总删除界面整体运行时间：" + watch5.Elapsed.TotalMilliseconds);
                //builder.Append("删除界面show方法运行时间：" + watch3.Elapsed.TotalMilliseconds + "\r\n").Append("删除界面ReFlush方法运行时间：" + watch4.Elapsed.TotalMilliseconds + "\r\n");
                string s = "删除界面整体运行时间：" + watch5.Elapsed.TotalMilliseconds;

                watch1.Reset();
                watch3.Reset();
                watch4.Reset();
                watch5.Reset();



            }
            catch (System.Exception ex)
            {
                Log4netHelper.Error(ex);
            }
            finally
            {
                //Monitor.Exit(this.lockObj);
            }

            
        }
        //导出界面
        private void simpleButton4_Click(object sender, EventArgs e)
        {

            try
            {
                watch7.Start();
                watch1.Start();
                this.initButton();
                this.simpleButton4.BackgroundImage = global::Monitor_HCCS.Properties.Resources.Output3D;
                watch1.Stop();
                formSwitchFlag = true;
                watch5.Start();
                this.ShowForm(pnlCenter, usb);
                watch5.Stop();

                formSwitchFlag = false;
                watch6.Start();
                usb.ReFlush();
                watch6.Stop();
                watch7.Stop();
                Console.WriteLine("1.切换图片运行时间：" + watch1.Elapsed.TotalMilliseconds);
                Console.WriteLine("2.导出界面show方法运行时间：" + watch5.Elapsed.TotalMilliseconds);
                Console.WriteLine("3.导出界面ReFlush方法运行时间：" + watch6.Elapsed.TotalMilliseconds);
                Console.WriteLine("总导出界面整体运行时间：" + watch7.Elapsed.TotalMilliseconds);
                //builder.Append("导出界面show方法运行时间：" + watch5.Elapsed.TotalMilliseconds + "\r\n").Append("导出界面ReFlush方法运行时间：" + watch6.Elapsed.TotalMilliseconds + "\r\n");
                string s = "导出界面整体运行时间：" + watch7.Elapsed.TotalMilliseconds;

                watch1.Reset();
                watch5.Reset();
                watch6.Reset();
                watch7.Reset();

            }
            catch (System.Exception ex)
            {
                Log4netHelper.Error(ex);
            }
            finally
            {
                //Monitor.Exit(this.lockObj);
            }

            
        }
        //设置界面
        private void simpleButton5_Click(object sender, EventArgs e)
        {

            try
            {
                watch9.Start();
                watch1.Start();
                this.initButton();
                this.simpleButton5.BackgroundImage = global::Monitor_HCCS.Properties.Resources.Setup3D;
                watch1.Stop();
                formSwitchFlag = true;
                watch7.Start();
                this.ShowForm(pnlCenter, setupForm);
                watch7.Stop();
                
                formSwitchFlag = false;
                watch8.Start();
                setupForm.ReFlush();
                watch8.Stop();
                watch9.Stop();
                Console.WriteLine("1.切换图片运行时间：" + watch1.Elapsed.TotalMilliseconds);
                Console.WriteLine("2.设置界面show方法运行时间：" + watch7.Elapsed.TotalMilliseconds);
                Console.WriteLine("3.设置界面ReFlush方法运行时间：" + watch8.Elapsed.TotalMilliseconds);
                Console.WriteLine("总设置界面整体运行时间：" + watch9.Elapsed.TotalMilliseconds);
                //builder.Append("设置界面show方法运行时间：" + watch7.Elapsed.TotalMilliseconds + "\r\n").Append("设置界面ReFlush方法运行时间：" + watch8.Elapsed.TotalMilliseconds + "\r\n");
                string s = "设置界面整体运行时间：" + watch9.Elapsed.TotalMilliseconds + "\r\n";

                watch1.Reset();
                watch7.Reset();
                watch8.Reset();
                watch9.Reset();


            }
            catch (System.Exception ex)
            {
                Log4netHelper.Error(ex);
            }
            finally
            {
                //Monitor.Exit(this.lockObj);
            }

            
        }

        //Toast服务
        public void ShowToast(string title, string body, int time)
        {
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() => { new ToastNotifications.Notification(title, body, time, ToastNotifications.FormAnimator.AnimationMethod.Slide, ToastNotifications.FormAnimator.AnimationDirection.Up).Show(); }));
        }
        //系统信息： 电量 && 时间
        private void Timer_Elapsed(object sender, EventArgs e)
        {
            if (this.IsHandleCreated)
            {
                //系统时间
                Action<string> action = (x) => { this.labelControl1.Text = x.ToString(); };
                this.labelControl1.Invoke(action, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //系统电量
                Action<int> action2 = (x) => { this.progressBarControl2.Position = x; };
                this.progressBarControl2.Invoke(action2, Convert.ToInt32(GetSystemPower()));
            }

        }

        #region 系统电量
        [DllImport("kernel32.dll", EntryPoint = "GetSystemPowerStatus")]   //win32 api
        private static extern void GetSystemPowerStatus(ref SYSTEM_POWER_STATUS lpSystemPowerStatus);

        public struct SYSTEM_POWER_STATUS    //结构体
        {
            public Byte ACLineStatus;                //0 = offline, 1 = Online, 255 = UnKnown Status.   
            public Byte BatteryFlag;
            public Byte BatteryLifePercent;
            public Byte Reserved1;
            public int BatteryLifeTime;
            public int BatteryFullLifeTime;
        }

        /// <summary>
        ///  获取系统电量百分比
        /// </summary>
        /// <returns></returns>
        public static float GetSystemPower()
        {
            SYSTEM_POWER_STATUS SysPower = new SYSTEM_POWER_STATUS();
            GetSystemPowerStatus(ref SysPower);

            return SysPower.BatteryLifePercent;
        }
        #endregion

        #region  USB系统消息
        public const int WM_DEVICECHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;
        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CUSTOMEVENT = 0x8006;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public const int DBT_QUERYCHANGECONFIG = 0x0017;
        public const int DBT_USERDEFINED = 0xFFFF;

        //系统消息处理机制
        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WM_DEVICECHANGE)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case WM_DEVICECHANGE:
                            break;
                        case DBT_DEVICEARRIVAL://U盘插入   
                            DriveInfo[] s = DriveInfo.GetDrives();
                            foreach (DriveInfo drive in s)
                            {
                                if (drive.DriveType == DriveType.Removable)
                                {
                                    DestPath = drive.Name.ToString();
                                    if (usb != null)
                                        usb.DestPath = DestPath + "FET";
                                    ShowToast("提示", "U盘已连接", 1);
                                    break;
                                }
                            }
                            break;
                        case DBT_CONFIGCHANGECANCELED:
                            break;
                        case DBT_CONFIGCHANGED:
                            break;
                        case DBT_CUSTOMEVENT:
                            break;
                        case DBT_DEVICEQUERYREMOVE:
                            break;
                        case DBT_DEVICEQUERYREMOVEFAILED:
                            break;
                        case DBT_DEVICEREMOVECOMPLETE:   //U盘卸载 
                            DestPath = "";
                            ShowToast("提示", "U盘已移除", 1);
                            break;
                        case DBT_DEVICEREMOVEPENDING:
                            break;
                        case DBT_DEVICETYPESPECIFIC:
                            break;
                        case DBT_DEVNODES_CHANGED:
                            break;
                        case DBT_QUERYCHANGECONFIG:
                            break;
                        case DBT_USERDEFINED:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowToast("警告", ex.Message, 2);
                Log4netHelper.Error(ex);
            }
            base.WndProc(ref m);
        }
        #endregion

        //系统关机
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBoxEX.Show("提示", "确认立即关机吗？");
            if (dr == DialogResult.OK)
            {
                System.Diagnostics.Process.Start("cmd.exe", "/cshutdown -s -t 0");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //关闭闪屏窗体
            SplashScreenManager.CloseForm(true);
        }

        //载入测量界面
        private void MainForm_Shown(object sender, EventArgs e)
        {
            //ShowMessage();      //显示等待窗体
            //载入测量界面
            simpleButton2_Click(this, null);
            //HideMessage();      //关闭等待窗体
        }

        #region 闪屏  &&  等待窗体
        private SplashScreenManager _loadForm;
        /// <summary>
        /// 等待窗体管理对象
        /// </summary>
        protected SplashScreenManager LoadForm
        {
            get
            {
                if (_loadForm == null)
                {
                    this._loadForm = new SplashScreenManager(this, typeof(WaitForm1), true, true);
                    this._loadForm.ClosingDelay = 0;
                }
                return _loadForm;
            }
        }
        /// <summary>
        /// 显示等待窗体
        /// </summary>
        public void ShowMessage()
        {
            bool flag = !this.LoadForm.IsSplashFormVisible;
            if (flag)
            {
                this.LoadForm.ShowWaitForm();
            }
        }
        /// <summary>
        /// 关闭等待窗体
        /// </summary>
        public void HideMessage()
        {
            bool isSplashFormVisible = this.LoadForm.IsSplashFormVisible;
            if (isSplashFormVisible)
            {
                this.LoadForm.CloseWaitForm();
            }
        }

        #endregion

        #region 运行时间监控
        System.Diagnostics.Stopwatch watch1 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch2 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch3 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch4 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch5 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch6 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch7 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch8 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch9 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch10 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch11 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch12 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch13 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch14 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch15 = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch watch16 = new System.Diagnostics.Stopwatch();

        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        public static void writefile(string lines)
        {
            using (FileStream fs = File.Open(@"C:\图片切换耗时.txt", FileMode.Append, FileAccess.Write, FileShare.Write))
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fs))
                {
                    file.WriteLine(lines);// 直接追加文件末尾，换行 
                }
            }


        }
        #endregion

    }
}