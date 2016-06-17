using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.ApiEnum
{
    public enum DataPermission
    {
        /// <summary>
        /// 查看公司
        /// </summary>
        Company,
        /// <summary>
        /// 查看部门
        /// </summary>
        Department,
        /// <summary>
        /// 查看部门及子部门
        /// </summary>
        AllDepartment,
        /// <summary>
        /// 查看当前用户
        /// </summary>
        CurrentUser
    }
}
