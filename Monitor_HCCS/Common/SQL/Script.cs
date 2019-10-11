using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Monitor_HCCS.Common.SQL
{
    public class Script
    {
        #region 方法一：
        public void GetFilePath(string FilePath)
        {
            string ConfigPath = System.AppDomain.CurrentDomain.BaseDirectory + "web.config";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigPath);
            XmlNode root = xmlDoc.SelectSingleNode("configuration");
 
            //解析connectionStringItems
            XmlNode list = root.SelectSingleNode("connectionStringItems");
            //循环遍历数据库连接Items
            foreach (XmlNode item in list.ChildNodes)
            {
                string ConnStr = item.InnerText;
 
                //获取数据库字符串中所有信息
                SqlConnection conn = new SqlConnection(ConnStr);
                SqlConnectionStringBuilder sqlSb = new SqlConnectionStringBuilder(ConnStr);
                string userId = sqlSb.UserID;  //获取登录名
                string password = sqlSb.Password; //获取数据库密码
                string ServerIP = conn.DataSource; //获取数据库连接IP
                string DataBase = conn.Database; //获取数据库名字           
 
                var files = Directory.GetFiles(FilePath, "*.sql");
                foreach (var it in files)
                {
                    string sqlQuery = "sqlcmd.exe -U  " + userId + "  -P " + password + " -S " + ServerIP + " -d " + DataBase + " -i " + it + " ";
                    string result = ExeCommand(sqlQuery);
                }
            }
        }
        /// <summary>
        /// 执行sql语句方法
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <returns></returns>
        public static string ExeCommand(string commandText)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            string strOutput = null;
            try
            {
                p.Start();
                p.StandardInput.WriteLine(commandText);
                p.StandardInput.WriteLine("exit");
                strOutput = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
            }
            catch (Exception e)
            {
                strOutput = e.Message;
            }
            return strOutput;
        }
        #endregion

        #region 方法二：
        public static int ExecuteSqlScript(string sqlFile)
        {
            int returnValue = -1;
            int sqlCount = 0, errorCount = 0;
            if (!File.Exists(sqlFile))
            {
                Log4netHelper.Error("sql file not exists!\t" + sqlFile);
                return -1;
            }
            using (StreamReader sr = new StreamReader(sqlFile))
            {
                string line = string.Empty;
                char spaceChar = ' ';
                string newLIne = "\r\n", semicolon = ";";
                string sprit = "/", whiffletree = "-";
                string sql = string.Empty;
                do
                {
                    line = sr.ReadLine();
                    // 文件结束
                    if (line == null) break;
                    // 跳过注释行
                    if (line.StartsWith(sprit) || line.StartsWith(whiffletree)) continue;
                    // 去除右边空格
                    line = line.TrimEnd(spaceChar);
                    sql += line;
                    // 以分号(;)结尾，则执行SQL
                    if (sql.EndsWith(semicolon))
                    {
                        try
                        {
                            sqlCount++;
                            SQLiteHelper.ExecuteNonQuery(sql, null);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            Log4netHelper.Error(ex);
                        }
                        sql = string.Empty;
                    }
                    else
                    {
                        // 添加换行符
                        if (sql.Length > 0) sql += newLIne;
                    }
                } while (true);
            }
            if (sqlCount > 0 && errorCount == 0)
                returnValue = 1;
            if (sqlCount == 0 && errorCount == 0)
                returnValue = 0;
            else if (sqlCount > errorCount && errorCount > 0)
                returnValue = -1;
            else if (sqlCount == errorCount)
                returnValue = -2;
            return returnValue;
        }
        #endregion
    }
}
