using System;

namespace Peacock.PEP.Data.Entities
{
    public class OnlineBusinessStop
    {
        /// <summary>
        /// 主键，自增长
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 终止时间
        /// </summary>
        public DateTime StopTime { get; set; }

        /// <summary>
        /// OnlineBusinessId
        /// </summary>
        public long OnlineBusinessId { get; set; }

        /// <summary>
        /// 线上导航属性
        /// </summary>
        public virtual OnLineBusiness OnLineBusiness { get; set; }

        /// <summary>
        /// 终止原因
        /// </summary>
        public string Reason { get; set; }
    }
    internal class OnlineBusinessStopConfig : EntityConfig<OnlineBusinessStop>
    {
        internal OnlineBusinessStopConfig()
            : base(20)
        {
            ToTable("OnlineBusiness_Stop");
            HasKey(x => x.Id);
            HasRequired(x => x.OnLineBusiness).WithMany().HasForeignKey(x => x.OnlineBusinessId);
            Property(x => x.Reason).HasMaxLength(255);
        }
    }
}
