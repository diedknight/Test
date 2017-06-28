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
public class ApiSellerModifiedShipmentParam {

       [DataMember(Order = 1)]
    private string oldServiceName;
    
        /**
       * @return 用户需要修改的的老的发货物流服务（物流服务key：该接口根据api.listLogisticsService列出平台所支持的物流服务 进行获取目前所支持的物流。）
    */
        public string getOldServiceName() {
               	return oldServiceName;
            }
    
    /**
     * 设置用户需要修改的的老的发货物流服务（物流服务key：该接口根据api.listLogisticsService列出平台所支持的物流服务 进行获取目前所支持的物流。）     *
     * 参数示例：<pre>EMS_SH_ZX_US；EMS；SEP；FEDEX；UPSE；FEDEX_IE；RUSTON；HKPAP；CPAM；SF；HKPAM；CHP；ZTORU；ARAMEX；CPAP；TNT；ECONOMIC139；DHL；UPS；SGP；</pre>     
             * 此参数必填
          */
    public void setOldServiceName(string oldServiceName) {
     	         	    this.oldServiceName = oldServiceName;
     	        }
    
        [DataMember(Order = 2)]
    private string oldLogisticsNo;
    
        /**
       * @return 用户需要修改的老的物流追踪号
    */
        public string getOldLogisticsNo() {
               	return oldLogisticsNo;
            }
    
    /**
     * 设置用户需要修改的老的物流追踪号     *
     * 参数示例：<pre>CP123456789CN</pre>     
             * 此参数必填
          */
    public void setOldLogisticsNo(string oldLogisticsNo) {
     	         	    this.oldLogisticsNo = oldLogisticsNo;
     	        }
    
        [DataMember(Order = 3)]
    private string newServiceName;
    
        /**
       * @return 用户需要修改的的新的发货物流服务（物流服务key：该接口根据api.listLogisticsService列出平台所支持的物流服务 进行获取目前所支持的物流。）
    */
        public string getNewServiceName() {
               	return newServiceName;
            }
    
    /**
     * 设置用户需要修改的的新的发货物流服务（物流服务key：该接口根据api.listLogisticsService列出平台所支持的物流服务 进行获取目前所支持的物流。）     *
     * 参数示例：<pre>EMS_SH_ZX_US；EMS；SEP；FEDEX；UPSE；FEDEX_IE；RUSTON；HKPAP；CPAM；SF；HKPAM；CHP；ZTORU；ARAMEX；CPAP；TNT；ECONOMIC139；DHL；UPS；SGP；</pre>     
             * 此参数必填
          */
    public void setNewServiceName(string newServiceName) {
     	         	    this.newServiceName = newServiceName;
     	        }
    
        [DataMember(Order = 4)]
    private string newLogisticsNo;
    
        /**
       * @return 用户需要修改的老的物流追踪号
    */
        public string getNewLogisticsNo() {
               	return newLogisticsNo;
            }
    
    /**
     * 设置用户需要修改的老的物流追踪号     *
     * 参数示例：<pre>CP123456123CN</pre>     
             * 此参数必填
          */
    public void setNewLogisticsNo(string newLogisticsNo) {
     	         	    this.newLogisticsNo = newLogisticsNo;
     	        }
    
        [DataMember(Order = 5)]
    private string description;
    
        /**
       * @return 备注(只能输入英文)
    */
        public string getDescription() {
               	return description;
            }
    
    /**
     * 设置备注(只能输入英文)     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setDescription(string description) {
     	         	    this.description = description;
     	        }
    
        [DataMember(Order = 6)]
    private string sendType;
    
        /**
       * @return 状态包括：全部发货(all)、部分发货(part)
    */
        public string getSendType() {
               	return sendType;
            }
    
    /**
     * 设置状态包括：全部发货(all)、部分发货(part)     *
     * 参数示例：<pre>all</pre>     
             * 此参数必填
          */
    public void setSendType(string sendType) {
     	         	    this.sendType = sendType;
     	        }
    
        [DataMember(Order = 7)]
    private string outRef;
    
        /**
       * @return 用户需要发货的订单id
    */
        public string getOutRef() {
               	return outRef;
            }
    
    /**
     * 设置用户需要发货的订单id     *
     * 参数示例：<pre>60769040695804</pre>     
             * 此参数必填
          */
    public void setOutRef(string outRef) {
     	         	    this.outRef = outRef;
     	        }
    
        [DataMember(Order = 8)]
    private string trackingWebsite;
    
        /**
       * @return 当serviceName=other的情况时，需要填写对应的追踪网址
    */
        public string getTrackingWebsite() {
               	return trackingWebsite;
            }
    
    /**
     * 设置当serviceName=other的情况时，需要填写对应的追踪网址     *
     * 参数示例：<pre>www.intl183.com</pre>     
             * 此参数必填
          */
    public void setTrackingWebsite(string trackingWebsite) {
     	         	    this.trackingWebsite = trackingWebsite;
     	        }
    
    
  }
}