using Peacock.PEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Model.DTO;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IRegionAdapter
    {
        IList<RegionModel> GetAll();
    }
}
