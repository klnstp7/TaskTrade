using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class ConfigListFunction
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

        public virtual ICollection<ConfigFunctioncol> ConfigFunctioncols {get;set; }

        public virtual ICollection<ConfigSolution> ConfigSolutions { get; set; }
    }
    internal class ConfigListFunctionConfig : EntityConfig<ConfigListFunction>
    {
        internal ConfigListFunctionConfig()
            : base(20)
        {
            base.ToTable("Config_ListFunction");
            base.HasKey(x=>x.tid);
            base.HasMany(x => x.ConfigSolutions).WithRequired(x => x.ConfigListFunction).HasForeignKey(x=>x.FuncID);
            base.HasMany(x => x.ConfigFunctioncols).WithRequired(x => x.ConfigListFunction).HasForeignKey(x => x.FuncID);
        }
    }
}
