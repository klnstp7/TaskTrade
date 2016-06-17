using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model.DTO;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IOnLineFeedBackAdapter
    {
        long Save(OnLineFeedBackModel entity);

        short SendMessageQueue(OnLineFeedBackModel entity, string transactionNo);
    }
}
