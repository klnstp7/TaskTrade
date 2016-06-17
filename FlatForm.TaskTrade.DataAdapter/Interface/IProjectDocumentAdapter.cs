using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Model;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IProjectDocumentAdapter
    {

        IList<ProjectDocumentModel> GetDocumentList(long onLineBusinessId);

    }
}
