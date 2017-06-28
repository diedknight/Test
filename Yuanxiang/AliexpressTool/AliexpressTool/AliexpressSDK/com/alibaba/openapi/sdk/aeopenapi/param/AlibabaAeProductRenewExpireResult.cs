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
public class AlibabaAeProductRenewExpireResult {

       [DataMember(Order = 1)]
    private AlibabaaeproductAeopModifyProductResponse modifyResponse;
    
        /**
       * @return 
    */
        public AlibabaaeproductAeopModifyProductResponse getModifyResponse() {
               	return modifyResponse;
            }
    
    /**
     * 设置     *
          
             * 此参数必填
          */
    public void setModifyResponse(AlibabaaeproductAeopModifyProductResponse modifyResponse) {
     	         	    this.modifyResponse = modifyResponse;
     	        }
    
    
  }
}