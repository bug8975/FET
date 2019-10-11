using Monitor_HCCS.Common;
using Monitor_HCCS.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TX.Framework.WindowUI.Forms;
using Windows.Devices.Bluetooth.Advertisement;

namespace Monitor_HCCS.View
{
    public partial class BLE : FormInfoEntity
    {
        public string InfoName { get; set; }

        private delegate void ListBoxCallback(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args);
        Dictionary<string, string> DeviceInfos = new Dictionary<string, string>();
        BLECode bluetooth = BLECode.GetIntance;
        // Create Bluetooth Listener
        BluetoothLEAdvertisementWatcher watcher = new BluetoothLEAdvertisementWatcher();

        public BLE()
        {
            InitializeComponent();
            Watcher();
        }
        //加载
        private void FormBLE_Load(object sender, EventArgs e)
        {

        }
        //确定
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                string mac = "";
                string deviceName = listView1.SelectedItems[0].Text;
                foreach (KeyValuePair<string, string> kvp in DeviceInfos)
                {
                    if (kvp.Value.Equals(deviceName))
                    {
                        mac = kvp.Key;
                        break;
                    }
                }
                //连接蓝牙
                bluetooth.SelectDeviceFromIdAsync(mac);
                //设备名称写入配置文件
                XmlHelper.setValue(deviceName);

                watcher.Received -= OnAdvertisementReceived;
                watcher.Stop();
                this.Close();
            }
        }
        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            watcher.Stop();
            this.Close();
        }

        //开启蓝牙广告观察者
        private void Watcher()
        {
            watcher.ScanningMode = BluetoothLEScanningMode.Active;

            // Only activate the watcher when we're recieving values >= -80
            watcher.SignalStrengthFilter.InRangeThresholdInDBm = -80;

            // Stop watching if the value drops below -90 (user walked away)
            watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -90;

            // Register callback for when we see an advertisements
            watcher.Received += OnAdvertisementReceived;

            // Wait 5 seconds to make sure the device is really out of range
            watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);
            watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(2000);

            // Starting watching for advertisements
            watcher.Start();
        }

        //广播中的设备
        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            //设备名称过滤
            string DeviceName = args.Advertisement.LocalName;

            if (!(StringHelper.IsMatch(DeviceName, 9) || StringHelper.IsMatch(DeviceName, 10)))
                return;

            //地址转为MAC
            ulong DeviceAddress = args.BluetoothAddress;
            string mac = DeviceAddress.ToString("x");
            for (int i = 2; i <= mac.Length - 2;)
            {
                mac = mac.Insert(i, ":");
                i += 3;
            }
            try
            {
                //跨线程委托
                if (this.InvokeRequired)
                {
                    ListBoxCallback lc = new ListBoxCallback(OnAdvertisementReceived);
                    this.Invoke(lc, new object[] { sender, args });
                    return;
                }
                //控件值初始化
                //listView1.Items.Clear();
                //DeviceInfos.Clear();
                if (DeviceInfos.ContainsValue(DeviceName))
                    return;

                ListViewItem lvi = new ListViewItem();
                lvi.Text = DeviceName;
                lvi.ImageIndex = 0;

                DeviceInfos.Add(mac, DeviceName);
                listView1.Items.Add(lvi);
            }
            catch (Exception e)
            {
                Log4netHelper.Error("BLE广告观察者已关闭，OnAdvertisementReceived事件还未停止。");
            }
        }
    }
}
