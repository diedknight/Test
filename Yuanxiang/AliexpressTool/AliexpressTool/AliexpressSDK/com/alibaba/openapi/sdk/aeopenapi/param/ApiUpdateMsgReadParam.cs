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
public class ApiUpdateMsgReadParam {

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
     * 参数示例：<pre>2344555</pre>     
             * 此参数必填
          */
    public void setChannelId(string channelId) {
     	         	    this.channelId = channelId;
     	        }
    
        [DataMember(Order = 2)]
    private string msgSources;
    
        /**
       * @return 查询类型
    */
        public string getMsgSources() {
               	return msgSources;
            }
    
    /**
     * 设置查询类型     *
     * 参数示例：<pre>message_center/order_msg</pre>     
             * 此参数必填
          */
    public void setMsgSources(string msgSources) {
     	         	    this.msgSources = msgSources;
     	        }
    
    
  }
}