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
public class ApiQueryIssueListResult {

       [DataMember(Order = 1)]
    private IssueAPIIssueDTO[] dataList;
    
        /**
       * @return 纠纷数据
    */
        public IssueAPIIssueDTO[] getDataList() {
               	return dataList;
            }
    
    /**
     * 设置纠纷数据     *
          
             * 此参数必填
          */
    public void setDataList(IssueAPIIssueDTO[] dataList) {
     	         	    this.dataList = dataList;
     	        }
    
        [DataMember(Order = 2)]
    private int? totalItem;
    
        /**
       * @return 纠纷总数
    */
        public int? getTotalItem() {
               	return totalItem;
            }
    
    /**
     * 设置纠纷总数     *
          
             * 此参数必填
          */
    public void setTotalItem(int totalItem) {
     	         	    this.totalItem = totalItem;
     	        }
    
        [DataMember(Order = 3)]
    private int? pageSize;
    
        /**
       * @return 每页条数
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页条数     *
          
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
        [DataMember(Order = 4)]
    private int? currentPage;
    
        /**
       * @return 当前页数
    */
        public int? getCurrentPage() {
               	return currentPage;
            }
    
    /**
     * 设置当前页数     *
          
             * 此参数必填
          */
    public void setCurrentPage(int currentPage) {
     	         	    this.currentPage = currentPage;
     	        }
    
        [DataMember(Order = 5)]
    private bool? success;
    
        /**
       * @return 是否成功
    */
        public bool? getSuccess() {
               	return success;
            }
    
    /**
     * 设置是否成功     *
          
             * 此参数必填
          */
    public void setSuccess(bool success) {
     	         	    this.success = success;
     	        }
    
        [DataMember(Order = 6)]
    private string code;
    
        /**
       * @return 错误码，当success=false时有值
    */
        public string getCode() {
               	return code;
            }
    
    /**
     * 设置错误码，当success=false时有值     *
          
             * 此参数必填
          */
    public void setCode(string code) {
     	         	    this.code = code;
     	        }
    
        [DataMember(Order = 7)]
    private string msg;
    
        /**
       * @return 错误原因，当success=false时有值
    */
        public string getMsg() {
               	return msg;
            }
    
    /**
     * 设置错误原因，当success=false时有值     *
          
             * 此参数必填
          */
    public void setMsg(string msg) {
     	         	    this.msg = msg;
     	        }
    
    
  }
}