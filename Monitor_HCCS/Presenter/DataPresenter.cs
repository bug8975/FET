using Monitor_HCCS.Model;
using Monitor_HCCS.View;
using System;
using Monitor_HCCS.Service;
using Monitor_HCCS.Common;
using System.Threading.Tasks;
using Monitor_HCCS.Common.GeoProspector;
using System.Timers;
using System.Text;
using System.Threading;
using System.Data;

namespace Monitor_HCCS.Presenter
{
    public class DataPresenter
    {
        //内部委托事件：刷新chart
        delegate void InsertDBAndDatHandler();

        private IDataView _DataForm { get; set; }
        private IDataRepository _DataRepository { get; set; }
        private BLECode bluetooth { get; set; }
        private Data _Data { get; set; }
        private bool isRePaintPic = true;
        public bool IsRePaintPic { get { return isRePaintPic; } set { value = isRePaintPic; } }
        private string ChipID { get; set; }

        private int pointNum = 0;
        public int PointNum { get { return pointNum; } set { value = pointNum; } }

        public DataPresenter(IDataView dataForm)
        {
            _DataForm = dataForm;
            _Data = new Data();
            bluetooth = BLECode.GetIntance;
            _DataRepository = new DataRepository();

            _DataForm.Load_DataForm += Load_Data;
            _DataForm.AddButton += AddButton;
            _DataForm.DeleteButton += DeleteButton;
            _DataForm.PaintButton += PaintButton;

            bluetooth.ValueChanged += Bluetooth_ValueChanged;

            _Data.ValueChangedInPercentage += Data_ProgressValueChanged;

            //定时刷新电量事件
            System.Timers.Timer clock = new System.Timers.Timer(1000 * 60 * 3);
            clock.Elapsed += Timer_Electric;
            clock.AutoReset = true;
            clock.Enabled = true;
        }


        //加载
        private void Load_Data(object sender, EventArgs e)
        {

        }

        //测量
        private void AddButton(object sender, EventArgs e)
        {
            if (bluetooth.CurrentService == null)
            {
                _DataForm.ShowToast("提示", "蓝牙未连接，请先连接蓝牙", 1);
                return;
            }

            try
            {
                if (!_DataRepository.FindFETByName(_DataForm.InfoName, XmlHelper.getValue("flag"), _DataForm.Gears))
                {
                    _DataForm.ShowToast("提示", "当前测线项目不支持该设备，请重新创建测线项目", 3);
                    return;
                }                
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return;
            }


            _DataRepository.ReadComond(_DataForm.InfoName);
            //ThreadPool.QueueUserWorkItem((obj) =>
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        Thread.Sleep(1000);
            //        _DataRepository.ReadComond(_DataForm.InfoName);
            //    }
            //});
            
        }

