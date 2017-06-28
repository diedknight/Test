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
public class AeopSKUProperty {

       [DataMember(Order = 1)]
    private int? skuPropertyId;
    
        /**
       * @return SKU属性名ID,用于表示SKU的一维。
    */
        public int? getSkuPropertyId() {
               	return skuPropertyId;
            }
    
    /**
     * 设置SKU属性名ID,用于表示SKU的一维。     *
     * 参数示例：<pre>14</pre>     
             * 此参数必填
          */
    public void setSkuPropertyId(int skuPropertyId) {
     	         	    this.skuPropertyId = skuPropertyId;
     	        }
    
        [DataMember(Order = 2)]
    private int? propertyValueId;
    
        /**
       * @return SKU属性值ID,用于表示SKU某一维的取值。
    */
        public int? getPropertyValueId() {
               	return propertyValueId;
            }
    
    /**
     * 设置SKU属性值ID,用于表示SKU某一维的取值。     *
     * 参数示例：<pre>771</pre>     
             * 此参数必填
          */
    public void setPropertyValueId(int propertyValueId) {
     	         	    this.propertyValueId = propertyValueId;
     	        }
    
        [DataMember(Order = 3)]
    private string propertyValueDefinitionName;
    
        /**
       * @return SKU属性值自定义名称。
    */
        public string getPropertyValueDefinitionName() {
               	return propertyValueDefinitionName;
            }
    
    /**
     * 设置SKU属性值自定义名称。     *
     * 参数示例：<pre>"black"</pre>     
             * 此参数必填
          */
    public void setPropertyValueDefinitionName(string propertyValueDefinitionName) {
     	         	    this.propertyValueDefinitionName = propertyValueDefinitionName;
     	        }
    
        [DataMember(Order = 4)]
    private string skuImage;
    
        /**
       * @return SKU自定义图片。
    */
        public string getSkuImage() {
               	return skuImage;
            }
    
    /**
     * 设置SKU自定义图片。     *
     * 参数示例：<pre>"http://g01.a.alicdn.com/kf/HTB13GKLJXXXXXbYaXXXq6xXFXXXi.jpg"</pre>     
             * 此参数必填
          */
    public void setSkuImage(string skuImage) {
     	         	    this.skuImage = skuImage;
     	        }
    
    
  }
}