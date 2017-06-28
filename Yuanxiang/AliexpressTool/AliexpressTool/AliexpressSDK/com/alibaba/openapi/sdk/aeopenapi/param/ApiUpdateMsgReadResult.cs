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
public class ApiUpdateMsgReadResult {

       [DataMember(Order = 1)]
    private EcResult result;
    
        /**
       * @return 更新插入返回结果
    */
        public EcResult getResult() {
               	return result;
            }
    
    /**
     * 设置更新插入返回结果     *
          
             * 此参数必填
          */
    public void setResult(EcResult result) {
     	         	    this.result = result;
     	        }
    
    
  }
}