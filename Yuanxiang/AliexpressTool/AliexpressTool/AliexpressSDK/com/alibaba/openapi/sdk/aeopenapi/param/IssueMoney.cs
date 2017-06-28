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
public class IssueMoney {

       [DataMember(Order = 1)]
    private double? amount;
    
        /**
       * @return 金额，单位元
    */
        public double? getAmount() {
               	return amount;
            }
    
    /**
     * 设置金额，单位元     *
     * 参数示例：<pre>12.31</pre>     
             * 此参数必填
          */
    public void setAmount(double amount) {
     	         	    this.amount = amount;
     	        }
    
        [DataMember(Order = 2)]
    private long? cent;
    
        /**
       * @return 金额，单位分
    */
        public long? getCent() {
               	return cent;
            }
    
    /**
     * 设置金额，单位分     *
     * 参数示例：<pre>1231</pre>     
             * 此参数必填
          */
    public void setCent(long cent) {
     	         	    this.cent = cent;
     	        }
    
        [DataMember(Order = 3)]
    private string currencyCode;
    
        /**
       * @return 币种
    */
        public string getCurrencyCode() {
               	return currencyCode;
            }
    
    /**
     * 设置币种     *
     * 参数示例：<pre>USD</pre>     
             * 此参数必填
          */
    public void setCurrencyCode(string currencyCode) {
     	         	    this.currencyCode = currencyCode;
     	        }
    
        [DataMember(Order = 4)]
    private int? centFactor;
    
        /**
       * @return 元/分换算比例
    */
        public int? getCentFactor() {
               	return centFactor;
            }
    
    /**
     * 设置元/分换算比例     *
     * 参数示例：<pre>100</pre>     
             * 此参数必填
          */
    public void setCentFactor(int centFactor) {
     	         	    this.centFactor = centFactor;
     	        }
    
    
  }
}