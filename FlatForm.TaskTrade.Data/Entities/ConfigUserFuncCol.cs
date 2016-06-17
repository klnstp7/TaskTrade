using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class ConfigUserFuncCol
    {
        /// TID
        /// </summary>
        public long TID { get; set; }

        /// <summary>
        /// 列ID
        /// </summary>
        public long ColID { get; set; }

        public virtual ConfigFunctioncol ConfigFunctioncol { get; set; }

        /// <summary>
        /// 解决方案ID
        /// </summary>
        public long SolutionID { get; set; }

        public virtual ConfigSolution ConfigSolution { get; set; }
    }
    internal class ConfigUserFuncColConfig : EntityConfig<ConfigUserFuncCol>
    {
        internal ConfigUserFuncColConfig()
            : base(20)
        {
            base.HasKey(x => x.TID);
            base.ToTable("Config_UserFuncCol");
            base.HasRequired(x => x.ConfigSolution).WithMany(x => x.ConfigUserFuncCols).HasForeignKey(x => x.SolutionID).WillCascadeOnDelete(true);
            base.HasRequired(x => x.ConfigFunctioncol).WithMany(x => x.ConfigUserFuncCols).HasForeignKey(x => x.ColID);
        }
    }
}
