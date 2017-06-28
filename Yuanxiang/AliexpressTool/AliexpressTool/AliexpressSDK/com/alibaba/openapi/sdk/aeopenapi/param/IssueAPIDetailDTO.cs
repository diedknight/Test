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
public class IssueAPIDetailDTO {

       [DataMember(Order = 1)]
    private string snapshotUrl;
    
        /**
       * @return 交易快照
    */
        public string getSnapshotUrl() {
               	return snapshotUrl;
            }
    
    /**
     * 设置交易快照     *
     * 参数示例：<pre>http://www.aliexpress.com/snapshot/3005612434.html?orderId=30***********804</pre>     
             * 此参数必填
          */
    public void setSnapshotUrl(string snapshotUrl) {
     	         	    this.snapshotUrl = snapshotUrl;
     	        }
    
        [DataMember(Order = 2)]
    private IssueAPIIssueDTO issueAPIIssueDTO;
    
        /**
       * @return 纠纷详情
    */
        public IssueAPIIssueDTO getIssueAPIIssueDTO() {
               	return issueAPIIssueDTO;
            }
    
    /**
     * 设置纠纷详情     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setIssueAPIIssueDTO(IssueAPIIssueDTO issueAPIIssueDTO) {
     	         	    this.issueAPIIssueDTO = issueAPIIssueDTO;
     	        }
    
    
  }
}