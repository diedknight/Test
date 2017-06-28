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
public class OrderLoanItemVO {

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
    private string orderStatus;
    
        /**
       * @return 订单状态
    */
        public string getOrderStatus() {
               	return orderStatus;
            }
    
    /**
     * 设置订单状态     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderStatus(string orderStatus) {
     	         	    this.orderStatus = orderStatus;
     	        }
    
        [DataMember(Order = 3)]
    private SonOrderLoanVO[] sonOrderList;
    
        /**
       * @return 子订单元素列表
    */
        public SonOrderLoanVO[] getSonOrderList() {
               	return sonOrderList;
            }
    
    /**
     * 设置子订单元素列表     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSonOrderList(SonOrderLoanVO[] sonOrderList) {
     	         	    this.sonOrderList = sonOrderList;
     	        }
    
        [DataMember(Order = 4)]
    private TradeMoney amountTotal;
    
        /**
       * @return 总金额
    */
        public TradeMoney getAmountTotal() {
               	return amountTotal;
            }
    
    /**
     * 设置总金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAmountTotal(TradeMoney amountTotal) {
     	         	    this.amountTotal = amountTotal;
     	        }
    
    
  }
}