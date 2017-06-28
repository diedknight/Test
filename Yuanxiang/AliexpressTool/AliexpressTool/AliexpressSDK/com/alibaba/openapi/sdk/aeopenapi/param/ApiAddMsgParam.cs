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
public class ApiAddMsgParam {

       [DataMember(Order = 1)]
    private string channelId;
    
        /**
       * @return 通道ID，即关系ID
    */
        public string getChannelId() {
               	return channelId;
            }
    
    /**
     * 设置通道ID，即关系ID     *
     * 参数示例：<pre>334455556</pre>     
             * 此参数必填
          */
    public void setChannelId(string channelId) {
     	         	    this.channelId = channelId;
     	        }
    
        [DataMember(Order = 2)]
    private string buyerId;
    
        /**
       * @return 买家账号
    */
        public string getBuyerId() {
               	return buyerId;
            }
    
    /**
     * 设置买家账号     *
     * 参数示例：<pre>uk33445</pre>     
             * 此参数必填
          */
    public void setBuyerId(string buyerId) {
     	         	    this.buyerId = buyerId;
     	        }
    
        [DataMember(Order = 3)]
    private string content;
    
        /**
       * @return 内容
    */
        public string getContent() {
               	return content;
            }
    
    /**
     * 设置内容     *
     * 参数示例：<pre>hello</pre>     
             * 此参数必填
          */
    public void setContent(string content) {
     	         	    this.content = content;
     	        }
    
        [DataMember(Order = 4)]
    private string msgSources;
    
        /**
       * @return 类型(message_center/order_msg)
    */
        public string getMsgSources() {
               	return msgSources;
            }
    
    /**
     * 设置类型(message_center/order_msg)     *
     * 参数示例：<pre>message_center</pre>     
             * 此参数必填
          */
    public void setMsgSources(string msgSources) {
     	         	    this.msgSources = msgSources;
     	        }
    
        [DataMember(Order = 5)]
    private string imgPath;
    
        /**
       * @return 图片地址
    */
        public string getImgPath() {
               	return imgPath;
            }
    
    /**
     * 设置图片地址     *
     * 参数示例：<pre>http://g02.a.alicdn.com/kf/HTB1U07VIVXXXXaiaXXXq6xXFXXXu.jpg</pre>     
             * 此参数必填
          */
    public void setImgPath(string imgPath) {
     	         	    this.imgPath = imgPath;
     	        }
    
    
  }
}