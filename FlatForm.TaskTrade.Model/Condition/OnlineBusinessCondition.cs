using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.Condition
{
    public class OnLineBusinessCondition
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string ProjectNo { get; set; }
        /// <summary>
        /// 交易编号
        /// </summary>
        public string TransactionNo { get; set; }

        /// <summary>
        /// 物业地址
        /// </summary>
        public string HouseAddress { get; set; }

        /// <summary>
        /// 小区名
        /// </summary>
        public string Community { get; set; }


    }
}
