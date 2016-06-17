using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peacock.PEP.MvcWebSite.Attributes
{
    /// <summary>
    /// 系统权限验证特性（加在Action上）
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PermissionAttribute : BaseAttribute
    {
        protected string PermissionCode { get; private set; }


        public PermissionAttribute(string permissionCode)
        {
            PermissionCode = permissionCode;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool hasPermission = false;

            var permissionAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(PermissionAttribute), false).Cast<PermissionAttribute>();
            if (permissionAttributes.Any())
            {
                var attributes = permissionAttributes as IList<PermissionAttribute> ?? permissionAttributes.ToList();

                foreach (var attr in attributes)
                {
                    if (PermissionList.Any(x => x.PowerCode.Equals(attr.PermissionCode, StringComparison.OrdinalIgnoreCase)))
                    {
                        hasPermission = true;
                        break;
                    }
                }

                if (!hasPermission)
                {
                    var urlHelper = new UrlHelper(filterContext.RequestContext);
                    var url = urlHelper.Action("NoPermission", "Home");
                    filterContext.Result = new RedirectResult(url); ;
                }
            }

            base.OnActionExecuting(filterContext);
        }

    }
}