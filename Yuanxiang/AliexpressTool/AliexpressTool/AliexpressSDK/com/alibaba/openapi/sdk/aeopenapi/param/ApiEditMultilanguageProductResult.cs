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
public class ApiEditMultilanguageProductResult {

       [DataMember(Order = 1)]
    private bool? success;
    
        /**
       * @return 接口调用结果。true/false分别表示成功和失败。
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置接口调用结果。true/false分别表示成功和失败。     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
        [DataMember(Order = 2)]
    private int? modifyCount;
    
        /**
       * @return 修改的商品数。
    */
        public int? getModifyCount() {
               	return modifyCount;
            }
    
    /**
     * 设置修改的商品数。     *
          
             * 此参数必填
          */
    public void setModifyCount(int modifyCount) {
     	         	    this.modifyCount = modifyCount;
     	        }
    
        [DataMember(Order = 3)]
    private long? productId;
    
        /**
       * @return 产品ID
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置产品ID     *
          
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
    
  }
}