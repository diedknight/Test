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
public class ChildOrderDTO {

       [DataMember(Order = 1)]
    private string frozenStatus;
    
        /**
       * @return 冻结状态(&quot;NO_FROZEN&quot;无冻结；&quot;IN_FROZEN&quot;冻结中；)
    */
        public string getFrozenStatus() {
               	return frozenStatus;
            }
    
    /**
     * 设置冻结状态(&quot;NO_FROZEN&quot;无冻结；&quot;IN_FROZEN&quot;冻结中；)     *
     * 参数示例：<pre>NO_FROZEN</pre>     
             * 此参数必填
          */
    public void setFrozenStatus(string frozenStatus) {
     	         	    this.frozenStatus = frozenStatus;
     	        }
    
        [DataMember(Order = 2)]
    private string fundStatus;
    
        /**
       * @return 资金状态(NOT_PAY,未付款； PAY_SUCCESS,付款成功； WAIT_SELLER_CHECK，卖家验款)
    */
        public string getFundStatus() {
               	return fundStatus;
            }
    
    /**
     * 设置资金状态(NOT_PAY,未付款； PAY_SUCCESS,付款成功； WAIT_SELLER_CHECK，卖家验款)     *
     * 参数示例：<pre>NOT_PAY</pre>     
             * 此参数必填
          */
    public void setFundStatus(string fundStatus) {
     	         	    this.fundStatus = fundStatus;
     	        }
    
        [DataMember(Order = 3)]
    private long? id;
    
        /**
       * @return 子订单ID
    */
        public long? getId() {
               	return id;
            }
    
    /**
     * 设置子订单ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setId(long id) {
     	         	    this.id = id;
     	        }
    
        [DataMember(Order = 4)]
    private string initOrderAmt;
    
        /**
       * @return 子订单初始金额
    */
        public string getInitOrderAmt() {
               	return initOrderAmt;
            }
    
    /**
     * 设置子订单初始金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setInitOrderAmt(string initOrderAmt) {
     	         	    this.initOrderAmt = initOrderAmt;
     	        }
    
        [DataMember(Order = 5)]
    private string initOrderAmtCur;
    
        /**
       * @return 货币单位
    */
        public string getInitOrderAmtCur() {
               	return initOrderAmtCur;
            }
    
    /**
     * 设置货币单位     *
     * 参数示例：<pre>USD</pre>     
             * 此参数必填
          */
    public void setInitOrderAmtCur(string initOrderAmtCur) {
     	         	    this.initOrderAmtCur = initOrderAmtCur;
     	        }
    
        [DataMember(Order = 6)]
    private string issueStatus;
    
        /**
       * @return 纠纷状态(&quot;NO_ISSUE&quot;无纠纷；&quot;IN_ISSUE&quot;纠纷中；&ldquo;END_ISSUE&rdquo;纠纷结束。) frozenStatus:冻结状态(&quot;NO_FROZEN&quot;无冻结；&quot;IN_FROZEN&quot;冻结中；)
    */
        public string getIssueStatus() {
               	return issueStatus;
            }
    
    /**
     * 设置纠纷状态(&quot;NO_ISSUE&quot;无纠纷；&quot;IN_ISSUE&quot;纠纷中；&ldquo;END_ISSUE&rdquo;纠纷结束。) frozenStatus:冻结状态(&quot;NO_FROZEN&quot;无冻结；&quot;IN_FROZEN&quot;冻结中；)     *
     * 参数示例：<pre>NO_ISSUE</pre>     
             * 此参数必填
          */
    public void setIssueStatus(string issueStatus) {
     	         	    this.issueStatus = issueStatus;
     	        }
    
        [DataMember(Order = 7)]
    private int? lotNum;
    
        /**
       * @return lot数量
    */
        public int? getLotNum() {
               	return lotNum;
            }
    
    /**
     * 设置lot数量     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setLotNum(int lotNum) {
     	         	    this.lotNum = lotNum;
     	        }
    
        [DataMember(Order = 8)]
    private string orderStatus;
    
        /**
       * @return 子订单状态
    */
        public string getOrderStatus() {
               	return orderStatus;
            }
    
    /**
     * 设置子订单状态     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderStatus(string orderStatus) {
     	         	    this.orderStatus = orderStatus;
     	        }
    
        [DataMember(Order = 9)]
    private string productAttributes;
    
        /**
       * @return 商品属性
    */
        public string getProductAttributes() {
               	return productAttributes;
            }
    
    /**
     * 设置商品属性     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductAttributes(string productAttributes) {
     	         	    this.productAttributes = productAttributes;
     	        }
    
        [DataMember(Order = 10)]
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
    
        [DataMember(Order = 11)]
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
    
        [DataMember(Order = 12)]
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
    
        [DataMember(Order = 13)]
    private string productPrice;
    
        /**
       * @return 商品价格
    */
        public string getProductPrice() {
               	return productPrice;
            }
    
    /**
     * 设置商品价格     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductPrice(string productPrice) {
     	         	    this.productPrice = productPrice;
     	        }
    
        [DataMember(Order = 14)]
    private string productPriceCur;
    
        /**
       * @return 货币单位
    */
        public string getProductPriceCur() {
               	return productPriceCur;
            }
    
    /**
     * 设置货币单位     *
     * 参数示例：<pre>USD</pre>     
             * 此参数必填
          */
    public void setProductPriceCur(string productPriceCur) {
     	         	    this.productPriceCur = productPriceCur;
     	        }
    
        [DataMember(Order = 15)]
    private string productStandard;
    
        /**
       * @return 产品规格
    */
        public string getProductStandard() {
               	return productStandard;
            }
    
    /**
     * 设置产品规格     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductStandard(string productStandard) {
     	         	    this.productStandard = productStandard;
     	        }
    
        [DataMember(Order = 16)]
    private string productUnit;
    
        /**
       * @return 产品单位
    */
        public string getProductUnit() {
               	return productUnit;
            }
    
    /**
     * 设置产品单位     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductUnit(string productUnit) {
     	         	    this.productUnit = productUnit;
     	        }
    
        [DataMember(Order = 17)]
    private string skuCode;
    
        /**
       * @return SKU编码
    */
        public string getSkuCode() {
               	return skuCode;
            }
    
    /**
     * 设置SKU编码     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSkuCode(string skuCode) {
     	         	    this.skuCode = skuCode;
     	        }
    
    
  }
}