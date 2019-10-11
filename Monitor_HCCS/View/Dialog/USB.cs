using System;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using ToastNotifications;
using TX.Framework.WindowUI.Forms;
using Monitor_HCCS.Presenter;
using Monitor_HCCS.Model;
using Monitor_HCCS.Common;
using System.Data;
using System.Threading.Tasks;
using System.Threading;

namespace Monitor_HCCS.View
{
    public partial class USB : DevExpress.XtraEditors.XtraForm, IUSBView
    {
        private IHomeRepository homeRep;
        public Info[] ifs;
        private USBPresenter _uSBPresenter;
        public string DestPath { get; set; }


        //定义一个用于保存静态变量的实例
        private static USB instance = null;
        //定义一个保证线程同步的标识
        private static readonly object locker = new object();
        public static USB GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new USB();
                }
                return instance;
            }
            return instance;
        }
        public USB()
        {
            InitializeComponent();
            _uSBPresenter = new USBPresenter(this);
            homeRep = new HomeRepository();

        }

        //加载
        private void USB_Load(object sender, EventArgs e)
        {

        }
        //导出
        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(DestPath))
                {
                    ShowToast("提示", "U盘未连接", 2);
                    return;
                }

                if (listView1.SelectedItems.Count == 0)
                {
                    ShowToast("提示", "没有选中任何文件", 2);
                    return;
                }

                if (!Directory.Exists(DestPath))
                    Directory.CreateDirectory(DestPath);

                string sourcePath = System.AppDomain.CurrentDomain.BaseDirectory;

                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    string fileName = item.Text + ".dat";
                    if (fileName == "")
                        continue;

                    if (!File.Exists(fileName))
                    {
                        //ShowToast("提示", "不存在" + fileName + "文件", 2);
                        continue;
                    }
                        

                    string SSImageName = item.Text + "_SSImage.jpg";
                    if (!File.Exists(SSImageName))
                        continue;

                    File.Copy(System.IO.Path.Combine(sourcePath, fileName), System.IO.Path.Combine(DestPath, fileName), true);
                    File.Copy(System.IO.Path.Combine(sourcePath, SSImageName), System.IO.Path.Combine(DestPath, SSImageName), true);
                }


                ShowToast("提示", "文件导出成功", 1);
            }
            catch (IOException copyError)
            {
                ShowToast("提示", copyError.Message, 2);
                return;
            }
        }
        //导出全部
        private void txButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(DestPath))
                {
                    ShowToast("提示", "U盘未连接", 2);
                    return;
                }

                if (listView1.Items.Count == 0)
                {
                    ShowToast("提示", "没有文件可以导出", 2);
                    return;
                }

                if (!Directory.Exists(DestPath))
                    Directory.CreateDirectory(DestPath);

                string sourcePath = System.AppDomain.CurrentDomain.BaseDirectory;

                foreach (ListViewItem item in listView1.Items)
                {
                    string fileName = item.Text + ".dat";
                    if (fileName == "")
                        continue;
                    if (!File.Exists(fileName))
                        continue;
                    File.Copy(System.IO.Path.Combine(sourcePath, fileName), System.IO.Path.Combine(DestPath, fileName), true);

                    string SSImageName = item.Text + "_SSImage.jpg";
                    if (!File.Exists(SSImageName))
                        continue;                    
                    File.Copy(System.IO.Path.Combine(sourcePath, SSImageName), System.IO.Path.Combine(DestPath, SSImageName), true);
                }


                ShowToast("提示", "所有文件导出成功", 2);
            }
            catch (IOException copyError)
            {
                ShowToast("提示", copyError.Message, 2);
                return;
            }
        }

        //Toast服务
        public void ShowToast(string title, string body, int time)
        {
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() => { new ToastNotifications.Notification(title, body, time, ToastNotifications.FormAnimator.AnimationMethod.Slide, ToastNotifications.FormAnimator.AnimationDirection.Up).Show(); }));
        }

        //项目名映射文件名
        public string FindFileByName(string name)
        {
            try
            {
                string sql1 = "select filename from info where name = '" + name + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql1);
                if (dt.Rows.Count == 0)
                    return "";

                return dt.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return "";
            }
        }

        //刷新
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

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
