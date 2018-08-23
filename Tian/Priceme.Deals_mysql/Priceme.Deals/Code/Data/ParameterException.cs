using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMe
{
    public class ParameterException : Exception
    {
        public ParameterException(string message)
            : base(message)
        {
        }
    }
}