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
public class ApiDelUnUsePhotoResult {

       [DataMember(Order = 1)]
    private bool? success;
    
        /**
       * @return success:调用是否成功, true或者false;
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置success:调用是否成功, true或者false;     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}