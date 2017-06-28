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
public class AlibabaproductAeopAEVideo {

       [DataMember(Order = 1)]
    private long? aliMemberId;
    
        /**
       * @return 卖家主账户ID
    */
        public long? getAliMemberId() {
               	return aliMemberId;
            }
    
    /**
     * 设置卖家主账户ID     *
     * 参数示例：<pre>1006680305</pre>     
             * 此参数必填
          */
    public void setAliMemberId(long aliMemberId) {
     	         	    this.aliMemberId = aliMemberId;
     	        }
    
        [DataMember(Order = 2)]
    private long? mediaId;
    
        /**
       * @return 视频ID
    */
        public long? getMediaId() {
               	return mediaId;
            }
    
    /**
     * 设置视频ID     *
     * 参数示例：<pre>12345678</pre>     
             * 此参数必填
          */
    public void setMediaId(long mediaId) {
     	         	    this.mediaId = mediaId;
     	        }
    
        [DataMember(Order = 3)]
    private string mediaType;
    
        /**
       * @return 视频的类型
    */
        public string getMediaType() {
               	return mediaType;
            }
    
    /**
     * 设置视频的类型     *
     * 参数示例：<pre>video</pre>     
             * 此参数必填
          */
    public void setMediaType(string mediaType) {
     	         	    this.mediaType = mediaType;
     	        }
    
        [DataMember(Order = 4)]
    private string mediaStatus;
    
        /**
       * @return 视频的状态
    */
        public string getMediaStatus() {
               	return mediaStatus;
            }
    
    /**
     * 设置视频的状态     *
     * 参数示例：<pre>approved</pre>     
             * 此参数必填
          */
    public void setMediaStatus(string mediaStatus) {
     	         	    this.mediaStatus = mediaStatus;
     	        }
    
        [DataMember(Order = 5)]
    private string posterUrl;
    
        /**
       * @return 视频封面图片的URL
    */
        public string getPosterUrl() {
               	return posterUrl;
            }
    
    /**
     * 设置视频封面图片的URL     *
     * 参数示例：<pre>http://img01.taobaocdn.com/bao/uploaded/TB1rNdGIVXXXXbTXFXXXXXXXXXX.jpg</pre>     
             * 此参数必填
          */
    public void setPosterUrl(string posterUrl) {
     	         	    this.posterUrl = posterUrl;
     	        }
    
    
  }
}