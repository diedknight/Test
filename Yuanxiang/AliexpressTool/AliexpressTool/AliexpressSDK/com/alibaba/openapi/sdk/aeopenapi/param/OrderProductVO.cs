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
public class OrderProductVO {

       [DataMember(Order = 1)]
    private string buyerSignerFirstName;
    
        /**
       * @return 买家firstName
    */
        public string getBuyerSignerFirstName() {
               	return buyerSignerFirstName;
            }
    
    /**
     * 设置买家firstName     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBuyerSignerFirstName(string buyerSignerFirstName) {
     	         	    this.buyerSignerFirstName = buyerSignerFirstName;
     	        }
    
        [DataMember(Order = 2)]
    private string buyerSignerLastName;
    
        /**
       * @return 买家lastName
    */
        public string getBuyerSignerLastName() {
               	return buyerSignerLastName;
            }
    
    /**
     * 设置买家lastName     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBuyerSignerLastName(string buyerSignerLastName) {
     	         	    this.buyerSignerLastName = buyerSignerLastName;
     	        }
    
        [DataMember(Order = 3)]
    private bool? canSubmitIssue;
    
        /**
       * @return 子订单是否能提交纠纷
    */
        public bool? getCanSubmitIssue() {
               	return canSubmitIssue;
            }
    
    /**
     * 设置子订单是否能提交纠纷     *
     * 参数示例：<pre>false</pre>     
             * 此参数必填
          */
    public void setCanSubmitIssue(bool canSubmitIssue) {
     	         	    this.canSubmitIssue = canSubmitIssue;
     	        }
    
        [DataMember(Order = 4)]
    private long? childId;
    
        /**
       * @return 子订单号
    */
        public long? getChildId() {
               	return childId;
            }
    
    /**
     * 设置子订单号     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setChildId(long childId) {
     	         	    this.childId = childId;
     	        }
    
        [DataMember(Order = 5)]
    private string deliveryTime;
    
        /**
       * @return 妥投时间
    */
        public string getDeliveryTime() {
               	return deliveryTime;
            }
    
    /**
     * 设置妥投时间     *
     * 参数示例：<pre>5-10</pre>     
             * 此参数必填
          */
    public void setDeliveryTime(string deliveryTime) {
     	         	    this.deliveryTime = deliveryTime;
     	        }
    
        [DataMember(Order = 6)]
    private string freightCommitDay;
    
        /**
       * @return 限时达
    */
        public string getFreightCommitDay() {
               	return freightCommitDay;
            }
    
    /**
     * 设置限时达     *
     * 参数示例：<pre>27</pre>     
             * 此参数必填
          */
    public void setFreightCommitDay(string freightCommitDay) {
     	         	    this.freightCommitDay = freightCommitDay;
     	        }
    
        [DataMember(Order = 7)]
    private string fundStatus;
    
        /**
       * @return 资金状态
    */
        public string getFundStatus() {
               	return fundStatus;
            }
    
    /**
     * 设置资金状态     *
     * 参数示例：<pre>NOT_PAY</pre>     
             * 此参数必填
          */
    public void setFundStatus(string fundStatus) {
     	         	    this.fundStatus = fundStatus;
     	        }
    
        [DataMember(Order = 8)]
    private int? goodsPrepareTime;
    
        /**
       * @return 备货时间
    */
        public int? getGoodsPrepareTime() {
               	return goodsPrepareTime;
            }
    
    /**
     * 设置备货时间     *
     * 参数示例：<pre>30</pre>     
             * 此参数必填
          */
    public void setGoodsPrepareTime(int goodsPrepareTime) {
     	         	    this.goodsPrepareTime = goodsPrepareTime;
     	        }
    
        [DataMember(Order = 9)]
    private string issueMode;
    
        /**
       * @return 纠纷类型
    */
        public string getIssueMode() {
               	return issueMode;
            }
    
