using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Peacock.Common.IO.Log
{
    public static class Logger
    {
        /// 日志级别
        /// </summary>
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        /// <summary>
        /// 日志级别
        /// </summary>
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        /// <summary>
        /// 配置
        /// </summary>
        public static void SetConfig()
        {
            // log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="configFile">文件属性</param>
        public static void SetConfig(FileInfo configFile)
        {
            //log4net.Config.XmlConfigurator.Configure(configFile);
        }

        /// <summary>
        /// 输出消息
        /// </summary>
        /// <param name="info">消息</param>
        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        /// <summary>
        /// 输出消息
        /// </summary>
        /// <param name="info">错误标题</param>
        /// <param name="se">异常消息</param>
        public static void WriteLog(string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, se);
            }
        }

        /// <summary>
        /// 错误记录封装
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static void Error(string message, Exception ex)
        {
            if (logerror.IsErrorEnabled)
            {
                if (!string.IsNullOrEmpty(message) && ex == null)
                {
                    logerror.ErrorFormat("<br/>【附加信息】 : {0}<br>", new object[] { message });
                }
                else if (!string.IsNullOrEmpty(message) && ex != null)
                {
                    string errorMsg = BeautyErrorMsg(ex);
                    logerror.ErrorFormat("<br/>【附加信息】 : {0}<br>{1}", new object[] { message, errorMsg });
                }
                else if (string.IsNullOrEmpty(message) && ex != null)
                {
                    string errorMsg = BeautyErrorMsg(ex);
                    logerror.Error(errorMsg);
                }
            }
        }

        /// <summary>
        /// 美化错误信息
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>错误信息</returns>
        private static string BeautyErrorMsg(Exception ex)
        {
            string errorMsg = string.Format("【异常类型】：{0} <br>【异常信息】：{1} <br>【堆栈调用】：{2}",
                new object[] { ex.GetType().Name, ex.Message, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            errorMsg = errorMsg.Replace("位置", "<strong style=\"color:red\">位置</strong><br/>");
            return errorMsg;
        }
    }
}
