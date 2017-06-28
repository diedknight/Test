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
public class ApiListTbProductByIdsResult {

       [DataMember(Order = 1)]
    private AeopTaoDaiXiaoProductResultDTO[] response;
    
        /**
       * @return 
    */
        public AeopTaoDaiXiaoProductResultDTO[] getResponse() {
               	return response;
            }
    
    /**
     * 设置     *
          
             * 此参数必填
          */
    public void setResponse(AeopTaoDaiXiaoProductResultDTO[] response) {
     	         	    this.response = response;
     	        }
    
    
  }
}