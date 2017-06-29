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
public class ApiExtendsBuyerAcceptGoodsTimeResult {

       [DataMember(Order = 1)]
    private OperationResult result;
    
        /**
       * @return 
    */
        public OperationResult getResult() {
               	return result;
            }
    
    /**
     * 设置     *
          
             * 此参数必填
          */
    public void setResult(OperationResult result) {
     	         	    this.result = result;
     	        }
    
    
  }
}