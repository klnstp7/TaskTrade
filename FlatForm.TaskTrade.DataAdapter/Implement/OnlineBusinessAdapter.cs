using System;
using System.Collections.Generic;
using System.Collections;
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
    public class OnLineBusinessAdapter : IOnLineBusinessAdapter
    {
        public List<OnLineBusinessListModel> GetOnlineBusinessList(OnLineBusinessCondition condition, int index, int size, out int total)
        {
            return OnlineBusinessService.Instance.GetOnlineBusinessList(condition, index, size, out total).ToListModel<OnLineBusinessListModel, OnLineBusiness>();
        }

        public OnLineBusinessModel GetById(long id)
        {
            return OnlineBusinessService.Instance.GetById(id).ToModel<OnLineBusinessModel>();
        }

        public void Refuse(long id, string reason)
        {
            OnlineBusinessService.Instance.Refuse(id, reason);
        }

        public void Accept(long id, long projectId)
        {
            OnlineBusinessService.Instance.Accept(id, projectId);
        }

        public IDictionary<string, object> GetOnlineReportDownloadUrl(string businessId)
        {
            return OnlineBusinessService.Instance.GetOnlineReportDownloadUrl(businessId);
        }

        public IDictionary<string, object> GetReportDownloadUrl(string reportNo, string delegateName)
        {
            return OnlineBusinessService.Instance.GetReportQueryDownloadUrl(reportNo, delegateName);
        }
    }
}