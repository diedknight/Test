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
public class AeopWlDeclareProductDTO {

       [DataMember(Order = 1)]
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
    
        [DataMember(Order = 2)]
    private string categoryCnDesc;
    
        /**
       * @return 类目中文名称
    */
        public string getCategoryCnDesc() {
               	return categoryCnDesc;
            }
    
    /**
     * 设置类目中文名称     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCategoryCnDesc(string categoryCnDesc) {
     	         	    this.categoryCnDesc = categoryCnDesc;
     	        }
    
        [DataMember(Order = 3)]
    private string categoryEnDesc;
    
        /**
       * @return 类目英文名称
    */
        public string getCategoryEnDesc() {
               	return categoryEnDesc;
            }
    
    /**
     * 设置类目英文名称     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCategoryEnDesc(string categoryEnDesc) {
     	         	    this.categoryEnDesc = categoryEnDesc;
     	        }
    
        [DataMember(Order = 4)]
    private int? productNum;
    
        /**
       * @return 商品数量
    */
        public int? getProductNum() {
               	return productNum;
            }
    
    /**
     * 设置商品数量     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductNum(int productNum) {
     	         	    this.productNum = productNum;
     	        }
    
        [DataMember(Order = 5)]
    private double? productDeclareAmount;
    
        /**
       * @return 商品申报金额
    */
        public double? getProductDeclareAmount() {
               	return productDeclareAmount;
            }
    
    /**
     * 设置商品申报金额     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductDeclareAmount(double productDeclareAmount) {
     	         	    this.productDeclareAmount = productDeclareAmount;
     	        }
    
        [DataMember(Order = 6)]
    private double? productWeight;
    
        /**
       * @return 商品重量
    */
        public double? getProductWeight() {
               	return productWeight;
            }
    
    /**
     * 设置商品重量     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductWeight(double productWeight) {
     	         	    this.productWeight = productWeight;
     	        }
    
        [DataMember(Order = 7)]
    private byte? isContainsBattery;
    
        /**
       * @return 是否包含电池
    */
        public byte? getIsContainsBattery() {
               	return isContainsBattery;
            }
    
    /**
     * 设置是否包含电池     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setIsContainsBattery(byte isContainsBattery) {
     	         	    this.isContainsBattery = isContainsBattery;
     	        }
    
        [DataMember(Order = 8)]
    private long? scItemId;
    
        /**
       * @return 
    */
        public long? getScItemId() {
               	return scItemId;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setScItemId(long scItemId) {
     	         	    this.scItemId = scItemId;
     	        }
    
        [DataMember(Order = 9)]
    private string skuValue;
    
        /**
       * @return SKU名称
    */
        public string getSkuValue() {
               	return skuValue;
            }
    
    /**
     * 设置SKU名称     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSkuValue(string skuValue) {
     	         	    this.skuValue = skuValue;
     	        }
    
        [DataMember(Order = 10)]
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
    
        [DataMember(Order = 11)]
    private string scItemName;
    
        /**
       * @return 
    */
        public string getScItemName() {
               	return scItemName;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setScItemName(string scItemName) {
     	         	    this.scItemName = scItemName;
     	        }
    
        [DataMember(Order = 12)]
    private string scItemCode;
    
        /**
       * @return 
    */
        public string getScItemCode() {
               	return scItemCode;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setScItemCode(string scItemCode) {
     	         	    this.scItemCode = scItemCode;
     	        }
    
        [DataMember(Order = 13)]
    private string hsCode;
    
        /**
       * @return 海关编码
    */
        public string getHsCode() {
               	return hsCode;
            }
    
    /**
     * 设置海关编码     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setHsCode(string hsCode) {
     	         	    this.hsCode = hsCode;
     	        }
    
        [DataMember(Order = 14)]
    private byte? isAneroidMarkup;
    
        /**
       * @return 判断是否属于非液体化妆品
    */
        public byte? getIsAneroidMarkup() {
               	return isAneroidMarkup;
            }
    
    /**
     * 设置判断是否属于非液体化妆品     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setIsAneroidMarkup(byte isAneroidMarkup) {
     	         	    this.isAneroidMarkup = isAneroidMarkup;
     	        }
    
        [DataMember(Order = 15)]
    private byte? isOnlyBattery;
    
        /**
       * @return 是否为纯电池
    */
        public byte? getIsOnlyBattery() {
               	return isOnlyBattery;
            }
    
    /**
     * 设置是否为纯电池     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setIsOnlyBattery(byte isOnlyBattery) {
     	         	    this.isOnlyBattery = isOnlyBattery;
     	        }
    
    
  }
}