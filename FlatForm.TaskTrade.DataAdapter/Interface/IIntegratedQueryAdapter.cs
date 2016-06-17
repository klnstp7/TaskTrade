using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model.DTO;
using System.Collections.Generic;
using System.Data;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IIntegratedQueryAdapter
    {
        byte[] ReportResourcesZip(long projectId, string[] DownLoadInfo);

        SummaryDataModel GetByProjectId(long projectId);

        List<ProjectModel> Search(IntegratedQueryCondition condition, int? index, int? size, out int total);

        DataTable Search(List<string> columns, IntegratedQueryCondition condition, int? index, int? size, out int total);

        DataTable Export(List<string> columns, IntegratedQueryCondition condition);
    }
}
