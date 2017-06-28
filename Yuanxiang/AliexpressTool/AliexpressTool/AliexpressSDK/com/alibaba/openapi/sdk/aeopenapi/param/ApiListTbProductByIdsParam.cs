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
public class ApiListTbProductByIdsParam {

       [DataMember(Order = 1)]
    private string productIds;
    
        /**
       * @return ae 产品id列表，用逗号分隔，最大不能超过100个id
    */
        public string getProductIds() {
               	return productIds;
            }
    
    /**
     * 设置ae 产品id列表，用逗号分隔，最大不能超过100个id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductIds(string productIds) {
     	         	    this.productIds = productIds;
     	        }
    
    
  }
}