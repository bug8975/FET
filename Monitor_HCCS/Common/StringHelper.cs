using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Monitor_HCCS.Common
{
    public class StringHelper
    {
        /// <summary>
        /// 将一个字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="symbol">分隔符</param>
        /// <returns></returns>
        public static List<string> StrSegmentationList(string str, char symbol)
        {
            List<string> list = str.Split(symbol).ToList<string>();
            return list;
        }
        /// <summary>
        /// 将一个字符串转按照分割符转换为数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="symbol">分隔符</param>
        /// <returns></returns>
        public static string[] StrSegmentation(string str, char symbol)
        {
            string[] vs = str.Split(symbol);
            return vs;
        }
        /// <summary>
        /// 把 List 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="symbol">分隔符</param>
        /// <returns></returns>
        public static string ListSegmentationStr(List<string> list, string symbol)
        {
            string str = string.Join(symbol, list);
            return str;
        }
        /// <summary>
        /// 把 List 按照分隔符得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string ListSegmentation(List<string> list)
        {

            string str = string.Join(",", list);
            return str;
        }
        /// <summary>
        /// 将Dictionary<int, int> 转换成数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static string DictionarySegmentationStr(Dictionary<int, int> pairs)
        {
            string str = string.Join(",", pairs);
            return str;
        }
        /// <summary>
        /// 删除字符串最后结尾的一个逗号
        /// </summary>
        public static string DeleteString(string str)
        {
            return str.Remove(str.LastIndexOf(","), 1);
        }
        /// <summary>
        /// 删除字符串最后结尾的指定字符后的字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string DeleteString(string str, string symbol)
        {
            return str.Remove(str.LastIndexOf(symbol), str.Length);
        }
        /// <summary>
        /// 将一个字符串转换成全角
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSBC(string str)
        {
            //半角转全角：
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 将一个字符串转换成半角
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDBC(string str)
        {
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 将一个字符串按照指定分隔符装成 List 去除重复
        /// </summary>
        /// <param name="str"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static List<string> StringSegmentationList(string str, char symbol)
        {
            List<string> list = str.Split(symbol).ToList<string>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list.Contains(list[i]))
                {
                    list.Remove(list[i]);
                }
            }
            return list;
        }
        /// <summary>
        /// 截取指定长度字符串  
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SegmentationLength(string str, int length)
        {
            return str.Substring(0, length);
        }

        #region 常用数据验证的封装，数字字符的验证
        /// <summary>
        /// 常用数据验证的封装，数字字符的验证
        /// </summary>
        /// <param name="inputVal">需要验证的数值【字符串，或者数字】</param>
        /// <param name="type">类型为哪一个验证</param>
        /// <returns>如果验证成功则返回True,否则返回false</returns>
        public static bool IsMatch(string inputVal, int type)
        {
            switch (type)
            {
                case 0:
                    return Regex.IsMatch(inputVal, @"^[1-9][0-9]*$");  //匹配正整数
                case 1:
                    return Regex.IsMatch(inputVal, @"^-?\d+$");  //匹配整数
                case 2:
                    return Regex.IsMatch(inputVal, @"^[A-Za-z0-9]+$");  //匹配由数字和26个英文字母组成的字符串 
                case 3:
                    return Regex.IsMatch(inputVal, @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$");  //匹配正浮点数
                case 4:
                    return Regex.IsMatch(inputVal, @"^[\u4e00-\u9fa5]{0,}$");  //匹配汉字
                case 5:
                    return Regex.IsMatch(inputVal, @"^[0-9]+(.[0-9]{1,3})?$");  //匹配1~3位小数的正实数
                case 6:
                    return Regex.IsMatch(inputVal, @"^[A-Za-z]+$");  //匹配英文字符
                case 7:
                    return Regex.IsMatch(inputVal, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");   //验证邮箱
                case 8:
                    return Regex.IsMatch(inputVal, @"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)");   //验证手机号码
                case 9:
                    return Regex.IsMatch(inputVal, @"^FET-(WT)?([1-9][0-9]{1,4})A$");  //匹配WT-A系列设备
                case 10:
                    return Regex.IsMatch(inputVal, @"^FET-EX([1-9][0-9]{1,3})$");  //匹配EX系列设备
                default:
                    return true;
            }
        }
        #endregion
    }
}