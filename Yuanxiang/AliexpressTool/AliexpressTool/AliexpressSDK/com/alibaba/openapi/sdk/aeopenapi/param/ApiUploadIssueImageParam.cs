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
public class ApiUploadIssueImageParam {

       [DataMember(Order = 1)]
    private string extension;
    
        /**
       * @return 图片扩展名
    */
        public string getExtension() {
               	return extension;
            }
    
    /**
     * 设置图片扩展名     *
     * 参数示例：<pre>32141243325234313.jpg</pre>     
             * 此参数必填
          */
    public void setExtension(string extension) {
     	         	    this.extension = extension;
     	        }
    
    
  }
}