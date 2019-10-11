using Monitor_HCCS.Common;
using Monitor_HCCS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Model.Domain
{
    public class SetupRepository : ISetupRepository
    {
        BLECode bluetooth = BLECode.GetIntance;

        #region 固件升级

        // 读芯片ID
        public bool ReadChipId()
        {
            try
            {
                if (bluetooth.CurrentService == null)
                    return false;

                Modbus.MBConfig(0xFA, 9600);
                Modbus.MBAddCmd(0x1024, 0x03, 0x0002);
                BLECode.GetIntance.Write(Modbus.ChipID);
                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        //连接服务器
        public string VerifyID(DownloadFile fwud, string data)
        {
            fwud.SocketConnet("101.37.16.60", 9990);
            fwud.Send(data);
            
            //下载bin文件
            return fwud.DownloadBin();
        }

        //向物探仪发送程序升级命令
        public bool StartFirmUpdata()
        {
            try
            {
                if (bluetooth.CurrentService == null)
                    return false;

                Modbus.MBConfig(0xFA, 9600);
                Modbus.MBAddCmd(0x1100, 0x06, 0x0001);
                BLECode.GetIntance.Write(Modbus.ChipID);
                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        #endregion


        public bool SoftwareUpdata()
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }
    }
}
