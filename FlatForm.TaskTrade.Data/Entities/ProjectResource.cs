using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{ /// <summary>
    /// 报告资源
    /// </summary>
    public class ProjectResource
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 资源库文档Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 报告文档类型 1:报告，2:计算表;默认报告
        /// </summary>
        public int ResourceType { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public Project Project { get; set; }

    }

    internal class ProjectResourceConfig : EntityConfig<ProjectResource>
    {
        internal ProjectResourceConfig()
        {
            this.ToTable("projectresource");
            this.HasKey(p => p.Id);
            this.HasRequired(p => p.Project).WithMany().HasForeignKey(p => p.ProjectId);
        }
    }
}
