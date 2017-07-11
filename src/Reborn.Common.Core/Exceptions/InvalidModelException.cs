using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Reborn.Common.Core.Exceptions
{
    public class InvalidModelException : Exception
    {
        public InvalidModelException(IDictionary obj)
        {
            ExceptionInfo = obj;
        }

        public IDictionary ExceptionInfo { get; set; }

        public override IDictionary Data => ExceptionInfo;
    }
}
