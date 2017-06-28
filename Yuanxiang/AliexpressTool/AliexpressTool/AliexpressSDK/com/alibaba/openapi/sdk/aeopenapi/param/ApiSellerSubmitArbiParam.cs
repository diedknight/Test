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
public class ApiSellerSubmitArbiParam {

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
    private string reason;
    
        /**
       * @return 纠纷原因(有两个值，一个是noMatchDesc（货不对版），另外一个是notReceived（未收到货）
    */
        public string getReason() {
               	return reason;
            }
    
    /**
     * 设置纠纷原因(有两个值，一个是noMatchDesc（货不对版），另外一个是notReceived（未收到货）     *
     * 参数示例：<pre>noMatchDesc</pre>     
             * 此参数必填
          */
    public void setReason(string reason) {
     	         	    this.reason = reason;
     	        }
    
        [DataMember(Order = 3)]
    private string content;
    
        /**
       * @return 卖家提交仲裁描述
    */
        public string getContent() {
               	return content;
            }
    
    /**
     * 设置卖家提交仲裁描述     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setContent(string content) {
     	         	    this.content = content;
     	        }
    
    
  }
}