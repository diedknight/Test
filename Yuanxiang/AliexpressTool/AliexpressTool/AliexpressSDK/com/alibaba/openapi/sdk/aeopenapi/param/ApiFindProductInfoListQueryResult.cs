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
public class ApiFindProductInfoListQueryResult {

       [DataMember(Order = 1)]
    private AeopAEProductDisplaySampleDTO[] aeopAEProductDisplayDTOList;
    
        /**
       * @return 商品基本信息列表
    */
        public AeopAEProductDisplaySampleDTO[] getAeopAEProductDisplayDTOList() {
               	return aeopAEProductDisplayDTOList;
            }
    
    /**
     * 设置商品基本信息列表     *
          
             * 此参数必填
          */
    public void setAeopAEProductDisplayDTOList(AeopAEProductDisplaySampleDTO[] aeopAEProductDisplayDTOList) {
     	         	    this.aeopAEProductDisplayDTOList = aeopAEProductDisplayDTOList;
     	        }
    
        [DataMember(Order = 2)]
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
    
        [DataMember(Order = 3)]
    private int? currentPage;
    
        /**
       * @return 当前页
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页     *
          
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 4)]
    private int? productCount;
    
        /**
       * @return 总商品数
    */
        public int? getProductCount() {
               	return productCount;
            }
    
    /**
     * 设置总商品数     *
          
             * 此参数必填
          */
    public void setProductCount(int productCount) {
     	         	    this.productCount = productCount;
     	        }
    
        [DataMember(Order = 5)]
    private bool? success;
    
        /**
       * @return 接口调用结果
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置接口调用结果     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}