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
public class ApiListLogisticsServiceResult {

       [DataMember(Order = 1)]
    private AeopLogisticsServiceResult[] result;
    
        /**
       * @return 物流方式列表
    */
        public AeopLogisticsServiceResult[] getResult() {
               	return result;
            }
    
    /**
     * 设置物流方式列表     *
          
             * 此参数必填
          */
    public void setResult(AeopLogisticsServiceResult[] result) {
     	         	    this.result = result;
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