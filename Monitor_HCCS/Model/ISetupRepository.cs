using Monitor_HCCS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Model
{
    public interface ISetupRepository
    {
        /// <summary>
        /// 验证ID && 开始下载
        /// </summary>
        /// <param name="fwud"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        string VerifyID(DownloadFile fwud, string str);

        /// <summary>
        /// 软件升级
        /// </summary>
        /// <returns></returns>
        bool SoftwareUpdata();

        /// <summary>
        /// 读芯片ID
        /// </summary>
        /// <returns></returns>
        bool ReadChipId();

        /// <summary>
        /// 向物探仪发送程序升级命令
        /// </summary>
        /// <returns></returns>
        bool StartFirmUpdata();
    }
}
