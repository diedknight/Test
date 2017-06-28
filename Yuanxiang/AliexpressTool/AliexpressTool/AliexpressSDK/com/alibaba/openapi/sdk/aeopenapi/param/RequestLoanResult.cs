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
public class RequestLoanResult {

       [DataMember(Order = 1)]
    private string memo;
    
        /**
       * @return 返回的备注信息，当为上传附件时 ，memo就是附件的路径
    */
        public string getMemo() {
               	return memo;
            }
    
    /**
     * 设置返回的备注信息，当为上传附件时 ，memo就是附件的路径     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMemo(string memo) {
     	         	    this.memo = memo;
     	        }
    
        [DataMember(Order = 2)]
    private int? resultCode;
    
        /**
       * @return 错误码，200表示成功
    */
        public int? getResultCode() {
               	return resultCode;
            }
    
    /**
     * 设置错误码，200表示成功     *
     * 参数示例：<pre>200</pre>     
             * 此参数必填
          */
    public void setResultCode(int resultCode) {
     	         	    this.resultCode = resultCode;
     	        }
    
        [DataMember(Order = 3)]
    private bool? success;
    
        /**
       * @return 请求放款是否成功
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置请求放款是否成功     *
     * 参数示例：<pre>false</pre>     
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}