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
public class ApiSellerModifiedShipmentResult {

       [DataMember(Order = 1)]
    private bool? success;
    
        /**
       * @return success=true 返回成功，否则失败
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置success=true 返回成功，否则失败     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}