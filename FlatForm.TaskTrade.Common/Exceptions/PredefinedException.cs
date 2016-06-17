using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peacock.Common.Exceptions
{
    public class PredefinedException : ApplicationException
    {
		public PredefinedException()
		{
		}
        public PredefinedException(string msg)
            : base(msg)
		{
		}
    }
}
