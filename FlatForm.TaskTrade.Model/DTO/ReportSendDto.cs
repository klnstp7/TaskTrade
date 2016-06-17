using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model
{
    public class ReportSendDto
    {
        public ReportSendDto()
        {
            CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// auto_increment
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 发送人编号
        /// </summary>
        public long? SenderId { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 发送份数
        /// </summary>
        public int? SendQuantity { get; set; }

        /// <summary>
        /// 快递类型
        /// </summary>
        public string SendType { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        public string SendExpress { get; set; }

        /// <summary>
        /// 发送地址
        /// </summary>
        public string SendAddress { get; set; }

        /// <summary>
        /// 快递号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 所属项目编号
        /// </summary>
        public long? ProjectId { get; set; }

        /// <summary>
        /// 收件人手机号
        /// </summary>
        public string ReciverMobile { get; set; }
    }
}
