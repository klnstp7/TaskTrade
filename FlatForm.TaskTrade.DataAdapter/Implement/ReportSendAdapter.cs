using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Service;
using Peacock.PEP.Data.Entities;
using System.Collections.Generic;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class ReportSendAdapter : IReportSendAdapter
    {
        public List<ReportSendDto> GetReportSendList(ReportSendCondition condition, int pageIndex, int pageSize, out int total)
        {
            return ReportSendService.Instance.GetReportSendList(condition, pageIndex, pageSize, out total).ToListModel<ReportSendDto, ReportSend>();
        }

        public int GetProjectReportCount(long projectId)
        {
            return ReportSendService.Instance.GetProjectReportCount(projectId);
        }

        public ResultInfo SaveReportSendData(ReportSendDto dto)
        {
            ReportSend saveItem = dto.ToModel<ReportSend>();
            return ReportSendService.Instance.SaveReportSendData(saveItem);
        }
    }
}
