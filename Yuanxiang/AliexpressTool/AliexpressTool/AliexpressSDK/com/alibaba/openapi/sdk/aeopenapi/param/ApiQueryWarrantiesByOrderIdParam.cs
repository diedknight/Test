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
public class ApiQueryWarrantiesByOrderIdParam {

       [DataMember(Order = 1)]
    private long? orderId;
    
        /**
       * @return 订单号
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置订单号     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 2)]
    private string supplierId;
    
        /**
       * @return 供应商id
    */
        public string getSupplierId() {
               	return supplierId;
            }
    
    /**
     * 设置供应商id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSupplierId(string supplierId) {
     	         	    this.supplierId = supplierId;
     	        }
    
    
  }
}