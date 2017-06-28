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
public class ApiGetWindowProductsResult {

       [DataMember(Order = 1)]
    private AeopAeProductWindow[] windowProducts;
    
        /**
       * @return 已使用的橱窗信息。
    */
        public AeopAeProductWindow[] getWindowProducts() {
               	return windowProducts;
            }
    
    /**
     * 设置已使用的橱窗信息。     *
          
             * 此参数必填
          */
    public void setWindowProducts(AeopAeProductWindow[] windowProducts) {
     	         	    this.windowProducts = windowProducts;
     	        }
    
        [DataMember(Order = 2)]
    private int? usedCount;
    
        /**
       * @return 已使用的橱窗个数，与windowProducts中记录的条数一致。
    */
        public int? getUsedCount() {
               	return usedCount;
            }
    
    /**
     * 设置已使用的橱窗个数，与windowProducts中记录的条数一致。     *
          
             * 此参数必填
          */
    public void setUsedCount(int usedCount) {
     	         	    this.usedCount = usedCount;
     	        }
    
        [DataMember(Order = 3)]
    private int? windowCount;
    
        /**
       * @return 当前用户的橱窗总数＝已使用的橱窗数＋未使用的橱窗数。
    */
        public int? getWindowCount() {
               	return windowCount;
            }
    
    /**
     * 设置当前用户的橱窗总数＝已使用的橱窗数＋未使用的橱窗数。     *
          
             * 此参数必填
          */
    public void setWindowCount(int windowCount) {
     	         	    this.windowCount = windowCount;
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
    
        [DataMember(Order = 5)]
    private long[] productList;
    
        /**
       * @return 已推荐为橱窗商品的ID列表。与windowProducts中的产品ID一致。
    */
        public long[] getProductList() {
               	return productList;
            }
    
    /**
     * 设置已推荐为橱窗商品的ID列表。与windowProducts中的产品ID一致。     *
          
             * 此参数必填
          */
    public void setProductList(long[] productList) {
     	         	    this.productList = productList;
     	        }
    
    
  }
}