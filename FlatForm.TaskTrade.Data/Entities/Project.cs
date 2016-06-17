using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    /// <summary>
    /// 项目
    /// </summary>
    public class Project
    {

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 报告编号
        /// </summary>
        public string ReportNo { get; set; }

        /// <summary>
        ///物业类型
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// 估价目的
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 项目来源
        /// </summary>
        public string ProjectSource { get; set; }

        /// <summary>
        /// 项目类型(合作项目，非合作项目) //项目类型干掉，目前不要
        /// </summary>
        //  public string ProjectType { get; set; }

        /// <summary>
        /// 项目所属部门ID
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// 立项人用户ID
        /// </summary>
        public long ProjectCreatorId { get; set; }

        /// <summary>
        /// 立项人用户
        /// </summary>
        public string ProjectCreatorName { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public string ProjectAddress { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string ResidentialAreaName { get; set; }

        /// <summary>
        /// 小区地址
        /// </summary>
        public string ResidentialAddress { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public decimal? BuildingArea { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 建成年代
        /// </summary>
        public int? BuildedYear { get; set; }

        /// <summary>
        /// 询值总价
        /// </summary>
        public decimal? InquiryResult { get; set; }

        /// <summary>
        /// 询值单价
        /// </summary>
        public decimal? InquiryPrice { get; set; }

        /// <summary>
        /// 紧急程度
        /// </summary>
        public string EmergencyLevel { get; set; }

        /// <summary>
        /// 估价委托方
        /// </summary>
        public string Principal { get; set; }

        /// <summary>
        /// 贷款银行
        /// </summary>
        public string CreditBank { get; set; }

        /// <summary>
        /// 贷款支行
        /// </summary>
        public string CreditSubbranch { get; set; }

        /// <summary>
        /// 贷款信息
        /// </summary>
        public string CreditInfo { get; set; }

        /// <summary>
        /// 是否需要勘察外采
        /// </summary>
        public bool IsProspecting { get; set; }

        /// <summary>
        /// 外业人员
        /// </summary>
        public string Investigator { get; set; }

        /// <summary>
        /// 项目备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否提交审核
        /// </summary>
        public bool IsSubmitted { get; set; }

        /// <summary>
        /// 是否审核通过
        /// </summary>
        public bool IsApprove { get; set; }

        /// <summary>
        /// 是否发送
        /// </summary>
        public bool IsSent { get; set; }

        /// <summary>
        /// 提交审核时间
        /// </summary>
        public DateTime? SubmitTime { get; set; }

        /// <summary>
        /// 审核通过时间
        /// </summary>
        public DateTime? ApproveTime { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
        public long ApproverId { get; set; }

        /// <summary>
        /// 审核人名称
        /// </summary>
        public string ApproverName { get; set; }

        /// <summary>
        /// 是否上传项目
        /// </summary>
        public bool IsUploadProject { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 报告类型
        /// </summary>
        public string ReportType { get; set; }

        //汇总数据
        public virtual SummaryData SummaryData { get; set; }

        /// <summary>
        /// 外采勘察表名称
        /// </summary>
        public string OutSurveyTableName { get; set; }

        /// <summary>
        /// 外采勘察表id
        /// </summary>
        public string OutSurveyTableId { get; set; }

        //看房联系人
        public virtual ICollection<ExplorationContacts> ExplorationContacts { get; set; }

        /// <summary>
        /// 是否已取消
        /// </summary>
        public bool IsCancle { get; set; }
    }

    internal class ProjectConfig : EntityConfig<Project>
    {
        internal ProjectConfig()
        {
            base.ToTable("project");
            base.HasKey(p => p.Id);
            base.HasRequired(p => p.Customer).WithMany().HasForeignKey(p => p.CustomerId);
            HasRequired(x => x.SummaryData).WithRequiredPrincipal(x => x.Project).Map(x => x.MapKey("ProjectID"));
            HasMany(x => x.ExplorationContacts).WithRequired(x => x.Project).HasForeignKey(x => x.ProjectId);
        }
    }
}
