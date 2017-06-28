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
public class ApiUploadImageResult {

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
    private string fileName;
    
        /**
       * @return 图片的名称。
    */
        public string getFileName() {
               	return fileName;
            }
    
    /**
     * 设置图片的名称。     *
          
             * 此参数必填
          */
    public void setFileName(string fileName) {
     	         	    this.fileName = fileName;
     	        }
    
        [DataMember(Order = 4)]
    private string photobankTotalSize;
    
        /**
       * @return 图片银行总的空间大小。单位：MB
    */
        public string getPhotobankTotalSize() {
               	return photobankTotalSize;
            }
    
    /**
     * 设置图片银行总的空间大小。单位：MB     *
          
             * 此参数必填
          */
    public void setPhotobankTotalSize(string photobankTotalSize) {
     	         	    this.photobankTotalSize = photobankTotalSize;
     	        }
    
        [DataMember(Order = 5)]
    private string photobankUsedSize;
    
        /**
       * @return 已经使用了的图片银行空间。单位：MB
    */
        public string getPhotobankUsedSize() {
               	return photobankUsedSize;
            }
    
    /**
     * 设置已经使用了的图片银行空间。单位：MB     *
          
             * 此参数必填
          */
    public void setPhotobankUsedSize(string photobankUsedSize) {
     	         	    this.photobankUsedSize = photobankUsedSize;
     	        }
    
        [DataMember(Order = 6)]
    private string photobankUrl;
    
        /**
       * @return 这张图片的URL。
    */
        public string getPhotobankUrl() {
               	return photobankUrl;
            }
    
    /**
     * 设置这张图片的URL。     *
          
             * 此参数必填
          */
    public void setPhotobankUrl(string photobankUrl) {
     	         	    this.photobankUrl = photobankUrl;
     	        }
    
        [DataMember(Order = 7)]
    private string status;
    
        /**
       * @return 图片上传的结果。
    */
        public string getStatus() {
               	return status;
            }
    
    /**
     * 设置图片上传的结果。     *
          
             * 此参数必填
          */
    public void setStatus(string status) {
     	         	    this.status = status;
     	        }
    
        [DataMember(Order = 8)]
    private bool? success;
    
        /**
       * @return 接口调用的结果。
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置接口调用的结果。     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}