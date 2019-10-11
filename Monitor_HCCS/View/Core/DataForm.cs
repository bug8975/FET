using BenNHControl;
using Monitor_HCCS.Presenter;
using System;
using System.Drawing;
using System.Windows.Forms;
using Monitor_HCCS.Common;
using Monitor_HCCS.Common.UIDataSource;
using DevExpress.XtraCharts;
using System.Threading.Tasks;
using Monitor_HCCS.Service;
using DevExpress.XtraSplashScreen;
using Monitor_HCCS.View.Core;
using Monitor_HCCS.View.Base;
using System.Collections.Generic;
using System.Threading;
using System.Data;

namespace Monitor_HCCS.View
{
    public partial class DataForm : DevExpress.XtraEditors.XtraForm, IDataView
    {
        private DataPresenter dataPresenter;
        BLECode bluetooth = BLECode.GetIntance;

        public string InfoName { get; set; }
        public string lastId { get; set; }
        public string Gears { get; set; }

        //public object GridDataSource { get { return gridControl1.DataSource; } set { gridControl1.DataSource = value; } }

        //1.声明自适应类实例  
        AutoSizeFormClass asc = new AutoSizeFormClass();
        //定义一个用于保存静态变量的实例
        private static DataForm instance = null;
        //定义一个保证线程同步的标识
        private static readonly object locker = new object();
        public static DataForm GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new DataForm();
                }
                return instance;
            }
            return instance;
        }
        private DataForm()
        {
            InitializeComponent();
            dataPresenter = new DataPresenter(this);

        }
        //自定义委托
        delegate void ShowProgressCallback_InData(int Percentage);
        delegate Task<bool> ShowChartCallback_InData(double[][] chartData);
        //转交DataPresenter的委托
        public event EventHandler Load_DataForm;
        public event EventHandler AddButton;
        public event EventHandler DeleteButton;
        public event EventHandler PaintButton;
        public event EventHandler Click_InData;

        #region 按钮事件

        //重写show方法
        public new void Show()
        {
            base.Show();
        }
        //加载
        private void DataForm_Load(object sender, EventArgs e)
        {
            gridView1.IndicatorWidth = 70;

            Load_DataForm.Invoke(sender, EventArgs.Empty);
        }

        //连接蓝牙
        private void Bluetooth_Click(object sender, EventArgs e)
        {
            if (bluetooth.CurrentService != null)
            {
                ShowToast("提示", "蓝牙已连接", 1);
                return;
            }

            BLE fb = new BLE();
            fb.InfoName = this.InfoName;
            fb.ShowDialog();
            fb.Dispose();
        }
        //开始测量
        private void Measure_Click(object sender, EventArgs e)
        {
            AddButton.Invoke(sender, EventArgs.Empty);
        }
        //删除测点
        private void DeletePoint_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                lastId = gridView1.GetRowCellValue(gridView1.RowCount - 1, gridView1.Columns[0]).ToString();
                DialogResult dr = MessageBoxEX.Show("提示", "确认删除最后一条数据吗？");
                if (dr == DialogResult.OK)
                    DeleteButton.Invoke(sender, EventArgs.Empty);
                return;
            }

            ShowToast("提示", "暂无数据可删除！", 2);
        }
        //一键绘图
        private void Painting_Click(object sender, EventArgs e)
        {
            PaintButton.Invoke(sender, EventArgs.Empty);
        }
        //档位深度选择
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Gears = comboBox1.SelectedItem.ToString();
        }

        #endregion

        #region 数据绑定  && UI状态服务

        //深度选择下拉框隐藏或显示服务
        public void ComboxVisible(bool b)
        {
            if (IsHandleCreated && InvokeRequired)
                this.Invoke(new Action(() => { comboBox1.Visible = b; }));
        }

        //曲线图chart服务
        public async Task ShowChart_InData(double[][] ds)
        {
            #region 异步优化
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action<object>(delegate (object arg)
                    {
                        watch1.Start();
                        chartControl1.Series.Clear();
                        List<Series> seriesList = new List<Series>();
                        List<SeriesPoint> pointList = new List<SeriesPoint>();
                        //如果没有数据，重置chart数据源
                        if (ds == null || ds.Length == 0)
                            return;
                        DevExpress.XtraCharts.Series series = null;
                        SeriesPoint point = null;
                        int num = Convert.ToInt32(XmlHelper.getValue(InfoName, "number"));
                        chartControl1.BeginInit();
                        for (int i = 0; i < num; i++)
                        {
                            series = new DevExpress.XtraCharts.Series(i.ToString(), ViewType.Spline);
                            pointList.Clear();

                            for (int j = 0; j < ds.Length; j++)
                            {
                                point = new SeriesPoint(j + 1, ds[j][i]);
                                pointList.Add(point);
                            }
                            series.Points.AddRange(pointList.ToArray());
                            seriesList.Add(series);
                        }
                        chartControl1.Series.AddRange(seriesList.ToArray());
                        chartControl1.EndInit();
                        watch1.Stop();
                        Console.WriteLine("Chart方法运行时长为：" + watch1.Elapsed.TotalMilliseconds);
                        watch1.Reset();
                    }), new object[] { null });
                }
            });
            #endregion

            return;
        }

        //Toast服务
        public void ShowToast(string title, string body, int time)
        {
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() => { new ToastNotifications.Notification(title, body, time, ToastNotifications.FormAnimator.AnimationMethod.Slide, ToastNotifications.FormAnimator.AnimationDirection.Up).Show(); }));
        }

        //Progress服务
        public void ShowProgress_InData(int Percentage)
        {

            if (this.InvokeRequired)
            {
                while (this.IsHandleCreated == false)
                {
                    if (this.Disposing || this.IsDisposed)
                        return;
                }
                ShowProgressCallback_InData sp = new ShowProgressCallback_InData(ShowProgress_InData);
                this.Invoke(sp, new object[] { Percentage });
            }
            else
            {
                progressBarControl1.Visible = true;
                simpleButton4.Visible = false;
                progressBarControl1.Position = Percentage;

                if (progressBarControl1.Position == progressBarControl1.Properties.Maximum)
                {
                    progressBarControl1.Position = 0;
                    progressBarControl1.Visible = false;
                    simpleButton4.Visible = true;
                }
            }
        }

        //DGV数据绑定服务
        public async Task DataSourceService(object dataSource)
        {
            Thread.Sleep(100);
            #region 异步渲染GridControl
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action<object>(delegate (object arg)
                    {
                        gridView1.Columns.Clear();
                        //GridDataSource = dataSource;
                        gridControl1.DataSource = dataSource;
                        //隐藏第一列的wt300a的ID
                        if (dataSource != null)
                        {
                            if (gridView1.Columns.Count == 0)
                                return;

                            gridView1.Columns[0].Visible = false;
                            gridView1.Columns[1].BestFit();
                            gridView1.Columns[gridView1.Columns.Count - 1].BestFit();
                            //gridView1.RefreshData();
                        }
                    }), new object[] { null });
                }
            });
            #endregion

            #region 渲染GridControl常规方法

            //this.Invoke(new Action(() =>
            //{
            //    gridView1.Columns.Clear();
            //    //GridDataSource = dataSource;
            //    gridControl1.DataSource = dataSource;
            //    //隐藏第一列的wt300a的ID
            //    if (dataSource != null)
            //    {
            //        if (gridView1.Columns.Count == 0)
            //            return;

            //        gridView1.Columns[0].Visible = false;
            //        gridView1.Columns[gridView1.Columns.Count - 1].BestFit();
            //        //gridView1.RefreshData();
            //    }

            //}));

            #endregion
        }

        //电量进度条服务
        public void ElectricQuantity_InData(int percentage)
        {
            if (this.IsHandleCreated)
                this.Invoke(new Action(() => { labelControl12.Text = percentage.ToString() + "%"; }));
        }

        //蓝牙信息： 型号、版本号
        public void Version_InData(string version)
        {
            if (this.IsHandleCreated)
                this.Invoke(new Action(() => { labelControl11.Text = version; labelControl4.Text = XmlHelper.getVer("DEVICE"); }));
        }

        //绘图按钮文本值服务
        public void SetPaintButtonText(string msg)
        {
            //simpleButton3.BackgroundImage = image;
            //simpleButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            if (this.IsHandleCreated)
                this.Invoke(new Action(() => { simpleButton3.Text = msg; }));
        }

        //设置测线的基本信息
        public void SetLineInfo(Monitor_HCCS.Model.Info info)
        {
            this.Invoke(new Action(() =>
            {
                labelControl6.Text = info.Name;
                labelControl8.Text = info.Distance + "米";
                labelControl10.Text = info.Site;
            }));
        }

        //设置蓝牙连接状态的文本信息
        public void SetBluetoothLEText(string text, Color color)
        {
            this.Invoke(new Action(() => { labelControl4.Text = text; labelControl4.ForeColor = color; }));
        }

        //GridControl加行号事件
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #region 绘图进度条服务
        public void Visable()
        {
            if (InvokeRequired && IsHandleCreated)
            {
                this.BeginInvoke(new Action(() =>
                {
                    progressBarControl2.Visible = true;
                    simpleButton3.Visible = false;
                    //Application.DoEvents();
                }));
            }
        }

        public void PaintProgressBar(int position)
        {

            if (position == 0)
            {
                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        progressBarControl2.Visible = true;
                        simpleButton3.Visible = false;
                        Application.DoEvents();
                    }));
                }
            }
            

            if (position == 99)
            {
                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        progressBarControl2.Visible = false;
                        simpleButton3.Visible = true;
                    }));
                }
            }

            if (position < 99)
            {
                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        progressBarControl2.Position = position;
                        
                    }));
                }
                else
                {
                    progressBarControl2.Position = position;
                }
            }
            Application.DoEvents();
        }
        #endregion

        #endregion

        #region 新功能 连接蓝牙跳转到测线界面
        public void TransToLine()
        {
            Click_InData.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region 闪屏  &&  等待窗体
        private SplashScreenManager _loadForm;
        /// <summary>
        /// 等待窗体管理对象
        /// </summary>
        protected SplashScreenManager LoadForm
        {
            get
            {
                if (_loadForm == null)
                {
                    this._loadForm = new SplashScreenManager(this, typeof(WaitForm1), true, true);
                    this._loadForm.ClosingDelay = 0;
                }
                return _loadForm;
            }
        }
        /// <summary>
        /// 显示等待窗体
        /// </summary>
        public void ShowMessage()
        {
            bool flag = !this.LoadForm.IsSplashFormVisible;
            if (flag)
            {
                this.LoadForm.ShowWaitForm();
            }
        }
        /// <summary>
        /// 关闭等待窗体
        /// </summary>
        public void HideMessage()
        {
            bool isSplashFormVisible = this.LoadForm.IsSplashFormVisible;
            if (isSplashFormVisible)
            {
                this.LoadForm.CloseWaitForm();
            }
        }

        #endregion

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

        #region 闪屏
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}
        #endregion

        #region 异步刷新DataForm
        public void ReFlush()
        {
            #region 异步GridControl刷新方法

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                Thread.Sleep(10);
                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action<object>(delegate (object arg)
                    {
                        //隐藏第一列的wt300a的ID
                        if (gridControl1.DataSource != null)
                        {
                            gridView1.Columns[0].Visible = false;
                            gridView1.Columns[gridView1.Columns.Count - 1].BestFit();
                            //gridView1.RefreshData();
                        }
                    }), new object[] { null });
                }
            });

            #endregion

            #region 异步后台刷新方法1

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                Task.Run(async () =>
                {
                    await dataPresenter.RePaintChart();
                });
            });

            #endregion

            #region 异步后台刷新方法2

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                Task.Run(async () =>
                {
                    await dataPresenter.RePaintDGV();
                });
            });

            #endregion

            #region 异步后台刷新方法3

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                Task.Run(async () =>
                {
                    await dataPresenter.LineInfos(this.InfoName);
                });
            });

            #endregion

            #region 异步后台刷新方法4   初始化绘图文本“查看”&&“一键成图”

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                string text = "";
                if (SQLiteHelper.IsRePaint(InfoName))
                    text = "一键成图";
                else
                    text = "查看";

                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action<object>(delegate(object arg)
                    {
                        simpleButton3.Text = text;
                    }), new object[] { null });
                }
            });

            #endregion            

            //异步刷新深度选择下拉框
            ReComb();
        }
        #endregion

        #region 深度选择  &&  刷新
        public void ReComb()
        {
            #region 异步后台刷新方法5   初始化深度选择下拉框显示状态

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                if (InfoName == null || InfoName.Equals(""))
                    return;
                string sql = "select isGears, fet from info where name = '" + InfoName + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql, null);
                if (dt.Rows.Count == 0)
                    return;
                bool isVisible = dt.Rows[0].ItemArray[0].ToString().Equals("1");
                string fet = dt.Rows[0].ItemArray[1].ToString();
                List<string> ds = new List<string>();

                if (InvokeRequired && IsHandleCreated)
                {
                    this.BeginInvoke(new Action<object>(delegate(object arg)
                    {
                        //当前版本号是否已读取
                        if (labelControl11.Text == "")
                            return;

                        //比较版本，是否显示深度选择
                        float NowVersion = Convert.ToSingle(labelControl11.Text.Remove(0, 1));
                        if (NowVersion < 7.2f)
                            return;
                        
                        //深度选择显示与否
                        if (!isVisible)
                            return;
                        comboBox1.Visible = isVisible;
                        
                        if (fet.Equals("") && BLECode.GetIntance.CurrentService != null)
                            fet = XmlHelper.getValue("flag");

                        foreach (KeyValuePair<string, string[]> kvp in ComboxDSHelper.DepthMap)
                        {
                            if (kvp.Key.Equals(fet))
                            {
                                for (int i = 0; i < kvp.Value.Length; i++)
                                {
                                    if (kvp.Value[i].Contains("wt"))
                                    {
                                        string s = kvp.Value[i].Split(new char[] { 'w', 't', 'a' })[2] + "米";
                                        ds.Add(s);
                                    }
                                    else
                                    {
                                        string s = kvp.Value[i].Split(new char[] { 'g', 'p' })[1] + "米";
                                        ds.Add(s);
                                    }
                                }
                            }
                        }
                        comboBox1.DataSource = ds;
                    }), new object[] { null });
                }
            });

            #endregion
        }
        #endregion
    }
}
