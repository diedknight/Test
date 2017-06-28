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
public class ApiQueryMsgRelationListParam {

       [DataMember(Order = 1)]
    private int? currentPage;
    
        /**
       * @return 当前页
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 2)]
    private int? pageSize;
    
        /**
       * @return 每页条数,pageSize取值范围(0~100) 最多返回前5000条数据
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页条数,pageSize取值范围(0~100) 最多返回前5000条数据     *
     * 参数示例：<pre>20</pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
        [DataMember(Order = 3)]
    private string msgSources;
    
        /**
       * @return 查询类型
    */
        public string getMsgSources() {
               	return msgSources;
            }
    
    /**
     * 设置查询类型     *
     * 参数示例：<pre>message_center/order_msg</pre>     
             * 此参数必填
          */
    public void setMsgSources(string msgSources) {
     	         	    this.msgSources = msgSources;
     	        }
    
        [DataMember(Order = 4)]
    private string filter;
    
        /**
       * @return 筛选条件(取值:dealStat/readStat/rank0/rank1/rank2/rank3/rank4/rank5)dealStat时将按未处理筛选，值为readStat时将按未读筛选，值为rank1时将按红色标签进行筛选
    */
        public string getFilter() {
               	return filter;
            }
    
    /**
     * 设置筛选条件(取值:dealStat/readStat/rank0/rank1/rank2/rank3/rank4/rank5)dealStat时将按未处理筛选，值为readStat时将按未读筛选，值为rank1时将按红色标签进行筛选     *
     * 参数示例：<pre>dealStat</pre>     
             * 此参数必填
          */
    public void setFilter(string filter) {
     	         	    this.filter = filter;
     	        }
    
    
  }
}