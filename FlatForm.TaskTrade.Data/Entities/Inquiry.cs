using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class Inquiry
    {

        /// <summary>
        /// auto_increment
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// ResidentialAreaName
        /// </summary>
        public string ResidentialAreaName { get; set; }

        /// <summary>
        /// ResidentialAddress
        /// </summary>
        public string ResidentialAddress { get; set; }

        /// <summary>
        /// BuildingArea
        /// </summary>
        public decimal? BuildingArea { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// District
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// BuildedYear
        /// </summary>
        public int? BuildedYear { get; set; }

        /// <summary>
        /// InquiryResult
        /// </summary>
        public decimal? InquiryResult { get; set; }

        /// <summary>
        /// InquiryPrice
        /// </summary>
        public decimal? InquiryPrice { get; set; }

        /// <summary>
        /// FeedbackPrice
        /// </summary>
        public decimal? FeedbackPrice { get; set; }

        /// <summary>
        /// MortgagePrice
        /// </summary>
        public decimal? MortgagePrice { get; set; }

        /// <summary>
        /// MortgageResult
        /// </summary>
        public decimal? MortgageResult { get; set; }

        /// <summary>
        /// CustomerId
        /// </summary>
        public long? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        /// <summary>
        /// BuildingName
        /// </summary>
        public string BuildingName { get; set; }

        /// <summary>
        /// UnitName
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// HouseNum
        /// </summary>
        public string HouseNum { get; set; }

        /// <summary>
        /// PropertyType
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// Floor
        /// </summary>
        public string Floor { get; set; }

        /// <summary>
        /// MaxFloor
        /// </summary>
        public string MaxFloor { get; set; }

        /// <summary>
        /// Toword
        /// </summary>
        public string Toword { get; set; }

        /// <summary>
        /// HouseType
        /// </summary>
        public string HouseType { get; set; }

        /// <summary>
        /// Decoration
        /// </summary>
        public string Decoration { get; set; }

        /// <summary>
        /// SpecialInfo
        /// </summary>
        public string SpecialInfo { get; set; }

        /// <summary>
        /// InquirySource
        /// </summary>
        public string InquirySource { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// FeedbackMessage
        /// </summary>
        public string FeedbackMessage { get; set; }

        /// <summary>
        /// CreatorId
        /// </summary>
        public long CreatorId { get; set; }

        /// <summary>
        /// CreatorName
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// IsToProject
        /// </summary>
        public bool IsToProject { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public long DepartmentId { get; set; }

        #region 询价单新增字段

        /// <summary>
        /// 询价单编号
        /// </summary>
        public string InquiryNo { get; set; }

        /// <summary>
        /// 估价使用方
        /// </summary>
        public string AppraiseUse { get; set; }

        /// <summary>
        /// 估价委托人
        /// </summary>
        public string AppraiseDelegatePerson { get; set; }

        /// <summary>
        /// 估价对象
        /// </summary>
        public string AppraiseObject { get; set; }

        /// <summary>
        /// 估价目的
        /// </summary>
        public string AppraisePurpose { get; set; }

        /// <summary>
        /// 价值时点
        /// </summary>
        public DateTime? AppraiseDate { get; set; }

        /// <summary>
        /// 房屋结构
        /// </summary>
        public string HouseStruct { get; set; }

        /// <summary>
        /// 他项权利状况
        /// </summary>
        public bool OtherPower { get; set; }

        /// <summary>
        /// 他项权利状况
        /// </summary>
        public string OtherPowerDesc { get; set; }

        /// <summary>
        /// 大写金额
        /// </summary>
        public string UpperAmount { get; set; }

        /// <summary>
        /// 预计处置税费
        /// </summary>
        public decimal? DisposeFee { get; set; }

        /// <summary>
        /// 应补地价
        /// </summary>
        public decimal? LandPremium { get; set; }

        /// <summary>
        /// 扣后净值
        /// </summary>
        public decimal? MinusNetWorth { get; set; }

        /// <summary>
        /// 特殊事项说明
        /// </summary>
        public string SpecialRemark { get; set; }

        /// <summary>
        /// 出具日期
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ValidityDate { get; set; }

        /// <summary>
        /// 估价机构名称
        /// </summary>
        public string AppraiseOrg { get; set; }

        /// <summary>
        /// 设定用途
        /// </summary>
        public string SetPurpose { get; set; }

        /// <summary>
        /// 设定其他
        /// </summary>
        public string SetOther { get; set; }

        /// <summary>
        /// 估价目的描述
        /// </summary>
        public string AppraisePurposeDesc { get; set; }

        /// <summary>
        /// 询价单有效期
        /// </summary>
        public string EffectivePeriod { get; set; }

        /// <summary>
        /// 是否有看房记录
        /// </summary>
        public bool IsHouseRecord { get; set; }

        /// <summary>
        /// 是否存在抵押
        /// </summary>
        public bool IsMortgage { get; set; }

        /// <summary>
        /// 抵押描述
        /// </summary>
        public string MortgageDesc { get; set; }
        #endregion

    }

    internal class InquiryConfig : EntityConfig<Inquiry>
    {
        internal InquiryConfig()
        {
            base.ToTable("Inquiry");
            base.HasKey(x => x.Id);
            base.HasRequired(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId);
            base.Property(x => x.ResidentialAreaName).HasMaxLength(100);
            base.Property(x => x.ResidentialAddress).HasMaxLength(100);
            base.Property(x => x.BuildingName).HasMaxLength(50);
            base.Property(x => x.UnitName).HasMaxLength(50);
            base.Property(x => x.HouseNum).HasMaxLength(50);
            base.Property(x => x.Decoration).HasMaxLength(100);
            base.Property(x => x.SpecialInfo).HasMaxLength(100);
            base.Property(x => x.Address).HasMaxLength(255);
            base.Property(x => x.Remark).HasMaxLength(255);
            base.Property(x => x.FeedbackMessage).HasMaxLength(255);
            base.Property(x => x.SpecialRemark).HasMaxLength(10000);
            base.Property(x=>x.OtherPowerDesc).HasMaxLength(5000);
        }
    }
}
