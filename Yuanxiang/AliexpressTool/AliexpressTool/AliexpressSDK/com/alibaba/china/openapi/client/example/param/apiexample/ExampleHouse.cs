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
class ExampleHouse {

       [DataMember(Order = 1)]
    private string location;
    
        /**
       * @return 
    */
        public string getLocation() {
               	return location;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLocation(string location) {
     	         	    this.location = location;
     	        }
    
        [DataMember(Order = 2)]
    private int? areaSize;
    
        /**
       * @return 
    */
        public int? getAreaSize() {
               	return areaSize;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAreaSize(int areaSize) {
     	         	    this.areaSize = areaSize;
     	        }
    
        [DataMember(Order = 3)]
    private bool? rent;
    
        /**
       * @return 
    */
        public bool? getRent() {
               	return rent;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setRent(bool rent) {
     	         	    this.rent = rent;
     	        }
    
        [DataMember(Order = 4)]
    private int? rooms;
    
        /**
       * @return 
    */
        public int? getRooms() {
               	return rooms;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setRooms(int rooms) {
     	         	    this.rooms = rooms;
     	        }
    
    
  }
}