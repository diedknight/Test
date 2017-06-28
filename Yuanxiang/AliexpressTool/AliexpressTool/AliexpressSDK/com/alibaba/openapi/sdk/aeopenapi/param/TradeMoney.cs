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
public class TradeMoney {

       [DataMember(Order = 1)]
    private string amount;
    
        /**
       * @return 金额
    */
        public string getAmount() {
               	return amount;
            }
    
    /**
     * 设置金额     *
     * 参数示例：<pre>100</pre>     
             * 此参数必填
          */
    public void setAmount(string amount) {
     	         	    this.amount = amount;
     	        }
    
        [DataMember(Order = 2)]
    private long? cent;
    
        /**
       * @return 分
    */
        public long? getCent() {
               	return cent;
            }
    
    /**
     * 设置分     *
     * 参数示例：<pre>10000</pre>     
             * 此参数必填
          */
    public void setCent(long cent) {
     	         	    this.cent = cent;
     	        }
    
        [DataMember(Order = 3)]
    private string currencyCode;
    
        /**
       * @return 币种USD/RUB
    */
        public string getCurrencyCode() {
               	return currencyCode;
            }
    
    /**
     * 设置币种USD/RUB     *
     * 参数示例：<pre>USD</pre>     
             * 此参数必填
          */
    public void setCurrencyCode(string currencyCode) {
     	         	    this.currencyCode = currencyCode;
     	        }
    
        [DataMember(Order = 4)]
    private Currency currency;
    
        /**
       * @return 
    */
        public Currency getCurrency() {
               	return currency;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCurrency(Currency currency) {
     	         	    this.currency = currency;
     	        }
    
    
  }
}