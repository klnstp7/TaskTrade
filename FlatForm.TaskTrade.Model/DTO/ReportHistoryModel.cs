using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    public class ReportHistoryModel
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

        public ProjectModel Project { get; set; }

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
}
