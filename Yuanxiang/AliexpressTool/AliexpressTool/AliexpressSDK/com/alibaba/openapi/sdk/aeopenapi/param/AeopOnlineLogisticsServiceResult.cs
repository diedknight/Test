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
public class AeopOnlineLogisticsServiceResult {

       [DataMember(Order = 1)]
    private string logisticsServiceId;
    
        /**
       * @return 物流方案ID
    */
        public string getLogisticsServiceId() {
               	return logisticsServiceId;
            }
    
    /**
     * 设置物流方案ID     *
     * 参数示例：<pre>CPAM_WLB_CPHSH</pre>     
             * 此参数必填
          */
    public void setLogisticsServiceId(string logisticsServiceId) {
     	         	    this.logisticsServiceId = logisticsServiceId;
     	        }
    
        [DataMember(Order = 2)]
    private string logisticsServiceName;
    
        /**
       * @return 物流方案名称
    */
        public string getLogisticsServiceName() {
               	return logisticsServiceName;
            }
    
    /**
     * 设置物流方案名称     *
     * 参数示例：<pre>速邮宝(中邮小包)</pre>     
             * 此参数必填
          */
    public void setLogisticsServiceName(string logisticsServiceName) {
     	         	    this.logisticsServiceName = logisticsServiceName;
     	        }
    
        [DataMember(Order = 3)]
    private string logisticsTimeliness;
    
        /**
       * @return 运输时效
    */
        public string getLogisticsTimeliness() {
               	return logisticsTimeliness;
            }
    
    /**
     * 设置运输时效     *
     * 参数示例：<pre>15-50天</pre>     
             * 此参数必填
          */
    public void setLogisticsTimeliness(string logisticsTimeliness) {
     	         	    this.logisticsTimeliness = logisticsTimeliness;
     	        }
    
        [DataMember(Order = 4)]
    private string deliveryAddress;
    
        /**
       * @return 交货地址
    */
        public string getDeliveryAddress() {
               	return deliveryAddress;
            }
    
    /**
     * 设置交货地址     *
     * 参数示例：<pre>上海市徐汇区百色路1218号生产大楼一层(速卖通)</pre>     
             * 此参数必填
          */
    public void setDeliveryAddress(string deliveryAddress) {
     	         	    this.deliveryAddress = deliveryAddress;
     	        }
    
        [DataMember(Order = 5)]
    private string trialResult;
    
        /**
       * @return 试算结果
    */
        public string getTrialResult() {
               	return trialResult;
            }
    
    /**
     * 设置试算结果     *
     * 参数示例：<pre>CN¥87.00</pre>     
             * 此参数必填
          */
    public void setTrialResult(string trialResult) {
     	         	    this.trialResult = trialResult;
     	        }
    
        [DataMember(Order = 6)]
    private string warehouseName;
    
        /**
       * @return 仓库中文名称
    */
        public string getWarehouseName() {
               	return warehouseName;
            }
    
    /**
     * 设置仓库中文名称     *
     * 参数示例：<pre>中邮北京仓</pre>     
             * 此参数必填
          */
    public void setWarehouseName(string warehouseName) {
     	         	    this.warehouseName = warehouseName;
     	        }
    
    
  }
}