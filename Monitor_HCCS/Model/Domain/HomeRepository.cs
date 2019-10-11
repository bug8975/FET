using System;
using System.Collections.Generic;
using System.Data;
using Monitor_HCCS.Common;
using Monitor_HCCS.Service;

namespace Monitor_HCCS.Model
{
    public class HomeRepository : IHomeRepository
    {
        public Info[] GetInfos()
        {
            string sql = "select name from info";
            DataTable dt = SQLiteHelper.ExecuteDataTable(sql);
            List<Info> _infos = new DbTableConvertor<Info>().ConvertToList(dt);
            Info[] infos = new Info[_infos.Count];
            int i = 0;
            foreach (var info in _infos)
            {
                infos[i] = info;
                i++;
            }
            return infos;
        }

        public int SaveInfo(HomeModel home)
        {
            string sql = "insert into info(name, increment,distance,site,time,fet)values('" + home.Name + "','" + home.Increment + "','" + home.Distance + "','" + home.Site + "','" + home.Time + "','" + home.Fet + "')";
            return SQLiteHelper.ExecuteNonQuery(sql);
        }

        public bool ElectricService()
        {
            Modbus.MBConfig(0xFA, 9600);
            Modbus.MBAddCmd(0x1000, 0x03);
            BLECode.GetIntance.Write(Modbus.WBuff);

            return true;
        }

        public int GetInfoByName(string tempName)
        {
            try
            {
                string sql = "select name from info where name = '" + tempName + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return 0;
            }
        }

        public bool DeleteInfo(string info_name)
        {
            try
            {
                if (!DatHelper.DeleteBoth(info_name))
                    return false;

                string sql2 = " delete from info where name = '" + info_name + "'";
                if (SQLiteHelper.ExecuteNonQuery(sql2) == 0)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        public bool FindFETByName(string name, string fet)
        {
            try
            {
                string sql = "select fet from info where name = '" + name + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql);
                if (!dt.Rows[0].ItemArray[0].Equals(fet))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        public void ReadComond()
        {
            Modbus.MBConfig(0xFA, 9600);
            Modbus.MBAddCmd(0x1100, 0x06);
            BLECode.GetIntance.Write(Modbus.WBuff);
        }
    }
}
