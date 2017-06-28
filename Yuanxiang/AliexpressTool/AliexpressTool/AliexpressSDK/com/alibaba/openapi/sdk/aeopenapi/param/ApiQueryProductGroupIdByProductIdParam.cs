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
public class ApiQueryProductGroupIdByProductIdParam {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 产品id
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置产品id     *
     * 参数示例：<pre>1629872654</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
    
  }
}