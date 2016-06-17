using Peacock.Common.Helper;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Repository.Repositories;
using Peacock.PEP.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Service
{
    /// <summary>
    /// 看房联系人服务
    /// </summary>
    public class ExplorationContactsService : SingModel<ExplorationContactsService>
    {
        private ExplorationContactsService()
        {

        }

        /// <summary>
        /// 根据项目id获取看房联系人列表
        /// </summary>
        /// <param name="ProjectId">项目id</param>
        /// <returns></returns>
        public List<ExplorationContacts> GetListByProjectId(long ProjectId)
        {
            LogHelper.Ilog("GetListByProjectId?ProjectId=" + ProjectId, "根据项目id获取看房联系人列表-" + Instance.ToString());
            List<ExplorationContacts> result = new List<ExplorationContacts>();
            var query = ExplorationContactsRepository.Instance.Source;
            query = query.Where(x => x.ProjectId == ProjectId);
            result = query.ToList();
            return result;
        }

        /// <summary>
        /// 根据在线业务ID获取看房联系人列表字符串
        /// </summary>
        /// <param name="ProjectId">在线业务ID</param>
        /// <returns></returns>
        public string GetListStrByBusinessId(long onlineBusinessId)
        {
            LogHelper.Ilog("GetListByBusinessId?onlineBusinessId=" + onlineBusinessId, "根据在线业务ID获取看房联系人列表字符串-" + Instance.ToString());            
            var query = ExplorationContactsRepository.Instance.Source;
            query = query.Where(x => x.OnlineBusinessId == onlineBusinessId);
            var result = query.ToList().Select(x => x.Contacts + "(" + x.Phone + ")").ToList();
            return string.Join(",", result);
        }

        /// <summary>
        /// 根据在线业务ID获取看房联系人列表
        /// </summary>
        /// <param name="onlineBusinessId"></param>
        /// <returns></returns>
        public List<ExplorationContacts> GetListByBusinessId(long onlineBusinessId)
        {
            LogHelper.Ilog("GetListByBusinessId?onlineBusinessId=" + onlineBusinessId, "根据在线业务ID获取看房联系人列表-" + Instance.ToString());
            var query = ExplorationContactsRepository.Instance.Source;
            query = query.Where(x => x.OnlineBusinessId == onlineBusinessId);
            var result = query.ToList();
            return result;
        }
    }
}