        //绘图
        private void PaintButton(object sender, EventArgs e)
        {
            //Task.Run(new Action(async () => { await RePaintDGV(); }));
            if (_Data.HZCount < 10)
            {
                _DataForm.ShowToast("提示", "测量数据不足10条不能绘图", 1);
                return;
            }

            pointNum = _DataRepository.getPointCount(_DataForm.InfoName).Result;
            if (SQLiteHelper.IsRePaint(_DataForm.InfoName))
            {
                
                

                #region 查询当前蓝牙的型号   &&  档位
                string fet = "";
                string sql = "select fet from info where name = '" + _DataForm.InfoName + "'";
                DataTable dt1 = SQLiteHelper.ExecuteDataTable(sql, null);
                if (dt1.Rows.Count != 0)
                    fet = dt1.Rows[0].ItemArray[0].ToString();

                string sql1 = "select gears from info where name = '" + _DataForm.InfoName + "'";
                DataTable dt = SQLiteHelper.ExecuteDataTable(sql1, null);
                if (dt.Rows.Count == 0)
                    return;

                string gears = dt.Rows[0].ItemArray[0].ToString();
                if (gears.Contains("wt"))
                    gears = gears.Split(new char[] { 'w', 't', 'a' })[2];
                else if(gears.Contains("g"))
                    gears = gears.Split(new char[] { 'g', 'p' })[1];
                #endregion
                //_DataForm.ShowToast("提示", "正在成图中，请稍后。。。", 2);

                #region 新增progressbar进度条显示
                //_DataForm.Visable();
                //显示绘图过程进度条
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(50);
                        _DataForm.PaintProgressBar(i);
                    }
                });
                #endregion

                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    if (!GEOHelper.Surfer(_Data.Name, pointNum, fet, gears))
                    {
                        _DataForm.ShowToast("提示", "成图失败", 2);
                        return;
                    }

                    
                });
                //修改是否重新成图的状态
                //isRePaintPic = true;
                _DataForm.SetPaintButtonText("查看");
                SQLiteHelper.UpdateRePaint(_DataForm.InfoName, "0");
                //_DataForm.ShowToast("提示", "成图成功！", 1);
                return;
            }

            _DataForm.SetPaintButtonText("查看");
            PictureForm _pictureForm = new PictureForm();
            _pictureForm.PictureName = _Data.Name;
            //_pictureForm.InfoSite = _DataForm.Site;
            _pictureForm.Show();

            
        }

        //删除
        private void DeleteButton(object sender, EventArgs e)
        {
            _Data.Name = _DataForm.InfoName;
            //删除原始DAT文件的最后一行，传入参数为文件名称
            if (!_DataRepository.DeleteDat(_Data.Name))
            {
                _DataForm.ShowToast("警告", "删除失败", 1);
                return;
            }
            //删除数据库wt300a表的最后一条记录，传入参数为wt.ID
            if (!_DataRepository.DeleteDataRow(_DataForm))
            {
                _DataForm.ShowToast("警告", "删除失败", 1);
                return;
            }

            DatToGEODat();
            ThreadPool.QueueUserWorkItem((obj) => { Task.Run(async () => { await RePaintDGV(); }); });
            ThreadPool.QueueUserWorkItem((obj) => { Task.Run(async () => { await RePaintChart(); }); });

            //修改是否重新成图的状态
            //isRePaintPic = true;
            _DataForm.SetPaintButtonText("一键成图");
            SQLiteHelper.UpdateRePaint(_DataForm.InfoName, "1");

            _DataForm.ShowToast("提示", "删除成功", 1);
        }

        //电量服务
        private void Timer_Electric(object sender, ElapsedEventArgs e)
        {
            if (bluetooth.CurrentService != null)
                _DataRepository.ElectricService();
        }


        //测线基本信息
        public async Task LineInfos(string name)
        {
            Info info = _DataRepository.FindLineInfo(name);
            if (info != null)
                _DataForm.SetLineInfo(info);
        }

        //chart曲线图归一化数据源，通过读取归一化dat文件 && 刷新曲线图
        public async Task RePaintChart()
        {
            _Data.Name = _DataForm.InfoName;
            //Thread.Sleep(100);
            if (!_DataRepository.GetChartData(_Data))
            {
                await _DataForm.ShowChart_InData(null);
                return;
            }

            await _DataForm.ShowChart_InData(_Data.ChartDataSource);

        }

        //原始数据dat =》 归一化dat
        private async Task<bool> DatToGEODat()
        {
            _Data.Name = _DataForm.InfoName;
            if (!_DataRepository.SetChartData(_Data))
            {
                //没数据  && 重置Chart
                await _DataForm.ShowChart_InData(null);
                return false;
            }

            return true;
        }

        //DGV表格 && 数据库查找HZ表所有记录  && 刷新表格
        public async Task RePaintDGV()
        {
            //数据库超找DGV数据
            _Data.Name = _DataForm.InfoName;
            if (!_DataRepository.FindAllHZ(_Data))
            {
                //没数据  &&  重置DGV
                await _DataForm.DataSourceService(null);
                return;
            }

            //DGV绑定数据
            await _DataForm.DataSourceService(_Data.GridDataSource);
        }

        //监听蓝牙数据事件
        private void Bluetooth_ValueChanged(MsgType type, string str, byte[] data = null)
        {
            try
            {
                if (str.Equals("Unreachable"))
                {
                    _DataForm.ShowToast("提示", "蓝牙无法连接", 2);
                    _DataForm.SetBluetoothLEText("未连接", System.Drawing.Color.Black);
                    return;
                }

                if (str.Equals("Disconnected"))
                {
                    _DataForm.ShowToast("提示", "蓝牙断开连接", 2);
                    _DataForm.SetBluetoothLEText("未连接", System.Drawing.Color.Black);
                    return;
                }

                if (str.Equals("Success"))
                {
                    _DataForm.ShowToast("提示", "蓝牙连接成功", 2);
                    //_DataForm.SetBluetoothLEText("已连接", System.Drawing.Color.BlueViolet);

                    //跳转到测线界面
                    //_DataForm.TransToLine();
                    System.Threading.Thread.Sleep(1000);
                    //立即查询电量
                    if (bluetooth.CurrentService != null)
                        _DataRepository.ElectricService();

                    
                    return;
                }

                byte[] b = ConvertHelper.StringToByte(str);
                //电量
                if (str.Length == 44 && b[1] == 0x03)
                {
                    int Quantity = Convert.ToInt32(b[8]);
                    _DataForm.ElectricQuantity_InData(Quantity);

                    //显示蓝牙型号、版本号
                    byte[] bt = new byte[4];
                    Array.Copy(b, 3, bt, 0, 4);
                    string version = Encoding.ASCII.GetString(bt);

                    #region 版本号写入数据库
                    //XmlHelper.setVer(version);
                    #endregion

                    //设置版本号
                    _DataForm.Version_InData(version);

                    //刷新深度选择下拉框的值
                    _DataForm.ReComb();

                    return;
                }

                //测量采集数据
                if (str.Length == 23 && str.Contains("01-06-10-30"))
                {
                    _Data.Percentage = Convert.ToInt32(b[5]);
                    //清空采集数据缓存
                    if (_Data.Percentage == 100)
                        _Data.SendByList.Clear();
                    return;
                }

                //采集数据插入数据库
                if (b[1] == 0x04)
                {
                    _Data.Bledata = ConvertHelper.StringToByte(str);
                    _Data.SendByList.AddRange(_Data.Bledata);
                    int num = Convert.ToInt32(XmlHelper.getValue(_DataForm.InfoName, "dataCount"));
                    if (_Data.SendByList.Count > num)
                    {
                        _Data.SendByList = new System.Collections.Generic.List<byte>();
                        return;
                    }
                    if (_Data.SendByList.Count == num)
                        InsertDBAndDat();
                    return;
                }

                if (!str.Contains("01-06-11-00") && !(str.Length == 86 && str.Contains("01-03")))
                {
                    _Data.Bledata = ConvertHelper.StringToByte(str);
                    _Data.SendByList.AddRange(_Data.Bledata);
                    int num = Convert.ToInt32(XmlHelper.getValue("dataCount"));
                    if (_Data.SendByList.Count > num)
                    {
                        _Data.SendByList = new System.Collections.Generic.List<byte>();
                        return;
                    }
                    if (_Data.SendByList.Count == num)
                        InsertDBAndDat();
                    return;
                }
            }
            catch (InvalidOperationException ex)
            {
                Log4netHelper.Error(ex);
                return;
            }
        }

        //进度条值改变事件
        private void Data_ProgressValueChanged(object sender, EventArgs e)
        {
            if (bluetooth.CurrentService == null)
                return;

            //System.Threading.Thread.Sleep(100);
            _DataForm.ShowProgress_InData(_Data.Percentage);
        }

        //采集进度已完成，进行CRC校验
        private void InsertDBAndDat()
        {
            watch8.Start();
            
            if (_Data.SendByList == null || _Data.SendByList.Count == 0)
                return;
            watch4.Start();
            if (!_DataRepository.ModbusCRC(_Data))
                return;
            watch4.Stop();

            #region 测试一

            watch5.Start();
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                Task.Run(async () =>
                {
                    if (!_DataRepository.AppendDat(_Data))
                        return;
                    watch1.Start();
                    DatToGEODat();
                    watch1.Stop();
                    watch2.Start();
                    await RePaintChart();
                    watch2.Stop();
                });
            });

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                if (!_DataRepository.InsertHZToDB(_Data))
                    return;
                watch3.Start();
                Task.Run(async () => { await RePaintDGV(); });
                watch3.Stop();
            });
            watch5.Stop();

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                _Data.SendByList = new System.Collections.Generic.List<byte>();
                //修改是否重新成图的状态
                //isRePaintPic = true;
                _DataForm.SetPaintButtonText("一键成图");
                SQLiteHelper.UpdateRePaint(_DataForm.InfoName, "1");
                System.Media.SystemSounds.Asterisk.Play();
                //隐藏已测量的项目的深度选择功能
                _DataForm.ComboxVisible(false);
                _DataForm.ShowToast("提示", "测量成功", 1);
            });
           
            watch8.Stop();

            Console.WriteLine("DatToGEODat方法运行时间：" + watch1.Elapsed.TotalMilliseconds);
            Console.WriteLine("RePaintChart方法运行时间：" + watch2.Elapsed.TotalMilliseconds);
            Console.WriteLine("RePaintDGV方法运行时间：" + watch3.Elapsed.TotalMilliseconds);
            Console.WriteLine("MODBUS运行时间：" + watch4.Elapsed.TotalMilliseconds);
            Console.WriteLine("InsertHZToDB和AppendDat运行时间：" + watch5.Elapsed.TotalMilliseconds);
            Console.WriteLine("总运行时间：" + watch8.Elapsed.TotalMilliseconds);
            watch1.Reset();
            watch2.Reset();
            watch3.Reset();
            watch4.Reset();
            watch5.Reset();

            watch8.Reset();
            #endregion
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
