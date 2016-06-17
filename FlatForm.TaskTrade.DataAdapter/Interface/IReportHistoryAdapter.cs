using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Model.DTO;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IReportHistoryAdapter
    {
        /// <summary>
        /// 根据项目id获取报告历史列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IList<ReportHistoryModel> GetReportHistoryByProjectId(long projectId);
    }
}
