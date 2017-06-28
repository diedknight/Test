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
public class ApiFindProductInfoListQueryV2Result {

       [DataMember(Order = 1)]
    private bool? isSuccess;
    
        /**
       * @return 是否成功
    */
        public bool? getIsSuccess() {
               	return isSuccess;
            }
    
    /**
     * 设置是否成功     *
          
             * 此参数必填
          */
    public void setIsSuccess(bool isSuccess) {
     	         	    this.isSuccess = isSuccess;
     	        }
    
    
  }
}