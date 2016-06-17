using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model.DTO;
using System.IO;
using ResourceLibraryExtension.Helper;
using Newtonsoft.Json;
using Peacock.PEP.Model;
using System.Net;
using System.Web.UI;
using System.Text;

namespace Peacock.PEP.MvcWebSite.Controllers
{
    public class ProjectReviewController : BaseController
    {
        /// <summary>
        /// 列表页
        /// 作者：BOBO
        /// 时间：2016-4-5
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

       
        /// <summary>
        /// 获取项目列表
        /// 作者：BOBO
        /// 时间：2016-4-5
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetProjectList(ProjectCondition condition, int pageSize = 10, int pageIndex = 1)
        { 
            condition.IsSubmitted = true;
            int total;
            var result = ProjectService.GetProjectList(condition, pageIndex, pageSize, out total);
            return Json(new
            {
                rows = result.Select(p => new
                {
                    p.Id,
                    p.EmergencyLevel,
                    p.ProjectNo,
                    p.ReportNo,
                    p.ProjectAddress,
                    p.ResidentialAreaName,
                    p.IsApprove,
                    p.ReportType,
                    p.IsSent,
                    sumId=p.SummaryDataId
                }),
                total
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新建报告
        /// 作者：BOBO
        /// 时间：2016-4-6
        /// </summary>
        /// <returns></returns>
        public ActionResult AddReport()
        {
            ViewBag.ProjectType = GetParameter("项目分类").Data;
            ViewBag.ReportType = GetParameter("报告类型").Data;
            ViewBag.WuyeType = GetParameter("物业类型").Data;
            ViewBag.BusinessType = GetParameter("估价目的").Data;
            return View();
        }

        /// <summary>
        /// 保存报告
        /// 作者：BOBO
        /// 时间：2016-4-6
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveReport(AddReportCondition condition)
        {
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = null;
            //是否有文件
            if (files.Count > 0)
            {
                file = files[0];
            }
            else
            {
                return Json(new { msg = "请上传文件！" },"text/html", JsonRequestBehavior.AllowGet);
            }
            //新建项目
            ProjectModel projectModel = new ProjectModel();
            projectModel.ReportNo = condition.ReportNo;
            //projectModel.ResidentialAddress = condition.ResidentialAddress;
            projectModel.PropertyType = condition.PropertyType;
            projectModel.ProjectType = condition.ProjectType;
            projectModel.BusinessType = condition.BusinessType;
            projectModel.ResidentialAreaName = condition.ResidentialAreaName;

            //projectModel.ProjectNo = Guid.NewGuid().ToString();
            projectModel.DepartmentId = UserService.GetCurUserApiDto().CompanyId;
            projectModel.ProjectCreatorId = UserService.GetCurUserApiDto().Id;
            projectModel.ProjectCreatorName = UserService.GetCurUserApiDto().UserAccount;
            //  projectModel.CreateTime = DateTime.Now;
            projectModel.ProjectSource = "新建报告";
            projectModel.ResidentialAddress = condition.ResidentialAddress;
            projectModel.ProjectAddress = condition.ResidentialAddress;
            projectModel.ReportType = condition.ResourceType;
            projectModel.EmergencyLevel = "普通";
            projectModel.IsSubmitted = "true";
            long projectId = ProjectService.Add(projectModel);
            //新建报告资源
            ProjectResourceModel projectResource = new ProjectResourceModel();
            projectResource.CreateTime = DateTime.Now;
            projectResource.ProjectId = projectId;
            projectResource.ResourceType = condition.ResourceType == "报告" ? 1 : 2;
            long projectResourceId = ProjectResourceService.AddProjectResource(projectResource);
            //上传资源库
            string fileName = file.FileName;
            string extensionName = fileName.Substring(fileName.LastIndexOf('.') + 1);
            int fileLength = file.ContentLength;
            Stream fileStream = file.InputStream;
            byte[] fileByte = new byte[fileLength];
            fileStream.Read(fileByte, 0, fileLength - 1);
            long resourceId = SingleFileManager.SaveFileResource(fileByte, extensionName, fileName, projectResourceId.ToString());
            //更新报告资源
            // ProjectResourceModel newProjectResource = new ProjectResourceModel();
            projectResource.FileName = fileName;
            projectResource.ResourceId = resourceId;
            projectResource.Id = projectResourceId;
            ProjectResourceService.UpdateProjectResource(projectResource);
            return Json(new { msg = "成功新建报告" },"text/html", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查看详细
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ActionResult Detail(long projectId)
        {
            ViewBag.ProjectID = projectId;
            return View();
        }

        /// <summary>
        /// 查看编辑报告
        /// 作者：BOBO
        /// 时间：2016-4-12
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewReport()
        {
            //项目id
            long projectId;
            long.TryParse(Request["projectId"] + "", out projectId);
            //报告或者计算表1:报告，2:计算表;默认报告
            int resourceType = 1;
            if (!string.IsNullOrEmpty(Request["resourceType"] + ""))
            {
                int.TryParse(Request["resourceType"] + "", out resourceType);
            }
            //提示信息
            ViewBag.msg = "";
            //是否是编辑
            string operation = "edit";

            ProjectResourceModel model = ProjectResourceService.GetProjectResource(projectId, resourceType);
            //判断是否为空
            if (model == null)
            {
                if (resourceType == 1)
                {
                    ViewBag.msg = "报告不存在";
                }
                else
                {
                    ViewBag.msg = "计算表不存在";
                }
                ViewBag.projectId = projectId;
                return View();
            }
            //获取资源库路径
            string reportFilePath = SingleFileManager.GetFileUrl(model.ResourceId);
            if (reportFilePath == null)
            {
                ViewBag.msg = "找不到对应文档";
                ViewBag.projectId = projectId;
                return View();
            }
            string extension = Path.GetExtension(reportFilePath);
            //如果文件没有扩展名，则调用资源库接口去获取
            if (string.IsNullOrEmpty(extension))
            {
                extension = SingleFileManager.GetFileFormat(model.ResourceId);
            }
            //本地路径
            string datestr = DateTime.Now.ToString("yyyyMMdd");
            string path = Server.MapPath("~/TempFile/" + datestr + "/");
            //判断路径是否存在
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string localFileName = path + Guid.NewGuid().ToString() + "." + extension;
            //下载文档
            WebClient webClient = new WebClient();
            webClient.DownloadFile(reportFilePath, localFileName);

            //pageOffice
            string controlOutput = string.Empty;
            PageOffice.PageOfficeCtrl PageOfficeCtrl2 = new PageOffice.PageOfficeCtrl();
            Page page = new Page();
            PageOfficeCtrl2.SaveFilePage = Url.Content("~/ProjectReview/SaveDoc?localFileName=") + localFileName;
            PageOfficeCtrl2.ServerPage = Url.Content("~/pageoffice/server.aspx");
            //是否编辑
            if (operation == "edit")
            {
                PageOfficeCtrl2.AddCustomToolButton("提交保存", "Save()", 0);
            }
            else
            {
                PageOfficeCtrl2.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            }
            //支持office,金山办公软件打开
            PageOfficeCtrl2.OfficeVendor = PageOffice.OfficeVendorType.AutoSelect;
            // 打开文档
            if (localFileName.IndexOf(".doc") > 0)
            {
                PageOfficeCtrl2.WebOpen(localFileName, PageOffice.OpenModeType.docNormalEdit, "Tom");
            }
            else if (localFileName.IndexOf(".xls") > 0)
            {
                PageOfficeCtrl2.WebOpen(localFileName, PageOffice.OpenModeType.xlsNormalEdit, "Tom");
            }
            else if (localFileName.IndexOf(".ppt") > 0)
            {
                PageOfficeCtrl2.WebOpen(localFileName, PageOffice.OpenModeType.pptNormalEdit, "Tom");
            }
            else
            {
                PageOfficeCtrl2.Visible = false;
            }
            PageOfficeCtrl2.ID = "PageOfficeCtrl2";
            page.Controls.Add(PageOfficeCtrl2);
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Server.Execute(page, htw, false); controlOutput = sb.ToString();
                }
            }
            ViewBag.EditorHtml = controlOutput;
            ViewBag.projectId = projectId;
            ViewBag.localFileName = Server.UrlEncode(localFileName);
            ViewBag.ResourceId = model.ResourceId;
            ViewBag.resourceType = resourceType;
            ViewBag.Id = model.Id;
            return View();
        }

        /// <summary>
        /// 保存报告文档
        /// 作者：BOBO
        /// 时间：2016-4-12
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveDoc(string localFileName)
        {
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            try
            {
                fs.SaveToFile(localFileName);
            }
            catch (Exception e)
            {
                fs.CustomSaveResult = "保存异常：" + e.Message;
            }
            finally
            {
                fs.Close();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 保存报告
        /// 作者：BOBO
        /// 时间：2016-4-12
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveEditReport()
        {
            long projectId = 0;
            long.TryParse(Request["projectId"] + "", out projectId);
            string localFileName = Server.UrlDecode(Request["localFileName"] + "");
            long oldResourceId = 0;
            long.TryParse(Request["resourceId"] + "", out oldResourceId);
            int resourceType;
            int.TryParse(Request["resourceType"] + "", out resourceType);
            int id;
            int.TryParse(Request["id"] + "", out id);
            //文件名
            string fileName = Path.GetFileName(localFileName);
            //long newResourceId = SingleFileManager.UpdateResourceFile(localFileName, oldResourceId, fileName, id.ToString(), projectId.ToString(), "");
            long newResourceId = SingleFileManager.SaveFileResource(localFileName, fileName, id.ToString());
            string result = ProjectResourceService.SaveReport(id, projectId, oldResourceId, newResourceId, fileName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 项目审核基本信息
        /// 作者：BOBO
        /// 时间：2016-4-7
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectInfo()
        {
            long projectId;
            long.TryParse(Request["Id"] + "", out projectId);
            ProjectModel result = ProjectService.GetProjectById(projectId);
            return Json(new
            {
                ReportNo = result.ReportNo,
                ProjectNo = result.ProjectNo,
                ProjectAddress = result.ProjectAddress,
                Principal = result.Principal,
                BuildedYear = result.BuildedYear,
                ProjectType = result.ProjectType,
                PropertyType = result.PropertyType,
                BusinessType = result.BusinessType,
                DepartmentId = result == null ? 0 : result.DepartmentId,
                CustomerName = result.Customer == null ? "" : result.Customer.CustomerName,
                InquiryPrice = result.InquiryPrice,
                InquiryResult = result.InquiryResult
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 流程跟进列表
        /// 作者：BOBO
        /// 时间：2016-4-7
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProjectStateInfoList()
        {
            long projectId;
            long.TryParse(Request["Id"] + "", out projectId);
            IList<ProjectStateInfoModel> list = ProjectStateService.GetProjectStateInfoList(projectId);
            return Json(new
            {
                list = list.Select(p => new
                {
                    Operator = p.Operator,
                    Content = p.Content,
                    OperationTime = p.OperationTime.ToString("yyyy-MM-dd HH:mm:ss")
                })
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取汇总信息
        /// 作者：BOBO
        /// 时间：2016-4-7
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSummaryInfo()
        {
            long projectId;
            long.TryParse(Request["Id"] + "", out projectId);
            SummaryDataModel result = IntegratedQueryService.GetByProjectId(projectId);
            return Json(new
            {
                sumCompany = result.Company,
                sumDepartment = result.Department,
                sumProjectName = result.ProjectName,
                sumReportNo = result.ReportNo,
                sumReportYear = result.ReportYear,
                sumReportMonth = result.ReportMonth,
                sumEvalGoal = result.EvalGoal,
                sumGoalDescription = result.GoalDescription,
                sumEvalEntrust = result.EvalEntrust,
                sumReportUse = result.ReportUse,
                sumSubbranch = result.Subbranch,
                sumSurveyPeople = result.SurveyPeople,
                sumReportWriting = result.ReportWriting,
                sumProjectPopularizeName = result.ProjectPopularizeName,
                sumProjectProvince = result.ProjectProvince,
                sumProjectCity = result.ProjectCity,
                sumProjectDistrict = result.ProjectDistrict,
                sumOtherAddress = result.OtherAddress,
                sumProjectAddress = result.ProjectAddress,
                sumSurveyTime = result.SurveyTime.HasValue ? result.SurveyTime.Value.ToString("yyyy-MM-dd") : string.Empty,
                sumWorthTime = result.WorthTime.HasValue ? result.WorthTime.Value.ToString("yyyy-MM-dd") : string.Empty,
                sumJobStartTime = result.JobStartTime.HasValue ? result.JobStartTime.Value.ToString("yyyy-MM-dd") : string.Empty,
                sumJobEndTime = result.JobEndTime.HasValue ? result.JobEndTime.Value.ToString("yyyy-MM-dd") : string.Empty,
                sumReportValidityTerm = result.ReportValidityTerm,
                sumQuantitySurveyor1 = result.QuantitySurveyor1,
                sumQuantitySurveyor2 = result.QuantitySurveyor2,
                sumQuantitySurveyorJoin = result.QuantitySurveyorJoin,
                sumEvalMethod1 = result.EvalMethod1,
                sumEvalMethod2 = result.EvalMethod2,
                sumEvalMethodJoin = result.EvalMethodJoin,
                sumEvaluateTotal = result.EvaluateTotal,
                sumEvaluatePrice = result.EvaluatePrice,
                sumHouserOwnerNo = result.HouserOwnerNo,
                sumHouserOwner = result.HouserOwner,
                sumPropertyNature = result.PropertyNature,
                sumArchitectureArea = result.ArchitectureArea,
                sumBuildingStructure = result.BuildingStructure,
                sumBuiltYear = result.BuiltYear,
                sumRegisterUse = result.RegisterUse,
                sumFloor = result.Floor,
                sumMaxFloor = result.MaxFloor,
                sumFloorJoin = result.FloorJoin,
                //朝向(缺省)
                sumHouseType = result.HouseType,
                sumDecoration = result.Decoration,
                sumLandUserNo = result.LandUserNo,
                sumLandAddress = result.LandAddress,
                sumLandUsePeople = result.LandUsePeople,
                sumLandUseType = result.LandUseType,
                sumLandUseArea = result.LandUseArea,
                //抵押权状况
                sumLandSpareYear = result.LandSpareYear,
                sumSpareEconomicsYear = result.SpareEconomicsYear,
                sumMaxUseYear = result.MaxUseYear,
                sumLandUse = result.LandUse,
                sumLandEndTime = result.LandEndTime.HasValue ? result.LandEndTime.Value.ToString("yyyy-MM-dd") : string.Empty,
                sumOtherInfluenceFactor = result.OtherInfluenceFactor,
                sumBusinessTime = result.BusinessTime.HasValue ? result.BusinessTime.Value.ToString("yyyy-MM-dd") : string.Empty,
                //户数
                //地上物状况
                sumLoopLine = result.LoopLine,
                sumDealTotal = result.DealTotal,
                sumDealPrice = result.DealPrice
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 上传报告
        /// 作者：BOBO
        /// 时间：2016-4-7
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadReport()
        {
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = null;
            //是否有文件
            if (files.Count > 0)
            {
                file = files[0];
            }
            else
            {
                return Json(new { msg = "请上传文件！" }, "text/html", JsonRequestBehavior.AllowGet);
            }
            long projectId;
            long.TryParse(Request["projectId"] + "", out projectId);

            Stream fileStream = file.InputStream;
            string fileName = file.FileName;
            byte[] fileByte = new byte[file.ContentLength];
            fileStream.Read(fileByte, 0, file.ContentLength - 1);
            string result = ProjectResourceService.UploadProjectResource(projectId, 1, fileName, fileByte);

            //将返回设置为"text/html",解决ajaxSubmit 在IE8下不执行success，而是作为附件下载
            return Json(new { msg = result }, "text/html", JsonRequestBehavior.AllowGet); 
        }


        /// <summary>
        /// 项目审核
        /// 作者：BOBO
        /// 时间：2016-4-11
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditProject()
        {
            long projectId;
            long.TryParse(Request["projectId"] + "", out projectId);
            bool result = ProjectService.AuditProject(projectId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取项目报告的历史记录
        /// 作者：BOBO
        /// 时间：2016-4-13
        /// </summary>
        /// <returns></returns>
        public ActionResult GetReportHistoryByProjectId()
        {
            long projectId;
            long.TryParse(Request["projectId"] + "", out projectId);
            IList<ReportHistoryModel> result = ReportHistoryService.GetReportHistoryByProjectId(projectId);
            return Json(new
            {
                list = result.Select(p => new
                {
                    FileName = p.FileName,
                    ResourceId = p.ResourceId,
                    CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
                })
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查看历史报告
        /// 作者：BOBO
        /// 时间：2016-4-13
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewReporHistory()
        {
            long resourceId;
            long.TryParse(Request["resourceId"] + "", out resourceId);
            //获取资源库路径
            string reportFilePath = SingleFileManager.GetFileUrl(resourceId);
            string extension = Path.GetExtension(reportFilePath);
            //如果文件没有扩展名，则调用资源库接口去获取
            if (string.IsNullOrEmpty(extension))
            {
                extension = SingleFileManager.GetFileFormat(resourceId);
            }
            if (reportFilePath == null)
            {
                ViewBag.msg = "找不到对应文档";
                return View();
            }
            //本地路径
            string datestr = DateTime.Now.ToString("yyyyMMdd");
            string path = Server.MapPath("~/TempFile/" + datestr + "/");
            //判断路径是否存在
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string localFileName = path + Guid.NewGuid().ToString() + "." + extension;
            //下载资源
            WebClient webClient = new WebClient();
            webClient.DownloadFile(reportFilePath, localFileName);
            //PageOffice
            string controlOutput = string.Empty;
            PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
            Page page = new Page();
            //pc.SaveFilePage = "/ProjectReview/SaveDoc?localFileName=" + localFileName;
            pc.ServerPage = "/pageoffice/server.aspx";
            pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            //支持office,金山办公软件打开
            pc.OfficeVendor = PageOffice.OfficeVendorType.AutoSelect;
            // 打开文档
            if (localFileName.IndexOf(".doc") > 0)
            {
                pc.WebOpen(localFileName, PageOffice.OpenModeType.docNormalEdit, "Tom");
            }
            else if (localFileName.IndexOf(".xls") > 0)
            {
                pc.WebOpen(localFileName, PageOffice.OpenModeType.xlsNormalEdit, "Tom");
            }
            else if (localFileName.IndexOf(".ppt") > 0)
            {
                pc.WebOpen(localFileName, PageOffice.OpenModeType.pptNormalEdit, "Tom");
            }
            else
            {
                pc.Visible = false;
            }
            pc.ID = "pc";
            page.Controls.Add(pc);
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Server.Execute(page, htw, false); controlOutput = sb.ToString();
                }
            }
            ViewBag.EditorHtml = controlOutput;
            return View();
        }


        /// <summary>
        /// 下载报告
        /// 作者：BOBO
        /// 时间：
        /// </summary>
        /// <returns></returns>
        public ActionResult DownReport()
        {
            string[] downLoadInfo = { "ReportInfo", "LianJiaInfo", "OutTaskInfo" };
            long projectId;
            long.TryParse(Request["projectId"] + "", out projectId);
            string projectNo = Request["ProjectNo"] + "";
            byte[] result = ProjectService.ReportResourcesZip(projectId, downLoadInfo);
            return File(result, "application/octet-stream", projectNo + ".zip");
        }

        #region 汇总数据修改
        /// <summary>
        /// 汇总数据修改页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AggregatedData(long id)
        {
            ViewBag.PropertyNatures = base.GetSelectList("产权性质");
            ViewBag.BuildingStructures = base.GetSelectList("建筑结构");
            ViewBag.LandUseTypes = base.GetSelectList("土地使用权类型");
            ViewBag.LandUses = base.GetSelectList("土地用途");
            ViewBag.LoopLines = base.GetSelectList("环线");
            ViewBag.ReportValidityTerms = base.GetSelectList("报告有效期");

            SummaryDataModel model = SummaryDataService.GetById(id);
            if (model == null)
            {
                model = new SummaryDataModel();
            }
            return View(model);
        }

        /// <summary>
        /// 汇总页面修改保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SaveAggregatedData(SummaryDataModel model)
        {
            return Help.ExceptionCatch.Invoke(() =>
            {
                model.CreateTime = DateTime.Now;
                SummaryDataService.Save(model);
            });
        }
        #endregion

    }
}
