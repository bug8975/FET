using Monitor_HCCS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Common
{
    public class BLECommand
    {

        /**
         * 发送所有数据的CRC校验码
         * 请求：地址域 功能码 寄存器地址 所有数据的CRC校验码 CRC16校验
         * 响应：地址域 功能码 寄存器地址 所有数据的校验码状态 CRC16校验
         * 主机发送：FA 06 11 04 5C 66 61 96
         * 从机返回：01 06 11 04 00 01 0C F7
         * 注： 5C 66 ----所有数据的CRC校验码
         *  00 01 ----校验码正确  00 00 ---- 错误
         */
        public static void sendDrviceUpdataDataCRC(byte[] datas)
        {
            if (datas == null || datas.Length == 0)
                return;

            //加入验证码
            //CRC16校验：2字节，从地址域开始前面所有数据的CRC
            Modbus.MBAddCmd(0x1104, 0x06, 0, datas);
            BLECode.GetIntance.Write(Modbus.WBuff);
        }

        public static byte[] shortToBytesL(short value)
        {
            //short = int16 = 16bits = 2byte
            byte[] by = new byte[2];
            by[1] = (byte)((value >> 8) & 0xFF);
            by[0] = (byte)(value & 0xFF);
            return by;
        }

        public static int dispersetCRC16(int Reg_CRC, byte[] bytes, int length)
        {
            Reg_CRC = Reg_CRC == 0 ? 0xffff : Reg_CRC;
            int temp;
            int i, j;
            for (i = 0; i < length; i++)
            {
                temp = bytes[i];
                if (temp < 0) temp += 256;
                temp &= 0xff;
                Reg_CRC ^= temp;
                for (j = 0; j < 8; j++)
                {
                    if ((Reg_CRC & 0x0001) == 0x0001)
                        Reg_CRC = (Reg_CRC >> 1) ^ 0xA001;
                    else
                        Reg_CRC >>= 1;
                }
            }

            return Reg_CRC;
        }

        public static int getDispersetAllCRC16(int Reg_CRC)
        {
            Reg_CRC &= 0xFFFF;
            int crc = ((Reg_CRC << 8) & 0xFF00) | ((Reg_CRC >> 8) & 0x00FF);
            return crc;
        }
    }
}
