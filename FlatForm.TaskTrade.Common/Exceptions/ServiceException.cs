using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peacock.Common.Exceptions
{
    public class ServiceException : PredefinedException
    {
        public ServiceException(string errormessage)
            : base(errormessage)
        {

        }
    }
}
