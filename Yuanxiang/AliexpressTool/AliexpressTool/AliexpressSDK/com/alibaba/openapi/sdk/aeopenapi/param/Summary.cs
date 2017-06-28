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
public class Summary {

       [DataMember(Order = 1)]
    private string productName;
    
        /**
       * @return 产品名
    */
        public string getProductName() {
               	return productName;
            }
    
    /**
     * 设置产品名     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductName(string productName) {
     	         	    this.productName = productName;
     	        }
    
        [DataMember(Order = 2)]
    private string productImageUrl;
    
        /**
       * @return 产品图片链接
    */
        public string getProductImageUrl() {
               	return productImageUrl;
            }
    
    /**
     * 设置产品图片链接     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductImageUrl(string productImageUrl) {
     	         	    this.productImageUrl = productImageUrl;
     	        }
    
        [DataMember(Order = 3)]
    private string productDetailUrl;
    
        /**
       * @return 产品链接
    */
        public string getProductDetailUrl() {
               	return productDetailUrl;
            }
    
    /**
     * 设置产品链接     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductDetailUrl(string productDetailUrl) {
     	         	    this.productDetailUrl = productDetailUrl;
     	        }
    
        [DataMember(Order = 4)]
    private string orderUrl;
    
        /**
       * @return 订单链接
    */
        public string getOrderUrl() {
               	return orderUrl;
            }
    
    /**
     * 设置订单链接     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderUrl(string orderUrl) {
     	         	    this.orderUrl = orderUrl;
     	        }
    
        [DataMember(Order = 5)]
    private string senderName;
    
        /**
       * @return 消息发送者名字
    */
        public string getSenderName() {
               	return senderName;
            }
    
    /**
     * 设置消息发送者名字     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSenderName(string senderName) {
     	         	    this.senderName = senderName;
     	        }
    
        [DataMember(Order = 6)]
    private string receiverName;
    
        /**
       * @return 消息接收者名字
    */
        public string getReceiverName() {
               	return receiverName;
            }
    
    /**
     * 设置消息接收者名字     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setReceiverName(string receiverName) {
     	         	    this.receiverName = receiverName;
     	        }
    
        [DataMember(Order = 7)]
    private string senderLoginId;
    
        /**
       * @return 消息发送者账号
    */
        public string getSenderLoginId() {
               	return senderLoginId;
            }
    
    /**
     * 设置消息发送者账号     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSenderLoginId(string senderLoginId) {
     	         	    this.senderLoginId = senderLoginId;
     	        }
    
    
  }
}