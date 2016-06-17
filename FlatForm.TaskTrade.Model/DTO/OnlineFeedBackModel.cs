using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    public class OnLineFeedBackModel
    {
        /// <summary>
        /// ����
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// ����ҵ��ID
        /// </summary>
        public long OnLineBussinessId { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
