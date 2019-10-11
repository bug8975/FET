using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_HCCS.Common
{
    public class Log4netHelper
    {
        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            Type type = MethodBase.GetCurrentMethod().DeclaringType;
            ILog m_log = LogManager.GetLogger(type);
            m_log.Info(message);
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">错误日志</param>
        //public static void Error(string message)
        //{
        //    Type type = MethodBase.GetCurrentMethod().DeclaringType;
        //    ILog m_log = LogManager.GetLogger(type);
        //    m_log.Error(message);
        //}


        #region  增强型日志方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(object msg)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger("logerror");
                Task.Run(() => log.Error(msg));   //异步
                // Task.Factory.StartNew(() =>log.Error(msg));//  这种异步也可以
                //log.Error(msg);    //这种也行跟你需要，性能越好，越强大，我还是使用异步方式
            }
            catch (Exception)
            {


            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger("logerror");
                Task.Run(() => log.Error(ex.Message.ToString() + "/r/n" + ex.Source.ToString() + "/r/n" + ex.TargetSite.ToString() + "/r/n" + ex.StackTrace.ToString()));

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Error(object msg, Exception ex)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger("logerror");
                if (ex != null)
                {
                    Task.Run(() => log.Error(msg, ex));   //异步
                }
                else
                {
                    Task.Run(() => log.Error(msg));   //异步
                }
            }
            catch (Exception)
            {


            }
        }
        #endregion
    }
}
