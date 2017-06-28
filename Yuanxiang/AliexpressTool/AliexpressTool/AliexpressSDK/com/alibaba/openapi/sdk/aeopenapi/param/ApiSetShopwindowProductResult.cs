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
public class ApiSetShopwindowProductResult {

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
    private int? remainingWindowCount;
    
        /**
       * @return 剩余的可用橱窗数。
    */
        public int? getRemainingWindowCount() {
               	return remainingWindowCount;
            }
    
    /**
     * 设置剩余的可用橱窗数。     *
          
             * 此参数必填
          */
    public void setRemainingWindowCount(int remainingWindowCount) {
     	         	    this.remainingWindowCount = remainingWindowCount;
     	        }
    
    
  }
}