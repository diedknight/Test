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
public class ApiQueryOpenAnouncementParam {

       [DataMember(Order = 1)]
    private string anouncement;
    
        /**
       * @return 公告id，一次只能查询一个。
    */
        public string getAnouncement() {
               	return anouncement;
            }
    
    /**
     * 设置公告id，一次只能查询一个。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setAnouncement(string anouncement) {
     	         	    this.anouncement = anouncement;
     	        }
    
        [DataMember(Order = 2)]
    private string publicDatetimeStart;
    
        /**
       * @return 公告创建时间起始值，格式: mm/dd/yyyy hh:mm:ss,如10/08/2013 00:00:00。
    */
        public string getPublicDatetimeStart() {
               	return publicDatetimeStart;
            }
    
    /**
     * 设置公告创建时间起始值，格式: mm/dd/yyyy hh:mm:ss,如10/08/2013 00:00:00。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPublicDatetimeStart(string publicDatetimeStart) {
     	         	    this.publicDatetimeStart = publicDatetimeStart;
     	        }
    
        [DataMember(Order = 3)]
    private string publicDatetimeEnd;
    
        /**
       * @return 公告创建截止值，格式: mm/dd/yyyy hh:mm:ss,如10/09/2013 00:00:00。
    */
        public string getPublicDatetimeEnd() {
               	return publicDatetimeEnd;
            }
    
    /**
     * 设置公告创建截止值，格式: mm/dd/yyyy hh:mm:ss,如10/09/2013 00:00:00。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPublicDatetimeEnd(string publicDatetimeEnd) {
     	         	    this.publicDatetimeEnd = publicDatetimeEnd;
     	        }
    
        [DataMember(Order = 4)]
    private int? page;
    
        /**
       * @return 当前页码。
    */
        public int? getPage() {
               	return page;
            }
    
    /**
     * 设置当前页码。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPage(int page) {
     	         	    this.page = page;
     	        }
    
        [DataMember(Order = 5)]
    private int? pageSize;
    
        /**
       * @return 每页个数，最大50。
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页个数，最大50。     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
    
  }
}