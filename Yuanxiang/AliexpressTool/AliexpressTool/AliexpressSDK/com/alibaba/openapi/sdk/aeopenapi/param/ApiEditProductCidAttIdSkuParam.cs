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
public class ApiEditProductCidAttIdSkuParam {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 商品id，一次只能上传一个
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置商品id，一次只能上传一个     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private long? categoryId;
    
        /**
       * @return 产品类目ID，如果不想调整类目，则不要填写。
    */
        public long? getCategoryId() {
               	return categoryId;
            }
    
    /**
     * 设置产品类目ID，如果不想调整类目，则不要填写。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCategoryId(long categoryId) {
     	         	    this.categoryId = categoryId;
     	        }
    
        [DataMember(Order = 3)]
    private AeopAeProductSKUs[] productSkus;
    
        /**
       * @return 该产品新的类目SKU属性。如果没有指定categoryId, 则该SKU属性则为当前产品所属类目下的SKU属性，如果指定了categoryId, 则该SKU属性则为新类目下的SKU属性。
特别提示：新增SKU实际可售库存属性ipmSkuStock，该属性值的合理取值范围为0~999999，如该商品有SKU时，请确保至少有一个SKU是有货状态，也就是ipmSkuStock取值是1~999999，在整个商品纬度库存值的取值范围是1~999999。
    */
        public AeopAeProductSKUs[] getProductSkus() {
               	return productSkus;
            }
    
    /**
     * 设置该产品新的类目SKU属性。如果没有指定categoryId, 则该SKU属性则为当前产品所属类目下的SKU属性，如果指定了categoryId, 则该SKU属性则为新类目下的SKU属性。
特别提示：新增SKU实际可售库存属性ipmSkuStock，该属性值的合理取值范围为0~999999，如该商品有SKU时，请确保至少有一个SKU是有货状态，也就是ipmSkuStock取值是1~999999，在整个商品纬度库存值的取值范围是1~999999。     *
     * 参数示例：<pre>[ { "aeopSKUProperty" : [ { "propertyValueId" : 496,
              "skuPropertyId" : 14
            },
            { "propertyValueId" : 4181,
              "skuPropertyId" : 5
            }
          ],
        "skuCode" : "asdfas",
        "skuPrice" : "999.00",
        "skuStock" : true,
        "ipmSkuStock":"100"
      },
      { "aeopSKUProperty" : [ { "propertyValueId" : 771,
              "skuPropertyId" : 14
            },
            { "propertyValueId" : 100014066,
              "skuPropertyId" : 5
            }
          ],
        "skuCode" : "fasdf",
        "skuPrice" : "999.00",
        "skuStock" : true
      },
      { "aeopSKUProperty" : [ { "propertyValueId" : 496,
              "skuPropertyId" : 14
            },
            { "propertyValueId" : 100014066,
              "skuPropertyId" : 5
            }
          ],
        "skuCode" : "fasdfa",
        "skuPrice" : "999.00",
        "skuStock" : true
      },
      { "aeopSKUProperty" : [ { "propertyValueId" : 771,
              "skuPropertyId" : 14
            },
            { "propertyValueId" : 4181,
              "skuPropertyId" : 5
            }
          ],
        "skuCode" : "sfas",
        "skuPrice" : "999.00",
        "skuStock" : true,
        "ipmSkuStock":"100"
      }
    ]</pre>     
             * 此参数必填
          */
    public void setProductSkus(AeopAeProductSKUs[] productSkus) {
     	         	    this.productSkus = productSkus;
     	        }
    
        [DataMember(Order = 4)]
    private AeopAeProductPropertys[] productProperties;
    
        /**
       * @return 该产品新的类目属性。如果没有指定categoryId, 则该类目属性则为当前产品所属类目下的类目属性，如果指定了categoryId, 则该类目属性则为新类目下的类目属性。
    */
        public AeopAeProductPropertys[] getProductProperties() {
               	return productProperties;
            }
    
    /**
     * 设置该产品新的类目属性。如果没有指定categoryId, 则该类目属性则为当前产品所属类目下的类目属性，如果指定了categoryId, 则该类目属性则为新类目下的类目属性。     *
     * 参数示例：<pre>[ { "attrNameId" : 200000137,
        "attrValueId" : 200001645
      },
      { "attrNameId" : 200000332,
        "attrValueId" : 1927
      },
      { "attrNameId" : 284,
        "attrValueId" : 494
      }
    ]</pre>     
             * 此参数必填
          */
    public void setProductProperties(AeopAeProductPropertys[] productProperties) {
     	         	    this.productProperties = productProperties;
     	        }
    
    
  }
}