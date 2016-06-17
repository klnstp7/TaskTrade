using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Peacock.PEP.Model;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model.DTO;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IOnLineBusinessAdapter
    {
        List<OnLineBusinessListModel> GetOnlineBusinessList(OnLineBusinessCondition condition, int index, int size, out int total);

        OnLineBusinessModel GetById(long id);

        void Refuse(long id, string reason);

        void Accept(long id, long projectId);

        IDictionary<string, object> GetOnlineReportDownloadUrl(string businessId);

        IDictionary<string, object> GetReportDownloadUrl(string reportNo, string delegateName);
    }
}
