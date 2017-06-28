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
class ExampleFamilyPostResult {

       [DataMember(Order = 1)]
    private ExampleFamily result;
    
        /**
       * @return 返回的接听信息
    */
        public ExampleFamily getResult() {
               	return result;
            }
    
    /**
     * 设置返回的接听信息     *
          
             * 此参数必填
          */
    public void setResult(ExampleFamily result) {
     	         	    this.result = result;
     	        }
    
        [DataMember(Order = 2)]
    private string resultDesc;
    
        /**
       * @return 返回结果描述
    */
        public string getResultDesc() {
               	return resultDesc;
            }
    
    /**
     * 设置返回结果描述     *
          
             * 此参数必填
          */
    public void setResultDesc(string resultDesc) {
     	         	    this.resultDesc = resultDesc;
     	        }
    
    
  }
}