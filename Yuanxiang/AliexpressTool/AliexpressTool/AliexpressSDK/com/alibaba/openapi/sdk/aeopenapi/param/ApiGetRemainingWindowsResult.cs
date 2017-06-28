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
public class ApiGetRemainingWindowsResult {

       [DataMember(Order = 1)]
    private int? validDays;
    
        /**
       * @return 每个橱窗的有效期(单位: 天)。
    */
        public int? getValidDays() {
               	return validDays;
            }
    
    /**
     * 设置每个橱窗的有效期(单位: 天)。     *
          
             * 此参数必填
          */
    public void setValidDays(int validDays) {
     	         	    this.validDays = validDays;
     	        }
    
        [DataMember(Order = 2)]
    private int? remainingWindowCount;
    
        /**
       * @return 剩余的可用橱窗数
    */
        public int? getRemainingWindowCount() {
               	return remainingWindowCount;
            }
    
    /**
     * 设置剩余的可用橱窗数     *
          
             * 此参数必填
          */
    public void setRemainingWindowCount(int remainingWindowCount) {
     	         	    this.remainingWindowCount = remainingWindowCount;
     	        }
    
    
  }
}