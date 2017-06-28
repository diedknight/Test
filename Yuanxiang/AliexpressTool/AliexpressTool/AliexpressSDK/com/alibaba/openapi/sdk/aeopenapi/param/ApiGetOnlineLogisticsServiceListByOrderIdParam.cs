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
public class ApiGetOnlineLogisticsServiceListByOrderIdParam {

       [DataMember(Order = 1)]
    private long? orderId;
    
        /**
       * @return 交易订单ID
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置交易订单ID     *
     * 参数示例：<pre>30003660495804</pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 2)]
    private double? goodsWeight;
    
        /**
       * @return 包裹重量
    */
        public double? getGoodsWeight() {
               	return goodsWeight;
            }
    
    /**
     * 设置包裹重量     *
     * 参数示例：<pre>1.50</pre>     
             * 此参数必填
          */
    public void setGoodsWeight(double goodsWeight) {
     	         	    this.goodsWeight = goodsWeight;
     	        }
    
        [DataMember(Order = 3)]
    private int? goodsLength;
    
        /**
       * @return 包裹长
    */
        public int? getGoodsLength() {
               	return goodsLength;
            }
    
    /**
     * 设置包裹长     *
     * 参数示例：<pre>11</pre>     
             * 此参数必填
          */
    public void setGoodsLength(int goodsLength) {
     	         	    this.goodsLength = goodsLength;
     	        }
    
        [DataMember(Order = 4)]
    private int? goodsWidth;
    
        /**
       * @return 包裹宽
    */
        public int? getGoodsWidth() {
               	return goodsWidth;
            }
    
    /**
     * 设置包裹宽     *
     * 参数示例：<pre>20</pre>     
             * 此参数必填
          */
    public void setGoodsWidth(int goodsWidth) {
     	         	    this.goodsWidth = goodsWidth;
     	        }
    
        [DataMember(Order = 5)]
    private int? goodsHeight;
    
        /**
       * @return 包裹高
    */
        public int? getGoodsHeight() {
               	return goodsHeight;
            }
    
    /**
     * 设置包裹高     *
     * 参数示例：<pre>25</pre>     
             * 此参数必填
          */
    public void setGoodsHeight(int goodsHeight) {
     	         	    this.goodsHeight = goodsHeight;
     	        }
    
    
  }
}