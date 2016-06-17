using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class OnLineFeedBack
    {

        /// <summary>
        /// 主键，自增长
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 线上业务ID
        /// </summary>
        public long OnLineBussinessId { get; set; }

        /// <summary>
        /// 线上业务导航属性
        /// </summary>
        public virtual OnLineBusiness OnlineBusiness { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        internal class OnLineFeedBackConfig : EntityConfig<OnLineFeedBack>
        {
            internal OnLineFeedBackConfig()
            {
                base.ToTable("onlinefeedback");
                base.HasKey(x => x.Id);
                base.HasRequired(x => x.OnlineBusiness).WithMany(x => x.OnLineFeedBacks).HasForeignKey(x => x.OnLineBussinessId);
            }
        }

    }
}
