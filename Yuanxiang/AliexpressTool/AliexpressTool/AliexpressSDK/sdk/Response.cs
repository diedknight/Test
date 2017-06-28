using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.alibaba.openapi.client.entity;

namespace com.alibaba.openapi.client
{
    class Response
    {
        private int statusCode;

        public int StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }
        private Object result;

        public Object Result
        {
            get { return result; }
            set { result = value; }
        }
        private Exception exception;

        public Exception Exception
        {
            get { return exception; }
            set { exception = value; }
        }
        private String charset = "UTF-8";

        public String Charset
        {
            get { return charset; }
            set { charset = value; }
        }
        private String encoding;

        public String Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        private ResponseWrapper responseWrapper;

        internal ResponseWrapper ResponseWrapper
        {
            get { return responseWrapper; }
            set { responseWrapper = value; }
        }
    }
}
