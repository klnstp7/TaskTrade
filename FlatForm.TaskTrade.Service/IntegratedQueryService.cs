using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Net;

using ICSharpCode.SharpZipLib.Zip;
using NPOI.SS.Formula.Functions;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Repository.Repositories;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Model.Condition;
using Peacock.Common.Exceptions;
using Peacock.PEP.Service.Extensions;
using ResourceLibraryExtension.Helper;
using ResourceLibraryExtension.Untity.Enum;
using ResourceLibraryExtension.Untity;
using Peacock.Common.Helper;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Common;

namespace Peacock.PEP.Service
{
    public class IntegratedQueryService : SingModel<IntegratedQueryService>
    {
        private IntegratedQueryService()
        {
        }

        #region 下载报告
        /// <summary>
        /// 下载报告
       /// </summary>
       /// <param name="projectId"></param>
        /// <param name="DownLoadInfo">压缩包文件路径</param>
       /// <returns></returns>
        public byte[] ReportResourcesZip(long projectId, string[] DownLoadInfo)
        {            
            Project project = ProjectRepository.Instance.Find(x => x.Id == projectId).FirstOrDefault();
            MemoryStream memoryStream = new MemoryStream();
            byte[] result = null;
            using (ZipOutputStream zipStream = new ZipOutputStream(memoryStream))
            {
                string refConcent = "";
                if (DownLoadInfo.Contains("ReportInfo"))
                {
                    refConcent += "报告资料.";
                    WriteReportInfo(zipStream, project);
                }                    
                if (DownLoadInfo.Contains("OutTaskInfo"))
                {
                    refConcent += "外采资料.";
                    WriteOutTaskInfo(zipStream, project);
                }                    
                if (DownLoadInfo.Contains("LianJiaInfo"))
                {
                    refConcent += "链家资料.";
                    WriteLianJiaInfo(zipStream, project);
                }                    
                zipStream.Finish();
                result = memoryStream.ToArray();
                zipStream.Close();
                LogHelper.Ilog("ReportResourcesZip?projectId=" + projectId + "&DownLoadInfo=" + DownLoadInfo.ToJson(), "下载报告"+ refConcent + "-" + Instance.ToString());                
            }
            return result;
        }

