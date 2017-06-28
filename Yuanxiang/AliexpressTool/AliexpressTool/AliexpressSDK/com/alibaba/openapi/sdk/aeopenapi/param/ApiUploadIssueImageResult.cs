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
public class ApiUploadIssueImageResult {

       [DataMember(Order = 1)]
    private bool? success;
    
        /**
       * @return 操作结果
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置操作结果     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
        [DataMember(Order = 2)]
    private string code;
    
        /**
       * @return 错误码，当success=false时有值
    */
        public string getCode() {
               	return code;
            }
    
    /**
     * 设置错误码，当success=false时有值     *
          
             * 此参数必填
          */
    public void setCode(string code) {
     	         	    this.code = code;
     	        }
    
        [DataMember(Order = 3)]
    private string msg;
    
        /**
       * @return 错误原因，当success=false时有值
    */
        public string getMsg() {
               	return msg;
            }
    
    /**
     * 设置错误原因，当success=false时有值     *
          
             * 此参数必填
          */
    public void setMsg(string msg) {
     	         	    this.msg = msg;
     	        }
    
    
  }
}