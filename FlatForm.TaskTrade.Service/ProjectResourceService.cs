using Peacock.PEP.Data.Entities;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Repository.Repositories;
using ResourceLibraryExtension.Helper;
using Peacock.Common.Helper;

namespace Peacock.PEP.Service
{
    public class ProjectResourceService : SingModel<ProjectResourceService>
    {
        private ProjectResourceService()
        {
        }

        /// <summary>
        /// 根据项目Id和报告文档类型判断报告资源是否存在
        /// 作者：BOBO
        /// 时间：2016-4-6
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public int IsExist(long projectId, int resourceType)
        {
            if (projectId == 0 || resourceType == 0)
                return 0;
            var query = from pr in ProjectResourceRepository.Instance.Source
                        where pr.ProjectId == projectId && pr.ResourceType == resourceType
                        select pr;
            var result = query.Count();
            return result;
        }

        /// <summary>
        /// 添加报告资源并返回Id
        /// 作者：BOBO
        /// 时间：2016-4-6
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long AddProjectResource(ProjectResource entity)
        {
            LogHelper.Ilog("AddProjectResource?entity=" + entity.ToJson(), "添加报告资源-" + Instance.ToString());
            ProjectResourceRepository.Instance.Insert(entity);
            return entity.Id;
        }

        /// <summary>
        /// 更新报告资源
        /// 作者：BOBO
        /// 时间：2016-4-7
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateProjectResource(ProjectResource entity)
        {
            LogHelper.Ilog("UpdateProjectResource?entity=" + entity.ToJson(), "更新报告资源-" + Instance.ToString());            
            var temp = ProjectResourceRepository.Instance.Find(x => x.Id == entity.Id).FirstOrDefault();
            var project = ProjectRepository.Instance.Find(x => x.Id == entity.ProjectId).FirstOrDefault();
            temp.Id = entity.Id;
            temp.ProjectId = entity.ProjectId;
            temp.ResourceId = entity.ResourceId;
            temp.ResourceType = entity.ResourceType;
            temp.FileName = entity.FileName;
            temp.Project = project;
            ProjectResourceRepository.Instance.Save(temp);
            //新建报告写入报告历史记录
            ReportHistory history = new ReportHistory();
            history.CreateTime = DateTime.Now;
            history.CreatorId = UserService.Instance.GetCrmUser().Id;
            history.FileName = entity.FileName;
            history.ProjectId = entity.ProjectId;
            history.ResourceId = entity.ResourceId;
            ReportHistoryRepository.Instance.Insert(history);
        }

        /// <summary>
        /// 根据Id获取报告资源
        /// 作者：BOBO
        /// 时间：2016-4-7
        /// </summary>
        /// <param name="projectResourceId"></param>
        /// <returns></returns>
        public ProjectResource GetProjectResourceById(long projectResourceId)
        {
            LogHelper.Ilog("GetProjectResourceById?projectResourceId=" + projectResourceId, "获取报告资源-" + Instance.ToString());
            if (projectResourceId == 0)
                return null;
            var projectResource = ProjectResourceRepository.Instance.Find(p => p.Id == projectResourceId).FirstOrDefault();
            return projectResource;
        }

        /// <summary>
        /// 根据项目Id和报告类型获取报告资源
        /// 作者：BOBO
        /// 时间：2016-4-12
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public ProjectResource GetProjectResource(long projectId, int resourceType)
        {
            if (projectId == 0)
                return null;
            var projectResource = ProjectResourceRepository.Instance.Find(p => p.ProjectId == projectId && p.ResourceType == resourceType).FirstOrDefault();
            return projectResource;
        }

