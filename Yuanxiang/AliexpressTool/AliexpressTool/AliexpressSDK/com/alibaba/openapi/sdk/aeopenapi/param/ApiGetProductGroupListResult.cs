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
public class ApiGetProductGroupListResult {

       [DataMember(Order = 1)]
    private AeopProductGroup[] target;
    
        /**
       * @return 产品分组信息
    */
        public AeopProductGroup[] getTarget() {
               	return target;
            }
    
    /**
     * 设置产品分组信息     *
          
             * 此参数必填
          */
    public void setTarget(AeopProductGroup[] target) {
     	         	    this.target = target;
     	        }
    
        [DataMember(Order = 2)]
    private string timeStamp;
    
        /**
       * @return 调用接口的时间戳
    */
        public string getTimeStamp() {
               	return timeStamp;
            }
    
    /**
     * 设置调用接口的时间戳     *
          
             * 此参数必填
          */
    public void setTimeStamp(string timeStamp) {
     	         	    this.timeStamp = timeStamp;
     	        }
    
        [DataMember(Order = 3)]
    private bool? success;
    
        /**
       * @return 接口的调用结果。true/false分别表示成功和失败。
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置接口的调用结果。true/false分别表示成功和失败。     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}