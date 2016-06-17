using System;

namespace Peacock.PEP.Data.Entities
{
    public class UserCompany
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
    }
    internal class UserCompanyConfig : EntityConfig<UserCompany>
    {
        internal UserCompanyConfig()
            : base(20)
        {
            base.ToTable("UserCompany");
            base.HasKey(x => x.Id);
        }
    }
}
