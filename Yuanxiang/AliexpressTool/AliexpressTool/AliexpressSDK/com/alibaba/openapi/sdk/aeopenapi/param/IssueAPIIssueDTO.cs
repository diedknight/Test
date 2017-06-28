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
public class IssueAPIIssueDTO {

       [DataMember(Order = 1)]
    private long? id;
    
        /**
       * @return 纠纷ID
    */
        public long? getId() {
               	return id;
            }
    
    /**
     * 设置纠纷ID     *
     * 参数示例：<pre>680*************804</pre>     
             * 此参数必填
          */
    public void setId(long id) {
     	         	    this.id = id;
     	        }
    
        [DataMember(Order = 2)]
    private string gmtCreate;
    
        /**
       * @return 纠纷创建时间
    */
        public DateTime? getGmtCreate() {
                 if (gmtCreate != null)
          {
              DateTime datetime = DateUtil.formatFromStr(gmtCreate);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置纠纷创建时间     *
     * 参数示例：<pre>20150714020749000-0700</pre>     
             * 此参数必填
          */
    public void setGmtCreate(DateTime gmtCreate) {
     	         	    this.gmtCreate = DateUtil.format(gmtCreate);
     	        }
    
        [DataMember(Order = 3)]
    private string gmtModified;
    
        /**
       * @return 纠纷修改时间
    */
        public DateTime? getGmtModified() {
                 if (gmtModified != null)
          {
              DateTime datetime = DateUtil.formatFromStr(gmtModified);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置纠纷修改时间     *
     * 参数示例：<pre>20150714021033000-0700</pre>     
             * 此参数必填
          */
    public void setGmtModified(DateTime gmtModified) {
     	         	    this.gmtModified = DateUtil.format(gmtModified);
     	        }
    
        [DataMember(Order = 4)]
    private long? orderId;
    
        /**
       * @return 子订单ID
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置子订单ID     *
     * 参数示例：<pre>680*************804</pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 5)]
    private long? parentOrderId;
    
        /**
       * @return 父订单ID
    */
        public long? getParentOrderId() {
               	return parentOrderId;
            }
    
    /**
     * 设置父订单ID     *
     * 参数示例：<pre>0</pre>     
             * 此参数必填
          */
    public void setParentOrderId(long parentOrderId) {
     	         	    this.parentOrderId = parentOrderId;
     	        }
    
        [DataMember(Order = 6)]
    private string issueStatus;
    
        /**
       * @return 纠纷状态
WAIT_SELLER_CONFIRM_REFUND 买家提起纠纷
SELLER_REFUSE_REFUND 卖家拒绝纠纷
ACCEPTISSUE 卖家接受纠纷
WAIT_BUYER_SEND_GOODS 等待买家发货
WAIT_SELLER_RECEIVE_GOODS 买家发货，等待卖家收货
ARBITRATING 仲裁中
SELLER_RESPONSE_ISSUE_TIMEOUT 卖家响应纠纷超时
    */
        public string getIssueStatus() {
               	return issueStatus;
            }
    
    /**
     * 设置纠纷状态
WAIT_SELLER_CONFIRM_REFUND 买家提起纠纷
SELLER_REFUSE_REFUND 卖家拒绝纠纷
ACCEPTISSUE 卖家接受纠纷
WAIT_BUYER_SEND_GOODS 等待买家发货
WAIT_SELLER_RECEIVE_GOODS 买家发货，等待卖家收货
ARBITRATING 仲裁中
SELLER_RESPONSE_ISSUE_TIMEOUT 卖家响应纠纷超时     *
     * 参数示例：<pre>WAIT_SELLER_CONFIRM_REFUND</pre>     
             * 此参数必填
          */
    public void setIssueStatus(string issueStatus) {
     	         	    this.issueStatus = issueStatus;
     	        }
    
        [DataMember(Order = 7)]
    private IssueAPIIssueProcessDTO[] issueProcessDTOs;
    
        /**
       * @return 纠纷处理过程，只有detail接口展示
    */
        public IssueAPIIssueProcessDTO[] getIssueProcessDTOs() {
               	return issueProcessDTOs;
            }
    
    /**
     * 设置纠纷处理过程，只有detail接口展示     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setIssueProcessDTOs(IssueAPIIssueProcessDTO[] issueProcessDTOs) {
     	         	    this.issueProcessDTOs = issueProcessDTOs;
     	        }
    
        [DataMember(Order = 8)]
    private IssueMoney limitRefundAmount;
    
        /**
       * @return 最大退款金额美金
    */
        public IssueMoney getLimitRefundAmount() {
               	return limitRefundAmount;
            }
    
    /**
     * 设置最大退款金额美金     *
     * 参数示例：<pre>{&quot;amount&quot;:0.01,&quot;cent&quot;:1,&quot;currencyCode&quot;:&quot;USD&quot;,&quot;centFactor&quot;:100,&quot;currency&quot;:{&quot;defaultFractionDigits&quot;:2,&quot;currencyCode&quot;:&quot;USD&quot;,&quot;symbol&quot;:&quot;$&quot;}</pre>     
             * 此参数必填
          */
    public void setLimitRefundAmount(IssueMoney limitRefundAmount) {
     	         	    this.limitRefundAmount = limitRefundAmount;
     	        }
    
        [DataMember(Order = 9)]
    private IssueMoney limitRefundLocalAmount;
    
        /**
       * @return 最大退款金额本币
    */
        public IssueMoney getLimitRefundLocalAmount() {
               	return limitRefundLocalAmount;
            }
    
    /**
     * 设置最大退款金额本币     *
     * 参数示例：<pre>{&quot;amount&quot;:0.1,&quot;cent&quot;:10,&quot;currencyCode&quot;:&quot;RUB&quot;,&quot;centFactor&quot;:100,&quot;currency&quot;:{&quot;defaultFractionDigits&quot;:2,&quot;currencyCode&quot;:&quot;RUB&quot;,&quot;symbol&quot;:&quot;RUB&quot;}</pre>     
             * 此参数必填
          */
    public void setLimitRefundLocalAmount(IssueMoney limitRefundLocalAmount) {
     	         	    this.limitRefundLocalAmount = limitRefundLocalAmount;
     	        }
    
        [DataMember(Order = 10)]
    private string reasonChinese;
    
        /**
       * @return 纠纷原因中文描述
    */
        public string getReasonChinese() {
               	return reasonChinese;
            }
    
    /**
     * 设置纠纷原因中文描述     *
     * 参数示例：<pre>产品数量不符</pre>     
             * 此参数必填
          */
    public void setReasonChinese(string reasonChinese) {
     	         	    this.reasonChinese = reasonChinese;
     	        }
    
        [DataMember(Order = 11)]
    private string reasonEnglish;
    
        /**
       * @return 纠纷原因英文描述
    */
        public string getReasonEnglish() {
               	return reasonEnglish;
            }
    
    /**
     * 设置纠纷原因英文描述     *
     * 参数示例：<pre>Quantity shortage</pre>     
             * 此参数必填
          */
    public void setReasonEnglish(string reasonEnglish) {
     	         	    this.reasonEnglish = reasonEnglish;
     	        }
    
    
  }
}