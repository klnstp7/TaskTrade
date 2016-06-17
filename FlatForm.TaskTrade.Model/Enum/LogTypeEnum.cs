using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.Enum
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogTypeEnum
    {
        /// <summary>
        /// 调试
        /// </summary>
        Debug = 1,

        /// <summary>
        /// 用户操作日志
        /// </summary>
        Info = 2,

        /// <summary>
        /// 错误日志
        /// </summary>
        Error = 3,
    }
}
