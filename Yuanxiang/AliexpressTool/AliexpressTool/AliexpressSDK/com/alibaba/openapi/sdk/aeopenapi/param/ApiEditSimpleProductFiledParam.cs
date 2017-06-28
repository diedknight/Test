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
public class ApiEditSimpleProductFiledParam {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 指定编辑产品的id
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置指定编辑产品的id     *
     * 参数示例：<pre>id=1234</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private string fiedName;
    
        /**
       * @return 编辑的字段名称，为以下字段内容里的其中一项,
可以编辑的字段包括:
subject: 商品的标题;
Detail: 商品的详细描述信息；
deliveryTime: 备货期；
groupId: 产品组；
freightTemplateId: 运费模版；
packageLength: 商品包装长度；
packageWidth: 商品包装宽度；
packageHeight：商品包装高度；
grossWeight: 商品毛重；
wsValidNum商品的有效天数（注意：该字段的提交修改，数据生效时间：商品（到期或手动）下架再上架生效。”）;
reduceStrategy: 库存扣减策略(总共有2种：下单减库存(place_order_withhold)和支付减库存(payment_success_deduct)。)
    */
        public string getFiedName() {
               	return fiedName;
            }
    
    /**
     * 设置编辑的字段名称，为以下字段内容里的其中一项,
可以编辑的字段包括:
subject: 商品的标题;
Detail: 商品的详细描述信息；
deliveryTime: 备货期；
groupId: 产品组；
freightTemplateId: 运费模版；
packageLength: 商品包装长度；
packageWidth: 商品包装宽度；
packageHeight：商品包装高度；
grossWeight: 商品毛重；
wsValidNum商品的有效天数（注意：该字段的提交修改，数据生效时间：商品（到期或手动）下架再上架生效。”）;
reduceStrategy: 库存扣减策略(总共有2种：下单减库存(place_order_withhold)和支付减库存(payment_success_deduct)。)     *
     * 参数示例：<pre>deliveryTime</pre>     
             * 此参数必填
          */
    public void setFiedName(string fiedName) {
     	         	    this.fiedName = fiedName;
     	        }
    
        [DataMember(Order = 3)]
    private string fiedvalue;
    
        /**
       * @return 指定编辑产品字段值,上述字段编辑后发提交值。
    */
        public string getFiedvalue() {
               	return fiedvalue;
            }
    
    /**
     * 设置指定编辑产品字段值,上述字段编辑后发提交值。     *
     * 参数示例：<pre>30</pre>     
             * 此参数必填
          */
    public void setFiedvalue(string fiedvalue) {
     	         	    this.fiedvalue = fiedvalue;
     	        }
    
    
  }
}