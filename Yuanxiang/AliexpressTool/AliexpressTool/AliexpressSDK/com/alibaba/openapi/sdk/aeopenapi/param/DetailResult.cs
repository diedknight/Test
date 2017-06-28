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
public class DetailResult {

       [DataMember(Order = 1)]
    private long? id;
    
        /**
       * @return 消息ID
    */
        public long? getId() {
               	return id;
            }
    
    /**
     * 设置消息ID     *
     * 参数示例：<pre>11333443434</pre>     
             * 此参数必填
          */
    public void setId(long id) {
     	         	    this.id = id;
     	        }
    
        [DataMember(Order = 2)]
    private long? gmtCreate;
    
        /**
       * @return 创建时间
    */
        public long? getGmtCreate() {
               	return gmtCreate;
            }
    
    /**
     * 设置创建时间     *
     * 参数示例：<pre>11333443434</pre>     
             * 此参数必填
          */
    public void setGmtCreate(long gmtCreate) {
     	         	    this.gmtCreate = gmtCreate;
     	        }
    
        [DataMember(Order = 3)]
    private string senderName;
    
        /**
       * @return 发送者名字
    */
        public string getSenderName() {
               	return senderName;
            }
    
    /**
     * 设置发送者名字     *
     * 参数示例：<pre>jack.li</pre>     
             * 此参数必填
          */
    public void setSenderName(string senderName) {
     	         	    this.senderName = senderName;
     	        }
    
        [DataMember(Order = 4)]
    private string messageType;
    
        /**
       * @return 消息类别(product/order/member/store)不同的消息类别，typeId为相应的值，如messageType为product,typeId为productId,对应summary中有相应的附属性信，如果为product,则有产品相关的信息
    */
        public string getMessageType() {
               	return messageType;
            }
    
    /**
     * 设置消息类别(product/order/member/store)不同的消息类别，typeId为相应的值，如messageType为product,typeId为productId,对应summary中有相应的附属性信，如果为product,则有产品相关的信息     *
     * 参数示例：<pre>product</pre>     
             * 此参数必填
          */
    public void setMessageType(string messageType) {
     	         	    this.messageType = messageType;
     	        }
    
        [DataMember(Order = 5)]
    private string content;
    
        /**
       * @return 消息详情
    */
        public string getContent() {
               	return content;
            }
    
    /**
     * 设置消息详情     *
     * 参数示例：<pre>hello</pre>     
             * 此参数必填
          */
    public void setContent(string content) {
     	         	    this.content = content;
     	        }
    
        [DataMember(Order = 6)]
    private long? typeId;
    
        /**
       * @return 相关类型ID
    */
        public long? getTypeId() {
               	return typeId;
            }
    
    /**
     * 设置相关类型ID     *
     * 参数示例：<pre>345555</pre>     
             * 此参数必填
          */
    public void setTypeId(long typeId) {
     	         	    this.typeId = typeId;
     	        }
    
        [DataMember(Order = 7)]
    private FilePath[] filePath;
    
        /**
       * @return 图片地址
    */
        public FilePath[] getFilePath() {
               	return filePath;
            }
    
    /**
     * 设置图片地址     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFilePath(FilePath[] filePath) {
     	         	    this.filePath = filePath;
     	        }
    
        [DataMember(Order = 8)]
    private Summary summary;
    
        /**
       * @return 附属信息
    */
        public Summary getSummary() {
               	return summary;
            }
    
    /**
     * 设置附属信息     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSummary(Summary summary) {
     	         	    this.summary = summary;
     	        }
    
    
  }
}