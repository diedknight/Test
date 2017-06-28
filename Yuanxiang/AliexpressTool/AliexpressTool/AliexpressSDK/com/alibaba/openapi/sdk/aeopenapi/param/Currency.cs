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
public class Currency {

       [DataMember(Order = 1)]
    private int? defaultFractionDigits;
    
        /**
       * @return 默认分数
    */
        public int? getDefaultFractionDigits() {
               	return defaultFractionDigits;
            }
    
    /**
     * 设置默认分数     *
     * 参数示例：<pre>2</pre>     
             * 此参数必填
          */
    public void setDefaultFractionDigits(int defaultFractionDigits) {
     	         	    this.defaultFractionDigits = defaultFractionDigits;
     	        }
    
        [DataMember(Order = 2)]
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
    
        [DataMember(Order = 3)]
    private string symbol;
    
        /**
       * @return 币种符号
    */
        public string getSymbol() {
               	return symbol;
            }
    
    /**
     * 设置币种符号     *
     * 参数示例：<pre>$</pre>     
             * 此参数必填
          */
    public void setSymbol(string symbol) {
     	         	    this.symbol = symbol;
     	        }
    
    
  }
}