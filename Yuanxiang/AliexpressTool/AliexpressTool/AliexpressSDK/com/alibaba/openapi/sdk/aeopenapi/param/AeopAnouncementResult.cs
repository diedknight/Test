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
public class AeopAnouncementResult {

       [DataMember(Order = 1)]
    private int? totalItem;
    
        /**
       * @return 总数量
    */
        public int? getTotalItem() {
               	return totalItem;
            }
    
    /**
     * 设置总数量     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setTotalItem(int totalItem) {
     	         	    this.totalItem = totalItem;
     	        }
    
        [DataMember(Order = 2)]
    private bool? success;
    
        /**
       * @return 是否成功
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置是否成功     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
        [DataMember(Order = 3)]
    private string errorCode;
    
        /**
       * @return 错误编码
    */
        public string getErrorCode() {
               	return errorCode;
            }
    
    /**
     * 设置错误编码     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setErrorCode(string errorCode) {
     	         	    this.errorCode = errorCode;
     	        }
    
        [DataMember(Order = 4)]
    private AeopAnouncementDTO anouncementList;
    
        /**
       * @return 列表
    */
        public AeopAnouncementDTO getAnouncementList() {
               	return anouncementList;
            }
    
    /**
     * 设置列表     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAnouncementList(AeopAnouncementDTO anouncementList) {
     	         	    this.anouncementList = anouncementList;
     	        }
    
    
  }
}