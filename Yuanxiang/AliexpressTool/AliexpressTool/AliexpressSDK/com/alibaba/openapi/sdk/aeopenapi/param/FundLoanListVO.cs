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
public class FundLoanListVO {

       [DataMember(Order = 1)]
    private int? totalItem;
    
        /**
       * @return 总条数
    */
        public int? getTotalItem() {
               	return totalItem;
            }
    
    /**
     * 设置总条数     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setTotalItem(int totalItem) {
     	         	    this.totalItem = totalItem;
     	        }
    
        [DataMember(Order = 2)]
    private OrderLoanItemVO[] orderList;
    
        /**
       * @return 订单放款列表
    */
        public OrderLoanItemVO[] getOrderList() {
               	return orderList;
            }
    
    /**
     * 设置订单放款列表     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderList(OrderLoanItemVO[] orderList) {
     	         	    this.orderList = orderList;
     	        }
    
    
  }
}