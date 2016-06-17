using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.Condition
{
    /// <summary>
    /// 项目查询条件
    /// </summary>
    public class ProjectCondition
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 报告号
        /// </summary>
        public string ReportNo { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public string ProjectAddress { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string ResidentialAreaName { get; set; }

        /// <summary>
        /// 是否发送报告
        /// </summary>
        public bool? IsSent { get; set; }

        /// <summary>
        /// 是否审核通过
        /// </summary>
        public bool? IsApprove { get; set; }

        /// <summary>
        /// 是否获取任务报告数量
        /// </summary>
        public bool GetReportCount { get; set; }

        /// <summary>
        /// 物业类型
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// 项目立项人
        /// </summary>
        public string ProjectCreatorName { get; set; }

        /// <summary>
        /// 项目立项时间->起始
        /// </summary>
        public DateTime? ProjectCreatorTimeStart { get; set; }

        /// <summary>
        /// 项目立项时间->结束
        /// </summary>
        public DateTime? ProjectCreatorTimeEnd { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// 估价委托方
        /// </summary>
        public string Principal { get; set; }

        /// <summary>
        /// 估价目的
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 是否提交审核
        /// </summary>
        public bool? IsSubmitted { get; set; }

        /// <summary>
        /// 项目来源
        /// </summary>
        public string ProjectSource { get; set; }
    }
}
