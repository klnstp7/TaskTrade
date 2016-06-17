using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class Customer
    {
        /// <summary>
        /// auto_increment
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string Phone { get; set; }


        /// <summary>
        /// 所属机构
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 分支机构
        /// </summary>
        public string Subbranch { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }

    internal class CustomerConfig : EntityConfig<Customer>
    {
        internal CustomerConfig() : base(20)
        {
            base.ToTable("Customer");
            base.HasKey(x => x.Id);
            base.Property(x => x.Bank).HasMaxLength(100);
            base.Property(x => x.Subbranch).HasMaxLength(100);
        }
    }
}
