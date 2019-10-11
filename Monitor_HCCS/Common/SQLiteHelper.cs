using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Monitor_HCCS.Common
{
    public class SQLiteHelper
    {
        //private static string connectionString = ConfigurationManager.ConnectionStrings["SqliteConn"].ToString();
        private static string connectionString = @"Data Source=measure.db;foreign keys=true;";
        /// <summary> 
        /// 对SQLite数据库执行增删改操作，返回受影响的行数
        /// </summary> 
        /// <param name="sql">要执行的增删改的SQL语句</param> 
        /// <param name="parameters">执行增删改语句的参数</param> 
        /// <returns></returns> 
        public static int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sql;
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        affectedRows = command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            return affectedRows;
        }
        /// <summary> 
        /// 执行一个查询语句，返回SQLiteDataReader实例 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行查询语句的参数</param> 
        /// <returns></returns> 
        public static SQLiteDataReader ExecuteReader(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }                                  
        }
        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行查询语句的参数</param> 
        /// <returns></returns> 
        public static DataTable ExecuteDataTable(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }
        /// <summary> 
        /// 执行一个查询语句，返回查询结果的第一行第一列 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public static Object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }
        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行查询语句的参数</param> 
        /// <returns></returns> 
        public static DataTable ExecuteDataTable(string sql)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }

        //修改是否需要重新制图
        public static int UpdateRePaint(string name,string flag)
        {
            string sql = "UPDATE info SET isRePaint = '" + flag + "' WHERE name = '" + name + "'";
            return ExecuteNonQuery(sql, null);
        }

        //查询是否需要重新制图
        public static bool IsRePaint(string name)
        {
            string sql = "SELECT isRePaint FROM info WHERE name = '" + name + "'";
            DataTable dt = ExecuteDataTable(sql, null);
            if (dt.Rows.Count == 0)
                return true;
            string isRePaint = dt.Rows[0].ItemArray[0].ToString();
            if (string.Equals(isRePaint,"0"))
                return false;

            return true;
        }

    }
}