    /**
     * 设置纠纷类型     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setIssueMode(string issueMode) {
     	         	    this.issueMode = issueMode;
     	        }
    
        [DataMember(Order = 10)]
    private string issueStatus;
    
        /**
       * @return 纠纷状态
    */
        public string getIssueStatus() {
               	return issueStatus;
            }
    
    /**
     * 设置纠纷状态     *
     * 参数示例：<pre>NO_ISSUE</pre>     
             * 此参数必填
          */
    public void setIssueStatus(string issueStatus) {
     	         	    this.issueStatus = issueStatus;
     	        }
    
        [DataMember(Order = 11)]
    private string logisticsServiceName;
    
        /**
       * @return 物流服务
    */
        public string getLogisticsServiceName() {
               	return logisticsServiceName;
            }
    
    /**
     * 设置物流服务     *
     * 参数示例：<pre>EMS</pre>     
             * 此参数必填
          */
    public void setLogisticsServiceName(string logisticsServiceName) {
     	         	    this.logisticsServiceName = logisticsServiceName;
     	        }
    
        [DataMember(Order = 12)]
    private string logisticsType;
    
        /**
       * @return 物流类型
    */
        public string getLogisticsType() {
               	return logisticsType;
            }
    
    /**
     * 设置物流类型     *
     * 参数示例：<pre>EMS</pre>     
             * 此参数必填
          */
    public void setLogisticsType(string logisticsType) {
     	         	    this.logisticsType = logisticsType;
     	        }
    
        [DataMember(Order = 13)]
    private string memo;
    
        /**
       * @return 订单备注
    */
        public string getMemo() {
               	return memo;
            }
    
    /**
     * 设置订单备注     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMemo(string memo) {
     	         	    this.memo = memo;
     	        }
    
        [DataMember(Order = 14)]
    private bool? moneyBack3x;
    
        /**
       * @return 是否支持假一赔三
    */
        public bool? getMoneyBack3x() {
               	return moneyBack3x;
            }
    
    /**
     * 设置是否支持假一赔三     *
     * 参数示例：<pre>false</pre>     
             * 此参数必填
          */
    public void setMoneyBack3x(bool moneyBack3x) {
     	         	    this.moneyBack3x = moneyBack3x;
     	        }
    
        [DataMember(Order = 15)]
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
    
        [DataMember(Order = 16)]
    private int? productCount;
    
        /**
       * @return 商品数量
    */
        public int? getProductCount() {
               	return productCount;
            }
    
    /**
     * 设置商品数量     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductCount(int productCount) {
     	         	    this.productCount = productCount;
     	        }
    
        [DataMember(Order = 17)]
    private long? productId;
    
        /**
       * @return 商品ID
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置商品ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 18)]
    private string productImgUrl;
    
        /**
       * @return 商品主图Url
    */
        public string getProductImgUrl() {
               	return productImgUrl;
            }
    
    /**
     * 设置商品主图Url     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductImgUrl(string productImgUrl) {
     	         	    this.productImgUrl = productImgUrl;
     	        }
    
        [DataMember(Order = 19)]
    private string productName;
    
        /**
       * @return 商品名称
    */
        public string getProductName() {
               	return productName;
            }
    
    /**
     * 设置商品名称     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductName(string productName) {
     	         	    this.productName = productName;
     	        }
    
        [DataMember(Order = 20)]
    private string productSnapUrl;
    
        /**
       * @return 快照Url
    */
        public string getProductSnapUrl() {
               	return productSnapUrl;
            }
    
    /**
     * 设置快照Url     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductSnapUrl(string productSnapUrl) {
     	         	    this.productSnapUrl = productSnapUrl;
     	        }
    
        [DataMember(Order = 21)]
    private string productStandard;
    
        /**
       * @return 商品规格
    */
        public string getProductStandard() {
               	return productStandard;
            }
    
