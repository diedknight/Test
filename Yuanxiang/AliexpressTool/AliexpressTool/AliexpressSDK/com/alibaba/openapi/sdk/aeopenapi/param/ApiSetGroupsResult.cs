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
public class ApiSetGroupsResult {

       [DataMember(Order = 1)]
    private string success;
    
        /**
       * @return 操作结果。true/false分别表示成功和失败。
    */
        public string getSuccess() {
               	return success;
            }
    
    /**
     * 设置操作结果。true/false分别表示成功和失败。     *
          
             * 此参数必填
          */
    public void setSuccess(string success) {
     	         	    this.success = success;
     	        }
    
        [DataMember(Order = 2)]
    private string timeStamp;
    
        /**
       * @return 20150714015815415-0700
    */
        public DateTime? getTimeStamp() {
                 if (timeStamp != null)
          {
              DateTime datetime = DateUtil.formatFromStr(timeStamp);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置20150714015815415-0700     *
          
             * 此参数必填
          */
    public void setTimeStamp(DateTime timeStamp) {
     	         	    this.timeStamp = DateUtil.format(timeStamp);
     	        }
    
        [DataMember(Order = 3)]
    private long[] target;
    
        /**
       * @return 绑定成功的产品分组列表。
    */
        public long[] getTarget() {
               	return target;
            }
    
    /**
     * 设置绑定成功的产品分组列表。     *
          
             * 此参数必填
          */
    public void setTarget(long[] target) {
     	         	    this.target = target;
     	        }
    
        [DataMember(Order = 4)]
    private string errorMessage;
    
        /**
       * @return 出错时的错误信息。
    */
        public string getErrorMessage() {
               	return errorMessage;
            }
    
    /**
     * 设置出错时的错误信息。     *
          
             * 此参数必填
          */
    public void setErrorMessage(string errorMessage) {
     	         	    this.errorMessage = errorMessage;
     	        }
    
    
  }
}