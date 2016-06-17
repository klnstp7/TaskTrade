using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model;
using Peacock.PEP.Service;
using Peacock.PEP.Model.Condition;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class InquiryAdapter : IInquiryAdapter
    {
        public IList<InquiryModel> GetInquiryList(InquiryCondition condition, int index, int size, out int total)
        {
            return
                InquiryService.Instance.GetInquiryList(condition, index, size, out total)
                    .ToListModel<InquiryModel, Inquiry>();
        }

        public List<string> GetRegion(string cityName)
        {
            return InquiryService.Instance.GetRegion(cityName);
        }

        public long SaveInquiry(InquiryModel inquiry, CustomerModel customer)
        {
            return InquiryService.Instance.SaveInquiry(inquiry.ToModel<Inquiry>(), customer.ToModel<Customer>());
        }

        public InquiryModel GetInquiry(long inquiryId)
        {
            return InquiryService.Instance.GetInquiry(inquiryId).ToModel<InquiryModel>();
        }

        public bool ToProject(long inquiryId, long projectId)
        {
            return InquiryService.Instance.ToProject(inquiryId, projectId);
        }

        public string GetCityByRegion(string regionName)
        {
            return InquiryService.Instance.GetCityByRegion(regionName);
        }

        public void SaveInquiryFromOnline(long onlineBusinessId, long inquiryId)
        {
            InquiryService.Instance.SaveInquiryFromOnline(onlineBusinessId, inquiryId);
        }
    }
}
