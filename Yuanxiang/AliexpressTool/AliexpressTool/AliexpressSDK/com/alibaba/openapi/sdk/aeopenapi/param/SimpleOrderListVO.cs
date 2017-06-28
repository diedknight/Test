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
public class SimpleOrderListVO {

       [DataMember(Order = 1)]
    private int? totalItem;
    
        /**
       * @return 总数量(SC订单不包含在结果中）
    */
        public int? getTotalItem() {
               	return totalItem;
            }
    
    /**
     * 设置总数量(SC订单不包含在结果中）     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setTotalItem(int totalItem) {
     	         	    this.totalItem = totalItem;
     	        }
    
        [DataMember(Order = 2)]
    private SimpleOrderItemVO[] orderList;
    
        /**
       * @return 订单数组
    */
        public SimpleOrderItemVO[] getOrderList() {
               	return orderList;
            }
    
    /**
     * 设置订单数组     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderList(SimpleOrderItemVO[] orderList) {
     	         	    this.orderList = orderList;
     	        }
    
    
  }
}