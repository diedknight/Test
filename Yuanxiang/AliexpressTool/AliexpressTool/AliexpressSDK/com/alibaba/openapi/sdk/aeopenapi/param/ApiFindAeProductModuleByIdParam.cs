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
public class ApiFindAeProductModuleByIdParam {

       [DataMember(Order = 1)]
    private long? moduleId;
    
        /**
       * @return moduleId 对应商品详情中的kse标签中的id属性;如: id="1004"
    */
        public long? getModuleId() {
               	return moduleId;
            }
    
    /**
     * 设置moduleId 对应商品详情中的kse标签中的id属性;如: id="1004"     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setModuleId(long moduleId) {
     	         	    this.moduleId = moduleId;
     	        }
    
    
  }
}