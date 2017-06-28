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
public class ApiQueryTrackingResultResult {

       [DataMember(Order = 1)]
    private AeopTrackingDetailResult[] details;
    
        /**
       * @return 追踪详细信息列表
    */
        public AeopTrackingDetailResult[] getDetails() {
               	return details;
            }
    
    /**
     * 设置追踪详细信息列表     *
          
             * 此参数必填
          */
    public void setDetails(AeopTrackingDetailResult[] details) {
     	         	    this.details = details;
     	        }
    
        [DataMember(Order = 2)]
    private bool? success;
    
        /**
       * @return 调用是否成功
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置调用是否成功     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
        [DataMember(Order = 3)]
    private string officialWebsite;
    
        /**
       * @return 追踪网址
    */
        public string getOfficialWebsite() {
               	return officialWebsite;
            }
    
    /**
     * 设置追踪网址     *
          
             * 此参数必填
          */
    public void setOfficialWebsite(string officialWebsite) {
     	         	    this.officialWebsite = officialWebsite;
     	        }
    
        [DataMember(Order = 4)]
    private string errorDesc;
    
        /**
       * @return 调用出错信息
    */
        public string getErrorDesc() {
               	return errorDesc;
            }
    
    /**
     * 设置调用出错信息     *
          
             * 此参数必填
          */
    public void setErrorDesc(string errorDesc) {
     	         	    this.errorDesc = errorDesc;
     	        }
    
    
  }
}