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
public class ApiQueryProductAddCartInfoEverydayByIdResult {

       [DataMember(Order = 1)]
    private string success;
    
        /**
       * @return 被加入购物车数量。
    */
        public string getSuccess() {
               	return success;
            }
    
    /**
     * 设置被加入购物车数量。     *
          
             * 此参数必填
          */
    public void setSuccess(string success) {
     	         	    this.success = success;
     	        }
    
    
  }
}