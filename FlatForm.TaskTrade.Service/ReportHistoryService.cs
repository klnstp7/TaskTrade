using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Repository.Repositories;

namespace Peacock.PEP.Service
{
    public class ReportHistoryService : SingModel<ReportHistoryService>
    {
        private ReportHistoryService()
        {
        }

        /// <summary>
        /// 根据项目id获取报告历史列表
        /// 作者：BOBO
        /// 时间：2016-4-13
        /// </summary>
        /// <returns></returns>
        public IList<ReportHistory> GetReportHistoryByProjectId(long projectId)
        {
            if (projectId <= 0)
                return null;
            var query = from rh in ReportHistoryRepository.Instance.Source
                        where rh.ProjectId == projectId
                        select rh;
            var result = query.ToList();
            return result;
        }

    }
}
