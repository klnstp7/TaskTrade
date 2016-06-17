using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Service;
using Peacock.PEP.Data.Entities;
using System.Collections.Generic;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class RegionAdapter : IRegionAdapter
    {
        public IList<RegionModel> GetAll()
        {
            return RegionService.Instance.GetAll().ToListModel<RegionModel, Region>();
        }
    }
}