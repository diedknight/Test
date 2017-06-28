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
public class IssueAPIIssueProcessDTO {

       [DataMember(Order = 1)]
    private long? id;
    
        /**
       * @return 过程id
    */
        public long? getId() {
               	return id;
            }
    
    /**
     * 设置过程id     *
     * 参数示例：<pre>680*************804</pre>     
             * 此参数必填
          */
    public void setId(long id) {
     	         	    this.id = id;
     	        }
    
        [DataMember(Order = 2)]
    private string gmtCreate;
    
        /**
       * @return 创建时间
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
     * 设置创建时间     *
     * 参数示例：<pre>20150714020749000-0700</pre>     
             * 此参数必填
          */
    public void setGmtCreate(DateTime gmtCreate) {
     	         	    this.gmtCreate = DateUtil.format(gmtCreate);
     	        }
    
        [DataMember(Order = 3)]
    private string gmtModified;
    
        /**
       * @return 修改时间
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
     * 设置修改时间     *
     * 参数示例：<pre>20150714020749000-0700</pre>     
             * 此参数必填
          */
    public void setGmtModified(DateTime gmtModified) {
     	         	    this.gmtModified = DateUtil.format(gmtModified);
     	        }
    
        [DataMember(Order = 4)]
    private long? issueId;
    
        /**
       * @return 纠纷id
    */
        public long? getIssueId() {
               	return issueId;
            }
    
    /**
     * 设置纠纷id     *
     * 参数示例：<pre>680*************804</pre>     
             * 此参数必填
          */
    public void setIssueId(long issueId) {
     	         	    this.issueId = issueId;
     	        }
    
        [DataMember(Order = 5)]
    private string reason;
    
        /**
       * @return 纠纷原因
    */
        public string getReason() {
               	return reason;
            }
    
    /**
     * 设置纠纷原因     *
     * 参数示例：<pre>Color_not_as_described@3rdIssueReason</pre>     
             * 此参数必填
          */
    public void setReason(string reason) {
     	         	    this.reason = reason;
     	        }
    
        [DataMember(Order = 6)]
    private string content;
    
        /**
       * @return 纠纷描述
    */
        public string getContent() {
               	return content;
            }
    
    /**
     * 设置纠纷描述     *
     * 参数示例：<pre>The produit don't turn one.</pre>     
             * 此参数必填
          */
    public void setContent(string content) {
     	         	    this.content = content;
     	        }
    
        [DataMember(Order = 7)]
    private IssueMoney refundAmount;
    
        /**
       * @return 退款金额本币
    */
        public IssueMoney getRefundAmount() {
               	return refundAmount;
            }
    
    /**
     * 设置退款金额本币     *
     * 参数示例：<pre>{&quot;amount&quot;:0.1,&quot;cent&quot;:10,&quot;currencyCode&quot;:&quot;RUB&quot;,&quot;centFactor&quot;:100,&quot;currency&quot;:{&quot;defaultFractionDigits&quot;:2,&quot;currencyCode&quot;:&quot;RUB&quot;,&quot;symbol&quot;:&quot;RUB&quot;}</pre>     
             * 此参数必填
          */
    public void setRefundAmount(IssueMoney refundAmount) {
     	         	    this.refundAmount = refundAmount;
     	        }
    
        [DataMember(Order = 8)]
    private IssueMoney refundConfirmAmount;
    
        /**
       * @return 退款金额美元
    */
        public IssueMoney getRefundConfirmAmount() {
               	return refundConfirmAmount;
            }
    
    /**
     * 设置退款金额美元     *
     * 参数示例：<pre>{&quot;amount&quot;:0.01,&quot;cent&quot;:1,&quot;currencyCode&quot;:&quot;USD&quot;,&quot;centFactor&quot;:100,&quot;currency&quot;:{&quot;defaultFractionDigits&quot;:2,&quot;currencyCode&quot;:&quot;USD&quot;,&quot;symbol&quot;:&quot;$&quot;}</pre>     
             * 此参数必填
          */
    public void setRefundConfirmAmount(IssueMoney refundConfirmAmount) {
     	         	    this.refundConfirmAmount = refundConfirmAmount;
     	        }
    
