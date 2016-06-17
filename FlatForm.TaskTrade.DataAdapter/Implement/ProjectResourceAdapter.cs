using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Service;
using Peacock.PEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Model.DTO;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class ProjectResourceAdapter : IProjectResourceAdapter
    {
        /// <summary>
        /// 根据项目Id和报告文档类型判断报告资源是否存在
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public int IsExist(long projectId, int resourceType)
        {
            return ProjectResourceService.Instance.IsExist(projectId, resourceType);
        }

        /// <summary>
        /// 添加报告资源并返回Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long AddProjectResource(ProjectResourceModel model)
        {
            return ProjectResourceService.Instance.AddProjectResource(model.ToModel<ProjectResource>());
        }


        /// <summary>
        /// 更新报告资源
        /// </summary>
        /// <param name="model"></param>
        public void UpdateProjectResource(ProjectResourceModel model)
        {
            ProjectResourceService.Instance.UpdateProjectResource(model.ToModel<ProjectResource>());
        }

        /// <summary>
        /// 根据Id获取报告资源
        /// </summary>
        /// <param name="projectResourceId"></param>
        /// <returns></returns>
        public ProjectResourceModel GetProjectResourceById(long projectResourceId)
        {
            return ProjectResourceService.Instance.GetProjectResourceById(projectResourceId).ToModel<ProjectResourceModel>();
        }


        /// <summary>
        /// 上传报告并写入历史记录
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="resourceType">报告类型</param>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        public string UploadProjectResource(long projectId, int resourceType, string fileName, byte[] fileByte)
        {
           return ProjectResourceService.Instance.UploadProjectResource(projectId, resourceType, fileName, fileByte);
        }

        /// <summary>
        /// 根据项目id和报告类型获取报告资源
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public ProjectResourceModel GetProjectResource(long projectId, int resourceType)
        {
            return ProjectResourceService.Instance.GetProjectResource(projectId, resourceType).ToModel<ProjectResourceModel>();
        }

        /// <summary>
        /// 保存pageOffice报告编辑
        /// </summary>
        /// <param name="projectResourceId"></param>
        /// <param name="projectId"></param>
        /// <param name="oldResourceId"></param>
        /// <param name="newResourceId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string SaveReport(long projectResourceId, long projectId, long oldResourceId, long newResourceId, string fileName)
        {
            return ProjectResourceService.Instance.SaveReport(projectResourceId, projectId, oldResourceId, newResourceId, fileName);
        }
    }
}
