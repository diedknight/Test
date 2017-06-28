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
public class ApiSetShopwindowProductParam {

       [DataMember(Order = 1)]
    private string productIdList;
    
        /**
       * @return 待设置橱窗的商品id，可输入多个，之前用半角分号分割。
    */
        public string getProductIdList() {
               	return productIdList;
            }
    
    /**
     * 设置待设置橱窗的商品id，可输入多个，之前用半角分号分割。     *
     * 参数示例：<pre>1351344486;1351344487</pre>     
             * 此参数必填
          */
    public void setProductIdList(string productIdList) {
     	         	    this.productIdList = productIdList;
     	        }
    
    
  }
}