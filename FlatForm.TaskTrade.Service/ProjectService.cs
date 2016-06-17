using System;
using System.Collections.Generic;
using System.Linq;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Data.Entities;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Configuration;
using System.Net;
using Peacock.PEP.Service.Extensions;
using ResourceLibraryExtension.Helper;
using Peacock.PEP.Repository.Repositories;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model.Enum;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Model;
using Peacock.Common.Exceptions;
using Peacock.Common.Helper;
using EIAS.Models;
using Peacock.PEP.Service.API;

namespace Peacock.PEP.Service
{
    public class ProjectService : SingModel<ProjectService>
    {
        private ProjectService()
        {
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="p">项目信息</param>
        /// <param name="model">客户信息</param>
        /// <param name="listExplorationContacts">外业查勘联系人信息</param>
        /// <returns></returns>
        public long Add(Project p, ProjectModel model, List<ExplorationContacts> listExplorationContacts)
        {
            LogHelper.Ilog("Add?Project=" + p.ToJson() + "&listExplorationContacts=" + listExplorationContacts.ToJson(), "立项-" + Instance.ToString());            
            long projectId = 0;
            Customer cust = new Customer();
            cust.CustomerName = model.CustomerName ?? string.Empty;
            cust.Tel = model.CustomerTel;
            cust.Phone = model.CustomerPhone;
            cust.Bank = model.CustomerBank;
            cust.Subbranch = model.CustomerSubbranch;
            cust.QQ = model.CustomerQQ;
            cust.Id = model.CustomerId;

            Project entity = new Project();
            entity = p;
            entity.CreateTime = DateTime.Now;
            entity.IsUploadProject = false;//IsUploadProject
            entity.ApproverId = 0;//审核人ID
            entity.IsSent = false;//是否发送
            entity.IsApprove = false;//是否审核通过
            entity.IsSubmitted = false;//是否提交审核
            entity.Deleted = false;//是否已删除

            entity.ProjectNo = GetInstanceNo();
            entity.ProjectCreatorId = UserService.Instance.GetCrmUser().Id;
            entity.ProjectCreatorName = UserService.Instance.GetCrmUser().UserAccount;
            entity.DepartmentId = UserService.Instance.GetUserDepartment().Id;

            if (string.IsNullOrWhiteSpace(entity.ProjectAddress))
            {
                throw new ServiceException("项目地址不能为空!");
            }

            if (string.IsNullOrWhiteSpace(entity.ProjectSource))
            {
                throw new ServiceException("项目来源不能为空!");
            }
            if (string.IsNullOrWhiteSpace(entity.BusinessType))
            {
                throw new ServiceException("估价目的不能为空!");
            }
            if (string.IsNullOrWhiteSpace(entity.PropertyType))
            {
                throw new ServiceException("物业类型不能为空!");
            }
            if (string.IsNullOrWhiteSpace(entity.ProjectNo))
            {
                throw new ServiceException("流水号不能为空!");
            }
            //汇总数据
            SummaryData sdata = new SummaryData();
            sdata.Company = UserService.Instance.GetUserCompany().CompanyName;
            sdata.Department = UserService.Instance.GetUserDepartment().CompanyName;
            sdata.ReportNo = entity.ReportNo;
            sdata.CreateTime = DateTime.Now;
            sdata.Id = 0;
            ProjectRepository.Instance.Transaction(() =>
            {
                Customer Customer = null;
                if (cust != null)
                {
                    Customer = cust;
                    Customer.CreateTime = DateTime.Now;
                    CustomerRepository.Instance.Insert(Customer);
                }
                if (Customer != null)
                {//客户id
                    entity.CustomerId = Customer.Id;
                }
                entity.SummaryData = sdata;
                ProjectRepository.Instance.Insert(entity);

                projectId = entity.Id;

                foreach (var item in listExplorationContacts)
                {
                    item.CreateTime = DateTime.Now;
                    item.ProjectId = entity.Id;
                    if (string.IsNullOrWhiteSpace(item.Contacts))
                    {
                        throw new ServiceException("联系人不能为空!");
                    }
                    if (string.IsNullOrWhiteSpace(item.Phone))
                    {
                        throw new ServiceException("联系电话不能为空!");
                    }
                }
                ExplorationContactsRepository.Instance.Insert(listExplorationContacts);

                if (entity.IsProspecting)
                {
                    //调用外采系统分配外采任务
                    OutTaskService.Instance.AssignedOutTask(entity, Customer, listExplorationContacts);
                    //添加流程记录(立项)
                }


            });
            ProjectStateInfoService.Instance.WriteInProjectStateInfo(projectId, StateInfoEnum.立项);
            return projectId;
        }

        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="pmodel"></param>
        /// <param name="listExplorationContacts"></param>
        /// <returns></returns>
        public long Update(ProjectModel pmodel, List<ExplorationContacts> listExplorationContacts)
        {
            LogHelper.Ilog("Add?ProjectModel=" + pmodel.ToJson() + "&List<ExplorationContacts>=" + listExplorationContacts.ToJson(), "修改项目-" + Instance.ToString());
            long projectId = 0;
            ProjectRepository.Instance.Transaction(() =>
            {
                Customer Customer = CustomerRepository.Instance.Find(x => x.Id == pmodel.CustomerId).FirstOrDefault();

                if (Customer != null)
                {
                    Customer.CustomerName = pmodel.CustomerName ?? string.Empty;// pmodel.CustomerName;
                    Customer.Tel = pmodel.CustomerTel;
                    Customer.Phone = pmodel.CustomerPhone;
                    Customer.Bank = pmodel.CustomerBank;
                    Customer.Subbranch = pmodel.CustomerSubbranch;
                    Customer.QQ = pmodel.CustomerQQ;
                    CustomerRepository.Instance.Save(Customer);
                }

                Project entity = ProjectRepository.Instance.Find(x => x.Id == pmodel.ProjectId).FirstOrDefault();
                bool OldIsProspecting = entity.IsProspecting;//记录旧的项目是否需要外采
                entity.ReportNo = pmodel.ReportNo;
                entity.PropertyType = pmodel.PropertyType;
                entity.BusinessType = pmodel.BusinessType;
                entity.ProjectSource = pmodel.ProjectSource;
                entity.OutSurveyTableId = pmodel.OutSurveyTableId;
                // entity.ProjectType = pmodel.ProjectType;
                // entity.DepartmentId=;
                // entity.ProjectCreatorId=;
                entity.ProjectAddress = pmodel.ProjectAddress;
                entity.ResidentialAreaName = pmodel.ResidentialAreaName;
                entity.ResidentialAddress = pmodel.ResidentialAddress;
                entity.BuildingArea = pmodel.BuildingArea;
                entity.City = pmodel.City;
                entity.District = pmodel.District;
                entity.BuildedYear = pmodel.BuildedYear;
                entity.InquiryResult = pmodel.InquiryResult;
                entity.InquiryPrice = pmodel.InquiryPrice;
                entity.EmergencyLevel = pmodel.EmergencyLevel;
                entity.Principal = pmodel.Principal;
                entity.CreditBank = pmodel.CreditBank;
                entity.CreditSubbranch = pmodel.CreditSubbranch;
                entity.CreditInfo = pmodel.CreditInfo;
                entity.IsProspecting = pmodel.IsProspecting;
                entity.Investigator = pmodel.Investigator;
                entity.Remark = pmodel.Remark;
                //entity.IsSubmitted=;
                entity.ReportType = pmodel.ReportType;


                if (string.IsNullOrWhiteSpace(entity.ProjectAddress))
                {
                    throw new ServiceException("项目地址不能为空!");
                }
                if (string.IsNullOrWhiteSpace(entity.ProjectSource))
                {
                    throw new ServiceException("项目来源不能为空!");
                }
                if (string.IsNullOrWhiteSpace(entity.BusinessType))
                {
                    throw new ServiceException("估价目的不能为空!");
                }
                if (string.IsNullOrWhiteSpace(entity.PropertyType))
                {
                    throw new ServiceException("物业类型不能为空!");
                }
                if (string.IsNullOrWhiteSpace(entity.ProjectNo))
                {
                    throw new ServiceException("流水号不能为空!");
                }
                entity.SummaryData.ReportNo = entity.ReportNo;
                ProjectRepository.Instance.Save(entity);
                projectId = entity.Id;

                //查出所有的该项目的看房联系人
                List<ExplorationContacts> listE = ExplorationContactsRepository.Instance.Find(x => x.ProjectId == projectId).ToList();

                //删除所有看房联系人
                ExplorationContactsRepository.Instance.Delete(listE);

                foreach (var item in listExplorationContacts)
                {
                    item.CreateTime = DateTime.Now;
                    item.ProjectId = entity.Id;
                    if (string.IsNullOrWhiteSpace(item.Contacts))
                    {
                        throw new ServiceException("联系人不能为空!");
                    }
                    if (string.IsNullOrWhiteSpace(item.Phone))
                    {
                        throw new ServiceException("联系电话不能为空!");
                    }
                }

                ExplorationContactsRepository.Instance.Insert(listExplorationContacts);

                //调用外采系统分配外采任务
                if (entity.IsProspecting && OldIsProspecting == false)
                {
                    OutTaskService.Instance.AssignedOutTask(entity, Customer, listExplorationContacts);
                }

            });
            return projectId;

        }

        /// <summary>
        /// 获取项目列表
        /// 作者：BOBO
        /// 时间:2016-4-1
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<Project> GetProjectList(ProjectCondition condition, int pageIndex, int pageSize, out int total)
        {
            LogHelper.Ilog("GetProjectList?condition=" + condition.ToJson() + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "获取项目列表-" + Instance.ToString());
            var query = ProjectRepository.Instance.Source;

            //数据权限控制
            query = query.FindByPower();

            //判断流水号是否为空
            if (!string.IsNullOrEmpty(condition.ProjectNo))
            {
                query = query.Where(p => p.ProjectNo.Contains(condition.ProjectNo));
            }
            //判断报告号是否为空
            if (!string.IsNullOrEmpty(condition.ReportNo))
            {
                query = query.Where(p => p.ReportNo.Contains(condition.ReportNo));
            }
            //判断项目地址是否为空
            if (!string.IsNullOrEmpty(condition.ProjectAddress))
            {
                query = query.Where(p => p.ProjectAddress.Contains(condition.ProjectAddress));
            }
            //判断小区名称是否为空
            if (!string.IsNullOrEmpty(condition.ResidentialAreaName))
            {
                query = query.Where(p => p.ResidentialAreaName.Contains(condition.ResidentialAreaName));
            }
            //判断是否审核通过
            if (condition.IsApprove != null)
            {
                query = query.Where(p => p.IsApprove == condition.IsApprove.Value);
            }
            //判断是否发送报告
            if (condition.IsSent != null)
            {
                query = query.Where(p => p.IsSent == condition.IsSent.Value);
            }
            //项目类型
            //if (!string.IsNullOrWhiteSpace(condition.ProjectType))
            //{
            //    query = query.Where(p => p.ProjectType == condition.ProjectType);
            //}
            //项目立项人
            if (!string.IsNullOrWhiteSpace(condition.ProjectCreatorName))
            {
                // query = query.Where(p => p.ProjectCreatorName == condition.ProjectCreatorName);
            }
            //估价委托方
            if (!string.IsNullOrWhiteSpace(condition.Principal))
            {
                query = query.Where(p => p.Principal == condition.Principal);
            }
            //项目立项时间->起始
            if (condition.ProjectCreatorTimeStart.HasValue)
            {
                var Time = DateTime.Parse(condition.ProjectCreatorTimeStart.Value.ToString("yyyy-MM-dd 00:00:00"));
                query = query.Where(p => p.CreateTime >= Time);
            }
            //项目立项时间->结束
            if (condition.ProjectCreatorTimeEnd.HasValue)
            {
                //var Time = DateTime.Parse(condition.ProjectCreatorTimeStart.Value.ToString("yyyy-MM-dd 23:59:59"));
                var Time = DateTime.Parse(condition.ProjectCreatorTimeEnd.Value.ToString("yyyy-MM-dd 23:59:59"));
                query = query.Where(p => p.CreateTime <= Time);
            }
            //是否提交审核
            if (condition.IsSubmitted.HasValue)
            {
                query = query.Where(p => p.IsSubmitted == condition.IsSubmitted.Value);
            }
            if (!string.IsNullOrWhiteSpace(condition.BusinessType))
            {
                query = query.Where(p => p.BusinessType == condition.BusinessType);
            }
            //物业类型
            if (!string.IsNullOrEmpty(condition.PropertyType))
            {
                query = query.Where(p => p.PropertyType == condition.PropertyType);
            }
            if (!string.IsNullOrEmpty(condition.ProjectSource))
            {
                query = query.Where(x => x.ProjectSource == condition.ProjectSource);
            }
            query = query.OrderByDescending(p => p.Id);
            var result = ProjectRepository.Instance.FindForPaging(pageSize, pageIndex, query, out total).ToList();
            return result;
        }


        /// <summary>
        /// 报告文件打包
        /// 作者：BOBO
        /// 时间：2016-4-1
        /// </summary>
        /// <param name="ProjectNo"></param>
        /// <param name="DownLoadInfo"></param>
        /// <returns></returns>
        public byte[] ReportResourcesZip(long projectId, string[] DownLoadInfo)
        {
            Project project = ProjectRepository.Instance.Find(p => p.Id == projectId).FirstOrDefault();
            //判断项目是否存在
            if (project == null)
            {
                throw new Exception("项目不存在");
            }
            MemoryStream memoryStream = new MemoryStream();
            byte[] result = null;
            using (ZipOutputStream zipOutPutStream = new ZipOutputStream(memoryStream))
            {
                string refConcent = "";
                //报告资料
                if (DownLoadInfo.Contains("ReportInfo"))
                {
                    WriteReportInfo(zipOutPutStream, project);
                    refConcent += "报告资料.";
                }
                //链家资料
                if (DownLoadInfo.Contains("LianJiaInfo"))
                {
                    WriteLianJiaInfo(zipOutPutStream, project);
                    refConcent += "链家资料.";
                }
                //外采资料
                if (DownLoadInfo.Contains("OutTaskInfo"))
                {
                    WriteOutTaskInfo(zipOutPutStream, project);
                    refConcent += "外采资料.";
                }
                zipOutPutStream.Finish();
                result = memoryStream.ToArray();
                zipOutPutStream.Close();
                LogHelper.Ilog("ReportResourcesZip?ProjectNo=" + projectId + "&DownLoadInfo=" + DownLoadInfo.ToJson(), "下载报告(" + refConcent + ")" + Instance.ToString());
            }
            return result;
        }

        /// <summary>
        /// 写入报告资料
        /// 作者：BOBO
        /// 时间：2016-4-1
        /// </summary>
        /// <param name="zipOutputStream"></param>
        /// <param name="project"></param>
        private void WriteReportInfo(ZipOutputStream zipOutputStream, Project project)
        {
            ResourcesEnum[] downLoadReportTypes = new ResourcesEnum[] { ResourcesEnum.报告, ResourcesEnum.计算表 };
            List<ProjectResource> list = ProjectResourceRepository.Instance.Find(p => p.Project.Id == project.Id
                                                                               && downLoadReportTypes.Contains((ResourcesEnum)p.ResourceType)).ToList();
            int index = 1;
            foreach (var item in list)
            {
                string sourceUrl = SingleFileManager.GetFileUrl(item.ResourceId);
                //判断资源链接是否存在
                if (string.IsNullOrEmpty(sourceUrl))
                {
                    continue;
                }
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        string fileName = string.Format("{0}/{1}_{2}", "内业资料", index++, item.FileName);
                        string extension = Path.GetExtension(fileName);
                        //如果文件没有扩展名，则调用资源库接口去获取
                        if (string.IsNullOrEmpty(extension))
                        {
                            extension = SingleFileManager.GetFileFormat(item.ResourceId);
                            fileName = string.Format("{0}.{1}", fileName, extension);
                        }
                        byte[] result = webClient.DownloadData(sourceUrl);
                        ZipEntry entry = new ZipEntry(fileName);
                        entry.DateTime = DateTime.Now;
                        zipOutputStream.PutNextEntry(entry);
                        zipOutputStream.Write(result, 0, result.Length);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 写入链家资料
        /// 作者：BOBO
        /// 时间：2016-4-1
        /// </summary>
        /// <param name="zipOutputStream"></param>
        /// <param name="project"></param>
        private void WriteLianJiaInfo(ZipOutputStream zipOutputStream, Project project)
        {
            List<ProjectDocument> list = ProjectDocumentRepository.Instance.Find(p => p.ProjectId == project.Id).ToList();
            int index = 1;
            foreach (var item in list)
            {
                var sourceUrl = SingleFileManager.GetFileUrl(item.ResourceId);
                //判断资源URL是否为空 
                if (!string.IsNullOrEmpty(sourceUrl))
                {
                    continue;
                }
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        byte[] result = webClient.DownloadData(sourceUrl);
                        string fileName = string.Format("{0}/{1}_{2}", "链接资料", index++, item.FileName);
                        string extension = Path.GetExtension(fileName);
                        //如果文件没有扩展名，则调用资源库接口去获取
                        if (string.IsNullOrEmpty(extension))
                        {
                            extension = SingleFileManager.GetFileFormat(item.ResourceId);
                            fileName = string.Format("{0}.{1}", fileName, extension);
                        }
                        ZipEntry entry = new ZipEntry(fileName);
                        entry.DateTime = DateTime.Now;
                        zipOutputStream.PutNextEntry(entry);
                        zipOutputStream.Write(result, 0, result.Length);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 写入外采资料
        /// 作者：BOBO
        /// 时间：2016-4-1
        /// </summary>
        /// <param name="zipOutputStream"></param>
        /// <param name="project"></param>
        private void WriteOutTaskInfo(ZipOutputStream zipOutputStream, Project project)
        {
            string apiUrl = ConfigurationManager.AppSettings["EIASURL"].ToString() + "apis/GetTaskZip";
            string url = string.Format("{0}?projectNo={1}", apiUrl, project.ProjectNo);
            try
            {
                WebClient webClient = new WebClient();
                byte[] result = webClient.DownloadData(url);
                //判断是否有资料
                if (result.Length <= 0)
                {
                    return;
                }
                ZipEntry entry = new ZipEntry("外采资料/外采资料包.zip");
                entry.DateTime = DateTime.Now;
                zipOutputStream.PutNextEntry(entry);
                zipOutputStream.Write(result, 0, result.Length);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 根据Id获得项目
        /// 作者:BOBO
        /// 时间:2016-4-7
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Project GetProjectById(long projectId)
        {
            LogHelper.Ilog("GetProjectById?projectId=" + projectId, "获取项目-" + Instance.ToString());
            if (projectId == 0)
                return null;
            var query = from p in ProjectRepository.Instance.Source
                        where p.Id == projectId
                        select p;
            var project = query.FirstOrDefault();
            return project;
        }

        /// <summary>
        /// 添加项目返回Id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long Add(Project entity)
        {
            LogHelper.Ilog("Add?entity=" + entity.ToJson(), "添加项目-" + Instance.ToString());
            entity.CreateTime = DateTime.Now;
            SummaryData sdata = new SummaryData();
            sdata.Company = UserService.Instance.GetUserCompany().CompanyName;
            sdata.Department = UserService.Instance.GetUserDepartment().CompanyName;
            sdata.ReportNo = entity.ReportNo;
            sdata.CreateTime = DateTime.Now;
            sdata.Id = 0;

            entity.SummaryData = sdata;
            entity.ProjectNo = GetInstanceNo();
            ProjectRepository.Instance.Insert(entity);
            return entity.Id;
        }

        /// <summary>
        /// 项目提交审核
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ResultInfo ComitAudit(long projectId)
        {
            LogHelper.Ilog("ComitAudit?projectId=" + projectId, "项目提交审核-" + Instance.ToString());
            ResultInfo result = new ResultInfo();

            ProjectRepository.Instance.Transaction(() =>
           {
               Project pj = ProjectRepository.Instance.Find(x => x.Id == projectId).FirstOrDefault();
               ReportFilePathInfoDTO DTO = new ReportAPIClient().GetReportAuditFilePath(pj.ProjectNo);

               List<ProjectResource> list = ProjectResourceRepository.Instance.Find(x => x.ProjectId == projectId).ToList();
               if (list.Count > 0)
               {
                   ProjectResourceRepository.Instance.Delete(list);
               }

               if (DTO != null && !string.IsNullOrWhiteSpace(DTO.WordPath))
               {
                   result.Success = true;
                   ProjectResource res = new ProjectResource();
                   res.CreateTime = DateTime.Now;
                   res.ResourceId = long.Parse(DTO.WordPath);
                   res.ResourceType = (int)ResourcesEnum.报告;
                   res.FileName = ResourcesEnum.报告.ToString();
                   res.ProjectId = projectId;
                   ProjectResourceRepository.Instance.Insert(res);
               }
               else
               {
                   result.Success = false;
                   result.Message = "此项目无报告，无法提交!";
               }

               if (DTO != null && !string.IsNullOrWhiteSpace(DTO.ExcelPath) && result.Success == true)
               {
                   ProjectResource res = new ProjectResource();
                   res.CreateTime = DateTime.Now;
                   res.ResourceId = long.Parse(DTO.ExcelPath);
                   res.ResourceType = (int)ResourcesEnum.计算表;
                   res.FileName = ResourcesEnum.计算表.ToString();
                   res.ProjectId = projectId;
                   ProjectResourceRepository.Instance.Insert(res);
               }

               if (result.Success)
               {
                   pj.IsSubmitted = true;
                   pj.SubmitTime = DateTime.Now;
                   ProjectRepository.Instance.Save(pj);
                   //写入流程记录(项目审核)
                   ProjectStateInfoService.Instance.WriteInProjectStateInfo(projectId, StateInfoEnum.提交审核);
                   //提取报告汇总数据
                   System.Threading.Tasks.Task.Factory.StartNew(() =>
                   {
                       SummaryDataService.Instance.ExtractionSummaryData(pj.ProjectNo);
                   });
               }
           });            
            return result;
        }

        /// <summary>
        /// 项目审核
        /// 作者：BOBO
        /// 时间：2016-4-11
        /// </summary>
        /// <returns></returns>
        public bool AuditProject(long projectId)
        {
            LogHelper.Ilog("AuditProject?projectId=" + projectId, "项目审核通过-" + Instance.ToString());
            if (projectId <= 0)
                return false;
            Project entity = ProjectRepository.Instance.Find(p => p.Id == projectId).FirstOrDefault();
            if (entity.IsSubmitted == false)
                return false;
            //已审核
            if (entity.IsApprove == true)
                return false;
            entity.IsApprove = true;
            entity.ApproveTime = DateTime.Now;
            entity.ApproverId = UserService.Instance.GetCrmUser().Id;
            entity.ApproverName = UserService.Instance.GetCrmUser().UserAccount;
            ProjectRepository.Instance.Save(entity);
            //写入流程记录(项目审核)
            ProjectStateInfoService.Instance.WriteInProjectStateInfo(projectId, StateInfoEnum.项目审核);
            //委托进度查询(报告完成)
            OnlineBusinessService.Instance.WriteFeedBackKafkaMQ(projectId, ProgressEnum.报告完成);
            return true;
        }

        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <returns></returns>
        private string GetInstanceNo()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            var instanceNo = BitConverter.ToInt64(buffer, 0).ToString().Substring(0, 10);
            if (ProjectRepository.Instance.Find(x => x.ProjectNo == instanceNo).Any())
            {
                instanceNo = GetInstanceNo();
            }

            return instanceNo;
        }

        /// <summary>
        /// 获取项目取消信息
        /// </summary>
        /// <returns></returns>
        public ProjectCancle GetProjectCancle(long projectId)
        {
            return ProjectCancleRepository.Instance.Source.FirstOrDefault(x => x.ProjectId == projectId);
        }
    }
}