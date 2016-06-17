
namespace Peacock.PEP.Model.ApiModel
{
    public class ApiModelCrmDepartment
    {
        public long CrmDepartmentId { get; set; }

        public string CrmDepartmentName { get; set; }

        public long? BaseCompanyId { get; set; }
        public string BaseCompanyName { get; set; }
    }
}
