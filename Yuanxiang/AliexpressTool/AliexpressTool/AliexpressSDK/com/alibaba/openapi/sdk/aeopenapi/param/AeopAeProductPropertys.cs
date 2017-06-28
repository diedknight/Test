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
public class AeopAeProductPropertys {

       [DataMember(Order = 1)]
    private int? attrNameId;
    
        /**
       * @return 属性名ID。从类目属性接口getAttributesResultByCateId获取普通类目属性，不可填入sku属性。 自定义属性名时,该项不填.
    */
        public int? getAttrNameId() {
               	return attrNameId;
            }
    
    /**
     * 设置属性名ID。从类目属性接口getAttributesResultByCateId获取普通类目属性，不可填入sku属性。 自定义属性名时,该项不填.     *
     * 参数示例：<pre>200000043</pre>     
             * 此参数必填
          */
    public void setAttrNameId(int attrNameId) {
     	         	    this.attrNameId = attrNameId;
     	        }
    
        [DataMember(Order = 2)]
    private string attrName;
    
        /**
       * @return 自定义属性名属性名。 自定义属性名时,该项必填.
    */
        public string getAttrName() {
               	return attrName;
            }
    
    /**
     * 设置自定义属性名属性名。 自定义属性名时,该项必填.     *
     * 参数示例：<pre>size</pre>     
             * 此参数必填
          */
    public void setAttrName(string attrName) {
     	         	    this.attrName = attrName;
     	        }
    
        [DataMember(Order = 3)]
    private int? attrValueId;
    
        /**
       * @return 属性值ID。从类目属性接口getAttributesResultByCateId获取普通类目属性，不可填入sku属性。自定义属性值时,该项不填。
    */
        public int? getAttrValueId() {
               	return attrValueId;
            }
    
    /**
     * 设置属性值ID。从类目属性接口getAttributesResultByCateId获取普通类目属性，不可填入sku属性。自定义属性值时,该项不填。     *
     * 参数示例：<pre>581</pre>     
             * 此参数必填
          */
    public void setAttrValueId(int attrValueId) {
     	         	    this.attrValueId = attrValueId;
     	        }
    
        [DataMember(Order = 4)]
    private string attrValue;
    
        /**
       * @return 自定义属性值。自定义属性名时,该项必填。 当自定义属性值内容为区间情况时，建议格式2 - 5 kg。(注意，数字'-'单位三者间是要加空格的！)
    */
        public string getAttrValue() {
               	return attrValue;
            }
    
    /**
     * 设置自定义属性值。自定义属性名时,该项必填。 当自定义属性值内容为区间情况时，建议格式2 - 5 kg。(注意，数字'-'单位三者间是要加空格的！)     *
     * 参数示例：<pre>2 - 5 kg</pre>     
             * 此参数必填
          */
    public void setAttrValue(string attrValue) {
     	         	    this.attrValue = attrValue;
     	        }
    
    
  }
}