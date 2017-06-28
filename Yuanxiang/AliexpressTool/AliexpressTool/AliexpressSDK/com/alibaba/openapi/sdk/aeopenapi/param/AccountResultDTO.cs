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
public class AccountResultDTO {

       [DataMember(Order = 1)]
    private bool? success;
    
        /**
       * @return 查询是否成功
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置查询是否成功     *
     * 参数示例：<pre>true</pre>     
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
        [DataMember(Order = 2)]
    private string errorMsg;
    
        /**
       * @return 错误信息
    */
        public string getErrorMsg() {
               	return errorMsg;
            }
    
    /**
     * 设置错误信息     *
     * 参数示例：<pre>权限不足，不能访问</pre>     
             * 此参数必填
          */
    public void setErrorMsg(string errorMsg) {
     	         	    this.errorMsg = errorMsg;
     	        }
    
        [DataMember(Order = 3)]
    private string errorCode;
    
        /**
       * @return 错误码
    */
        public string getErrorCode() {
               	return errorCode;
            }
    
    /**
     * 设置错误码     *
     * 参数示例：<pre>10001</pre>     
             * 此参数必填
          */
    public void setErrorCode(string errorCode) {
     	         	    this.errorCode = errorCode;
     	        }
    
        [DataMember(Order = 4)]
    private string level;
    
        /**
       * @return 会员等级
    */
        public string getLevel() {
               	return level;
            }
    
    /**
     * 设置会员等级     *
     * 参数示例：<pre>A0</pre>     
             * 此参数必填
          */
    public void setLevel(string level) {
     	         	    this.level = level;
     	        }
    
    
  }
}