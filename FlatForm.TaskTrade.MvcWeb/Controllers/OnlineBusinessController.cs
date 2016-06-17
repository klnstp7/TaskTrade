using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.DataAdapter.Interface;


namespace Peacock.PEP.MvcWebSite.Controllers
{
    public class OnlineBusinessController : BaseController
    {
      
        /// <summary>
        /// 线上业务页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

       /// <summary>
       /// 获取在线业务列表
       /// </summary>
       /// <param name="condition"></param>
       /// <param name="page"></param>
       /// <param name="rows"></param>
       /// <returns></returns>
        public JsonResult GetOnLineBusinessList(OnLineBusinessCondition condition, int pageNumber, int pageSize)
        {
            int total;
            var result = OnLineBusinessService.GetOnlineBusinessList(condition, pageNumber, pageSize, out total);
            var jsonresult = new
            {
                rows = result.Select(x =>
                {
                    return new
                    {
                        x.Id,
                        x.TransactionNo,
                        x.HouseAddress,
                        x.Community,
                        x.Urgency,
                        x.LoanType,
                        x.PropertysType,
                        x.IsAccept,
                        x.RefusedReason,
                        x.IsInquiry,
                        x.DataSource,
                        x.Assessment,
                        DelegateType=x.DelegateType.ToString()
                    };
                }),
                total
            };
            return Json(jsonresult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///线上受理详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(long id)
        {
            OnLineBusinessModel model = OnLineBusinessService.GetById(id);
            if (model == null)
            {
                model = new OnLineBusinessModel();
            }
            else
            {
                var company =UserService.GetDepartmentById(model.DepartmentId);
                ViewBag.CompanyName = company == null ? "" : company.CompanyName;

                ViewBag.DocumentList = ProjectDocumentService.GetDocumentList(model.Id);
            }
           
            return View(model);
        }

        /// <summary>
        ///获取线上受理详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetOnlineBusinessById(long id)
        {
            var result = OnLineBusinessService.GetById(id);
            return Json(new
            {
                result.IsStop,
                result.IsInquiry,
                result.PropertysType,
                result.Region,
                result.Community,
                result.HouseAddress,
                result.BuildYear,
                result.Remark,
                result.Area,
                result.Orientation,
                result.Floor,
                result.City,
                result.TotalFloor,
                result.Assessment,
                result.FloorBuilding,
                result.DecorateCase,
                result.PlanUse
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 拒绝受理
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="RefusedReason"></param>
        /// <returns></returns>
        public ActionResult Refuse(long Id,string RefusedReason)
        {
            return Help.ExceptionCatch.Invoke(() =>
            {
                OnLineBusinessService.Refuse(Id, RefusedReason);
            });
        }

        /// <summary>
        /// 保存反馈意见
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public ActionResult SubmitFeeBack(long onLineBusinessId, string content, string remark, string transactionNo)
        {
            return Help.ExceptionCatch.Invoke(() =>
            {
                OnLineFeedBackService.Save(new OnLineFeedBackModel() {  OnLineBussinessId = onLineBusinessId, Content = content, Remark = remark, CreateTime=DateTime.Now});
                //发送消息到消息队列
                OnLineFeedBackService.SendMessageQueue(new OnLineFeedBackModel() { OnLineBussinessId = onLineBusinessId, Content = content, Remark = remark, CreateTime = DateTime.Now }, transactionNo);
            });
        }
    }
}
