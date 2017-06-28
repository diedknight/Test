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
public class ApiCreateProductGroupResult {

       [DataMember(Order = 1)]
    private string timeStamp;
    
        /**
       * @return 创建产品分组的时间
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
     * 设置创建产品分组的时间     *
          
             * 此参数必填
          */
    public void setTimeStamp(DateTime timeStamp) {
     	         	    this.timeStamp = DateUtil.format(timeStamp);
     	        }
    
        [DataMember(Order = 2)]
    private long? target;
    
        /**
       * @return 新创建的产品组ID
    */
        public long? getTarget() {
               	return target;
            }
    
    /**
     * 设置新创建的产品组ID     *
          
             * 此参数必填
          */
    public void setTarget(long target) {
     	         	    this.target = target;
     	        }
    
        [DataMember(Order = 3)]
    private string errorMessage;
    
        /**
       * @return 创建失败时的错误信息
    */
        public string getErrorMessage() {
               	return errorMessage;
            }
    
    /**
     * 设置创建失败时的错误信息     *
          
             * 此参数必填
          */
    public void setErrorMessage(string errorMessage) {
     	         	    this.errorMessage = errorMessage;
     	        }
    
        [DataMember(Order = 4)]
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
    
    
  }
}