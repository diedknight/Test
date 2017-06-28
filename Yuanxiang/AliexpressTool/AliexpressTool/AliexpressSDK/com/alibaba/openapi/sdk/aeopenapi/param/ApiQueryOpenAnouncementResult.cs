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
public class ApiQueryOpenAnouncementResult {

       [DataMember(Order = 1)]
    private AeopAnouncementResult success;
    
        /**
       * @return 返回结果。
    */
        public AeopAnouncementResult getSuccess() {
               	return success;
            }
    
    /**
     * 设置返回结果。     *
          
             * 此参数必填
          */
    public void setSuccess(AeopAnouncementResult success) {
     	         	    this.success = success;
     	        }
    
    
  }
}