        [DataMember(Order = 9)]
    private string actionType;
    
        /**
       * @return 操作
    */
        public string getActionType() {
               	return actionType;
            }
    
    /**
     * 设置操作     *
     * 参数示例：<pre>seller_accept</pre>     
             * 此参数必填
          */
    public void setActionType(string actionType) {
     	         	    this.actionType = actionType;
     	        }
    
        [DataMember(Order = 10)]
    private string submitMemberType;
    
        /**
       * @return 操作人类型
seller 卖家
buyer 买家
system 系统
    */
        public string getSubmitMemberType() {
               	return submitMemberType;
            }
    
    /**
     * 设置操作人类型
seller 卖家
buyer 买家
system 系统     *
     * 参数示例：<pre>seller</pre>     
             * 此参数必填
          */
    public void setSubmitMemberType(string submitMemberType) {
     	         	    this.submitMemberType = submitMemberType;
     	        }
    
        [DataMember(Order = 11)]
    private string[] attachments;
    
        /**
       * @return 附件列表
    */
        public string[] getAttachments() {
               	return attachments;
            }
    
    /**
     * 设置附件列表     *
     * 参数示例：<pre>[&quot;http://g02.a.alicdn.com/kf/UT8B.pjXtxbXXcUQpbXm.png&quot;]}]</pre>     
             * 此参数必填
          */
    public void setAttachments(string[] attachments) {
     	         	    this.attachments = attachments;
     	        }
    
        [DataMember(Order = 12)]
    private string isReceivedGoods;
    
        /**
       * @return 是否收到货
Y
N
    */
        public string getIsReceivedGoods() {
               	return isReceivedGoods;
            }
    
    /**
     * 设置是否收到货
Y
N     *
     * 参数示例：<pre>Y</pre>     
             * 此参数必填
          */
    public void setIsReceivedGoods(string isReceivedGoods) {
     	         	    this.isReceivedGoods = isReceivedGoods;
     	        }
    
        [DataMember(Order = 13)]
    private string[] videos;
    
        /**
       * @return 视频列表
    */
        public string[] getVideos() {
               	return videos;
            }
    
    /**
     * 设置视频列表     *
     * 参数示例：<pre>[&quot;http://cloud.video.taobao.com/play/u/133146836577/p/1/e/1/t/1/d/hd/fv/27046845.swf&quot;]}]</pre>     
             * 此参数必填
          */
    public void setVideos(string[] videos) {
     	         	    this.videos = videos;
     	        }
    
        [DataMember(Order = 14)]
    private IssueRefundSuggestionDTO[] issueRefundSuggestionList;
    
        /**
       * @return 纠纷协商方案
    */
        public IssueRefundSuggestionDTO[] getIssueRefundSuggestionList() {
               	return issueRefundSuggestionList;
            }
    
    /**
     * 设置纠纷协商方案     *
     * 参数示例：<pre>{&quot;isDefault&quot;:true,&quot;issueMoney&quot;:{&quot;amount&quot;:74.47,&quot;cent&quot;:7447,&quot;centFactor&quot;:100,&quot;currency&quot;:{&quot;currencyCode&quot;:&quot;RUB&quot;,&quot;symbol&quot;:&quot;RUB&quot;},&quot;currencyCode&quot;:&quot;RUB&quot;},&quot;issueMoneyPost&quot;:{&quot;amount&quot;:1.42,&quot;cent&quot;:142,&quot;centFactor&quot;:100,&quot;currency&quot;:{&quot;currencyCode&quot;:&quot;USD&quot;,&quot;symbol&quot;:&quot;$&quot;},&quot;currencyCode&quot;:&quot;USD&quot;},&quot;issueRefundType&quot;:&quot;full_amount_refund&quot;,&quot;issueReturnGoods&quot;:false}</pre>     
             * 此参数必填
          */
    public void setIssueRefundSuggestionList(IssueRefundSuggestionDTO[] issueRefundSuggestionList) {
     	         	    this.issueRefundSuggestionList = issueRefundSuggestionList;
     	        }
    
    
  }
}