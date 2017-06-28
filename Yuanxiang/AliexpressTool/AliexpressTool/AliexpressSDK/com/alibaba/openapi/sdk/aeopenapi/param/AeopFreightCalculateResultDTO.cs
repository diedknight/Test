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
public class AeopFreightCalculateResultDTO {

       [DataMember(Order = 1)]
    private int? errorCode;
    
        /**
       * @return 错误代码
    */
        public int? getErrorCode() {
               	return errorCode;
            }
    
    /**
     * 设置错误代码     *
     * 参数示例：<pre>1表示没有错误；-1表示参数不完整；-2表示计算限制，无法计算结果；-3表示货币错误或转换错误；-4表示无有效的运费头数据；-5表示无有效的运费数据
-6表示卖家设置的全局不支持的国家；-7表示不支持的国家；-8表示商品小于起购量；-9表示大小包由于纠纷限制；-10表示产品在搜索中不存在；-11表示商品重量大于卖家设置的重量区间结束值；
-12表示商品或国家满足不支持的规则；-100表示其他错误</pre>     
             * 此参数必填
          */
    public void setErrorCode(int errorCode) {
     	         	    this.errorCode = errorCode;
     	        }
    
        [DataMember(Order = 2)]
    private string serviceName;
    
        /**
       * @return 物流方式名称
    */
        public string getServiceName() {
               	return serviceName;
            }
    
    /**
     * 设置物流方式名称     *
     * 参数示例：<pre>CPAM</pre>     
             * 此参数必填
          */
    public void setServiceName(string serviceName) {
     	         	    this.serviceName = serviceName;
     	        }
    
        [DataMember(Order = 3)]
    private Money freight;
    
        /**
       * @return 实际运费值
    */
        public Money getFreight() {
               	return freight;
            }
    
    /**
     * 设置实际运费值     *
     * 参数示例：<pre>{&quot;cent&quot;:677,&quot;amount&quot;:6.77,&quot;currencyCode&quot;:&quot;USD&quot;}</pre>     
             * 此参数必填
          */
    public void setFreight(Money freight) {
     	         	    this.freight = freight;
     	        }
    
        [DataMember(Order = 4)]
    private Money standardFreight;
    
        /**
       * @return 标准运费值
    */
        public Money getStandardFreight() {
               	return standardFreight;
            }
    
    /**
     * 设置标准运费值     *
     * 参数示例：<pre>{&quot;cent&quot;:677,&quot;amount&quot;:6.77,&quot;currencyCode&quot;:&quot;USD&quot;}</pre>     
             * 此参数必填
          */
    public void setStandardFreight(Money standardFreight) {
     	         	    this.standardFreight = standardFreight;
     	        }
    
    
  }
}