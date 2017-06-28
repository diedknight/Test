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
public class ApiExtendsBuyerAcceptGoodsTimeParam {

       [DataMember(Order = 1)]
    private long? orderId;
    
        /**
       * @return 需要延长的订单ID。
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置需要延长的订单ID。     *
     * 参数示例：<pre>1234567890</pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 2)]
    private int? day;
    
        /**
       * @return 请求延长的具体天数。
    */
        public int? getDay() {
               	return day;
            }
    
    /**
     * 设置请求延长的具体天数。     *
     * 参数示例：<pre>30</pre>     
             * 此参数必填
          */
    public void setDay(int day) {
     	         	    this.day = day;
     	        }
    
    
  }
}