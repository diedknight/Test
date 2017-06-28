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
public class IssueRefundSuggestionDTO {

       [DataMember(Order = 1)]
    private IssueMoney issueMoney;
    
        /**
       * @return 退款金额本币
    */
        public IssueMoney getIssueMoney() {
               	return issueMoney;
            }
    
    /**
     * 设置退款金额本币     *
     * 参数示例：<pre>1120 RUB</pre>     
             * 此参数必填
          */
    public void setIssueMoney(IssueMoney issueMoney) {
     	         	    this.issueMoney = issueMoney;
     	        }
    
        [DataMember(Order = 2)]
    private IssueMoney issueMoneyPost;
    
        /**
       * @return 退款金额美金
    */
        public IssueMoney getIssueMoneyPost() {
               	return issueMoneyPost;
            }
    
    /**
     * 设置退款金额美金     *
     * 参数示例：<pre>56 USD</pre>     
             * 此参数必填
          */
    public void setIssueMoneyPost(IssueMoney issueMoneyPost) {
     	         	    this.issueMoneyPost = issueMoneyPost;
     	        }
    
        [DataMember(Order = 3)]
    private string issueRefundType;
    
        /**
       * @return 退款类型:
1.full_amount_refund  全额退款
2.part_amount_refund 部分退款
3.no_amount_refund 不退款
    */
        public string getIssueRefundType() {
               	return issueRefundType;
            }
    
    /**
     * 设置退款类型:
1.full_amount_refund  全额退款
2.part_amount_refund 部分退款
3.no_amount_refund 不退款     *
     * 参数示例：<pre>full_amount_refund</pre>     
             * 此参数必填
          */
    public void setIssueRefundType(string issueRefundType) {
     	         	    this.issueRefundType = issueRefundType;
     	        }
    
        [DataMember(Order = 4)]
    private bool? issueReturnGoods;
    
        /**
       * @return 是否退货
    */
        public bool? getIssueReturnGoods() {
               	return issueReturnGoods;
            }
    
    /**
     * 设置是否退货     *
     * 参数示例：<pre>true</pre>     
             * 此参数必填
          */
    public void setIssueReturnGoods(bool issueReturnGoods) {
     	         	    this.issueReturnGoods = issueReturnGoods;
     	        }
    
        [DataMember(Order = 5)]
    private bool? isDefault;
    
        /**
       * @return 是否默认方案
    */
        public bool? getIsDefault() {
               	return isDefault;
            }
    
    /**
     * 设置是否默认方案     *
     * 参数示例：<pre>true</pre>     
             * 此参数必填
          */
    public void setIsDefault(bool isDefault) {
     	         	    this.isDefault = isDefault;
     	        }
    
    
  }
}