using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model
{
    public class CompanyResourseModel
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
        public string CreateTime { get; set; }

        /// <summary>
        /// 文件字节
        /// </summary>
        public byte[] FileConcent { get; set; }

        /// <summary>
        /// 文件后缀名
        /// </summary>
        public string Ext { get; set; }
    }
}
