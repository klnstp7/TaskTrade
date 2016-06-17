using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Data.Entities;

namespace Peacock.PEP.Repository.Repositories
{
    public sealed class ProjectResourceRepository : Repository<ProjectResource, ProjectResourceRepository>
    {
        private ProjectResourceRepository()
        {
        }
    }
}