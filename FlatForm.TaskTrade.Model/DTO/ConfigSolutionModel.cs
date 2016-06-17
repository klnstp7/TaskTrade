using Peacock.PEP.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    public class ConfigSolutionModel
    {
        /// <summary>
        /// auto_increment
        /// </summary>
        public long tid { get; set; }

        /// <summary>
        /// 所属功能
        /// </summary>
        public virtual ConfigListFunctionModel ConfigListFunction { get; set; }

        /// <summary>
        /// 所属功能
        /// </summary>
        public long FuncID { get; set; }

        /// <summary>
        /// 解决方案名称
        /// </summary>
        public string SolutionName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 解决方案类型（）
        /// </summary>
        public SolutionUseType SolutionType { get; set; }

        /// <summary>
        /// 是否默认方案
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        public virtual List<ConfigUserFuncColModel> ConfigUserFuncCols { get; set; }
    }
}
