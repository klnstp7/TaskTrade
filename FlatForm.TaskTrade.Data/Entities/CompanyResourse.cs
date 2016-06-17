using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class CompanyResourse
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
        /// FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// CompanyId
        /// </summary>
        public long CompanyId { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal class CompanyResourseConfig : EntityConfig<CompanyResourse>
        {
            internal CompanyResourseConfig()
                : base(20)
            {
                base.ToTable("CompanyResourse");
                base.Property(x => x.FileName).HasMaxLength(200);
                base.HasKey(x => x.Id);
            }
        }
    }
}
