using Monitor_HCCS.Common;
using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace Monitor_HCCS.Service
{
    public class BLECode
    {
        #region 初始化

        //存储检测的设备MAC。
        public string CurrentDeviceMAC { get; set; }
        //存储检测到的设备。
        public BluetoothLEDevice CurrentDevice { get; set; }
        //存储检测到的主服务。
        public GattDeviceService CurrentService { get; set; }
        //存储检测到的写特征对象。
        public GattCharacteristic CurrentWriteCharacteristic { get; set; }
        //存储检测到的通知特征对象。
        public GattCharacteristic CurrentNotifyCharacteristic { get; set; }

        public string ServiceGuid { get; set; }

        public string WriteCharacteristicGuid { get; set; }
        public string NotifyCharacteristicGuid { get; set; }


        private const int CHARACTERISTIC_INDEX = 0;
        //特性通知类型通知启用
        private const GattClientCharacteristicConfigurationDescriptorValue CHARACTERISTIC_NOTIFICATION_TYPE =
            GattClientCharacteristicConfigurationDescriptorValue.Notify;

        private Boolean asyncLock = false;

        #endregion

        #region 委托事件
        //定义值改变委托
        public delegate void eventRun(MsgType type, string str, byte[] data = null);
        //定义值改变事件
        public event eventRun ValueChanged;

        #endregion

        #region 构造方法

        private BLECode(string serviceGuid, string writeCharacteristicGuid, string notifyCharacteristicGuid)
        {
            ServiceGuid = serviceGuid;
            WriteCharacteristicGuid = writeCharacteristicGuid;
            NotifyCharacteristicGuid = notifyCharacteristicGuid;
        }
        //将蓝牙类设置为单例模式
        private static BLECode bluetooth;
        public static BLECode GetIntance
        {
            get
            {
                if (bluetooth != null)
                {
                    return bluetooth;
                }
                else
                {
                    bluetooth = new BLECode("0000ffe0-0000-1000-8000-00805f9b34fb", "0000ffe1-0000-1000-8000-00805f9b34fb", "0000ffe1-0000-1000-8000-00805f9b34fb");
                    return bluetooth;
                }
            }
        }
        #endregion

        #region 查找设备
        /// <summary>
        /// 按MAC地址查找系统中配对设备
        /// </summary>
        /// <param name="MAC"></param>
        public async Task SelectDevice(string MAC)
        {
            CurrentDeviceMAC = MAC;
            CurrentDevice = null;
            DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelector()).Completed = async (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Completed)
                {
                    DeviceInformationCollection deviceInformation = asyncInfo.GetResults();
                    foreach (DeviceInformation di in deviceInformation)
                    {
                        await Matching(di.Id);
                    }
                }
            };
        }
        /// <summary>
        /// 按MAC地址直接组装设备ID查找设备
        /// </summary>
        /// <param name="MAC"></param>
        /// <returns></returns>
        public async Task SelectDeviceFromIdAsync(string MAC)
        {
            CurrentDeviceMAC = MAC;
            CurrentDevice = null;
            BluetoothAdapter.GetDefaultAsync().Completed = async (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Completed)
                {
                    BluetoothAdapter mBluetoothAdapter = asyncInfo.GetResults();
                    byte[] _Bytes1 = BitConverter.GetBytes(mBluetoothAdapter.BluetoothAddress);//ulong转换为byte数组
                    Array.Reverse(_Bytes1);
                    string macAddress = BitConverter.ToString(_Bytes1, 2, 6).Replace('-', ':').ToLower();
                    string Id = "BluetoothLE#BluetoothLE" + macAddress + "-" + MAC;
                    await Matching(Id);
                }

            };

        }
        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task Matching(string Id)
        {
            try
            {
                BluetoothLEDevice.FromIdAsync(Id).Completed = async (asyncInfo, asyncStatus) =>
                {
                    if (asyncStatus == AsyncStatus.Completed)
                    {
                        BluetoothLEDevice bleDevice = asyncInfo.GetResults();
                        //在当前设备变量中保存检测到的设备。
                        CurrentDevice = bleDevice;
                        if (CurrentService == null)
                            await Connect();
                    }
                };
            }
            catch (Exception e)
            {
                string msg = "没有发现设备" + e.ToString();
                Log4netHelper.Error(msg);
            }

        }
        #endregion

        #region 连接
        private async Task Connect()
        {
            CurrentDevice.ConnectionStatusChanged += CurrentDevice_ConnectionStatusChanged;
            await SelectDeviceService();

        }
        #endregion

        #region 断开连接
        /// <summary>
        /// 主动断开连接
        /// </summary>
        /// <returns></returns>
        public void Dispose()
        {
            CurrentDeviceMAC = null;
            if(CurrentService != null)
                CurrentService.Dispose();
            if(CurrentDevice != null)
                CurrentDevice.Dispose();
            CurrentDevice = null;
            CurrentService = null;
            CurrentWriteCharacteristic = null;
            CurrentNotifyCharacteristic = null;
        }
        /// <summary>
        /// 连接状态改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CurrentDevice_ConnectionStatusChanged(BluetoothLEDevice sender, object args)
        {
            if (sender.ConnectionStatus == BluetoothConnectionStatus.Disconnected && CurrentDeviceMAC != null)
            {
                string msg = "Disconnected";
                ValueChanged(MsgType.NotifyTxt, msg);
                CurrentDevice.ConnectionStatusChanged -= CurrentDevice_ConnectionStatusChanged;
                if (!asyncLock)
                {
                    asyncLock = true;
                    CurrentDevice.Dispose();
                    CurrentDevice = null;
                    CurrentService = null;
                    CurrentWriteCharacteristic = null;
                    CurrentNotifyCharacteristic = null;
                }

            }
            else
            {
                string msg = "Success";
                ValueChanged(MsgType.NotifyTxt, msg);
            }
        }
        #endregion

        #region 查找服务
        /// <summary>
        /// 按GUID 查找主服务
        /// </summary>
        /// <param name="characteristic">GUID 字符串</param>
        /// <returns></returns>
        public async Task SelectDeviceService()
        {
            Guid guid = new Guid(ServiceGuid);
            CurrentDevice.GetGattServicesForUuidAsync(guid).Completed = (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Completed)
                {
                    try
                    {
                        GattDeviceServicesResult result = asyncInfo.GetResults();

                        if (result.Services.Count > 0)
                        {
                            CurrentService = result.Services[CHARACTERISTIC_INDEX];
                            if (CurrentService != null)
                            {
                                asyncLock = true;
                                GetCurrentWriteCharacteristic();
                                GetCurrentNotifyCharacteristic();

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4netHelper.Error(ex);
                    }
                }
            };
        }
        #endregion


        /// <summary>
        /// 设置写特征对象。
        /// </summary>
        /// <returns></returns>
        public async Task GetCurrentWriteCharacteristic()
        {
            Guid guid = new Guid(WriteCharacteristicGuid);
            CurrentService.GetCharacteristicsForUuidAsync(guid).Completed = async (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Completed)
                {
                    GattCharacteristicsResult result = asyncInfo.GetResults();

                    if (result.Characteristics.Count > 0)
                    {
                        CurrentWriteCharacteristic = result.Characteristics[CHARACTERISTIC_INDEX];
                    }
                    else
                    {
                        //没有发现写特征对象,自动重试中
                        await GetCurrentWriteCharacteristic();
                    }
                }
            };
        }
        /// <summary>
        /// 发送数据接口
        /// </summary>
        /// <param name="characteristic"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Write(byte[] data)
        {
            if (CurrentWriteCharacteristic != null)
            {
                CurrentWriteCharacteristic.WriteValueAsync(CryptographicBuffer.CreateFromByteArray(data), GattWriteOption.WriteWithResponse);
            }

        }

        #region 设置通知对象&设置特征对象为接收对象
        /// <summary>
        /// 设置通知特征对象。
        /// </summary>
        /// <returns></returns>
        public async Task GetCurrentNotifyCharacteristic()
        {
            Guid guid = new Guid(NotifyCharacteristicGuid);
            CurrentService.GetCharacteristicsForUuidAsync(guid).Completed = async (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Completed)
                {
                    GattCharacteristicsResult result = asyncInfo.GetResults();

                    if (result.Characteristics.Count > 0)
                    {
                        CurrentNotifyCharacteristic = result.Characteristics[CHARACTERISTIC_INDEX];
                        CurrentNotifyCharacteristic.ProtectionLevel = GattProtectionLevel.Plain;
                        CurrentNotifyCharacteristic.ValueChanged += Characteristic_ValueChanged;
                        await EnableNotifications(CurrentNotifyCharacteristic);

                    }
                    else
                    {
                        //没有发现通知特征对象,自动重试中
                        await GetCurrentNotifyCharacteristic();
                    }
                }
            };
        }
        /// <summary>
        /// 设置特征对象为接收通知对象
        /// </summary>
        /// <param name="characteristic"></param>
        /// <returns></returns>
        public async Task EnableNotifications(GattCharacteristic characteristic)
        {
            characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(CHARACTERISTIC_NOTIFICATION_TYPE).Completed = async (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Completed)
                {
                    GattCommunicationStatus status = asyncInfo.GetResults();
                    if (status == GattCommunicationStatus.Unreachable)
                    {
                        if (CurrentNotifyCharacteristic != null && !asyncLock)
                        {
                            await EnableNotifications(CurrentNotifyCharacteristic);
                        }
                    }
                    asyncLock = false;
                }
            };
        }
        /// <summary>
        /// 通知：值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            byte[] data;
            CryptographicBuffer.CopyToByteArray(args.CharacteristicValue, out data);
            string str = BitConverter.ToString(data);
            ValueChanged(MsgType.BLEData,str, data);
        }
        #endregion
    }
    #region 结构体
    public enum MsgType
    {
        NotifyTxt,
        BLEData
    }
    #endregion
}
