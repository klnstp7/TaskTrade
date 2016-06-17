using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    /// <summary>
    /// 线上业务银行列表
    /// </summary>
    public class OnLineBusinessBankModel
    {
        public long Id { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>

        public long? OnBusinessId { get; set; }

        public string BankName { get; set; }

        public string BankbranchName { get; set; }

    }
}
