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
public class ApiUpdateMsgProcessedParam {

       [DataMember(Order = 1)]
    private string channelId;
    
        /**
       * @return 通道ID(即关系ID)
    */
        public string getChannelId() {
               	return channelId;
            }
    
    /**
     * 设置通道ID(即关系ID)     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setChannelId(string channelId) {
     	         	    this.channelId = channelId;
     	        }
    
        [DataMember(Order = 2)]
    private string dealStat;
    
        /**
       * @return 处理状态(0未处理,1已处理)
    */
        public string getDealStat() {
               	return dealStat;
            }
    
    /**
     * 设置处理状态(0未处理,1已处理)     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setDealStat(string dealStat) {
     	         	    this.dealStat = dealStat;
     	        }
    
    
  }
}