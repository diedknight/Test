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
public class AeopProductGroup {

       [DataMember(Order = 1)]
    private long? groupId;
    
        /**
       * @return 产品分组
    */
        public long? getGroupId() {
               	return groupId;
            }
    
    /**
     * 设置产品分组     *
     * 参数示例：<pre>262007001</pre>     
             * 此参数必填
          */
    public void setGroupId(long groupId) {
     	         	    this.groupId = groupId;
     	        }
    
        [DataMember(Order = 2)]
    private string groupName;
    
        /**
       * @return 分组名称
    */
        public string getGroupName() {
               	return groupName;
            }
    
    /**
     * 设置分组名称     *
     * 参数示例：<pre>test112fasdfds</pre>     
             * 此参数必填
          */
    public void setGroupName(string groupName) {
     	         	    this.groupName = groupName;
     	        }
    
        [DataMember(Order = 3)]
    private AeopChildProductGroup[] childGroup;
    
        /**
       * @return 子分组列表
    */
        public AeopChildProductGroup[] getChildGroup() {
               	return childGroup;
            }
    
    /**
     * 设置子分组列表     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setChildGroup(AeopChildProductGroup[] childGroup) {
     	         	    this.childGroup = childGroup;
     	        }
    
    
  }
}