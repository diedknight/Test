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
public class ApiQueryWarrantiesInforceParam {

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
    private string startTime;
    
        /**
       * @return 开始时间
    */
        public string getStartTime() {
               	return startTime;
            }
    
    /**
     * 设置开始时间     *
     * 参数示例：<pre>2016-01-06 00:00:00</pre>     
             * 此参数必填
          */
    public void setStartTime(string startTime) {
     	         	    this.startTime = startTime;
     	        }
    
        [DataMember(Order = 3)]
    private string endTime;
    
        /**
       * @return 结束时间
    */
        public string getEndTime() {
               	return endTime;
            }
    
    /**
     * 设置结束时间     *
     * 参数示例：<pre>2016-01-06 00:00:00</pre>     
             * 此参数必填
          */
    public void setEndTime(string endTime) {
     	         	    this.endTime = endTime;
     	        }
    
        [DataMember(Order = 4)]
    private int? pageSize;
    
        /**
       * @return 页面大小(不得超过200)
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置页面大小(不得超过200)     *
     * 参数示例：<pre>50</pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
        [DataMember(Order = 5)]
    private int? pageNo;
    
        /**
       * @return 显示的页码
    */
        public int? getPageNo() {
               	return pageNo;
            }
    
    /**
     * 设置显示的页码     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setPageNo(int pageNo) {
     	         	    this.pageNo = pageNo;
     	        }
    
    
  }
}