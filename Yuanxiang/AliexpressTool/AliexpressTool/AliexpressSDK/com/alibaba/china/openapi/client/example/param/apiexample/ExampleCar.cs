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
class ExampleCar {

       [DataMember(Order = 1)]
    private string builtDate;
    
        /**
       * @return 
    */
        public DateTime? getBuiltDate() {
                 if (builtDate != null)
          {
              DateTime datetime = DateUtil.formatFromStr(builtDate);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBuiltDate(DateTime builtDate) {
     	         	    this.builtDate = DateUtil.format(builtDate);
     	        }
    
        [DataMember(Order = 2)]
    private string boughtDate;
    
        /**
       * @return 
    */
        public DateTime? getBoughtDate() {
                 if (boughtDate != null)
          {
              DateTime datetime = DateUtil.formatFromStr(boughtDate);
              return datetime;
          }
    	  return null;
    	    }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBoughtDate(DateTime boughtDate) {
     	         	    this.boughtDate = DateUtil.format(boughtDate);
     	        }
    
        [DataMember(Order = 3)]
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
    
        [DataMember(Order = 4)]
    private string builtArea;
    
        /**
       * @return 
    */
        public string getBuiltArea() {
               	return builtArea;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setBuiltArea(string builtArea) {
     	         	    this.builtArea = builtArea;
     	        }
    
        [DataMember(Order = 5)]
    private string carNumber;
    
        /**
       * @return 
    */
        public string getCarNumber() {
               	return carNumber;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setCarNumber(string carNumber) {
     	         	    this.carNumber = carNumber;
     	        }
    
        [DataMember(Order = 6)]
    private double? price;
    
        /**
       * @return 
    */
        public double? getPrice() {
               	return price;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPrice(double price) {
     	         	    this.price = price;
     	        }
    
        [DataMember(Order = 7)]
    private int? seats;
    
        /**
       * @return 
    */
        public int? getSeats() {
               	return seats;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSeats(int seats) {
     	         	    this.seats = seats;
     	        }
    
    
  }
}