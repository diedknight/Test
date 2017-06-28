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
public class ApiGetOnlineLogisticsInfoResult {

       [DataMember(Order = 1)]
    private AeopLogisticsWarehouseOrderResult[] result;
    
        /**
       * @return 物流订单详细信息列表
    */
        public AeopLogisticsWarehouseOrderResult[] getResult() {
               	return result;
            }
    
    /**
     * 设置物流订单详细信息列表     *
          
             * 此参数必填
          */
    public void setResult(AeopLogisticsWarehouseOrderResult[] result) {
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
    private int? currentPage;
    
        /**
       * @return 当前页数
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页数     *
          
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 4)]
    private int? totalPage;
    
        /**
       * @return 总页数
    */
        public int? getTotalPage() {
               	return totalPage;
            }
    
    /**
     * 设置总页数     *
          
             * 此参数必填
          */
    public void setTotalPage(int totalPage) {
     	         	    this.totalPage = totalPage;
     	        }
    
        [DataMember(Order = 5)]
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