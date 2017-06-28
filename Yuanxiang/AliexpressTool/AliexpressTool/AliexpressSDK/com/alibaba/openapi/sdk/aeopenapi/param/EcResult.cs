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
public class EcResult {

       [DataMember(Order = 1)]
    private bool? isSuccess;
    
        /**
       * @return 是否成功
    */
        public bool? getIsSuccess() {
               	return isSuccess;
            }
    
    /**
     * 设置是否成功     *
     * 参数示例：<pre>false</pre>     
             * 此参数必填
          */
    public void setIsSuccess(bool isSuccess) {
     	         	    this.isSuccess = isSuccess;
     	        }
    
        [DataMember(Order = 2)]
    private int? errorCode;
    
        /**
       * @return 错误码
    */
        public int? getErrorCode() {
               	return errorCode;
            }
    
    /**
     * 设置错误码     *
     * 参数示例：<pre>9001</pre>     
             * 此参数必填
          */
    public void setErrorCode(int errorCode) {
     	         	    this.errorCode = errorCode;
     	        }
    
        [DataMember(Order = 3)]
    private string errorMsg;
    
        /**
       * @return 错误信息
    */
        public string getErrorMsg() {
               	return errorMsg;
            }
    
    /**
     * 设置错误信息     *
     * 参数示例：<pre>参数有误</pre>     
             * 此参数必填
          */
    public void setErrorMsg(string errorMsg) {
     	         	    this.errorMsg = errorMsg;
     	        }
    
    
  }
}