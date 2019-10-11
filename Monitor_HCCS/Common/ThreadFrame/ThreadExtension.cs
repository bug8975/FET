using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monitor_HCCS.Common.ThreadFrame
{
    /// <summary>
    /// 线程 扩展类
    /// </summary>
    public static class ThreadExtension
    {
        /// <summary>
        /// 当前线程是否是主线程
        /// </summary>
        public static bool IsMainThread(this Thread thread)
        {
            if (thread == null)
            {
                throw new ArgumentNullException("thread");
            }
            if (thread.Name == null)
            {
                return false;
            }
            return thread.Name.Equals(LyxThreadFrame.MainThreadIdiograph);
        }
    }
}
