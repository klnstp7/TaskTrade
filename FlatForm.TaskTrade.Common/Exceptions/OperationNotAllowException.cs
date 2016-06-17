using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peacock.Common.Exceptions
{
    public class OperationNotAllowException : PredefinedException
    {
        private const string DefaultMessage = "不允许执行当前操作";
		public OperationNotAllowException() : base("不允许执行当前操作")
		{
		}
        public OperationNotAllowException(string msg)
            : base(string.Format("{0}：{1}", "不允许执行当前操作", msg))
		{
		}
    }
}
