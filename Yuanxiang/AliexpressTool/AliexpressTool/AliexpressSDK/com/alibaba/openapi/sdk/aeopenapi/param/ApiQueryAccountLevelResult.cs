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
public class ApiQueryAccountLevelResult {

       [DataMember(Order = 1)]
    private AccountResultDTO result;
    
        /**
       * @return 会员等级情况
    */
        public AccountResultDTO getResult() {
               	return result;
            }
    
    /**
     * 设置会员等级情况     *
          
             * 此参数必填
          */
    public void setResult(AccountResultDTO result) {
     	         	    this.result = result;
     	        }
    
    
  }
}