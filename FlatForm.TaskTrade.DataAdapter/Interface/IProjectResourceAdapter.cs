using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Model;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IProjectResourceAdapter
    {
        /// <summary>
        /// 根据项目Id和报告文档类型判断报告资源是否存在
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        int IsExist(long projectId, int resourceType);

        /// <summary>
        /// 添加报告资源并返回Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        long AddProjectResource(ProjectResourceModel model);


        /// <summary>
        /// 更新报告资源
        /// </summary>
        /// <param name="model"></param>
        void UpdateProjectResource(ProjectResourceModel model);


        /// <summary>
        /// 根据Id获取报告资源
        /// </summary>
        /// <param name="projectResourceId"></param>
        /// <returns></returns>
        ProjectResourceModel GetProjectResourceById(long projectResourceId);


        /// <summary>
        /// 上传报告并写入历史记录
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="resourceType">报告类型</param>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        string UploadProjectResource(long projectId, int resourceType, string fileName, byte[] fileByte);


        /// <summary>
        /// 根据项目id和报告类型获取报告资源
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        ProjectResourceModel GetProjectResource(long projectId, int resourceType);


        /// <summary>
        /// 保存pageOffice报告编辑
        /// </summary>
        /// <param name="projectResourceId"></param>
        /// <param name="projectId"></param>
        /// <param name="oldResourceId"></param>
        /// <param name="newResourceId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string SaveReport(long projectResourceId, long projectId, long oldResourceId, long newResourceId, string fileName);


    }
}
