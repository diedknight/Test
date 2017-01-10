using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon
{
    public class RetailerCountry
    {
        private String code;
        private String name;

        public RetailerCountry(String code, String name)
        {
            this.code = code;
            this.name = name;
        }

        public String getCode()
        {
            return code;
        }

        public String getName()
        {
            return name;
        }
    }
}
