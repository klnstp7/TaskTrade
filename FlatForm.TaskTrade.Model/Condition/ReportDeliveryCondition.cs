namespace Peacock.PEP.Model.Condition
{
    /// <summary>
    /// ���ͱ����ѯ����
    /// </summary>
    public class ReportDeliveryCondition
    {
        /// <summary>
        /// ��ˮ��
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string ReportNo { get; set; }

        /// <summary>
        /// ��Ŀ��ַ
        /// </summary>
        public string ProjectAddress { get; set; }

        /// <summary>
        /// С������
        /// </summary>
        public string ResidentialAreaName { get; set; }

        /// <summary>
        /// ���ͱ�������
        /// </summary>
        public string SendReportCount { get; set; }
    }
}
