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
public class TpOpenAddressDTO {

       [DataMember(Order = 1)]
    private string address;
    
        /**
       * @return 地址1
    */
        public string getAddress() {
               	return address;
            }
    
    /**
     * 设置地址1     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAddress(string address) {
     	         	    this.address = address;
     	        }
    
        [DataMember(Order = 2)]
    private string address2;
    
        /**
       * @return 地址2
    */
        public string getAddress2() {
               	return address2;
            }
    
    /**
     * 设置地址2     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAddress2(string address2) {
     	         	    this.address2 = address2;
     	        }
    
        [DataMember(Order = 3)]
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
    
        [DataMember(Order = 4)]
    private string contactPerson;
    
        /**
       * @return 收件人
    */
        public string getContactPerson() {
               	return contactPerson;
            }
    
    /**
     * 设置收件人     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setContactPerson(string contactPerson) {
     	         	    this.contactPerson = contactPerson;
     	        }
    
        [DataMember(Order = 5)]
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
    
        [DataMember(Order = 6)]
    private string detailAddress;
    
        /**
       * @return 街道详细地址
    */
        public string getDetailAddress() {
               	return detailAddress;
            }
    
    /**
     * 设置街道详细地址     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setDetailAddress(string detailAddress) {
     	         	    this.detailAddress = detailAddress;
     	        }
    
        [DataMember(Order = 7)]
    private string faxArea;
    
        /**
       * @return 传真区号
    */
        public string getFaxArea() {
               	return faxArea;
            }
    
    /**
     * 设置传真区号     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFaxArea(string faxArea) {
     	         	    this.faxArea = faxArea;
     	        }
    
        [DataMember(Order = 8)]
    private string faxCountry;
    
        /**
       * @return 传真国家码
    */
        public string getFaxCountry() {
               	return faxCountry;
            }
    
    /**
     * 设置传真国家码     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFaxCountry(string faxCountry) {
     	         	    this.faxCountry = faxCountry;
     	        }
    
        [DataMember(Order = 9)]
    private string faxNumber;
    
        /**
       * @return 传真号
    */
        public string getFaxNumber() {
               	return faxNumber;
            }
    
    /**
     * 设置传真号     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFaxNumber(string faxNumber) {
     	         	    this.faxNumber = faxNumber;
     	        }
    
        [DataMember(Order = 10)]
    private string mobileNo;
    
        /**
       * @return 手机
    */
        public string getMobileNo() {
               	return mobileNo;
            }
    
    /**
     * 设置手机     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMobileNo(string mobileNo) {
     	         	    this.mobileNo = mobileNo;
     	        }
    
        [DataMember(Order = 11)]
    private string phoneArea;
    
        /**
       * @return 区号
    */
        public string getPhoneArea() {
               	return phoneArea;
            }
    
    /**
     * 设置区号     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPhoneArea(string phoneArea) {
     	         	    this.phoneArea = phoneArea;
     	        }
    
        [DataMember(Order = 12)]
    private string phoneCountry;
    
        /**
       * @return 国家码
    */
        public string getPhoneCountry() {
               	return phoneCountry;
            }
    
    /**
     * 设置国家码     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPhoneCountry(string phoneCountry) {
     	         	    this.phoneCountry = phoneCountry;
     	        }
    
        [DataMember(Order = 13)]
    private string phoneNumber;
    
        /**
       * @return 电话
    */
        public string getPhoneNumber() {
               	return phoneNumber;
            }
    
    /**
     * 设置电话     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPhoneNumber(string phoneNumber) {
     	         	    this.phoneNumber = phoneNumber;
     	        }
    
        [DataMember(Order = 14)]
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
    
        [DataMember(Order = 15)]
    private string zip;
    
        /**
       * @return 邮编
    */
        public string getZip() {
               	return zip;
            }
    
    /**
     * 设置邮编     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setZip(string zip) {
     	         	    this.zip = zip;
     	        }
    
    
  }
}