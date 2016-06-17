using System.Collections.Generic;
using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IReportSendAdapter
    {
        List<ReportSendDto> GetReportSendList(ReportSendCondition condition, int pageIndex, int pageSize, out int total);

        int GetProjectReportCount(long projectId);

        ResultInfo SaveReportSendData(ReportSendDto dto);
    }
}