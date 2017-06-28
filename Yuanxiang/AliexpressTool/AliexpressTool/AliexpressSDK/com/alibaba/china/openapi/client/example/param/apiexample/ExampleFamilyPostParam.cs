using com.alibaba.openapi.client.primitive;
using com.alibaba.openapi.client.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace com.alibaba.china.openapi.client.example.param.apiexample
{
[DataContract(Namespace = "com.alibaba.openapi.client")]
class ExampleFamilyPostParam {

       [DataMember(Order = 1)]
    private ExampleFamily family;
    
        /**
       * @return 上传Family对象信息
    */
        public ExampleFamily getFamily() {
               	return family;
            }
    
    /**
     * 设置上传Family对象信息     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFamily(ExampleFamily family) {
     	         	    this.family = family;
     	        }
    
        [DataMember(Order = 2)]
    private string comments;
    
        /**
       * @return 备注信息
    */
        public string getComments() {
               	return comments;
            }
    
    /**
     * 设置备注信息     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setComments(string comments) {
     	         	    this.comments = comments;
     	        }
    
        [DataMember(Order = 3)]
    private byte[] houseImg;
    
        /**
       * @return 房屋信息
    */
        public byte[] getHouseImg() {
               	return houseImg;
            }
    
    /**
     * 设置房屋信息     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setHouseImg(byte[] houseImg) {
     	         	    this.houseImg = houseImg;
     	        }
    
    
  }
}