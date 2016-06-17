using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Service;
using Peacock.PEP.Data.Entities;
using System.Collections.Generic;
using Peacock.PEP.Model.DTO;
using System.Linq;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class ProjectAdapter : IProjectAdapter
    {
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ProjectListModel> GetProjectList(ProjectCondition condition, int pageIndex, int pageSize, out int total)
        {
            IList<Project> projectList = ProjectService.Instance.GetProjectList(condition, pageIndex, pageSize, out total);
            var result = projectList.ToListModel<ProjectListModel, Project>();
            if (condition.GetReportCount)
            {
                foreach (var item in result)
                {
                    item.SendReportCount = ReportSendService.Instance.GetProjectReportCount(item.Id);
                }
            }
            return result;
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="model">项目信息</param>
        /// <param name="list">外业勘察联系人信息</param>
        /// <returns></returns>
        public long Save(ProjectModel model, List<ExplorationContactsModel> list)
        {
            if (model.ProjectId > 0)
            {
                return ProjectService.Instance.Update(model, list.ToModel<List<ExplorationContacts>>());
            }
            else
            {
                return ProjectService.Instance.Add(model.ToModel<Project>(), model, list.ToModel<List<ExplorationContacts>>());
            }

        }

        /// <summary>
        /// 获取当前登陆用户所属部门的外采用户信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<long, string> GetOuterTaskUsers()
        {
            return OutTaskService.Instance.GetOuterTaskUsers();
        }

        /// <summary>
        /// 获取当前登陆用户的勘察表
        /// </summary>
        /// <returns></returns>
        public Dictionary<long, string> GetDataDefines()
        {
            return OutTaskService.Instance.GetDataDefines();
        }

        /// <summary>
        /// 根据Id获得项目
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ProjectModel GetProjectById(long projectId)
        {
            return ProjectService.Instance.GetProjectById(projectId).ToModel<ProjectModel>();
        }

        /// <summary>
        /// 添加项目返回Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long Add(ProjectModel model)
        {
            return ProjectService.Instance.Add(model.ToModel<Project>());
        }

        /// <summary>
        ///  项目审核
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public bool AuditProject(long projectId)
        {
            return ProjectService.Instance.AuditProject(projectId);
        }

        /// <summary>
        /// 报告文件打包
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="DownLoadInfo"></param>
        /// <returns></returns>
        public byte[] ReportResourcesZip(long projectId, string[] DownLoadInfo)
        {
            return ProjectService.Instance.ReportResourcesZip(projectId, DownLoadInfo);
        }

        /// <summary>
        /// 项目提交审核
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public ResultInfo ComitAudit(long projectId)
        {
            return ProjectService.Instance.ComitAudit(projectId);
        }

        /// <summary>
        /// 获取项目取消信息
        /// </summary>
        /// <returns></returns>
        public ProjectCancleModel GetProjectCancle(long projectId)
        {
            return ProjectService.Instance.GetProjectCancle(projectId).ToModel<ProjectCancleModel>();
        }
    }
}
