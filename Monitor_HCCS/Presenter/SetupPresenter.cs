using FSLib.App.SimpleUpdater;
using Monitor_HCCS.Common;
using Monitor_HCCS.Model;
using Monitor_HCCS.Model.Domain;
using Monitor_HCCS.Service;
using Monitor_HCCS.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Presenter
{
    public class SetupPresenter
    {
        FSLib.App.SimpleUpdater.Updater updater;
        private ISetupView _setupForm;
        private ISetupRepository _setupRepository;
        private DownloadFile _downloadFile;
        private FirmUpdata _firmUpdata;
        private BLECode bluetooth { get; set; }
        public string ChipID { get; set; }
        string msg;
        string Version { get; set; }

        public SetupPresenter(ISetupView SetupView)
        {
            this._setupForm = SetupView;
            _setupRepository = new SetupRepository();
            _downloadFile = new DownloadFile();
            _firmUpdata = new FirmUpdata();
            bluetooth = BLECode.GetIntance;

            bluetooth.ValueChanged += Bluetooth_ValueChanged;

            _setupForm.FirmwareUpdata += FirmwareUpdata_InSetup;
            _setupForm.SoftwareUpdata += SoftwareUpdata_InSetup;

            _downloadFile.ProgressSer += this.ProgressSer_ValueChanged;
            _downloadFile.StateTextChanged += this.StateText_ValueChanged;

            _firmUpdata.ProgressUp += this.ProgressUp_ValueChanged;

        }


        //固件升级
        private void FirmwareUpdata_InSetup(Object sender, EventArgs e)
        {

            Modbus.MBConfig(0xFA, 9600);
            Modbus.MBAddCmd(0x1000, 0x03);
            BLECode.GetIntance.Write(Modbus.WBuff);

            if (!_setupRepository.ReadChipId())
            {
                _setupForm.ShowToast_InSetup("提示", "请检查蓝牙设备是否打开", 2);
                _setupForm.SetButtonUp(true);
                return;
            }
        }

        //软件升级
        private void SoftwareUpdata_InSetup(Object sender, EventArgs eArgs)
        {
            if (File.Exists("Setup.exe"))
                File.Delete("Setup.exe");
            if (updater == null)
            {
                updater = Updater.CreateUpdaterInstance("http://47.102.136.78:8888/Win10APPS/{0}", "update_c.xml");
                updater.Error += (s, e) => { Log4netHelper.Error(updater.Context.Exception); _setupForm.ShowToast_InSetup("更新发生了错误：", updater.Context.Exception.Message, 1); };
                updater.UpdatesFound += (s, e) => { _setupForm.ShowToast_InSetup("发现了新版本：", updater.Context.UpdateInfo.AppVersion, 1); };
                updater.NoUpdatesFound += (s, e) => { _setupForm.ShowToast_InSetup("提示", "没有新版本！", 1); };
                updater.MinmumVersionRequired += (s, e) => { _setupForm.ShowToast_InSetup("提示", "当前版本过低无法使用自动更新！", 1); };
            }
            Updater.CheckUpdateSimple();
        }


        //蓝牙监听服务
        private void Bluetooth_ValueChanged(MsgType type, string str, byte[] data = null)
        {
            if (str.Equals("Disconnected") || str.Equals("Unreachable") || str.Equals("Success"))
                return;

            byte[] b = ConvertHelper.StringToByte(str);

            //仪器基本信息  && 版本号
            if (str.Length == 44 && b[1] == 0x03)
            {
                byte[] bt = new byte[4];
                Array.Copy(b, 3, bt, 0, 4);
                string version = Encoding.ASCII.GetString(bt);

                _setupForm.SetLabelText(version);
                return;
            }

            //读仪器芯片ID
            if (str.Length == 86 && b[1] == 0x03)
            {

                if (!Modbus.ReceiveByteArray(b))
                    return;

                ChipID = System.Text.Encoding.Default.GetString(Modbus.Datas);
                if (ChipID == null || ChipID.Equals(""))
                    return;

                string msg = _setupRepository.VerifyID(_downloadFile, ChipID);

                if (msg != "")
                {
                    _setupForm.ShowToast_InSetup("提示", msg, 2);

                    if (msg.Equals("下载成功"))
                    {
                        _setupForm.ProgressSerChanged();

                        //启动程序升级
                        if (!_setupRepository.StartFirmUpdata())
                            _setupForm.ShowToast_InSetup("提示", "发送升级命令失败", 2);
                        return;
                    }
                    return;
                }


                

            }

            //启动升级返回结果
            if (str.Contains("01-06-11-00"))
            {
                if (Convert.ToInt32(b[5]) != 1)
                {
                    _setupForm.ShowToast_InSetup("提示", "启动固件升级失败", 2);
                    _setupForm.ProgressUpChanged();
                    return;
                }

                _setupForm.ShowToast_InSetup("提示", "正在更新固件设备，请耐心等待。", 2);
                //首次发送固件升级数据
                msg = _firmUpdata.sendDrviceUpdate(false);
                if (msg != "")
                    _setupForm.ShowToast_InSetup("提示", msg, 2);
                _setupForm.ProgressUp(_firmUpdata.percentage);
                return;
            }

            //循环发送更新数据
            if (str.Contains("01-06-11-02"))
            {
                if (Convert.ToInt32(b[10]) == 1)
                {
                    msg = _firmUpdata.sendDrviceUpdate(false);
                    if (msg != "")
                        _setupForm.ShowToast_InSetup("提示", msg, 2);
                    _setupForm.ProgressUp(_firmUpdata.percentage);
                }
                else
                {
                    msg = _firmUpdata.sendDrviceUpdate(true);
                    if (msg != "")
                        _setupForm.ShowToast_InSetup("提示", msg, 2);
                    _setupForm.ProgressUp(_firmUpdata.percentage);
                }
                return;
            }

            //整体CRC校验
            if (str.Contains("01-06-11-04"))
            {
                if (Convert.ToInt32(b[5]) == 1)
                    _setupForm.ShowToast_InSetup("提示", "固件更新成功！", 2);
                else
                    _setupForm.ShowToast_InSetup("提示", "固件更新失败！", 2);

                if (File.Exists(XmlHelper.getVer("VER")))
                    File.Delete(XmlHelper.getVer("VER"));
                _setupForm.ProgressUp(100);
                _setupForm.ProgressUpChanged();
                SetUpText();
                _setupForm.ShowToast_InSetup("提示", "请重启蓝牙设备，已刷新蓝牙状态！", 7);
                return;
            }
        }

        //下载进度条服务
        private void ProgressSer_ValueChanged(Object sender, EventArgs e)
        {
            _setupForm.ProgressSer(_downloadFile.Percentage);
        }

        //固件更新进度条服务
        private void ProgressUp_ValueChanged(Object sender, EventArgs e)
        {
            _setupForm.ProgressUp(_firmUpdata.Percentage);
        }

        //更新状态文本服务
        private void StateText_ValueChanged(object sender, EventArgs e)
        {
            SetUpText();

        }

        //通用更新最新蓝牙型号
        private void SetUpText()
        {
            string server = _downloadFile.StateText;
            Version = server;

            string ver = XmlHelper.getValue("flag");
            string[] str = server.Split(new char[] { Convert.ToChar(ver) });
            //server = "V" + str[0] + ver;
            server = "V" + str[0];
            _setupForm.SetUpText(server);
        }
    }
}
