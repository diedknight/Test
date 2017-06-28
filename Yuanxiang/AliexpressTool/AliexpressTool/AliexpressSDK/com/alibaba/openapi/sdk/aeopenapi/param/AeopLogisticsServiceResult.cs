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
public class AeopLogisticsServiceResult {

       [DataMember(Order = 1)]
    private string logisticsCompany;
    
        /**
       * @return 物流公司
    */
        public string getLogisticsCompany() {
               	return logisticsCompany;
            }
    
    /**
     * 设置物流公司     *
     * 参数示例：<pre>CPAM</pre>     
             * 此参数必填
          */
    public void setLogisticsCompany(string logisticsCompany) {
     	         	    this.logisticsCompany = logisticsCompany;
     	        }
    
        [DataMember(Order = 2)]
    private string displayName;
    
        /**
       * @return 展示名称
    */
        public string getDisplayName() {
               	return displayName;
            }
    
    /**
     * 设置展示名称     *
     * 参数示例：<pre>China Post Registered Air Mail</pre>     
             * 此参数必填
          */
    public void setDisplayName(string displayName) {
     	         	    this.displayName = displayName;
     	        }
    
        [DataMember(Order = 3)]
    private string serviceName;
    
        /**
       * @return 物流服务key
    */
        public string getServiceName() {
               	return serviceName;
            }
    
    /**
     * 设置物流服务key     *
     * 参数示例：<pre>CPAM</pre>     
             * 此参数必填
          */
    public void setServiceName(string serviceName) {
     	         	    this.serviceName = serviceName;
     	        }
    
        [DataMember(Order = 4)]
    private int? minProcessDay;
    
        /**
       * @return 最小处理时间
    */
        public int? getMinProcessDay() {
               	return minProcessDay;
            }
    
    /**
     * 设置最小处理时间     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setMinProcessDay(int minProcessDay) {
     	         	    this.minProcessDay = minProcessDay;
     	        }
    
        [DataMember(Order = 5)]
    private int? maxProcessDay;
    
        /**
       * @return 最大处理时间
    */
        public int? getMaxProcessDay() {
               	return maxProcessDay;
            }
    
    /**
     * 设置最大处理时间     *
     * 参数示例：<pre>5</pre>     
             * 此参数必填
          */
    public void setMaxProcessDay(int maxProcessDay) {
     	         	    this.maxProcessDay = maxProcessDay;
     	        }
    
        [DataMember(Order = 6)]
    private string trackingNoRegex;
    
        /**
       * @return 物流追踪号码校验规则，采用正则表达式
    */
        public string getTrackingNoRegex() {
               	return trackingNoRegex;
            }
    
    /**
     * 设置物流追踪号码校验规则，采用正则表达式     *
     * 参数示例：<pre>^[a-zA-z]{2}[A-Za-z0-9]{9}[a-zA-z]{2}$</pre>     
             * 此参数必填
          */
    public void setTrackingNoRegex(string trackingNoRegex) {
     	         	    this.trackingNoRegex = trackingNoRegex;
     	        }
    
        [DataMember(Order = 7)]
    private int? recommendOrder;
    
        /**
       * @return 推荐显示排序
    */
        public int? getRecommendOrder() {
               	return recommendOrder;
            }
    
    /**
     * 设置推荐显示排序     *
     * 参数示例：<pre>110</pre>     
             * 此参数必填
          */
    public void setRecommendOrder(int recommendOrder) {
     	         	    this.recommendOrder = recommendOrder;
     	        }
    
    
  }
}