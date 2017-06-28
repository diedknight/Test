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
public class ApiSellerShipmentParam {

       [DataMember(Order = 1)]
    private string serviceName;
    
        /**
       * @return 用户选择的实际发货物流服务（物流服务key：该接口根据api.listLogisticsService列出平台所支持的物流服务 进行获取目前所支持的物流。平台支持物流服务详细一览表详见论坛链接http://bbs.seller.aliexpress.com/bbs/read.php?tid=266120&page=1&toread=1#tpc）
    */
        public string getServiceName() {
               	return serviceName;
            }
    
    /**
     * 设置用户选择的实际发货物流服务（物流服务key：该接口根据api.listLogisticsService列出平台所支持的物流服务 进行获取目前所支持的物流。平台支持物流服务详细一览表详见论坛链接http://bbs.seller.aliexpress.com/bbs/read.php?tid=266120&page=1&toread=1#tpc）     *
     * 参数示例：<pre>AUSPOST, ROYAL_MAIL, CORREOS, DEUTSCHE_POST, LAPOSTE, POSTEITALIANE, RUSSIAN_POST, USPS, UPS_US, UPS, JNE, ACOMMERCE, UPSE, DHL_UK, DHL_ES, DHL_IT, DHL_DE, ENVIALIA, DHL_FR, DHL, FEDEX, FEDEX_IE, TNT, SF, EMS, ROYAL_MAIL_PY, EMS_ZX_ZX_US, E_EMS, EMS_SH_ZX_US, SINOTRANS_AM, ITELLA_PY, ITELLA, CPAM, SINOTRANS_PY, YANWEN_JYT, CPAP, TOLL, HKPAM, HKPAP, SGP, CHP, SEP, ARAMEX, ECONOMIC139, SPSR_RU, YANWEN_AM, CPAM_HRB, CTR_LAND_PICKUP, SPSR_CN, POST_NL, POST_MY, OTHER_ES, OTHER_IT, OTHER_FR, OTHER_US, 
 OTHER_UK, OTHER_RU, OTHER_DE, OTHER_AU, Other</pre>     
             * 此参数必填
          */
    public void setServiceName(string serviceName) {
     	         	    this.serviceName = serviceName;
     	        }
    
        [DataMember(Order = 2)]
    private string logisticsNo;
    
        /**
       * @return 物流追踪号
    */
        public string getLogisticsNo() {
               	return logisticsNo;
            }
    
    /**
     * 设置物流追踪号     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLogisticsNo(string logisticsNo) {
     	         	    this.logisticsNo = logisticsNo;
     	        }
    
        [DataMember(Order = 3)]
    private string description;
    
        /**
       * @return 备注(只能输入英文，且长度限制在512个字符。）
    */
        public string getDescription() {
               	return description;
            }
    
    /**
     * 设置备注(只能输入英文，且长度限制在512个字符。）     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setDescription(string description) {
     	         	    this.description = description;
     	        }
    
        [DataMember(Order = 4)]
    private string sendType;
    
        /**
       * @return 状态包括：全部发货(all)、部分发货(part)
    */
        public string getSendType() {
               	return sendType;
            }
    
    /**
     * 设置状态包括：全部发货(all)、部分发货(part)     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSendType(string sendType) {
     	         	    this.sendType = sendType;
     	        }
    
        [DataMember(Order = 5)]
    private string outRef;
    
        /**
       * @return 用户需要发货的订单id
    */
        public string getOutRef() {
               	return outRef;
            }
    
    /**
     * 设置用户需要发货的订单id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOutRef(string outRef) {
     	         	    this.outRef = outRef;
     	        }
    
        [DataMember(Order = 6)]
    private string trackingWebsite;
    
        /**
       * @return 当serviceName=Other的情况时，需要填写对应的追踪网址
    */
        public string getTrackingWebsite() {
               	return trackingWebsite;
            }
    
    /**
     * 设置当serviceName=Other的情况时，需要填写对应的追踪网址     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setTrackingWebsite(string trackingWebsite) {
     	         	    this.trackingWebsite = trackingWebsite;
     	        }
    
    
  }
}