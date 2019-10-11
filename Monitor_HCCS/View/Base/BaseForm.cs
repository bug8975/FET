using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Monitor_HCCS.View.Base
{
    public partial class BaseForm : DevExpress.XtraEditors.XtraForm
    {
        public BaseForm()
        {
            InitializeComponent();
        }

        //Toast服务
        public void ShowToast(string title, string body, int time)
        {
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() => { new ToastNotifications.Notification(title, body, time, ToastNotifications.FormAnimator.AnimationMethod.Slide, ToastNotifications.FormAnimator.AnimationDirection.Up).Show(); }));
        }
    }
}