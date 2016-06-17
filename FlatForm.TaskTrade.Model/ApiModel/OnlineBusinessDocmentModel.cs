using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peacock.PEP.Model.ApiModel
{
    public class OnlineBusinessDocmentModel
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        [CheckFieldValidate(true, "文件类型", EnumStr = "图片,音频,视频,其他")]
        public string fileType { get; set; }

        /// <summary>
        /// 文件类别
        /// </summary>
        [CheckFieldValidate(true, "文件类别", EnumStr = "房产证,身份证,军官证,护照,港澳台身份证,其他")]
        public string fileCategory { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        [CheckFieldValidate(true, "文件格式")]
        public string fileFormat { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [CheckFieldValidate(true, "文件名称")]
        public string fileName { get; set; }

        /// <summary>
        /// 资源库ID
        /// </summary>
         [CheckFieldValidate(true, "文件名称",IsCheckNull=true)]
        public long resourceID { get; set; }
    }
}
