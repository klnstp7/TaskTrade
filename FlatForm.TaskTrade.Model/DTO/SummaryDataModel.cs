using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    public class SummaryDataModel
    {
        /// <summary>
        /// 主键，自增长
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 报告年份
        /// </summary>
        public Nullable<int> ReportYear { get; set; }

        /// <summary>
        /// 报告月份
        /// </summary>
        public Nullable<int> ReportMonth { get; set; }

        /// <summary>
        /// 报告编号
        /// </summary>
        public string ReportNo { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 估价目的
        /// </summary>
        public string EvalGoal { get; set; }

        /// <summary>
        /// 目的描述
        /// </summary>
        public string GoalDescription { get; set; }

        /// <summary>
        /// 项目所在省
        /// </summary>
        public string ProjectProvince { get; set; }

        /// <summary>
        /// 项目所在市
        /// </summary>
        public string ProjectCity { get; set; }

        /// <summary>
        /// 项目区县
        /// </summary>
        public string ProjectDistrict { get; set; }

        /// <summary>
        /// 小区地址
        /// </summary>
        public string ResidentialAddress { get; set; }

        /// <summary>
        /// 余下地址
        /// </summary>
        public string OtherAddress { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public string ProjectAddress { get; set; }

        /// <summary>
        /// 项目推广名
        /// </summary>
        public string ProjectPopularizeName { get; set; }

        /// <summary>
        /// 估价委托人
        /// </summary>
        public string EvalEntrust { get; set; }

        /// <summary>
        /// 报告使用方
        /// </summary>
        public string ReportUse { get; set; }

        /// <summary>
        /// 支行
        /// </summary>
        public string Subbranch { get; set; }

        /// <summary>
        /// 报告有效期
        /// </summary>
        public string ReportValidityTerm { get; set; }

        /// <summary>
        /// 实地勘察日期
        /// </summary>
        public Nullable<System.DateTime> SurveyTime { get; set; }

        /// <summary>
        /// 价值日期
        /// </summary>
        public Nullable<System.DateTime> WorthTime { get; set; }

        /// <summary>
        /// 作业起始日期
        /// </summary>
        public Nullable<System.DateTime> JobStartTime { get; set; }

        /// <summary>
        /// 作业终止日期
        /// </summary>
        public Nullable<System.DateTime> JobEndTime { get; set; }

        /// <summary>
        /// 现场勘察人员
        /// </summary>
        public string SurveyPeople { get; set; }

        /// <summary>
        /// 报告撰写人
        /// </summary>
        public string ReportWriting { get; set; }

        /// <summary>
        /// 估价师1
        /// </summary>
        public string QuantitySurveyor1 { get; set; }

        /// <summary>
        /// 估价师2
        /// </summary>
        public string QuantitySurveyor2 { get; set; }

        /// <summary>
        /// 估价师合并
        /// </summary>
        public string QuantitySurveyorJoin { get; set; }

        /// <summary>
        /// 估价方法1
        /// </summary>
        public string EvalMethod1 { get; set; }

        /// <summary>
        /// 估价方法2
        /// </summary>
        public string EvalMethod2 { get; set; }

        /// <summary>
        /// 估价方法汇总
        /// </summary>
        public string EvalMethodJoin { get; set; }

        /// <summary>
        /// 评估总价
        /// </summary>
        public Nullable<double> EvaluateTotal { get; set; }

        /// <summary>
        /// 评估单价
        /// </summary>
        public Nullable<double> EvaluatePrice { get; set; }

        /// <summary>
        /// 成交总价（万元）
        /// </summary>
        public Nullable<double> DealTotal { get; set; }

        /// <summary>
        /// 成交单价（元/㎡）
        /// </summary>
        public Nullable<double> DealPrice { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        public Nullable<System.DateTime> BusinessTime { get; set; }

        /// <summary>
        /// 处置扣税额(万元)
        /// </summary>
        public string HandleTaxQuota { get; set; }

        /// <summary>
        /// 扣税后净值（万元）
        /// </summary>
        public string TaxAfterNet { get; set; }

        /// <summary>
        /// 环线
        /// </summary>
        public string LoopLine { get; set; }

        /// <summary>
        /// 其它影响因素
        /// </summary>
        public string OtherInfluenceFactor { get; set; }

        /// <summary>
        /// 补交出让金/综合地价款（元）
        /// </summary>
        public string PayLeasing { get; set; }

        /// <summary>
        /// 优先受偿款
        /// </summary>
        public string PriorityRepaymentInfo { get; set; }

        /// <summary>
        /// 物理结构是否变化
        /// </summary>
        public string PhysicalHasChanged { get; set; }

        /// <summary>
        /// 使用现状
        /// </summary>
        public string StatusQuo { get; set; }

        /// <summary>
        /// 登记用途
        /// </summary>
        public string RegisterUse { get; set; }

        /// <summary>
        /// 现状用途
        /// </summary>
        public string PresentUse { get; set; }

        /// <summary>
        /// 建筑结构
        /// </summary>
        public string BuildingStructure { get; set; }

        /// <summary>
        /// 建成年代
        /// </summary>
        public string BuiltYear { get; set; }

        /// <summary>
        /// 所在楼层
        /// </summary>
        public string Floor { get; set; }

        /// <summary>
        /// 总楼层
        /// </summary>
        public string MaxFloor { get; set; }

        /// <summary>
        /// 楼层
        /// </summary>
        public string FloorJoin { get; set; }

        /// <summary>
        /// 户型
        /// </summary>
        public string HouseType { get; set; }

        /// <summary>
        /// 房屋所有权号
        /// </summary>
        public string HouserOwnerNo { get; set; }

        /// <summary>
        /// 房屋所有权人
        /// </summary>
        public string HouserOwner { get; set; }

        /// <summary>
        /// 建筑面积（㎡）
        /// </summary>
        public Nullable<decimal> ArchitectureArea { get; set; }

        /// <summary>
        /// 产别
        /// </summary>
        public string Yield { get; set; }

        /// <summary>
        /// 装修程度
        /// </summary>
        public string Decoration { get; set; }

        /// <summary>
        /// 产权性质
        /// </summary>
        public string PropertyNature { get; set; }

        /// <summary>
        /// 最高使用年限
        /// </summary>
        public Nullable<int> MaxUseYear { get; set; }

        /// <summary>
        /// 剩余经济寿命
        /// </summary>
        public Nullable<int> SpareEconomicsYear { get; set; }

        /// <summary>
        /// 土地使用权人
        /// </summary>
        public string LandUsePeople { get; set; }

        /// <summary>
        /// 土地用途
        /// </summary>
        public string LandUse { get; set; }

        /// <summary>
        /// 使用权类型
        /// </summary>
        public string LandUseType { get; set; }

        /// <summary>
        /// 土地使用权面积（㎡）
        /// </summary>
        public Nullable<int> LandUseArea { get; set; }

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo { get; set; }

        /// <summary>
        /// 地号
        /// </summary>
        public string LandNo { get; set; }

        /// <summary>
        /// 终止日期
        /// </summary>
        public Nullable<System.DateTime> LandEndTime { get; set; }

        /// <summary>
        /// 剩余土地年限（年）
        /// </summary>
        public Nullable<float> LandSpareYear { get; set; }

        /// <summary>
        /// 国有土地使用号
        /// </summary>
        public string LandUserNo { get; set; }

        /// <summary>
        /// 土地坐落
        /// </summary>
        public string LandAddress { get; set; }

        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string BorrowerName { get; set; }

        /// <summary>
        /// 借款人身份证
        /// </summary>public string BorrowerName { get; set; }
        public string BorrowerId { get; set; }

        /// <summary>
        /// 项目负责人
        /// </summary>
        public string ProjectHead { get; set; }

        /// <summary>
        /// 单价类型
        /// </summary>
        public string UnitPriceType { get; set; }

        /// <summary>
        /// 朝向
        /// 备注:南北、东西、东、南、西、北、东南、西南、西北、东北
        /// </summary>

        public string Toword { get; set; }

        /// <summary>
        /// 土地增值税税额（元）
        /// </summary>

        public string LandVatQuota { get; set; }

        /// <summary>
        /// 扣除土增税后的价值（万元）
        /// </summary>

        public string TaxAfterVatQuota { get; set; }

        /// <summary>
        /// 楼栋号
        /// </summary>

        public string BuildingNo { get; set; }

        /// <summary>
        /// 单元号
        /// </summary>

        public string BuildingUnitNo { get; set; }

        /// <summary>
        /// 户号
        /// </summary>

        public string HouseNo { get; set; }

        /// <summary>
        /// 汇总数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateTime { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 项目导航属性
        /// </summary>
        public ProjectModel Project { get; set; }
    }
}
