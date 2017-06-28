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
public class OperationResult {

       [DataMember(Order = 1)]
    private int? errorCode;
    
        /**
       * @return 错误码：100 无此订单,601 帐号无权限,200 业务数据错误，无对应的业务数据,201 业务数据错误无法执行此操作
    */
        public int? getErrorCode() {
               	return errorCode;
            }
    
    /**
     * 设置错误码：100 无此订单,601 帐号无权限,200 业务数据错误，无对应的业务数据,201 业务数据错误无法执行此操作     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setErrorCode(int errorCode) {
     	         	    this.errorCode = errorCode;
     	        }
    
        [DataMember(Order = 2)]
    private string memo;
    
        /**
       * @return 详细说明
    */
        public string getMemo() {
               	return memo;
            }
    
    /**
     * 设置详细说明     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMemo(string memo) {
     	         	    this.memo = memo;
     	        }
    
        [DataMember(Order = 3)]
    private bool? success;
    
        /**
       * @return 是否成功true/false
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置是否成功true/false     *
     * 参数示例：<pre>true</pre>     
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}