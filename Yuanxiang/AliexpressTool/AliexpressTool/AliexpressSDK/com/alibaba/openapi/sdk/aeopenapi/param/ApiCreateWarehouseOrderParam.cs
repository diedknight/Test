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
public class ApiCreateWarehouseOrderParam {

       [DataMember(Order = 1)]
    private long? tradeOrderId;
    
        /**
       * @return 交易订单号
    */
        public long? getTradeOrderId() {
               	return tradeOrderId;
            }
    
    /**
     * 设置交易订单号     *
     * 参数示例：<pre>60000970354018</pre>     
             * 此参数必填
          */
    public void setTradeOrderId(long tradeOrderId) {
     	         	    this.tradeOrderId = tradeOrderId;
     	        }
    
        [DataMember(Order = 2)]
    private string tradeOrderFrom;
    
        /**
       * @return 交易订单来源,AE订单为ESCROW ；
    */
        public string getTradeOrderFrom() {
               	return tradeOrderFrom;
            }
    
    /**
     * 设置交易订单来源,AE订单为ESCROW ；     *
     * 参数示例：<pre>ESCROW</pre>     
             * 此参数必填
          */
    public void setTradeOrderFrom(string tradeOrderFrom) {
     	         	    this.tradeOrderFrom = tradeOrderFrom;
     	        }
    
        [DataMember(Order = 3)]
    private string warehouseCarrierService;
    
        /**
       * @return ”根据订单号获取线上发货物流方案“API获取用户选择的实际发货物流服务（物流服务key,即仓库服务名称)例如：HRB_WLB_ZTOGZ是 中俄航空 Ruston广州仓库； 
HRB_WLB_RUSTONHEB为哈尔滨备货仓暂不支持，该渠道请做忽略。
    */
        public string getWarehouseCarrierService() {
               	return warehouseCarrierService;
            }
    
    /**
     * 设置”根据订单号获取线上发货物流方案“API获取用户选择的实际发货物流服务（物流服务key,即仓库服务名称)例如：HRB_WLB_ZTOGZ是 中俄航空 Ruston广州仓库； 
HRB_WLB_RUSTONHEB为哈尔滨备货仓暂不支持，该渠道请做忽略。     *
     * 参数示例：<pre>CPAM_WLB_FPXSZ;CPAM_WLB_CPHSH;CPAM_WLB_ZTOBJ;HRB_WLB_ZTOGZ;HRB_WLB_ZTOSH</pre>     
             * 此参数必填
          */
    public void setWarehouseCarrierService(string warehouseCarrierService) {
     	         	    this.warehouseCarrierService = warehouseCarrierService;
     	        }
    
        [DataMember(Order = 4)]
    private long? domesticLogisticsCompanyId;
    
        /**
       * @return 国内快递ID
    */
        public long? getDomesticLogisticsCompanyId() {
               	return domesticLogisticsCompanyId;
            }
    
    /**
     * 设置国内快递ID     *
     * 参数示例：<pre>505(物流公司是other时,ID为-1)</pre>     
             * 此参数必填
          */
    public void setDomesticLogisticsCompanyId(long domesticLogisticsCompanyId) {
     	         	    this.domesticLogisticsCompanyId = domesticLogisticsCompanyId;
     	        }
    
        [DataMember(Order = 5)]
    private string domesticLogisticsCompany;
    
        /**
       * @return 国内快递公司名称
    */
        public string getDomesticLogisticsCompany() {
               	return domesticLogisticsCompany;
            }
    
    /**
     * 设置国内快递公司名称     *
     * 参数示例：<pre>物流公司Id为-1时,必填</pre>     
             * 此参数必填
          */
    public void setDomesticLogisticsCompany(string domesticLogisticsCompany) {
     	         	    this.domesticLogisticsCompany = domesticLogisticsCompany;
     	        }
    
        [DataMember(Order = 6)]
    private string domesticTrackingNo;
    
        /**
       * @return 国内快递运单号,长度1-32
    */
        public string getDomesticTrackingNo() {
               	return domesticTrackingNo;
            }
    
    /**
     * 设置国内快递运单号,长度1-32     *
     * 参数示例：<pre>123231231</pre>     
             * 此参数必填
          */
    public void setDomesticTrackingNo(string domesticTrackingNo) {
     	         	    this.domesticTrackingNo = domesticTrackingNo;
     	        }
    
        [DataMember(Order = 7)]
    private string remark;
    
        /**
       * @return 备注
    */
        public string getRemark() {
               	return remark;
            }
    
    /**
     * 设置备注     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setRemark(string remark) {
     	         	    this.remark = remark;
     	        }
    
        [DataMember(Order = 8)]
    private AeopWlDeclareProductDTO[] declareProductDTOs;
    
        /**
       * @return 申报产品信息,列表类型，以json格式来表达。{productId为产品ID(必填,如为礼品,则设置为0);categoryCnDesc为申报中文名称(必填,长度1-20);categoryEnDesc为申报英文名称(必填,长度1-60);productNum产品件数(必填1-999);productDeclareAmount为产品申报金额(必填,0.01-10000.00);productWeight为产品申报重量(必填0.001-2.000);isContainsBattery为是否包含锂电池(必填0/1);scItemId为仓储发货属性代码（团购订单，仓储发货必填，物流服务为RUSTON 哈尔滨备货仓 HRB_WLB_RUSTONHEB，属性代码对应AE商品的sku属性一级，暂时没有提供接口查询属性代码，可以在仓储管理--库存管理页面查看，例如： 团购产品的sku属性White对应属性代码 40414943126）;skuValue为属性名称（团购订单，仓储发货必填，例如：White）;hsCode为产品海关编码，获取相关数据请至：http://www.customs.gov.cn/Tabid/67737/Default.aspx};isAneroidMarkup为是否含非液体化妆品（必填，填0代表不含非液体化妆品；填1代表含非液体化妆品；默认为0）;isOnlyBattery为是否含纯电池产品（必填，填0代表不含纯电池产品；填1代表含纯电池产品；默认为0）;
    */
        public AeopWlDeclareProductDTO[] getDeclareProductDTOs() {
               	return declareProductDTOs;
            }
    
