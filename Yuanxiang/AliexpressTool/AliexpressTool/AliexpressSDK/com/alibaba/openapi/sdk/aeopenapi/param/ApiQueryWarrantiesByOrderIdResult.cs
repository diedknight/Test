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
public class ApiQueryWarrantiesByOrderIdResult {

       [DataMember(Order = 1)]
    private WarrantyDetailApiDTO[] dataList;
    
        /**
       * @return 保修数据明细
    */
        public WarrantyDetailApiDTO[] getDataList() {
               	return dataList;
            }
    
    /**
     * 设置保修数据明细     *
          
             * 此参数必填
          */
    public void setDataList(WarrantyDetailApiDTO[] dataList) {
     	         	    this.dataList = dataList;
     	        }
    
        [DataMember(Order = 2)]
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
    
        [DataMember(Order = 3)]
    private string code;
    
        /**
       * @return 错误码，当success=false时存在
    */
        public string getCode() {
               	return code;
            }
    
    /**
     * 设置错误码，当success=false时存在     *
          
             * 此参数必填
          */
    public void setCode(string code) {
     	         	    this.code = code;
     	        }
    
        [DataMember(Order = 4)]
    private string msg;
    
        /**
       * @return 错误描述，当success=false时存在
    */
        public string getMsg() {
               	return msg;
            }
    
    /**
     * 设置错误描述，当success=false时存在     *
          
             * 此参数必填
          */
    public void setMsg(string msg) {
     	         	    this.msg = msg;
     	        }
    
    
  }
}