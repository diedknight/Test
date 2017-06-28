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
public class WarrantyInforceApiDTO {

       [DataMember(Order = 1)]
    private long? orderId;
    
        /**
       * @return 主订单ID
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置主订单ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 2)]
    private string snapshotId;
    
        /**
       * @return 交易快照id
    */
        public string getSnapshotId() {
               	return snapshotId;
            }
    
    /**
     * 设置交易快照id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSnapshotId(string snapshotId) {
     	         	    this.snapshotId = snapshotId;
     	        }
    
        [DataMember(Order = 3)]
    private string createTime;
    
        /**
       * @return 创建时间
    */
        public string getCreateTime() {
               	return createTime;
            }
    
    /**
     * 设置创建时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCreateTime(string createTime) {
     	         	    this.createTime = createTime;
     	        }
    
        [DataMember(Order = 4)]
    private long? bizId;
    
        /**
       * @return 业务id
    */
        public long? getBizId() {
               	return bizId;
            }
    
    /**
     * 设置业务id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBizId(long bizId) {
     	         	    this.bizId = bizId;
     	        }
    
        [DataMember(Order = 5)]
    private string startTime;
    
        /**
       * @return 保修生效时间
    */
        public string getStartTime() {
               	return startTime;
            }
    
    /**
     * 设置保修生效时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setStartTime(string startTime) {
     	         	    this.startTime = startTime;
     	        }
    
    
  }
}