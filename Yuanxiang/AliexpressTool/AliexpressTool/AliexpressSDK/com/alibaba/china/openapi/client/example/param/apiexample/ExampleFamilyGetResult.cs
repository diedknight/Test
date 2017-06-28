using com.alibaba.openapi.client.primitive;
using com.alibaba.openapi.client.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace com.alibaba.china.openapi.client.example.param.apiexample
{
[DataContract(Namespace = "com.alibaba.openapi.client")]
class ExampleFamilyGetResult {

       [DataMember(Order = 1)]
    private ExampleFamily result;
    
        /**
       * @return 
    */
        public ExampleFamily getResult() {
               	return result;
            }
    
    /**
     * 设置     *
          
             * 此参数必填
          */
    public void setResult(ExampleFamily result) {
     	         	    this.result = result;
     	        }
    
    
  }
}