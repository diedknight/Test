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
public class RelationResult {

       [DataMember(Order = 1)]
    private long? unreadCount;
    
        /**
       * @return 未读数
    */
        public long? getUnreadCount() {
               	return unreadCount;
            }
    
    /**
     * 设置未读数     *
     * 参数示例：<pre>2</pre>     
             * 此参数必填
          */
    public void setUnreadCount(long unreadCount) {
     	         	    this.unreadCount = unreadCount;
     	        }
    
        [DataMember(Order = 2)]
    private string channelId;
    
        /**
       * @return 通道ID，即关系ID(订单留言为订单号，站内信为站内信的关系ID)
    */
        public string getChannelId() {
               	return channelId;
            }
    
    /**
     * 设置通道ID，即关系ID(订单留言为订单号，站内信为站内信的关系ID)     *
     * 参数示例：<pre>2234455</pre>     
             * 此参数必填
          */
    public void setChannelId(string channelId) {
     	         	    this.channelId = channelId;
     	        }
    
        [DataMember(Order = 3)]
    private long? lastMessageId;
    
        /**
       * @return 最后一条消息ID
    */
        public long? getLastMessageId() {
               	return lastMessageId;
            }
    
    /**
     * 设置最后一条消息ID     *
     * 参数示例：<pre>2455566</pre>     
             * 此参数必填
          */
    public void setLastMessageId(long lastMessageId) {
     	         	    this.lastMessageId = lastMessageId;
     	        }
    
        [DataMember(Order = 4)]
    private string readStat;
    
        /**
       * @return 未读状态(0未读1已读)
    */
        public string getReadStat() {
               	return readStat;
            }
    
    /**
     * 设置未读状态(0未读1已读)     *
     * 参数示例：<pre>0</pre>     
             * 此参数必填
          */
    public void setReadStat(string readStat) {
     	         	    this.readStat = readStat;
     	        }
    
        [DataMember(Order = 5)]
    private string lastMessageContent;
    
        /**
       * @return 最后一条消息内容
    */
        public string getLastMessageContent() {
               	return lastMessageContent;
            }
    
    /**
     * 设置最后一条消息内容     *
     * 参数示例：<pre>hello</pre>     
             * 此参数必填
          */
    public void setLastMessageContent(string lastMessageContent) {
     	         	    this.lastMessageContent = lastMessageContent;
     	        }
    
        [DataMember(Order = 6)]
    private bool? lastMessageIsOwn;
    
        /**
       * @return 最后一条消息是否自己这边发送(true是，false否)
    */
        public bool? getLastMessageIsOwn() {
               	return lastMessageIsOwn;
            }
    
    /**
     * 设置最后一条消息是否自己这边发送(true是，false否)     *
     * 参数示例：<pre>true</pre>     
             * 此参数必填
          */
    public void setLastMessageIsOwn(bool lastMessageIsOwn) {
     	         	    this.lastMessageIsOwn = lastMessageIsOwn;
     	        }
    
        [DataMember(Order = 7)]
    private string childName;
    
        /**
       * @return 消息所属账号(主账号查询默认包含子账号的信息，如果属于子账号，这里有子账号的名字)
    */
        public string getChildName() {
               	return childName;
            }
    
    /**
     * 设置消息所属账号(主账号查询默认包含子账号的信息，如果属于子账号，这里有子账号的名字)     *
     * 参数示例：<pre>jack.liu</pre>     
             * 此参数必填
          */
    public void setChildName(string childName) {
     	         	    this.childName = childName;
     	        }
    
        [DataMember(Order = 8)]
    private long? messageTime;
    
        /**
       * @return 最后一条消息时间
    */
        public long? getMessageTime() {
               	return messageTime;
            }
    
    /**
     * 设置最后一条消息时间     *
     * 参数示例：<pre>33443344455</pre>     
             * 此参数必填
          */
    public void setMessageTime(long messageTime) {
     	         	    this.messageTime = messageTime;
     	        }
    
        [DataMember(Order = 9)]
    private long? childId;
    
        /**
       * @return 消息所属账号(主账号查询默认包含子账号的信息，如果属于子账号，这里有子账号的ID)
    */
        public long? getChildId() {
               	return childId;
            }
    
    /**
     * 设置消息所属账号(主账号查询默认包含子账号的信息，如果属于子账号，这里有子账号的ID)     *
     * 参数示例：<pre>6645774</pre>     
             * 此参数必填
          */
    public void setChildId(long childId) {
     	         	    this.childId = childId;
     	        }
    
        [DataMember(Order = 10)]
    private string otherName;
    
        /**
       * @return 与当前卖家或子账号建立关系的买家名字
    */
        public string getOtherName() {
               	return otherName;
            }
    
    /**
     * 设置与当前卖家或子账号建立关系的买家名字     *
     * 参数示例：<pre>jack.ma</pre>     
             * 此参数必填
          */
    public void setOtherName(string otherName) {
     	         	    this.otherName = otherName;
     	        }
    
        [DataMember(Order = 11)]
    private string otherLoginId;
    
        /**
       * @return 与当前卖家或子账号建立关系的买家账号
    */
        public string getOtherLoginId() {
               	return otherLoginId;
            }
    
    /**
     * 设置与当前卖家或子账号建立关系的买家账号     *
     * 参数示例：<pre>us3333</pre>     
             * 此参数必填
          */
    public void setOtherLoginId(string otherLoginId) {
     	         	    this.otherLoginId = otherLoginId;
     	        }
    
        [DataMember(Order = 12)]
    private string dealStat;
    
        /**
       * @return 处理状态(0未处理,1已处理)
    */
        public string getDealStat() {
               	return dealStat;
            }
    
    /**
     * 设置处理状态(0未处理,1已处理)     *
     * 参数示例：<pre>0</pre>     
             * 此参数必填
          */
    public void setDealStat(string dealStat) {
     	         	    this.dealStat = dealStat;
     	        }
    
        [DataMember(Order = 13)]
    private string rank;
    
        /**
       * @return 标签值(0,1,2,3,4,5)依次表示为白，红，橙，绿，蓝，紫
    */
        public string getRank() {
               	return rank;
            }
    
    /**
     * 设置标签值(0,1,2,3,4,5)依次表示为白，红，橙，绿，蓝，紫     *
     * 参数示例：<pre>0</pre>     
             * 此参数必填
          */
    public void setRank(string rank) {
     	         	    this.rank = rank;
     	        }
    
    
  }
}