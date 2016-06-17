using Peacock.PEP.Data.Entities;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Service;
using System.Collections.Generic;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface ISummaryDataAdapter
    {
        SummaryDataModel GetById(long id);

        SummaryDataModel GetByProjectId(long projectId);

        void Save(SummaryDataModel model);
    }
}
