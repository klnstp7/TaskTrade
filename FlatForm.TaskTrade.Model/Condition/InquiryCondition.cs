using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.Condition
{
    public class InquiryCondition
    {
        /// <summary>
        /// 小区名称
        /// </summary>
        public string ResidentialAreaName { set; get; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// 询价机构
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 报询价人(客户名称)
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 是否立项
        /// </summary>
        public bool? IsToProject { get; set; }
    }
}
