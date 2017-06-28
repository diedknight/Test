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
public class OrderItemVO {

       [DataMember(Order = 1)]
    private string bizType;
    
        /**
       * @return 订单类型
    */
        public string getBizType() {
               	return bizType;
            }
    
    /**
     * 设置订单类型     *
     * 参数示例：<pre>AE_COMMON</pre>     
             * 此参数必填
          */
    public void setBizType(string bizType) {
     	         	    this.bizType = bizType;
     	        }
    
        [DataMember(Order = 2)]
    private string buyerLoginId;
    
        /**
       * @return 买家登录ID
    */
        public string getBuyerLoginId() {
               	return buyerLoginId;
            }
    
    /**
     * 设置买家登录ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBuyerLoginId(string buyerLoginId) {
     	         	    this.buyerLoginId = buyerLoginId;
     	        }
    
        [DataMember(Order = 3)]
    private string buyerSignerFullname;
    
        /**
       * @return 买家全名
    */
        public string getBuyerSignerFullname() {
               	return buyerSignerFullname;
            }
    
    /**
     * 设置买家全名     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBuyerSignerFullname(string buyerSignerFullname) {
     	         	    this.buyerSignerFullname = buyerSignerFullname;
     	        }
    
        [DataMember(Order = 4)]
    private int? escrowFeeRate;
    
        /**
       * @return 手续费率
    */
        public int? getEscrowFeeRate() {
               	return escrowFeeRate;
            }
    
    /**
     * 设置手续费率     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setEscrowFeeRate(int escrowFeeRate) {
     	         	    this.escrowFeeRate = escrowFeeRate;
     	        }
    
        [DataMember(Order = 5)]
    private string frozenStatus;
    
        /**
       * @return 冻结状态
    */
        public string getFrozenStatus() {
               	return frozenStatus;
            }
    
    /**
     * 设置冻结状态     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFrozenStatus(string frozenStatus) {
     	         	    this.frozenStatus = frozenStatus;
     	        }
    
        [DataMember(Order = 6)]
    private string fundStatus;
    
        /**
       * @return 资金状态
    */
        public string getFundStatus() {
               	return fundStatus;
            }
    
