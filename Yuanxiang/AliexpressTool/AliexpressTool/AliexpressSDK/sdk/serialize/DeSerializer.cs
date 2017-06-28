using com.alibaba.openapi.client.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.serialize
{
    interface DeSerializer
    {
        //返回该反序列化接口支持的数据协议.
         String supportedContentType();

         ResponseWrapper deSerialize(Stream istream, Type resultType, String charSet);

         Exception buildException(Stream inputStream, int statusCode, String charSet);

    }
}
