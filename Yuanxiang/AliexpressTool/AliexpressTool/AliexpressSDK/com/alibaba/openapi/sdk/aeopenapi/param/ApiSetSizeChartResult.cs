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
public class ApiSetSizeChartResult {

       [DataMember(Order = 1)]
    private bool? success;
    
        /**
       * @return 尺码表设置成功
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置尺码表设置成功     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}