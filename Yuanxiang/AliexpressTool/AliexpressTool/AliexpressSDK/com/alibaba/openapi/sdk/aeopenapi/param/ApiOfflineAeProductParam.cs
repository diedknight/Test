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
public class ApiOfflineAeProductParam {

       [DataMember(Order = 1)]
    private string productIds;
    
        /**
       * @return 需要下架的产品id。多个产品ID用英文分号隔开。
    */
        public string getProductIds() {
               	return productIds;
            }
    
    /**
     * 设置需要下架的产品id。多个产品ID用英文分号隔开。     *
     * 参数示例：<pre>109827;109828</pre>     
             * 此参数必填
          */
    public void setProductIds(string productIds) {
     	         	    this.productIds = productIds;
     	        }
    
    
  }
}