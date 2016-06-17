using Peacock.PEP.Service.Base;
using Peacock.PEP.Model.Enum;
using System.IO;
using Peacock.Common.Helper;
using System;

namespace Peacock.PEP.Service
{
    public class LogService : SingModel<LogService>
    { 
        private LogService()
        {

        }

        #region 写入日志到本地硬盘

        /// <summary>
        /// 写入日志到本地硬盘
        /// </summary>
        /// <param name="concent">记录内容</param>
        /// <param name="userAccount">操作用户</param>
        /// <param name="logType">日志类型</param>
        public void Ilog(string concent,string userAccount = "", LogTypeEnum logType = LogTypeEnum.Info)
        {
            string token = DateTime.Now.ToString("yyyyMMddHH");
            string logRoot =  FileStreamHelper.GetPathAtApplication("log");
            StreamWriter sw = new StreamWriter(logRoot, true);
            sw.WriteLine(concent);
            sw.Close();//写入
        }

        #endregion
    }
}
