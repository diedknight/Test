using com.alibaba.openapi.client;
using com.alibaba.openapi.client.entity;
using com.alibaba.openapi.client.policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.alibaba.openapi.sdk.aeopenapi.param;

namespace com.alibaba.openapi.sdk.aeopenapi
{

    class ApiFacade
    {
        private ClientPolicy clientPolicy;

        public ApiFacade(ClientPolicy clientPolicy)
        {
            this.clientPolicy = clientPolicy;
        }

        private SyncAPIClient getAPIClient()
        {
            return new SyncAPIClient(clientPolicy);
        }

        public AuthorizationToken getToken(string code)
        {
            return getAPIClient().getToken(code);
        }

        public AuthorizationToken refreshToken(String refreshToken)
        {
            return getAPIClient().refreshToken(refreshToken);
        }

                    
                       
           
        public AlibabaAeProductRenewExpireResult alibabaAeProductRenewExpire(AlibabaAeProductRenewExpireParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "alibaba.ae.product.renewExpire";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<AlibabaAeProductRenewExpireResult>(request, reqPolicy);
        }
                    
                       
           
        public AlibabaProductPostMultilanguageAeProductResult alibabaProductPostMultilanguageAeProduct(AlibabaProductPostMultilanguageAeProductParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "alibaba.product.postMultilanguageAeProduct";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<AlibabaProductPostMultilanguageAeProductResult>(request, reqPolicy);
        }
                    
                       
           
        public AlibabaAeWarrantieGetListResult alibabaAeWarrantieGetList(AlibabaAeWarrantieGetListParam param) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=false;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "alibaba.ae.warrantie.getList";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
                        
