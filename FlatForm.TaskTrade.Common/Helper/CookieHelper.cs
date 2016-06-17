#region << 版 本 注 释 >>
/*
 * ========================================================================
 * Copyright(c) 2004-2015 广州光汇软件科技有限公司, All Rights Reserved.
 * ========================================================================
*/
#endregion

using System;
using System.Web;
using System.Net;
using Peacock.PEP.Model.ApiModel;
using Newtonsoft.Json;
using Peacock.PEP.Model;
using System.Configuration;
using System.Text;
using System.IO;

namespace Peacock.Common.Helper
{
    /// <summary>
    /// Cookie处理
    /// </summary>
    /// <remarks>
    ///     <para>    Creator：Samir</para>
    ///     <para>CreatedTime：2015-09-23</para>
    /// </remarks>
    public static class CookieHelper
    {
        public static readonly string UserStateKey = "Inwork-User";

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <returns></returns>
        public static string GetUserAccount()
        {
            return GetCookie(UserStateKey);
        }

        /// <summary>
        /// 写cookie值(多个键值的 Cookie 对象)
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="key">Key</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
                //cookie.Domain = "yunfangdata.com";
            }
            cookie[key] = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="key">Key</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
                //cookie.Domain = "yunfangdata.com";                
            }
            //FormsAuthentication.HashPasswordForStoringInConfigFile(strValue, "md5");
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
            
        }
        /// <summary>
        /// 设置cookie过期时间
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="expires">过期时间(天数)</param>
        public static void SetCookieExpires(string strName, int day)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Expires = DateTime.Now.AddDays(day);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 设置cookie过期时间
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="expires">过去时间点</param>
        public static void SetCookieExpires(string strName, DateTime expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Expires = expires;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value;
            }
            return "";
        }

        /// <summary>
        /// 读cookie值(多个键值的 Cookie 对象)
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="key">Key</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null 
                && HttpContext.Current.Request.Cookies[strName] != null 
                && HttpContext.Current.Request.Cookies[strName][key] != null)
            {
                return HttpContext.Current.Request.Cookies[strName][key];
            }
            return "";
        }

        /// <summary>
        /// 移除客户端 Cookie
        /// </summary>
        /// <param name="strName"></param>
        public static void RemoveCookie(string strName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie != null)
            {
                cookie.Value = "";
                HttpContext.Current.Request.Cookies.Remove(strName);
                //清除 
                TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                cookie.Expires = DateTime.Now.Add(ts);//立即过期 
                HttpContext.Current.Response.Cookies.Add(cookie);//写入立即过期的*/
                HttpContext.Current.Response.Cookies[strName].Expires = DateTime.Now.AddDays(-1);
            }
        }

        /// <summary>
        /// 移除客户端 Cookie(多值 Cookie 中的某个值)
        /// </summary>
        /// <param name="strName"></param>
        public static void RemoveCookie(string strName, string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie != null)
            {
                cookie.Values.Remove(key);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        /// <summary>
        /// 获取客户端ip
        /// </summary>
        /// <returns></returns>
        public static string GetWebClientIp()
        {
            //string result = "";            
            //try
            //{
            //    if (HttpContext.Current == null
            //|| HttpContext.Current.Request == null
            //|| HttpContext.Current.Request.ServerVariables == null)
            //        result = "";

            //    string CustomerIP = "";

            //    //CDN加速后取到的IP 
            //    CustomerIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            //    if (!string.IsNullOrEmpty(CustomerIP))
            //    {
            //        result = CustomerIP;
            //    }

            //    CustomerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];


            //    if (!string.IsNullOrEmpty(CustomerIP))
            //        result = CustomerIP;

            //    if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            //    {
            //        CustomerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //        if (CustomerIP == null)
            //            CustomerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //    }
            //    else
            //    {
            //        CustomerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            //    }

            //    if (string.Compare(CustomerIP, "unknown", true) == 0)
            //        result = HttpContext.Current.Request.UserHostAddress;
            //    result = CustomerIP;
            //}
            //catch { }
            //return result;

            return HttpContext.Current.Request.UserHostAddress;
        }

        /// <summary>
        /// 获取站点ip和端口
        /// </summary>
        /// <returns></returns>
        public static string GetWebSiteIpAndPort()
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
        }

        /// <summary>
        /// 获取站点端口
        /// </summary>
        /// <returns></returns>
        public static string GetWebSitePort()
        {
            return HttpContext.Current.Request.Url.Port.ToString();
        }

        /// <summary>
        /// 获取外围IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string tempIP = string.Empty;
            if (Dns.GetHostEntry(Dns.GetHostName()).AddressList.Length > 1)
                tempIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            return tempIP;
        }

        /// <summary>
        /// 获取iis信息
        /// </summary>
        /// <returns></returns>
        public static string GetIIS()
        {
            return HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"].ToString();
        }

        #region 发送验证码

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phoneNumber">发送号码</param>
        /// <param name="securityCode">验证码</param>
        /// <returns></returns>
        public static ResultInfo SendSecurityCode(string phoneNumber, string securityCode)
        {
            ResultInfo result = new ResultInfo();
            var securityCodeInterval = int.Parse(ConfigurationManager.AppSettings["SecurityCodeInterval"]);
            var template = ConfigurationManager.AppSettings["Template"];
            var platformId = int.Parse(ConfigurationManager.AppSettings["PlatformId"]);
            var postUrl = ConfigurationManager.AppSettings["PostUrl"];

            var data = JsonConvert.SerializeObject(new
            {
                platformID = platformId,
                content = string.Format(template, securityCode),
                phones = phoneNumber,
                contentType = "验证码"
            });

            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = byteArray.Length;

            using (Stream stream = webReq.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
                using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        var res = sr.ReadToEnd();
                        dynamic resJson = JsonConvert.DeserializeObject(res);
                        if ((bool)resJson.success && ((string)resJson.msg).ToLower() == "success")
                        {
                            result.Message = "发送验证码成功";
                            result.Success = true;
                        }
                        else
                        {
                            result.Message = resJson.msg;
                            result.Success = false;
                        }
                    }
                }
            }
            return result;
        }

        #endregion
    }
}
