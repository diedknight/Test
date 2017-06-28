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
public class ApiEditProductCategoryAttributesParam {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 产品的ID
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置产品的ID     *
     * 参数示例：<pre>1706468951</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private AeopAeProductPropertys[] productCategoryAttributes;
    
        /**
       * @return 该产品要修改的类目属性列表
    */
        public AeopAeProductPropertys[] getProductCategoryAttributes() {
               	return productCategoryAttributes;
            }
    
    /**
     * 设置该产品要修改的类目属性列表     *
     * 参数示例：<pre>[{"attrNameId":284, "attrValueId":491}, {"attrNameId":200000137,"attrValueId":7926}, {"attrName":"Color", "attrValue":"Red"}, {"attrName":"Additional", "attrValue":"Value"}]</pre>     
             * 此参数必填
          */
    public void setProductCategoryAttributes(AeopAeProductPropertys[] productCategoryAttributes) {
     	         	    this.productCategoryAttributes = productCategoryAttributes;
     	        }
    
    
  }
}