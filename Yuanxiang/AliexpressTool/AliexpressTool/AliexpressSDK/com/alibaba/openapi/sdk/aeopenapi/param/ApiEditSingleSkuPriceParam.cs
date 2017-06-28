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
public class ApiEditSingleSkuPriceParam {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 需修改编辑的商品ID
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置需修改编辑的商品ID     *
     * 参数示例：<pre>123456789</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private string skuId;
    
        /**
       * @return 需修改编辑的商品单个SKUID。SKU ID可以通过api.findAeProductById接口中的aeopAeproductSKUs获取单个产品信息中"id"进行获取。 没有SKU属性的商品SKUID回传“<none>”
    */
        public string getSkuId() {
               	return skuId;
            }
    
    /**
     * 设置需修改编辑的商品单个SKUID。SKU ID可以通过api.findAeProductById接口中的aeopAeproductSKUs获取单个产品信息中"id"进行获取。 没有SKU属性的商品SKUID回传“<none>”     *
     * 参数示例：<pre>14:771;5:100014066</pre>     
             * 此参数必填
          */
    public void setSkuId(string skuId) {
     	         	    this.skuId = skuId;
     	        }
    
        [DataMember(Order = 3)]
    private string skuPrice;
    
        /**
       * @return 修改编辑后的商品价格
    */
        public string getSkuPrice() {
               	return skuPrice;
            }
    
    /**
     * 设置修改编辑后的商品价格     *
     * 参数示例：<pre>999</pre>     
             * 此参数必填
          */
    public void setSkuPrice(string skuPrice) {
     	         	    this.skuPrice = skuPrice;
     	        }
    
    
  }
}