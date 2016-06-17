using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    /// <summary>
    /// 看房联系人
    /// </summary>
    public class ExplorationContacts
    {

        /// <summary>
        /// auto_increment
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Contacts
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public long? ProjectId { get; set; }

        /// <summary>
        /// 线上业务ID
        /// </summary>
        public long? OnlineBusinessId { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        //所属项目
        public virtual Project Project { get; set; }

        public virtual OnLineBusiness OnLineBusiness { get; set; }
    }

    internal class ExplorationContactsConfig : EntityConfig<ExplorationContacts>
    {
        internal ExplorationContactsConfig()
            : base(200)
        {
            base.HasKey(x => x.Id);
            base.ToTable("ExplorationContacts");
            HasRequired(x => x.Project).WithMany(x => x.ExplorationContacts).HasForeignKey(x => x.ProjectId);
            HasRequired(x => x.OnLineBusiness).WithMany(x=>x.ExplorationContacts).HasForeignKey(x=>x.OnlineBusinessId);
        }
    }
}
