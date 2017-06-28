using com.alibaba.openapi.client.primitive;
using com.alibaba.openapi.client.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace com.alibaba.openapi.sdk.aeopenapi.param
{
[DataContract(Namespace = "com.alibaba.openapi.client")]
public class ApiQueryAccountLevelParam {

       [DataMember(Order = 1)]
    private string loginId;
    
        /**
       * @return 买家账号ID
    */
        public string getLoginId() {
               	return loginId;
            }
    
    /**
     * 设置买家账号ID     *
     * 参数示例：<pre>jordenmail</pre>     
             * 此参数必填
          */
    public void setLoginId(string loginId) {
     	         	    this.loginId = loginId;
     	        }
    
    
  }
}