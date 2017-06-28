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
public class AeopAnouncementDTO {

       [DataMember(Order = 1)]
    private string anouncementId;
    
        /**
       * @return 公告id
    */
        public string getAnouncementId() {
               	return anouncementId;
            }
    
    /**
     * 设置公告id     *
     * 参数示例：<pre>12222222</pre>     
             * 此参数必填
          */
    public void setAnouncementId(string anouncementId) {
     	         	    this.anouncementId = anouncementId;
     	        }
    
        [DataMember(Order = 2)]
    private string businessType;
    
        /**
       * @return 业务类型
    */
        public string getBusinessType() {
               	return businessType;
            }
    
    /**
     * 设置业务类型     *
     * 参数示例：<pre>放款</pre>     
             * 此参数必填
          */
    public void setBusinessType(string businessType) {
     	         	    this.businessType = businessType;
     	        }
    
        [DataMember(Order = 3)]
    private string createDatetime;
    
        /**
       * @return 公告时间
    */
        public string getCreateDatetime() {
               	return createDatetime;
            }
    
    /**
     * 设置公告时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCreateDatetime(string createDatetime) {
     	         	    this.createDatetime = createDatetime;
     	        }
    
        [DataMember(Order = 4)]
    private string content;
    
        /**
       * @return 公告内容
    */
        public string getContent() {
               	return content;
            }
    
    /**
     * 设置公告内容     *
     * 参数示例：<pre>1202速卖通平台将开展大促。。。。。</pre>     
             * 此参数必填
          */
    public void setContent(string content) {
     	         	    this.content = content;
     	        }
    
    
  }
}