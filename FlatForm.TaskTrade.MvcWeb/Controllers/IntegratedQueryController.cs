using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.MvcWebSite.Help;
using Peacock.PEP.Model.Enum;
using System.Data;
using Peacock.Common.Helper;
using System.Reflection;
using System.Collections;

namespace Peacock.PEP.MvcWebSite.Controllers
{
    public class IntegratedQueryController : BaseController
    {
        /// <summary>
        /// 综合查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var querySolution = ConfigSolutionService.GetUserSolutionByUserId(SolutionUseType.列表, "ProjectQuery");
            var currentPropotery = ParameterAdapter.GetDataByName("物业类型");
            if(currentPropotery !=null){
                var PropoteryList = ParameterAdapter.GetListByParentId(currentPropotery.Id);
                ViewBag.PropoteryList = PropoteryList;
            }
            var reportType = ParameterAdapter.GetDataByName("报告类型");
            if (reportType != null)
            {
                var reportTypeList = ParameterAdapter.GetListByParentId(reportType.Id);
                ViewBag.ReportTypeList = reportTypeList;
            }
            var departlist = UserService.GetDepartmentsByCompany().ToDictionary(x => x.Id, x => x.CompanyName);
            var personal = departlist.FirstOrDefault(x => x.Value == "个人用户组");
            if (personal.Key > 0 && personal.Value != null)
                departlist.Remove(personal.Key);
            ViewBag.DepartList = departlist;
            ViewBag.querySolution = querySolution;
            return View();
        }

