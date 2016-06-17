using Peacock.PEP.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Repository.Repositories
{
    public sealed class CustomerRepository : Repository<Customer, CustomerRepository>
    {
        private CustomerRepository()
        {

        }
    }
}
