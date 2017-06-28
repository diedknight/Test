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
public class ChinaAddressItemDTO {

       [DataMember(Order = 1)]
    private long? areaId;
    
        /**
       * @return 地址区域的ID
    */
        public long? getAreaId() {
               	return areaId;
            }
    
    /**
     * 设置地址区域的ID     *
     * 参数示例：<pre>10001</pre>     
             * 此参数必填
          */
    public void setAreaId(long areaId) {
     	         	    this.areaId = areaId;
     	        }
    
        [DataMember(Order = 2)]
    private string cnDiplayName;
    
        /**
       * @return 中文展示名称
    */
        public string getCnDiplayName() {
               	return cnDiplayName;
            }
    
    /**
     * 设置中文展示名称     *
     * 参数示例：<pre>北京市</pre>     
             * 此参数必填
          */
    public void setCnDiplayName(string cnDiplayName) {
     	         	    this.cnDiplayName = cnDiplayName;
     	        }
    
        [DataMember(Order = 3)]
    private string pyDiplayName;
    
        /**
       * @return 英文文展示名称
    */
        public string getPyDiplayName() {
               	return pyDiplayName;
            }
    
    /**
     * 设置英文文展示名称     *
     * 参数示例：<pre>bei jing shi</pre>     
             * 此参数必填
          */
    public void setPyDiplayName(string pyDiplayName) {
     	         	    this.pyDiplayName = pyDiplayName;
     	        }
    
    
  }
}