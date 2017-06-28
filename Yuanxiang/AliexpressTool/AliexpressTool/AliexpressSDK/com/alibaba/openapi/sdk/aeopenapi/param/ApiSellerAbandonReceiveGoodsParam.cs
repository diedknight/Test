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
public class ApiSellerAbandonReceiveGoodsParam {

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
    
    
  }
}