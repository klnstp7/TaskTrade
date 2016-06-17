using Peacock.PEP.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    /// <summary>
    /// 报告文件信息存储地址
    /// </summary>
    public class ReportFilePathInfoDTO
    {
        /// <summary>
        /// 报告主文件地址
        /// </summary>
        public string WordPath { get; set; }

        /// <summary>
        /// 测算表地址
        /// </summary>
        public string ExcelPath { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }


        /// <summary>
        /// 压缩后的Html路径
        /// </summary>
        public string ExcelHtmlZipPath { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public string[] AttachmentPath { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string[] ImgPath { get; set; }


        /// <summary>
        /// 是否是系统库资源
        /// </summary>
        public bool IsResourceSys { get; set; }

        /// <summary>
        /// 资料类型
        /// </summary>
        public ResourcesEnum ResourcesType { get; set; }

        /// <summary>
        /// 报告文件类型
        /// </summary>
        public ReportEnum ReportFileType { get; set; }

        public long ResourceID { get; set; }
    }
}
