using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.Enum
{
    /// <summary>
    /// 报告资源枚举
    /// </summary>
    public enum ResourcesEnum
    {
        /// <summary>
        /// 报告
        /// </summary>
        报告 = 1,

        /// <summary>
        /// 计算表
        /// </summary>
        计算表
    }

    /// <summary>
    /// 报告类型
    /// </summary>
    public enum ReportEnum
    {
        完整报告 = 0,
        技术报告,
        结果报告,
        计算过程表,
        其他资料
    }
}
