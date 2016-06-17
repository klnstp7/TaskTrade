using Peacock.PEP.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.DataAdapter.Interface
{
    /// <summary>
    /// 看房联系人
    /// </summary>
    public interface IExplorationContactsAdapter
    {
        /// <summary>
        /// 根据项目id获取看房联系人列表
        /// </summary>
        /// <param name="ProjectId">项目id</param>
        /// <returns></returns>
        List<ExplorationContactsModel> GetListByProjectId(long ProjectId);

        /// <summary>
        /// 根据线上业务id获取看房联系人列表
        /// </summary>
        /// <param name="ProjectId">项目id</param>
        /// <returns></returns>
        List<ExplorationContactsModel> GetListByOnlineBusinessId(long OnlineBusinessId);
    }
}
