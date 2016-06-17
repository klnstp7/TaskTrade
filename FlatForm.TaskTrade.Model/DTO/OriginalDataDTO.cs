using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace EIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <remark>
    ///     <para>    Creator：helang </para>
    ///     <para>CreatedTime：2012-10-10</para>
    /// </remark>
    public class OriginalDataDTO
    {
        public OriginalDataDTO()
        {
            DDID = 0;
            DataDefineVersion = -1;
            ID = 0;
            TargetAddress = "";
            TaskNum = "";
            Status = 0;
            CreatedDate = DateTime.Now.ToString("yyyy-MM-dd");
            User = "";
            IsUrgent = "0";
            ReceiveDate = DateTime.Now.ToString("yyyy-MM-dd");
            DoneDate = DateTime.Now.ToString("yyyy-MM-dd");
            ModifyDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            Count = 0;
            Total = 0;
            Remark = "";
            TargetNumber = "";
            TargetArea = "";
            TargetType = "住宅";
            TargetName = "";
            OwnerTelePhone = "";
            Owner = "";
            ClientTelephone = "";
            ClientName = "";
            ClientDepartment = "";
            ClientUnit = "";
            Building = "";
            ResidentialArea = "";
            Floor = "";
            ContactPerson = "";
            ContactTel = "";
            TaskRemark = "";
            CustomerSource = "";
        }


        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("ID")]
        public long ID { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        [DisplayName("配置ID")]
        public long DDID { get; set; }


        /// <summary>
        /// 公司编号
        /// </summary>
        [DisplayName("公司编号")]
        public long CompanyId { get; set; }

        /// <summary>
        /// 配置表版本
        /// </summary>
        public int DataDefineVersion { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public String CreatedDate { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public string ModifyDate { set; get; }

        /// <summary>
        /// PID
        /// </summary>
        [DisplayName("PID")]
        public String TaskNum { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [DisplayName("备注信息")]
        public string Remark { set; get; }

        /// <summary>
        /// 领取时间
        /// </summary>
        [DisplayName("领取时间")]
        public string ReceiveDate { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [DisplayName("完成时间")]
        public string DoneDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 估价物编号
        /// </summary>
        [DisplayName("估价物编号")]
        public string TargetNumber { set; get; }

        /// <summary>
        /// 完成项目数
        /// </summary>
        [DisplayName("完成项目数")]
        public int Count { set; get; }

        /// <summary>
        /// 紧急程度
        /// </summary>
        [DisplayName("紧急程度")]
        public string IsUrgent { get; set; }

        /// <summary>
        /// 总项目数
        /// </summary>
        [DisplayName("总项目数")]
        public int Total { set; get; }

        /// <summary>
        /// 地址
        /// </summary>
        [DisplayName("地址")]
        public String TargetAddress { get; set; }

        /// <summary>
        /// 业主
        /// </summary>        
        [DisplayName("业主")]
        public string Owner { set; get; }

        /// <summary>
        /// 业主电话
        /// </summary>        
        [DisplayName("业主电话")]
        public string OwnerTelePhone { set; get; }

        /// <summary>
        /// 小区名称
        /// </summary>
        [DisplayName("小区名称")]
        public string ResidentialArea { set; get; }

        /// <summary>
        /// 楼栋名称
        /// </summary>
        [DisplayName("楼栋名称")]
        public string Building { set; get; }

        /// <summary>
        /// 楼层
        /// </summary>
        [DisplayName("楼层")]
        public string Floor { set; get; }

        /// <summary>
        /// 物业类型
        /// </summary>        
        [DisplayName("物业类型")]
        public string TargetName { set; get; }

        /// <summary>
        /// 用途
        /// </summary>        
        [DisplayName("用途")]
        public string TargetType { set; get; }

        /// <summary>
        /// 建筑面积
        /// </summary>        
        [DisplayName("建筑面积（平方米）")]
        public string TargetArea { set; get; }


        /// <summary>
        /// 委托人单位
        /// </summary>        
        [DisplayName("委托人单位")]
        public string ClientUnit { set; get; }

        /// <summary>
        /// 委托人部门
        /// </summary>        
        [DisplayName("委托人部门")]
        public string ClientDepartment { set; get; }

        /// <summary>
        /// 委托人名称
        /// </summary>        
        [DisplayName("委托人名称")]
        public string ClientName { set; get; }

        /// <summary>
        /// 委托人联系电话
        /// </summary>        
        [DisplayName("委托人联系电话")]
        public string ClientTelephone { set; get; }

        /// <summary>
        /// 用户
        /// </summary>
        [DisplayName("用户")]
        public string User { get; set; }

        /// <summary>
        /// 创建方式，用户自建、系统界面创建、系统统一创建、第三方系统创建等
        /// </summary>
        public int CreateType { set; get; }

        /// <summary>
        /// 收费金额
        /// </summary>
        [DisplayName("收费金额")]
        public string Fee { set; get; }

        /// <summary>
        /// 收据号
        /// </summary>
        [DisplayName("收据号")]
        public string ReceiptNo { set; get; }

        /// <summary>
        /// 领取人编号
        /// </summary>
        public long InquirerNumber { set; get; }

        /// <summary>
        /// 领取人
        /// </summary>
        public string InquirerName { set; get; }

        /// <summary>
        /// 待领取数量
        /// </summary>
        public int TodoCount { set; get; }

        /// <summary>
        /// 待提交数量
        /// </summary>
        public int DoingCount { set; get; }


        /// <summary>
        /// 预约人
        /// </summary>
        public string ContactPerson { set; get; }

        /// <summary>
        /// 预约电话
        /// </summary>
        public string ContactTel { set; get; }

        /// <summary>
        /// 内业报告是否完成
        /// </summary>
        public bool InworkReportFinish { set; get; }

        /// <summary>
        /// 加急费用
        /// </summary>
        public double UrgentFee { get; set; }

        /// <summary>
        /// 应收费用
        /// </summary>
        public double AdjustFee { get; set; }

        /// <summary>
        /// 预收费用
        /// </summary>
        public double LiveSearchCharge { get; set; }

        /// <summary>
        /// 暂停时间
        /// </summary>
        public string PauseDate { get; set; }

        /// <summary>
        /// 重启时间
        /// </summary>
        public string RestartDate { get; set; }

        /// <summary>
        /// crm部门编号
        /// </summary>
        public long Crmdepid { get; set; }

        /// <summary>
        /// 用户在移动端设置备注信息
        /// </summary>
        public string TaskRemark { set; get; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { set; get; }

        /// <summary>
        ///  客户来源
        /// </summary>
        public string CustomerSource { get; set; }

    }
}