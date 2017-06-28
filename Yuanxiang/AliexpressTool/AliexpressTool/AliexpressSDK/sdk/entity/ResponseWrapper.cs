using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.entity
{
    class ResponseWrapper
    {
        private String invokeStartTime;

        public String InvokeStartTime
        {
            get { return invokeStartTime; }
            set { invokeStartTime = value; }
        }
        private long invokeCostTime;

        public long InvokeCostTime
        {
            get { return invokeCostTime; }
            set { invokeCostTime = value; }
        }
        private ResponseStatus status;

        internal ResponseStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        private Object result;

        public Object Result
        {
            get { return result; }
            set { result = value; }
        }
    }
}
