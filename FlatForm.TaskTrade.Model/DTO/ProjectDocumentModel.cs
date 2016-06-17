using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{ /// <summary>
    /// 项目文档
    /// </summary>
    public class ProjectDocumentModel
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 线上业务Id
        /// </summary>
        public long OnLineBussinessId { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { get; set; }

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
        public OnLineBusinessModel OnLineBusiness { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public ProjectModel Project { get; set; }

        /// <summary>
        /// 原图URL
        /// </summary>
        public string OriginalImageUrl { get; set; }

        /// <summary>
        /// 缩略图URL
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// 后缀名
        /// </summary>
        public string Extension { get; set; }
    }
}
