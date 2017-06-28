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
public class AeopTaoDaiXiaoProductResultDTO {

       [DataMember(Order = 1)]
    private string detailUrl;
    
        /**
       * @return 淘代销商品URL
    */
        public string getDetailUrl() {
               	return detailUrl;
            }
    
    /**
     * 设置淘代销商品URL     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setDetailUrl(string detailUrl) {
     	         	    this.detailUrl = detailUrl;
     	        }
    
        [DataMember(Order = 2)]
    private string title;
    
        /**
       * @return 淘代销商品的标题
    */
        public string getTitle() {
               	return title;
            }
    
    /**
     * 设置淘代销商品的标题     *
     * 参数示例：<pre>beautiful clothes in 2015</pre>     
             * 此参数必填
          */
    public void setTitle(string title) {
     	         	    this.title = title;
     	        }
    
        [DataMember(Order = 3)]
    private long? tbNumIid;
    
        /**
       * @return 淘宝商品ID
    */
        public long? getTbNumIid() {
               	return tbNumIid;
            }
    
    /**
     * 设置淘宝商品ID     *
     * 参数示例：<pre>1234</pre>     
             * 此参数必填
          */
    public void setTbNumIid(long tbNumIid) {
     	         	    this.tbNumIid = tbNumIid;
     	        }
    
        [DataMember(Order = 4)]
    private string nick;
    
        /**
       * @return 淘宝卖家的昵称
    */
        public string getNick() {
               	return nick;
            }
    
    /**
     * 设置淘宝卖家的昵称     *
     * 参数示例：<pre>hello</pre>     
             * 此参数必填
          */
    public void setNick(string nick) {
     	         	    this.nick = nick;
     	        }
    
        [DataMember(Order = 5)]
    private string picUrl;
    
        /**
       * @return 产品主图的URL
    */
        public string getPicUrl() {
               	return picUrl;
            }
    
    /**
     * 设置产品主图的URL     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPicUrl(string picUrl) {
     	         	    this.picUrl = picUrl;
     	        }
    
        [DataMember(Order = 6)]
    private string shopTitle;
    
        /**
       * @return 淘宝店铺名称
    */
        public string getShopTitle() {
               	return shopTitle;
            }
    
    /**
     * 设置淘宝店铺名称     *
     * 参数示例：<pre>qi gege</pre>     
             * 此参数必填
          */
    public void setShopTitle(string shopTitle) {
     	         	    this.shopTitle = shopTitle;
     	        }
    
        [DataMember(Order = 7)]
    private string userType;
    
        /**
       * @return 商品所属卖家用户类型，b：商城， c：淘宝集市
    */
        public string getUserType() {
               	return userType;
            }
    
    /**
     * 设置商品所属卖家用户类型，b：商城， c：淘宝集市     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setUserType(string userType) {
     	         	    this.userType = userType;
     	        }
    
    
  }
}