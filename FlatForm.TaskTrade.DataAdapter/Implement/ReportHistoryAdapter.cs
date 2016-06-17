using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Service;
using Peacock.PEP.Data.Entities;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class ReportHistoryAdapter : IReportHistoryAdapter
    {
        /// <summary>
        /// 根据项目id获取报告历史列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IList<ReportHistoryModel> GetReportHistoryByProjectId(long projectId)
        {
            return ReportHistoryService.Instance.GetReportHistoryByProjectId(projectId).ToListModel<ReportHistoryModel,ReportHistory>();
        }
    }
}
