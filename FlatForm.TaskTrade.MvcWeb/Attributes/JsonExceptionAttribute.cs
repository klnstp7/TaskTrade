using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peacock.PEP.MvcWebSite
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class JsonExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled && filterContext.Exception is ApplicationException)
            {
                //返回异常JSON
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, message = filterContext.Exception.Message }
                };
            }
        }
    }
}