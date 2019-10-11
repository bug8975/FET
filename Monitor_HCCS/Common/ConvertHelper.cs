using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Common
{
    public class ConvertHelper
    {
        /// <summary>
        /// 保留三位小数
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double[] RoundAccuracy(double[] d)
        {
            for(int i = 0; i < d.Length; i++)
            {
                d[i] = Convert.ToDouble(d[i].ToString("f3"));
            }
            return d;
        }
        /// <summary>
        /// 将二进制值转ASCII格式十六进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length">定长度</param>
        /// <returns></returns>
        public static string ToHexString(int data, int length)
        {
            string result = "";
            if (data > 0)
                result = Convert.ToString(data, 16).ToUpper();
            if (result.Length < length)
            {
                // 位数不够补0
                StringBuilder msg = new StringBuilder(0);
                msg.Length = 0;
                msg.Append(result);
                for (; msg.Length < length; msg.Insert(0, "0")) ;
                result = msg.ToString();
            }
            return result;
        }

        ///<summary>
        /// 将浮点数转ASCII格式十六进制字符串（符合IEEE-754标准（32））
        /// </summary>
        /// <paramname="data">浮点数值</param>
        /// <returns>十六进制字符串</returns>
        public static string DoubleToIntString(double data)
        {
            byte[] intBuffer = BitConverter.GetBytes(data);
            StringBuilder stringBuffer = new StringBuilder(0);
            for (int i = 0; i < intBuffer.Length; i++)
            {
                stringBuffer.Insert(0, ToHexString(intBuffer[i] & 0xff, 2));
            }
            return stringBuffer.ToString();
        }

        /// <summary>
        /// 将ASCII格式十六进制字符串转浮点数（符合IEEE-754标准（32））
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double IntStringTodouble(String data)
        {
            if (data.Length < 8 || data.Length > 8)
            {
                //throw new NotEnoughDataInBufferException(data.length(), 8);
                throw (new ApplicationException("缓存中的数据不完整。"));
            }
            else
            {
                byte[] intBuffer = new byte[4];
                // 将16进制串按字节逆序化（一个字节2个ASCII码）
                for (int i = 0; i < 4; i++)
                {
                    intBuffer[i] = Convert.ToByte(data.Substring((3 - i) * 2, 2), 16);
                }
                return BitConverter.ToSingle(intBuffer, 0);
            }
        }
        /// <summary>
        /// 十六进制字符串转字节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] StringToByte(string s)
        {
            string[] hexValues = s.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[hexValues.Length];
            for (int i = 0; i < hexValues.Length; i++)
            {
                b[i] = Convert.ToByte(hexValues[i], 16);
            }
            return b;
        }
    }
}
