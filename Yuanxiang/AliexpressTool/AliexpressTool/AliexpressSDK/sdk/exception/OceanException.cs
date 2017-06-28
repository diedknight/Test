using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.exception
{
    class OceanException : Exception
    {
        public OceanException(string mess)
            : base(mess)
        {
            
        }
         public OceanException()
        {

        }
        private string error_code;

        private string error_message;

        private string exception;

        public string getError_code()
        {
            return this.error_code;
        }

        public void setError_code(string error_code)
        {
            this.error_code = error_code;
        }

        public string getError_message()
        {
            return this.error_message;
        }

        public void setError_message(string error_message)
        {
            this.error_message = error_message;
        }

        public string getException()
        {
            return this.exception;
        }

        public void setException(String exception)
        {
            this.exception = exception;
        }

    }
}
