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
public class AlibabaaeproductAeopModifyProductResponse {

       [DataMember(Order = 1)]
    private bool? isSuccess;
    
        /**
       * @return 是否操作成功
    */
        public bool? getIsSuccess() {
               	return isSuccess;
            }
    
    /**
     * 设置是否操作成功     *
     * 参数示例：<pre>true</pre>     
             * 此参数必填
          */
    public void setIsSuccess(bool isSuccess) {
     	         	    this.isSuccess = isSuccess;
     	        }
    
        [DataMember(Order = 2)]
    private long? productId;
    
        /**
       * @return 操作的商品id
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置操作的商品id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 3)]
    private int? modifyCount;
    
        /**
       * @return 成功个数
    */
        public int? getModifyCount() {
               	return modifyCount;
            }
    
    /**
     * 设置成功个数     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setModifyCount(int modifyCount) {
     	         	    this.modifyCount = modifyCount;
     	        }
    
        [DataMember(Order = 4)]
    private AlibabaproductonlineErrorDetail[] errorDetails;
    
        /**
       * @return 错误详情
    */
        public AlibabaproductonlineErrorDetail[] getErrorDetails() {
               	return errorDetails;
            }
    
    /**
     * 设置错误详情     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setErrorDetails(AlibabaproductonlineErrorDetail[] errorDetails) {
     	         	    this.errorDetails = errorDetails;
     	        }
    
    
  }
}