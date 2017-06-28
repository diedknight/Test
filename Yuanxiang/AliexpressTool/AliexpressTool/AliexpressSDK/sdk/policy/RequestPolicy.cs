using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.policy
{
    class RequestPolicy
    {
        private bool requestSendTimestamp = false;

        public bool RequestSendTimestamp
        {
            get { return requestSendTimestamp; }
            set { requestSendTimestamp = value; }
        }
        private bool useHttps = false;

        public bool UseHttps
        {
            get { return useHttps; }
            set { useHttps = value; }
        }
        private string requestProtocol = Protocol.param2;

        internal string RequestProtocol
        {
            get { return requestProtocol; }
            set { requestProtocol = value; }
        }
        private string responseProtocol = Protocol.json2;

        internal string ResponseProtocol
        {
            get { return responseProtocol; }
            set { responseProtocol = value; }
        }
        private bool responseCompress = true;

        public bool ResponseCompress
        {
            get { return responseCompress; }
            set { responseCompress = value; }
        }
        private int requestCompressThreshold = -1;

        public int RequestCompressThreshold
        {
            get { return requestCompressThreshold; }
            set { requestCompressThreshold = value; }
        }
        private int timeout = 5000;

        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }
        private string httpMethod = "POST";

        public string HttpMethod
        {
            get { return httpMethod; }
            set { httpMethod = value; }
        }
        private String queryStringCharset = "GB18030";

        public String QueryStringCharset
        {
            get { return queryStringCharset; }
            set { queryStringCharset = value; }
        }
        private String contentCharset = "UTF-8";

        public String ContentCharset
        {
            get { return contentCharset; }
            set { contentCharset = value; }
        }
        private bool useSignture = true;

        public bool UseSignture
        {
            get { return useSignture; }
            set { useSignture = value; }
        }
        private bool needAuthorization = false;

        public bool NeedAuthorization
        {
            get { return needAuthorization; }
            set { needAuthorization = value; }
        }
        private bool accessPrivateApi = false;

        public bool AccessPrivateApi
        {
            get { return accessPrivateApi; }
            set { accessPrivateApi = value; }
        }
        private int defaultApiVersion = 1;

        public int DefaultApiVersion
        {
            get { return defaultApiVersion; }
            set { defaultApiVersion = value; }
        }



    }
}
