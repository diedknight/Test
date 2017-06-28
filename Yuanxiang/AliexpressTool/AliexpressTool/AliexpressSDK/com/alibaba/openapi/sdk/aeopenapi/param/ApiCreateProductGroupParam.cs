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
public class ApiCreateProductGroupParam {

       [DataMember(Order = 1)]
    private string name;
    
        /**
       * @return 分组的名称, 请控制在1～50个英文字符之内。
    */
        public string getName() {
               	return name;
            }
    
    /**
     * 设置分组的名称, 请控制在1～50个英文字符之内。     *
     * 参数示例：<pre>foo</pre>     
             * 此参数必填
          */
    public void setName(string name) {
     	         	    this.name = name;
     	        }
    
        [DataMember(Order = 2)]
    private long? parentId;
    
        /**
       * @return 父分组的ID。如果为0则表示创建根分组，否则创建指定分组下的二级分组。
    */
        public long? getParentId() {
               	return parentId;
            }
    
    /**
     * 设置父分组的ID。如果为0则表示创建根分组，否则创建指定分组下的二级分组。     *
     * 参数示例：<pre>0或者100</pre>     
             * 此参数必填
          */
    public void setParentId(long parentId) {
     	         	    this.parentId = parentId;
     	        }
    
    
  }
}