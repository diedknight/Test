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
public class ApiListImagePaginationResult {

       [DataMember(Order = 1)]
    private PhtobankPaginationQuery query;
    
        /**
       * @return 当前参数组成的查询对象。
    */
        public PhtobankPaginationQuery getQuery() {
               	return query;
            }
    
    /**
     * 设置当前参数组成的查询对象。     *
          
             * 此参数必填
          */
    public void setQuery(PhtobankPaginationQuery query) {
     	         	    this.query = query;
     	        }
    
        [DataMember(Order = 2)]
    private PhotobankImageRecord[] images;
    
        /**
       * @return 本次查询结果返回的图片列表。
    */
        public PhotobankImageRecord[] getImages() {
               	return images;
            }
    
    /**
     * 设置本次查询结果返回的图片列表。     *
          
             * 此参数必填
          */
    public void setImages(PhotobankImageRecord[] images) {
     	         	    this.images = images;
     	        }
    
        [DataMember(Order = 3)]
    private int? total;
    
        /**
       * @return 当前分组下的图片总数。如果locationType取值为"allGroup", 则为这个用户的图片总数。
    */
        public int? getTotal() {
               	return total;
            }
    
    /**
     * 设置当前分组下的图片总数。如果locationType取值为"allGroup", 则为这个用户的图片总数。     *
          
             * 此参数必填
          */
    public void setTotal(int total) {
     	         	    this.total = total;
     	        }
    
        [DataMember(Order = 4)]
    private int? totalPage;
    
        /**
       * @return 本次查询结果分页的页数。
    */
        public int? getTotalPage() {
               	return totalPage;
            }
    
    /**
     * 设置本次查询结果分页的页数。     *
          
             * 此参数必填
          */
    public void setTotalPage(int totalPage) {
     	         	    this.totalPage = totalPage;
     	        }
    
        [DataMember(Order = 5)]
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