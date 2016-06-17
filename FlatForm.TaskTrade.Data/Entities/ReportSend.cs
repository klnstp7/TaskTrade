using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class ReportSend
    {

        /// <summary>
        /// auto_increment
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 发送者编号
        /// </summary>
        public long? SenderId { get; set; }

        /// <summary>
        /// 收件人/接收人 
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 发送数量
        /// </summary>
        public int? SendQuantity { get; set; }

        /// <summary>
        /// 发送类型
        /// </summary>
        public string SendType { get; set; }

        /// <summary>
        /// 发送快递
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
        /// 任务编号
        /// </summary>
        public long? ProjectId { get; set; }

        /// <summary>
        /// 收件人/接收人 电话
        /// </summary>
        public string ReciverMobile { get; set; }

        internal class ReportSendConfig : EntityConfig<ReportSend>
        {
            internal ReportSendConfig()
                : base(20)
            {
                base.ToTable("ReportSend");
                base.HasKey(x => x.Id);
            }
        }
    }
}
