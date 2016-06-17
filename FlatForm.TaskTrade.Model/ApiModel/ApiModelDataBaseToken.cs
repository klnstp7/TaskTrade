using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.ApiModel
{
    [Serializable]
    public class ApiModelDataBaseToken
    {
        /// <summary>
        /// Token
        /// </summary>
        public string BaseToken { get; set; }

        /// <summary>
        /// Token失效时间
        /// </summary>
        public DateTime BaseTokenExpirationTime { get; set; }
    }
}
