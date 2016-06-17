using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peacock.Common.Exceptions
{

    public class InvalidWFOperationException : PredefinedException
    {
        public InvalidWFOperationException()
            : base()
        {
        }

        public InvalidWFOperationException(string message)
            : base(message)
        {
        }
    }

}
