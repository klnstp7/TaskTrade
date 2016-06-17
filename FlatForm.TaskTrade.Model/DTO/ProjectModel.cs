using System;
using System.Collections;
using System.Collections.Generic;
using Peacock.PEP.Model.DTO;

namespace Peacock.PEP.Model
{
    public class ProjectModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 项目表id
        /// </summary>
        public long ProjectId { get; set; }

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
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }

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
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 行政区
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
        /// 贷款信息
        /// </summary>
        public string CreditInfo { get; set; }

        /// <summary>
        /// 是否需要勘察外业       
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
        public string IsSubmitted { get; set; }

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
        public CustomerModel Customer { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 是否已删除        
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 贷款支行
        /// </summary>
        public string CreditSubbranch { get; set; }

        ///// <summary>
        ///// 已发报告数量
        ///// </summary>
        public int SendReportCount { get; set; }

        /// <summary>
        /// 客户姓名(客户)
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 联系电话(客户)
        /// </summary>
        public string CustomerTel { get; set; }

        /// <summary>
        /// 固定电话(客户)
        /// </summary>
        public string CustomerPhone { get; set; }

        /// <summary>
        /// 所属机构(客户)
        /// </summary>
        public string CustomerBank { get; set; }

        /// <summary>
        /// 分支机构(客户)
        /// </summary>
        public string CustomerSubbranch { get; set; }

        /// <summary>
        /// QQ(客户)
        /// </summary>
        public string CustomerQQ { get; set; }

        /// <summary>
        /// 报告类型
        /// </summary>
        public string ReportType { get; set; }

        /// <summary>
        /// 外采勘察表名称
        /// </summary>
        public string OutSurveyTableName { get; set; }

        /// <summary>
        /// 外采勘察表id
        /// </summary>
        public string OutSurveyTableId { get; set; }

        /// <summary>
        /// 看房联系人json
        /// </summary>
        public string ExplorationContactsStr { get; set; }

        //汇总数据
        public SummaryDataModel SummaryData { get; set; }

        //看房联系人
        public ICollection<ExplorationContactsModel> ExplorationContacts { get; set; }

        /// <summary>
        /// 是否已取消
        /// </summary>
        public bool IsCancle { get; set; }

        /// <summary>
        /// 在线业务ID，界面用
        /// </summary>
        public string onlineid { get; set; }
    }
}
