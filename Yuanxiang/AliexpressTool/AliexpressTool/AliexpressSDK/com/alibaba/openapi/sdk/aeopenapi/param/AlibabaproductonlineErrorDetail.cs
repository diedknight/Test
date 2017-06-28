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
public class AlibabaproductonlineErrorDetail {

       [DataMember(Order = 1)]
    private string errorCode;
    
        /**
       * @return 错误码
    */
        public string getErrorCode() {
               	return errorCode;
            }
    
    /**
     * 设置错误码     *
     * 参数示例：<pre>11015111</pre>     
             * 此参数必填
          */
    public void setErrorCode(string errorCode) {
     	         	    this.errorCode = errorCode;
     	        }
    
        [DataMember(Order = 2)]
    private long[] productIds;
    
        /**
       * @return 产品ID列表
    */
        public long[] getProductIds() {
               	return productIds;
            }
    
    /**
     * 设置产品ID列表     *
     * 参数示例：<pre>[50001056157, 50001056153]</pre>     
             * 此参数必填
          */
    public void setProductIds(long[] productIds) {
     	         	    this.productIds = productIds;
     	        }
    
    
  }
}