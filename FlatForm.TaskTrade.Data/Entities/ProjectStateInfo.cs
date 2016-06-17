using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class ProjectStateInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }

    }

    internal class ProjectStateInfoConfig:EntityConfig<ProjectStateInfo>
    {
        internal ProjectStateInfoConfig():base(50)
        {
            base.ToTable("projectstateinfo");
            base.HasKey(p => p.Id);
            base.Property(p => p.Content).HasMaxLength(250);
        }
    }
}