        /// <summary>
        /// 上传报告
        /// 作者:BOBO
        /// 时间:2016-4-8
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="resourceType"></param>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        public string UploadProjectResource(long projectId, int resourceType, string fileName, byte[] fileByte)
        {
            LogHelper.Ilog("UploadProjectResource?projectId=" + projectId + "&resourceType=" + resourceType + "&resourceType=" + resourceType + "&fileByte=" + fileByte.Length, "上传报告-" + Instance.ToString());
            if (projectId == 0)
                return "上传失败!";
            //使用事务有错
            //ProjectResourceRepository.Instance.Transaction(() =>
            //{
            //});

            //扩展名
            string extendName = fileName.Substring(fileName.LastIndexOf('.') + 1);
            //上传资源库
            long resourceId = SingleFileManager.SaveFileResource(fileByte, extendName, fileName, projectId.ToString());
            ProjectResource projectResource = ProjectResourceRepository.Instance.Find(p => p.ProjectId == projectId && p.ResourceType == resourceType).FirstOrDefault();
            //是否为空
            if (projectResource == null)
            {
                ProjectResource newEntity = new ProjectResource();
                newEntity.CreateTime = DateTime.Now;
                newEntity.FileName = fileName;
                newEntity.ProjectId = projectId;
                newEntity.ResourceId = resourceId;
                newEntity.ResourceType = resourceType;
                ProjectResourceRepository.Instance.Insert(newEntity);
                //上传报告写入历史记录
                ReportHistory history = new ReportHistory();
                history.CreateTime = DateTime.Now;
                history.CreatorId = UserService.Instance.GetCrmUser().Id;
                history.FileName = fileName;
                history.ProjectId = projectId;
                history.ResourceId = resourceId;
                ReportHistoryRepository.Instance.Insert(history);
                //写入项目流程，上传报告
                ProjectStateInfoService.Instance.WriteInProjectStateInfo(projectId, StateInfoEnum.报告上传);
            }
            else
            {
                ReportHistory history = new ReportHistory();
                history.CreateTime = DateTime.Now;
                history.CreatorId = UserService.Instance.GetCrmUser().Id;
                //history.FileName = projectResource.FileName;
                //history.ProjectId = projectResource.ProjectId;
                //history.ResourceId = projectResource.ResourceId;
                history.FileName = fileName;
                history.ProjectId = projectId;
                history.ResourceId = resourceId;
                ReportHistoryRepository.Instance.Insert(history);
                //更新报告表
                projectResource.FileName = fileName;
                projectResource.ProjectId = projectId;
                projectResource.ResourceId = resourceId;
                projectResource.ResourceType = resourceType;
                ProjectResourceRepository.Instance.Save(projectResource);
                //写入项目流程，上传报告
                ProjectStateInfoService.Instance.WriteInProjectStateInfo(projectId, StateInfoEnum.报告修改);
            }  
            return "上传成功!";
        }

        /// <summary>
        /// 保存pageOffice报告编辑
        /// 作者：BOBO
        /// 时间：2016-4-12
        /// </summary>
        /// <param name="projectResourceId"></param>
        /// <param name="projectId"></param>
        /// <param name="oldResourceId"></param>
        /// <param name="newResourceId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string SaveReport(long projectResourceId, long projectId, long oldResourceId, long newResourceId, string fileName)
        {
            string result = "保存报告成功!";
            if (projectId <= 0 || projectResourceId <= 0)
            {
                result = "保存报告失败!";
                return result;
            }
            LogHelper.Ilog("SaveReport?projectResourceId=" + projectResourceId + "&projectId=" + projectId + "&oldResourceId=" + oldResourceId + "&newResourceId=" + newResourceId + "&fileName=" + fileName, "保存pageOffice报告编辑-" + Instance.ToString());
            ProjectResourceRepository.Instance.Transaction(() =>
            {
                //更新资源表记录
                ProjectResource entity = ProjectResourceRepository.Instance.Find(p => p.Id == projectResourceId).FirstOrDefault();
                entity.ResourceId = newResourceId;
                ProjectResourceRepository.Instance.Save(entity);
                //写入历史记录
                ReportHistory history = new ReportHistory();
                history.CreateTime = DateTime.Now;
                history.CreatorId = UserService.Instance.GetCrmUser().Id;
                history.FileName = fileName;
                history.ProjectId = projectId;
                history.ResourceId = oldResourceId;
                ReportHistoryRepository.Instance.Insert(history);
                //写入项目流程(报告修改)
                ProjectStateInfoService.Instance.WriteInProjectStateInfo(projectId, StateInfoEnum.报告修改);
                //委托进度查询(制作报告)
                OnlineBusinessService.Instance.WriteFeedBackKafkaMQ(projectId, ProgressEnum.制作报告);
            });
            return result;
        }

    }
}
