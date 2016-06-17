using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class Company
    {
        public Company()
        {
            IsApprove = false;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 公司管理员账户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 公司管理员密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 公司所在城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 审核拒绝原因
        /// </summary>
        public string Reson { get; set; }

        /// <summary>
        /// 是否审核通过
        /// </summary>
        public bool? IsApprove { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateTime { get; set; }

        internal class CompanyConfig : EntityConfig<Company>
        {
            internal CompanyConfig()
                : base(20)
            {
                base.ToTable("Company");
                base.HasKey(x => x.Id);
            }
        }

    }
}
