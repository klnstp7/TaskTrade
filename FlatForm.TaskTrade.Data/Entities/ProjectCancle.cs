using System;

namespace Peacock.PEP.Data.Entities
{
    public class ProjectCancle
    {
        /// <summary>
        /// 主键，自增长
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime CancleTime { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 项目导航属性
        /// </summary>
        //public virtual Project Project { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        public string Reason { get; set; }
    }
    internal class ProjectCancleConfig : EntityConfig<ProjectCancle>
    {
        internal ProjectCancleConfig():base(20)
        {
            ToTable("Project_Cancle");
            HasKey(x => x.Id);
            //HasRequired(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId);
            Property(x => x.Reason).HasMaxLength(255);
        }
    }
}
