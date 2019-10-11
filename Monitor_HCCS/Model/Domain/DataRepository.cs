using Monitor_HCCS.Common;
using Monitor_HCCS.Common.GeoProspector;
using Monitor_HCCS.Common.UIDataSource;
using Monitor_HCCS.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Model
{
    public class DataRepository : IDataRepository
    {

        /// <summary>
        /// 删除原始dat文件最后一条测点
        /// </summary>
        /// <param name="Lastid"></param>
        /// <returns></returns>
        public bool DeleteDat(string FileName)
        {
            try
            {
                return DatHelper.DeleteDat(FileName);
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 删除数据库中HZ表最后一条测点
        /// </summary>
        /// <param name="Lastid"></param>
        /// <returns></returns>
        public bool DeleteDataRow(Monitor_HCCS.View.IDataView df)
        {
            try
            {
                string sql1 = "select gears from info where name = '" + df.InfoName + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql1, null);
                if (dt.Rows.Count == 0)
                    return false;
                string dbname = dt.Rows[0].ItemArray[0].ToString();
                string sql2 = " delete from '" + dbname + "' where Id = '" + df.lastId + "'";
                if (SQLiteHelper.ExecuteNonQuery(sql2) > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 查找数据库中所有的HZ表记录
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public bool FindAllHZ(Data _data)
        {
            try
            {
                
                string sql1 = "select gears from info where name = '" + _data.Name + "'";
                DataTable dt1 = SQLiteHelper.ExecuteDataTable(sql1, null);
                if(dt1.Rows.Count != 1)
                    return false;

                string fetname = dt1.Rows[0].ItemArray[0].ToString();
                if (fetname.Equals(""))
                    return false;
                string sql = @XmlHelper.getValue(_data.Name, "select") + "'" + _data.Name + "'";
                string sql2 = string.Format(sql, fetname);
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql2, null);
                _data.HZCount = dt.Rows.Count;
                if (_data.HZCount == 0)
                    return false;

                _data.GridDataSource = dt;
                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 查找测线基本信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Info FindLineInfo(string name)
        {
            try
            {
                string sql = "select name,site,distance from Info where name = '" + name + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                    return null;

                List<Info> _infos = new DbTableConvertor<Info>().ConvertToList(dt);
                return _infos[0];
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 校验当前测线项目与物探仪型号是否适配  &&  插入当前测量的测线的蓝牙型号和保存数据的表名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fet"></param>
        /// <returns></returns>
        public bool FindFETByName(string name, string fet, string dbname)
        {
            try
            {
                string sql = "select fet from info where name = '" + name + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                    return false;
                string flag = dt.Rows[0].ItemArray[0].ToString();
                if (flag.Equals(""))
                {
                    try
                    {
                        //设置每条测线绑定的型号
                        string gears = XmlHelper.getVer("DEVICE").Remove(0,4).ToLower();

                        #region 2000A和3000A没有WT前缀，需转字符串处理
                        if (gears.Contains("2000a"))
                            gears = "wt2000a";
                        else if (gears.Contains("3000a"))
                            gears = "wt3000a";
                        #endregion

                        foreach (KeyValuePair<string, string> kvp in ComboxDSHelper.FetList)
                        {
                            if(kvp.Value.Equals(gears))
                            {
                                flag = kvp.Key;
                                break;
                            }
                        }

                        #region 档位深度调节  &&  gears对应数据库表名
                        string isGears = "select isGears from info where name = '" + name + "'";
                        DataTable dt2 = SQLiteHelper.ExecuteDataTable(isGears, null);
                        if (dt2.Rows.Count == 0)
                            return false;
                        //如果深度档位已选择，则重新绑定数据库表
                        bool isCombViable = dt2.Rows[0].ItemArray[0].ToString().Equals("1");
                        //isCombViable为true代表该测线还未测量过
                        if (isCombViable)
                        {
                            if(dbname == null || dbname.Length == 0 || dbname.Equals(""))
                            {
                                //默认不选择档位深度
                                string sql2 = "update info set fet = '" + flag + "', gears = '" + gears + "', isGears = '" + "0" + "' where name = '" + name + "'";
                                SQLiteHelper.ExecuteNonQuery(sql2, null);
                                return true;
                            }

                            dbname = dbname.Split(new char[] { '米' })[0];
                            //选择档位深度
                            foreach (KeyValuePair<string,string[]> kvp in ComboxDSHelper.DepthMap)
                            {
                                if(kvp.Key.Equals(flag))
                                {
                                    foreach (string item in kvp.Value)
                                    {
                                        if (item.Contains(dbname))
                                        {
                                            gears = item;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            string sql3 = "update info set fet = '" + flag + "', gears = '" + gears + "', isGears = '" + "0" + "' where name = '" + name + "'";
                            SQLiteHelper.ExecuteNonQuery(sql3, null);
                            return true;
                        }
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        Log4netHelper.Error(ex);
                    }
                }

                if (!flag.Equals(fet))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        //获得测量的点数
        public async Task<int> getPointCount(string name)
        {
            try
            {
                string sql1 = "select gears from info where name = '" + name + "'";
                DataTable dt1 = SQLiteHelper.ExecuteDataTable(sql1, null);
                if (dt1.Rows.Count == 0)
                    return 0;
                string dbname = dt1.Rows[0].ItemArray[0].ToString();
                string sql2 = "select count(Id) from '" + dbname + "' where info_name = '" + name + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql2);
                return Convert.ToInt32(dt.Rows[0].ItemArray[0]);
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取曲线图Chart数据，通过读取归一化dat文件
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public bool GetChartData(Data _data)
        {
            try
            {
                double[][] dt = DatHelper.ReadGEODat(_data.Name);
                _data.ChartDataSource = dt;
                if (dt == null || dt.Length == 0)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 原始数据dat =》 归一化dat
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public bool SetChartData(Data _data)
        {
            try
            {
                if (!GetDGVData(_data))
                    return false;

                //if (GEOHelper.normalizationsToFile((double[][])_data.DGVDataSource, _data.Name) == null)
                //    return false;

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 获取表格DVG数据，通过读取原始dat文件
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public bool GetDGVData(Data _data)
        {
            try
            {
                watch1.Start();
                List<double[]> dt = DatHelper.ReadDat(_data.Name);
                watch1.Stop();
                //_data.DGVDataSource = dt;
                if (dt == null || dt.Count == 0)
                    return false;
                watch2.Start();

                if (GEOHelper.normalizationsToFile(dt, _data.Name) == null)
                    return false;
                watch2.Stop();

                Console.WriteLine("DatHelper.ReadDat方法运行时长：" + watch1.Elapsed.TotalMilliseconds);
                Console.WriteLine("调用geo方法运行时长：" + watch2.Elapsed.TotalMilliseconds);
                watch1.Reset();
                watch2.Reset();

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }


        /// <summary>
        /// 测量 && 采集数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ReadComond(string name)
        {
            #region 深度档位选择
            string sql = "select fet,gears from info where name = '" + name + "'";
            DataTable dt = SQLiteHelper.ExecuteDataTable(sql, null);
            if (dt.Rows.Count == 0)
                return false;
            string fet = dt.Rows[0].ItemArray[0].ToString();
            string gears = dt.Rows[0].ItemArray[1].ToString();
            int Index = -1;
            foreach (KeyValuePair<string,string[]> kvp in ComboxDSHelper.DepthMap)
            {
                if (kvp.Key.Equals(fet))
                {
                    for (int i = 0; i < kvp.Value.Length; i++)
                    {
                        if (kvp.Value[i].Equals(gears))
                        {
                            Index = i;
                            break;
                        }
                    }
                    break;
                }
            }
            #endregion


            Modbus.MBConfig(0xFA, 9600);
            if(Index == 1)
                Modbus.MBAddCmd(0x20, 0x04);
            else if(Index == 2)
                Modbus.MBAddCmd(0x22, 0x04);
            else
                Modbus.MBAddCmd(0x16, 0x04);
            BLECode.GetIntance.Write(Modbus.WBuff);

            return true;
        }        

        /// <summary>
        /// 超时判断
        /// </summary>
        /// <returns></returns>
        //public async Task<bool> TimeTask()
        //{
        //    var task = PrimeTask();
        //    int timeout = 1000;
        //    if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        /// <summary>
        /// 调用Modbus的CRC校验处理
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public bool ModbusCRC(Data _data)
        {
            try
            {
                byte[] bt = _data.SendByList.ToArray();
                if (bt == null || bt.Length == 0)
                    return false;

                if (!Modbus.ReceiveByteArray(bt))
                    return false;

                _data.ModbusData = Modbus.double_data;

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 采集数据插入数据库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertHZToDB(Data _data)
        {
            try
            {
                StringBuilder str = new StringBuilder(1024);
                foreach (double d in _data.ModbusData)
                {
                    str.Append(d + ",");
                }
                string sql = XmlHelper.getValue(_data.Name, "insert") + "'" + _data.Name + "'" + "," + str + "'" + DateTime.Now.ToLocalTime() + "')";
                if (SQLiteHelper.ExecuteNonQuery(sql) > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 采集数据插入原始dat文件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AppendDat(Data _data)
        {
            try
            {
                if (DatHelper.AppendDat(_data.Name, _data.ModbusData))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 电量服务
        /// </summary>
        /// <returns></returns>
        public bool ElectricService()
        {
            Modbus.MBConfig(0xFA, 9600);
            Modbus.MBAddRepeatCmd(0x1000, 0x03);
            BLECode.GetIntance.Write(Modbus.WBuff);

            return true;
        }


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
    }
}
