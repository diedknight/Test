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
public class AeopTrackingDetailResult {

       [DataMember(Order = 1)]
    private string eventDate;
    
        /**
       * @return 时间
    */
        public DateTime? getEventDate() {
                 if (eventDate != null)
          {
              DateTime datetime = DateUtil.formatFromStr(eventDate);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置时间     *
     * 参数示例：<pre>2015-09-15 12:12:00</pre>     
             * 此参数必填
          */
    public void setEventDate(DateTime eventDate) {
     	         	    this.eventDate = DateUtil.format(eventDate);
     	        }
    
        [DataMember(Order = 2)]
    private string eventDesc;
    
        /**
       * @return 描述
    */
        public string getEventDesc() {
               	return eventDesc;
            }
    
    /**
     * 设置描述     *
     * 参数示例：<pre>BILLING INFORMATION RECEIVED</pre>     
             * 此参数必填
          */
    public void setEventDesc(string eventDesc) {
     	         	    this.eventDesc = eventDesc;
     	        }
    
        [DataMember(Order = 3)]
    private string address;
    
        /**
       * @return 地址
    */
        public string getAddress() {
               	return address;
            }
    
    /**
     * 设置地址     *
     * 参数示例：<pre>RE</pre>     
             * 此参数必填
          */
    public void setAddress(string address) {
     	         	    this.address = address;
     	        }
    
        [DataMember(Order = 4)]
    private string signedName;
    
        /**
       * @return 签收人名
    */
        public string getSignedName() {
               	return signedName;
            }
    
    /**
     * 设置签收人名     *
     * 参数示例：<pre>Mr LI</pre>     
             * 此参数必填
          */
    public void setSignedName(string signedName) {
     	         	    this.signedName = signedName;
     	        }
    
        [DataMember(Order = 5)]
    private string status;
    
        /**
       * @return 状态
    */
        public string getStatus() {
               	return status;
            }
    
    /**
     * 设置状态     *
     * 参数示例：<pre>TR</pre>     
             * 此参数必填
          */
    public void setStatus(string status) {
     	         	    this.status = status;
     	        }
    
    
  }
}