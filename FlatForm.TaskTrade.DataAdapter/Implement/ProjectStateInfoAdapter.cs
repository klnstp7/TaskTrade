using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Service;
using Peacock.PEP.Data.Entities;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class ProjectStateInfoAdapter:IProjectStateInfoAdapter
    {
        /// <summary>
        /// 根据项目Id获取流程状态记录列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<ProjectStateInfoModel> GetProjectStateInfoList(long projectId)
        {
            return ProjectStateInfoService.Instance.GetProjectStateInfoList(projectId).ToListModel<ProjectStateInfoModel,ProjectStateInfo>();
        }
    }
}
