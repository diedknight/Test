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
public class ApiFindProductInfoListQueryParam {

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
     * 参数示例：<pre>knew odd</pre>     
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
     * 参数示例：<pre>124</pre>     
             * 此参数必填
          */
    public void setGroupId(int groupId) {
     	         	    this.groupId = groupId;
     	        }
    
        [DataMember(Order = 4)]
    private string wsDisplay;
    
        /**
       * @return 商品下架原因：expire_offline(过期下架)，user_offline(用户手工下架)、violate_offline(违规下架)、punish_offline(处罚下架)、degrade_offline(降级下架)、industry_offline(行业准入未续约下架)
    */
        public string getWsDisplay() {
               	return wsDisplay;
            }
    
    /**
     * 设置商品下架原因：expire_offline(过期下架)，user_offline(用户手工下架)、violate_offline(违规下架)、punish_offline(处罚下架)、degrade_offline(降级下架)、industry_offline(行业准入未续约下架)     *
     * 参数示例：<pre>expire_offline</pre>     
             * 此参数必填
          */
    public void setWsDisplay(string wsDisplay) {
     	         	    this.wsDisplay = wsDisplay;
     	        }
    
        [DataMember(Order = 5)]
    private int? offLineTime;
    
        /**
       * @return 商品的剩余有效期。如果想查3天之内即将下架的商品，则offLineTime值为3。
    */
        public int? getOffLineTime() {
               	return offLineTime;
            }
    
    /**
     * 设置商品的剩余有效期。如果想查3天之内即将下架的商品，则offLineTime值为3。     *
     * 参数示例：<pre>7</pre>     
             * 此参数必填
          */
    public void setOffLineTime(int offLineTime) {
     	         	    this.offLineTime = offLineTime;
     	        }
    
        [DataMember(Order = 6)]
    private long? productId;
    
        /**
       * @return 商品id搜索字段。
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置商品id搜索字段。     *
     * 参数示例：<pre>123</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 7)]
    private long[] exceptedProductIds;
    
        /**
       * @return 待排除的产品ID列表。
    */
        public long[] getExceptedProductIds() {
               	return exceptedProductIds;
            }
    
    /**
     * 设置待排除的产品ID列表。     *
     * 参数示例：<pre>[123,456]</pre>     
             * 此参数必填
          */
    public void setExceptedProductIds(long[] exceptedProductIds) {
     	         	    this.exceptedProductIds = exceptedProductIds;
     	        }
    
        [DataMember(Order = 8)]
    private int? pageSize;
    
        /**
       * @return 每页查询商品数量。输入值小于100，默认20。
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页查询商品数量。输入值小于100，默认20。     *
     * 参数示例：<pre>30</pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
        [DataMember(Order = 9)]
    private int? currentPage;
    
        /**
       * @return 需要商品的当前页数。默认第一页。
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置需要商品的当前页数。默认第一页。     *
     * 参数示例：<pre>2</pre>     
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
    
  }
}