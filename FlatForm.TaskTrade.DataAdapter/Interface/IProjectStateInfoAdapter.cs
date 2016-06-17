using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Model.DTO;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IProjectStateInfoAdapter
    {
        /// <summary>
        /// 根据项目Id获取流程状态记录列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IList<ProjectStateInfoModel> GetProjectStateInfoList(long projectId);

    }
}
