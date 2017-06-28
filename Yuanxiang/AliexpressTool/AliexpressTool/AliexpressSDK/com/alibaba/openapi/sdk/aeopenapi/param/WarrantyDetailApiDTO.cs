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
public class WarrantyDetailApiDTO {

       [DataMember(Order = 1)]
    private long? orderId;
    
        /**
       * @return 主订单号
    */
        public long? getOrderId() {
               	return orderId;
            }
    
    /**
     * 设置主订单号     *
     * 参数示例：<pre>60023440513182</pre>     
             * 此参数必填
          */
    public void setOrderId(long orderId) {
     	         	    this.orderId = orderId;
     	        }
    
        [DataMember(Order = 2)]
    private string orderDate;
    
        /**
       * @return 下单日期，格式yyyy-MM-dd HH:mm:ss
    */
        public string getOrderDate() {
               	return orderDate;
            }
    
    /**
     * 设置下单日期，格式yyyy-MM-dd HH:mm:ss     *
     * 参数示例：<pre>2015-10-23 11:33:22</pre>     
             * 此参数必填
          */
    public void setOrderDate(string orderDate) {
     	         	    this.orderDate = orderDate;
     	        }
    
        [DataMember(Order = 3)]
    private string productName;
    
        /**
       * @return 产品名称，英文
    */
        public string getProductName() {
               	return productName;
            }
    
    /**
     * 设置产品名称，英文     *
     * 参数示例：<pre>testetst</pre>     
             * 此参数必填
          */
    public void setProductName(string productName) {
     	         	    this.productName = productName;
     	        }
    
        [DataMember(Order = 4)]
    private string skuInfo;
    
        /**
       * @return 商品sku，json序列化
    */
        public string getSkuInfo() {
               	return skuInfo;
            }
    
    /**
     * 设置商品sku，json序列化     *
     * 参数示例：<pre>{“color”:”red”,”size”:”s”}</pre>     
             * 此参数必填
          */
    public void setSkuInfo(string skuInfo) {
     	         	    this.skuInfo = skuInfo;
     	        }
    
        [DataMember(Order = 5)]
    private string productSnapshotUrl;
    
        /**
       * @return 商品快照url
    */
        public string getProductSnapshotUrl() {
               	return productSnapshotUrl;
            }
    
    /**
     * 设置商品快照url     *
     * 参数示例：<pre>http://www.aliexpress.com:1080/snapshot/3000438019.html?orderId=60023440513182</pre>     
             * 此参数必填
          */
    public void setProductSnapshotUrl(string productSnapshotUrl) {
     	         	    this.productSnapshotUrl = productSnapshotUrl;
     	        }
    
        [DataMember(Order = 6)]
    private string productProperties;
    
        /**
       * @return 产品属性，json序列化
    */
        public string getProductProperties() {
               	return productProperties;
            }
    
    /**
     * 设置产品属性，json序列化     *
     * 参数示例：<pre>{"a":"aa","b":"bb"}</pre>     
             * 此参数必填
          */
    public void setProductProperties(string productProperties) {
     	         	    this.productProperties = productProperties;
     	        }
    
        [DataMember(Order = 7)]
    private string storeUrl;
    
        /**
       * @return 店铺url
    */
        public string getStoreUrl() {
               	return storeUrl;
            }
    
    /**
     * 设置店铺url     *
     * 参数示例：<pre>http://www.aliexpress.com:1080/store/1335004</pre>     
             * 此参数必填
          */
    public void setStoreUrl(string storeUrl) {
     	         	    this.storeUrl = storeUrl;
     	        }
    
        [DataMember(Order = 8)]
    private string country;
    
        /**
       * @return 买家国家
    */
        public string getCountry() {
               	return country;
            }
    
    /**
     * 设置买家国家     *
     * 参数示例：<pre>RU</pre>     
             * 此参数必填
          */
    public void setCountry(string country) {
     	         	    this.country = country;
     	        }
    
