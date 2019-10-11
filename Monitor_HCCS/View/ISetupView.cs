using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.View
{
    public interface ISetupView
    {
        event EventHandler FirmwareUpdata;
        event EventHandler SoftwareUpdata;

        // Toast服务
        void ShowToast_InSetup(string title, string body, int time);

        //下载进度条服务
        void ProgressSer(int quantity);

        //固件更新进度条服务
        void ProgressUp(int quantity);

        //下载进度条  &&  按钮UI改变服务
        void ProgressSerChanged();

        //固件更新进度条  &&  按钮UI改变服务
        void ProgressUpChanged();

        //设置更新状态文本服务
        void SetUpText(string p);

        //设置版本号等显示内容
        void SetLabelText(string strs);

        //固件升级按钮UI状态管理
        void SetButtonUp(bool b);
    }
}
