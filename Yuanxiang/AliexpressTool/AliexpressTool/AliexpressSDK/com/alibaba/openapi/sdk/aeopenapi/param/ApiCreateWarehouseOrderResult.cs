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
public class ApiCreateWarehouseOrderResult {

       [DataMember(Order = 1)]
    private AeopWlCreateWarehouseOrderResultDTO result;
    
        /**
       * @return 创建订单返回的结果
    */
        public AeopWlCreateWarehouseOrderResultDTO getResult() {
               	return result;
            }
    
    /**
     * 设置创建订单返回的结果     *
          
             * 此参数必填
          */
    public void setResult(AeopWlCreateWarehouseOrderResultDTO result) {
     	         	    this.result = result;
     	        }
    
        [DataMember(Order = 2)]
    private bool? success;
    
        /**
       * @return 调用是否成功
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置调用是否成功     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}