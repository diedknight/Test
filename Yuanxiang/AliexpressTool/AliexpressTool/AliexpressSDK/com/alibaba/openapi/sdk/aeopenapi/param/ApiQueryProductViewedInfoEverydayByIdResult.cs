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
public class ApiQueryProductViewedInfoEverydayByIdResult {

       [DataMember(Order = 1)]
    private string success;
    
        /**
       * @return 浏览量总数。{
    "addCartCount":       加入购物车,
    "exposedCount":       曝光量,
    "favoritedCount": ,
    "gmvPerBuyer": 客单价,
    "gmvPerBuyer30d":最近30天客单价,
    "gmvPerOrder": 订单均额,
    "gmvPerOrder30d": 最近30天订单均额,
    "outputOrder": 成交订单数,
    "refundAmt": 退款金额,
    "success": 本次调用是否成功,
    "viewedCount": 30天浏览量 
}
    */
        public string getSuccess() {
               	return success;
            }
    
    /**
     * 设置浏览量总数。{
    "addCartCount":       加入购物车,
    "exposedCount":       曝光量,
    "favoritedCount": ,
    "gmvPerBuyer": 客单价,
    "gmvPerBuyer30d":最近30天客单价,
    "gmvPerOrder": 订单均额,
    "gmvPerOrder30d": 最近30天订单均额,
    "outputOrder": 成交订单数,
    "refundAmt": 退款金额,
    "success": 本次调用是否成功,
    "viewedCount": 30天浏览量 
}     *
          
             * 此参数必填
          */
    public void setSuccess(string success) {
     	         	    this.success = success;
     	        }
    
    
  }
}