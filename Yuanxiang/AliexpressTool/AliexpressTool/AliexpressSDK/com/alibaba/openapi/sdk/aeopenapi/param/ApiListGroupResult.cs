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
public class ApiListGroupResult {

       [DataMember(Order = 1)]
    private PhotobankGroup[] photoBankImageGroupList;
    
        /**
       * @return 图片银行分组列表，如果没有任何的分组信息。这个属性为:[]。
    */
        public PhotobankGroup[] getPhotoBankImageGroupList() {
               	return photoBankImageGroupList;
            }
    
    /**
     * 设置图片银行分组列表，如果没有任何的分组信息。这个属性为:[]。     *
          
             * 此参数必填
          */
    public void setPhotoBankImageGroupList(PhotobankGroup[] photoBankImageGroupList) {
     	         	    this.photoBankImageGroupList = photoBankImageGroupList;
     	        }
    
        [DataMember(Order = 2)]
    private bool? success;
    
        /**
       * @return 本次调用是否成功。
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置本次调用是否成功。     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
    
  }
}