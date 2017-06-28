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
public class SimpleOrderProductVO {

       [DataMember(Order = 1)]
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
    
        [DataMember(Order = 2)]
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
    
        [DataMember(Order = 3)]
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
    
        [DataMember(Order = 4)]
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
    
        [DataMember(Order = 5)]
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
    
        [DataMember(Order = 6)]
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
    
        [DataMember(Order = 7)]
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
    
        [DataMember(Order = 8)]
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
    
        [DataMember(Order = 9)]
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
    
        [DataMember(Order = 10)]
    private string productUnitPrice;
    
        /**
       * @return 商品单价
    */
        public string getProductUnitPrice() {
               	return productUnitPrice;
            }
    
    /**
     * 设置商品单价     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductUnitPrice(string productUnitPrice) {
     	         	    this.productUnitPrice = productUnitPrice;
     	        }
    
        [DataMember(Order = 11)]
    private string productUnitPriceCur;
    
        /**
       * @return 商品货币名称
    */
        public string getProductUnitPriceCur() {
               	return productUnitPriceCur;
            }
    
    /**
     * 设置商品货币名称     *
     * 参数示例：<pre>USD</pre>     
             * 此参数必填
          */
    public void setProductUnitPriceCur(string productUnitPriceCur) {
     	         	    this.productUnitPriceCur = productUnitPriceCur;
     	        }
    
        [DataMember(Order = 12)]
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
    
        [DataMember(Order = 13)]
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
    
    
  }
}