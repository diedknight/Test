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
public class AeopWlCreateWarehouseOrderResultDTO {

       [DataMember(Order = 1)]
    private bool? success;
    
        /**
       * @return 创建订单是否成功
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置创建订单是否成功     *
     * 参数示例：<pre>true</pre>     
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
        [DataMember(Order = 2)]
    private long? warehouseOrderId;
    
        /**
       * @return 物流订单号
    */
        public long? getWarehouseOrderId() {
               	return warehouseOrderId;
            }
    
    /**
     * 设置物流订单号     *
     * 参数示例：<pre>3017539175</pre>     
             * 此参数必填
          */
    public void setWarehouseOrderId(long warehouseOrderId) {
     	         	    this.warehouseOrderId = warehouseOrderId;
     	        }
    
        [DataMember(Order = 3)]
    private string intlTrackingNo;
    
        /**
       * @return 国际运单号
    */
        public string getIntlTrackingNo() {
               	return intlTrackingNo;
            }
    
    /**
     * 设置国际运单号     *
     * 参数示例：<pre>LN123123123CN</pre>     
             * 此参数必填
          */
    public void setIntlTrackingNo(string intlTrackingNo) {
     	         	    this.intlTrackingNo = intlTrackingNo;
     	        }
    
        [DataMember(Order = 4)]
    private string tradeOrderFrom;
    
        /**
       * @return 交易订单来源(ESCROW)
    */
        public string getTradeOrderFrom() {
               	return tradeOrderFrom;
            }
    
    /**
     * 设置交易订单来源(ESCROW)     *
     * 参数示例：<pre>ESCROW</pre>     
             * 此参数必填
          */
    public void setTradeOrderFrom(string tradeOrderFrom) {
     	         	    this.tradeOrderFrom = tradeOrderFrom;
     	        }
    
        [DataMember(Order = 5)]
    private long? tradeOrderId;
    
        /**
       * @return 关联的交易订单号
    */
        public long? getTradeOrderId() {
               	return tradeOrderId;
            }
    
    /**
     * 设置关联的交易订单号     *
     * 参数示例：<pre>66715700375804</pre>     
             * 此参数必填
          */
    public void setTradeOrderId(long tradeOrderId) {
     	         	    this.tradeOrderId = tradeOrderId;
     	        }
    
        [DataMember(Order = 6)]
    private long? outOrderId;
    
        /**
       * @return 外部订单号
    */
        public long? getOutOrderId() {
               	return outOrderId;
            }
    
    /**
     * 设置外部订单号     *
     * 参数示例：<pre>35631664365</pre>     
             * 此参数必填
          */
    public void setOutOrderId(long outOrderId) {
     	         	    this.outOrderId = outOrderId;
     	        }
    
        [DataMember(Order = 7)]
    private int? errorCode;
    
        /**
       * @return 创建时错误码(1表示无错误)
    */
        public int? getErrorCode() {
               	return errorCode;
            }
    
    /**
     * 设置创建时错误码(1表示无错误)     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setErrorCode(int errorCode) {
     	         	    this.errorCode = errorCode;
     	        }
    
        [DataMember(Order = 8)]
    private string errorDesc;
    
        /**
       * @return 创建时错误信息
    */
        public string getErrorDesc() {
               	return errorDesc;
            }
    
    /**
     * 设置创建时错误信息     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setErrorDesc(string errorDesc) {
     	         	    this.errorDesc = errorDesc;
     	        }
    
    
  }
}