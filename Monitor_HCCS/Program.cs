using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.Threading;
using System.Reflection;
using System.Configuration;
using Monitor_HCCS.View;
using Monitor_HCCS.View.Dialog;
using Monitor_HCCS.Common;
using Monitor_HCCS.Common.SQL;
using DevExpress.XtraSplashScreen;
using System.IO;

namespace Monitor_HCCS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //界面汉化
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
                BonusSkins.Register();
                SkinManager.EnableFormSkins();
                UserLookAndFeel.Default.SetSkinStyle("Money Twins");

                #region 执行脚本
                string filename = "measure.sql";
                if (File.Exists(filename))
                    if (Script.ExecuteSqlScript(filename) == 1)
                        File.Delete(filename);
                #endregion





                //新增
                SplashScreenManager.ShowForm(typeof(SplashScreen1));
                //Thread.Sleep(7000);

                Application.Run(View.MainForm.GetInstance());
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
            }

        }
    }
}