using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Common
{
    public class Percentage
    {

        public static float GetPerc(string Name, int pointNum, string gears)
        {
            int axisw = 0;
            string sql = "select fet from info where name = '" + Name + "'";
            DataTable dt = SQLiteHelper.ExecuteDataTable(sql, null);
            if (dt.Rows.Count == 0)
                return 0;

            string flag = dt.Rows[0].ItemArray[0].ToString();

            #region 300A
            if ("F".Equals(flag))
            {
                if (gears.Equals("100"))
                {
                    axisw = 900;
                }
                else if (gears.Equals("200"))
                {
                    axisw = 910;
                }
                else
                {
                    if (pointNum <= 20)
                        axisw = 920;
                    if (pointNum > 20)
                        axisw = 918;
                }
            }
            #endregion

            #region 400A
            if ("G".Equals(flag))
            {
                if (gears.Equals("200"))
                {
                    axisw = 910;
                }
                else if (gears.Equals("300"))
                {
                    axisw = 915;
                }
                else
                {
                    if (pointNum == 10)
                        axisw = 912;
                    if (pointNum == 11)
                        axisw = 908;
                    if (pointNum == 12)
                        axisw = 901;
                    if (pointNum == 13)
                        axisw = 894;
                    if (pointNum >= 14)
                        axisw = 890;
                }                
            }
            #endregion

            #region 500A
            if ("H".Equals(flag))
            {
                if (gears.Equals("200"))
                {
                    axisw = 910;
                }
                else if (gears.Equals("400"))
                {
                    axisw = 910;
                    if (pointNum == 12)
                        axisw = 900;
                    if (pointNum > 12)
                        axisw = 890;
                }
                else
                {
                    axisw = 912;
                    if (pointNum == 11)
                        axisw = 905;
                    if (pointNum == 12)
                        axisw = 898;
                    if (pointNum == 13)
                        axisw = 890;
                    if (pointNum == 14)
                        axisw = 884;
                    if (pointNum == 15)
                        axisw = 877;
                    if (pointNum == 16)
                        axisw = 870;
                    if (pointNum == 17)
                        axisw = 868;
                    if (pointNum > 17)
                    {
                        axisw = 870;
                    }
                }
                
            }
            #endregion

            #region 600A
            if ("I".Equals(flag))
            {
                if (gears.Equals("200"))
                {
                    axisw = 910;
                }
                else if (gears.Equals("400"))
                {
                    axisw = 910;
                    if (pointNum == 12)
                        axisw = 900;
                    if (pointNum > 12)
                        axisw = 890;
                }
                else
                {
                    if (pointNum <= 20)
                        axisw = 920;
                    if (pointNum > 20)
                        axisw = 918;
                }
                axisw = 911;
                if (pointNum == 11)
                    axisw = 905;
                if (pointNum == 12)
                    axisw = 898;
                if (pointNum == 13)
                    axisw = 891;
                if (pointNum == 14)
                    axisw = 883;
                if (pointNum == 15)
                    axisw = 877;
                if (pointNum == 16)
                    axisw = 870;
                if (pointNum == 17)
                    axisw = 863;
                if (pointNum == 18)
                    axisw = 855;
                if (pointNum >= 19)
                    axisw = 849;
            }
            #endregion

            #region 800A
            if ("J".Equals(flag))
            {
                 if (gears.Equals("200"))
                {
                    axisw = 910;
                }
                 else if (gears.Equals("400"))
                 {
                     axisw = 910;
                     if (pointNum == 12)
                         axisw = 900;
                     if (pointNum > 12)
                         axisw = 890;
                 }
                 else
                 {
                     axisw = 911;
                     if (pointNum == 11)
                         axisw = 905;
                     if (pointNum == 12)
                         axisw = 898;
                     if (pointNum == 13)
                         axisw = 891;
                     if (pointNum == 14)
                         axisw = 883;
                     if (pointNum == 15)
                         axisw = 877;
                     if (pointNum == 16)
                         axisw = 870;
                     if (pointNum == 17)
                         axisw = 863;
                     if (pointNum == 18)
                         axisw = 855;
                     if (pointNum == 19)
                         axisw = 849;
                     if (pointNum == 20)
                         axisw = 842;
                     if (pointNum == 21)
                         axisw = 836;
                     if (pointNum == 22)
                         axisw = 829;
                     if (pointNum == 23)
                         axisw = 823;
                     if (pointNum == 24)
                         axisw = 815;
                     if (pointNum == 25)
                         axisw = 812;
                     if (pointNum == 26)
                         axisw = 807;
                     if (pointNum == 27)
                         axisw = 807;
                     if (pointNum >= 28)
                         axisw = 807;
                 }
            }
            #endregion

            #region 1000A
            if ("K".Equals(flag))
            {
                if (gears.Equals("300"))
                {
                    axisw = 918;
                }
                else if (gears.Equals("600"))
                {
                    axisw = 913;
                    if (pointNum == 11)
                    {
                        axisw = 905;
                    }
                    if (pointNum == 12)
                    {
                        axisw = 900;
                    }
                    if (pointNum >= 13)
                    {
                        axisw = 890;
                    }
                    if (pointNum >= 14)
                    {
                        axisw = 885;
                    }
                    if (pointNum >= 15)
                    {
                        axisw = 875;
                    }
                    if (pointNum >= 17)
                    {
                        axisw = 860;
                    }
                    if (pointNum >= 19)
                    {
                        axisw = 850;
                    }
                }
                else
                {
                    axisw = 911;
                    if (pointNum == 11)
                        axisw = 905;
                    if (pointNum == 12)
                        axisw = 898;
                    if (pointNum == 13)
                        axisw = 891;
                    if (pointNum == 14)
                        axisw = 883;
                    if (pointNum == 15)
                        axisw = 877;
                    if (pointNum == 16)
                        axisw = 870;
                    if (pointNum == 17)
                        axisw = 863;
                    if (pointNum == 18)
                        axisw = 855;
                    if (pointNum == 19)
                        axisw = 849;
                    if (pointNum == 20)
                        axisw = 842;
                    if (pointNum == 21)
                        axisw = 836;
                    if (pointNum == 22)
                        axisw = 829;
                    if (pointNum == 23)
                        axisw = 823;
                    if (pointNum == 24)
                        axisw = 815;
                    if (pointNum == 25)
                        axisw = 812;
                    if (pointNum == 26)
                        axisw = 803;
                    if (pointNum == 27)
                        axisw = 797;
                    if (pointNum >= 28)
                        axisw = 792;
                    if (pointNum == 29)
                        axisw = 785;
                    if (pointNum == 30)
                        axisw = 778;
                    if (pointNum == 31)
                        axisw = 775;
                    if (pointNum == 32)
                        axisw = 773;
                    if (pointNum >= 33)
                        axisw = 773;
                }
                
            }
            #endregion

            #region 1200A
            if ("L".Equals(flag))
            {
                if (gears.Equals("300"))
                {
                    axisw = 918;
                }
                else if (gears.Equals("600"))
                {
                    axisw = 913;
                    if (pointNum == 11)
                    {
                        axisw = 905;
                    }
                    if (pointNum == 12)
                    {
                        axisw = 900;
                    }
                    if (pointNum >= 13)
                    {
                        axisw = 890;
                    }
                    if (pointNum >= 14)
                    {
                        axisw = 885;
                    }
                    if (pointNum >= 15)
                    {
                        axisw = 875;
                    }
                    if (pointNum >= 17)
                    {
                        axisw = 860;
                    }
                    if (pointNum >= 19)
                    {
                        axisw = 850;
                    }
                }
                else
                {
                    axisw = 911;
                    if (pointNum == 11)
                        axisw = 905;
                    if (pointNum == 12)
                        axisw = 898;
                    if (pointNum == 13)
                        axisw = 891;
                    if (pointNum == 14)
                        axisw = 883;
                    if (pointNum == 15)
                        axisw = 877;
                    if (pointNum == 16)
                        axisw = 870;
                    if (pointNum == 17)
                        axisw = 863;
                    if (pointNum == 18)
                        axisw = 855;
                    if (pointNum == 19)
                        axisw = 849;
                    if (pointNum == 20)
                        axisw = 842;
                    if (pointNum == 21)
                        axisw = 836;
                    if (pointNum == 22)
                        axisw = 829;
                    if (pointNum == 23)
                        axisw = 823;
                    if (pointNum == 24)
                        axisw = 815;
                    if (pointNum == 25)
                        axisw = 812;
                    if (pointNum == 26)
                        axisw = 803;
                    if (pointNum == 27)
                        axisw = 797;
                    if (pointNum >= 28)
                        axisw = 792;
                    if (pointNum == 29)
                        axisw = 785;
                    if (pointNum == 30)
                        axisw = 778;
                    if (pointNum == 31)
                        axisw = 775;
                    if (pointNum == 32)
                        axisw = 770;
                    if (pointNum >= 33)
                        axisw = 765;
                    if (pointNum == 34)
                        axisw = 759;
                    if (pointNum == 35)
                        axisw = 754;
                    if (pointNum == 36)
                        axisw = 747;
                    if (pointNum >= 37)
                        axisw = 742;
                    if (pointNum == 38)
                        axisw = 739;
                    if (pointNum == 39)
                        axisw = 739;
                    if (pointNum >= 40)
                        axisw = 739;
                }
                
            }
            #endregion

            #region 2000A
            if ("S".Equals(flag))
            {
                axisw = 928;
                if(pointNum >= 10 && pointNum <= 15)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 10) * 7.00));
                    axisw = 928 - ceil;
                }
                if (pointNum > 15 && pointNum <= 20)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 15) * 5.00));
                    axisw = 893 - ceil;
                }
                if (pointNum > 20 && pointNum <= 30)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 21) * 6.80));
                    axisw = 868 - ceil;
                }
                if (pointNum > 30 && pointNum <= 60)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 30) * 2.90));
                    axisw = 879 - ceil;
                }
                if (pointNum > 60 && pointNum <= 230)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 61) * 0.0));
                    axisw = 868 - ceil;
                }
                if (pointNum > 230)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 230) * 0.02));
                    axisw = 868 - ceil;
                }
                
            }
            #endregion

            #region 3000A
            if ("R".Equals(flag))
            {
                axisw = 911;
                if (pointNum == 11)
                    axisw = 905;
                if (pointNum == 12)
                    axisw = 898;
                if (pointNum == 13)
                    axisw = 891;
                if (pointNum == 14)
                    axisw = 883;
                if (pointNum == 15)
                    axisw = 877;
                if (pointNum == 16)
                    axisw = 870;
                if (pointNum == 17)
                    axisw = 863;
                if (pointNum == 18)
                    axisw = 855;
                if (pointNum == 19)
                    axisw = 849;
                if (pointNum == 20)
                    axisw = 842;
                if (pointNum == 21)
                    axisw = 836;
                if (pointNum == 22)
                    axisw = 829;
                if (pointNum == 23)
                    axisw = 823;
                if (pointNum == 24)
                    axisw = 815;
                if (pointNum == 25)
                    axisw = 812;
                if (pointNum == 26)
                    axisw = 803;
                if (pointNum == 27)
                    axisw = 797;
                if (pointNum == 28)
                    axisw = 792;
                if (pointNum == 29)
                    axisw = 785;
                if (pointNum == 30)
                    axisw = 778;
                if (pointNum == 31)
                    axisw = 775;
                if (pointNum == 32)
                    axisw = 770;
                if (pointNum == 33)
                    axisw = 765;
                if (pointNum == 34)
                    axisw = 759;
                if (pointNum == 35)
                    axisw = 754;
                if (pointNum == 36)
                    axisw = 747;
                if (pointNum == 37)
                    axisw = 742;
                if (pointNum == 38)
                    axisw = 736;
                if (pointNum == 39)
                    axisw = 733;
                if (pointNum == 40)
                    axisw = 729;
                if (pointNum == 41)
                    axisw = 723;
                if (pointNum == 42)
                    axisw = 717;
                if (pointNum == 43)
                    axisw = 714;
                if (pointNum == 44)
                    axisw = 708;
                if (pointNum == 45)
                    axisw = 703;
                if (pointNum == 46)
                    axisw = 699;
                if (pointNum == 47)
                    axisw = 693;
                if (pointNum == 48)
                    axisw = 691;
                if (pointNum == 49)
                    axisw = 686;
                if (pointNum == 50)
                    axisw = 681;
                if (pointNum == 51)
                    axisw = 676;
                if (pointNum == 52)
                    axisw = 673;
                if (pointNum == 53)
                    axisw = 670;
                if (pointNum == 54)
                    axisw = 665;
                if (pointNum == 55)
                    axisw = 660;
                if (pointNum == 56)
                    axisw = 656;
                if (pointNum >= 56 & pointNum <= 80)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 56) * 3.5333));
                    axisw = 656 - ceil;
                }
                if (pointNum >= 80 && pointNum <= 90)
                {
                    int ceil = (pointNum - 80) * 3;
                    axisw = 573 - ceil;
                }
                if (pointNum > 90 && pointNum <= 110)
                {
                    int ceil = (pointNum - 90) * 1;
                    axisw = 544 - ceil;
                }
                if (pointNum > 110)
                    axisw = 514;
            }
            #endregion

            #region EX30
            //EX30
            if ("M".Equals(flag))
            {
                axisw = 920;//
                if (pointNum >= 32)
                {
                    axisw = 910;
                }
                if (pointNum >= 36)
                {
                    axisw = 900;
                }
                if (pointNum >= 41)
                {
                    axisw = 890;
                }
                if (pointNum >= 46 && pointNum < 50)
                {
                    axisw = 877;
                }
                if (pointNum >= 50 && pointNum <= 60)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 50) * 1.3));
                    axisw = 944 - ceil;
                }
                if (pointNum > 60 && pointNum <= 70)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 50) * 1.15));
                    axisw = 944 - ceil;
                }
                if (pointNum > 70 && pointNum <= 80)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 50) * 1.0));
                    axisw = 944 - ceil;
                }
                if (pointNum > 80 && pointNum <= 110)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 50) * 1.0));
                    axisw = 944 - ceil;
                }
                if (pointNum > 110 && pointNum < 140)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 50) * 0.95));
                    axisw = 944 - ceil;
                }
                if (pointNum >= 140 && pointNum < 210)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 140) * 0.6));
                    axisw = 902 - ceil;
                }
                if (pointNum >= 210 && pointNum <= 300)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 210) * 0.35));
                    axisw = 906 - ceil;
                }
            }
            #endregion

            #region EX50
            //EX50
            if ("N".Equals(flag))
            {
                axisw = 915;//
                if (pointNum >= 18)
                    axisw = 920;
                if (pointNum >= 36)
                    axisw = 922;
                if (pointNum >= 52)
                    axisw = 919;
                if (pointNum >= 54)
                    axisw = 916;
                if (pointNum >= 57)
                    axisw = 912;
                if (pointNum >= 59)
                    axisw = 908;
                if (pointNum >= 62)
                    axisw = 908 - (pointNum - 59);
                if (pointNum >= 67)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 67) * 2.0));
                    axisw = 927 - ceil;
                }
                if (pointNum >= 73)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 67) * 1.00));
                    axisw = 927 - ceil;
                }
                if (pointNum >= 98)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 67) * 0.90));
                    axisw = 927 - ceil;
                }
                if (pointNum >= 120)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 67) * 0.90));
                    axisw = 927 - ceil;
                }
                if (pointNum >= 140)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 140) * 0.70));
                    axisw = 903 - ceil;
                }
                if (pointNum >= 180)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 140) * 0.60));
                    axisw = 903 - ceil;
                }
                if (pointNum >= 210)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 210) * 0.60));
                    axisw = 908 - ceil;
                }
                if (pointNum >= 250)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 210) * 0.40));
                    axisw = 908 - ceil;
                }
            }
            #endregion

            #region EX60
            //EX60
            if ("O".Equals(flag))
            {
                axisw = 900;//
                if (pointNum >= 10)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 10) * 0.2));
                    axisw = 900 + ceil;
                }
                if (pointNum >= 50 && pointNum <=60)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 50) * 0.0));
                    axisw = 935 + ceil;
                }
                if (pointNum > 60 && pointNum < 140)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 60) * 1.00));
                    axisw = 935 - ceil;
                }
                if (pointNum >= 140 && pointNum < 210)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 140) * 0.60));
                    axisw = 902 - ceil;
                }
                if (pointNum >= 210)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 210) * 0.40));
                    axisw = 908 - ceil;
                }
            }
            #endregion

            #region EX80
            //EX80
            if ("P".Equals(flag))
            {
                axisw = 868;//
                if (pointNum >= 10)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 10) * 0.35));
                    axisw = 868 + ceil;
                }
                if (pointNum > 50 && pointNum <= 60)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 51) * 0.50));
                    axisw = 910 + ceil;
                }
                if (pointNum > 60 && pointNum <= 75)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 60) * 0.00));
                    axisw = 914 - ceil;
                }
                if (pointNum > 75 && pointNum <= 110)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 75) * 0.75));
                    axisw = 914 - ceil;
                }
                if (pointNum > 110 && pointNum < 140)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 110) * 0.90));
                    axisw = 888 - ceil;
                }
                if (pointNum >= 140 && pointNum < 210)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 140) * 0.60));
                    axisw = 902 - ceil;
                }
                if (pointNum >= 210)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 210) * 0.40));
                    axisw = 908 - ceil;
                }
            }
            #endregion

            #region EX100
            //EX100
            if ("Q".Equals(flag))
            {
                axisw = 901;//
                if (pointNum >= 10)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 10) * 0.35));
                    axisw = 901 + ceil;
                }
                if (pointNum > 30 && pointNum < 50)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 10) * 0.10));
                    axisw = 901 + ceil;
                }
                if (pointNum >= 50 && pointNum <= 60)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 50) * 0.00));
                    axisw = 895 + ceil;
                }
                if (pointNum > 60 && pointNum <= 75)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 60) * 0.00));
                    axisw = 895 - ceil;
                }
                if (pointNum > 75 && pointNum <= 100)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 75) * 0.00));
                    axisw = 895 - ceil;
                }
                if (pointNum > 100 && pointNum <= 140)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 100) * 0.80));
                    axisw = 895 - ceil;
                }
                if (pointNum >= 140 && pointNum < 160)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 140) * 0.45));
                    axisw = 902 - ceil;
                }
                if (pointNum >= 160 && pointNum < 210)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 140) * 0.60));
                    axisw = 902 - ceil;
                }
                if (pointNum >= 210)
                {
                    int ceil = Convert.ToInt32(Math.Ceiling((pointNum - 210) * 0.40));
                    axisw = 908 - ceil;
                }
            }
            #endregion

            return axisw * 0.001f;
        }
    }
}