    /**
     * 设置申报产品信息,列表类型，以json格式来表达。{productId为产品ID(必填,如为礼品,则设置为0);categoryCnDesc为申报中文名称(必填,长度1-20);categoryEnDesc为申报英文名称(必填,长度1-60);productNum产品件数(必填1-999);productDeclareAmount为产品申报金额(必填,0.01-10000.00);productWeight为产品申报重量(必填0.001-2.000);isContainsBattery为是否包含锂电池(必填0/1);scItemId为仓储发货属性代码（团购订单，仓储发货必填，物流服务为RUSTON 哈尔滨备货仓 HRB_WLB_RUSTONHEB，属性代码对应AE商品的sku属性一级，暂时没有提供接口查询属性代码，可以在仓储管理--库存管理页面查看，例如： 团购产品的sku属性White对应属性代码 40414943126）;skuValue为属性名称（团购订单，仓储发货必填，例如：White）;hsCode为产品海关编码，获取相关数据请至：http://www.customs.gov.cn/Tabid/67737/Default.aspx};isAneroidMarkup为是否含非液体化妆品（必填，填0代表不含非液体化妆品；填1代表含非液体化妆品；默认为0）;isOnlyBattery为是否含纯电池产品（必填，填0代表不含纯电池产品；填1代表含纯电池产品；默认为0）;     *
     * 参数示例：<pre>[
    {
        "categoryCnDesc": "小米手机",
        "categoryEnDesc": "xiaomi Phone",
        "isContainsBattery": 1,
        "productDeclareAmount": 1,
        "productId": 20003,
        "productNum": 1,
        "productWeight": 10,
        "hsCode": 12345678,
        "isAneroidMarkup": 0,
        "isOnlyBattery": 1
    },
    {
        "categoryCnDesc": "MP3",
        "categoryEnDesc": "MP3",
        "isContainsBattery": 0,
        "productDeclareAmount": 2,
        "productId": 0,
        "productNum": 4,
        "productWeight": 20,
        "hsCode": 12345678,
        "isAneroidMarkup": 1,
        "isOnlyBattery": 0
    }
]</pre>     
             * 此参数必填
          */
    public void setDeclareProductDTOs(AeopWlDeclareProductDTO[] declareProductDTOs) {
     	         	    this.declareProductDTOs = declareProductDTOs;
     	        }
    
        [DataMember(Order = 9)]
    private AddressDTOs addressDTOs;
    
        /**
       * @return 地址信息,包含发货人地址,收货人地址.发货人地址key值是sender; 收货人地址key值是receiver,都必填{country为国家简称,必填;province为省/州,（必填，长度限制1-48字节）;city为城市,（必填，长度限制1-48，可以直接填写城市信息）,county为区县，（收货人地址中不需要填写，发货人地址必填，长度限制1-20字节）；street为街道 ,（选填，长度限制1-90字节）streetAddress为详细地址 ,（必填，长度限制1-90字节）;name为姓名,（必填，长度限制1-90字节）;phone,mobile两者二选一,phone（长度限制1- 54字节）;mobile（长度限制1-30字节）;email邮箱非必填（长度限制1-64字节）;trademanageId旺旺（非必填，长度限制1-32字节）;如果是中俄航空Ruston需要揽收的订单，则再添加揽收地址信息，key值是pickup,字段同上，内容必须是中文（如无需揽收，则不必传pickup的值）
    */
        public AddressDTOs getAddressDTOs() {
               	return addressDTOs;
            }
    
    /**
     * 设置地址信息,包含发货人地址,收货人地址.发货人地址key值是sender; 收货人地址key值是receiver,都必填{country为国家简称,必填;province为省/州,（必填，长度限制1-48字节）;city为城市,（必填，长度限制1-48，可以直接填写城市信息）,county为区县，（收货人地址中不需要填写，发货人地址必填，长度限制1-20字节）；street为街道 ,（选填，长度限制1-90字节）streetAddress为详细地址 ,（必填，长度限制1-90字节）;name为姓名,（必填，长度限制1-90字节）;phone,mobile两者二选一,phone（长度限制1- 54字节）;mobile（长度限制1-30字节）;email邮箱非必填（长度限制1-64字节）;trademanageId旺旺（非必填，长度限制1-32字节）;如果是中俄航空Ruston需要揽收的订单，则再添加揽收地址信息，key值是pickup,字段同上，内容必须是中文（如无需揽收，则不必传pickup的值）     *
     * 参数示例：<pre>{"receiver":{"city":"Russian City","country":"BR","email":"db1007825240@alibaba.com","fax":"23 3423 324","memberType":"receiver","mobile":"123123","name":"Mrs.Kson","phone":"23 05 1231232","postcode":"123456","province":"Russian State","streetAddress":"abasa basd basd ","trademanageId":"db1007825240"},"sender":{"city":"310100","country":"CN","county":"310115","email":"hjy_seller@aliqatest.com","memberType":"sender","name":"lisi","phone":"123123123","postcode":"310052","province":"310100","streetAddress":"dong da jie No.123","trademanageId":"hjy_seller"}}</pre>     
             * 此参数必填
          */
    public void setAddressDTOs(AddressDTOs addressDTOs) {
     	         	    this.addressDTOs = addressDTOs;
     	        }
    
    
  }
}