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
public class ApiFindOrderListQueryParam {

       [DataMember(Order = 1)]
    private int? page;
    
        /**
       * @return 当前页码
    */
        public int? getPage() {
               	return page;
            }
    
    /**
     * 设置当前页码     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setPage(int page) {
     	         	    this.page = page;
     	        }
    
        [DataMember(Order = 2)]
    private int? pageSize;
    
        /**
       * @return 每页个数，最大50
    */
        public int? getPageSize() {
               	return pageSize;
            }
    
    /**
     * 设置每页个数，最大50     *
     * 参数示例：<pre>50</pre>     
             * 此参数必填
          */
    public void setPageSize(int pageSize) {
     	         	    this.pageSize = pageSize;
     	        }
    
        [DataMember(Order = 3)]
    private string createDateStart;
    
        /**
       * @return 订单创建时间起始值，格式: mm/dd/yyyy hh:mm:ss,如10/08/2013 00:00:00
倘若时间维度未精确到时分秒，故该时间条件筛选不许生效。
    */
        public string getCreateDateStart() {
               	return createDateStart;
            }
    
    /**
     * 设置订单创建时间起始值，格式: mm/dd/yyyy hh:mm:ss,如10/08/2013 00:00:00
倘若时间维度未精确到时分秒，故该时间条件筛选不许生效。     *
     * 参数示例：<pre>10/01/2015 00:00:00</pre>     
             * 此参数必填
          */
    public void setCreateDateStart(string createDateStart) {
     	         	    this.createDateStart = createDateStart;
     	        }
    
        [DataMember(Order = 4)]
    private string createDateEnd;
    
        /**
       * @return 订单创建时间结束值，格式: mm/dd/yyyy hh:mm:ss,如10/08/2013 00:00:00
倘若时间维度未精确到时分秒，故该时间条件筛选不许生效。
    */
        public string getCreateDateEnd() {
               	return createDateEnd;
            }
    
    /**
     * 设置订单创建时间结束值，格式: mm/dd/yyyy hh:mm:ss,如10/08/2013 00:00:00
倘若时间维度未精确到时分秒，故该时间条件筛选不许生效。     *
     * 参数示例：<pre>10/07/2015 00:00:00</pre>     
             * 此参数必填
          */
    public void setCreateDateEnd(string createDateEnd) {
     	         	    this.createDateEnd = createDateEnd;
     	        }
    
        [DataMember(Order = 5)]
    private string orderStatus;
    
        /**
       * @return 订单状态：
PLACE_ORDER_SUCCESS:等待买家付款;
IN_CANCEL:买家申请取消;
WAIT_SELLER_SEND_GOODS:等待您发货;
SELLER_PART_SEND_GOODS:部分发货;
WAIT_BUYER_ACCEPT_GOODS:等待买家收货;
FUND_PROCESSING:买卖家达成一致，资金处理中；
IN_ISSUE:含纠纷中的订单;
IN_FROZEN:冻结中的订单;
WAIT_SELLER_EXAMINE_MONEY:等待您确认金额;
RISK_CONTROL:订单处于风控24小时中，从买家在线支付完成后开始，持续24小时。
以上状态查询可分别做单独查询，不传订单状态查询订单信息不包含（FINISH，已结束订单状态）
FINISH:已结束的订单，需单独查询。
    */
        public string getOrderStatus() {
               	return orderStatus;
            }
    
    /**
     * 设置订单状态：
PLACE_ORDER_SUCCESS:等待买家付款;
IN_CANCEL:买家申请取消;
WAIT_SELLER_SEND_GOODS:等待您发货;
SELLER_PART_SEND_GOODS:部分发货;
WAIT_BUYER_ACCEPT_GOODS:等待买家收货;
FUND_PROCESSING:买卖家达成一致，资金处理中；
IN_ISSUE:含纠纷中的订单;
IN_FROZEN:冻结中的订单;
WAIT_SELLER_EXAMINE_MONEY:等待您确认金额;
RISK_CONTROL:订单处于风控24小时中，从买家在线支付完成后开始，持续24小时。
以上状态查询可分别做单独查询，不传订单状态查询订单信息不包含（FINISH，已结束订单状态）
FINISH:已结束的订单，需单独查询。     *
     * 参数示例：<pre>PLACE_ORDER_SUCCESS</pre>     
             * 此参数必填
          */
    public void setOrderStatus(string orderStatus) {
     	         	    this.orderStatus = orderStatus;
     	        }
    
    
  }
}