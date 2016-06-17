using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    public class ConfigListFunctionModel
    {
        /// <summary>
        /// auto_increment
        /// </summary>
        public long tid { get; set; }

        /// <summary>
        /// 模块标识
        /// </summary>
        public string FunCode { get; set; }

        /// <summary>
        /// 模块描述
        /// </summary>
        public string FunDesc { get; set; }

        /// <summary>
        /// 上级菜单
        /// </summary>
        public string ParentMenu { get; set; }

        public ConfigFunctioncolModel ConfigFunctioncol { get; set; }

        //public List<ConfigSolutionModel> ConfigSolution { get; set; }
    }
}
