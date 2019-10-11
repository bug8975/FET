using BenNHControl;
using Monitor_HCCS.Model;
using Monitor_HCCS.View.Base;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitor_HCCS.View.Dialog
{
    public partial class Delete : DevExpress.XtraEditors.XtraForm
    {
        private IHomeRepository homeRep;
        Info[] ifs;

        //定义一个用于保存静态变量的实例
        private static Delete instance = null;
        //定义一个保证线程同步的标识
        private static readonly object locker = new object();
        public static Delete GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new Delete();
                }
                return instance;
            }
            return instance;
        }
        public Delete()
        {
            InitializeComponent();
            homeRep = new HomeRepository();
            
        }

        //加载
        private void Delete_Load(object sender, EventArgs e)
        {
            
        }

        //删除
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    ShowToast("提示", "没有选中任何文件", 2);
                    return;
                }

                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    if (!homeRep.DeleteInfo(item.Text))
                        ShowToast("提示", "删除" + item.Text + "失败", 1);
                }

                ReFlush();
                ShowToast("提示", "删除成功", 1);
            }
            catch (IOException copyError)
            {
                ShowToast("提示", copyError.Message, 2);
                return;
            }
        }
        //全部删除
        private void txButton1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBoxEX.Show("提示", "确认全部删除吗？");
            if (dr != DialogResult.OK)
                return;

            foreach (ListViewItem item in listView1.Items)
            {
                if (!homeRep.DeleteInfo(item.Text))
                    ShowToast("提示", "删除" + item.Text + "失败", 1);
            }

            ReFlush();
            ShowToast("提示", "全部删除成功！", 2);
        }

        //Toast服务
        public void ShowToast(string title, string body, int time)
        {
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() => { new ToastNotifications.Notification(title, body, time, ToastNotifications.FormAnimator.AnimationMethod.Slide, ToastNotifications.FormAnimator.AnimationDirection.Up).Show(); }));
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
    }
}
