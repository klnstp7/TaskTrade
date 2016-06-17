namespace Peacock.PEP.Model.Condition
{
    /// <summary>
    /// 项目查询条件
    /// </summary>
    public class CompanyCondition
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司所在城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int? ApproveStatus { get; set; }
    }
}
