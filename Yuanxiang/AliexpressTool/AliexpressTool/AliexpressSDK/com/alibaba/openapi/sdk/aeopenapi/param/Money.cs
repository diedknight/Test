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
public class Money {

       [DataMember(Order = 1)]
    private double? amount;
    
        /**
       * @return 
    */
        public double? getAmount() {
               	return amount;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAmount(double amount) {
     	         	    this.amount = amount;
     	        }
    
        [DataMember(Order = 2)]
    private long? cent;
    
        /**
       * @return 
    */
        public long? getCent() {
               	return cent;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCent(long cent) {
     	         	    this.cent = cent;
     	        }
    
        [DataMember(Order = 3)]
    private string currencyCode;
    
        /**
       * @return 
    */
        public string getCurrencyCode() {
               	return currencyCode;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCurrencyCode(string currencyCode) {
     	         	    this.currencyCode = currencyCode;
     	        }
    
    
  }
}