    /**
     * 设置资金状态     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFundStatus(string fundStatus) {
     	         	    this.fundStatus = fundStatus;
     	        }
    
        [DataMember(Order = 7)]
    private string gmtCreate;
    
        /**
       * @return 订单创建时间
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
     * 设置订单创建时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGmtCreate(DateTime gmtCreate) {
     	         	    this.gmtCreate = DateUtil.format(gmtCreate);
     	        }
    
        [DataMember(Order = 8)]
    private string gmtPayTime;
    
        /**
       * @return 支付时间（和订单详情中gmtPaysuccess字段意义相同。)
    */
        public DateTime? getGmtPayTime() {
                 if (gmtPayTime != null)
          {
              DateTime datetime = DateUtil.formatFromStr(gmtPayTime);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置支付时间（和订单详情中gmtPaysuccess字段意义相同。)     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGmtPayTime(DateTime gmtPayTime) {
     	         	    this.gmtPayTime = DateUtil.format(gmtPayTime);
     	        }
    
        [DataMember(Order = 9)]
    private string gmtSendGoodsTime;
    
        /**
       * @return 发货时间
    */
        public DateTime? getGmtSendGoodsTime() {
                 if (gmtSendGoodsTime != null)
          {
              DateTime datetime = DateUtil.formatFromStr(gmtSendGoodsTime);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置发货时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGmtSendGoodsTime(DateTime gmtSendGoodsTime) {
     	         	    this.gmtSendGoodsTime = DateUtil.format(gmtSendGoodsTime);
     	        }
    
        [DataMember(Order = 10)]
    private bool? hasRequestLoan;
    
        /**
       * @return 是否已请求放款
    */
        public bool? getHasRequestLoan() {
               	return hasRequestLoan;
            }
    
    /**
     * 设置是否已请求放款     *
     * 参数示例：<pre>false</pre>     
             * 此参数必填
          */
    public void setHasRequestLoan(bool hasRequestLoan) {
     	         	    this.hasRequestLoan = hasRequestLoan;
     	        }
    
        [DataMember(Order = 11)]
    private string issueStatus;
    
        /**
       * @return 纠纷状态
    */
        public string getIssueStatus() {
               	return issueStatus;
            }
    
    /**
     * 设置纠纷状态     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setIssueStatus(string issueStatus) {
     	         	    this.issueStatus = issueStatus;
     	        }
    
        [DataMember(Order = 12)]
    private string leftSendGoodDay;
    
        /**
       * @return 剩余发货时间（天）
    */
        public string getLeftSendGoodDay() {
               	return leftSendGoodDay;
            }
    
    /**
     * 设置剩余发货时间（天）     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLeftSendGoodDay(string leftSendGoodDay) {
     	         	    this.leftSendGoodDay = leftSendGoodDay;
     	        }
    
        [DataMember(Order = 13)]
    private string leftSendGoodHour;
    
        /**
       * @return 剩余发货时间（小时）
    */
        public string getLeftSendGoodHour() {
               	return leftSendGoodHour;
            }
    
    /**
     * 设置剩余发货时间（小时）     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLeftSendGoodHour(string leftSendGoodHour) {
     	         	    this.leftSendGoodHour = leftSendGoodHour;
     	        }
    
        [DataMember(Order = 14)]
    private string leftSendGoodMin;
    
        /**
       * @return 剩余发货时间（分钟）
    */
        public string getLeftSendGoodMin() {
               	return leftSendGoodMin;
            }
    
    /**
     * 设置剩余发货时间（分钟）     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLeftSendGoodMin(string leftSendGoodMin) {
     	         	    this.leftSendGoodMin = leftSendGoodMin;
     	        }
    
        [DataMember(Order = 15)]
    private string logisticsStatus;
    
        /**
       * @return 物流状态
    */
        public string getLogisticsStatus() {
               	return logisticsStatus;
            }
    
    /**
     * 设置物流状态     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLogisticsStatus(string logisticsStatus) {
     	         	    this.logisticsStatus = logisticsStatus;
     	        }
    
        [DataMember(Order = 16)]
    private string orderDetailUrl;
    
        /**
       * @return 订单详情链接
    */
        public string getOrderDetailUrl() {
               	return orderDetailUrl;
            }
    
    /**
     * 设置订单详情链接     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderDetailUrl(string orderDetailUrl) {
     	         	    this.orderDetailUrl = orderDetailUrl;
     	        }
    
        [DataMember(Order = 17)]
    private long? orderId;
    
        /**
       * @return 订单ID
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置订单ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 18)]
    private string orderStatus;
    
        /**
       * @return 订单状态
    */
        public string getOrderStatus() {
               	return orderStatus;
            }
    
    /**
     * 设置订单状态     *
     * 参数示例：<pre>PLACE_ORDER_SUCCESS</pre>     
             * 此参数必填
          */
    public void setOrderStatus(string orderStatus) {
     	         	    this.orderStatus = orderStatus;
     	        }
    
        [DataMember(Order = 19)]
    private string paymentType;
    
        /**
       * @return 支付类型
    */
        public string getPaymentType() {
               	return paymentType;
            }
    
    /**
     * 设置支付类型     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPaymentType(string paymentType) {
     	         	    this.paymentType = paymentType;
     	        }
    
        [DataMember(Order = 20)]
    private string sellerLoginId;
    
        /**
       * @return 卖家登录ID
    */
        public string getSellerLoginId() {
               	return sellerLoginId;
            }
    
    /**
     * 设置卖家登录ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSellerLoginId(string sellerLoginId) {
     	         	    this.sellerLoginId = sellerLoginId;
     	        }
    
        [DataMember(Order = 21)]
    private string sellerSignerFullname;
    
        /**
       * @return 卖家全名
    */
        public string getSellerSignerFullname() {
               	return sellerSignerFullname;
            }
    
    /**
     * 设置卖家全名     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSellerSignerFullname(string sellerSignerFullname) {
     	         	    this.sellerSignerFullname = sellerSignerFullname;
     	        }
    
        [DataMember(Order = 22)]
    private long? timeoutLeftTime;
    
        /**
       * @return 超时剩余时间
    */
        public long? getTimeoutLeftTime() {
               	return timeoutLeftTime;
            }
    
    /**
     * 设置超时剩余时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setTimeoutLeftTime(long timeoutLeftTime) {
     	         	    this.timeoutLeftTime = timeoutLeftTime;
     	        }
    
        [DataMember(Order = 23)]
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
    
        [DataMember(Order = 24)]
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
    
        [DataMember(Order = 25)]
    private TradeMoney payAmount;
    
        /**
       * @return 付款金额
    */
        public TradeMoney getPayAmount() {
               	return payAmount;
            }
    
    /**
     * 设置付款金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPayAmount(TradeMoney payAmount) {
     	         	    this.payAmount = payAmount;
     	        }
    
        [DataMember(Order = 26)]
    private OrderProductVO[] productList;
    
        /**
       * @return 商品列表
    */
        public OrderProductVO[] getProductList() {
               	return productList;
            }
    
    /**
     * 设置商品列表     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductList(OrderProductVO[] productList) {
     	         	    this.productList = productList;
     	        }
    
    
  }
}