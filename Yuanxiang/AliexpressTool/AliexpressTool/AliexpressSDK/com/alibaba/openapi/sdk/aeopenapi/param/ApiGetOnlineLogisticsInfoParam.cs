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
public class ApiGetOnlineLogisticsInfoParam {

       [DataMember(Order = 1)]
    private long? orderId;
    
        /**
       * @return 交易订单ID
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置交易订单ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 2)]
    private string internationalLogisticsId;
    
        /**
       * @return 国际运单号
    */
        public string getInternationalLogisticsId() {
               	return internationalLogisticsId;
            }
    
    /**
     * 设置国际运单号     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setInternationalLogisticsId(string internationalLogisticsId) {
     	         	    this.internationalLogisticsId = internationalLogisticsId;
     	        }
    
        [DataMember(Order = 3)]
    private string chinaLogisticsId;
    
        /**
       * @return 国内快递运单号
    */
        public string getChinaLogisticsId() {
               	return chinaLogisticsId;
            }
    
    /**
     * 设置国内快递运单号     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setChinaLogisticsId(string chinaLogisticsId) {
     	         	    this.chinaLogisticsId = chinaLogisticsId;
     	        }
    
        [DataMember(Order = 4)]
    private string logisticsStatus;
    
        /**
       * @return 物流订单状态
    */
        public string getLogisticsStatus() {
               	return logisticsStatus;
            }
    
    /**
     * 设置物流订单状态     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLogisticsStatus(string logisticsStatus) {
     	         	    this.logisticsStatus = logisticsStatus;
     	        }
    
        [DataMember(Order = 5)]
    private string gmtCreateStartStr;
    
        /**
       * @return 开始时间
    */
        public string getGmtCreateStartStr() {
               	return gmtCreateStartStr;
            }
    
    /**
     * 设置开始时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGmtCreateStartStr(string gmtCreateStartStr) {
     	         	    this.gmtCreateStartStr = gmtCreateStartStr;
     	        }
    
        [DataMember(Order = 6)]
    private string gmtCreateEndStr;
    
        /**
       * @return 结束时间
    */
        public string getGmtCreateEndStr() {
               	return gmtCreateEndStr;
            }
    
    /**
     * 设置结束时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGmtCreateEndStr(string gmtCreateEndStr) {
     	         	    this.gmtCreateEndStr = gmtCreateEndStr;
     	        }
    
        [DataMember(Order = 7)]
    private int? currentPage;
    
        /**
       * @return 当前页面
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页面     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 8)]
    private int? pageSize;
    
        /**
       * @return 页面大小
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置页面大小     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
    
  }
}