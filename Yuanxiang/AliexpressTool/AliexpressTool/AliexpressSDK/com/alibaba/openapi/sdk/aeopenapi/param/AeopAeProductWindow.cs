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
public class AeopAeProductWindow {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 被推荐的产品ID
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置被推荐的产品ID     *
     * 参数示例：<pre>1234</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private string enabledDate;
    
        /**
       * @return 橱窗的开始生效时间。
    */
        public DateTime? getEnabledDate() {
                 if (enabledDate != null)
          {
              DateTime datetime = DateUtil.formatFromStr(enabledDate);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置橱窗的开始生效时间。     *
     * 参数示例：<pre>20150423224923499-0700</pre>     
             * 此参数必填
          */
    public void setEnabledDate(DateTime enabledDate) {
     	         	    this.enabledDate = DateUtil.format(enabledDate);
     	        }
    
        [DataMember(Order = 3)]
    private string expiredDate;
    
        /**
       * @return 橱窗的失效时间。
    */
        public DateTime? getExpiredDate() {
                 if (expiredDate != null)
          {
              DateTime datetime = DateUtil.formatFromStr(expiredDate);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置橱窗的失效时间。     *
     * 参数示例：<pre>20150423224923499-0700</pre>     
             * 此参数必填
          */
    public void setExpiredDate(DateTime expiredDate) {
     	         	    this.expiredDate = DateUtil.format(expiredDate);
     	        }
    
        [DataMember(Order = 4)]
    private long? remainingDays;
    
        /**
       * @return 当前橱窗的剩余有效天数。
    */
        public long? getRemainingDays() {
               	return remainingDays;
            }
    
    /**
     * 设置当前橱窗的剩余有效天数。     *
     * 参数示例：<pre>3</pre>     
             * 此参数必填
          */
    public void setRemainingDays(long remainingDays) {
     	         	    this.remainingDays = remainingDays;
     	        }
    
    
  }
}