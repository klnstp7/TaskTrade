using Peacock.PEP.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.ApiModel
{
    /// <summary>
    /// 在线业务报告推送项目推送实体
    /// </summary>
    public class OnlineBusinessApiModel
    {
        //public string dealuserName { get; set; }

        //public string dealuserMobile { get; set; }

        //public string reportDownloadUrl { get; set; }
        /// <summary>
        /// 备注-委托
        /// </summary>
        [CheckFieldValidate(false, "备注-委托", MaxLength = 255)]
        public string comment { get; set; }

        /// <summary>
        /// 规划用途
        /// </summary>
        [CheckFieldValidate(false, "规划用途", EnumStr = "住宅,商业,办公,其他")]
        public string planUse { get; set; }

        /// <summary>
        ///物业类型 
        /// </summary>
        [CheckFieldValidate(false, "物业类型", EnumStr = "住宅,别墅,成套办公,成套商业,工业")]
        public string houseType { get; set; }

        /// <summary>
        /// 房屋地址(证载)
        /// </summary>
        [CheckFieldValidate(true, "房屋地址(证载)")]
        public string address { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        [CheckFieldValidate(true, "面积", isCheckNum = true)]
        public string area { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        [CheckFieldValidate(false, "朝向", EnumStr = "南,南北,东南,西南,东北,西北,东西,东,西,北")]
        public string toward { get; set; }

        /// <summary>
        /// 行政区域
        /// </summary>
        [CheckFieldValidate(false, "行政区域")]
        public string region { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        [CheckFieldValidate(true, "小区名称")]
        public string residentialAreaName { get; set; }

        /// <summary>
        /// 建成年代
        /// </summary>
        [CheckFieldValidate(false, "建成年代", isCheckNum = true)]
        public string buildYear { get; set; }

        /// <summary>
        /// 卖房人姓名
        /// </summary>
        [CheckFieldValidate(false, "卖房人姓名")]
        public string sellerName { get; set; }

        /// <summary>
        /// 卖房人证件类型
        /// </summary>
        [CheckFieldValidate(false, "卖房人电话", EnumStr = "身份证,港澳台身份证,军官证,护照")]
        public string sellerCardType { get; set; }

        /// <summary>
        /// 卖房人电话
        /// </summary>
        [CheckFieldValidate(false, "卖房人电话")]
        public string sellerPhone { get; set; }

        /// <summary>
        /// 卖方证件号
        /// </summary>
        [CheckFieldValidate(false, "卖方证件号")]
        public string sellerID { get; set; }

        /// <summary>
        /// 经纪人姓名
        /// </summary>
        [CheckFieldValidate(false, "业务联系人")]
        public string bussinessContactName { get; set; }

        /// <summary>
        /// 经纪人电话
        /// </summary>
        [CheckFieldValidate(false, "业务联系人电话")]
        public string bussinessContactTelephone { get; set; }

        /// <summary>
        /// 分配给指定的公司ID
        /// </summary>
        [CheckFieldValidate(true, "评估公司ID", isCheckNum = true, MaxLength = 20)]
        public long evaluationCompanyId { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public string dataSource { get; set; }

        /// <summary>
        /// 期望评估总价
        /// </summary>
        [CheckFieldValidate(true, "期望评估总价", isCheckNum = true, MaxLength = 20)]
        public string expectPrice { get; set; }

        /// <summary>
        /// 期望评估单价
        /// </summary>
        [CheckFieldValidate(false, "期望评估单价", isCheckNum = true, MaxLength = 20)]
        public string assessPrice { get; set; }

        /// <summary>
        /// 贷款银行
        /// </summary>
        public List<BankInfo> bank { get; set; }

        /// <summary>
        /// 高低评
        /// </summary>
        [CheckFieldValidate(true, "高低评", "高评", EnumStr = "高评,低评")]
        public string highLowAssess { get; set; }

        /// <summary>
        /// 总楼层
        /// </summary>
        [CheckFieldValidate(false, "总楼层", isCheckNum = true)]
        public string totalFloor { get; set; }

        /// <summary>
        /// 楼层
        /// </summary>
        [CheckFieldValidate(false, "楼层", isCheckNum = true)]
        public string floor { get; set; }

        /// <summary>
        /// 户型-室
        /// </summary>
        [CheckFieldValidate(false, "户型-室", "零", EnumStr = "零,一,二,三,四,五,六,七,八")]
        public string roomType { get; set; }

        /// <summary>
        /// 户型-厅
        /// </summary>
        [CheckFieldValidate(false, "户型-厅", "零", EnumStr = "零,一,二,三,四,五,六,七,八")]
        public string doorModelHall { get; set; }

        /// <summary>
        /// 户型-卫
        /// </summary>
        [CheckFieldValidate(false, "户型-卫", "零", EnumStr = "零,一,二,三,四,五,六,七,八")]
        public string doorModelWash { get; set; }

        /// <summary>
        /// 装修情况
        /// </summary>
        [CheckFieldValidate(false, "装修情况", EnumStr = "毛坯,中等装修,精装,豪装")]
        public string decorateCase { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [CheckFieldValidate(true, "城市")]
        public string cityName { get; set; }

        /// <summary>
        /// 楼栋名称
        /// </summary>
        [CheckFieldValidate(false, "楼栋名称")]
        public string floorBuilding { get; set; }

        /// <summary>
        /// 单元
        /// </summary>
        [CheckFieldValidate(false, "单元")]
        public string unitName { get; set; }

        /// <summary>
        /// 房间号
        /// </summary>
        [CheckFieldValidate(false, "户号")]
        public string houseNo { get; set; }

        /// <summary>
        /// 借款人
        /// </summary>
        [CheckFieldValidate(false, "借款人")]
        public string borrowName { get; set; }

        /// <summary>
        /// 借款人证件类型
        /// </summary>
        [CheckFieldValidate(false, "借款人证件类型", EnumStr = "身份证,港澳台身份证,军官证,护照")]
        public string borrowCardType { get; set; }

        /// <summary>
        /// 借款人证件号码
        /// </summary>
        [CheckFieldValidate(false, "借款人证件号码")]
        public string borrowID { get; set; }

        /// <summary>
        /// 委托人
        /// </summary>
        [CheckFieldValidate(false, "委托人")]
        public string delegateName { get; set; }

        /// <summary>
        /// 委托人电话
        /// </summary>
        [CheckFieldValidate(false, "委托人电话")]
        public string delegatePhone { get; set; }

        /// <summary>
        /// 委托人证件类型
        /// </summary>
        [CheckFieldValidate(false, "委托人证件类型", EnumStr = "身份证,港澳台身份证,军官证,护照")]
        public string delegateCardType { get; set; }

        /// <summary>
        /// 委托人证件号码
        /// </summary>
        [CheckFieldValidate(false, "委托人证件号码")]
        public string delegateCardID { get; set; }

        /// <summary>
        /// 看房联系人
        /// </summary>
        [CheckFieldValidate(false, "看房联系人", MaxLength = 2000)]
        public List<houseContact> houseContact { get; set; }

        /// <summary>
        /// 看房地址
        /// </summary>
        [CheckFieldValidate(false, "看房地址", MaxLength = 200)]
        public string seeHouseAddr { get; set; }

        /// <summary>
        /// 报告接收人
        /// </summary>
        [CheckFieldValidate(false, "收件人")]
        public string recipientName { get; set; }

        /// <summary>
        /// 报告接受地址
        /// </summary>
        public string recipientAddress { get; set; }

        /// <summary>
        /// 报告接收地址-省份
        /// </summary>
        [CheckFieldValidate(false, "报告接收地址-省份")]
        public string reportReceiveProvince { get; set; }

        /// <summary>
        /// 报告接收地址-城市
        /// </summary>
        [CheckFieldValidate(false, "报告接收地址-城市")]
        public string reportReceiveCity { get; set; }

        /// <summary>
        /// 报告接收地址-行政区域
        /// </summary>
        [CheckFieldValidate(false, "报告接收地址-行政区域")]
        public string reportReceiveRegion { get; set; }

        /// <summary>
        /// 报告接收地址-街道地址
        /// </summary>
        [CheckFieldValidate(false, "报告接收地址-街道地址", MaxLength = 200)]
        public string reportReceiveStreet { get; set; }

        /// <summary>
        /// 报告接收人电话
        /// </summary>
        [CheckFieldValidate(false, "收件人电话")]
        public string recipientTelephone { get; set; }

        /// <summary>
        /// 贷款类型
        /// </summary>
        public string loanType { get; set; }

        /// <summary>
        /// 贷款成数
        /// </summary>
        public string loanRatio { get; set; }

        /// <summary>
        /// 报告份数
        /// </summary>
        [CheckFieldValidate(false, "报告份数", isCheckNum = true)]
        public string reportNum { get; set; }

        /// <summary>
        /// 房屋性质
        /// </summary>
        public string houseNature { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        [CheckFieldValidate(false, "成交价", isCheckNum = true)]
        public string realMoney { get; set; }

        /// <summary>
        /// 网签价
        /// </summary>
        [CheckFieldValidate(false, "网签价", isCheckNum = true)]
        public string contractMoney { get; set; }
    }

    public class BankInfo
    {
        /// <summary>
        /// 贷款总行银行
        /// </summary>
        public string bankName { get; set; }

        /// <summary>
        /// 贷款银行分行
        /// </summary>
        public string bankbranchName { get; set; }

    }

    public class houseContact
    {
        /// <summary>
        /// 看房联系人
        /// </summary>
        public string houseContactName { get; set; }

        /// <summary>
        /// 看房联系人电话
        /// </summary>
        public string houseContactTelephone { get; set; }
    }
}
