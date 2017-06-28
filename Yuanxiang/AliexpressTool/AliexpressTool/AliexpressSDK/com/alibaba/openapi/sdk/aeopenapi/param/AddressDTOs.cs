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
public class AddressDTOs {

       [DataMember(Order = 1)]
    private AddressDTO receiver;
    
        /**
       * @return 收货人信息
    */
        public AddressDTO getReceiver() {
               	return receiver;
            }
    
    /**
     * 设置收货人信息     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setReceiver(AddressDTO receiver) {
     	         	    this.receiver = receiver;
     	        }
    
        [DataMember(Order = 2)]
    private AddressDTO sender;
    
        /**
       * @return 发货人信息
    */
        public AddressDTO getSender() {
               	return sender;
            }
    
    /**
     * 设置发货人信息     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSender(AddressDTO sender) {
     	         	    this.sender = sender;
     	        }
    
        [DataMember(Order = 3)]
    private AddressDTO pickup;
    
        /**
       * @return 揽收人信息
    */
        public AddressDTO getPickup() {
               	return pickup;
            }
    
    /**
     * 设置揽收人信息     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPickup(AddressDTO pickup) {
     	         	    this.pickup = pickup;
     	        }
    
    
  }
}