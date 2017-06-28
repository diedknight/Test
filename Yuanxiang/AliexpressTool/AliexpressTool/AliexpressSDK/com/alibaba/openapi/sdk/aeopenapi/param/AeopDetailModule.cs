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
public class AeopDetailModule {

       [DataMember(Order = 1)]
    private long? id;
    
        /**
       * @return 模块ID
    */
        public long? getId() {
               	return id;
            }
    
    /**
     * 设置模块ID     *
     * 参数示例：<pre>123</pre>     
             * 此参数必填
          */
    public void setId(long id) {
     	         	    this.id = id;
     	        }
    
        [DataMember(Order = 2)]
    private string name;
    
        /**
       * @return 模块名称
    */
        public string getName() {
               	return name;
            }
    
    /**
     * 设置模块名称     *
     * 参数示例：<pre>hello</pre>     
             * 此参数必填
          */
    public void setName(string name) {
     	         	    this.name = name;
     	        }
    
        [DataMember(Order = 3)]
    private string status;
    
        /**
       * @return 模块的状态
    */
        public string getStatus() {
               	return status;
            }
    
    /**
     * 设置模块的状态     *
     * 参数示例：<pre>status</pre>     
             * 此参数必填
          */
    public void setStatus(string status) {
     	         	    this.status = status;
     	        }
    
        [DataMember(Order = 4)]
    private string type;
    
        /**
       * @return 模块的类型
    */
        public string getType() {
               	return type;
            }
    
    /**
     * 设置模块的类型     *
     * 参数示例：<pre>custom</pre>     
             * 此参数必填
          */
    public void setType(string type) {
     	         	    this.type = type;
     	        }
    
        [DataMember(Order = 5)]
    private string displayContent;
    
        /**
       * @return 模块的内容
    */
        public string getDisplayContent() {
               	return displayContent;
            }
    
    /**
     * 设置模块的内容     *
     * 参数示例：<pre>hello content</pre>     
             * 此参数必填
          */
    public void setDisplayContent(string displayContent) {
     	         	    this.displayContent = displayContent;
     	        }
    
        [DataMember(Order = 6)]
    private string moduleContents;
    
        /**
       * @return 模块的内容
    */
        public string getModuleContents() {
               	return moduleContents;
            }
    
    /**
     * 设置模块的内容     *
     * 参数示例：<pre>hello content</pre>     
             * 此参数必填
          */
    public void setModuleContents(string moduleContents) {
     	         	    this.moduleContents = moduleContents;
     	        }
    
        [DataMember(Order = 7)]
    private long? aliMemberId;
    
        /**
       * @return 卖家主账户的ID
    */
        public long? getAliMemberId() {
               	return aliMemberId;
            }
    
    /**
     * 设置卖家主账户的ID     *
     * 参数示例：<pre>1006680305</pre>     
             * 此参数必填
          */
    public void setAliMemberId(long aliMemberId) {
     	         	    this.aliMemberId = aliMemberId;
     	        }
    
    
  }
}