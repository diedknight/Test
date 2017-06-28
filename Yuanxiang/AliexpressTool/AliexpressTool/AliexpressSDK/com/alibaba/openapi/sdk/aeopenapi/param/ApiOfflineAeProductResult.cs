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
public class ApiOfflineAeProductResult {

       [DataMember(Order = 1)]
    private int? modifyCount;
    
        /**
       * @return 成功下架的商品数。
    */
        public int? getModifyCount() {
               	return modifyCount;
            }
    
    /**
     * 设置成功下架的商品数。     *
          
             * 此参数必填
          */
    public void setModifyCount(int modifyCount) {
     	         	    this.modifyCount = modifyCount;
     	        }
    
        [DataMember(Order = 2)]
    private bool? success;
    
        /**
       * @return 接口调用结果。成功为true, 失败为false。
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置接口调用结果。成功为true, 失败为false。     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}