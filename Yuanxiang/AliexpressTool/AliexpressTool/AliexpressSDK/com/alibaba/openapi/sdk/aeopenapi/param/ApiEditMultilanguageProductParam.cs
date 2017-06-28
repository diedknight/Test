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
public class ApiEditMultilanguageProductParam {

       [DataMember(Order = 1)]
    private long? productId;
    
        /**
       * @return 待编辑的商品ID。
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置待编辑的商品ID。     *
     * 参数示例：<pre>32234411234</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
        [DataMember(Order = 2)]
    private string locale;
    
        /**
       * @return 语种，合法的参数有: ru_RU(俄语);pt_BR(葡语);fr_FR(法语);es_ES(西班牙语);in_ID(印尼语);it_IT(意大利语);de_DE(德语);nl_NL(荷兰语);tr_TR(土耳其语);iw_IL(以色列语);ja_JP(日语);ar_MA(阿拉伯语);th_TH(泰语);vi_VN(越南语);ko_KR(韩语);
    */
        public string getLocale() {
               	return locale;
            }
    
    /**
     * 设置语种，合法的参数有: ru_RU(俄语);pt_BR(葡语);fr_FR(法语);es_ES(西班牙语);in_ID(印尼语);it_IT(意大利语);de_DE(德语);nl_NL(荷兰语);tr_TR(土耳其语);iw_IL(以色列语);ja_JP(日语);ar_MA(阿拉伯语);th_TH(泰语);vi_VN(越南语);ko_KR(韩语);     *
     * 参数示例：<pre>ru_RU</pre>     
             * 此参数必填
          */
    public void setLocale(string locale) {
     	         	    this.locale = locale;
     	        }
    
        [DataMember(Order = 3)]
    private string subject;
    
        /**
       * @return 商品对应语种的标题, 长度控制在1～218个字符之间。
    */
        public string getSubject() {
               	return subject;
            }
    
    /**
     * 设置商品对应语种的标题, 长度控制在1～218个字符之间。     *
     * 参数示例：<pre>foo</pre>     
             * 此参数必填
          */
    public void setSubject(string subject) {
     	         	    this.subject = subject;
     	        }
    
        [DataMember(Order = 4)]
    private string detail;
    
        /**
       * @return 商品对应语种的详描
    */
        public string getDetail() {
               	return detail;
            }
    
    /**
     * 设置商品对应语种的详描     *
     * 参数示例：<pre>bar</pre>     
             * 此参数必填
          */
    public void setDetail(string detail) {
     	         	    this.detail = detail;
     	        }
    
    
  }
}