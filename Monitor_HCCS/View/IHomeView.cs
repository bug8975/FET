using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Presenter;

namespace Monitor_HCCS.View
{
    public interface IHomeView
    {
        event EventHandler LoadHome;
        event EventHandler SaveInfo;
        event EventHandler VerifyInfoName;
        event EventHandler VerifyIncrement;
        event EventHandler VerifyDistance;
        event EventHandler VerifyInfoSite;
        event EventHandler Click_InMain;

        string InfoName { get; set; }
        HomePresenter HomePresenter { get; set; }
        string Increment { get; set; }
        string Distance { get; set; }
        string InfoSite { get; set; }

        void ReFlush();

        /// <summary>
        /// Toast服务
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="time"></param>
        void ShowToast(string title, string body, int time);

    }
}
