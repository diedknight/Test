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
public class ApiDelUnUsePhotoParam {

       [DataMember(Order = 1)]
    private long? imageRepositoryId;
    
        /**
       * @return 图片ID(通过“图片银行列表分页查询”接口进行获取，出参中“ iid:图片”为图片ID。
    */
        public long? getImageRepositoryId() {
               	return imageRepositoryId;
            }
    
    /**
     * 设置图片ID(通过“图片银行列表分页查询”接口进行获取，出参中“ iid:图片”为图片ID。     *
     * 参数示例：<pre>100403959</pre>     
             * 此参数必填
          */
    public void setImageRepositoryId(long imageRepositoryId) {
     	         	    this.imageRepositoryId = imageRepositoryId;
     	        }
    
    
  }
}