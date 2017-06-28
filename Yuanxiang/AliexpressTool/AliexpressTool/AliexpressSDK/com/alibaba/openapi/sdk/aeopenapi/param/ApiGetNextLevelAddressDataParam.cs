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
public class ApiGetNextLevelAddressDataParam {

       [DataMember(Order = 1)]
    private long? areaId;
    
        /**
       * @return 区域Id
    */
        public long? getAreaId() {
               	return areaId;
            }
    
    /**
     * 设置区域Id     *
     * 参数示例：<pre>1001</pre>     
             * 此参数必填
          */
    public void setAreaId(long areaId) {
     	         	    this.areaId = areaId;
     	        }
    
    
  }
}