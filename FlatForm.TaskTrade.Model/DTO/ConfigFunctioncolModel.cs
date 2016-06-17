using Peacock.PEP.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    public class ConfigFunctioncolModel
    {
        /// <summary>
        /// auto_increment
        /// </summary>
        public long tid { get; set; }

        /// <summary>
        /// 所属功能
        /// </summary>
        public ConfigListFunctionModel ConfigListFunction { get; set; }

        public long FuncID { get; set; }

        /// <summary>
        /// 列英文名称
        /// </summary>
        public string ColCode { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// 控件类型（供以后扩展自定义查询条件用）
        /// </summary>
        public string CotorlType { get; set; }

        /// <summary>
        /// 控件类型
        /// </summary>
        public string CotorlCss { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 使用类型
        /// </summary>
        public SolutionUseType UseType { get; set; }

        /// <summary>
        /// 格式化
        /// </summary>
        public string StrFormat { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 是否默认显示
        /// </summary>
        public bool IsDefault { get; set; }

        public List<ConfigUserFuncColModel> ConfigUserFuncCol { get; set; }
    }
}
