using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    public class RegionModel
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
}