        [DataMember(Order = 9)]
    private string province;
    
        /**
       * @return 买家省
    */
        public string getProvince() {
               	return province;
            }
    
    /**
     * 设置买家省     *
     * 参数示例：<pre>Idaho</pre>     
             * 此参数必填
          */
    public void setProvince(string province) {
     	         	    this.province = province;
     	        }
    
        [DataMember(Order = 10)]
    private string city;
    
        /**
       * @return 买家城市
    */
        public string getCity() {
               	return city;
            }
    
    /**
     * 设置买家城市     *
     * 参数示例：<pre>setest</pre>     
             * 此参数必填
          */
    public void setCity(string city) {
     	         	    this.city = city;
     	        }
    
        [DataMember(Order = 11)]
    private string streetAddress;
    
        /**
       * @return 买家街道
    */
        public string getStreetAddress() {
               	return streetAddress;
            }
    
    /**
     * 设置买家街道     *
     * 参数示例：<pre>streetAddress</pre>     
             * 此参数必填
          */
    public void setStreetAddress(string streetAddress) {
     	         	    this.streetAddress = streetAddress;
     	        }
    
        [DataMember(Order = 12)]
    private string postCode;
    
        /**
       * @return 买家邮编
    */
        public string getPostCode() {
               	return postCode;
            }
    
    /**
     * 设置买家邮编     *
     * 参数示例：<pre>1000453</pre>     
             * 此参数必填
          */
    public void setPostCode(string postCode) {
     	         	    this.postCode = postCode;
     	        }
    
        [DataMember(Order = 13)]
    private string sendGoodsTime;
    
        /**
       * @return 产品发货时间，yyyy-MM-dd HH:mm:ss
    */
        public string getSendGoodsTime() {
               	return sendGoodsTime;
            }
    
    /**
     * 设置产品发货时间，yyyy-MM-dd HH:mm:ss     *
     * 参数示例：<pre>2015-10-25 11:33:22</pre>     
             * 此参数必填
          */
    public void setSendGoodsTime(string sendGoodsTime) {
     	         	    this.sendGoodsTime = sendGoodsTime;
     	        }
    
        [DataMember(Order = 14)]
    private string getGoodsTime;
    
        /**
       * @return 产品收货时间，yyyy-MM-dd HH:mm:ss
    */
        public string getGetGoodsTime() {
               	return getGoodsTime;
            }
    
    /**
     * 设置产品收货时间，yyyy-MM-dd HH:mm:ss     *
     * 参数示例：<pre>2015-11-20 11:33:22</pre>     
             * 此参数必填
          */
    public void setGetGoodsTime(string getGoodsTime) {
     	         	    this.getGoodsTime = getGoodsTime;
     	        }
    
        [DataMember(Order = 15)]
    private int? productCount;
    
        /**
       * @return 产品数量
    */
        public int? getProductCount() {
               	return productCount;
            }
    
    /**
     * 设置产品数量     *
     * 参数示例：<pre>1</pre>     
             * 此参数必填
          */
    public void setProductCount(int productCount) {
     	         	    this.productCount = productCount;
     	        }
    
        [DataMember(Order = 16)]
    private string status;
    
        /**
       * @return 交易状态。订单状态：
1.PLACE_ORDER_SUCCESS（下单成功）2.WAIT_SELLER_SEND_GOODS（待卖家发货）3.SELLER_PART_SEND_GOODS（待卖家部分发货）4.WAIT_BUYER_ACCEPT_GOODS（等待买家收货）5.FUND_PROCESSING（资金处理中）6.FINISH（交易成功）
    */
        public string getStatus() {
               	return status;
            }
    
    /**
     * 设置交易状态。订单状态：
1.PLACE_ORDER_SUCCESS（下单成功）2.WAIT_SELLER_SEND_GOODS（待卖家发货）3.SELLER_PART_SEND_GOODS（待卖家部分发货）4.WAIT_BUYER_ACCEPT_GOODS（等待买家收货）5.FUND_PROCESSING（资金处理中）6.FINISH（交易成功）     *
     * 参数示例：<pre>FINISH</pre>     
             * 此参数必填
          */
    public void setStatus(string status) {
     	         	    this.status = status;
     	        }
    
