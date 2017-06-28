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
public class OrderListVO {

       [DataMember(Order = 1)]
    private int? totalItem;
    
        /**
       * @return 订单总数
    */
        public int? getTotalItem() {
               	return totalItem;
            }
    
    /**
     * 设置订单总数     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setTotalItem(int totalItem) {
     	         	    this.totalItem = totalItem;
     	        }
    
        [DataMember(Order = 2)]
    private OrderItemVO[] orderList;
    
        /**
       * @return 订单列表
    */
        public OrderItemVO[] getOrderList() {
               	return orderList;
            }
    
    /**
     * 设置订单列表     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderList(OrderItemVO[] orderList) {
     	         	    this.orderList = orderList;
     	        }
    
    
  }
}