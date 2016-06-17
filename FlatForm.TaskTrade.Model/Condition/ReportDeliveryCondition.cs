namespace Peacock.PEP.Model.Condition
{
    /// <summary>
    /// 发送报告查询条件
    /// </summary>
    public class ReportDeliveryCondition
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 报告编号
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
        /// 发送报告数量
        /// </summary>
        public string SendReportCount { get; set; }
    }
}