        /// <summary>
        /// 下载报告页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadFiles(long projectId, string projectNo)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.ProjectNo = projectNo;
            return View();
        }

        /// <summary>
        /// 下载报告
        /// </summary>
        /// <param name="ProjectNo"></param>
        /// <returns></returns>
        public ActionResult GetFileUrl(long projectId, string projectNo, string[] DownLoadInfo)
        {
            byte[] source = IntegratedQueryService.ReportResourcesZip(projectId, DownLoadInfo);
            return File(source, "application/octet-stream",projectNo + ".zip");  //按时间命名
        }


        /// <summary>
        /// 综合查询详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(long id)
        {
            SummaryDataModel model = SummaryDataService.GetById(id);
            if (model == null)
            {
                model = new SummaryDataModel();
            }
            return View(model);
        }


        public ActionResult ConfigListFunction()
        {

            return View();
        }

        public JsonResult GetSolutionList(string SolutionType, string FunCode, int pageNumber, int pageSize)
        {
            var total = 0;
            var result = ConfigSolutionService.GetList(SolutionType, FunCode, pageNumber, pageSize, out total);
            var jsonresult = new
            {
                rows = result.Select(x =>
                {
                    return new
                    {
                        x.tid,
                        x.SolutionName,
                        SolutionType = x.SolutionType.ToString(),
                        SolutionTypeEnum = x.SolutionType,
                        IsDefault = x.IsDefault ? "是" : "否",
                        x.Remark,
                        ConfigListFunctionId = x.ConfigListFunction.tid
                    };
                }),
                total
            };
            return Json(jsonresult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSolutionInfo(ConfigSolutionModel model)
        {
            return ExceptionCatch.Invoke(() =>
            {
                ConfigSolutionService.SaveSolution(model);
            });
        }
        [HttpPost]
        public JsonResult SetSolutionIsDefault(long SolutionId, SolutionUseType SolutionType)
        {
            return ExceptionCatch.Invoke(() =>
            {
                ConfigSolutionService.SetSolutionIsDefault(SolutionId, SolutionType);
            });
        }

        [HttpPost]
        public JsonResult DeleteSolution(long SolutionId)
        {
            return ExceptionCatch.Invoke(() =>
            {
                ConfigSolutionService.DeleteSolutionById(SolutionId);
            });
        }

        public ActionResult AddConfigListFunction(string FuncCode, long? SolutionId)
        {
            if (SolutionId == null || SolutionId < 1)
            {
                ViewBag.FuncID = ConfigSolutionService.GetConfigListFunctionByFuncCode(FuncCode).tid;
                ViewBag.Soution = new ConfigSolutionModel();
            }
            else
            {
                var SoutionInfo = ConfigSolutionService.GetSolutionEntityById(SolutionId.Value);
                ViewBag.FuncID = SoutionInfo.FuncID;
                ViewBag.Soution = SoutionInfo;
            }
            return View();
        }

        public ActionResult SetMyConfigListFunction(long SolutionId, string FuncCode, SolutionUseType Solutiontype)
        {
            ViewBag.UserFuncCols = ConfigSolutionService.GetUserConfigColBySolutionId(SolutionId, FuncCode,"Edit");
            ViewBag.FuncCols = ConfigSolutionService.GetConfigFuncColListByFuncCode(FuncCode, Solutiontype);
            return View();
        }

        /// <summary>
        /// 设置方案所拥有的列
        /// </summary>
        /// <param name="SolutionId"></param>
        /// <param name="ColIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveSolutionColInfo(long SolutionId, string ColIds)
        {
            return ExceptionCatch.Invoke(() =>
            {
                ConfigSolutionService.SaveSolutionColInfo(SolutionId, ColIds.Split(',').Select(x => long.Parse(x)).ToList());
            });
        }

        /// <summary>
        /// 根据用户选择的方案，查询出配置的列
        /// </summary>
        /// <param name="SolutionId"></param>
        /// <param name="FuncCode"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSolutionCols(long SolutionId, string FuncCode)
        {
            var colList = ConfigSolutionService.GetUserConfigColBySolutionId(SolutionId, FuncCode);
            var colwidth = 90 / colList.Count;
            var cols = new
            {
                columns = colList.Select(x =>
                {
                     return new
                        {
                            title = x.ColName,
                            field = x.ColCode,
                            width = colwidth + "%",
                            valign = "middle",
                            align = "left"
                        };
                    //if (colList.Count <= 9)
                    //{
                    //    return new
                    //    {
                    //        title = x.ColName,
                    //        field = x.ColCode,
                    //        width = colwidth + "%",
                    //        valign = "middle",
                    //        align = "left"
                    //    };
                    //}
                    //else
                    //{
                    //    return new
                    //    {
                    //        title = x.ColName,
                    //        field = x.ColCode,
                    //        width = "11%",
                    //        valign = "middle",
                    //        align = "left"
                    //    };
                    //}
                })
            };
            return Json(cols, JsonRequestBehavior.AllowGet);
        }

         


        public FileResult ExportData(IntegratedQueryCondition condition, long SolutionId)
        {
            var colList = ConfigSolutionService.GetUserConfigColBySolutionId(SolutionId, "ProjectQuery");
            var table = IntegratedQueryService.Export(colList.Select(c => c.ColCode).ToList(), condition); 
            var excel = ExcelHelper.DataTableToExcel(table, colList.ToDictionary(c => c.ColName, c => c.ColCode.Trim())); 
            return File(excel, "application/octet-stream", "综合查询数据导出" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }
         
        /// <summary>
        /// 综合查询
        /// </summary>
        /// <returns></returns>
        [JsonException]
        public JsonResult Search(IntegratedQueryCondition condition, int index, int rows, long SolutionId)
        {
            int total;
            var colList = ConfigSolutionService.GetUserConfigColBySolutionId(SolutionId, "ProjectQuery");
            var result = IntegratedQueryService.Search(colList.Select(c => c.ColCode).ToList(), condition, index, rows,
                out total);
            var query = ToArray(result);
            return Json(new
            {
                rows = query,
                total
            }, JsonRequestBehavior.AllowGet);
        }

        private ArrayList ToArray(DataTable dt)
        {
            var arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                var dictionary = new Dictionary<string, object>();
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary);
            }
            return arrayList;
        }
    }
}
