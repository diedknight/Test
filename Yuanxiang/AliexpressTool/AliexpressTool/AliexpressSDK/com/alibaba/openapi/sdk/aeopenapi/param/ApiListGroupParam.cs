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
public class ApiListGroupParam {

       [DataMember(Order = 1)]
    private string groupId;
    
        /**
       * @return 图片组ID。不填groupId则返回所有图片组信息
    */
        public string getGroupId() {
               	return groupId;
            }
    
    /**
     * 设置图片组ID。不填groupId则返回所有图片组信息     *
     * 参数示例：<pre>8401</pre>     
             * 此参数必填
          */
    public void setGroupId(string groupId) {
     	         	    this.groupId = groupId;
     	        }
    
    
  }
}