            return this.getAPIClient().send<AlibabaAeWarrantieGetListResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiUploadTempImage4SDKResult apiUploadTempImage4SDK(ApiUploadTempImage4SDKParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.uploadTempImage4SDK";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiUploadTempImage4SDKResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiUploadImage4SDKResult apiUploadImage4SDK(ApiUploadImage4SDKParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.uploadImage4SDK";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiUploadImage4SDKResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiGetNextLevelAddressDataResult apiGetNextLevelAddressData(ApiGetNextLevelAddressDataParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.getNextLevelAddressData";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiGetNextLevelAddressDataResult>(request, reqPolicy);
        }
                                
                       
           
        public ApiGetAllProvinceResult apiGetAllProvince( string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.getAllProvince";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
                        
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiGetAllProvinceResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryWarrantiesInforceResult apiQueryWarrantiesInforce(ApiQueryWarrantiesInforceParam param) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=false;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryWarrantiesInforce";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
                        
            return this.getAPIClient().send<ApiQueryWarrantiesInforceResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryIssueListForSpecialResult apiQueryIssueListForSpecial(ApiQueryIssueListForSpecialParam param) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=false;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryIssueListForSpecial";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
                        
            return this.getAPIClient().send<ApiQueryIssueListForSpecialResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryWarrantiesByOrderIdResult apiQueryWarrantiesByOrderId(ApiQueryWarrantiesByOrderIdParam param) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=false;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryWarrantiesByOrderId";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
                        
            return this.getAPIClient().send<ApiQueryWarrantiesByOrderIdResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiUpdateMsgProcessedResult apiUpdateMsgProcessed(ApiUpdateMsgProcessedParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.updateMsgProcessed";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiUpdateMsgProcessedResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiUpdateMsgRankResult apiUpdateMsgRank(ApiUpdateMsgRankParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.updateMsgRank";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiUpdateMsgRankResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryMsgDetailListByBuyerIdResult apiQueryMsgDetailListByBuyerId(ApiQueryMsgDetailListByBuyerIdParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryMsgDetailListByBuyerId";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryMsgDetailListByBuyerIdResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindAeProductStatusByIdResult apiFindAeProductStatusById(ApiFindAeProductStatusByIdParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findAeProductStatusById";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindAeProductStatusByIdResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryMsgRelationListResult apiQueryMsgRelationList(ApiQueryMsgRelationListParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryMsgRelationList";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryMsgRelationListResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiUpdateMsgReadResult apiUpdateMsgRead(ApiUpdateMsgReadParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.updateMsgRead";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiUpdateMsgReadResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryMsgDetailListResult apiQueryMsgDetailList(ApiQueryMsgDetailListParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryMsgDetailList";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryMsgDetailListResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiAddMsgResult apiAddMsg(ApiAddMsgParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.addMsg";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiAddMsgResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryAccountLevelResult apiQueryAccountLevel(ApiQueryAccountLevelParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryAccountLevel";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryAccountLevelResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiLeaveOrderMessageResult apiLeaveOrderMessage(ApiLeaveOrderMessageParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.leaveOrderMessage";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiLeaveOrderMessageResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiSellerSubmitArbiResult apiSellerSubmitArbi(ApiSellerSubmitArbiParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.sellerSubmitArbi";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiSellerSubmitArbiResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiSellerConrimReceiveGoodsResult apiSellerConrimReceiveGoods(ApiSellerConrimReceiveGoodsParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.sellerConrimReceiveGoods";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiSellerConrimReceiveGoodsResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiSellerAbandonReceiveGoodsResult apiSellerAbandonReceiveGoods(ApiSellerAbandonReceiveGoodsParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.sellerAbandonReceiveGoods";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiSellerAbandonReceiveGoodsResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiSellerRefuseIssueResult apiSellerRefuseIssue(ApiSellerRefuseIssueParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.sellerRefuseIssue";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiSellerRefuseIssueResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiSellerAgreeIssueResult apiSellerAgreeIssue(ApiSellerAgreeIssueParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.sellerAgreeIssue";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiSellerAgreeIssueResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiUploadIssueImageResult apiUploadIssueImage(ApiUploadIssueImageParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.uploadIssueImage";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiUploadIssueImageResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryIssueDetailResult apiQueryIssueDetail(ApiQueryIssueDetailParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryIssueDetail";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryIssueDetailResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryIssueListResult apiQueryIssueList(ApiQueryIssueListParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryIssueList";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryIssueListResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindAeProductProhibitedWordsResult apiFindAeProductProhibitedWords(ApiFindAeProductProhibitedWordsParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findAeProductProhibitedWords";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindAeProductProhibitedWordsResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiEditSingleSkuStockResult apiEditSingleSkuStock(ApiEditSingleSkuStockParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.editSingleSkuStock";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiEditSingleSkuStockResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiEditSingleSkuPriceResult apiEditSingleSkuPrice(ApiEditSingleSkuPriceParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.editSingleSkuPrice";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiEditSingleSkuPriceResult>(request, reqPolicy);
        }
                    
                       
           
        public PushMessageConfirmResult pushMessageConfirm(PushMessageConfirmParam param) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=false;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "push.message.confirm";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
                        
            return this.getAPIClient().send<PushMessageConfirmResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiEditMultilanguageProductResult apiEditMultilanguageProduct(ApiEditMultilanguageProductParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.editMultilanguageProduct";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiEditMultilanguageProductResult>(request, reqPolicy);
        }
                                
                       
           
        public ApiGetRemainingWindowsResult apiGetRemainingWindows( string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.getRemainingWindows";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
                        
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiGetRemainingWindowsResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiExtendsBuyerAcceptGoodsTimeResult apiExtendsBuyerAcceptGoodsTime(ApiExtendsBuyerAcceptGoodsTimeParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.extendsBuyerAcceptGoodsTime";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiExtendsBuyerAcceptGoodsTimeResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiCreateProductGroupResult apiCreateProductGroup(ApiCreateProductGroupParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.createProductGroup";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiCreateProductGroupResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiSetSizeChartResult apiSetSizeChart(ApiSetSizeChartParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.setSizeChart";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiSetSizeChartResult>(request, reqPolicy);
        }
                                
                       
           
        public ApiGetWindowProductsResult apiGetWindowProducts( string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.getWindowProducts";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
                        
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiGetWindowProductsResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindOrderTradeInfoResult apiFindOrderTradeInfo(ApiFindOrderTradeInfoParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findOrderTradeInfo";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindOrderTradeInfoResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindOrderReceiptInfoResult apiFindOrderReceiptInfo(ApiFindOrderReceiptInfoParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findOrderReceiptInfo";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindOrderReceiptInfoResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindOrderBaseInfoResult apiFindOrderBaseInfo(ApiFindOrderBaseInfoParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="GET";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findOrderBaseInfo";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindOrderBaseInfoResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindOrderListSimpleQueryResult apiFindOrderListSimpleQuery(ApiFindOrderListSimpleQueryParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findOrderListSimpleQuery";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindOrderListSimpleQueryResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiEditProductCategoryAttributesResult apiEditProductCategoryAttributes(ApiEditProductCategoryAttributesParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.editProductCategoryAttributes";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiEditProductCategoryAttributesResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiSetGroupsResult apiSetGroups(ApiSetGroupsParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.setGroups";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiSetGroupsResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiUpdateDeliveriedConfirmationFileResult apiUpdateDeliveriedConfirmationFile(ApiUpdateDeliveriedConfirmationFileParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.updateDeliveriedConfirmationFile";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiUpdateDeliveriedConfirmationFileResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryProductGroupIdByProductIdResult apiQueryProductGroupIdByProductId(ApiQueryProductGroupIdByProductIdParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryProductGroupIdByProductId";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryProductGroupIdByProductIdResult>(request, reqPolicy);
        }
                                
                       
           
        public ApiGetProductGroupListResult apiGetProductGroupList( string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.getProductGroupList";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
                        
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiGetProductGroupListResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindLoanListQueryResult apiFindLoanListQuery(ApiFindLoanListQueryParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findLoanListQuery";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindLoanListQueryResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryProductViewedInfoEverydayByIdResult apiQueryProductViewedInfoEverydayById(ApiQueryProductViewedInfoEverydayByIdParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryProductViewedInfoEverydayById";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryProductViewedInfoEverydayByIdResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryProductAddCartInfoEverydayByIdResult apiQueryProductAddCartInfoEverydayById(ApiQueryProductAddCartInfoEverydayByIdParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryProductAddCartInfoEverydayById";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryProductAddCartInfoEverydayByIdResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryProductFavoritedInfoEverydayByIdResult apiQueryProductFavoritedInfoEverydayById(ApiQueryProductFavoritedInfoEverydayByIdParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryProductFavoritedInfoEverydayById";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryProductFavoritedInfoEverydayByIdResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiEditProductCidAttIdSkuResult apiEditProductCidAttIdSku(ApiEditProductCidAttIdSkuParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.editProductCidAttIdSku";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiEditProductCidAttIdSkuResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiEditSimpleProductFiledResult apiEditSimpleProductFiled(ApiEditSimpleProductFiledParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.editSimpleProductFiled";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiEditSimpleProductFiledResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiSellerModifiedShipmentResult apiSellerModifiedShipment(ApiSellerModifiedShipmentParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.sellerModifiedShipment";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiSellerModifiedShipmentResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryOpenAnouncementResult apiQueryOpenAnouncement(ApiQueryOpenAnouncementParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryOpenAnouncement";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryOpenAnouncementResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiClaimTaobaoProducts4APIResult apiClaimTaobaoProducts4API(ApiClaimTaobaoProducts4APIParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.claimTaobaoProducts4API";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiClaimTaobaoProducts4APIResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiSetShopwindowProductResult apiSetShopwindowProduct(ApiSetShopwindowProductParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.setShopwindowProduct";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiSetShopwindowProductResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiGetOnlineLogisticsInfoResult apiGetOnlineLogisticsInfo(ApiGetOnlineLogisticsInfoParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.getOnlineLogisticsInfo";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiGetOnlineLogisticsInfoResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiGetOnlineLogisticsServiceListByOrderIdResult apiGetOnlineLogisticsServiceListByOrderId(ApiGetOnlineLogisticsServiceListByOrderIdParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.getOnlineLogisticsServiceListByOrderId";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiGetOnlineLogisticsServiceListByOrderIdResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiCreateWarehouseOrderResult apiCreateWarehouseOrder(ApiCreateWarehouseOrderParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.createWarehouseOrder";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiCreateWarehouseOrderResult>(request, reqPolicy);
        }
                                
                       
           
        public ApiQureyWlbDomesticLogisticsCompanyResult apiQureyWlbDomesticLogisticsCompany( string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.qureyWlbDomesticLogisticsCompany";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
                        
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQureyWlbDomesticLogisticsCompanyResult>(request, reqPolicy);
        }
                                
                       
           
        public ApiGetPhotoBankInfoResult apiGetPhotoBankInfo( string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.getPhotoBankInfo";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
                        
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiGetPhotoBankInfoResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiDelUnUsePhotoResult apiDelUnUsePhoto(ApiDelUnUsePhotoParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.delUnUsePhoto";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiDelUnUsePhotoResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryPromiseTemplateByIdResult apiQueryPromiseTemplateById(ApiQueryPromiseTemplateByIdParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryPromiseTemplateById";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryPromiseTemplateByIdResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiListTbProductByIdsResult apiListTbProductByIds(ApiListTbProductByIdsParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.listTbProductByIds";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiListTbProductByIdsResult>(request, reqPolicy);
        }
                                
                       
           
        public ApiListLogisticsServiceResult apiListLogisticsService( string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.listLogisticsService";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
                        
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiListLogisticsServiceResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiSellerShipmentResult apiSellerShipment(ApiSellerShipmentParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.sellerShipment";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiSellerShipmentResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiQueryTrackingResultResult apiQueryTrackingResult(ApiQueryTrackingResultParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.queryTrackingResult";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiQueryTrackingResultResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindOrderListQueryResult apiFindOrderListQuery(ApiFindOrderListQueryParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findOrderListQuery";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindOrderListQueryResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindAeProductDetailModuleListByQureyResult apiFindAeProductDetailModuleListByQurey(ApiFindAeProductDetailModuleListByQureyParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findAeProductDetailModuleListByQurey";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindAeProductDetailModuleListByQureyResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindAeProductModuleByIdResult apiFindAeProductModuleById(ApiFindAeProductModuleByIdParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findAeProductModuleById";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindAeProductModuleByIdResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiOnlineAeProductResult apiOnlineAeProduct(ApiOnlineAeProductParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.onlineAeProduct";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiOnlineAeProductResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiOfflineAeProductResult apiOfflineAeProduct(ApiOfflineAeProductParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.offlineAeProduct";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiOfflineAeProductResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiEditAeProductResult apiEditAeProduct(ApiEditAeProductParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.editAeProduct";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiEditAeProductResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindProductInfoListQueryV2Result apiFindProductInfoListQueryV2(ApiFindProductInfoListQueryV2Param param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=false;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findProductInfoListQuery";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 2;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindProductInfoListQueryV2Result>(request, reqPolicy);
        }
                    
                       
           
        public ApiFindProductInfoListQueryResult apiFindProductInfoListQuery(ApiFindProductInfoListQueryParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.findProductInfoListQuery";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiFindProductInfoListQueryResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiUploadImageResult apiUploadImage(ApiUploadImageParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.uploadImage";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiUploadImageResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiListGroupResult apiListGroup(ApiListGroupParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.listGroup";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiListGroupResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiListImagePaginationResult apiListImagePagination(ApiListImagePaginationParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.listImagePagination";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiListImagePaginationResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiUploadTempImageResult apiUploadTempImage(ApiUploadTempImageParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=false;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.uploadTempImage";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiUploadTempImageResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiCalculateFreightResult apiCalculateFreight(ApiCalculateFreightParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.calculateFreight";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiCalculateFreightResult>(request, reqPolicy);
        }
                    
                       
           
        public ApiPostAeProductResult apiPostAeProduct(ApiPostAeProductParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "api.postAeProduct";
            apiId.NamespaceValue = "aliexpress.open";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ApiPostAeProductResult>(request, reqPolicy);
        }
           }
}