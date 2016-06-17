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
        /// С������
        /// </summary>
        public string ResidentialAreaName { set; get; }

        /// <summary>
        /// ��Ŀ��ַ
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// ѯ�ۻ���
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// ��ѯ����(�ͻ�����)
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool? IsToProject { get; set; }
    }
}
