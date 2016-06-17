using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.DataAdapter.Implement
{
    /// <summary>
    /// 看房联系人
    /// </summary>
    public class ExplorationContactsAdapter : IExplorationContactsAdapter
    {
        /// <summary>
        /// 根据项目id获取看房联系人列表
        /// </summary>
        /// <param name="ProjectId">项目id</param>
        /// <returns></returns>
        public List<ExplorationContactsModel> GetListByProjectId(long ProjectId)
        {
            return ExplorationContactsService.Instance.GetListByProjectId(ProjectId).ToModel<List<ExplorationContactsModel>>();
        }

        /// <summary>
        /// 根据线上业务id获取看房联系人列表
        /// </summary>
        /// <param name="ProjectId">项目id</param>
        /// <returns></returns>
        public List<ExplorationContactsModel> GetListByOnlineBusinessId(long OnlineBusinessId)
        {
            return ExplorationContactsService.Instance.GetListByBusinessId(OnlineBusinessId).ToModel<List<ExplorationContactsModel>>();
        }
    }
}
