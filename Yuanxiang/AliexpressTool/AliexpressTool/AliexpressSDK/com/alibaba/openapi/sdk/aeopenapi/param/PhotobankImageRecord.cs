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
public class PhotobankImageRecord {

       [DataMember(Order = 1)]
    private long? iid;
    
        /**
       * @return 这张图片在图片银行中的ID。
    */
        public long? getIid() {
               	return iid;
            }
    
    /**
     * 设置这张图片在图片银行中的ID。     *
     * 参数示例：<pre>3207108922</pre>     
             * 此参数必填
          */
    public void setIid(long iid) {
     	         	    this.iid = iid;
     	        }
    
        [DataMember(Order = 2)]
    private int? width;
    
        /**
       * @return 这张图片的宽度。单位：像素。
    */
        public int? getWidth() {
               	return width;
            }
    
    /**
     * 设置这张图片的宽度。单位：像素。     *
     * 参数示例：<pre>588</pre>     
             * 此参数必填
          */
    public void setWidth(int width) {
     	         	    this.width = width;
     	        }
    
        [DataMember(Order = 3)]
    private int? height;
    
        /**
       * @return 这张图片的高度。单位：像素。
    */
        public int? getHeight() {
               	return height;
            }
    
    /**
     * 设置这张图片的高度。单位：像素。     *
     * 参数示例：<pre>421</pre>     
             * 此参数必填
          */
    public void setHeight(int height) {
     	         	    this.height = height;
     	        }
    
        [DataMember(Order = 4)]
    private int? fileSize;
    
        /**
       * @return 这张图片的大小。单位：字节(B)。
    */
        public int? getFileSize() {
               	return fileSize;
            }
    
    /**
     * 设置这张图片的大小。单位：字节(B)。     *
     * 参数示例：<pre>35151</pre>     
             * 此参数必填
          */
    public void setFileSize(int fileSize) {
     	         	    this.fileSize = fileSize;
     	        }
    
        [DataMember(Order = 5)]
    private int? referenceCount;
    
        /**
       * @return 这张图片被引用的次数。
    */
        public int? getReferenceCount() {
               	return referenceCount;
            }
    
    /**
     * 设置这张图片被引用的次数。     *
     * 参数示例：<pre>0</pre>     
             * 此参数必填
          */
    public void setReferenceCount(int referenceCount) {
     	         	    this.referenceCount = referenceCount;
     	        }
    
        [DataMember(Order = 6)]
    private string displayName;
    
        /**
       * @return 这张图片在图片银行中名称。可以根据这个值在图片银行中搜索到对应的图片。
    */
        public string getDisplayName() {
               	return displayName;
            }
    
    /**
     * 设置这张图片在图片银行中名称。可以根据这个值在图片银行中搜索到对应的图片。     *
     * 参数示例：<pre>10 E14 6W Warm White 30 SMD LED Spotlight Light Lamp Bulb</pre>     
             * 此参数必填
          */
    public void setDisplayName(string displayName) {
     	         	    this.displayName = displayName;
     	        }
    
        [DataMember(Order = 7)]
    private string url;
    
        /**
       * @return 这张图片的URL。可以将这个URL添加到产品的主图或者详描中。
    */
        public string getUrl() {
               	return url;
            }
    
    /**
     * 设置这张图片的URL。可以将这个URL添加到产品的主图或者详描中。     *
     * 参数示例：<pre>http://g03.a.alicdn.com/kf/HTB1PP5AGVXXXXaIXXXXq6xXFXXXN/200042360/HTB1PP5AGVXXXXaIXXXXq6xXFXXXN.jpg</pre>     
             * 此参数必填
          */
    public void setUrl(string url) {
     	         	    this.url = url;
     	        }
    
    
  }
}