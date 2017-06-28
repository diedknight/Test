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
public class ApiFindAeProductDetailModuleListByQureyResult {

       [DataMember(Order = 1)]
    private AeopDetailModule[] aeopDetailModuleList;
    
        /**
       * @return 模块信息列表
    */
        public AeopDetailModule[] getAeopDetailModuleList() {
               	return aeopDetailModuleList;
            }
    
    /**
     * 设置模块信息列表     *
          
             * 此参数必填
          */
    public void setAeopDetailModuleList(AeopDetailModule[] aeopDetailModuleList) {
     	         	    this.aeopDetailModuleList = aeopDetailModuleList;
     	        }
    
        [DataMember(Order = 2)]
    private int? currentPage;
    
        /**
       * @return 当前页号
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页号     *
          
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 3)]
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