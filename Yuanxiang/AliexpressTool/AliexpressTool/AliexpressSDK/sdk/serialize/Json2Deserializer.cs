using com.alibaba.openapi.client.entity;
using com.alibaba.openapi.client.exception;
using com.alibaba.openapi.client.policy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.serialize
{
    class Json2Deserializer : DeSerializer
    {
        //返回该反序列化接口支持的数据协议.
        public String supportedContentType()
        {
            return Protocol.json2;
        }

        public ResponseWrapper deSerialize(Stream istream, Type resultType, String charSet)
        {
            StreamReader sr = new StreamReader(istream, Encoding.UTF8);
            string strhtml = sr.ReadToEnd();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strhtml)))
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(resultType);
                object result = jsonSerializer.ReadObject(ms);
                ResponseWrapper responseWrapper = new ResponseWrapper();
                responseWrapper.Result = result;

                return responseWrapper;
            }
        }

        public Exception buildException(Stream istream, int statusCode, String charSet)
        {


            istream.Position = 0;
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ErrorExceptionDesc));
            object resultObj = jsonSerializer.ReadObject(istream);
            ErrorExceptionDesc result = (ErrorExceptionDesc)resultObj;
            String errorCodeStr = result.getError_code();
            String errorMesage = result.getError_message();

            OceanException oceanException = new OceanException(errorMesage);
            oceanException.setError_code(errorCodeStr);
            oceanException.setError_message(errorMesage);
            return oceanException;


        }
    }
}
