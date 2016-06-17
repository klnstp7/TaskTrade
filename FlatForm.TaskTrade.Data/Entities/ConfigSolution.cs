using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class ConfigSolution
    {
        /// <summary>
        /// auto_increment
        /// </summary>
        public long tid { get; set; }

        /// <summary>
        /// 所属功能
        /// </summary>
        public virtual ConfigListFunction ConfigListFunction { get; set; }

        /// <summary>
        /// 所属功能
        /// </summary>
        public long FuncID { get; set; }

        /// <summary>
        /// 解决方案名称
        /// </summary>
        public string SolutionName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 解决方案类型（）
        /// </summary>
        public UseType SolutionType { get; set; }

        /// <summary>
        /// 是否默认方案
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        public virtual ICollection<ConfigUserFuncCol> ConfigUserFuncCols { get; set; }
    }

    internal class ConfigSolutionConfig : EntityConfig<ConfigSolution>
    {
        internal ConfigSolutionConfig()
            : base(50)
        {
            base.ToTable("Config_Solution");
            base.HasKey(x => x.tid);
            base.Property(x => x.Remark).HasMaxLength(200);
            base.HasRequired(x => x.ConfigListFunction).WithMany(x => x.ConfigSolutions).HasForeignKey(x=>x.FuncID);
            //base.HasMany(x => x.ConfigListFunction).WithRequiredDependent(x=>x.ConfigSolution).Map(x => x.MapKey("FuncID"));
            base.HasMany(x => x.ConfigUserFuncCols).WithRequired(x => x.ConfigSolution).HasForeignKey(x => x.SolutionID).WillCascadeOnDelete(true);
           
        }
    }
    /// <summary>
    /// 使用类型
    /// </summary>
    public enum UseType
    {
        全部 = -1,
        查询 = 0,
        列表 = 1,
        导出 = 2
    }
}
