using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Service
{
    public interface IWriteLog
    {
        string GetUserAccount();

        void Ilog(string concent);
    }
}