        [DataMember(Order = 17)]
    private long? productAmount;
    
        /**
       * @return 产品金额，分
    */
        public long? getProductAmount() {
               	return productAmount;
            }
    
    /**
     * 设置产品金额，分     *
     * 参数示例：<pre>1234</pre>     
             * 此参数必填
          */
    public void setProductAmount(long productAmount) {
     	         	    this.productAmount = productAmount;
     	        }
    
        [DataMember(Order = 18)]
    private long? serviceAmount;
    
        /**
       * @return 保修金额，分
    */
        public long? getServiceAmount() {
               	return serviceAmount;
            }
    
    /**
     * 设置保修金额，分     *
     * 参数示例：<pre>23</pre>     
             * 此参数必填
          */
    public void setServiceAmount(long serviceAmount) {
     	         	    this.serviceAmount = serviceAmount;
     	        }
    
        [DataMember(Order = 19)]
    private string payCurrency;
    
        /**
       * @return 支付币种
    */
        public string getPayCurrency() {
               	return payCurrency;
            }
    
    /**
     * 设置支付币种     *
     * 参数示例：<pre>USD</pre>     
             * 此参数必填
          */
    public void setPayCurrency(string payCurrency) {
     	         	    this.payCurrency = payCurrency;
     	        }
    
        [DataMember(Order = 20)]
    private string orderCurrency;
    
        /**
       * @return 订单币种
    */
        public string getOrderCurrency() {
               	return orderCurrency;
            }
    
    /**
     * 设置订单币种     *
     * 参数示例：<pre>USD</pre>     
             * 此参数必填
          */
    public void setOrderCurrency(string orderCurrency) {
     	         	    this.orderCurrency = orderCurrency;
     	        }
    
        [DataMember(Order = 21)]
    private double? exchange;
    
        /**
       * @return 汇率
    */
        public double? getExchange() {
               	return exchange;
            }
    
    /**
     * 设置汇率     *
     * 参数示例：<pre>1.0000</pre>     
             * 此参数必填
          */
    public void setExchange(double exchange) {
     	         	    this.exchange = exchange;
     	        }
    
        [DataMember(Order = 22)]
    private string warrantyType;
    
        /**
       * @return 保修类型.
1.warranty_common（普通保修）2.warranty_refund（只退不修）
    */
        public string getWarrantyType() {
               	return warrantyType;
            }
    
    /**
     * 设置保修类型.
1.warranty_common（普通保修）2.warranty_refund（只退不修）     *
     * 参数示例：<pre>warranty_common</pre>     
             * 此参数必填
          */
    public void setWarrantyType(string warrantyType) {
     	         	    this.warrantyType = warrantyType;
     	        }
    
        [DataMember(Order = 23)]
    private string serviceStartTime;
    
        /**
       * @return 服务开始时间，买家确认收货之前为空
    */
        public string getServiceStartTime() {
               	return serviceStartTime;
            }
    
    /**
     * 设置服务开始时间，买家确认收货之前为空     *
     * 参数示例：<pre>2015-11-25 11:33:22</pre>     
             * 此参数必填
          */
    public void setServiceStartTime(string serviceStartTime) {
     	         	    this.serviceStartTime = serviceStartTime;
     	        }
    
        [DataMember(Order = 24)]
    private string serviceEndTime;
    
        /**
       * @return 服务结束时间，买家确认收货之前为空
    */
        public string getServiceEndTime() {
               	return serviceEndTime;
            }
    
    /**
     * 设置服务结束时间，买家确认收货之前为空     *
     * 参数示例：<pre>2016-11-24 11:33:22</pre>     
             * 此参数必填
          */
    public void setServiceEndTime(string serviceEndTime) {
     	         	    this.serviceEndTime = serviceEndTime;
     	        }
    
