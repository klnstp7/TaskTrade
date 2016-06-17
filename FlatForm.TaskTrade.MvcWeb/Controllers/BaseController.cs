using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Peacock.PEP.DataAdapter;
using Peacock.PEP.Model;
using System;
using Peacock.PEP.DataAdapter.Interface;
using System.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.IO;
using Peacock.Common.Helper;

namespace Peacock.PEP.MvcWebSite.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected static readonly string SuccessMessage = "操作成功";
        protected static readonly IInquiryAdapter InquiryService = ConditionFactory.Conditions.Resolve<IInquiryAdapter>();
        protected static readonly IUserAdapter UserService = ConditionFactory.Conditions.Resolve<IUserAdapter>();
        protected static readonly IProjectAdapter ProjectService = ConditionFactory.Conditions.Resolve<IProjectAdapter>();
        protected static readonly IProjectResourceAdapter ProjectResourceService = ConditionFactory.Conditions.Resolve<IProjectResourceAdapter>();
        protected static readonly IOnLineBusinessAdapter OnLineBusinessService = ConditionFactory.Conditions.Resolve<IOnLineBusinessAdapter>();
        protected static readonly IOnLineFeedBackAdapter OnLineFeedBackService = ConditionFactory.Conditions.Resolve<IOnLineFeedBackAdapter>();
        protected static readonly IBaseAPIAdapter BaseAPIService = ConditionFactory.Conditions.Resolve<IBaseAPIAdapter>();
        protected static readonly IParameterAdapter ParameterAdapter = ConditionFactory.Conditions.Resolve<IParameterAdapter>();
        protected static readonly ICompanyAdapter CompanyService = ConditionFactory.Conditions.Resolve<ICompanyAdapter>();
        protected static readonly IIntegratedQueryAdapter IntegratedQueryService = ConditionFactory.Conditions.Resolve<IIntegratedQueryAdapter>();
        protected static readonly IProjectAdapter ProjectAdapter = ConditionFactory.Conditions.Resolve<IProjectAdapter>();
        protected static readonly IProjectDocumentAdapter ProjectDocumentService = ConditionFactory.Conditions.Resolve<IProjectDocumentAdapter>();
        protected static readonly IDtgjAPIAdapter DtgjAPIService = ConditionFactory.Conditions.Resolve<IDtgjAPIAdapter>();
        protected static readonly IProjectStateInfoAdapter ProjectStateService = ConditionFactory.Conditions.Resolve<IProjectStateInfoAdapter>();
        protected static readonly IExplorationContactsAdapter ExplorationContactsService = ConditionFactory.Conditions.Resolve<IExplorationContactsAdapter>();
        protected static readonly ISummaryDataAdapter SummaryDataService = ConditionFactory.Conditions.Resolve<ISummaryDataAdapter>();
        protected static readonly IRegionAdapter RegionService = ConditionFactory.Conditions.Resolve<IRegionAdapter>();
        protected static readonly IConfigSolutionAdapter ConfigSolutionService = ConditionFactory.Conditions.Resolve<IConfigSolutionAdapter>();
        protected static readonly IReportHistoryAdapter ReportHistoryService = ConditionFactory.Conditions.Resolve<IReportHistoryAdapter>();
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            CookieHelper.WriteCookie(CookieHelper.UserStateKey, requestContext.HttpContext.User.Identity.Name);
            base.Initialize(requestContext);
        }

        #region 获取参数字典信息
        /// <summary>
        /// 获取分类(IsGetChild为true时，返回该（父节点）下的所有子节点，false时则直接返回该节点)
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="IsGetChild"></param>
        /// <returns></returns>
        public JsonResult GetParameter(string name, bool IsGetChild = true)
        {
            IList<ParameterModel> childList = new List<ParameterModel>();
            var parentParameter = ParameterAdapter.GetParameterTree(name);
            if (parentParameter != null && IsGetChild)
            {
                childList = ParameterAdapter.GetListByParentId(parentParameter.Id);
            }
            return IsGetChild ? Json(childList) : Json(parentParameter);
        }

        /// <summary>
        /// 获取分类参数列表
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetSelectList(string Key)
        {
            var data = GetParameter(Key).Data as List<ParameterModel>;
            if (data == null)
                yield break;
            foreach (var item in data)
                yield return new SelectListItem() { Text = item.Name, Value = item.Name };
        }

        #endregion

        private static string _currentCity = "_currentCity";
        protected string CurrentCity
        {
            get
            {
                if (Request.Cookies[_currentCity] == null)
                {
                    CookieHelper.WriteCookie(_currentCity, LogHelper.GetCityByIP(HttpContext.Request.UserHostAddress));
                    CookieHelper.SetCookieExpires(_currentCity, 7);
                }
                return CookieHelper.GetCookie(_currentCity);
            }
            set
            {
                CookieHelper.WriteCookie(_currentCity, value);
            }
        }
    }
}