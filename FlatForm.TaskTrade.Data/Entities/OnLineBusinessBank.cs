using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    /// <summary>
    /// 线上业务银行列表
    /// </summary>
    public class OnLineBusinessBank
    {
        public long Id { get; set; }

        public OnLineBusiness OnLineBusiness { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public long? ProjectId { get; set; }

        public long? OnBusinessId { get; set; }

        public string BankName { get; set; }

        public string BankbranchName { get; set; }

    }
    internal class OnLineBusinessBankConfig : EntityConfig<OnLineBusinessBank>
    {
        internal OnLineBusinessBankConfig()
            : base(50)
        {
            base.HasKey(x=>x.Id);
            base.ToTable("onlinebusiness_bank");
            base.HasRequired(x => x.OnLineBusiness).WithMany(x=>x.OnLineBusinessBanks).HasForeignKey(x=>x.OnBusinessId);
        }
    }
}
