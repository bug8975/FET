using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitor_HCCS.View
{
    public interface IDataView
    {
        string InfoName { get; }
        string lastId { get; set; }
        string Gears { get; set; }

        //object GridDataSource { get; set; }

        event EventHandler Load_DataForm;
        event EventHandler AddButton;
        event EventHandler DeleteButton;
        event EventHandler PaintButton;
        //event EventHandler CRCNotify;

        /// <summary>
        /// 绑定曲线图chart的数据源
        /// </summary>
        /// <param name="chartData"></param>
        /// <returns></returns>
        //bool GetChartDataSource(double[][] chartData);


        /// <summary>
        /// Toast服务
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="time"></param>
        void ShowToast(string title, string body, int time);

        /// <summary>
        /// 进度条服务
        /// </summary>
        /// <param name="Percentage"></param>
        void ShowProgress_InData(int Percentage);

        /// <summary>
        /// 曲线图Chart服务
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        Task ShowChart_InData(double[][] ds);

        /// <summary>
        /// 更新UI界面数据服务
        /// </summary>
        /// <param name="dataSource">Model中的数据</param>
        Task DataSourceService(object dataSource);

        /// <summary>
        /// 电量服务
        /// </summary>
        /// <param name="percentage"></param>
        void ElectricQuantity_InData(int percentage);

        /// <summary>
        /// 蓝牙设备版本号
        /// </summary>
        /// <param name="version"></param>
        void Version_InData(string version);

        /// <summary>
        /// 设置绘图按钮的文本
        /// </summary>
        /// <param name="image"></param>
        void SetPaintButtonText(string msg);

        /// <summary>
        /// 设置测线的基本信息
        /// </summary>
        /// <param name="info"></param>
        void SetLineInfo(Monitor_HCCS.Model.Info info);

        /// <summary>
        /// 设置蓝牙连接状态的文本信息
        /// </summary>
        /// <param name="text"></param>
        void SetBluetoothLEText(string text, System.Drawing.Color color);

        /// <summary>
        /// 绘图按钮隐藏，进度条显示
        /// </summary>
        void Visable();

        /// <summary>
        /// 绘图进度条服务
        /// </summary>
        void PaintProgressBar(int i);

        /// <summary>
        /// 连接蓝牙后跳转到测线界面
        /// </summary>
        void TransToLine();

        /// <summary>
        /// 深度选择下拉框显示与否
        /// </summary>
        /// <param name="b"></param>
        void ComboxVisible(bool b);

        /// <summary>
        /// 深度选择下拉框数据源
        /// </summary>
        void ReComb();
    }
}
