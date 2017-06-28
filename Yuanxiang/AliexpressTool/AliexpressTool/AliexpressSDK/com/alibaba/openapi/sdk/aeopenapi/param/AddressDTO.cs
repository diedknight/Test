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
public class AddressDTO {

       [DataMember(Order = 1)]
    private string city;
    
        /**
       * @return 城市
    */
        public string getCity() {
               	return city;
            }
    
    /**
     * 设置城市     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCity(string city) {
     	         	    this.city = city;
     	        }
    
        [DataMember(Order = 2)]
    private string country;
    
        /**
       * @return 国家
    */
        public string getCountry() {
               	return country;
            }
    
    /**
     * 设置国家     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCountry(string country) {
     	         	    this.country = country;
     	        }
    
        [DataMember(Order = 3)]
    private string email;
    
        /**
       * @return 邮箱
    */
        public string getEmail() {
               	return email;
            }
    
    /**
     * 设置邮箱     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setEmail(string email) {
     	         	    this.email = email;
     	        }
    
        [DataMember(Order = 4)]
    private string fax;
    
        /**
       * @return 传真
    */
        public string getFax() {
               	return fax;
            }
    
    /**
     * 设置传真     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFax(string fax) {
     	         	    this.fax = fax;
     	        }
    
        [DataMember(Order = 5)]
    private string memberType;
    
        /**
       * @return 类型
    */
        public string getMemberType() {
               	return memberType;
            }
    
    /**
     * 设置类型     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMemberType(string memberType) {
     	         	    this.memberType = memberType;
     	        }
    
        [DataMember(Order = 6)]
    private string mobile;
    
        /**
       * @return 电话
    */
        public string getMobile() {
               	return mobile;
            }
    
    /**
     * 设置电话     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMobile(string mobile) {
     	         	    this.mobile = mobile;
     	        }
    
        [DataMember(Order = 7)]
    private string name;
    
        /**
       * @return 姓名
    */
        public string getName() {
               	return name;
            }
    
    /**
     * 设置姓名     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setName(string name) {
     	         	    this.name = name;
     	        }
    
        [DataMember(Order = 8)]
    private string phone;
    
        /**
       * @return 电话
    */
        public string getPhone() {
               	return phone;
            }
    
    /**
     * 设置电话     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPhone(string phone) {
     	         	    this.phone = phone;
     	        }
    
        [DataMember(Order = 9)]
    private string postcode;
    
        /**
       * @return 邮编
    */
        public string getPostcode() {
               	return postcode;
            }
    
    /**
     * 设置邮编     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPostcode(string postcode) {
     	         	    this.postcode = postcode;
     	        }
    
        [DataMember(Order = 10)]
    private string province;
    
        /**
       * @return 省份
    */
        public string getProvince() {
               	return province;
            }
    
    /**
     * 设置省份     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setProvince(string province) {
     	         	    this.province = province;
     	        }
    
        [DataMember(Order = 11)]
    private string streetAddress;
    
        /**
       * @return 详细地址
    */
        public string getStreetAddress() {
               	return streetAddress;
            }
    
    /**
     * 设置详细地址     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setStreetAddress(string streetAddress) {
     	         	    this.streetAddress = streetAddress;
     	        }
    
        [DataMember(Order = 12)]
    private string trademanageId;
    
        /**
       * @return 旺旺
    */
        public string getTrademanageId() {
               	return trademanageId;
            }
    
    /**
     * 设置旺旺     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setTrademanageId(string trademanageId) {
     	         	    this.trademanageId = trademanageId;
     	        }
    
        [DataMember(Order = 13)]
    private string county;
    
        /**
       * @return 区
    */
        public string getCounty() {
               	return county;
            }
    
    /**
     * 设置区     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCounty(string county) {
     	         	    this.county = county;
     	        }
    
        [DataMember(Order = 14)]
    private string street;
    
        /**
       * @return 街道
    */
        public string getStreet() {
               	return street;
            }
    
    /**
     * 设置街道     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setStreet(string street) {
     	         	    this.street = street;
     	        }
    
    
  }
}