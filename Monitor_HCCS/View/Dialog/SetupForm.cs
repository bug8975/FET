using BenNHControl;
using Monitor_HCCS.Common;
using Monitor_HCCS.Presenter;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WifiTool;
using System.Runtime.InteropServices;

namespace Monitor_HCCS.View.Dialog
{
    public partial class SetupForm : DevExpress.XtraEditors.XtraForm, ISetupView
    {

        private SetupPresenter _setupPresenter;
        public event EventHandler FirmwareUpdata;
        public event EventHandler SoftwareUpdata;
        public string DestPath { get; set; }
        public string FETDevice { get; set; }
        public XtraForm1 xf;


        //定义一个用于保存静态变量的实例
        private static SetupForm instance = null;
        //定义一个保证线程同步的标识
        private static readonly object locker = new object();
        public static SetupForm GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new SetupForm();
                }
                return instance;
            }
            return instance;
        }
        private SetupForm()
        {
            InitializeComponent();
            _setupPresenter = new SetupPresenter(this);
        }

        //固件升级
        private void txButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!InternetGetConnectedState(out dwFlag, 0))
                {
                    ShowToast_InSetup("提示", "网络未连接，请打开WIFI功能", 2);
                    return;
                }

                if (Monitor_HCCS.Service.BLECode.GetIntance.CurrentService == null)
                {
                    ShowToast_InSetup("提示", "蓝牙未连接", 2);
                    return;
                }
                simpleButton3.Text = "检查更新";
                simpleButton3.Visible = false;
                if (labelControl3.Text.Equals(labelControl4.Text))
                {
                    ShowToast_InSetup("提示", "已经是最新版本！", 2);
                    simpleButton3.Visible = true;
                    simpleButton3.Text = "检查更新";
                    return;
                }
                FirmwareUpdata.Invoke(sender, EventArgs.Empty);

            }
            catch (Exception ex)
            {
                Monitor_HCCS.Common.Log4netHelper.Error(ex);
            }
        }
        //软件升级
        private void txButton3_Click(object sender, EventArgs e)
        {
            if (!InternetGetConnectedState(out dwFlag, 0))
            {
                ShowToast_InSetup("提示", "网络未连接，请打开WIFI功能", 2);
                return;
            }

            SoftwareUpdata.Invoke(sender, EventArgs.Empty);
        }
        //WIFI
        private void WIFI_Click(object sender, EventArgs e)
        {
            XtraForm1 xf = new XtraForm1();
            xf.ShowDialog();
            xf.Dispose();
        }

        //Toast服务
        public void ShowToast_InSetup(string title, string body, int time)
        {
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() => { new ToastNotifications.Notification(title, body, time, ToastNotifications.FormAnimator.AnimationMethod.Slide, ToastNotifications.FormAnimator.AnimationDirection.Up).Show(); }));
        }

        //下载进度条服务
        public void ProgressSer(int quantity)
        {
            this.Invoke(new Action(() =>
            {
                progressBarControl1.Visible = true;
                simpleButton3.Visible = false;
                progressBarControl1.Position = quantity;
            }));
        }

        //固件更新进度条服务
        public void ProgressUp(int quantity)
        {

            if (this.IsHandleCreated)
                this.Invoke(new Action(() =>
                {
                    progressBarControl2.Visible = true;
                    progressBarControl2.Position = quantity;
                }));
        }

        //下载进度条  &&  按钮UI改变服务
        public void ProgressSerChanged()
        {
            if (this.IsHandleCreated)
                this.Invoke(new Action(() =>
                {
                    progressBarControl1.Position = 0;
                    progressBarControl1.Visible = false;
                }));
        }

        //固件更新进度条  &&  按钮UI改变服务
        public void ProgressUpChanged()
        {
            if (this.IsHandleCreated)
                this.Invoke(new Action(() =>
                {
                    progressBarControl2.Position = 0;
                    progressBarControl2.Visible = false;
                    simpleButton3.Visible = true;
                }));
        }

        //更新状态文本服务
        public void SetUpText(string msg)
        {
            if (this.IsHandleCreated)
                this.Invoke(new Action(() => { labelControl4.Text = msg; simpleButton3.Text = "开始升级"; simpleButton3.Visible = true; }));
        }

        //设置版本号等显示内容
        public void SetLabelText(string msg)
        {
            if (this.IsHandleCreated)
                this.Invoke(new Action(() => { labelControl3.Text = msg; simpleButton3.Visible = true; }));
        }

        //固件升级按钮UI状态管理
        public void SetButtonUp(bool b)
        {
            if (this.IsHandleCreated)
                this.Invoke(new Action(() => { simpleButton3.Visible = b; }));
        }

        public void ReFlush()
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action<object>(delegate(object arg)
                    {
                        labelControl2.Text = XmlHelper.getVer("DEVICE");
                    }), new object[] { null });
                }
            });
        }

        #region 检查网络是否连接
        //导入判断网络是否连接的 .dll  
        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        //判断网络状况的方法,返回值true为连接，false为未连接  
        public extern static bool InternetGetConnectedState(out int conState, int reder);
        int dwFlag = 0;
        #endregion

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
