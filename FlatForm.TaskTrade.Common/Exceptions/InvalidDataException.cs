using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peacock.Common.Exceptions
{
    public class InvalidDataException : PredefinedException
    {
        private const string DefaultMessage = "数据错误";
        public InvalidDataException()
            : base("数据错误")
        {
        }
        public InvalidDataException(string msg)
            : base(string.Format("{0}：{1}", "数据错误", msg))
        {
        }
    }
}
