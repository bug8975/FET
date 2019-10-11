using Normalization;
using System;
using System.Collections.Generic;
//using BenNHControl;
using System.IO;
using System.Drawing;

namespace Monitor_HCCS.Common.GeoProspector
{
    public class GEOHelper
    {        
       //归一化到DAT
        public static double[] normalizationsToFile(List<double[]> d, string Name)
        {

            #region 运行时间监控
            System.Diagnostics.Stopwatch watch1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch watch2 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch watch3 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch watch4 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch watch5 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch watch6 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch watch7 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch watch8 = new System.Diagnostics.Stopwatch();
            #endregion

            watch1.Start();
            string normalFile = Name + ".dat";
            if (!File.Exists(normalFile))
                return null;
            if (Convert.ToInt32(new FileInfo(normalFile).Length) == 0)
                return null;

            TTestData[] testData = new TTestData[d.Count];
            for (int i = 0; i < d.Count; i++ )
            {
                testData[i] = new TTestData(1, i, d[i], new byte[] { 0 });
;            }
            string[] head_strings = StringHelper.StrSegmentation(XmlHelper.getValue(Name, "head"), ',');
            float[] head = new float[head_strings.Length]; 
            for (int i = 0; i < head_strings.Length; i++)
            {
                head[i] = float.Parse(head_strings[i]);
            }
            //float[] head = new float[] { 62500, 25000, 10000, 7000, 5800, 5000, 3800, 3400, 3000, 2400, 1500, 1100, 800, 600, 500, 400, 300, 250, 200, 150, 120, 100, 90, 80, 70, 67, 58, 43, 40, 38, 34, 30, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11 };
            List<TPoint3D> pointList = new List<TPoint3D>();


            watch2.Start();
            int normalization_result_count = Normalization.Normalization.getNormalizations(testData, head, head.Length, 0.18, 120, pointList);
            watch2.Stop();


            if (normalization_result_count <= 0)
            {
                //MessageBoxEX.Show("调用归一化失败");
                return null;
            }

            string strFileContext = "";
            //count为测量次数
            int count = 0;
            //number为HZ频率的数量
            int number = 0;
            double[] point_z = new double[pointList.Count];
            List<TPoint3D> point_chart = new List<TPoint3D>();
            for (int i = 1; i < pointList.Count; i++)
            {
                if (pointList[i].x == pointList[0].x)
                {
                    count = i;
                    break;
                }
            }
            number = pointList.Count / count;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < number; j++)
                {
                    point_chart.Add(new TPoint3D(pointList[i].x+1, pointList[j * count + i].y, pointList[j * count + i].z));
                }
            }
            for (int i = 0; i < normalization_result_count; i++)
            {
                TPoint3D point = point_chart[i];
                double z = Convert.ToDouble(point.z.ToString("f3"));
                strFileContext += (point.x + "\t" + point.y + "\t" + z + "\r\n");
                point_z[i] = point.z;
            }

            string geoFile = "GEO_" + Name + ".dat";
            File.WriteAllText(geoFile, strFileContext);

            #region 测试运行时长

            watch1.Stop();
            Console.WriteLine("归一化总时长为：" + watch1.Elapsed.TotalMilliseconds);
            Console.WriteLine("归一化第三方方法运行时长为：" + watch2.Elapsed.TotalMilliseconds);
            watch1.Reset();
            watch2.Reset();
            #endregion



            return point_z;
        }

        //制图
        public static bool Surfer(string Name, int pointNum, string fet, string gears)
        {
            string normalFileName = System.AppDomain.CurrentDomain.BaseDirectory + "GEO_" + Name + ".dat";
            string imageFileName = System.AppDomain.CurrentDomain.BaseDirectory + Name + "img.jpg";
            if (!File.Exists(normalFileName))
                return false;

            //if (File.Exists(imageFileName))
            //    return true;
            if (pointNum == 0)
                return false;

            if (!SurferHandler.SurferKriging(normalFileName, 557, 882, imageFileName, pointNum, fet, gears))
                return false;

            try
            {
                Image image = Image.FromFile(imageFileName);
                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                image.Save(imageFileName);
                image.Dispose(); 
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
