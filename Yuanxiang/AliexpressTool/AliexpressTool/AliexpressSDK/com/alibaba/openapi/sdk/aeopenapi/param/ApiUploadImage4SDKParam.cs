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
public class ApiUploadImage4SDKParam {

       [DataMember(Order = 1)]
    private string fileName;
    
        /**
       * @return 上传文件名称，长度不要超过256个字符。
    */
        public string getFileName() {
               	return fileName;
            }
    
    /**
     * 设置上传文件名称，长度不要超过256个字符。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFileName(string fileName) {
     	         	    this.fileName = fileName;
     	        }
    
        [DataMember(Order = 2)]
    private byte[] imageBytes;
    
        /**
       * @return 图片文件的字节流
图片大小限制：3MB
    */
        public byte[] getImageBytes() {
               	return imageBytes;
            }
    
    /**
     * 设置图片文件的字节流
图片大小限制：3MB     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setImageBytes(byte[] imageBytes) {
     	         	    this.imageBytes = imageBytes;
     	        }
    
        [DataMember(Order = 3)]
    private string groupId;
    
        /**
       * @return 图片保存的图片组，groupId为空，则图片保存在Other组中。
    */
        public string getGroupId() {
               	return groupId;
            }
    
    /**
     * 设置图片保存的图片组，groupId为空，则图片保存在Other组中。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGroupId(string groupId) {
     	         	    this.groupId = groupId;
     	        }
    
    
  }
}