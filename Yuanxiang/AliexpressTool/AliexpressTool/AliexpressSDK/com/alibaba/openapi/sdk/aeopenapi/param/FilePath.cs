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
public class FilePath {

       [DataMember(Order = 1)]
    private string sPath;
    
        /**
       * @return 
    */
        public string getSPath() {
               	return sPath;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setSPath(string sPath) {
     	         	    this.sPath = sPath;
     	        }
    
        [DataMember(Order = 2)]
    private string mPath;
    
        /**
       * @return 
    */
        public string getMPath() {
               	return mPath;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMPath(string mPath) {
     	         	    this.mPath = mPath;
     	        }
    
        [DataMember(Order = 3)]
    private string lPath;
    
        /**
       * @return 
    */
        public string getLPath() {
               	return lPath;
            }
    
    /**
     * 设置     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setLPath(string lPath) {
     	         	    this.lPath = lPath;
     	        }
    
    
  }
}