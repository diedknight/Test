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
public class ApiFindLoanListQueryParam {

       [DataMember(Order = 1)]
    private int? page;
    
        /**
       * @return 当前页码.。
    */
        public int? getPage() {
               	return page;
            }
    
    /**
     * 设置当前页码.。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPage(int page) {
     	         	    this.page = page;
     	        }
    
        [DataMember(Order = 2)]
    private int? pageSize;
    
        /**
       * @return 每页个数，最大50。
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页个数，最大50。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
        [DataMember(Order = 3)]
    private string createDateStart;
    
        /**
       * @return 放款时间起始值，格式: mm/dd/yyyy hh:mm:ss,如10/08/2013 00:00:00
    */
        public string getCreateDateStart() {
               	return createDateStart;
            }
    
    /**
     * 设置放款时间起始值，格式: mm/dd/yyyy hh:mm:ss,如10/08/2013 00:00:00     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCreateDateStart(string createDateStart) {
     	         	    this.createDateStart = createDateStart;
     	        }
    
        [DataMember(Order = 4)]
    private string createDateEnd;
    
        /**
       * @return 放款时间截止值，格式: mm/dd/yyyy hh:mm:ss,如10/09/2013 00:00:00
    */
        public string getCreateDateEnd() {
               	return createDateEnd;
            }
    
    /**
     * 设置放款时间截止值，格式: mm/dd/yyyy hh:mm:ss,如10/09/2013 00:00:00     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCreateDateEnd(string createDateEnd) {
     	         	    this.createDateEnd = createDateEnd;
     	        }
    
        [DataMember(Order = 5)]
    private string loanStatus;
    
        /**
       * @return 订单放款状态：wait_loan 未放款，loan_ok已放款。
    */
        public string getLoanStatus() {
               	return loanStatus;
            }
    
    /**
     * 设置订单放款状态：wait_loan 未放款，loan_ok已放款。     *
     * 参数示例：<pre>wait_loan</pre>     
             * 此参数必填
          */
    public void setLoanStatus(string loanStatus) {
     	         	    this.loanStatus = loanStatus;
     	        }
    
        [DataMember(Order = 6)]
    private long? orderId;
    
        /**
       * @return 主订单id，一次只能查询一个
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置主订单id，一次只能查询一个     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
    
  }
}