        /// <summary>
        /// 报告资料
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="project"></param>
        private void WriteReportInfo(ZipOutputStream zipStream, Project project)
        {            
            IList<ProjectResource> resources = ProjectResourceRepository.Instance.Find(x => x.ProjectId== project.Id).ToList();
            int index = 1;
            foreach (var item in resources)
            {
                string sourceUrl = SingleFileManager.GetFileUrl(item.ResourceId);
                if (string.IsNullOrEmpty(sourceUrl)) continue;
                using (WebClient web = new WebClient())
                {
                    string fileName = string.Format("{0}/{1}_{2}", "内业资料", index++, item.FileName);
                    byte[] data = web.DownloadData(sourceUrl);
                    string extension = Path.GetExtension(fileName);
                    if (string.IsNullOrEmpty(extension))//如果文件没有扩展名，则调用资源库接口去获取
                    {
                        extension = SingleFileManager.GetFileFormat(item.ResourceId);
                        fileName = string.Format("{0}.{1}", fileName, extension);
                    }
                    ZipEntry entry = new ZipEntry(fileName);
                    entry.DateTime = DateTime.Now;
                    zipStream.PutNextEntry(entry);
                    zipStream.Write(data, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// 外采资料
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="project"></param>
        private void WriteOutTaskInfo(ZipOutputStream zipStream, Project project)
        {            
            string url = string.Format("{0}?projectNo={1}", ConfigurationManager.AppSettings["OutTaskFileDownload"], project.ProjectNo);
            WebClient web = new WebClient();
            byte[] receive = web.DownloadData(url);
            if (receive.Length <= 0)
                return;
            ZipEntry entry = new ZipEntry("外采资料/外采资料包.zip");
            entry.DateTime = DateTime.Now;
            zipStream.PutNextEntry(entry);
            zipStream.Write(receive, 0, receive.Length);

        }

        /// <summary>
        /// 链家资料
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="project"></param>
        private void WriteLianJiaInfo(ZipOutputStream zipStream, Project project)
        {            
            IList<ProjectDocument> resources = ProjectDocumentRepository.Instance.Find(x => x.ProjectId == project.Id).ToList();
            int index = 1;
            foreach (var item in resources)
            {
                var sourceUrl = SingleFileManager.GetFileUrl(item.ResourceId);
                if (string.IsNullOrEmpty(sourceUrl)) continue;
                using (WebClient web = new WebClient())
                {
                    string fileName = string.Format("{0}/{1}_{2}", "链家资料", index++, item.FileName);
                    byte[] data = web.DownloadData(sourceUrl);
                    string extension = Path.GetExtension(fileName);
                    if (string.IsNullOrEmpty(extension))//如果文件没有扩展名，则调用资源库接口去获取
                    {
                        extension = SingleFileManager.GetFileFormat(item.ResourceId);
                        fileName = string.Format("{0}.{1}", fileName, extension);
                    }
                    ZipEntry entry = new ZipEntry(fileName);
                    entry.DateTime = DateTime.Now;
                    zipStream.PutNextEntry(entry);
                    zipStream.Write(data, 0, data.Length);
                }
            }
        }
        #endregion

        public SummaryData GetByProjectId(long projectId)
        {
            if (projectId <= 0)
                return null;
            var pj = ProjectRepository.Instance.Source.FirstOrDefault(x => x.Id == projectId);
            if (pj != null && pj.SummaryData != null)
                return pj.SummaryData;
            else
                return null;
            //var entity = SummaryDataRepository.Instance.Find(p => p.Project == pj).FirstOrDefault();
            //return entity;
        }

        public List<Project> Search(IntegratedQueryCondition condition, int? index, int? size, out int total)
        {
            var result = new List<Project>();
            total = 0;
            var query = ProjectRepository.Instance.Source;
            query = query.FindByPower();

            if (condition.DepartmentId.HasValue && condition.DepartmentId > 0)
            {
                query = query.Where(x => x.DepartmentId == condition.DepartmentId);
            }
            if (!string.IsNullOrEmpty(condition.ProjectNo))
                query = query.Where(x => x.ProjectNo.Contains(condition.ProjectNo));
            if (!string.IsNullOrEmpty(condition.ReportNo))
                query = query.Where(x => x.ReportNo.Contains(condition.ReportNo));
            if (!string.IsNullOrEmpty(condition.Address))
                query = query.Where(x => x.ProjectAddress.Contains(condition.Address));
            if (!string.IsNullOrEmpty(condition.ResidentialAreaName))
                query = query.Where(x => x.ResidentialAreaName.Contains(condition.ResidentialAreaName));
            if (!string.IsNullOrEmpty(condition.Bank))
                query = query.Where(x => x.Customer.Bank.Contains(condition.Bank));
            if (!string.IsNullOrEmpty(condition.Subbranch))
                query = query.Where(x => x.Customer.Subbranch.Contains(condition.Subbranch));
            if (!string.IsNullOrEmpty(condition.CustomerName))
                query = query.Where(x => x.Customer.CustomerName.Contains(condition.CustomerName));
            if (!string.IsNullOrEmpty(condition.CustomerTel))
                query = query.Where(x => x.Customer.Tel.Contains(condition.CustomerTel));
            if (!string.IsNullOrEmpty(condition.ContactsPhone))
            {
                query = query.Where(x => x.ExplorationContacts.Any(c => c.Phone.Contains(condition.ContactsPhone)));
            }
            if (!string.IsNullOrEmpty(condition.PropertyType) )
                query = query.Where(x => x.PropertyType == condition.PropertyType);
            if (!string.IsNullOrEmpty(condition.ReportType))
                query = query.Where(x => x.ReportType == condition.ReportType);
            if (!string.IsNullOrEmpty(condition.CustomerPhone))
                query = query.Where(x => x.Customer.Phone.Contains(condition.CustomerPhone));
            if (!string.IsNullOrEmpty(condition.ContactsName))
            {
                query = query.Where(x => x.ExplorationContacts.Any(c => c.Contacts.Contains(condition.ContactsName)));
            }
            if (!string.IsNullOrEmpty(condition.Remark))
                query = query.Where(x => x.Remark.Contains(condition.Remark));
            if (condition.CreateTimeBegin.HasValue)
                query = query.Where(x => x.CreateTime >= condition.CreateTimeBegin.Value);
            if (condition.CreateTimeEnd.HasValue)
            {
                var endDate = Convert.ToDateTime( condition.CreateTimeEnd.Value.ToString("yyyy-MM-dd 23:59:59"));
                query = query.Where(x => x.CreateTime < endDate);
            }
            if (!string.IsNullOrEmpty(condition.State))
            {
                query = query.Where(x => x.IsCancle == (condition.State == "取消"));
            }

            query = query.OrderByDescending(x => x.Id);
            if (size.HasValue && index.HasValue)
                result = ProjectRepository.Instance.FindForPaging(size.Value, index.Value, query, out total).ToList();
            else
                result = query.ToList();
            return result;
        }

        public DataTable Search(List<string> columns, IntegratedQueryCondition condition, int? index, int? size, out int total){
            if (columns != null && columns.Count > 0)
            {
                columns.Add("ProjectId");
                columns.Add("sumId");
                columns.Add("DepartmentId");
                if (!columns.Any(c => c == "ProjectNo"))
                {
                    columns.Add("ProjectNo");
                } 
            }
            
            return Search(columns,condition,index,size,out total,"");
        }
        public DataTable Search(List<string> columns, IntegratedQueryCondition condition, int? index, int? size, out int total,string searchType)
        {
            if(!index.HasValue)
                index=1;
            if(!size.HasValue )
                size=10;
            var user = UserService.Instance.GetUser();
            var company = ConfigurationManager.AppSettings["PersonalUserCompany"];
             var departmentList = UserService.Instance.GetDepartmentsByCompany(null, user.UserAccount);
             string deptIds = "";
             if (departmentList.FirstOrDefault().CompanyName != company)
             {
                 deptIds = string.Join(",", departmentList.Select(c => c.Id));
             }
              total = 0;
             MySql.Data.MySqlClient.MySqlParameter[] paramers = new MySql.Data.MySqlClient.MySqlParameter[25];
             paramers[0] = new MySql.Data.MySqlClient.MySqlParameter("exceType", searchType);
            paramers[1] = new MySql.Data.MySqlClient.MySqlParameter("iIndex",index);
            paramers[2] = new MySql.Data.MySqlClient.MySqlParameter("iSize",size);
            paramers[3] = new MySql.Data.MySqlClient.MySqlParameter("columnSel",string.Join(",",columns));
            paramers[4] = new MySql.Data.MySqlClient.MySqlParameter("currentUser",user.Id);
            paramers[5] = new MySql.Data.MySqlClient.MySqlParameter("departIds",deptIds);
            paramers[6] = new MySql.Data.MySqlClient.MySqlParameter("deptId",condition.DepartmentId.HasValue?condition.DepartmentId.Value:0);
            paramers[7] = new MySql.Data.MySqlClient.MySqlParameter("projectNo",condition.ProjectNo??"");
            paramers[8] = new MySql.Data.MySqlClient.MySqlParameter("reportNo",condition.ReportNo??"");
            paramers[9] = new MySql.Data.MySqlClient.MySqlParameter("address",condition.Address??"");
            paramers[10] = new MySql.Data.MySqlClient.MySqlParameter("areaName",condition.ResidentialAreaName??"");
            paramers[11] = new MySql.Data.MySqlClient.MySqlParameter("bank",condition.Bank??"");
            paramers[12] = new MySql.Data.MySqlClient.MySqlParameter("subBank",condition.Subbranch??"");
            paramers[13] = new MySql.Data.MySqlClient.MySqlParameter("customerName",condition.CustomerName??"");
            paramers[14] = new MySql.Data.MySqlClient.MySqlParameter("customerTel",condition.CustomerTel??"");
            paramers[15] = new MySql.Data.MySqlClient.MySqlParameter("customerPhone",condition.CustomerPhone??"");
            paramers[16] = new MySql.Data.MySqlClient.MySqlParameter("contactName",condition.ContactsName??"");
            paramers[17] = new MySql.Data.MySqlClient.MySqlParameter("contactPhone",condition.ContactsPhone??"");
            paramers[18] = new MySql.Data.MySqlClient.MySqlParameter("propertyType",condition.PropertyType??"");
            paramers[19] = new MySql.Data.MySqlClient.MySqlParameter("reportType",condition.ReportType??"");
            paramers[20] = new MySql.Data.MySqlClient.MySqlParameter("remark",condition.Remark??"");
            paramers[21] = new MySql.Data.MySqlClient.MySqlParameter("createTimeBegin",condition.CreateTimeBegin.HasValue?condition.CreateTimeBegin.Value.ToString("yyyy-MM-dd 00:00:00"):"");
            paramers[22] = new MySql.Data.MySqlClient.MySqlParameter("createTimeEnd", condition.CreateTimeEnd.HasValue ? condition.CreateTimeEnd.Value.ToString("yyyy-MM-dd 23:59:59") : "");
            paramers[23] = new MySql.Data.MySqlClient.MySqlParameter("isCancle",condition.State=="取消"?1:0);
            paramers[24] = new MySql.Data.MySqlClient.MySqlParameter("itotal", total);
            paramers[24].Direction = ParameterDirection.InputOutput; 
           var table= ProjectRepository.Instance.ExecuteFunction("P_DB_IntegratedQuery", paramers);
           total = Convert.ToInt32(paramers[24].Value);

           if (columns.Count==0 || columns.Contains("DepartmentName"))
           {
               foreach (DataRow row in table.Rows)
               {
                   if (row["DepartmentId"] != null)
                       row["DepartmentName"] = departmentList.Where(c => c.Id == Convert.ToInt64(row["DepartmentId"])).Select(c => c.CompanyName).FirstOrDefault();
               }
           }
           return table;
        }

        public DataTable Export(List<string> columns, IntegratedQueryCondition condition)
        {
            columns.Add("DepartmentId");
            int index = 1;
            int size = 100;
            int total = 0;
            return Search(columns, condition, index, size, out total, "导出");
        }
    }
}
