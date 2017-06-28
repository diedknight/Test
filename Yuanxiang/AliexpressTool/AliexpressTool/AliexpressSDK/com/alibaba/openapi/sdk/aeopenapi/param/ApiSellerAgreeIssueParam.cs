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
public class ApiSellerAgreeIssueParam {

       [DataMember(Order = 1)]
    private long? issueId;
    
        /**
       * @return 纠纷ID
    */
        public long? getIssueId() {
               	return issueId;
            }
    
    /**
     * 设置纠纷ID     *
     * 参数示例：<pre>60573020065804</pre>     
             * 此参数必填
          */
    public void setIssueId(long issueId) {
     	         	    this.issueId = issueId;
     	        }
    
        [DataMember(Order = 2)]
    private bool? isReturnGoods;
    
        /**
       * @return 是否退货，需要先查询纠纷详情接口，得到买家资金方案中的isReceivedGoods
    */
        public bool? getIsReturnGoods() {
               	return isReturnGoods;
            }
    
    /**
     * 设置是否退货，需要先查询纠纷详情接口，得到买家资金方案中的isReceivedGoods     *
     * 参数示例：<pre>true</pre>     
             * 此参数必填
          */
    public void setIsReturnGoods(bool isReturnGoods) {
     	         	    this.isReturnGoods = isReturnGoods;
     	        }
    
        [DataMember(Order = 3)]
    private long? refundAmount;
    
        /**
       * @return 退款金额（单位：分），需要先查询纠纷详情接口，得到买家资金方案中的issueMoney中的cent
    */
        public long? getRefundAmount() {
               	return refundAmount;
            }
    
    /**
     * 设置退款金额（单位：分），需要先查询纠纷详情接口，得到买家资金方案中的issueMoney中的cent     *
     * 参数示例：<pre>3000</pre>     
             * 此参数必填
          */
    public void setRefundAmount(long refundAmount) {
     	         	    this.refundAmount = refundAmount;
     	        }
    
        [DataMember(Order = 4)]
    private string refundCurrency;
    
        /**
       * @return 币种，需要先查询纠纷详情接口，得到买家资金方案中的issueMoney中的币种
    */
        public string getRefundCurrency() {
               	return refundCurrency;
            }
    
    /**
     * 设置币种，需要先查询纠纷详情接口，得到买家资金方案中的issueMoney中的币种     *
     * 参数示例：<pre>USD、RUB</pre>     
             * 此参数必填
          */
    public void setRefundCurrency(string refundCurrency) {
     	         	    this.refundCurrency = refundCurrency;
     	        }
    
    
  }
}