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
public class ApiCalculateFreightParam {

       [DataMember(Order = 1)]
    private int? length;
    
        /**
       * @return 长
    */
        public int? getLength() {
               	return length;
            }
    
    /**
     * 设置长     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLength(int length) {
     	         	    this.length = length;
     	        }
    
        [DataMember(Order = 2)]
    private int? width;
    
        /**
       * @return 宽
    */
        public int? getWidth() {
               	return width;
            }
    
    /**
     * 设置宽     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setWidth(int width) {
     	         	    this.width = width;
     	        }
    
        [DataMember(Order = 3)]
    private int? height;
    
        /**
       * @return 高
    */
        public int? getHeight() {
               	return height;
            }
    
    /**
     * 设置高     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setHeight(int height) {
     	         	    this.height = height;
     	        }
    
        [DataMember(Order = 4)]
    private double? weight;
    
        /**
       * @return 毛重
    */
        public double? getWeight() {
               	return weight;
            }
    
    /**
     * 设置毛重     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setWeight(double weight) {
     	         	    this.weight = weight;
     	        }
    
        [DataMember(Order = 5)]
    private int? count;
    
        /**
       * @return 数量
    */
        public int? getCount() {
               	return count;
            }
    
    /**
     * 设置数量     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCount(int count) {
     	         	    this.count = count;
     	        }
    
        [DataMember(Order = 6)]
    private string country;
    
        /**
       * @return country
    */
        public string getCountry() {
               	return country;
            }
    
    /**
     * 设置country     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCountry(string country) {
     	         	    this.country = country;
     	        }
    
        [DataMember(Order = 7)]
    private bool? customPackWeight;
    
        /**
       * @return 是否为自定义打包计重,Y/N
    */
        public bool? getCustomPackWeight() {
               	return customPackWeight;
            }
    
    /**
     * 设置是否为自定义打包计重,Y/N     *
     * 参数示例：<pre>Y</pre>     
             * 此参数必填
          */
    public void setCustomPackWeight(bool customPackWeight) {
     	         	    this.customPackWeight = customPackWeight;
     	        }
    
        [DataMember(Order = 8)]
    private int? packBaseUnit;
    
        /**
       * @return 打包计重几件以内按单个产品计重,当isCustomPackWeight=Y时必选
    */
        public int? getPackBaseUnit() {
               	return packBaseUnit;
            }
    
    /**
     * 设置打包计重几件以内按单个产品计重,当isCustomPackWeight=Y时必选     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPackBaseUnit(int packBaseUnit) {
     	         	    this.packBaseUnit = packBaseUnit;
     	        }
    
        [DataMember(Order = 9)]
    private int? packAddUnit;
    
        /**
       * @return 打包计重超过部分每增加件数,当isCustomPackWeight=Y时必选
    */
        public int? getPackAddUnit() {
               	return packAddUnit;
            }
    
    /**
     * 设置打包计重超过部分每增加件数,当isCustomPackWeight=Y时必选     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPackAddUnit(int packAddUnit) {
     	         	    this.packAddUnit = packAddUnit;
     	        }
    
        [DataMember(Order = 10)]
    private double? packAddWeight;
    
        /**
       * @return 打包计重超过部分续重,当isCustomPackWeight=Y时必选
    */
        public double? getPackAddWeight() {
               	return packAddWeight;
            }
    
    /**
     * 设置打包计重超过部分续重,当isCustomPackWeight=Y时必选     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPackAddWeight(double packAddWeight) {
     	         	    this.packAddWeight = packAddWeight;
     	        }
    
        [DataMember(Order = 11)]
    private int? freightTemplateId;
    
        /**
       * @return 运费模板ID 必选
    */
        public int? getFreightTemplateId() {
               	return freightTemplateId;
            }
    
    /**
     * 设置运费模板ID 必选     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFreightTemplateId(int freightTemplateId) {
     	         	    this.freightTemplateId = freightTemplateId;
     	        }
    
        [DataMember(Order = 12)]
    private Money productPrice;
    
        /**
       * @return 产品价格
    */
        public Money getProductPrice() {
               	return productPrice;
            }
    
    /**
     * 设置产品价格     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductPrice(Money productPrice) {
     	         	    this.productPrice = productPrice;
     	        }
    
    
  }
}