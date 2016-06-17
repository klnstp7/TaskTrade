using Peacock.PEP.Model;
using Peacock.PEP.Model.DTO;
using System.Collections.Generic;
using Peacock.PEP.Model.Condition;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IProjectAdapter
    {
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<ProjectListModel> GetProjectList(ProjectCondition condition, int pageIndex, int pageSize, out int total);

        /// 添加项目
        /// </summary>
        /// <param name="model">项目信息</param>
        /// <param name="list">外业勘察联系人信息</param>
        /// <returns></returns>
        long Save(ProjectModel model, List<ExplorationContactsModel> list);

        /// <summary>
        /// 获取当前登陆用户所属部门的外采用户信息
        /// </summary>
        /// <returns></returns>
        Dictionary<long, string> GetOuterTaskUsers();

        /// <summary>
        /// 根据Id获得项目
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        ProjectModel GetProjectById(long projectId);

        /// <summary>
        /// 添加项目返回Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        long Add(ProjectModel model);

        /// <summary>
        /// 项目审核
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        bool AuditProject(long projectId);

        /// <summary>
        /// 获取当前登陆用户的勘察表
        /// 报告文件打包
        /// </summary>
        /// <returns></returns>
        Dictionary<long, string> GetDataDefines();

        /// <param name="projectId"></param>
        /// <param name="DownLoadInfo"></param>
        /// <returns></returns>
        byte[] ReportResourcesZip(long projectId, string[] DownLoadInfo);

        /// <summary>
        /// 项目提交审核
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        ResultInfo ComitAudit(long projectId);


        /// <summary>
        /// 获取项目取消信息
        /// </summary>
        /// <returns></returns>
        ProjectCancleModel GetProjectCancle(long projectId);
    }
}
