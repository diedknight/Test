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
public class ApiUpdateMsgRankParam {

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
     * 参数示例：<pre>1123445</pre>     
             * 此参数必填
          */
    public void setChannelId(string channelId) {
     	         	    this.channelId = channelId;
     	        }
    
        [DataMember(Order = 2)]
    private string rank;
    
        /**
       * @return 标签(rank0,rank1,rank2,rank3,rank4,rank5)rank0~rank5为六种不同颜色标记依次为白，红，橙，绿，蓝，紫
    */
        public string getRank() {
               	return rank;
            }
    
    /**
     * 设置标签(rank0,rank1,rank2,rank3,rank4,rank5)rank0~rank5为六种不同颜色标记依次为白，红，橙，绿，蓝，紫     *
     * 参数示例：<pre>rank1</pre>     
             * 此参数必填
          */
    public void setRank(string rank) {
     	         	    this.rank = rank;
     	        }
    
    
  }
}