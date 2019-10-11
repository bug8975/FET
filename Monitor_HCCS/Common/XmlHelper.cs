using System.Data;
using System.Xml;

namespace Monitor_HCCS.Common
{
    public class XmlHelper
    {
        private static string pathFile = @"Mapping.xml";
        public static string getValue(string name, string child)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(pathFile);

                string fetname = "";
                string sql = "select gears from info where name = '" + name + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql, null);
                if (dt.Rows.Count == 0)
                    return "";

                fetname = dt.Rows[0].ItemArray[0].ToString();
                if(fetname.Equals("")  && Monitor_HCCS.Service.BLECode.GetIntance.CurrentService != null)
                {
                    fetname = doc.SelectSingleNode("Mapping/DEVICE").InnerText.ToString();
                    XmlNode xn = doc.SelectSingleNode("Mapping/" + fetname.ToUpper() + "/" + child);
                    string str = xn.InnerText.ToString();
                    doc = null;
                    return str;
                }
                else
                {
                    XmlNode xn = doc.SelectSingleNode("Mapping/" + "FET-" + fetname.ToUpper() + "/" + child);
                    string str = xn.InnerText.ToString();
                    doc = null;
                    return str;
                }
            }
            catch (System.Exception ex)
            {
                Log4netHelper.Error(ex);
                return "";
            }
            
        }


        public static string getValue(string child)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(pathFile);
                string fetname = doc.SelectSingleNode("Mapping/DEVICE").InnerText.ToString();

                #region 2000A和3000A没有WT前缀，需转字符串处理
                if (fetname.Contains("2000A"))
                    fetname = "FET-WT2000A";
                else if (fetname.Contains("3000A"))
                    fetname = "FET-WT3000A";
                #endregion

                XmlNode xn = doc.SelectSingleNode("Mapping/" + fetname + "/" + child);
                string str = xn.InnerText.ToString();
                doc = null;
                return str;

            }
            catch (System.Exception ex)
            {
                Log4netHelper.Error(ex);
                return "";
            }

        }

        public static void setValue(string fet)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathFile);
            XmlNode xn = doc.SelectSingleNode("Mapping/DEVICE");
            xn.InnerText = fet;
            doc.Save(pathFile);
            doc = null;
        }

        public static void setVer(string ver)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathFile);
            XmlNode xn = doc.SelectSingleNode("Mapping/VER");
            xn.InnerText = ver;
            doc.Save(pathFile);
            doc = null;
        }

        public static string getVer(string ver)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathFile);
            return doc.SelectSingleNode("Mapping/" + ver).InnerText.ToString();
        }
    }
}
