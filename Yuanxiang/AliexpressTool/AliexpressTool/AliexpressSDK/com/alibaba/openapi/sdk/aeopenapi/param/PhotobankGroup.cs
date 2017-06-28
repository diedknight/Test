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
public class PhotobankGroup {

       [DataMember(Order = 1)]
    private string groupId;
    
        /**
       * @return 图片分组ID
    */
        public string getGroupId() {
               	return groupId;
            }
    
    /**
     * 设置图片分组ID     *
     * 参数示例：<pre>2000123</pre>     
             * 此参数必填
          */
    public void setGroupId(string groupId) {
     	         	    this.groupId = groupId;
     	        }
    
        [DataMember(Order = 2)]
    private string groupName;
    
        /**
       * @return 图片分组名称
    */
        public string getGroupName() {
               	return groupName;
            }
    
    /**
     * 设置图片分组名称     *
     * 参数示例：<pre>Test Group Name</pre>     
             * 此参数必填
          */
    public void setGroupName(string groupName) {
     	         	    this.groupName = groupName;
     	        }
    
    
  }
}