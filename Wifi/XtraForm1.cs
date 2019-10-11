using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NativeWifi;
using BenNHControl;

namespace WifiTool
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        private List<Wlan.WlanAvailableNetwork> NetWorkList = new List<Wlan.WlanAvailableNetwork>();
        private WlanClient.WlanInterface WlanIface;
        
        public XtraForm1()
        {
            InitializeComponent();
        }

        #region MyRegion

        void WlanIface_WlanConnectionNotification(Wlan.WlanNotificationData notifyData, Wlan.WlanConnectionNotificationData connNotifyData)
        {
            if (notifyData.notificationSource == NativeWifi.Wlan.WlanNotificationSource.MSM)
            {
                //这里是完成连接
                if ((NativeWifi.Wlan.WlanNotificationCodeMsm)notifyData.NotificationCode == NativeWifi.Wlan.WlanNotificationCodeMsm.Connected)
                {
                    Invoke(new Action(() =>
                    {
                        labelControl2.Text = connNotifyData.profileName;
                        labelControl2.ForeColor = Color.Red;
                        ShowToast_InSetup("提示", "网络连接成功", 2);
                    }));
                }
            }
            else if (notifyData.notificationSource == NativeWifi.Wlan.WlanNotificationSource.ACM)
            {
                //连接失败
                if ((NativeWifi.Wlan.WlanNotificationCodeAcm)notifyData.NotificationCode == NativeWifi.Wlan.WlanNotificationCodeAcm.ConnectionAttemptFail)
                {
                    Invoke(new Action(() =>
                    {
                        labelControl2.Text = "未连接";
                    }));
                    new Toast().ShowInfo(2500, "WIFI连接失败，请检查密码是否正确");
                    WlanIface.DeleteProfile(connNotifyData.profileName);
                }
                if ((NativeWifi.Wlan.WlanNotificationCodeAcm)notifyData.NotificationCode == NativeWifi.Wlan.WlanNotificationCodeAcm.Disconnected)
                {
                    Invoke(new Action(() =>
                    {
                        labelControl2.Text = "未连接";
                    }));
                }
                if ((NativeWifi.Wlan.WlanNotificationCodeAcm)notifyData.NotificationCode == NativeWifi.Wlan.WlanNotificationCodeAcm.Disconnecting)
                {
                    Invoke(new Action(() =>
                    {
                        labelControl2.Text = "未连接";
                    }));
                }
                if ((NativeWifi.Wlan.WlanNotificationCodeAcm)notifyData.NotificationCode == NativeWifi.Wlan.WlanNotificationCodeAcm.ConnectionStart)
                {
                    Invoke(new Action(() =>
                    {
                        labelControl2.Text = "连接中…";
                    }));
                }
            }

        }

        private void MyWifi_Load(object sender, EventArgs e)
        {
            WlanClient client = new WlanClient();
            WlanIface = client.Interfaces[0];//一般就一个网卡，有2个没试过。
            WlanIface.WlanConnectionNotification += WlanIface_WlanConnectionNotification;
            LoadNetWork();
        }

        private void LoadNetWork()
        {
            Wlan.WlanAvailableNetwork[] networks = WlanIface.GetAvailableNetworkList(0);
            if(networks == null)
            {
                MessageBox.Show("WIFI服务未开启");
                return;
            }
            foreach (Wlan.WlanAvailableNetwork network in networks)
            {
                //过滤信号强度小于20的网络
                if (network.wlanSignalQuality < 20)
                    continue;
                string SSID = WlanHelper.GetStringForSSID(network.dot11Ssid);
                if (network.flags.HasFlag(Wlan.WlanAvailableNetworkFlags.Connected))
                {
                    labelControl2.Text = SSID;
                    labelControl2.ForeColor = Color.RosyBrown;
                }
                //如果有配置文件的SSID会重复出现。过滤掉
                if (!myListBox1.Items.Contains(SSID))
                {
                    myListBox1.Items.Add(SSID);
                    NetWorkList.Add(network);
                }
            }

            //信号强度排序
            NetWorkList.Sort(delegate(Wlan.WlanAvailableNetwork a, Wlan.WlanAvailableNetwork b)
            {
                return b.wlanSignalQuality.CompareTo(a.wlanSignalQuality);
            });
            myListBox1.Items.Clear();
            foreach (Wlan.WlanAvailableNetwork network in NetWorkList)
            {
                myListBox1.Items.Add(WlanHelper.GetStringForSSID(network.dot11Ssid));
            }
        }

        //连接
        private void txButton1_Click(object sender, EventArgs e)
        {
            if (InternetGetConnectedState(out dwFlag, 0))
            {
                ShowToast_InSetup("提示", "网络已连接，请勿重新连接", 2);
                return;
            }

            if (myListBox1.SelectedItems.Count == 0)
                return;
            if (myListBox1.SelectedIndex == -1)
                return;

            Wlan.WlanAvailableNetwork wn = NetWorkList[myListBox1.SelectedIndex];
            if (wn.securityEnabled && !WlanHelper.HasProfile(WlanIface, WlanHelper.GetStringForSSID(wn.dot11Ssid)))
            {
                Form_Password fp = new Form_Password();
                if (fp.ShowDialog() == DialogResult.OK)
                {
                    string pw = fp.Password;

                    WlanHelper.ConnetWifi(WlanIface, wn, pw);
                    fp.Dispose();
                }
            }
            else
            {
                WlanHelper.ConnetWifi(WlanIface, wn);
            }

        }

        //listbox选择框
        private void myListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Wlan.WlanAvailableNetwork wn = NetWorkList[myListBox1.SelectedIndex];
            toolTip1.SetToolTip(myListBox1, WlanHelper.GetWifiToolTip(wn));
        }

        //刷新
        private void txButton2_Click(object sender, EventArgs e)
        {
            NetWorkList.Clear();
            myListBox1.Items.Clear();
            LoadNetWork();
        }
        #endregion

        //退出
        private void TxButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void myListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            StringFormat strFmt = new System.Drawing.StringFormat();
            strFmt.Alignment = StringAlignment.Near; //文本垂直居中
            strFmt.LineAlignment = StringAlignment.Center; //文本水平居中
            e.Graphics.DrawString(myListBox1.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, strFmt);
        }


        #region 检查网络是否连接
        //导入判断网络是否连接的 .dll  
        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        //判断网络状况的方法,返回值true为连接，false为未连接  
        public extern static bool InternetGetConnectedState(out int conState, int reder);
        int dwFlag = 0;
        #endregion

        //Toast服务
        public void ShowToast_InSetup(string title, string body, int time)
        {
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() => { new ToastNotifications.Notification(title, body, time, ToastNotifications.FormAnimator.AnimationMethod.Slide, ToastNotifications.FormAnimator.AnimationDirection.Up).Show(); }));
        }        
    }
}