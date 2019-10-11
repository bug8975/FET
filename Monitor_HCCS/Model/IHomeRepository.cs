using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Model
{
    public interface IHomeRepository
    {
        /// <summary>
        /// 保存测线
        /// </summary>
        /// <param name="home"></param>
        /// <returns></returns>
        int SaveInfo(HomeModel home);

        /// <summary>
        /// 获取所有动态列表数据源
        /// </summary>
        /// <returns></returns>
        Info[] GetInfos();

        /// <summary>
        /// 电量
        /// </summary>
        bool ElectricService();

        /// <summary>
        /// 验证数据库中是否有重复的测线名称
        /// </summary>
        /// <param name="tempName"></param>
        /// <returns></returns>
        int GetInfoByName(string tempName);

        /// <summary>
        /// 删除一条测线
        /// </summary>
        /// <param name="info_name"></param>
        /// <returns></returns>
        bool DeleteInfo(string info_name);


        /// <summary>
        /// 通过设备名字找到FET字母代号，当前的设备是否匹配该测线项目
        /// </summary>
        /// <param name="name">动态按钮上显示的测线名称</param>
        /// <param name="fet">设备字符代码</param>
        /// <returns></returns>
        bool FindFETByName(string name, string fet);

        /// <summary>
        /// 发送固件升级命令
        /// </summary>
        void ReadComond();
    }
}
