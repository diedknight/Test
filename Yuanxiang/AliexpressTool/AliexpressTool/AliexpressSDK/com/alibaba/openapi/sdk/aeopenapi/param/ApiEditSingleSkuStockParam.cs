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
public class ApiEditSingleSkuStockParam {

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
     * 参数示例：<pre>32297192242</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private string skuId;
    
        /**
       * @return 需修改编辑的商品单个SKUID。SKU ID可以通过api.findAeProductById接口中的aeopAeproductSKUs获取单个产品信息中"id"进行获取。
    */
        public string getSkuId() {
               	return skuId;
            }
    
    /**
     * 设置需修改编辑的商品单个SKUID。SKU ID可以通过api.findAeProductById接口中的aeopAeproductSKUs获取单个产品信息中"id"进行获取。     *
     * 参数示例：<pre>14:200003699;5:100014065</pre>     
             * 此参数必填
          */
    public void setSkuId(string skuId) {
     	         	    this.skuId = skuId;
     	        }
    
        [DataMember(Order = 3)]
    private long? ipmSkuStock;
    
        /**
       * @return SKU的库存值
    */
        public long? getIpmSkuStock() {
               	return ipmSkuStock;
            }
    
    /**
     * 设置SKU的库存值     *
     * 参数示例：<pre>299</pre>     
             * 此参数必填
          */
    public void setIpmSkuStock(long ipmSkuStock) {
     	         	    this.ipmSkuStock = ipmSkuStock;
     	        }
    
    
  }
}