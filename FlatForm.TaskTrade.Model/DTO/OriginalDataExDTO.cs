using System;
using System.ComponentModel;

namespace EIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <remark>
    ///     <para>    Creator：helang </para>
    ///     <para>CreatedTime：2013-02-20</para>
    /// </remark>
    public class OriginalDataExDTO : OriginalDataDTO
    {
        public OriginalDataExDTO()
        {
          
            DistrictName = "";
            CityName = "";
            ProvinceName = "";
            CompanyName = "";
            DDName = "";
            OperationType = "";
            InquirerName = "";
            EmergencyLevel = "常规";
        }

        #region Add by 2013-2-19 09:56 贺隽
        /// <summary>
        /// 配置表编号
        /// </summary>
        [DisplayName("配置表编号")]
        public string DataDefineId { set; get; }
        

        /// <summary>
        /// 是否下发到PAD
        /// </summary>
        [DisplayName("是否下发到PAD")]
        public bool IsPad { set; get; }

        /// <summary>
        /// 行政区名称
        /// </summary>
        [DisplayName("行政区名称")]
        public string DistrictName { set; get; }

        /// <summary>
        /// 行政区编码
        /// </summary>
        [DisplayName("行政区编码")]
        public string DistrictCode { set; get; }

        /// <summary>
        /// 城市名称
        /// </summary>
        [DisplayName("城市名称")]
        public string CityName { set; get; }

        /// <summary>
        /// 城市编码
        /// </summary>
        [DisplayName("城市编码")]
        public string CityCode { set; get; }

        /// <summary>
        /// 省份名称
        /// </summary>
        [DisplayName("省份名称")]
        public string ProvinceName { set; get; }

        /// <summary>
        /// 省份编码
        /// </summary>
        [DisplayName("省份编码")]
        public string ProvinceCode { set; get; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [DisplayName("公司名称")]
        public string CompanyName { set; get; }

        /// <summary>
        /// 使用的配置
        /// </summary>
        [DisplayName("使用的配置")]
        public string DDName { set; get; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [DisplayName("操作类型")]
        public string OperationType { set; get; }        

        /// <summary>
        /// 紧急程度
        /// </summary>
        [DisplayName("紧急程度")]
        public string EmergencyLevel { set; get; }
        
        /// <summary>
        /// 匹配地址
        /// </summary>
        [DisplayName("匹配地址")]
        public string CommunityAddress { set; get; }
        
        /// <summary>
        /// 加急金额
        /// </summary>
        public double QuickPrice { set; get; }        
        #endregion
    }
}