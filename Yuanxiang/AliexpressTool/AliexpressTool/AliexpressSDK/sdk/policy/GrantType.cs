using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.policy
{
    class GrantType
    {
       public  const string  refresh_token="refresh_token";
        /**
         * 请求参数通过json串的方式传递,默认格式_data_={"key":"value"}
         */
       public  const string get_token = "get_token";
    }
}
