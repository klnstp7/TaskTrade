using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Service;
using Peacock.PEP.Data.Entities;
using PermissionsMiddle.Dto;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class OnLineFeedBackAdapter : IOnLineFeedBackAdapter
    {
        public long Save(OnLineFeedBackModel entity)
        {
            return OnlineFeedBackService.Instance.Save(entity.ToModel<OnLineFeedBack>());
        }

        public short SendMessageQueue(OnLineFeedBackModel entity,string transactionNo)
        {
            return OnlineFeedBackService.Instance.SendMessageQueue(entity.ToModel<OnLineFeedBack>(), transactionNo);
        }
    }
}