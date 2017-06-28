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
public class AeopChildProductGroup {

       [DataMember(Order = 1)]
    private long? groupId;
    
        /**
       * @return 
    */
        public long? getGroupId() {
               	return groupId;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGroupId(long groupId) {
     	         	    this.groupId = groupId;
     	        }
    
        [DataMember(Order = 2)]
    private string groupName;
    
        /**
       * @return 
    */
        public string getGroupName() {
               	return groupName;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGroupName(string groupName) {
     	         	    this.groupName = groupName;
     	        }
    
    
  }
}