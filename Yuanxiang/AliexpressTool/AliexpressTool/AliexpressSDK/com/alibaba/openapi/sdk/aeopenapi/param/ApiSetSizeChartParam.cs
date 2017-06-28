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
public class ApiSetSizeChartParam {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 商品Id
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置商品Id     *
     * 参数示例：<pre>4363434343</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private long? sizechartId;
    
        /**
       * @return 尺码表模版Id, 必须与当前商品所在类目想对应。
    */
        public long? getSizechartId() {
               	return sizechartId;
            }
    
    /**
     * 设置尺码表模版Id, 必须与当前商品所在类目想对应。     *
     * 参数示例：<pre>544113</pre>     
             * 此参数必填
          */
    public void setSizechartId(long sizechartId) {
     	         	    this.sizechartId = sizechartId;
     	        }
    
    
  }
}