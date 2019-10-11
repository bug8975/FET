using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Model
{
    interface IDataRepository
    {        

        /// <summary>
        /// 删除原始dat文件最后一条测点
        /// </summary>
        /// <param name="Lastid"></param>
        /// <returns></returns>
        bool DeleteDat(string Lastid);

        /// <summary>
        /// 删除数据库中HZ表最后一条测点
        /// </summary>
        /// <param name="Lastid"></param>
        /// <returns></returns>
        bool DeleteDataRow(Monitor_HCCS.View.IDataView df);

        /// <summary>
        /// 查找数据库中所有的HZ表记录
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        bool FindAllHZ(Data data);

        /// <summary>
        /// 查找测线基本信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Info FindLineInfo(string name);

        /// <summary>
        /// 获得测量的点数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<int> getPointCount(string name);

        /// <summary>
        /// 获取归一化的dat文件
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        bool GetChartData(Data data);

        /// <summary>
        /// 原始数据dat =》 归一化dat
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        bool SetChartData(Data _data);

        /// <summary>
        /// 获取表格DVG数据，通过读取原始dat文件
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        bool GetDGVData(Data _data);


        /// <summary>
        /// 测量 && 数据库新增一条测点记录
        /// </summary>
        /// <returns></returns>
        bool ReadComond(string name);

        /// <summary>
        /// 采集数据进行CRC校验
        /// </summary>
        /// <returns></returns>
        bool ModbusCRC(Data _data);

        /// <summary>
        /// 采集数据插入数据库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool InsertHZToDB(Data data);

        /// <summary>
        /// 采集数据插入原始dat文件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool AppendDat(Data data);

        /// <summary>
        /// 物探仪设备电量
        /// </summary>
        bool ElectricService();

        /// <summary>
        /// 通过设备名字找到FET字母代号，当前的设备是否匹配该测线项目
        /// </summary>
        /// <param name="name">测线名称</param>
        /// <param name="fet">设备字符代码</param>
        /// <returns></returns>
        bool FindFETByName(string name, string fet, string dbname);
    }
}
