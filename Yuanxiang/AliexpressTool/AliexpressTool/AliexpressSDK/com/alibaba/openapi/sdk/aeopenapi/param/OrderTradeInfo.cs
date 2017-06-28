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
public class OrderTradeInfo {

       [DataMember(Order = 1)]
    private long? id;
    
        /**
       * @return 订单ID
    */
        public long? getId() {
               	return id;
            }
    
    /**
     * 设置订单ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setId(long id) {
     	         	    this.id = id;
     	        }
    
        [DataMember(Order = 2)]
    private string initOderAmount;
    
        /**
       * @return 订单初始金额
    */
        public string getInitOderAmount() {
               	return initOderAmount;
            }
    
    /**
     * 设置订单初始金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setInitOderAmount(string initOderAmount) {
     	         	    this.initOderAmount = initOderAmount;
     	        }
    
        [DataMember(Order = 3)]
    private string initOderAmountCur;
    
        /**
       * @return 订单金额货币单位
    */
        public string getInitOderAmountCur() {
               	return initOderAmountCur;
            }
    
    /**
     * 设置订单金额货币单位     *
     * 参数示例：<pre>USD</pre>     
             * 此参数必填
          */
    public void setInitOderAmountCur(string initOderAmountCur) {
     	         	    this.initOderAmountCur = initOderAmountCur;
     	        }
    
        [DataMember(Order = 4)]
    private bool? isPhone;
    
        /**
       * @return 是否手机订单
    */
        public bool? getIsPhone() {
               	return isPhone;
            }
    
    /**
     * 设置是否手机订单     *
     * 参数示例：<pre>false</pre>     
             * 此参数必填
          */
    public void setIsPhone(bool isPhone) {
     	         	    this.isPhone = isPhone;
     	        }
    
        [DataMember(Order = 5)]
    private string logisticsAmount;
    
        /**
       * @return 物流金额
    */
        public string getLogisticsAmount() {
               	return logisticsAmount;
            }
    
    /**
     * 设置物流金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLogisticsAmount(string logisticsAmount) {
     	         	    this.logisticsAmount = logisticsAmount;
     	        }
    
        [DataMember(Order = 6)]
    private string logisticsAmountCur;
    
        /**
       * @return 物流金额货币单位
    */
        public string getLogisticsAmountCur() {
               	return logisticsAmountCur;
            }
    
    /**
     * 设置物流金额货币单位     *
     * 参数示例：<pre>USD</pre>     
             * 此参数必填
          */
    public void setLogisticsAmountCur(string logisticsAmountCur) {
     	         	    this.logisticsAmountCur = logisticsAmountCur;
     	        }
    
        [DataMember(Order = 7)]
    private string orderAmount;
    
        /**
       * @return 订单金额
    */
        public string getOrderAmount() {
               	return orderAmount;
            }
    
    /**
     * 设置订单金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderAmount(string orderAmount) {
     	         	    this.orderAmount = orderAmount;
     	        }
    
        [DataMember(Order = 8)]
    private string orderAmountCur;
    
        /**
       * @return 货币单位
    */
        public string getOrderAmountCur() {
               	return orderAmountCur;
            }
    
    /**
     * 设置货币单位     *
     * 参数示例：<pre>USD</pre>     
             * 此参数必填
          */
    public void setOrderAmountCur(string orderAmountCur) {
     	         	    this.orderAmountCur = orderAmountCur;
     	        }
    
        [DataMember(Order = 9)]
    private ChildOrderDTO[] childOrderList;
    
        /**
       * @return 
    */
        public ChildOrderDTO[] getChildOrderList() {
               	return childOrderList;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setChildOrderList(ChildOrderDTO[] childOrderList) {
     	         	    this.childOrderList = childOrderList;
     	        }
    
    
  }
}