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
public class ApiQueryIssueListParam {

       [DataMember(Order = 1)]
    private long? orderNo;
    
        /**
       * @return 订单ID
    */
        public long? getOrderNo() {
               	return orderNo;
            }
    
    /**
     * 设置订单ID     *
     * 参数示例：<pre>1234567890</pre>     
             * 此参数必填
          */
    public void setOrderNo(long orderNo) {
     	         	    this.orderNo = orderNo;
     	        }
    
        [DataMember(Order = 2)]
    private string buyerName;
    
        /**
       * @return 买家名称
    */
        public string getBuyerName() {
               	return buyerName;
            }
    
    /**
     * 设置买家名称     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBuyerName(string buyerName) {
     	         	    this.buyerName = buyerName;
     	        }
    
        [DataMember(Order = 3)]
    private string issueStatus;
    
        /**
       * @return 纠纷状态：WAIT_SELLER_CONFIRM_REFUND 买家提起纠纷,SELLER_REFUSE_REFUND 卖家拒绝纠,ACCEPTISSUE 卖家接受纠纷,WAIT_BUYER_SEND_GOODS 等待买家发货,WAIT_SELLER_RECEIVE_GOODS 买家发货，等待卖家收货,ARBITRATING 仲裁中,SELLER_RESPONSE_ISSUE_TIMEOUT 卖家响应纠纷超时
    */
        public string getIssueStatus() {
               	return issueStatus;
            }
    
    /**
     * 设置纠纷状态：WAIT_SELLER_CONFIRM_REFUND 买家提起纠纷,SELLER_REFUSE_REFUND 卖家拒绝纠,ACCEPTISSUE 卖家接受纠纷,WAIT_BUYER_SEND_GOODS 等待买家发货,WAIT_SELLER_RECEIVE_GOODS 买家发货，等待卖家收货,ARBITRATING 仲裁中,SELLER_RESPONSE_ISSUE_TIMEOUT 卖家响应纠纷超时     *
     * 参数示例：<pre>WAIT_SELLER_CONFIRM_REFUND</pre>     
             * 此参数必填
          */
    public void setIssueStatus(string issueStatus) {
     	         	    this.issueStatus = issueStatus;
     	        }
    
        [DataMember(Order = 4)]
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
    
    
  }
}