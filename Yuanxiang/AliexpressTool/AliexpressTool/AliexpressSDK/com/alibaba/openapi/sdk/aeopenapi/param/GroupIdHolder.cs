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
public class GroupIdHolder {

       [DataMember(Order = 1)]
    private long? groupId;
    
        /**
       * @return 产品分组ID
    */
        public long? getGroupId() {
               	return groupId;
            }
    
    /**
     * 设置产品分组ID     *
     * 参数示例：<pre>1234</pre>     
             * 此参数必填
          */
    public void setGroupId(long groupId) {
     	         	    this.groupId = groupId;
     	        }
    
    
  }
}