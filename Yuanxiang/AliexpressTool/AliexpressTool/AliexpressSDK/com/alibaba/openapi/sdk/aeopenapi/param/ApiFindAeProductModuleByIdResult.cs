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
public class ApiFindAeProductModuleByIdResult {

       [DataMember(Order = 1)]
    private long? id;
    
        /**
       * @return 模块的id
    */
        public long? getId() {
               	return id;
            }
    
    /**
     * 设置模块的id     *
          
             * 此参数必填
          */
    public void setId(long id) {
     	         	    this.id = id;
     	        }
    
        [DataMember(Order = 2)]
    private string gmtCreate;
    
        /**
       * @return 模块的创建时间
    */
        public DateTime? getGmtCreate() {
                 if (gmtCreate != null)
          {
              DateTime datetime = DateUtil.formatFromStr(gmtCreate);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置模块的创建时间     *
          
             * 此参数必填
          */
    public void setGmtCreate(DateTime gmtCreate) {
     	         	    this.gmtCreate = DateUtil.format(gmtCreate);
     	        }
    
        [DataMember(Order = 3)]
    private string gmtModified;
    
        /**
       * @return 模块的最近一次修改时间
    */
        public DateTime? getGmtModified() {
                 if (gmtModified != null)
          {
              DateTime datetime = DateUtil.formatFromStr(gmtModified);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置模块的最近一次修改时间     *
          
             * 此参数必填
          */
    public void setGmtModified(DateTime gmtModified) {
     	         	    this.gmtModified = DateUtil.format(gmtModified);
     	        }
    
        [DataMember(Order = 4)]
    private string name;
    
        /**
       * @return 模块的名称
    */
        public string getName() {
               	return name;
            }
    
    /**
     * 设置模块的名称     *
          
             * 此参数必填
          */
    public void setName(string name) {
     	         	    this.name = name;
     	        }
    
        [DataMember(Order = 5)]
    private string type;
    
        /**
       * @return 模块的类型
    */
        public string getType() {
               	return type;
            }
    
    /**
     * 设置模块的类型     *
          
             * 此参数必填
          */
    public void setType(string type) {
     	         	    this.type = type;
     	        }
    
        [DataMember(Order = 6)]
    private string status;
    
        /**
       * @return 模块的状态
    */
        public string getStatus() {
               	return status;
            }
    
    /**
     * 设置模块的状态     *
          
             * 此参数必填
          */
    public void setStatus(string status) {
     	         	    this.status = status;
     	        }
    
        [DataMember(Order = 7)]
    private string moduleContents;
    
        /**
       * @return 模块的内容
    */
        public string getModuleContents() {
               	return moduleContents;
            }
    
    /**
     * 设置模块的内容     *
          
             * 此参数必填
          */
    public void setModuleContents(string moduleContents) {
     	         	    this.moduleContents = moduleContents;
     	        }
    
        [DataMember(Order = 8)]
    private long? aliMemberId;
    
        /**
       * @return 这个模块所有者的主账户ID
    */
        public long? getAliMemberId() {
               	return aliMemberId;
            }
    
    /**
     * 设置这个模块所有者的主账户ID     *
          
             * 此参数必填
          */
    public void setAliMemberId(long aliMemberId) {
     	         	    this.aliMemberId = aliMemberId;
     	        }
    
        [DataMember(Order = 9)]
    private string displayContent;
    
        /**
       * @return 
    */
        public string getDisplayContent() {
               	return displayContent;
            }
    
    /**
     * 设置     *
          
             * 此参数必填
          */
    public void setDisplayContent(string displayContent) {
     	         	    this.displayContent = displayContent;
     	        }
    
        [DataMember(Order = 10)]
    private bool? success;
    
        /**
       * @return 接口的调用结果。true/false分别表示成功和失败。
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置接口的调用结果。true/false分别表示成功和失败。     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}