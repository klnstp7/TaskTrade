using System;

namespace Peacock.PEP.Model.DTO
{
    public class ProjectCancleModel
    {
        /// <summary>
        /// 主键，自增长
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime CancleTime { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        public string Reason { get; set; }
    }
}
