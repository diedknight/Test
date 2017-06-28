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
public class AeopAEProductDisplaySampleDTO {

       [DataMember(Order = 1)]
    private string subject;
    
        /**
       * @return 商品标题
    */
        public string getSubject() {
               	return subject;
            }
    
    /**
     * 设置商品标题     *
     * 参数示例：<pre>knew odd</pre>     
             * 此参数必填
          */
    public void setSubject(string subject) {
     	         	    this.subject = subject;
     	        }
    
        [DataMember(Order = 2)]
    private int? groupId;
    
        /**
       * @return 商品分组id
    */
        public int? getGroupId() {
               	return groupId;
            }
    
    /**
     * 设置商品分组id     *
     * 参数示例：<pre>123</pre>     
             * 此参数必填
          */
    public void setGroupId(int groupId) {
     	         	    this.groupId = groupId;
     	        }
    
        [DataMember(Order = 3)]
    private string wsOfflineDate;
    
        /**
       * @return 下架时间
    */
        public DateTime? getWsOfflineDate() {
                 if (wsOfflineDate != null)
          {
              DateTime datetime = DateUtil.formatFromStr(wsOfflineDate);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置下架时间     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setWsOfflineDate(DateTime wsOfflineDate) {
     	         	    this.wsOfflineDate = DateUtil.format(wsOfflineDate);
     	        }
    
        [DataMember(Order = 4)]
    private long? productId;
    
        /**
       * @return 商品ID
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置商品ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 5)]
    private string imageURLs;
    
        /**
       * @return 图片URL.静态单图主图个数为1,动态多图主图个数为2-6. 多个图片url用&lsquo;;&rsquo;分隔符连接。
    */
        public string getImageURLs() {
               	return imageURLs;
            }
    
    /**
     * 设置图片URL.静态单图主图个数为1,动态多图主图个数为2-6. 多个图片url用&lsquo;;&rsquo;分隔符连接。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setImageURLs(string imageURLs) {
     	         	    this.imageURLs = imageURLs;
     	        }
    
        [DataMember(Order = 6)]
    private string src;
    
        /**
       * @return 产品来源。'tdx'为淘宝代销产品，isv为通过API发布的商品。其他字符或空为普通产品。
    */
        public string getSrc() {
               	return src;
            }
    
    /**
     * 设置产品来源。'tdx'为淘宝代销产品，isv为通过API发布的商品。其他字符或空为普通产品。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSrc(string src) {
     	         	    this.src = src;
     	        }
    
        [DataMember(Order = 7)]
    private string gmtCreate;
    
        /**
       * @return 产品发布时间。
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
     * 设置产品发布时间。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGmtCreate(DateTime gmtCreate) {
     	         	    this.gmtCreate = DateUtil.format(gmtCreate);
     	        }
    
        [DataMember(Order = 8)]
    private string gmtModified;
    
        /**
       * @return 商品最后更新时间（系统更新时间也会记录）。
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
     * 设置商品最后更新时间（系统更新时间也会记录）。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setGmtModified(DateTime gmtModified) {
     	         	    this.gmtModified = DateUtil.format(gmtModified);
     	        }
    
        [DataMember(Order = 9)]
    private string productMinPrice;
    
        /**
       * @return 最小价格。
    */
        public string getProductMinPrice() {
               	return productMinPrice;
            }
    
    /**
     * 设置最小价格。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductMinPrice(string productMinPrice) {
     	         	    this.productMinPrice = productMinPrice;
     	        }
    
        [DataMember(Order = 10)]
    private string productMaxPrice;
    
        /**
       * @return 最大价格。
    */
        public string getProductMaxPrice() {
               	return productMaxPrice;
            }
    
    /**
     * 设置最大价格。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProductMaxPrice(string productMaxPrice) {
     	         	    this.productMaxPrice = productMaxPrice;
     	        }
    
        [DataMember(Order = 11)]
    private string ownerMemberId;
    
        /**
       * @return 商品所属人loginId
    */
        public string getOwnerMemberId() {
               	return ownerMemberId;
            }
    
    /**
     * 设置商品所属人loginId     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOwnerMemberId(string ownerMemberId) {
     	         	    this.ownerMemberId = ownerMemberId;
     	        }
    
        [DataMember(Order = 12)]
    private int? ownerMemberSeq;
    
        /**
       * @return 商品所属人Seq
    */
        public int? getOwnerMemberSeq() {
               	return ownerMemberSeq;
            }
    
    /**
     * 设置商品所属人Seq     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOwnerMemberSeq(int ownerMemberSeq) {
     	         	    this.ownerMemberSeq = ownerMemberSeq;
     	        }
    
        [DataMember(Order = 13)]
    private int? wsDisplay;
    
        /**
       * @return 商品下架原因：expire_offline：过期下架，user_offline：用户下架,violate_offline：违规下架,punish_offline：交易违规下架，degrade_offline：降级下架
    */
        public int? getWsDisplay() {
               	return wsDisplay;
            }
    
    /**
     * 设置商品下架原因：expire_offline：过期下架，user_offline：用户下架,violate_offline：违规下架,punish_offline：交易违规下架，degrade_offline：降级下架     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setWsDisplay(int wsDisplay) {
     	         	    this.wsDisplay = wsDisplay;
     	        }
    
        [DataMember(Order = 14)]
    private string currencyCode;
    
        /**
       * @return 货币单位
    */
        public string getCurrencyCode() {
               	return currencyCode;
            }
    
    /**
     * 设置货币单位     *
     * 参数示例：<pre>USD;RUB</pre>     
             * 此参数必填
          */
    public void setCurrencyCode(string currencyCode) {
     	         	    this.currencyCode = currencyCode;
     	        }
    
    
  }
}