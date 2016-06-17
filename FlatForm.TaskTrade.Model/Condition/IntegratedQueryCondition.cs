using System;

namespace Peacock.PEP.Model.Condition
{
    //综合查询条件
    public class IntegratedQueryCondition
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 报告号
        /// </summary>
        public string ReportNo { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string ResidentialAreaName { get; set; }

        /// <summary>
        /// 询价机构
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 分支机构
        /// </summary>
        public string Subbranch { get; set; }

        /// <summary>
        /// 报询价人(客户名称)
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户手机
        /// </summary>
        public string CustomerTel { get; set; }

        /// <summary>
        /// 看房联系人电话
        /// </summary>
        public string ContactsPhone { get; set; }

        /// <summary>
        /// 物业类型
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// 报告类型
        /// </summary>
        public string ReportType { get; set; }

        /// <summary>
        /// 项目所属部门ID
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        /// 客户电话
        /// </summary>
        public string CustomerPhone { get; set; }

        /// <summary>
        /// 看房联系人
        /// </summary>
        public string ContactsName { get; set; }

        /// <summary>
        /// 立项备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 立项时间(开始)
        /// </summary>
        public DateTime? CreateTime_begin { get; set; }

        /// <summary>
        /// 立项时间(结束)
        /// </summary>
        public DateTime? CreateTime_end { get; set; }

        /// <summary>
        /// 项目状态（正常，取消）
        /// </summary>
        public string State { get; set; }
         
        public string BusinessType { get; set; }
        public string ProjectSource { get; set; }
        
        public string ProjectCreatorName { get; set; }
        public string ProjectAddress { get; set; } 
        public string ResidentialAddress { get; set; }
        public string BuildingArea { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string BuildedYear { get; set; }
        public string InquiryResult { get; set; }
        public string InquiryPrice { get; set; }
        public string EmergencyLevel { get; set; }
        public string Principal { get; set; }
        public string CreditBank { get; set; }
        public string CreditSubbranch { get; set; }
        public string CreditInfo { get; set; }
        public string IsProspecting { get; set; }
        public string Investigator { get; set; } 
        public string IsSubmitted { get; set; }
        public string IsApprove { get; set; }
        public string IsSent { get; set; }
        public DateTime? SubmitTime_Begin { get; set; }

        public DateTime? SubmitTime_end { get; set; }

        public DateTime? ApproveTime_Begin { get; set; }
        public DateTime? ApproveTime_end { get; set; }
        public string ApproverName { get; set; }
        public string IsUploadProject { get; set; } 
        public string Deleted { get; set; }
        public string SendReportCount { get; set; } 
        public string CustomerBank { get; set; }
        public string CustomerSubbranch { get; set; }
        public string CustomerQQ { get; set; } 
        public string SumCompany { get; set; }
        public string SumDepartment { get; set; }
        public string SumProjectName { get; set; }
        public string SumReportNo { get; set; }
        public string SumReportYear { get; set; }
        public string SumReportMonth { get; set; }
        public string SumEvalGoal { get; set; }
        public string SumGoalDescription { get; set; }
        public string SumEvalEntrust { get; set; }
        public string SumReportUse { get; set; }
        public string SumSubbranch { get; set; }
        public string SumSurveyPeople { get; set; }
        public string SumReportWriting { get; set; }
        public string SumProjectPopularizeName { get; set; }
        public string SumProjectProvince { get; set; }
        public string SumProjectCity { get; set; }
        public string SumProjectDistrict { get; set; }
        public string SumOtherAddress { get; set; }
        public string SumProjectAddress { get; set; }
        public DateTime? sumSurveyTime_Begin { get; set; }
        public DateTime? sumSurveyTime_End { get; set; }
        public DateTime? sumWorthTime_Begin { get; set; }

        public DateTime? sumWorthTime_End { get; set; }
        public DateTime? sumJobStartTime_Begin { get; set; }

        public DateTime? sumJobStartTime_End { get; set; }
        public DateTime? sumJobEndTime_Begin { get; set; }
         
        public DateTime? sumJobEndTime_End { get; set; }
        public string SumReportValidityTerm { get; set; }
        public string SumQuantitySurveyor1 { get; set; }
        public string SumQuantitySurveyor2 { get; set; }
        public string SumQuantitySurveyorJoin { get; set; }
        public string SumEvalMethod1 { get; set; }
        public string SumEvalMethod2 { get; set; }
        public string SumEvalMethodJoin { get; set; }
        public string SumEvaluateTotal { get; set; }
        public string SumEvaluatePrice { get; set; }
        public string SumHouserOwnerNo { get; set; }
        public string SumHouserOwner { get; set; }
        public string SumPropertyNature { get; set; }
        public string SumArchitectureArea { get; set; }
        public string SumBuildingStructure { get; set; }
        public string SumBuiltYear { get; set; }
        public string SumRegisterUse { get; set; }
        public string SumFloor { get; set; }
        public string SumMaxFloor { get; set; }
        public string SumFloorJoin { get; set; }
        public string SumHouseType { get; set; }
        public string SumDecoration { get; set; }
        public string SumLandUserNo { get; set; }
        public string SumLandAddress { get; set; }
        public string SumLandUsePeople { get; set; }
        public string SumLandUseType { get; set; }
        public string SumLandUseArea { get; set; }
        public string SumLandSpareYear { get; set; }
        public string SumSpareEconomicsYear { get; set; }
        public string SumMaxUseYear { get; set; }
        public string SumLandUse { get; set; }
        public DateTime? SumLandEndTime_Begin { get; set; }

        public DateTime? SumLandEndTime_End { get; set; }
        public string SumOtherInfluenceFactor { get; set; }
        public DateTime? SumBusinessTime_Begin { get; set; }

        public DateTime? SumBusinessTime_End { get; set; }
        public string SumLoopLine { get; set; }
        public string SumDealTotal { get; set; }
        public string SumDealPrice { get; set; }
        public DateTime? CancleTime_Begin { get; set; }

        public DateTime CancleTime_End { get; set; }
        public string CancleReason { get; set; }

    }
}
