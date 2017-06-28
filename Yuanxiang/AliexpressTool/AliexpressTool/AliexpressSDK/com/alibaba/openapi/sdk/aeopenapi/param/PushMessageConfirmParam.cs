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
public class PushMessageConfirmParam {

       [DataMember(Order = 1)]
    private int[] msgIdList;
    
        /**
       * @return 待确认的消息id列表
    */
        public int[] getMsgIdList() {
               	return msgIdList;
            }
    
    /**
     * 设置待确认的消息id列表     *
     * 参数示例：<pre>[123,456]</pre>     
             * 此参数必填
          */
    public void setMsgIdList(int[] msgIdList) {
     	         	    this.msgIdList = msgIdList;
     	        }
    
    
  }
}