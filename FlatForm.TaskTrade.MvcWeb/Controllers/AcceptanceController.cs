using Peacock.PEP.Model;
using Newtonsoft.Json;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model.DTO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peacock.PEP.MvcWebSite.Controllers
{
    public class AcceptanceController : BaseController
    {
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.ProjectSourceList = GetSelectList("项目来源");
            ViewBag.ProjectTypeList = GetSelectList("项目类型");
            ViewBag.BusinessTypeList = GetSelectList("估价目的");
            ViewBag.PropertyTypeList = GetSelectList("物业类型");
            ViewBag.ReportCreaUrl = ConfigurationManager.AppSettings["ReportCreaUrl"];
            ViewBag.ReportLogInUrl = ConfigurationManager.AppSettings["ReportLogInUrl"];
            return View();
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ActionResult AddingProject()
        {
            ViewBag.ReportCreaUrl = ConfigurationManager.AppSettings["ReportCreaUrl"];
            ViewBag.ReportLogInUrl = ConfigurationManager.AppSettings["ReportLogInUrl"];
            ViewBag.ProjectSourceList = GetSelectList("项目来源");
            ViewBag.ProjectTypeList = GetSelectList("项目类型");
            ViewBag.BusinessTypeList = GetSelectList("估价目的");
            ViewBag.PropertyTypeList = GetSelectList("物业类型");
            ViewBag.ReportTypeList = GetSelectList("报告类型");
            ViewBag.Citys = ConfigurationManager.GetSection("xunjiaCitys") as NameValueCollection;//城市信息
            ViewBag.OutTaskUserList = ProjectAdapter.GetOuterTaskUsers();//获取用户信息
            ViewBag.DataDefinesList = ProjectAdapter.GetDataDefines();//获取登陆用户勘察表
            return View();
        }

        public ActionResult ProjectList()
        {
            return View();
        }

        /// <summary>
        /// 立项添加
        /// </summary>
        /// <param name="DTO">项目参数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProject(ProjectModel DTO)
        {
            return Help.ExceptionCatch.Invoke(() =>
            {
                string ExplorationContactsStr = HttpUtility.UrlDecode(DTO.ExplorationContactsStr);
                List<ExplorationContactsModel> listExplorationContacts = JsonConvert.DeserializeObject<List<ExplorationContactsModel>>(ExplorationContactsStr);
                //过滤查勘联系人信息
                listExplorationContacts = listExplorationContacts.Where(x => x.IsDelete == false).ToList();

                long projectId = ProjectAdapter.Save(DTO, listExplorationContacts);
                if (!string.IsNullOrEmpty(Request["onlineid"]))     //受理在线业务
                {
                    OnLineBusinessService.Accept(long.Parse(Request["onlineid"]), projectId);
                }
                if (!string.IsNullOrEmpty(Request["InquiryId"]))
                {
                    InquiryService.ToProject(long.Parse(Request["InquiryId"]), projectId);
                }
                ProjectModel project = ProjectAdapter.GetProjectById(projectId);                 
                return project.Id;
            }, x => "操作成功");
        }

        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页页数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProjectList(ProjectCondition condition, int pageIndex, int pageSize)
        {
            int total;
            var result = ProjectService.GetProjectList(condition, pageIndex, pageSize, out total);
            return Json(new
            {
                rows = result,
                total
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据在线业务TID获取Project对象
        /// </summary>
        /// <param name="onlineid"></param>
        /// <returns></returns>
        public JsonResult LoadProjectByOnline(long onlineid)
        {
            OnLineBusinessModel result = OnLineBusinessService.GetById(onlineid);
            return Json(new
            {
                IsAccept = result.IsAccept,
                //BankName = result.BankListStr,
                PropertyType = result.PropertysType,
                HouseAddress = result.HouseAddress,
                result.City,
                Region = result.Region,
                Community = result.Community,
                Area = result.Area,
                BuildedYear = result.BuildYear,
                result.IsStop,
                DelegateType = result.DelegateType.ToString(),
                EvaluationCompany = UserService.GetDepartmentById(result.DepartmentId).CompanyName,
                BankName = result.OnLineBusinessBanks.FirstOrDefault() != null ? result.OnLineBusinessBanks.FirstOrDefault().BankName : "",
                BankBranch = result.OnLineBusinessBanks.FirstOrDefault() != null ? result.OnLineBusinessBanks.FirstOrDefault().BankbranchName : ""
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据项目id获取看房联系人信息
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public JsonResult GetExplorationContactsList(long ProjectId)
        {
            var query = ExplorationContactsService.GetListByProjectId(ProjectId);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据线上业务id获取看房联系人信息
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public JsonResult GetExplorationContactsListByBusiness(long BusinessId)
        {
            var query = ExplorationContactsService.GetListByOnlineBusinessId(BusinessId);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据项目id获取项目信息
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public JsonResult GetProjiectById(long ProjectId)
        {
            var query = ProjectService.GetProjectById(ProjectId);
            query.SummaryData = null;
            query.ExplorationContacts = null;
            query.Customer = null;
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据项目Id获取Project对象
        /// 作者：BOBO
        /// 时间:2016-4-11
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ActionResult LoadProjectById(long projectId)
        {
            ProjectModel result = ProjectService.GetProjectById(projectId);
            return Json(new
            {
                //客户信息
                CustomerTel = result.Customer == null ? "" : result.Customer.Tel,
                CustomerPhone = result.Customer == null ? "" : result.Customer.Phone,
                CustomerName = result.Customer == null ? "" : result.Customer.CustomerName,
                CustomerBank = result.Customer == null ? "" : result.Customer.Bank,
                CustomerSubbranch = result.Customer == null ? "" : result.Customer.Subbranch,
                CustomerQQ = result.Customer == null ? "" : result.Customer.QQ,
                //项目信息
                ReportNo = result.ReportNo,
                ProjectSource = result.ProjectSource,
                ProjectClassificat = result.ProjectType,
                //ReportType=result.re
                BusinessType = result.BusinessType,
                PropertyType = result.PropertyType,
                City = result.City,
                District = result.District,
                ResidentialAreaName = result.ResidentialAreaName,
                ResidentialAddress = result.ResidentialAddress,
                ProjectAddress = result.ProjectAddress,
                BuildingArea = result.BuildingArea,
                InquiryPrice = result.InquiryPrice,
                InquiryResult = result.InquiryResult,
                BuildedYear = result.BuildedYear,
                EmergencyLevel = result.EmergencyLevel,
                CreditBank = result.CreditBank,
                CreditSubbranch = result.CreditSubbranch,
                CreditInfo = result.CreditInfo,
                Principal = result.Principal,
                IsProspecting = result.IsProspecting,
                Investigator = result.Investigator
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交项目审核
        /// </summary>
        /// <param name="ProjectId">项目表id</param>
        /// <returns></returns>
        public JsonResult ComitAudit(long ProjectId)
        {
            var query = ProjectService.ComitAudit(ProjectId);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}
