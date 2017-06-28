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
public class ApiQueryMsgRelationListResult {

       [DataMember(Order = 1)]
    private RelationResult[] result;
    
        /**
       * @return 站内信/订单留言关系列表返回结果集
    */
        public RelationResult[] getResult() {
               	return result;
            }
    
    /**
     * 设置站内信/订单留言关系列表返回结果集     *
          
             * 此参数必填
          */
    public void setResult(RelationResult[] result) {
     	         	    this.result = result;
     	        }
    
    
  }
}