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
public class ApiQueryProductFavoritedInfoEverydayByIdParam {

       [DataMember(Order = 1)]
    private string productId;
    
        /**
       * @return 商品id。
    */
        public string getProductId() {
               	return productId;
            }
    
    /**
     * 设置商品id。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductId(string productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private string startDate;
    
        /**
       * @return 查询时间段的开始时间。例如：yyyy-mm-dd
    */
        public string getStartDate() {
               	return startDate;
            }
    
    /**
     * 设置查询时间段的开始时间。例如：yyyy-mm-dd     *
     * 参数示例：<pre>2014-01-22</pre>     
             * 此参数必填
          */
    public void setStartDate(string startDate) {
     	         	    this.startDate = startDate;
     	        }
    
        [DataMember(Order = 3)]
    private string endDate;
    
        /**
       * @return 查询时间段的截止时间。例如：yyyy-mm-dd
    */
        public string getEndDate() {
               	return endDate;
            }
    
    /**
     * 设置查询时间段的截止时间。例如：yyyy-mm-dd     *
     * 参数示例：<pre>2014-01-22</pre>     
             * 此参数必填
          */
    public void setEndDate(string endDate) {
     	         	    this.endDate = endDate;
     	        }
    
        [DataMember(Order = 4)]
    private int? currentPage;
    
        /**
       * @return 当前页码。
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页码。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 5)]
    private int? pageSize;
    
        /**
       * @return 每页结果数量，默认20个，最大值 50
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页结果数量，默认20个，最大值 50     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
    
  }
}