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
    public class ApiFindOrderBaseInfoResult
    {

        [DataMember(Order = 1)]
        private string frozenStatus;

        /**
       * @return 冻结状态(&quot;NO_FROZEN&quot;无冻结；&quot;IN_FROZEN&quot;冻结中；)
    */
        public string getFrozenStatus()
        {
            return frozenStatus;
        }

        /**
         * 设置冻结状态(&quot;NO_FROZEN&quot;无冻结；&quot;IN_FROZEN&quot;冻结中；)     *
         * 参数示例：<pre>NO_FROZEN</pre>     
                 * 此参数必填
              */
        public void setFrozenStatus(string frozenStatus)
        {
            this.frozenStatus = frozenStatus;
        }

        [DataMember(Order = 2)]
        private string fundStatus;

        /**
       * @return 资金状态(NOT_PAY,未付款； PAY_SUCCESS,付款成功； WAIT_SELLER_CHECK，卖家验款)
    */
        public string getFundStatus()
        {
            return fundStatus;
        }

        /**
         * 设置资金状态(NOT_PAY,未付款； PAY_SUCCESS,付款成功； WAIT_SELLER_CHECK，卖家验款)     *
         * 参数示例：<pre>NOT_PAY</pre>     
                 * 此参数必填
              */
        public void setFundStatus(string fundStatus)
        {
            this.fundStatus = fundStatus;
        }

        [DataMember(Order = 3)]
        private string gmtCreate;

        /**
       * @return 创建时间
    */
        public string getGmtCreate()
        {
            return gmtCreate;
        }

        /**
         * 设置创建时间     *
         * 参数示例：<pre></pre>     
                 * 此参数必填
              */
        public void setGmtCreate(string gmtCreate)
        {
            this.gmtCreate = gmtCreate;
        }

        [DataMember(Order = 4)]
        private string gmtModified;

        /**
       * @return 修改时间
    */
        public string getGmtModified()
        {
            return gmtModified;
        }

        /**
         * 设置修改时间     *
         * 参数示例：<pre></pre>     
                 * 此参数必填
              */
        public void setGmtModified(string gmtModified)
        {
            this.gmtModified = gmtModified;
        }

        [DataMember(Order = 5)]
        private string issueStatus;

        /**
       * @return 纠纷状态(&quot;NO_ISSUE&quot;无纠纷；&quot;IN_ISSUE&quot;纠纷中；&ldquo;END_ISSUE&rdquo;纠纷结束。) frozenStatus:冻结状态(&quot;NO_FROZEN&quot;无冻结；&quot;IN_FROZEN&quot;冻结中；)
    */
        public string getIssueStatus()
        {
            return issueStatus;
        }

        /**
         * 设置纠纷状态(&quot;NO_ISSUE&quot;无纠纷；&quot;IN_ISSUE&quot;纠纷中；&ldquo;END_ISSUE&rdquo;纠纷结束。) frozenStatus:冻结状态(&quot;NO_FROZEN&quot;无冻结；&quot;IN_FROZEN&quot;冻结中；)     *
         * 参数示例：<pre>NO_ISSUE</pre>     
                 * 此参数必填
              */
        public void setIssueStatus(string issueStatus)
        {
            this.issueStatus = issueStatus;
        }

        [DataMember(Order = 6)]
        private string loanStatus;

        /**
       * @return 订单放款状态：wait_loan 未放款，loan_ok已放款。
    */
        public string getLoanStatus()
        {
            return loanStatus;
        }

        /**
         * 设置订单放款状态：wait_loan 未放款，loan_ok已放款。     *
         * 参数示例：<pre></pre>     
                 * 此参数必填
              */
        public void setLoanStatus(string loanStatus)
        {
            this.loanStatus = loanStatus;
        }

        [DataMember(Order = 7)]
        private string logisticsStatus;

        /**
       * @return 物流状态
    */
        public string getLogisticsStatus()
        {
            return logisticsStatus;
        }

        /**
         * 设置物流状态     *
         * 参数示例：<pre></pre>     
                 * 此参数必填
              */
        public void setLogisticsStatus(string logisticsStatus)
        {
            this.logisticsStatus = logisticsStatus;
        }

        [DataMember(Order = 8)]
        private string orderStatus;

        /**
       * @return 订单状态
    */
        public string getOrderStatus()
        {
            return orderStatus;
        }

        /**
         * 设置订单状态     *
         * 参数示例：<pre></pre>     
                 * 此参数必填
              */
        public void setOrderStatus(string orderStatus)
        {
            this.orderStatus = orderStatus;
        }

        [DataMember(Order = 9)]
        private string sellerOperatorLoginId;

        /**
       * @return 卖家ID
    */
        public string getSellerOperatorLoginId()
        {
            return sellerOperatorLoginId;
        }

        /**
         * 设置卖家ID     *
         * 参数示例：<pre></pre>     
                 * 此参数必填
              */
        public void setSellerOperatorLoginId(string sellerOperatorLoginId)
        {
            this.sellerOperatorLoginId = sellerOperatorLoginId;
        }

        [DataMember(Order = 10)]
        private string sellerSignerFullname;

        /**
       * @return 卖家全名
    */
        public string getSellerSignerFullname()
        {
            return sellerSignerFullname;
        }

        /**
         * 设置卖家全名     *
         * 参数示例：<pre></pre>     
                 * 此参数必填
              */
        public void setSellerSignerFullname(string sellerSignerFullname)
        {
            this.sellerSignerFullname = sellerSignerFullname;
        }
    }
}