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
public class AlibabaProductPostMultilanguageAeProductResult {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 新商品的ID
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置新商品的ID     *
          
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private bool? success;
    
        /**
       * @return 接口调用结果。true表示发布成功，false表示发布失败。
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置接口调用结果。true表示发布成功，false表示发布失败。     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}