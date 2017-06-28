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
public class ApiUpdateDeliveriedConfirmationFileParam {

       [DataMember(Order = 1)]
    private byte[] input;
    
        /**
       * @return 
    */
        public byte[] getInput() {
               	return input;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setInput(byte[] input) {
     	         	    this.input = input;
     	        }
    
        [DataMember(Order = 2)]
    private string filename;
    
        /**
       * @return 图片原名，上传证明文件，支持jpg和png格式，大小不超过2MB。
    */
        public string getFilename() {
               	return filename;
            }
    
    /**
     * 设置图片原名，上传证明文件，支持jpg和png格式，大小不超过2MB。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFilename(string filename) {
     	         	    this.filename = filename;
     	        }
    
        [DataMember(Order = 3)]
    private long? orderId;
    
        /**
       * @return 订单ID
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置订单ID     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
    
  }
}