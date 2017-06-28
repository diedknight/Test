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
public class ApiQueryMsgDetailListByBuyerIdParam {

       [DataMember(Order = 1)]
    private int? currentPage;
    
        /**
       * @return 当前页数
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页数     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 2)]
    private int? pageSize;
    
        /**
       * @return 每页条数
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页条数     *
     * 参数示例：<pre>10(pageSize取值范围(0~100) 最多返回前5000条数据)</pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
        [DataMember(Order = 3)]
    private string buyerId;
    
        /**
       * @return 买家loginId
    */
        public string getBuyerId() {
               	return buyerId;
            }
    
    /**
     * 设置买家loginId     *
     * 参数示例：<pre>us23344</pre>     
             * 此参数必填
          */
    public void setBuyerId(string buyerId) {
     	         	    this.buyerId = buyerId;
     	        }
    
        [DataMember(Order = 4)]
    private string sellerId;
    
        /**
       * @return 卖家loginId(与买家建立关系的账号，即信息所属账号)
    */
        public string getSellerId() {
               	return sellerId;
            }
    
    /**
     * 设置卖家loginId(与买家建立关系的账号，即信息所属账号)     *
     * 参数示例：<pre>cn4444</pre>     
             * 此参数必填
          */
    public void setSellerId(string sellerId) {
     	         	    this.sellerId = sellerId;
     	        }
    
    
  }
}