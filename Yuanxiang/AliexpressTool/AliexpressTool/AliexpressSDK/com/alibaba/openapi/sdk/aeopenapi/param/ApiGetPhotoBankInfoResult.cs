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
public class ApiGetPhotoBankInfoResult {

       [DataMember(Order = 1)]
    private long? capicity;
    
        /**
       * @return 图片银行总容量，单位字节。
    */
        public long? getCapicity() {
               	return capicity;
            }
    
    /**
     * 设置图片银行总容量，单位字节。     *
          
             * 此参数必填
          */
    public void setCapicity(long capicity) {
     	         	    this.capicity = capicity;
     	        }
    
        [DataMember(Order = 2)]
    private long? useage;
    
        /**
       * @return 图片银行已使用量，单位字节。
    */
        public long? getUseage() {
               	return useage;
            }
    
    /**
     * 设置图片银行已使用量，单位字节。     *
          
             * 此参数必填
          */
    public void setUseage(long useage) {
     	         	    this.useage = useage;
     	        }
    
    
  }
}