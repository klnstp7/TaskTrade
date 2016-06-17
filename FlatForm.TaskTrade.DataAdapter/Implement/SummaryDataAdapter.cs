using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Service;
using PermissionsMiddle.Dto;
using System.Collections.Generic;
using Peacock.PEP.Data.Entities;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class SummaryDataAdapter : ISummaryDataAdapter
    {
        public SummaryDataModel GetById(long id)
        {
            return SummaryDataService.Instance.GetById(id).ToModel<SummaryDataModel>();
        }

        public SummaryDataModel GetByProjectId(long projectId)
        {
            return SummaryDataService.Instance.GetByProjectId(projectId).ToModel<SummaryDataModel>();
        }

        public void Save(SummaryDataModel model)
        {
            SummaryDataService.Instance.Save(model.ToModel<SummaryData>());
        }
    }
}