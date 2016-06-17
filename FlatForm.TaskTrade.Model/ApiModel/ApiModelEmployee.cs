
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.ApiModel
{
    public class ApiModelEmployee
    {
        /// <summary>
        /// 员工ID(CRM)
        /// </summary>
        public long TID { get; set; }
        /// <summary>
        /// 员工姓名(CRM)
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
      
    }
}
