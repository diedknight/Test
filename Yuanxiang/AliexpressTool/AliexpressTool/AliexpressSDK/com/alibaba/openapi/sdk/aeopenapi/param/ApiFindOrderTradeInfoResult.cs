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
public class ApiFindOrderTradeInfoResult {

       [DataMember(Order = 1)]
    private OrderTradeInfo result;
    
        /**
       * @return 
    */
        public OrderTradeInfo getResult() {
               	return result;
            }
    
    /**
     * 设置     *
          
             * 此参数必填
          */
    public void setResult(OrderTradeInfo result) {
     	         	    this.result = result;
     	        }
    
    
  }
}