        [DataMember(Order = 25)]
    private string warrantyStatus;
    
        /**
       * @return 保修状态
1.notSubmit(未提交)2.create（创建）3.cancel（取消）4.finish（完成）
    */
        public string getWarrantyStatus() {
               	return warrantyStatus;
            }
    
    /**
     * 设置保修状态
1.notSubmit(未提交)2.create（创建）3.cancel（取消）4.finish（完成）     *
     * 参数示例：<pre>create</pre>     
             * 此参数必填
          */
    public void setWarrantyStatus(string warrantyStatus) {
     	         	    this.warrantyStatus = warrantyStatus;
     	        }
    
        [DataMember(Order = 26)]
    private string name;
    
        /**
       * @return 买家姓名,warrantyStatus=notSubmit时为空
    */
        public string getName() {
               	return name;
            }
    
    /**
     * 设置买家姓名,warrantyStatus=notSubmit时为空     *
     * 参数示例：<pre>ctest</pre>     
             * 此参数必填
          */
    public void setName(string name) {
     	         	    this.name = name;
     	        }
    
        [DataMember(Order = 27)]
    private string telephone;
    
        /**
       * @return 买家电话,warrantyStatus=notSubmit时为空
    */
        public string getTelephone() {
               	return telephone;
            }
    
    /**
     * 设置买家电话,warrantyStatus=notSubmit时为空     *
     * 参数示例：<pre>1248945</pre>     
             * 此参数必填
          */
    public void setTelephone(string telephone) {
     	         	    this.telephone = telephone;
     	        }
    
        [DataMember(Order = 28)]
    private string description;
    
        /**
       * @return 买家保修描述,warrantyStatus=notSubmit时为空
    */
        public string getDescription() {
               	return description;
            }
    
    /**
     * 设置买家保修描述,warrantyStatus=notSubmit时为空     *
     * 参数示例：<pre>my phone not work!</pre>     
             * 此参数必填
          */
    public void setDescription(string description) {
     	         	    this.description = description;
     	        }
    
        [DataMember(Order = 29)]
    private string attachments;
    
        /**
       * @return 买家保修提交附件，逗号分隔,warrantyStatus=notSubmit时为空
    */
        public string getAttachments() {
               	return attachments;
            }
    
    /**
     * 设置买家保修提交附件，逗号分隔,warrantyStatus=notSubmit时为空     *
     * 参数示例：<pre>aaa.jpg,b.jpg</pre>     
             * 此参数必填
          */
    public void setAttachments(string attachments) {
     	         	    this.attachments = attachments;
     	        }
    
        [DataMember(Order = 30)]
    private string snapshotId;
    
        /**
       * @return 交易快照id
    */
        public string getSnapshotId() {
               	return snapshotId;
            }
    
    /**
     * 设置交易快照id     *
     * 参数示例：<pre>1111</pre>     
             * 此参数必填
          */
    public void setSnapshotId(string snapshotId) {
     	         	    this.snapshotId = snapshotId;
     	        }
    
        [DataMember(Order = 31)]
    private int? productLeafCategoryId;
    
        /**
       * @return 产品叶子类目id
    */
        public int? getProductLeafCategoryId() {
               	return productLeafCategoryId;
            }
    
    /**
     * 设置产品叶子类目id     *
     * 参数示例：<pre>111</pre>     
             * 此参数必填
          */
    public void setProductLeafCategoryId(int productLeafCategoryId) {
     	         	    this.productLeafCategoryId = productLeafCategoryId;
     	        }
    
        [DataMember(Order = 32)]
    private long? productId;
    
        /**
       * @return 商品id
    */
        public long? getProductId() {
               	return productId;
            }
    
    /**
     * 设置商品id     *
     * 参数示例：<pre>111</pre>     
             * 此参数必填
          */
    public void setProductId(long productId) {
     	         	    this.productId = productId;
     	        }
    
    
  }
}