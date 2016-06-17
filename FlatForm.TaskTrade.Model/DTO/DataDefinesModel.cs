using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    /// <summary>
    /// 获取外业勘察表模型
    /// </summary>
    public class DataDefinesModel
    {
        public string ProvinceName { get; set; }
        public string ProvinceCode { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictCode { get; set; }
        public string Name { get; set; }
        public string TargetType { get; set; }
        public int Total { get; set; }
        public int CompanyID { get; set; }
        public string CrmDepId { get; set; }
        public int Version { get; set; }
        public bool IsActived { get; set; }
        public bool IsDefault { get; set; }
        public string Note { get; set; }
        public int ID { get; set; }
        public string TGuid { get; set; }
        public DateTime CreatedTime { get; set; }
        public int IOrder { get; set; }
    }

    /// <summary>
    /// 获取外业勘察表模型
    /// </summary>
    public class DataDefinesResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public DataDefinesModel[] Data { get; set; }
        public object Others { get; set; }
    }
}
