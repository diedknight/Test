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
public class ApiListImagePaginationParam {

       [DataMember(Order = 1)]
    private string locationType;
    
        /**
       * @return 图片存放位置.可选值:ALL_GROUP(所有分组), TEMP(被禁用的图片), SUB_GROUP(某一分组), UNGROUP(非分组). 
如果locationType参数值为ALL_GROUP,TEMP,UNGROUP时，将忽略groupId参数。
如果locationType的参数值为SUB_GROUP,须指定groupId参数。
    */
        public string getLocationType() {
               	return locationType;
            }
    
    /**
     * 设置图片存放位置.可选值:ALL_GROUP(所有分组), TEMP(被禁用的图片), SUB_GROUP(某一分组), UNGROUP(非分组). 
如果locationType参数值为ALL_GROUP,TEMP,UNGROUP时，将忽略groupId参数。
如果locationType的参数值为SUB_GROUP,须指定groupId参数。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLocationType(string locationType) {
     	         	    this.locationType = locationType;
     	        }
    
        [DataMember(Order = 2)]
    private string groupId;
    
        /**
       * @return 图片组id
    */
        public string getGroupId() {
               	return groupId;
            }
    
    /**
     * 设置图片组id     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGroupId(string groupId) {
     	         	    this.groupId = groupId;
     	        }
    
        [DataMember(Order = 3)]
    private int? currentPage;
    
        /**
       * @return 当前页码
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页码     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 4)]
    private int? pageSize;
    
        /**
       * @return 默认18个，最大值 50
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置默认18个，最大值 50     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
    
  }
}