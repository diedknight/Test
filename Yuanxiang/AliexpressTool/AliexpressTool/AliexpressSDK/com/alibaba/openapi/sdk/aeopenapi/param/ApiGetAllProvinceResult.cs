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
public class ApiGetAllProvinceResult {

       [DataMember(Order = 1)]
    private ChinaAddressItemDTO[] result;
    
        /**
       * @return 地址信息
    */
        public ChinaAddressItemDTO[] getResult() {
               	return result;
            }
    
    /**
     * 设置地址信息     *
          
             * 此参数必填
          */
    public void setResult(ChinaAddressItemDTO[] result) {
     	         	    this.result = result;
     	        }
    
        [DataMember(Order = 2)]
    private bool? isSuccess;
    
        /**
       * @return 调用是否成功
    */
        public bool? getIsSuccess() {
               	return isSuccess;
            }
    
    /**
     * 设置调用是否成功     *
          
             * 此参数必填
          */
    public void setIsSuccess(bool isSuccess) {
     	         	    this.isSuccess = isSuccess;
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