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
public class ApiQueryMsgDetailListParam {

       [DataMember(Order = 1)]
    private int? currentPage;
    
        /**
       * @return 当前页
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 2)]
    private int? pageSize;
    
        /**
       * @return 每页条数,pageSize取值范围(0~100) 最多返回前5000条数据
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页条数,pageSize取值范围(0~100) 最多返回前5000条数据     *
     * 参数示例：<pre>20</pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
        [DataMember(Order = 3)]
    private string channelId;
    
        /**
       * @return 通道ID，即关系ID
    */
        public string getChannelId() {
               	return channelId;
            }
    
    /**
     * 设置通道ID，即关系ID     *
     * 参数示例：<pre>22323233</pre>     
             * 此参数必填
          */
    public void setChannelId(string channelId) {
     	         	    this.channelId = channelId;
     	        }
    
        [DataMember(Order = 4)]
    private string msgSources;
    
        /**
       * @return 类型(message_center/order_msg)
    */
        public string getMsgSources() {
               	return msgSources;
            }
    
    /**
     * 设置类型(message_center/order_msg)     *
     * 参数示例：<pre>message_center</pre>     
             * 此参数必填
          */
    public void setMsgSources(string msgSources) {
     	         	    this.msgSources = msgSources;
     	        }
    
    
  }
}