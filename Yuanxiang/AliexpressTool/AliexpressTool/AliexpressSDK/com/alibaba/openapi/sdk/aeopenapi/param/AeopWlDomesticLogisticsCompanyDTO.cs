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
public class AeopWlDomesticLogisticsCompanyDTO {

       [DataMember(Order = 1)]
    private long? companyId;
    
        /**
       * @return 物流公司ID
    */
        public long? getCompanyId() {
               	return companyId;
            }
    
    /**
     * 设置物流公司ID     *
     * 参数示例：<pre>101</pre>     
             * 此参数必填
          */
    public void setCompanyId(long companyId) {
     	         	    this.companyId = companyId;
     	        }
    
        [DataMember(Order = 2)]
    private string name;
    
        /**
       * @return 物流公司名称
    */
        public string getName() {
               	return name;
            }
    
    /**
     * 设置物流公司名称     *
     * 参数示例：<pre>圆通速递</pre>     
             * 此参数必填
          */
    public void setName(string name) {
     	         	    this.name = name;
     	        }
    
        [DataMember(Order = 3)]
    private string companyCode;
    
        /**
       * @return 物流公司编码
    */
        public string getCompanyCode() {
               	return companyCode;
            }
    
    /**
     * 设置物流公司编码     *
     * 参数示例：<pre>YTO</pre>     
             * 此参数必填
          */
    public void setCompanyCode(string companyCode) {
     	         	    this.companyCode = companyCode;
     	        }
    
    
  }
}