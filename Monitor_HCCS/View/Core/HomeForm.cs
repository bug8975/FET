using Presenter;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Monitor_HCCS.Common;
using DevExpress.XtraEditors;
using System.Threading;
using Monitor_HCCS.Model;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Monitor_HCCS.View
{
    public partial class HomeForm : XtraForm, IHomeView
    {
        // 申明要使用的dll和api
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "MoveWindow")]
        public static extern bool MoveWindow(System.IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private HomePresenter homePresenter;
        private IHomeRepository homeRep;
        public Info[] ifs;
        Process kbpr;

        //事件
        public event EventHandler LoadHome;
        public event EventHandler SaveInfo;
        public event EventHandler VerifyInfoName;
        public event EventHandler VerifyIncrement;
        public event EventHandler VerifyDistance;
        public event EventHandler VerifyInfoSite;
        public event EventHandler Click_InMain;

        //刷新委托
        delegate void ReFlushCallBack();

        //定义一个用于保存静态变量的实例
        private static HomeForm instance = null;
        //定义一个保证线程同步的标识
        private static readonly object locker = new object();
        public static HomeForm GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new HomeForm();
                }
                return instance;
            }
            return instance;
        }
        private HomeForm()
        {
            InitializeComponent();

            this.homePresenter = new HomePresenter(this);
            homeRep = new HomeRepository();
        }

        #region 属性
        public string InfoName { get { return DBCHelper.ToDBC(textEdit1.Text).Trim(); } set { textEdit1.Text = value; } }
        public string Increment { get { return DBCHelper.ToDBC(textEdit2.Text).Trim(); } set { textEdit2.Text = value; } }
        public string Distance { get { return DBCHelper.ToDBC(textEdit3.Text).Trim(); } set { textEdit3.Text = value; } }
        public string InfoSite { get { return DBCHelper.ToDBC(textEdit4.Text).Trim(); } set { textEdit4.Text = value; } }
        public HomePresenter HomePresenter { get { return HomePresenter; } set { new HomePresenter(this); } }
        #endregion

        //加载
        private void HomeForm_Load(object sender, EventArgs e)
        {
            LoadHome.Invoke(sender, EventArgs.Empty);
        }

        //保存
        private void Save_Click(object sender, EventArgs e)
        {
            SaveInfo.Invoke(sender, EventArgs.Empty);

            DataForm dataForm = DataForm.GetInstance();
            dataForm.InfoName = this.InfoName;
            Click_InMain.Invoke(this, EventArgs.Empty);
        }
        //ListView单选事件
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                DataForm dataForm = DataForm.GetInstance();
                dataForm.InfoName = listView1.SelectedItems[0].Text;
                listView1.SelectedItems.Clear();
                Click_InMain.Invoke(this, EventArgs.Empty);
            }
        }


        #region 按钮焦点改变  && 软键盘关闭和打开
        private void Btn_LostFocus(object sender, EventArgs e)
        {
            StartOSK(false);
        }
        private void Btn_GotFocus(object sender, EventArgs e)
        {
            StartOSK(true);
        }
        #endregion

        #region 服务
        //Toast服务
        public void ShowToast(string title, string body, int time)
        {
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() => { new ToastNotifications.Notification(title, body, time, ToastNotifications.FormAnimator.AnimationMethod.Slide, ToastNotifications.FormAnimator.AnimationDirection.Up).Show(); }));
        }
        //启动软键盘
        private void StartOSK(bool OpenOrKill)
        {
            string windir = System.AppDomain.CurrentDomain.BaseDirectory;
            string osk = null;

            if (osk == null)
            {
                osk = Path.Combine(windir, "osk.exe");
                if (!File.Exists(osk))
                    osk = null;
            }

            if (osk == null)
            {
                osk = Path.Combine(Path.Combine(windir, "system32"), "osk.exe");
                if (!File.Exists(osk))
                {
                    osk = null;
                }
            }

            if (osk == null)
                osk = "osk.exe";

            try
            {
                if (kbpr == null)
                {
                    kbpr = System.Diagnostics.Process.Start(osk); // 打开系统键盘

                    //IntPtr intptr = IntPtr.Zero;
                    //while (IntPtr.Zero == intptr)
                    //{
                    //    System.Threading.Thread.Sleep(100);
                    //    intptr = FindWindow(null, "屏幕键盘");
                    //}

                    //// 获取屏幕尺寸
                    //int iActulaWidth = Screen.PrimaryScreen.Bounds.Width;
                    //int iActulaHeight = Screen.PrimaryScreen.Bounds.Height;

                    //// 设置软键盘的显示位置，底部居中
                    //int posX = (iActulaWidth - 1600) / 2;
                    //int posY = (iActulaHeight - 350);

                    ////设定键盘显示位置
                    //MoveWindow(intptr, posX, posY, 1600, 350, true);

                    ////设置软键盘到前端显示
                    //SetForegroundWindow(intptr);

                    return;
                }

                if (OpenOrKill)
                {
                    if (kbpr.HasExited)
                        kbpr = System.Diagnostics.Process.Start(osk); // 打开系统键盘
                    return;
                }

                if (!kbpr.HasExited)
                    kbpr.Kill();
            }
            catch (Exception)
            {
                ShowToast("提示", "软键盘已关闭", 1);
            }

        }

        #endregion

        #region 输入验证事件

        //测线名称输入合法性校验
        private void TxTextBox1_Validated(object sender, EventArgs e)
        {
            VerifyInfoName.Invoke(sender, EventArgs.Empty);
        }
        //测点增量
        private void TxTextBox2_Validated(object sender, EventArgs e)
        {
            VerifyIncrement.Invoke(sender, EventArgs.Empty);
        }
        //点距
        private void TxTextBox3_Validated(object sender, EventArgs e)
        {
            VerifyDistance.Invoke(sender, EventArgs.Empty);
        }
        //地点
        private void TxTextBox4_Validated(object sender, EventArgs e)
        {
            VerifyInfoSite.Invoke(sender, EventArgs.Empty);
        }

        #endregion


        //刷新数据
        public void ReFlush()
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                ifs = homeRep.GetInfos();
                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action<object>(delegate (object arg)
                    {
                        listView1.Items.Clear();
                        listView1.BeginUpdate();
                        int i = 0;
                        foreach (var item in ifs)
                        {
                            if ((i % 2 == 0))
                            {
                                ListViewItem lv = new ListViewItem
                                {
                                    ImageIndex = 0,
                                    Text = item.Name
                                };
                                listView1.Items.Add(lv);
                            }
                            else
                            {
                                ListViewItem lv = new ListViewItem
                                {
                                    ImageIndex = 1,
                                    Text = item.Name
                                };
                                listView1.Items.Add(lv);
                            }
                            i++;
                        }
                        SendMessage(this.listView1.Handle, LVM_SETICONSPACING, 0, 0x10000 * 180 + 286);
                        listView1.EndUpdate();
                    }), new object[] { null });
                }
            });
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

        #region 调整ListView图片间距
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private int LVM_SETICONSPACING = 0x1035;
        #endregion
    }
}
