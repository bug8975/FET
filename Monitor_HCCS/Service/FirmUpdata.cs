using Monitor_HCCS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Service
{
    public class FirmUpdata
    {

        #region 文本状态&&进度条值  委托事件

        public delegate void ValueChangedHandler(object sender, EventArgs e);
        public event ValueChangedHandler StateTextChanged;
        public event ValueChangedHandler ProgressUp;

        private string stateText;
        public string StateText
        {
            get { return stateText; }
            set
            {
                if (value.Length != stateText.Length)
                {
                    stateText = value;
                    StateTextChanged.Invoke(this, null);
                }
            }
        }

        public int percentage = 0;
        public int Percentage
        {
            get { return percentage; }
            set
            {
                if (value != percentage)
                {
                    percentage = value;
                    ProgressUp.Invoke(this, null);
                }
            }
        }

        #endregion

        #region 全局属性

        BLECode bluetooth = BLECode.GetIntance;     

        //已经下载的新版本
        string versionFile = XmlHelper.getVer("VER");
        FileInfo newFileInfo;

        bool isUpdateing = false; //是否升级中
        bool isUpdateFileing = false;//是否是在发送升级文件中。可以中断

        readonly int OUTTIME = 1000; //超时
        readonly int CHECK_UPDATED = 1001;//查询升级结果
        readonly int outTime = 60000;//等待升级结果超时时间 1分钟

        FileStream versionFileStream = null;
        double total = 0;
        int sendTotal = 0;
        byte[] sendDatas = new byte[200];
        int Read = 0;
        int allCRC = 0;
        int restCount = 0;
        int po = 0;

        #endregion


        //发送升级数据
        public string sendDrviceUpdate(bool isRestSend)
        {

            if (versionFile == null)
            {                
                stateText = "未下载升级文件或本地文件丢失";
                isUpdateFileing = false;
                return "未下载升级文件或本地文件丢失";
            }

            try 
            {
                if(restCount > 3 )
                {
                    stateText = "发送升级数据失败，请重启蓝牙设备后重试";
                    versionFileStream.Close();
                    versionFileStream = null;
                    return "发送升级数据失败，请重启蓝牙设备后重试";
                }

                if(versionFileStream == null) {
                    restCount = 0;
                    Read = 0;
                    allCRC = 0;
                    sendDatas = new byte[200];
                    sendTotal = 0;
                    versionFile = XmlHelper.getVer("VER");
                    newFileInfo = new FileInfo(versionFile);
                    total = newFileInfo.Length;
                    versionFileStream = new FileStream(versionFile, FileMode.Open, FileAccess.Read);
                    stateText = "开始固件升级";
                    isUpdateFileing = true;
                }

                //原提示  发送升级文件中
                stateText = "固件设备正在升级中...";
                po = (int) ((sendTotal/total) * 100f);
                percentage = po;

                //重新发送上次读取的数据则不读取数据
                if(!isRestSend) {
                    Read = versionFileStream.Read(sendDatas, 0, sendDatas.Length);
                }else{
                    restCount ++;
                }

                if(Read == 0)
                {
                    //数据发送完成
                    versionFileStream.Close();
                    versionFileStream = null;
                    allCRC = BLECommand.getDispersetAllCRC16(allCRC);
                    Log4netHelper.Info("发送所有数据CRC="+allCRC);

                    Modbus.MBAddCmd(0x1104, 0x06, sendTotal, BLECommand.shortToBytesL((short)allCRC));
                    bluetooth.Write(Modbus.UpData);
                    return "";
                }

                if(Read < sendDatas.Length)
                {
                    byte[] send1 = new byte[Read];
                    Array.Copy(sendDatas,0,send1,0,Read);
                    allCRC = BLECommand.dispersetCRC16(allCRC,send1,Read);

                    Modbus.MBAddCmd(0x1102, 0x06, sendTotal, send1);
                    if (Modbus.UpData == null || Modbus.UpData.Length == 0)
                        return "发送数据有误";
                    bluetooth.Write(Modbus.UpData);
                    return "";
                }

                allCRC = BLECommand.dispersetCRC16(allCRC,sendDatas,Read);

                Modbus.MBAddCmd(0x1102, 0x06, sendTotal, sendDatas);
                bluetooth.Write(Modbus.UpData);
                sendTotal += Read;
                return "";

            } catch (IOException ex) {
                Log4netHelper.Error(ex);
                return "";
            }
        }

    }
}
