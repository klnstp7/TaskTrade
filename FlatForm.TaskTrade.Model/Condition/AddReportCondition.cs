using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.Condition
{
    public class AddReportCondition
    {
        /// <summary>
        /// 报告号
        /// </summary>
        public string ReportNo { get; set; }

        /// <summary>
        /// 小区地址
        /// </summary>
        public string ResidentialAddress { get; set; }

        /// <summary>
        /// 物业类型
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// 估价目的
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 报告类型
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// 小区名
        /// </summary>
        public string ResidentialAreaName { get; set; }

    }
}
