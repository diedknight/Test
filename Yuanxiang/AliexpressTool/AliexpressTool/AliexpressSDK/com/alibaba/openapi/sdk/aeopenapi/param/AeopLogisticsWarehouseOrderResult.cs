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
public class AeopLogisticsWarehouseOrderResult {

       [DataMember(Order = 1)]
    private long? orderId;
    
        /**
       * @return 交易订单ID
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置交易订单ID     *
     * 参数示例：<pre>17939268153</pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 2)]
    private long? onlineLogisticsId;
    
        /**
       * @return 物流订单ID
    */
        public long? getOnlineLogisticsId() {
               	return onlineLogisticsId;
            }
    
    /**
     * 设置物流订单ID     *
     * 参数示例：<pre>1100001</pre>     
             * 此参数必填
          */
    public void setOnlineLogisticsId(long onlineLogisticsId) {
     	         	    this.onlineLogisticsId = onlineLogisticsId;
     	        }
    
        [DataMember(Order = 3)]
    private string internationalLogisticsType;
    
        /**
       * @return 国际物流订单类型（CPAM_WLB_FPXSZ小包-物流宝仓库-深圳市递四方速递FPXSZ；CPAM_WLB_ZTOBJ小包-物流宝仓库-中通海外北京仓ZTOBJ；CPAM_WLB_CPHSH小包-物流宝仓库-上海市邮政CPHSH；
    */
        public string getInternationalLogisticsType() {
               	return internationalLogisticsType;
            }
    
    /**
     * 设置国际物流订单类型（CPAM_WLB_FPXSZ小包-物流宝仓库-深圳市递四方速递FPXSZ；CPAM_WLB_ZTOBJ小包-物流宝仓库-中通海外北京仓ZTOBJ；CPAM_WLB_CPHSH小包-物流宝仓库-上海市邮政CPHSH；     *
     * 参数示例：<pre>CAINIAO_STANDARD_YANWENBJ</pre>     
             * 此参数必填
          */
    public void setInternationalLogisticsType(string internationalLogisticsType) {
     	         	    this.internationalLogisticsType = internationalLogisticsType;
     	        }
    
        [DataMember(Order = 4)]
    private string internationallogisticsId;
    
        /**
       * @return 物流运单号
    */
        public string getInternationallogisticsId() {
               	return internationallogisticsId;
            }
    
    /**
     * 设置物流运单号     *
     * 参数示例：<pre>RE700150389CN</pre>     
             * 此参数必填
          */
    public void setInternationallogisticsId(string internationallogisticsId) {
     	         	    this.internationallogisticsId = internationallogisticsId;
     	        }
    
        [DataMember(Order = 5)]
    private string logisticsStatus;
    
        /**
       * @return 物流订单状态:init等待分配物流单号；waitWarehouseReceiveGoods等待仓库操作；pickup_success揽收成功；pickup_fail揽收失败；warehouseRejectGoods入库失败；waitWarehouseSendGoods等待出库；out_stock_success等待发货；out_stock_fail出库失败；send_goods_fail发货失败；warehouseSendGoodsSuccess已发货；
    */
        public string getLogisticsStatus() {
               	return logisticsStatus;
            }
    
    /**
     * 设置物流订单状态:init等待分配物流单号；waitWarehouseReceiveGoods等待仓库操作；pickup_success揽收成功；pickup_fail揽收失败；warehouseRejectGoods入库失败；waitWarehouseSendGoods等待出库；out_stock_success等待发货；out_stock_fail出库失败；send_goods_fail发货失败；warehouseSendGoodsSuccess已发货；     *
     * 参数示例：<pre>wait_warehouse_receive_goods</pre>     
             * 此参数必填
          */
    public void setLogisticsStatus(string logisticsStatus) {
     	         	    this.logisticsStatus = logisticsStatus;
     	        }
    
        [DataMember(Order = 6)]
    private string channelCode;
    
        /**
       * @return 渠道编码
    */
        public string getChannelCode() {
               	return channelCode;
            }
    
    /**
     * 设置渠道编码     *
     * 参数示例：<pre>105</pre>     
             * 此参数必填
          */
    public void setChannelCode(string channelCode) {
     	         	    this.channelCode = channelCode;
     	        }
    
        [DataMember(Order = 7)]
    private string lpNumber;
    
        /**
       * @return LP编号
    */
        public string getLpNumber() {
               	return lpNumber;
            }
    
    /**
     * 设置LP编号     *
     * 参数示例：<pre>LP00012621594229</pre>     
             * 此参数必填
          */
    public void setLpNumber(string lpNumber) {
     	         	    this.lpNumber = lpNumber;
     	        }
    
        [DataMember(Order = 8)]
    private Money logisticsFee;
    
        /**
       * @return 运费
    */
        public Money getLogisticsFee() {
               	return logisticsFee;
            }
    
    /**
     * 设置运费     *
     * 参数示例：<pre>{{&quot;amount&quot;:6.77,&quot;cent&quot;:677,&quot;currencyCode&quot;:&quot;CNY&quot;}</pre>     
             * 此参数必填
          */
    public void setLogisticsFee(Money logisticsFee) {
     	         	    this.logisticsFee = logisticsFee;
     	        }
    
    
  }
}