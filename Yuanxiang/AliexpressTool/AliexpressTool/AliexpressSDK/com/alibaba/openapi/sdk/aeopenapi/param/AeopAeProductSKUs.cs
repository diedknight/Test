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
public class AeopAeProductSKUs {

       [DataMember(Order = 1)]
    private AeopSKUProperty[] aeopSKUProperty;
    
        /**
       * @return Sku属性对象list，允许1-3个sku属性对象，按sku属性顺序排放。sku属性从类目属性接口getAttributesResultByCateId获取。该项值输入sku属性，不能输入普通类目属性。注意，sku属性是有顺序的，必须按照顺序存放。
    */
        public AeopSKUProperty[] getAeopSKUProperty() {
               	return aeopSKUProperty;
            }
    
    /**
     * 设置Sku属性对象list，允许1-3个sku属性对象，按sku属性顺序排放。sku属性从类目属性接口getAttributesResultByCateId获取。该项值输入sku属性，不能输入普通类目属性。注意，sku属性是有顺序的，必须按照顺序存放。     *
     * 参数示例：<pre>"aeopSKUProperty":[{"skuPropertyId":14,"propertyValueId":771, "propertyValueDefinitionName": "black", "skuImage": "http://g01.a.alicdn.com/kf/HTB13GKLJXXXXXbYaXXXq6xXFXXXi.jpg" },{"skuPropertyId":25,"propertyValueId":775,  "propertyValueDefinitionName": "red"}]</pre>     
             * 此参数必填
          */
    public void setAeopSKUProperty(AeopSKUProperty[] aeopSKUProperty) {
     	         	    this.aeopSKUProperty = aeopSKUProperty;
     	        }
    
        [DataMember(Order = 2)]
    private string skuPrice;
    
        /**
       * @return Sku价格。取值范围:0.01-100000;单位:美元。 如:200.07，表示:200美元7分。需要在正确的价格区间内。
    */
        public string getSkuPrice() {
               	return skuPrice;
            }
    
    /**
     * 设置Sku价格。取值范围:0.01-100000;单位:美元。 如:200.07，表示:200美元7分。需要在正确的价格区间内。     *
     * 参数示例：<pre>"200.07"</pre>     
             * 此参数必填
          */
    public void setSkuPrice(string skuPrice) {
     	         	    this.skuPrice = skuPrice;
     	        }
    
        [DataMember(Order = 3)]
    private string skuCode;
    
        /**
       * @return Sku商家编码。 格式:半角英数字,长度20,不包含空格大于号和小于号。如果用户只填写零售价（productprice）和商品编码，需要完整生成一条SKU记录提交，否则商品编码无法保存。系统会认为只提交了零售价，而没有SKU，导致商品编辑未保存。
    */
        public string getSkuCode() {
               	return skuCode;
            }
    
    /**
     * 设置Sku商家编码。 格式:半角英数字,长度20,不包含空格大于号和小于号。如果用户只填写零售价（productprice）和商品编码，需要完整生成一条SKU记录提交，否则商品编码无法保存。系统会认为只提交了零售价，而没有SKU，导致商品编辑未保存。     *
     * 参数示例：<pre>"cfas00973"</pre>     
             * 此参数必填
          */
    public void setSkuCode(string skuCode) {
     	         	    this.skuCode = skuCode;
     	        }
    
        [DataMember(Order = 4)]
    private bool? skuStock;
    
        /**
       * @return Sku库存,数据格式有货true，无货false；至少有一条sku记录是有货的。
    */
        public bool? getSkuStock() {
               	return skuStock;
            }
    
    /**
     * 设置Sku库存,数据格式有货true，无货false；至少有一条sku记录是有货的。     *
     * 参数示例：<pre>true</pre>     
             * 此参数必填
          */
    public void setSkuStock(bool skuStock) {
     	         	    this.skuStock = skuStock;
     	        }
    
        [DataMember(Order = 5)]
    private long? ipmSkuStock;
    
        /**
       * @return SKU实际可售库存属性ipmSkuStock，该属性值的合理取值范围为0~999999，如该商品有SKU时，请确保至少有一个SKU是有货状态，也就是ipmSkuStock取值是1~999999，在整个商品纬度库存值的取值范围是1~999999。 如果同时设置了skuStock属性，那么系统以ipmSkuStock属性为优先；如果没有设置ipmSkuStock属性，那么系统会根据skuStock属性进行设置库存，true表示999，false表示0。
    */
        public long? getIpmSkuStock() {
               	return ipmSkuStock;
            }
    
    /**
     * 设置SKU实际可售库存属性ipmSkuStock，该属性值的合理取值范围为0~999999，如该商品有SKU时，请确保至少有一个SKU是有货状态，也就是ipmSkuStock取值是1~999999，在整个商品纬度库存值的取值范围是1~999999。 如果同时设置了skuStock属性，那么系统以ipmSkuStock属性为优先；如果没有设置ipmSkuStock属性，那么系统会根据skuStock属性进行设置库存，true表示999，false表示0。     *
     * 参数示例：<pre>1234</pre>     
             * 此参数必填
          */
    public void setIpmSkuStock(long ipmSkuStock) {
     	         	    this.ipmSkuStock = ipmSkuStock;
     	        }
    
        [DataMember(Order = 6)]
    private string id;
    
        /**
       * @return SKU ID。 可以唯一表示一个商品范围内的SKU。注意: 这是一个只读参数，在发布和编辑商品信息时，在设置aeopAeProductSKUs参数时不需要提供这个参数。isv可以通过api.findAeProductById(查询单个商品信息)接口来获取到这个属性。这个属性被api.editMutilpleSkuStocks(编辑一个或多个SKU的可售库存)、api.editSingleSkuStock(编辑单个SKU的可售库存)、api.editSingleSkuPrice(编辑单个SKU的价格)三个接口使用到。
    */
        public string getId() {
               	return id;
            }
    
    /**
     * 设置SKU ID。 可以唯一表示一个商品范围内的SKU。注意: 这是一个只读参数，在发布和编辑商品信息时，在设置aeopAeProductSKUs参数时不需要提供这个参数。isv可以通过api.findAeProductById(查询单个商品信息)接口来获取到这个属性。这个属性被api.editMutilpleSkuStocks(编辑一个或多个SKU的可售库存)、api.editSingleSkuStock(编辑单个SKU的可售库存)、api.editSingleSkuPrice(编辑单个SKU的价格)三个接口使用到。     *
     * 参数示例：<pre>"200000182:193;200007763:201336100"</pre>     
             * 此参数必填
          */
    public void setId(string id) {
     	         	    this.id = id;
     	        }
    
        [DataMember(Order = 7)]
    private string currencyCode;
    
        /**
       * @return 货币单位。如果不提供该值信息，则默认为"USD"；非俄罗斯卖家这个属性值可以不提供。对于俄罗斯海外卖家，该单位值必须提供，如: "RUB"。
    */
        public string getCurrencyCode() {
               	return currencyCode;
            }
    
    /**
     * 设置货币单位。如果不提供该值信息，则默认为"USD"；非俄罗斯卖家这个属性值可以不提供。对于俄罗斯海外卖家，该单位值必须提供，如: "RUB"。     *
     * 参数示例：<pre>"USD"或者"RUB"</pre>     
             * 此参数必填
          */
    public void setCurrencyCode(string currencyCode) {
     	         	    this.currencyCode = currencyCode;
     	        }
    
    
  }
}