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
public class AeProhibitedWord {

       [DataMember(Order = 1)]
    private string primaryWord;
    
        /**
       * @return 违禁词名称
    */
        public string getPrimaryWord() {
               	return primaryWord;
            }
    
    /**
     * 设置违禁词名称     *
     * 参数示例：<pre>nike</pre>     
             * 此参数必填
          */
    public void setPrimaryWord(string primaryWord) {
     	         	    this.primaryWord = primaryWord;
     	        }
    
        [DataMember(Order = 2)]
    private string[] types;
    
        /**
       * @return 违禁原因
    */
        public string[] getTypes() {
               	return types;
            }
    
    /**
     * 设置违禁原因     *
     * 参数示例：<pre>[&quot;FORBIDEN_TYPE&quot;, &quot;RESTRICT_TYPE&quot;]</pre>     
             * 此参数必填
          */
    public void setTypes(string[] types) {
     	         	    this.types = types;
     	        }
    
    
  }
}