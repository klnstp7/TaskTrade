using Peacock.PEP.DataAdapter;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using System.Linq;
using System.Web.Mvc;

namespace Peacock.PEP.MvcWebSite.Controllers
{
    public class ReportDeliveryController : BaseController
    {
        protected static readonly IProjectAdapter _IProjectAdapter = ConditionFactory.Conditions.Resolve<IProjectAdapter>();
        protected static readonly IReportSendAdapter _IReportSendAdapter = ConditionFactory.Conditions.Resolve<IReportSendAdapter>();

        //
        // GET: /ReportDelivery/

        public ActionResult Index()
        {
            ViewBag.SendType = GetSelectList("报告发送方式");
            return View();
        }

        /// <summary>
        /// 报告发送详细
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportSendDetail()
        {
            long id = 0;
            long.TryParse(Request["Id"] + "", out id);
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>        
        public JsonResult GetProjectData(bool? isSent, string projectNo,string reportNo,string projectAddress,string residentialAreaName,bool isApprove,bool getReportCount,int index, int rows)
        {
            ProjectCondition condition = new ProjectCondition();
            condition.IsSent = isSent;            
            condition.ProjectNo = projectNo;
            condition.ReportNo = reportNo;
            condition.ProjectAddress = projectAddress;
            condition.ResidentialAreaName = residentialAreaName;            
            condition.GetReportCount = getReportCount;
            condition.IsApprove = isApprove;
            int total;
            var result = _IProjectAdapter.GetProjectList(condition, index, rows, out total);
            return Json(new
            {
                rows = result.Select(x => new
                {
                    x.Id,
                    x.ProjectNo,
                    x.ReportNo,
                    x.ProjectAddress,
                    x.ResidentialAreaName,
                    x.SendReportCount
                }),
                total
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>        
        public JsonResult GetReportData(long projectId, string sendExpress, string expressNo, string sendAddress, string reciverMobile, int index, int rows)
        {
            ReportSendCondition condition = new ReportSendCondition();
            condition.ProjectId = projectId;
            condition.SendExpress = sendExpress;
            condition.ExpressNo = expressNo;
            condition.SendAddress = sendAddress;
            condition.ReciverMobile = reciverMobile;
            int total;
            var result = _IReportSendAdapter.GetReportSendList(condition, index, rows, out total);
            return Json(new
            {
                rows = result.Select(x => new
                {
                    x.Id,
                    x.SendType,
                    x.SendExpress,
                    x.ExpressNo,
                    x.SendAddress,
                    x.Receiver,
                    x.ReciverMobile,
                    x.SendQuantity,
                    x.CreateTime,
                    x.Remark
                }),
                total
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存报告发送记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public JsonResult SaveReportSendData(ReportSendDto dto)
        {
            //TODO 需要当前登录用户ID
            //dto.SenderId = 1;
            ResultInfo result = new ResultInfo();
            if(dto.ProjectId > 0)
            {
                result = _IReportSendAdapter.SaveReportSendData(dto);
            }
            else
            {
                result.Message = "项目不存在或者已经被删除";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendType()
        {
            var result = GetSelectList("报告发送方式").ToList();
            string[] data = new string[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                data[i] = result[i].Text;
            }
            //return Json(new { data = result.Select(x => new { x.Text, x.Value }) }, JsonRequestBehavior.AllowGet);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

    }
}
