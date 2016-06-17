using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Model.Condition;
using Peacock.PEP.Model;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IInquiryAdapter
    {
        IList<InquiryModel> GetInquiryList(InquiryCondition condition, int index, int size, out int total);

        List<string> GetRegion(string cityName);

        long SaveInquiry(InquiryModel inquiry, CustomerModel customer);

        InquiryModel GetInquiry(long inquiryId);

        bool ToProject(long inquiryId, long projectId);

        string GetCityByRegion(string regionName);

        void SaveInquiryFromOnline(long onlineBusinessId, long inquiryId);
    }
}
