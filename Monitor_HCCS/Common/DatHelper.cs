using Monitor_HCCS.Common.UIDataSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Monitor_HCCS.Common
{
    public class DatHelper
    {
        #region  原始DAT文件
        /// <summary>
        /// 写入原始DAT文件
        /// </summary>
        /// <param name="strFileContext">传入dat文件的字符串数据</param>
        public static void WriteDat(string strFileContext)
        {
            try
            {
                string normalFileName = AppDomain.CurrentDomain.BaseDirectory + "normalization.dat";
                using (FileStream fs = new FileStream(normalFileName, FileMode.OpenOrCreate))
                {
                    File.WriteAllText(normalFileName, strFileContext);
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
            }
        }

        /// <summary>
        /// 读取原始dat方法
        /// </summary>
        /// <returns>数据源</returns>
        public static List<double[]> ReadDat(string Name)
        {
            string normalFile = Name + ".dat";
            if (!File.Exists(normalFile))
                return null;
            if (Convert.ToInt32(new FileInfo(normalFile).Length) == 0)
                return null;

            using (FileStream fs = new FileStream(normalFile, FileMode.Open, FileAccess.Read))
            {
                List<double[]> list = new List<double[]>();
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] str = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        double[] d1 = new double[str.Length];
                        for (int j = 0; j < str.Length; j++)
                        {
                            d1[j] = Convert.ToDouble(str[j]);
                        }
                        list.Add(d1);
                    }

                    return list;
                }
            }
        }

        /// <summary>
        /// 删除最后一条原始记录
        /// </summary>
        /// <returns></returns>
        public static bool DeleteDat(string Name)
        {
            string normalFile = Name + ".dat";
            if (!File.Exists(normalFile))
                return false;
            if (Convert.ToInt32(new FileInfo(normalFile).Length) == 0)
                return false;

            #region 删除方法改进
            List<string> lines = new List<string>(File.ReadAllLines(normalFile));
            lines.RemoveAt(lines.Count - 1);//删除第最后一行
            File.WriteAllLines(normalFile, lines.ToArray());
            #endregion

            return true;
        }

        /// <summary>
        /// 在原dat文件上追加一条原始数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ModbusData"></param>
        public static bool AppendDat(string Name, double[] ModbusData)
        {
            StringBuilder strFileContext = new StringBuilder(1024);
            string normalFile = Name + ".dat";
            //if (!File.Exists(normalFile))
            //    return false;
            //if (Convert.ToInt32(new FileInfo(normalFile).Length) == 0)
            //    return false;
            if (ModbusData.Length == 0)
                return false;

            for (int i = 0; i < ModbusData.Length; i++)
            {
                strFileContext.Append(ModbusData[i].ToString() + "\t");
            }

            if (strFileContext.Equals(""))
                return false;
            if (strFileContext == null)
                return false;

            try
            {
                using (FileStream fs = File.Open(normalFile, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(strFileContext);
                    }
                }              
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
            }

            return true;
        }
        #endregion

        #region 归一化Dat文件
        /// <summary>
        /// 读取归一化dat方法
        /// </summary>
        /// <returns>数据源</returns>
        public static double[][] ReadGEODat(string Name)
        {
            string geoFile = "GEO_" + Name + ".dat";

            if (!File.Exists(geoFile))
                return null;
            if (Convert.ToInt32(new FileInfo(geoFile).Length) == 0)
                return null;

            int hzNumber = Convert.ToInt32(XmlHelper.getValue(Name, "number"));
            string[] datStr = File.ReadAllLines(geoFile);
            string[][] data_string = new string[datStr.Length][];
            double[][] data_double = new double[datStr.Length / hzNumber][];
            double[] temp_double = new double[datStr.Length];
            int[] temp_int = new int[datStr.Length];

            for (int i = 0; i < datStr.Length; i++)
            {
                data_string[i] = datStr[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int i = 0; i < data_string.Length; i++)
            {
                temp_int[i] = Convert.ToInt32(data_string[i][0]);
                temp_double[i] = Convert.ToDouble(data_string[i][2]);
            }
            for (int i = 0; i < datStr.Length / hzNumber; i++)
            {
                double[] temp = new double[hzNumber];
                for (int j = 0; j < hzNumber; j++)
                {
                    temp[j] = temp_double[j + i * hzNumber];
                }
                data_double[i] = temp;
            }

            return data_double;
        }
        #endregion

        /// <summary>
        /// 删除某条测线，包涵dat源文件和dat归一化文件
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static bool DeleteBoth(string Name)
        {
            try
            {
                //删除原文件
                string normalFile = Name + ".dat";
                if (!File.Exists(normalFile))
                    return true;
                File.Delete(normalFile);

                //删除归一化文件
                string geoFile = "GEO_" + Name + ".dat";
                if (!File.Exists(geoFile))
                    return true;
                File.Delete(geoFile);

                //删除原始图片
                string pictureFile = Name + "img.jpg";
                string pictureFile_help = pictureFile + ".gsr2";
                if (!File.Exists(pictureFile))
                    return true;
                File.Delete(pictureFile);
                File.Delete(pictureFile_help);

                //删除截图
                string SSImageFile = Name + "_SSImage.jpg";
                if (!File.Exists(SSImageFile))
                    return true;
                File.Delete(SSImageFile);

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
