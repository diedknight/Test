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
public class ApiFindAeProductDetailModuleListByQureyParam {

       [DataMember(Order = 1)]
    private string moduleStatus;
    
        /**
       * @return 要查询模块的状态，包含：tbd(审核不通过),auditing（审核中）,approved（审核通过）
    */
        public string getModuleStatus() {
               	return moduleStatus;
            }
    
    /**
     * 设置要查询模块的状态，包含：tbd(审核不通过),auditing（审核中）,approved（审核通过）     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setModuleStatus(string moduleStatus) {
     	         	    this.moduleStatus = moduleStatus;
     	        }
    
        [DataMember(Order = 2)]
    private string type;
    
        /**
       * @return 要查询模块的类型，包含：custom（自定义模块）,relation（关联模块）
    */
        public string getType() {
               	return type;
            }
    
    /**
     * 设置要查询模块的类型，包含：custom（自定义模块）,relation（关联模块）     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setType(string type) {
     	         	    this.type = type;
     	        }
    
        [DataMember(Order = 3)]
    private int? pageIndex;
    
        /**
       * @return 要查询当前页码，每页返回50条记录，从1开始
    */
        public int? getPageIndex() {
               	return pageIndex;
            }
    
    /**
     * 设置要查询当前页码，每页返回50条记录，从1开始     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPageIndex(int pageIndex) {
     	         	    this.pageIndex = pageIndex;
     	        }
    
    
  }
}