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
public class AlibabaaewarrantyWarrantyInfo {

       [DataMember(Order = 1)]
    private long? orderId;
    
        /**
       * @return 订单id
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置订单id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 2)]
    private string supplierId;
    
        /**
       * @return 服务商id
    */
        public string getSupplierId() {
               	return supplierId;
            }
    
    /**
     * 设置服务商id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSupplierId(string supplierId) {
     	         	    this.supplierId = supplierId;
     	        }
    
        [DataMember(Order = 3)]
    private string buyTime;
    
        /**
       * @return 服务购买时间
    */
        public string getBuyTime() {
               	return buyTime;
            }
    
    /**
     * 设置服务购买时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBuyTime(string buyTime) {
     	         	    this.buyTime = buyTime;
     	        }
    
        [DataMember(Order = 4)]
    private long? bizId;
    
        /**
       * @return 业务维一标识
    */
        public long? getBizId() {
               	return bizId;
            }
    
    /**
     * 设置业务维一标识     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBizId(long bizId) {
     	         	    this.bizId = bizId;
     	        }
    
        [DataMember(Order = 5)]
    private string startTime;
    
        /**
       * @return 服务开始时间
    */
        public string getStartTime() {
               	return startTime;
            }
    
    /**
     * 设置服务开始时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setStartTime(string startTime) {
     	         	    this.startTime = startTime;
     	        }
    
        [DataMember(Order = 6)]
    private string endTime;
    
        /**
       * @return 服务结束时间
    */
        public string getEndTime() {
               	return endTime;
            }
    
    /**
     * 设置服务结束时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setEndTime(string endTime) {
     	         	    this.endTime = endTime;
     	        }
    
    
  }
}