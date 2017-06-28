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
public class AlibabaAeWarrantieGetListParam {

       [DataMember(Order = 1)]
    private string supplierId;
    
        /**
       * @return 供应商id
    */
        public string getSupplierId() {
               	return supplierId;
            }
    
    /**
     * 设置供应商id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSupplierId(string supplierId) {
     	         	    this.supplierId = supplierId;
     	        }
    
        [DataMember(Order = 2)]
    private long? orderId;
    
        /**
       * @return 订单id
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置订单id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 3)]
    private string startBuyTime;
    
        /**
       * @return 服务购买开始时间
    */
        public string getStartBuyTime() {
               	return startBuyTime;
            }
    
    /**
     * 设置服务购买开始时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setStartBuyTime(string startBuyTime) {
     	         	    this.startBuyTime = startBuyTime;
     	        }
    
        [DataMember(Order = 4)]
    private string endBuyTime;
    
        /**
       * @return 服务购买结束时间
    */
        public string getEndBuyTime() {
               	return endBuyTime;
            }
    
    /**
     * 设置服务购买结束时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setEndBuyTime(string endBuyTime) {
     	         	    this.endBuyTime = endBuyTime;
     	        }
    
        [DataMember(Order = 5)]
    private string startCreateTime;
    
        /**
       * @return 服务判定生效开始时间
    */
        public string getStartCreateTime() {
               	return startCreateTime;
            }
    
    /**
     * 设置服务判定生效开始时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setStartCreateTime(string startCreateTime) {
     	         	    this.startCreateTime = startCreateTime;
     	        }
    
        [DataMember(Order = 6)]
    private string endCreateTime;
    
        /**
       * @return 服务判定生效结束时间
    */
        public string getEndCreateTime() {
               	return endCreateTime;
            }
    
    /**
     * 设置服务判定生效结束时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setEndCreateTime(string endCreateTime) {
     	         	    this.endCreateTime = endCreateTime;
     	        }
    
        [DataMember(Order = 7)]
    private int? pageSize;
    
        /**
       * @return 每页获取条数
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页获取条数     *
     * 参数示例：<pre>200</pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
        [DataMember(Order = 8)]
    private int? pageNo;
    
        /**
       * @return 页码
    */
        public int? getPageNo() {
               	return pageNo;
            }
    
    /**
     * 设置页码     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setPageNo(int pageNo) {
     	         	    this.pageNo = pageNo;
     	        }
    
    
  }
}