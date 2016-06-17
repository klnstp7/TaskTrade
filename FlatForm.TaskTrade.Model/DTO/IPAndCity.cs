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
    public class IPAndCity
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public int code { set; get; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public IPAndCityInfo data { set; get; }

        public IPAndCity()
        {
            code = 0;
        }

    }
}
