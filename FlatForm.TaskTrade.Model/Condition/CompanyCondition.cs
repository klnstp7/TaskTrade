namespace Peacock.PEP.Model.Condition
{
    /// <summary>
    /// ��Ŀ��ѯ����
    /// </summary>
    public class CompanyCondition
    {
        /// <summary>
        /// ��˾����
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// ��˾���ڳ���
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// ���״̬
        /// </summary>
        public int? ApproveStatus { get; set; }
    }
}
