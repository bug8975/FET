using Monitor_HCCS.Model;
using Monitor_HCCS.Common;
using Monitor_HCCS.View;
using System;

using DevExpress.XtraEditors;
using System.Threading;
using System.Threading.Tasks;

namespace Presenter
{
    public class HomePresenter
    {
        public IHomeView _homeForm { get; set; }
        private HomeModel _homeModel { get; set; }
        private IHomeRepository _homeRepository { get; set; }

        public HomePresenter(IHomeView view)
        {
            _homeForm = view;
            _homeModel = new HomeModel();
            _homeRepository = new HomeRepository();

            _homeForm.LoadHome += Load_Home;
            _homeForm.SaveInfo += SaveInfo;
            _homeForm.VerifyInfoName += VerifyInfoName;
            _homeForm.VerifyIncrement += VerifyIncrement;
            _homeForm.VerifyDistance += VerifyDistance;
            _homeForm.VerifyInfoSite += VerifyInfoSite;
        }

        //加载
        public void Load_Home(object sender, EventArgs e)
        {

        }
        //保存
        public void SaveInfo(object sender, EventArgs e)
        {
            string TempName = _homeForm.InfoName;
            if (TempName == null || TempName.Length == 0)
            {
                _homeForm.ShowToast("提示", "测线名称不能为空", 4);
                return;
            }
            if (_homeForm.InfoSite.Equals("") || _homeForm.InfoSite.Equals(" "))
            {
                _homeForm.ShowToast("提示", "测量地点不能为空", 4);
                return;
            }

            if (_homeRepository.GetInfoByName(TempName) > 0)
            {
                _homeForm.ShowToast("提示", "测线名称已存在，请重新命名", 4);
                _homeForm.InfoName = "";
                return;
            }

            #region 测线名称

            if (_homeForm.InfoName.Contains("'"))
            {
                _homeForm.ShowToast("提示", "格式不正确，测线名称不能输入“'”符号", 4);
                _homeForm.InfoName = "";
                return;
            }

            if (TempName.Length > 8)
            {
                _homeForm.ShowToast("提示", "测线名称长度过长，请不要输入超过7个中文字符", 4);
                _homeForm.InfoSite = "";
                return;
            }
    
            if (_homeRepository.GetInfoByName(TempName) > 0)
            {
                _homeForm.ShowToast("提示", "测线名称已存在，请重新命名", 4);
                _homeForm.InfoName = "";
                return;
            }
            #endregion

            #region 地点
            if (_homeForm.InfoSite.Length > 8)
            {
                _homeForm.ShowToast("提示", "地点名称过长，请不要输入超过7个中文字符", 4);
                _homeForm.InfoSite = "";
                return;
            }
            if (_homeForm.InfoSite.Contains("'"))
            {
                _homeForm.ShowToast("提示", "格式不正确，测量地点名称不能输入“'”符号", 4);
                _homeForm.InfoSite = "";
                return;
            }
            #endregion

            _homeModel.Name = _homeForm.InfoName;
            _homeModel.Increment = _homeForm.Increment;
            _homeModel.Distance = _homeForm.Distance;
            _homeModel.Site = _homeForm.InfoSite;


            if (_homeRepository.SaveInfo(_homeModel) == 0)
            {
                _homeForm.ShowToast("提示", "新建失败", 1);
                return;
            }

            _homeForm.ShowToast("提示", "新建成功", 1);

            //刷新
            _homeForm.ReFlush();

        }

        //测线名称 && 输入验证
        private void VerifyInfoName(object sender, EventArgs e)
        {
            string TempName = _homeForm.InfoName;
            if (TempName == null || TempName.Length == 0)
            {
                _homeForm.ShowToast("提示", "测线名称不能为空", 4);
                return;
            }

            if (_homeForm.InfoName.Contains("'"))
            {
                _homeForm.ShowToast("提示", "格式不正确，测量名称名称不能输入“'”符号", 4);
                _homeForm.InfoName = "";
                return;
            }

            if (TempName.Length > 8)
            {
                _homeForm.ShowToast("提示", "测线名称长度过长，请不要输入超过7个中文字符", 4);
                _homeForm.InfoSite = "";
                return;
            }
    
            if (_homeRepository.GetInfoByName(TempName) > 0)
            {
                _homeForm.ShowToast("提示", "测线名称已存在，请重新命名", 4);
                _homeForm.InfoName = "";
                return;
            }

        }
        //测点增量  && 输入验证
        private void VerifyIncrement(object sender, EventArgs e)
        {
            if (!StringHelper.IsMatch(_homeForm.Increment, 0))
            {
                _homeForm.ShowToast("提示", "格式不正确，测点增量必须为正整数", 4);
                _homeForm.Increment = "1";
                return;
            }

            if (Convert.ToInt32(_homeForm.Increment) > 999999)
            {
                _homeForm.ShowToast("提示", "测点增量最大为999999", 4);
                _homeForm.Increment = "1";
                return;
            }   
        }
        //点距 && 输入验证
        private void VerifyDistance(object sender, EventArgs e)
        {
            if (!StringHelper.IsMatch(_homeForm.Distance, 0))
            {
                _homeForm.ShowToast("提示", "格式不正确，点距必须为正整数", 4);
                _homeForm.Distance = "1";
                return;
            }
               
            if(Convert.ToInt32(_homeForm.Distance) > 999999)
            {
                _homeForm.ShowToast("提示", "点距最大为999999", 4);
                _homeForm.Distance = "1";
                return;
            }
        }
        //地点 && 输入验证
        private void VerifyInfoSite(object sender, EventArgs e)
        {

            string TempName = _homeForm.InfoSite;
            if (_homeForm.InfoSite.Contains("'"))
            {
                _homeForm.ShowToast("提示", "格式不正确，测量地点名称不能输入“'”符号", 4);
                _homeForm.InfoSite = "";
                return;
            }

            if (TempName.Length > 8)
            {
                _homeForm.ShowToast("提示", "测线名称长度过长，请不要输入超过7个中文字符", 4);
                _homeForm.InfoSite = "";
                return;
            }
        }

    }
}