#region << 版 本 注 释 >>
/*
 * ========================================================================
 * Copyright(c) 2004-2015 广州光汇软件科技有限公司, All Rights Reserved.
 * ========================================================================
*/
#endregion

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
using System.Threading;
using RestSharp;
using Peacock.PEP.Model;
using Newtonsoft.Json;

namespace Peacock.Common.Helper
{
    /// <summary>
    /// 日志写入类
    /// </summary>
    /// <remarks>
    ///     <para>    Creator：hl</para>
    ///     <para>CreatedTime：2015-01-23 11:07:21</para>
    /// </remarks>
    public static class LogHelper
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// 调试
            /// </summary>
            Debug = 1,

            /// <summary>
            /// 用户操作日志
            /// </summary>
            Info = 2,

            /// <summary>
            /// 错误日志
            /// </summary>
            Error = 3,
        }

        /// <summary>
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

        /// <summary>
        /// 指定格式写入日志
        /// </summary>
        /// <param name="concent">记录内容</param>
        public static void Ilog(string http_referer,string concent)
        {
            try
            {
                string userAccount = "评E评管理员";
                try
                {
                    userAccount = CookieHelper.GetCookie(CookieHelper.UserStateKey);
                }
                catch
                {
                    userAccount = "评E评管理员";
                }
                string httpClientDoMain = ConfigurationManager.AppSettings["ReLogInUrl"];
                string clientIp = CookieHelper.GetWebClientIp();
                string insiteIp = CookieHelper.GetWebSiteIpAndPort();
                string fwVersion = Environment.Version.ToString();
                string port = CookieHelper.GetWebSitePort();
                string iis = CookieHelper.GetIIS();
                string city = "";
                if (!IPAndCity.Keys.Contains(clientIp))
                {
                    city = GetCityByIP(clientIp);
                    IPAndCity.Add(clientIp, city);
                }
                else
                {
                    city = IPAndCity[clientIp];
                }
                //{0}            {1}          {2}          {3}           {4}   {5}      {6}              {7}
                //$remote_addr - $remote_user $remote_city [$time_local] $host $request $requestconcent  $upstream_addr
                string logConcentFormat = "{0} - {1} {2} [{3}] {4} {5} {6} {7}";
                string logConcent = string.Format(logConcentFormat, clientIp, userAccount, city, DateTime.Now.ToString("yyyyMMddHHmmss"), httpClientDoMain, http_referer, concent, insiteIp);
                string token = DateTime.Now.ToString("yyyyMMdd") + ".log";
                //string logRoot = FileStreamHelper.GetPathAtApplication("userlogs");
                string logRoot = FileStreamHelper.GetPathAtApplication("Logs\\" + DateTime.Now.ToString("yyyy"));
                if (!Directory.Exists(logRoot))
                {
                    Directory.CreateDirectory(logRoot);
                }
                StreamWriter sw = new StreamWriter(logRoot + "\\" + token, true);
                sw.WriteLine(logConcent);//写入
                sw.Close();
            }
            catch (Exception ex)
            {
                Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 外业任务分配
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string GetCityByIP(string ip)
        {
            string result = "北京";
            string ipcity = ConfigurationManager.AppSettings["ipcity"];
            var client = new RestClient(string.Format(ipcity,ip));
            var request = new RestRequest("", Method.GET);
            var data = client.Execute<IPAndCity>(request);
            try
            {
                IPAndCity city = JsonConvert.DeserializeObject<IPAndCity>(data.Content);
                result = city.data.city;
            }
            catch { }
            return result;
        }



        private static Dictionary<string, string> IPAndCity = new Dictionary<string, string>();

    }

}