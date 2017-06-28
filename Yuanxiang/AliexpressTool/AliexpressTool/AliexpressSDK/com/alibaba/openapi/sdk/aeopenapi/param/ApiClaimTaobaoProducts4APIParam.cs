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
public class ApiClaimTaobaoProducts4APIParam {

       [DataMember(Order = 1)]
    private string url;
    
        /**
       * @return 淘宝或者天猫产品的detail url
    */
        public string getUrl() {
               	return url;
            }
    
    /**
     * 设置淘宝或者天猫产品的detail url     *
     * 参数示例：<pre>http://detail.tmall.com/item.htm?spm=a2106.m861.1000384.1.vxsG6t&id=13187863348&source=dou&scm=1029.newlist-0.tagbeta.50000582&ppath=&sku=&ug=</pre>     
             * 此参数必填
          */
    public void setUrl(string url) {
     	         	    this.url = url;
     	        }
    
    
  }
}