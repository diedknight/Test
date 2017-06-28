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
public class AlibabaproductAeopAEMultimedia {

       [DataMember(Order = 1)]
    private AlibabaproductAeopAEVideo[] aeopAEVideos;
    
        /**
       * @return 多媒体信息。
    */
        public AlibabaproductAeopAEVideo[] getAeopAEVideos() {
               	return aeopAEVideos;
            }
    
    /**
     * 设置多媒体信息。     *
     * 参数示例：<pre>[
	{
		"aliMemberId": 117284237,
		"mediaId": 35683461,
		"mediaType": "video",
		"mediaStatus": "approved",
		"posterUrl": "http://img02.taobaocdn.com/bao/uploaded/TB1a7HKLVXXXXX5XVXXXXXXXXXX.jpg"
	}
]</pre>     
             * 此参数必填
          */
    public void setAeopAEVideos(AlibabaproductAeopAEVideo[] aeopAEVideos) {
     	         	    this.aeopAEVideos = aeopAEVideos;
     	        }
    
    
  }
}