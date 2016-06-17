using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Data.Entities
{
    public class OnLineBusiness
    {
        public OnLineBusiness()
        {
            LastupdateTime = DateTime.Now;
            CreateTime = DateTime.Now;
        }
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 交易编号
        /// </summary>
        public string TransactionNo { get; set; }

        /// <summary>
        /// 期望评估价
        /// </summary>
        public string Assessment { get; set; }

        /// <summary>
        /// 邮寄地址
        /// </summary>
        public string PostAddress { get; set; }

        /// <summary>
        /// 贷款类型
        /// </summary>
        public string LoanType { get; set; }

        /// <summary>
        /// 贷款成数
        /// </summary>
        public string LoanRatio { get; set; }

        /// <summary>
        /// 报告份数
        /// </summary>
        public string ReportQuantity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 规划用途
        /// </summary>
        public string PlanUse { get; set; }

        /// <summary>
        /// 物业类型
        /// </summary>
        public string PropertysType { get; set; }

        /// <summary>
        /// 房屋性质
        /// </summary>
        public string HouserOperties { get; set; }

        /// <summary>
        /// 物业地址
        /// </summary>
        public string HouseAddress { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        public string Orientation { get; set; }

        /// <summary>
        /// 楼层
        /// </summary>
        public string Floor { get; set; }

        /// <summary>
        /// 行政区域
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 小区名
        /// </summary>
        public string Community { get; set; }

        /// <summary>
        /// 建成年代
        /// </summary>
        public string BuildYear { get; set; }

        /// <summary>
        /// 买方姓名
        /// </summary>
        public string BuyerName { get; set; }

        /// <summary>
        /// 买方性别
        /// </summary>
        public string BuyerGender { get; set; }

        /// <summary>
        /// 买方身份证号
        /// </summary>
        public string BuyerID { get; set; }

        /// <summary>
        /// 卖方姓名
        /// </summary>
        public string SellerName { get; set; }

        /// <summary>
        /// 卖方身份证号
        /// </summary>
        public string SellerID { get; set; }

        /// <summary>
        /// 经纪人姓名
        /// </summary>
        public string DealuserName { get; set; }

        /// <summary>
        /// 经纪人电话
        /// </summary>
        public string DealuserMobile { get; set; }

        /// <summary>
        /// 金融顾问
        /// </summary>
        public string LinkMan { get; set; }

        /// <summary>
        /// 金融顾问
        /// </summary>
        public string LinkMobile { get; set; }

        /// <summary>
        /// 中台姓名
        /// </summary>
        public string ServName { get; set; }

        /// <summary>
        /// 中台电话
        /// </summary>
        public string ServMobile { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        public string RealMoney { get; set; }

        /// <summary>
        /// 网签价
        /// </summary>
        public string ContractMoney { get; set; }

        /// <summary>
        /// 紧急程度
        /// </summary>
        public string Urgency { get; set; }

        /// <summary>
        /// 线上业务所属部门ID
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 是否已受理
        /// </summary>
        public bool IsAccept { get; set; }

        /// <summary>
        /// 不受理原因
        /// </summary>
        public string RefusedReason { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public long? ProjectId { get; set; }

        /// <summary>
        /// 项目导航属性
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 期望评估单价
        /// </summary>
        public string AssessPrice { get; set; }

        /// <summary>
        /// 高低评
        /// </summary>
        public string HighLowAssess { get; set; }

        /// <summary>
        /// 总楼层
        /// </summary>
        public string TotalFloor { get; set; }

        /// <summary>
        /// 户型-室
        /// </summary>
        public string DoorModelRoom { get; set; }

        /// <summary>
        /// 户型-厅
        /// </summary>
        public string DoorModelHall { get; set; }

        /// <summary>
        /// 户型-卫
        /// </summary>
        public string DoorModelWash { get; set; }

        /// <summary>
        /// 装修情况
        /// </summary>
        public string DecorateCase { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 楼栋号
        /// </summary>
        public string FloorBuilding { get; set; }

        /// <summary>
        /// 单元
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 房间号
        /// </summary>
        public string HouseNo { get; set; }

        /// <summary>
        /// 卖房人电话
        /// </summary>
        public string SellerPhone { get; set; }

        /// <summary>
        /// 卖房人证件类型
        /// </summary>
        public string SellerCardType { get; set; }


        /// <summary>
        /// 借款人
        /// </summary>
        public string BorrowName { get; set; }

        /// <summary>
        /// 借款人证件类型
        /// </summary>
        public string BorrowCardType { get; set; }

        /// <summary>
        /// 借款人证件号码
        /// </summary>
        public string BorrowID { get; set; }

        /// <summary>
        /// 委托人
        /// </summary>
        public string DelegateName { get; set; }

        /// <summary>
        /// 委托人电话
        /// </summary>
        public string DelegatePhone { get; set; }

        /// <summary>
        /// 看房联系人
        /// </summary>
        public string SeeHouseLinker { get; set; }

        /// <summary>
        /// 看房地址
        /// </summary>
        public string SeeHouseAddr { get; set; }

        /// <summary>
        /// 报告接收人
        /// </summary>
        public string ReportReceive { get; set; }

        /// <summary>
        /// 报告接收地址-省份
        /// </summary>
        public string ReportReceiveProvince { get; set; }

        /// <summary>
        /// 报告接收地址-城市
        /// </summary>
        public string ReportReceiveCity { get; set; }

        /// <summary>
        /// 报告接收地址-行政区域
        /// </summary>
        public string ReportReceiveRegion { get; set; }

        /// <summary>
        /// 报告接收地址-街道地址
        /// </summary>
        public string ReportReceiveStreet { get; set; }

        /// <summary>
        /// 报告接收人电话
        /// </summary>
        public string ReportReceivePhone { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastupdateTime { get; set; }

        /// <summary>
        /// 委托人证件类型
        /// </summary>
        public string DelegateCardType { get; set; }

        /// <summary>
        /// 委托人证件号码
        /// </summary>
        public string DelegateCardId { get; set; }

        /// <summary>
        /// 是否已询价
        /// </summary>
        public bool IsInquiry { get; set; }

        /// <summary>
        /// 是否已终止
        /// </summary>
        public bool IsStop { get; set; }

        /// <summary>
        /// 委托类型(-1 无,预评估 0，正式委托 1，人工询价 2)
        /// </summary>
        public DelegateTypeEnum DelegateType { get; set; }

        /// <summary>
        /// 业务反馈
        /// </summary>
        public virtual ICollection<OnLineFeedBack> OnLineFeedBacks { get; set; }

        public virtual ICollection<OnLineBusinessBank> OnLineBusinessBanks { get; set; }

        public virtual ICollection<ExplorationContacts> ExplorationContacts { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        internal class OnLineBusinessConfig : EntityConfig<OnLineBusiness>
        {
            internal OnLineBusinessConfig()
                : base(50)
            {
                base.ToTable("OnlineBusiness");
                base.HasKey(x => x.Id);
                base.Property(x => x.SeeHouseLinker).HasMaxLength(2000);
                base.Property(x=>x.SeeHouseAddr).HasMaxLength(200);
                base.Property(x=>x.ReportReceiveStreet).HasMaxLength(200);
                base.Property(x => x.HouseAddress).HasMaxLength(200);
                base.Property(x => x.Remark).HasMaxLength(255);
                //base.Property(x => x.BankbranchName).HasMaxLength(100);
                //base.Property(x => x.BankName).HasMaxLength(100);
                base.HasRequired(x => x.Project).WithMany().HasForeignKey(x => x.ProjectId);
                base.HasMany(x => x.OnLineBusinessBanks).WithRequired(x=>x.OnLineBusiness).HasForeignKey(x=>x.OnBusinessId);
                base.HasMany(x => x.ExplorationContacts).WithRequired(x => x.OnLineBusiness).HasForeignKey(x=>x.OnlineBusinessId);
            }
        }
        /// <summary>
        /// 委托类型
        /// </summary>
        public enum DelegateTypeEnum
        { 
          无=-1,预评估=0,正式委托=1,人工询价=2
        }
    }
}
