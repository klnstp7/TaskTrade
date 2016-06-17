using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.Condition
{
    /// <summary>
    /// ��Ŀ��ѯ����
    /// </summary>
    public class ProjectCondition
    {
        /// <summary>
        /// ��ˮ��
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// �����
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
        /// �Ƿ��ͱ���
        /// </summary>
        public bool? IsSent { get; set; }

        /// <summary>
        /// �Ƿ����ͨ��
        /// </summary>
        public bool? IsApprove { get; set; }

        /// <summary>
        /// �Ƿ��ȡ���񱨸�����
        /// </summary>
        public bool GetReportCount { get; set; }

        /// <summary>
        /// ��ҵ����
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// ��Ŀ������
        /// </summary>
        public string ProjectCreatorName { get; set; }

        /// <summary>
        /// ��Ŀ����ʱ��->��ʼ
        /// </summary>
        public DateTime? ProjectCreatorTimeStart { get; set; }

        /// <summary>
        /// ��Ŀ����ʱ��->����
        /// </summary>
        public DateTime? ProjectCreatorTimeEnd { get; set; }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// ����ί�з�
        /// </summary>
        public string Principal { get; set; }

        /// <summary>
        /// ����Ŀ��
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// �Ƿ��ύ���
        /// </summary>
        public bool? IsSubmitted { get; set; }

        /// <summary>
        /// ��Ŀ��Դ
        /// </summary>
        public string ProjectSource { get; set; }
    }
}
