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
public class ApiFindAeProductProhibitedWordsParam {

       [DataMember(Order = 1)]
    private int? categoryId;
    
        /**
       * @return 商品类目ID
    */
        public int? getCategoryId() {
               	return categoryId;
            }
    
    /**
     * 设置商品类目ID     *
     * 参数示例：<pre>322</pre>     
             * 此参数必填
          */
    public void setCategoryId(int categoryId) {
     	         	    this.categoryId = categoryId;
     	        }
    
        [DataMember(Order = 2)]
    private string title;
    
        /**
       * @return 商品的标题
    */
        public string getTitle() {
               	return title;
            }
    
    /**
     * 设置商品的标题     *
     * 参数示例：<pre>nike</pre>     
             * 此参数必填
          */
    public void setTitle(string title) {
     	         	    this.title = title;
     	        }
    
        [DataMember(Order = 3)]
    private string[] keywords;
    
        /**
       * @return 商品的关键字列表
    */
        public string[] getKeywords() {
               	return keywords;
            }
    
    /**
     * 设置商品的关键字列表     *
     * 参数示例：<pre>["nike", "shoes", "adidas"]</pre>     
             * 此参数必填
          */
    public void setKeywords(string[] keywords) {
     	         	    this.keywords = keywords;
     	        }
    
        [DataMember(Order = 4)]
    private string[] productProperties;
    
        /**
       * @return 商品的类目属性，只能填写字符形式的类目属性
    */
        public string[] getProductProperties() {
               	return productProperties;
            }
    
    /**
     * 设置商品的类目属性，只能填写字符形式的类目属性     *
     * 参数示例：<pre>["red", "nike", "shoes"]</pre>     
             * 此参数必填
          */
    public void setProductProperties(string[] productProperties) {
     	         	    this.productProperties = productProperties;
     	        }
    
        [DataMember(Order = 5)]
    private string detail;
    
        /**
       * @return 商品的详细描述
    */
        public string getDetail() {
               	return detail;
            }
    
    /**
     * 设置商品的详细描述     *
     * 参数示例：<pre>This is a test for the product.</pre>     
             * 此参数必填
          */
    public void setDetail(string detail) {
     	         	    this.detail = detail;
     	        }
    
    
  }
}