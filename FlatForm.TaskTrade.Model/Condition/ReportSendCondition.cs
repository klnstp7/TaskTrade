namespace Peacock.PEP.Model.Condition
{
    /// <summary>
    /// ��Ŀ��ѯ����
    /// </summary>
    public class ReportSendCondition
    {
        public long ProjectId { get; set; }

        /// <summary>
        /// ��ݹ�˾
        /// </summary>
        public string SendExpress { get; set; }

        /// <summary>
        /// ��ݵ���
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// ���յ�ַ
        /// </summary>
        public string SendAddress { get; set; }

        /// <summary>
        /// �ջ��˵绰
        /// </summary>
        public string ReciverMobile { get; set; }        

    }
}