    /**
     * 设置商品规格     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductStandard(string productStandard) {
     	         	    this.productStandard = productStandard;
     	        }
    
        [DataMember(Order = 22)]
    private string productUnit;
    
        /**
       * @return 商品单位
    */
        public string getProductUnit() {
               	return productUnit;
            }
    
    /**
     * 设置商品单位     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductUnit(string productUnit) {
     	         	    this.productUnit = productUnit;
     	        }
    
        [DataMember(Order = 23)]
    private string sellerSignerFirstName;
    
        /**
       * @return 卖家firstName
    */
        public string getSellerSignerFirstName() {
               	return sellerSignerFirstName;
            }
    
    /**
     * 设置卖家firstName     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSellerSignerFirstName(string sellerSignerFirstName) {
     	         	    this.sellerSignerFirstName = sellerSignerFirstName;
     	        }
    
        [DataMember(Order = 24)]
    private string sellerSignerLastName;
    
        /**
       * @return 卖家lastName
    */
        public string getSellerSignerLastName() {
               	return sellerSignerLastName;
            }
    
    /**
     * 设置卖家lastName     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSellerSignerLastName(string sellerSignerLastName) {
     	         	    this.sellerSignerLastName = sellerSignerLastName;
     	        }
    
        [DataMember(Order = 25)]
    private string sendGoodsTime;
    
        /**
       * @return 发货时间
    */
        public DateTime? getSendGoodsTime() {
                 if (sendGoodsTime != null)
          {
              DateTime datetime = DateUtil.formatFromStr(sendGoodsTime);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置发货时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSendGoodsTime(DateTime sendGoodsTime) {
     	         	    this.sendGoodsTime = DateUtil.format(sendGoodsTime);
     	        }
    
        [DataMember(Order = 26)]
    private string showStatus;
    
        /**
       * @return 订单显示状态
    */
        public string getShowStatus() {
               	return showStatus;
            }
    
    /**
     * 设置订单显示状态     *
     * 参数示例：<pre>PLACE_ORDER_SUCCESS</pre>     
             * 此参数必填
          */
    public void setShowStatus(string showStatus) {
     	         	    this.showStatus = showStatus;
     	        }
    
        [DataMember(Order = 27)]
    private string skuCode;
    
        /**
       * @return 商品编码
    */
        public string getSkuCode() {
               	return skuCode;
            }
    
    /**
     * 设置商品编码     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSkuCode(string skuCode) {
     	         	    this.skuCode = skuCode;
     	        }
    
        [DataMember(Order = 28)]
    private string sonOrderStatus;
    
        /**
       * @return 子订单状态
    */
        public string getSonOrderStatus() {
               	return sonOrderStatus;
            }
    
    /**
     * 设置子订单状态     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSonOrderStatus(string sonOrderStatus) {
     	         	    this.sonOrderStatus = sonOrderStatus;
     	        }
    
        [DataMember(Order = 29)]
    private TradeMoney logisticsAmount;
    
        /**
       * @return 
    */
        public TradeMoney getLogisticsAmount() {
               	return logisticsAmount;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLogisticsAmount(TradeMoney logisticsAmount) {
     	         	    this.logisticsAmount = logisticsAmount;
     	        }
    
        [DataMember(Order = 30)]
    private TradeMoney productUnitPrice;
    
        /**
       * @return 
    */
        public TradeMoney getProductUnitPrice() {
               	return productUnitPrice;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductUnitPrice(TradeMoney productUnitPrice) {
     	         	    this.productUnitPrice = productUnitPrice;
     	        }
    
        [DataMember(Order = 31)]
    private TradeMoney totalProductAmount;
    
        /**
       * @return 
    */
        public TradeMoney getTotalProductAmount() {
               	return totalProductAmount;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setTotalProductAmount(TradeMoney totalProductAmount) {
     	         	    this.totalProductAmount = totalProductAmount;
     	        }
    
    
  }
}