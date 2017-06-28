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
public class ApiFindAeProductProhibitedWordsResult {

       [DataMember(Order = 1)]
    private AeProhibitedWord[] titleProhibitedWords;
    
        /**
       * @return 标题中的违禁词列表, 如果标题字中没有违禁词, 则返回一个"'[]"。否则将以示例值中的格式返回。其中每个违禁词都包含了2个属性: primaryWord和types。其中primaryWord表示违禁词，types表示违禁词的类型，总共有四种类型: FORBIDEN_TYPE(禁用), RESTRICT_TYPE(限定), BRAND_TYPE(品牌), TORT_TYPE(侵权)。
    */
        public AeProhibitedWord[] getTitleProhibitedWords() {
               	return titleProhibitedWords;
            }
    
    /**
     * 设置标题中的违禁词列表, 如果标题字中没有违禁词, 则返回一个"'[]"。否则将以示例值中的格式返回。其中每个违禁词都包含了2个属性: primaryWord和types。其中primaryWord表示违禁词，types表示违禁词的类型，总共有四种类型: FORBIDEN_TYPE(禁用), RESTRICT_TYPE(限定), BRAND_TYPE(品牌), TORT_TYPE(侵权)。     *
          
             * 此参数必填
          */
    public void setTitleProhibitedWords(AeProhibitedWord[] titleProhibitedWords) {
     	         	    this.titleProhibitedWords = titleProhibitedWords;
     	        }
    
        [DataMember(Order = 2)]
    private AeProhibitedWord[] keywordsProhibitedWords;
    
        /**
       * @return 关键字的违禁词列表, 如果关键字中没有违禁词, 则返回一个"'[]"。否则将以示例值中的格式返回。其中每个违禁词都包含了2个属性: primaryWord和types。其中primaryWord表示违禁词，types表示违禁词的类型，总共有四种类型: FORBIDEN_TYPE(禁用), RESTRICT_TYPE(限定), BRAND_TYPE(品牌), TORT_TYPE(侵权)。
    */
        public AeProhibitedWord[] getKeywordsProhibitedWords() {
               	return keywordsProhibitedWords;
            }
    
    /**
     * 设置关键字的违禁词列表, 如果关键字中没有违禁词, 则返回一个"'[]"。否则将以示例值中的格式返回。其中每个违禁词都包含了2个属性: primaryWord和types。其中primaryWord表示违禁词，types表示违禁词的类型，总共有四种类型: FORBIDEN_TYPE(禁用), RESTRICT_TYPE(限定), BRAND_TYPE(品牌), TORT_TYPE(侵权)。     *
          
             * 此参数必填
          */
    public void setKeywordsProhibitedWords(AeProhibitedWord[] keywordsProhibitedWords) {
     	         	    this.keywordsProhibitedWords = keywordsProhibitedWords;
     	        }
    
        [DataMember(Order = 3)]
    private AeProhibitedWord[] productPropertiesProhibitedWords;
    
        /**
       * @return 类目属性的违禁词列表, 如果类目属性中没有违禁词, 则返回一个"'[]"。否则将以示例值中的格式返回。其中每个违禁词都包含了2个属性: primaryWord和types。其中primaryWord表示违禁词，types表示违禁词的类型，总共有四种类型: FORBIDEN_TYPE(禁用), RESTRICT_TYPE(限定), BRAND_TYPE(品牌), TORT_TYPE(侵权)。
    */
        public AeProhibitedWord[] getProductPropertiesProhibitedWords() {
               	return productPropertiesProhibitedWords;
            }
    
    /**
     * 设置类目属性的违禁词列表, 如果类目属性中没有违禁词, 则返回一个"'[]"。否则将以示例值中的格式返回。其中每个违禁词都包含了2个属性: primaryWord和types。其中primaryWord表示违禁词，types表示违禁词的类型，总共有四种类型: FORBIDEN_TYPE(禁用), RESTRICT_TYPE(限定), BRAND_TYPE(品牌), TORT_TYPE(侵权)。     *
          
             * 此参数必填
          */
    public void setProductPropertiesProhibitedWords(AeProhibitedWord[] productPropertiesProhibitedWords) {
     	         	    this.productPropertiesProhibitedWords = productPropertiesProhibitedWords;
     	        }
    
        [DataMember(Order = 4)]
    private AeProhibitedWord[] detailProhibitedWords;
    
        /**
       * @return 商品详描中的违禁词列表, 如果商品详描中没有违禁词, 则返回一个"'[]"。否则将以示例值中的格式返回。其中每个违禁词都包含了2个属性: primaryWord和types。其中primaryWord表示违禁词，types表示违禁词的类型，总共有四种类型: FORBIDEN_TYPE(禁用), RESTRICT_TYPE(限定), BRAND_TYPE(品牌), TORT_TYPE(侵权)。
    */
        public AeProhibitedWord[] getDetailProhibitedWords() {
               	return detailProhibitedWords;
            }
    
    /**
     * 设置商品详描中的违禁词列表, 如果商品详描中没有违禁词, 则返回一个"'[]"。否则将以示例值中的格式返回。其中每个违禁词都包含了2个属性: primaryWord和types。其中primaryWord表示违禁词，types表示违禁词的类型，总共有四种类型: FORBIDEN_TYPE(禁用), RESTRICT_TYPE(限定), BRAND_TYPE(品牌), TORT_TYPE(侵权)。     *
          
             * 此参数必填
          */
    public void setDetailProhibitedWords(AeProhibitedWord[] detailProhibitedWords) {
     	         	    this.detailProhibitedWords = detailProhibitedWords;
     	        }
    
    
  }
}