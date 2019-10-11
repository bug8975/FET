using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Common.ThreadFrame
{
    /// <summary>
    /// Lyx 线程框架 类
    /// </summary>
    public class LyxThreadFrame
    {
        /// <summary>
        /// 主线程 签名
        /// </summary>
        public const string MainThreadIdiograph = "Main Thread";

        /// <summary>
        /// 初始化 线程检测框架
        /// <para>请在UI(主)线程下初始化</para>
        /// </summary>
        public static void Init()
        {
            var thread = System.Threading.Thread.CurrentThread;
            thread.Name = MainThreadIdiograph;
        }
    }
}
