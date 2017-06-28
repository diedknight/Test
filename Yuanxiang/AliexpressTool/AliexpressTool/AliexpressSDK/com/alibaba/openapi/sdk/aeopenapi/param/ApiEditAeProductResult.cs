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
public class ApiEditAeProductResult {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 商品的ID。
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置商品的ID。     *
          
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private int? modifyCount;
    
        /**
       * @return 编辑成功次数。对于编辑商品来说，这个参数为1。
    */
        public int? getModifyCount() {
               	return modifyCount;
            }
    
    /**
     * 设置编辑成功次数。对于编辑商品来说，这个参数为1。     *
          
             * 此参数必填
          */
    public void setModifyCount(int modifyCount) {
     	         	    this.modifyCount = modifyCount;
     	        }
    
        [DataMember(Order = 3)]
    private bool? success;
    
        /**
       * @return 接口调用结果。成功为true，失败为false。
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置接口调用结果。成功为true，失败为false。     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}