using PermissionsMiddle.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Peacock.PEP.DataAdapter;
using Peacock.PEP.DataAdapter.Interface;

namespace Peacock.PEP.MvcWebSite.Attributes
{
    public abstract class BaseAttribute : FilterAttribute, IActionFilter
    {
        protected static readonly IUserAdapter UserAdapter = ConditionFactory.Conditions.Resolve<IUserAdapter>();

        public IList<UserPowerApiDto> PermissionList
        {
            get { return UserAdapter.GetUserPermissions(); }
        }

        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
    }
}