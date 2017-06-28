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
class ExamplePerson {

       [DataMember(Order = 1)]
    private string name;
    
        /**
       * @return 
    */
        public string getName() {
               	return name;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setName(string name) {
     	         	    this.name = name;
     	        }
    
        [DataMember(Order = 2)]
    private int? age;
    
        /**
       * @return 
    */
        public int? getAge() {
               	return age;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAge(int age) {
     	         	    this.age = age;
     	        }
    
        [DataMember(Order = 3)]
    private string birthday;
    
        /**
       * @return 
    */
        public DateTime? getBirthday() {
                 if (birthday != null)
          {
              DateTime datetime = DateUtil.formatFromStr(birthday);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBirthday(DateTime birthday) {
     	         	    this.birthday = DateUtil.format(birthday);
     	        }
    
        [DataMember(Order = 4)]
    private string mobileNumber;
    
        /**
       * @return 
    */
        public string getMobileNumber() {
               	return mobileNumber;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMobileNumber(string mobileNumber) {
     	         	    this.mobileNumber = mobileNumber;
     	        }
    
    
  }
}