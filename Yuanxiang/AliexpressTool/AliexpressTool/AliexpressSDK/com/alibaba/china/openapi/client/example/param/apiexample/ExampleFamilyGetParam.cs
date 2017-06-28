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
class ExampleFamilyGetParam {

       [DataMember(Order = 1)]
    private int? familyNumber;
    
        /**
       * @return 可接受参数1或者2，其余参数无法找到family对象
    */
        public int? getFamilyNumber() {
               	return familyNumber;
            }
    
    /**
     * 设置可接受参数1或者2，其余参数无法找到family对象     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFamilyNumber(int familyNumber) {
     	         	    this.familyNumber = familyNumber;
     	        }
    
    
  }
}