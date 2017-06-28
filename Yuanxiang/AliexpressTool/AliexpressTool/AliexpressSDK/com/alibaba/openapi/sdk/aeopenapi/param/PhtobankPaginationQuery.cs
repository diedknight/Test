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
public class PhtobankPaginationQuery {

       [DataMember(Order = 1)]
    private string groupId;
    
        /**
       * @return 图片银行产品分组ID
    */
        public string getGroupId() {
               	return groupId;
            }
    
    /**
     * 设置图片银行产品分组ID     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setGroupId(string groupId) {
     	         	    this.groupId = groupId;
     	        }
    
        [DataMember(Order = 2)]
    private int? currentPage;
    
        /**
       * @return 当前页的值
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页的值     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 3)]
    private int? pageSize;
    
        /**
       * @return 每页的记录数
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页的记录数     *
     * 参数示例：<pre>19</pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
    
  }
}