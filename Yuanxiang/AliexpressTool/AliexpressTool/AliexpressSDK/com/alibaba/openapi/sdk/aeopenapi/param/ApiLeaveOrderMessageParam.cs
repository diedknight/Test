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
public class ApiLeaveOrderMessageParam {

       [DataMember(Order = 1)]
    private long? orderId;
    
        /**
       * @return 订单ID
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置订单ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 2)]
    private string content;
    
        /**
       * @return 留言内容
    */
        public string getContent() {
               	return content;
            }
    
    /**
     * 设置留言内容     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setContent(string content) {
     	         	    this.content = content;
     	        }
    
    
  }
}