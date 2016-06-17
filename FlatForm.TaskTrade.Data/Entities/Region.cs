using System;

namespace Peacock.PEP.Data.Entities
{
    public class Region
    {

        /// <summary>
        /// auto_increment
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// RegionType
        /// </summary>
        public string RegionType { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        public long? ParentId { get; set; }
    }

    internal class RegionConfig : EntityConfig<Region>
    {
        internal RegionConfig()
        {
            base.ToTable("Region");
            base.HasKey(x => x.Id);
        }
    }
}
