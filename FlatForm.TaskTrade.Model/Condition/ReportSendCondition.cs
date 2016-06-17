namespace Peacock.PEP.Model.Condition
{
    /// <summary>
    /// 项目查询条件
    /// </summary>
    public class ReportSendCondition
    {
        public long ProjectId { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        public string SendExpress { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 接收地址
        /// </summary>
        public string SendAddress { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ReciverMobile { get; set; }        

    }
}
