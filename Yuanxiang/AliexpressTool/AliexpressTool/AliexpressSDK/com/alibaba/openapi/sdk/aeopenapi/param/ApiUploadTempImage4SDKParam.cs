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
public class ApiUploadTempImage4SDKParam {

       [DataMember(Order = 1)]
    private string srcFileName;
    
        /**
       * @return 图片原名
    */
        public string getSrcFileName() {
               	return srcFileName;
            }
    
    /**
     * 设置图片原名     *
     * 参数示例：<pre>1.jpg</pre>     
             * 此参数必填
          */
    public void setSrcFileName(string srcFileName) {
     	         	    this.srcFileName = srcFileName;
     	        }
    
        [DataMember(Order = 2)]
    private byte[] fileData;
    
        /**
       * @return 字符串形式的图片文件二进制数据流
    */
        public byte[] getFileData() {
               	return fileData;
            }
    
    /**
     * 设置字符串形式的图片文件二进制数据流     *
     * 参数示例：<pre>aff3fadfafd3fdd00123</pre>     
             * 此参数必填
          */
    public void setFileData(byte[] fileData) {
     	         	    this.fileData = fileData;
     	        }
    
    
  }
}