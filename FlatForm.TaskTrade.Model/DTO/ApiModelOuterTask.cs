using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.DTO
{
    /// <summary>
    /// 外业任务分配DTO
    /// </summary>
    public class ApiModelOuterTask
    {
        /// <summary>
        /// 委托编号
        /// </summary>
        public string PID { get; set; }

        /// <summary>
        /// 委托编号
        /// </summary>
        public string TaskNum { get; set; }

        /// <summary>
        /// 操作类型 派发/收回/重新派发
        /// </summary>
        public string OperationType { set; get; }

        /// <summary>
        /// 勘察人员姓名
        /// </summary>
        public string InquirerName { set; get; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public string TargetAddress { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { set; get; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string ResidentialArea { set; get; }

        /// <summary>
        /// comid
        /// </summary>
        public string CompanyID { get; set; }

        /// <summary>
        /// comid
        /// </summary>
        public string CompanyName { get; set; }

        //<summary>
        // 结算方式：单笔单结，月结
        // </summary>
        public string PayType { set; get; }

        /// <summary>
        /// 勘察表名称
        /// </summary>
        public string DDName { get; set; }

        /// <summary>
        /// 勘察表ID
        /// </summary>
        public string DataDefineId { get; set; }

        /// <summary>
        /// 是否下发到pad
        /// </summary>
        public bool IsPad { get; set; }

        /// <summary>
        /// 看房联系人
        /// </summary>
        public string ContactPerson { set; get; }

        /// <summary>
        /// 看房联系人电话
        /// </summary>
        public string ContactTel { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }

        /// <summary>
        /// 加急金额
        /// </summary>
        public double QuickPrice { get; set; }

        /// <summary>
        /// 应收费用
        /// </summary>
        public decimal AdjustFee { get; set; }

        /// <summary>
        /// 预收费用
        /// </summary>
        public decimal LiveSearchCharge { get; set; }

        /// <summary>
        /// 紧急程度（常规、紧急）
        /// </summary>
        public string EmergencyLevel { get; set; }

        /// <summary>
        /// 物业类型
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 委托人名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 委托人电话
        /// </summary>
        public string ClientTelephone { get; set; }

        /// <summary>
        /// 楼幢名称
        /// </summary>
        public string Building { get; set; }

        /// <summary>
        /// 建筑面积
        /// </summary>
        public string TargetArea { get; set; }

        /// <summary>
        /// 客户来源
        /// </summary>
        public string CustomerSource { get; set; }
    }
}
