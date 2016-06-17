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
    public class ResultInfo
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { set; get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// 是否已经登录
        /// </summary>
        public bool IsAuthenticated { set; get; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public dynamic Data { set; get; }

        /// <summary>
        /// 其他附加数据
        /// </summary>
        public dynamic Others { set; get; }

        public ResultInfo()
        {
            Success = false;
            Message = "操作失败！";
            IsAuthenticated = true;
        }

    }
}
