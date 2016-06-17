using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{ /// <summary>
    /// 项目文档
    /// </summary>
    public class ProjectDocument
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 线上业务Id
        /// </summary>
        public long? OnLineBussinessId { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public long? ProjectId { get; set; }

        /// <summary>
        /// 资源库Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 在线业务
        /// </summary>
        public OnLineBusiness OnLineBusiness { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// 文件类别（房产证、身份证、其他）
        /// </summary>
        public string FileClass { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string FileFormat { get; set; }

        /// <summary>
        /// 文件类型（图片、音频、视频、其他）
        /// </summary>
        public string FileType { get; set; }

    }

    internal class ProjectDocumentConfig : EntityConfig<ProjectDocument>
    {
        internal ProjectDocumentConfig()
        {
            base.ToTable("projectdocument");
            base.HasKey(p => p.Id);
            base.HasRequired(p => p.OnLineBusiness).WithMany().HasForeignKey(p => p.OnLineBussinessId);
            base.HasRequired(p => p.Project).WithMany().HasForeignKey(p => p.ProjectId);
        }
    }
}
