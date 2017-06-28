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
public class SimpleOrderItemVO {

       [DataMember(Order = 1)]
    private string bizType;
    
        /**
       * @return 订单类型
    */
        public string getBizType() {
               	return bizType;
            }
    
    /**
     * 设置订单类型     *
     * 参数示例：<pre>AE_COMMON</pre>     
             * 此参数必填
          */
    public void setBizType(string bizType) {
     	         	    this.bizType = bizType;
     	        }
    
        [DataMember(Order = 2)]
    private string gmtCreate;
    
        /**
       * @return 交易创建时间
    */
        public string getGmtCreate() {
               	return gmtCreate;
            }
    
    /**
     * 设置交易创建时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGmtCreate(string gmtCreate) {
     	         	    this.gmtCreate = gmtCreate;
     	        }
    
        [DataMember(Order = 3)]
    private string gmtModified;
    
        /**
       * @return 订单修改时间
    */
        public string getGmtModified() {
               	return gmtModified;
            }
    
    /**
     * 设置订单修改时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGmtModified(string gmtModified) {
     	         	    this.gmtModified = gmtModified;
     	        }
    
        [DataMember(Order = 4)]
    private string memo;
    
        /**
       * @return 订单备注
    */
        public string getMemo() {
               	return memo;
            }
    
    /**
     * 设置订单备注     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMemo(string memo) {
     	         	    this.memo = memo;
     	        }
    
        [DataMember(Order = 5)]
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
    
        [DataMember(Order = 6)]
    private string orderStatus;
    
        /**
       * @return 订单状态
    */
        public string getOrderStatus() {
               	return orderStatus;
            }
    
    /**
     * 设置订单状态     *
     * 参数示例：<pre>PLACE_ORDER_SUCCESS</pre>     
             * 此参数必填
          */
    public void setOrderStatus(string orderStatus) {
     	         	    this.orderStatus = orderStatus;
     	        }
    
        [DataMember(Order = 7)]
    private long? timeoutLeftTime;
    
        /**
       * @return 超时剩余时间
    */
        public long? getTimeoutLeftTime() {
               	return timeoutLeftTime;
            }
    
    /**
     * 设置超时剩余时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setTimeoutLeftTime(long timeoutLeftTime) {
     	         	    this.timeoutLeftTime = timeoutLeftTime;
     	        }
    
        [DataMember(Order = 8)]
    private SimpleOrderProductVO[] productList;
    
        /**
       * @return 
    */
        public SimpleOrderProductVO[] getProductList() {
               	return productList;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductList(SimpleOrderProductVO[] productList) {
     	         	    this.productList = productList;
     	        }
    
    
  }
}