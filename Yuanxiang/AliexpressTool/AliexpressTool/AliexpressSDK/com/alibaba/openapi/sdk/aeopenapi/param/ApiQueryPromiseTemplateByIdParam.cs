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
public class ApiQueryPromiseTemplateByIdParam {

       [DataMember(Order = 1)]
    private long? templateId;
    
        /**
       * @return 输入服务模板编号。注：输入为-1时，获取所有服务模板列表。
    */
        public long? getTemplateId() {
               	return templateId;
            }
    
    /**
     * 设置输入服务模板编号。注：输入为-1时，获取所有服务模板列表。     *
     * 参数示例：<pre>-1</pre>     
             * 此参数必填
          */
    public void setTemplateId(long templateId) {
     	         	    this.templateId = templateId;
     	        }
    
    
  }
}