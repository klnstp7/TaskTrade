using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    public class ConfigUserFuncColModel
    {
        /// TID
        /// </summary>
        public long TID { get; set; }

        /// <summary>
        /// 列ID
        /// </summary>
        public long ColID { get; set; }

        /// <summary>
        /// 解决方案ID
        /// </summary>
        public long SolutionID { get; set; }

        public ConfigFunctioncolModel ConfigFunctionCol { get; set; }

        public ConfigSolutionModel ConfigSolution { get; set; }
    }
}
