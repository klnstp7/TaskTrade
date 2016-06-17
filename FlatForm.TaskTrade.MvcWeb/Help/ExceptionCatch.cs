using log4net;
using Peacock;
using Peacock.Common.Exceptions;
using Peacock.Common.Helper;
using Peacock.Common.IO.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peacock.PEP.MvcWebSite.Help
{
    public class ExceptionCatch
    {
        public static JsonResult Invoke(Action action, string successMessage = "操作成功！")
        {
            return Invoke
            (
                () =>
                {
                    action();
                    return successMessage;
                },
                x => x
            );
        }

        public static JsonResult Invoke<TResult>(Func<TResult> funData, Func<TResult, string> messageConvert)
        {
            string message = null;
            bool success = true;
            object data = null;
            try
            {
                TResult receive = funData();
                data = receive;
                message = messageConvert(receive);
            }
            catch (Exception ex)
            {
                success = false;
                if (ex is ServiceException)
                {
                    message = ex.Message;
                }
                else
                {
                    message = "操作失败！";
                    LogHelper.Error("操作失败", ex);
                }
            }
            var json = new JsonResult();
            json.Data = new { success, message, data };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
    }
}