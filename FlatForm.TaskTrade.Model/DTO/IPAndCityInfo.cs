#region << 版 权 声 明 >>
/*
 * ========================================================================
 * Copyright(c) 2010-2015 北京云房数据技术有限责任公司, All Rights Reserved.
 * ========================================================================
*/
#endregion

namespace Peacock.PEP.Model
{
    /// <summary>
    /// 操作结果返回
    /// </summary>
    /// <remark>
    ///     <para>    Creator：贺隽 </para>
    ///     <para>CreatedTime：2016-4-1</para>
    /// </remark>
    public class IPAndCityInfo
    {
        public string country { set; get; }
        public string country_id { set; get; }
        public string area { set; get; }
        public string area_id { set; get; }
        public string region { set; get; }
        public string region_id { set; get; }
        public string city { set; get; }
        public string city_id { set; get; }
        public string county { set; get; }
        public string county_id { set; get; }
        public string isp { set; get; }
        public string isp_id { set; get; }
        public string ip { set; get; }
    }
}
