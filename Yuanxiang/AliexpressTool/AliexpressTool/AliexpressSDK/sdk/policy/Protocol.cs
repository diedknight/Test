using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.policy
{
    class Protocol
    {
        public  const string param2 = "param2";
        /**
         * 请求参数通过json串的方式传递,默认格式_data_={"key":"value"}
         */
        public  const string json2 = "json2";
        /**
         * 
         * 请求参数通过xml的方式传递,默认格式_data_=&lt;test>data&lt;/test>
         */
        public  const string xml2 = "xml2";
        public  const string param = "param";
        public  const string json = "json";
        public  const string xml = "xml";
        public  const string http = "http";
    }
}
