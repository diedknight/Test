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
public class ApiSetGroupsParam {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 产品ID
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置产品ID     *
     * 参数示例：<pre>32218803874</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private string groupIds;
    
        /**
       * @return 商品分组ID。如果需要将一个商品设置成多个分组，需要将分组id用逗号分隔，如：'123,456,789' 至多3个。
    */
        public string getGroupIds() {
               	return groupIds;
            }
    
    /**
     * 设置商品分组ID。如果需要将一个商品设置成多个分组，需要将分组id用逗号分隔，如：'123,456,789' 至多3个。     *
     * 参数示例：<pre>254562048</pre>     
             * 此参数必填
          */
    public void setGroupIds(string groupIds) {
     	         	    this.groupIds = groupIds;
     	        }
    
    
  }
}