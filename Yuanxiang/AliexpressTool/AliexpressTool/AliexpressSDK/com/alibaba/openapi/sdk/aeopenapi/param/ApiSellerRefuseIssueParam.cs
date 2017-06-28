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
public class ApiSellerRefuseIssueParam {

       [DataMember(Order = 1)]
    private long? issueId;
    
        /**
       * @return 纠纷ID
    */
        public long? getIssueId() {
               	return issueId;
            }
    
    /**
     * 设置纠纷ID     *
     * 参数示例：<pre>60573020065804</pre>     
             * 此参数必填
          */
    public void setIssueId(long issueId) {
     	         	    this.issueId = issueId;
     	        }
    
        [DataMember(Order = 2)]
    private string refundType;
    
        /**
       * @return 退款类型（full_amount_refund(全额退款)/part_amount_refund(部分退款)/no_amount_refund(不退款)
    */
        public string getRefundType() {
               	return refundType;
            }
    
    /**
     * 设置退款类型（full_amount_refund(全额退款)/part_amount_refund(部分退款)/no_amount_refund(不退款)     *
     * 参数示例：<pre>full_amount_refund</pre>     
             * 此参数必填
          */
    public void setRefundType(string refundType) {
     	         	    this.refundType = refundType;
     	        }
    
        [DataMember(Order = 3)]
    private string isReturnGoods;
    
        /**
       * @return 是否退货（Y/N）
    */
        public string getIsReturnGoods() {
               	return isReturnGoods;
            }
    
    /**
     * 设置是否退货（Y/N）     *
     * 参数示例：<pre>Y</pre>     
             * 此参数必填
          */
    public void setIsReturnGoods(string isReturnGoods) {
     	         	    this.isReturnGoods = isReturnGoods;
     	        }
    
        [DataMember(Order = 4)]
    private string refundAmount;
    
        /**
       * @return 退款金额（单位：分，保修两位小数，币种：USD）
    */
        public string getRefundAmount() {
               	return refundAmount;
            }
    
    /**
     * 设置退款金额（单位：分，保修两位小数，币种：USD）     *
     * 参数示例：<pre>5800</pre>     
             * 此参数必填
          */
    public void setRefundAmount(string refundAmount) {
     	         	    this.refundAmount = refundAmount;
     	        }
    
        [DataMember(Order = 5)]
    private string content;
    
        /**
       * @return 拒绝买家纠纷方案的原因描述
    */
        public string getContent() {
               	return content;
            }
    
    /**
     * 设置拒绝买家纠纷方案的原因描述     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setContent(string content) {
     	         	    this.content = content;
     	        }
    
        [DataMember(Order = 6)]
    private string attachments;
    
        /**
       * @return 图片附件，上传多张图片请以半角逗号“,”进行分隔上传
    */
        public string getAttachments() {
               	return attachments;
            }
    
    /**
     * 设置图片附件，上传多张图片请以半角逗号“,”进行分隔上传     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAttachments(string attachments) {
     	         	    this.attachments = attachments;
     	        }
    
    
  }
}