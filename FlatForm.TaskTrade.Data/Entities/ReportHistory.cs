using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class ReportHistory
    {

        /// <summary>
        /// auto_increment
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ResourceId
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public long ProjectId { get; set; }

        public virtual Project Project { get; set; }

        /// <summary>
        /// FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// CreatorId
        /// </summary>
        public long CreatorId { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

    }

    internal class ReportHistoryConfig : EntityConfig<ReportHistory>
    {
        internal ReportHistoryConfig()
        {
            this.ToTable("reporthistory");
            this.HasKey(p => p.Id);
            this.HasRequired(p => p.Project).WithMany().HasForeignKey(p => p.ProjectId);
        }
    }

}
