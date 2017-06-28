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
public class ApiQueryProductGroupIdByProductIdResult {

       [DataMember(Order = 1)]
    private GroupIdHolder[] target;
    
        /**
       * @return 这个产品所关联的产品分组ID列表。
    */
        public GroupIdHolder[] getTarget() {
               	return target;
            }
    
    /**
     * 设置这个产品所关联的产品分组ID列表。     *
          
             * 此参数必填
          */
    public void setTarget(GroupIdHolder[] target) {
     	         	    this.target = target;
     	        }
    
        [DataMember(Order = 2)]
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
    
        [DataMember(Order = 3)]
    private string timeStamp;
    
        /**
       * @return 调用接口的时间。
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
     * 设置调用接口的时间。     *
          
             * 此参数必填
          */
    public void setTimeStamp(DateTime timeStamp) {
     	         	    this.timeStamp = DateUtil.format(timeStamp);
     	        }
    
    
  }
}