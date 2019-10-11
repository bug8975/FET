using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Common.ThreadFrame
{
    /// <summary>
    /// 委托 扩展类
    /// </summary>
    public static class DelegateExtension
    {
        /// <summary>
        /// 在UI(主)线程中执行
        /// </summary>
        public static object SafetyInvoke(this Delegate dele, params object[] param)
        {
            var thread = System.Threading.Thread.CurrentThread;
            if (thread.IsMainThread())
            {
                return dele.DynamicInvoke(param);
            }
            else
            {
                //return Application.Current.Dispatcher.Invoke(dele, param);
                return null;
            }
        }
    }
}
