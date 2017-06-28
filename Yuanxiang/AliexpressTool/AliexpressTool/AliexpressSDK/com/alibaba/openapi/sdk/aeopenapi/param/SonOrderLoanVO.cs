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
public class SonOrderLoanVO {

       [DataMember(Order = 1)]
    private long? childOrderId;
    
        /**
       * @return 子订单ID
    */
        public long? getChildOrderId() {
               	return childOrderId;
            }
    
    /**
     * 设置子订单ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setChildOrderId(long childOrderId) {
     	         	    this.childOrderId = childOrderId;
     	        }
    
        [DataMember(Order = 2)]
    private string loanStatus;
    
        /**
       * @return 放款状态
    */
        public string getLoanStatus() {
               	return loanStatus;
            }
    
    /**
     * 设置放款状态     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLoanStatus(string loanStatus) {
     	         	    this.loanStatus = loanStatus;
     	        }
    
        [DataMember(Order = 3)]
    private string releasedDatetime;
    
        /**
       * @return 放款时间
    */
        public DateTime? getReleasedDatetime() {
                 if (releasedDatetime != null)
          {
              DateTime datetime = DateUtil.formatFromStr(releasedDatetime);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置放款时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setReleasedDatetime(DateTime releasedDatetime) {
     	         	    this.releasedDatetime = DateUtil.format(releasedDatetime);
     	        }
    
        [DataMember(Order = 4)]
    private string waitLoanReson;
    
        /**
       * @return 待放款原因
    */
        public string getWaitLoanReson() {
               	return waitLoanReson;
            }
    
    /**
     * 设置待放款原因     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setWaitLoanReson(string waitLoanReson) {
     	         	    this.waitLoanReson = waitLoanReson;
     	        }
    
        [DataMember(Order = 5)]
    private TradeMoney affiliateCommission;
    
        /**
       * @return 联盟佣金
    */
        public TradeMoney getAffiliateCommission() {
               	return affiliateCommission;
            }
    
    /**
     * 设置联盟佣金     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAffiliateCommission(TradeMoney affiliateCommission) {
     	         	    this.affiliateCommission = affiliateCommission;
     	        }
    
        [DataMember(Order = 6)]
    private TradeMoney amount;
    
        /**
       * @return 放款金额(已废弃)
    */
        public TradeMoney getAmount() {
               	return amount;
            }
    
    /**
     * 设置放款金额(已废弃)     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAmount(TradeMoney amount) {
     	         	    this.amount = amount;
     	        }
    
        [DataMember(Order = 7)]
    private TradeMoney escrowFee;
    
        /**
       * @return 手续费
    */
        public TradeMoney getEscrowFee() {
               	return escrowFee;
            }
    
    /**
     * 设置手续费     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setEscrowFee(TradeMoney escrowFee) {
     	         	    this.escrowFee = escrowFee;
     	        }
    
        [DataMember(Order = 8)]
    private TradeMoney loanAmount;
    
        /**
       * @return 放款金额
    */
        public TradeMoney getLoanAmount() {
               	return loanAmount;
            }
    
    /**
     * 设置放款金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLoanAmount(TradeMoney loanAmount) {
     	         	    this.loanAmount = loanAmount;
     	        }
    
        [DataMember(Order = 9)]
    private TradeMoney realLoanAmount;
    
        /**
       * @return 实际放款出账金额
    */
        public TradeMoney getRealLoanAmount() {
               	return realLoanAmount;
            }
    
    /**
     * 设置实际放款出账金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setRealLoanAmount(TradeMoney realLoanAmount) {
     	         	    this.realLoanAmount = realLoanAmount;
     	        }
    
        [DataMember(Order = 10)]
    private TradeMoney realRefundAmount;
    
        /**
       * @return 实际退款出账金额
    */
        public TradeMoney getRealRefundAmount() {
               	return realRefundAmount;
            }
    
    /**
     * 设置实际退款出账金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setRealRefundAmount(TradeMoney realRefundAmount) {
     	         	    this.realRefundAmount = realRefundAmount;
     	        }
    
        [DataMember(Order = 11)]
    private TradeMoney refundAmount;
    
        /**
       * @return 退款金额
    */
        public TradeMoney getRefundAmount() {
               	return refundAmount;
            }
    
    /**
     * 设置退款金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setRefundAmount(TradeMoney refundAmount) {
     	         	    this.refundAmount = refundAmount;
     	        }
    
    
  }
}