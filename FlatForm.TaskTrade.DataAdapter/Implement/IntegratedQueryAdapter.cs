using Peacock.PEP.Data.Entities;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Service;
using System.Collections.Generic;
using System.Data;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class IntegratedQueryAdapter : IIntegratedQueryAdapter
    {
        public byte[] ReportResourcesZip(long projectId, string[] DownLoadInfo)
        {
            return IntegratedQueryService.Instance.ReportResourcesZip(projectId, DownLoadInfo);
        }

        public SummaryDataModel GetByProjectId(long projectId)
        {
            return IntegratedQueryService.Instance.GetByProjectId(projectId).ToModel<SummaryDataModel>();
        }

        public List<ProjectModel> Search(IntegratedQueryCondition condition, int? index, int? size, out int total)
        {
            return IntegratedQueryService.Instance.Search(condition, index, size, out total)
                    .ToListModel<ProjectModel, Project>();
        }


        public DataTable Search(List<string> columns, IntegratedQueryCondition condition, int? index, int? size, out int total)
        {
            return IntegratedQueryService.Instance.Search(columns, condition, index, size, out total);

        }


        public DataTable Export(List<string> columns, IntegratedQueryCondition condition)
        {
            return IntegratedQueryService.Instance.Export(columns, condition);
        }
    }
}