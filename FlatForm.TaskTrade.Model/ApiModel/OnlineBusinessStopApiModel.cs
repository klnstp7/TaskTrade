using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.ApiModel
{
    public class OnlineBusinessStopApiModel
    {
        /// <summary>
        /// 终止原因
        /// </summary>
        [CheckFieldValidate(true, "终止原因")]
        public string reason { get; set; }
    }
}
