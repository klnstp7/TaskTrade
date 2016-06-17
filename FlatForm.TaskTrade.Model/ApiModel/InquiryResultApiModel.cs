
namespace Peacock.PEP.Model.ApiModel
{
    public class InquiryResultApiModel
    {
        /// <summary>
        /// 小区名称
        /// </summary>
        public string residentialAreaName { get; set; }

        /// <summary>
        /// 小区地址
        /// </summary>
        public string residentialAddress { get; set; }

        /// <summary>
        /// 建筑面积
        /// </summary>
        public string ruildingArea { get; set; }

        /// <summary>
        /// 建成年代
        /// </summary>
        public string ruildedYear { get; set; }

        /// <summary>
        /// 市场总价
        /// </summary>
        public string inquiryResult { get; set; }

        /// <summary>
        /// 市场单价
        /// </summary>
        public string inquiryPrice { get; set; }

        /// <summary>
        /// 抵押单价
        /// </summary>
        public string mortgagePrice { get; set; }

        /// <summary>
        /// 抵押总价
        /// </summary>
        public string mortgageResult { get; set; }

        /// <summary>
        /// 特殊因素
        /// </summary>
        public string specialInfo { get; set; }
    }
}
