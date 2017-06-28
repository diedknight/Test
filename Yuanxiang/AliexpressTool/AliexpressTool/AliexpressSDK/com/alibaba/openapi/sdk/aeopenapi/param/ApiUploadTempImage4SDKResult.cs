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
public class ApiUploadTempImage4SDKResult {

       [DataMember(Order = 1)]
    private int? width;
    
        /**
       * @return 图片的宽度。单位：像素
    */
        public int? getWidth() {
               	return width;
            }
    
    /**
     * 设置图片的宽度。单位：像素     *
          
             * 此参数必填
          */
    public void setWidth(int width) {
     	         	    this.width = width;
     	        }
    
        [DataMember(Order = 2)]
    private int? height;
    
        /**
       * @return 图片的高度。单位：像素
    */
        public int? getHeight() {
               	return height;
            }
    
    /**
     * 设置图片的高度。单位：像素     *
          
             * 此参数必填
          */
    public void setHeight(int height) {
     	         	    this.height = height;
     	        }
    
        [DataMember(Order = 3)]
    private string url;
    
        /**
       * @return 图片的URL。
    */
        public string getUrl() {
               	return url;
            }
    
    /**
     * 设置图片的URL。     *
          
             * 此参数必填
          */
    public void setUrl(string url) {
     	         	    this.url = url;
     	        }
    
        [DataMember(Order = 4)]
    private bool? success;
    
        /**
       * @return 本次操作的结果。
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置本次操作的结果。     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
        [DataMember(Order = 5)]
    private string srcFileName;
    
        /**
       * @return 图片的文件名。
    */
        public string getSrcFileName() {
               	return srcFileName;
            }
    
    /**
     * 设置图片的文件名。     *
          
             * 此参数必填
          */
    public void setSrcFileName(string srcFileName) {
     	         	    this.srcFileName = srcFileName;
     	        }
    
    
  }
}