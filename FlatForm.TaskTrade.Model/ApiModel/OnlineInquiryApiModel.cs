using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.ApiModel
{
    /// <summary>
    /// 在线业务报告推送询价单
    /// </summary>
    public class OnlineInquiryApiModel
    {

        /// <summary>
        /// 城市
        /// </summary>
        [CheckFieldValidate(true, "城市")]
        public string cityName { get; set; }

        /// <summary>
        /// 分配给指定的公司ID
        /// </summary>
        [CheckFieldValidate(true, "分配给指定的公司", isCheckNum = true, MaxLength = 20)]
        public long evaluationCompanyId { get; set; }

        /// <summary>
        /// 行政区域
        /// </summary>
        [CheckFieldValidate(true, "行政区域")]
        public string region { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        [CheckFieldValidate(true, "小区名称")]
        public string residentialAreaName { get; set; }

        /// <summary>
        /// 楼栋名称
        /// </summary>
        [CheckFieldValidate(true, "楼栋名称")]
        public string floorBuilding { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        [CheckFieldValidate(false, "朝向", EnumStr = "南,南北,东南,西南,东北,西北,东西,东,西,北")]
        public string toward { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        [CheckFieldValidate(false, "面积", isCheckNum = true)]
        public string area { get; set; }

        /// <summary>
        /// 居室类型
        /// </summary>
        [CheckFieldValidate(false, "居室类型", EnumStr = "零,一,二,三,四,五,六,七,八")]
        public string roomType { get; set; }

        /// <summary>
        /// 规划用途
        /// </summary>
        [CheckFieldValidate(false, "规划用途")]
        public string planUse { get; set; }

        /// <summary>
        /// 所在楼层
        /// </summary>
        [CheckFieldValidate(true, "所在楼层", isCheckNum = true)]
        public string floor { get; set; }

        /// <summary>
        /// 总楼层
        /// </summary>
        [CheckFieldValidate(true, "总楼层", isCheckNum = true)]
        public string totalFloor { get; set; }

        /// <summary>
        /// 建成年代
        /// </summary>
        [CheckFieldValidate(false, "建成年代", isCheckNum = true)]
        public string buildYear { get; set; }

        /// <summary>
        /// 装修情况
        /// </summary>
        [CheckFieldValidate(false, "装修情况", EnumStr = "毛坯,中等装修,精装,豪装")]
        public string decorateCase { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [CheckFieldValidate(false, "备注", MaxLength = 255)]
        public string comment { get; set; }

        /// <summary>
        /// 贷款银行
        /// </summary>
        [CheckFieldValidate(false, "贷款银行")]
        public string BankName { get; set; }

        /// <summary>
        /// 期望评估总价
        /// </summary>
        [CheckFieldValidate(false, "期望评估总价", isCheckNum = true)]
        public string expectPrice { get; set; }

        /// <summary>
        /// 评估期望（高评、低评）
        /// </summary>
        [CheckFieldValidate(false, "高低评", "高评", EnumStr = "高评,低评")]
        public string highLowAssess { get; set; }
        
        /// <summary>
        /// 贷款银行
        /// </summary>
        public List<BankInfo> bank { get; set; }
    }
}
