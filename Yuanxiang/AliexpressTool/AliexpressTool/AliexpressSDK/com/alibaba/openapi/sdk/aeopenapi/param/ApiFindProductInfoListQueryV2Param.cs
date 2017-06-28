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
public class ApiFindProductInfoListQueryV2Param {

       [DataMember(Order = 1)]
    private string productStatusType;
    
        /**
       * @return 商品业务状态，目前提供4种，输入参数分别是：上架:onSelling ；下架:offline ；审核中:auditing ；审核不通过:editingRequired。
    */
        public string getProductStatusType() {
               	return productStatusType;
            }
    
    /**
     * 设置商品业务状态，目前提供4种，输入参数分别是：上架:onSelling ；下架:offline ；审核中:auditing ；审核不通过:editingRequired。     *
     * 参数示例：<pre>onSelling</pre>     
             * 此参数必填
          */
    public void setProductStatusType(string productStatusType) {
     	         	    this.productStatusType = productStatusType;
     	        }
    
        [DataMember(Order = 2)]
    private string subject;
    
        /**
       * @return 商品标题模糊搜索字段。只支持半角英数字，长度不超过128。
    */
        public string getSubject() {
               	return subject;
            }
    
    /**
     * 设置商品标题模糊搜索字段。只支持半角英数字，长度不超过128。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSubject(string subject) {
     	         	    this.subject = subject;
     	        }
    
        [DataMember(Order = 3)]
    private int? groupId;
    
        /**
       * @return 商品分组搜索字段。输入商品分组id(groupId).
    */
        public int? getGroupId() {
               	return groupId;
            }
    
    /**
     * 设置商品分组搜索字段。输入商品分组id(groupId).     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGroupId(int groupId) {
     	         	    this.groupId = groupId;
     	        }
    
        [DataMember(Order = 4)]
    private string wsDisplay;
    
        /**
       * @return 商品下架原因搜索字段。expire_offline：过期下架；user_offline：用户下架；violate_offline：违规下架。
    */
        public string getWsDisplay() {
               	return wsDisplay;
            }
    
    /**
     * 设置商品下架原因搜索字段。expire_offline：过期下架；user_offline：用户下架；violate_offline：违规下架。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setWsDisplay(string wsDisplay) {
     	         	    this.wsDisplay = wsDisplay;
     	        }
    
        [DataMember(Order = 5)]
    private int? offLineTime;
    
        /**
       * @return 到期时间搜索字段。商品到期时间，输入值小于等于30，单位天。相当查询与现在时+offLineTime天数之内的商品。
    */
        public int? getOffLineTime() {
               	return offLineTime;
            }
    
    /**
     * 设置到期时间搜索字段。商品到期时间，输入值小于等于30，单位天。相当查询与现在时+offLineTime天数之内的商品。     *
     * 参数示例：<pre>7</pre>     
             * 此参数必填
          */
    public void setOffLineTime(int offLineTime) {
     	         	    this.offLineTime = offLineTime;
     	        }
    
        [DataMember(Order = 6)]
    private long? productId;
    
        /**
       * @return 商品id搜索字段。输入所需查询的商品id。
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置商品id搜索字段。输入所需查询的商品id。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 7)]
    private int? pageSize;
    
        /**
       * @return 每页查询商品数量。输入值小于100，默认20。
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页查询商品数量。输入值小于100，默认20。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
        [DataMember(Order = 8)]
    private int? currentPage;
    
        /**
       * @return 需要商品的当前页数。默认第一页。
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置需要商品的当前页数。默认第一页。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 9)]
    private string ownerMemberId;
    
        /**
       * @return 商品所有者登陆id。缺省情况下，主账号查询所有商品，子账号只查询自身所属商品。
    */
        public string getOwnerMemberId() {
               	return ownerMemberId;
            }
    
    /**
     * 设置商品所有者登陆id。缺省情况下，主账号查询所有商品，子账号只查询自身所属商品。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOwnerMemberId(string ownerMemberId) {
     	         	    this.ownerMemberId = ownerMemberId;
     	        }
    
    
  }
}