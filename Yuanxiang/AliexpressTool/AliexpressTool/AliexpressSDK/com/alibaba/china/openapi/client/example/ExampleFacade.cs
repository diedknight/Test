using com.alibaba.openapi.client;
using com.alibaba.openapi.client.entity;
using com.alibaba.openapi.client.policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.alibaba.china.openapi.client.example.param.apiexample;

namespace com.alibaba.china.openapi.client.example
{

    class ExampleFacade
    {
        private ClientPolicy clientPolicy;

        public ExampleFacade(ClientPolicy clientPolicy)
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

                    
                       
           
        public ExampleFamilyGetResult exampleFamilyGet(ExampleFamilyGetParam param) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=false;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=false;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "example.family.get";
            apiId.NamespaceValue = "api.example";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
                        
            return this.getAPIClient().send<ExampleFamilyGetResult>(request, reqPolicy);
        }
                    
                       
           
        public ExampleFamilyPostResult exampleFamilyPost(ExampleFamilyPostParam param,  string accessToken) {
            RequestPolicy reqPolicy = new RequestPolicy();
            reqPolicy.HttpMethod="POST";
            reqPolicy.NeedAuthorization=true;
            reqPolicy.RequestSendTimestamp=false;
            reqPolicy.UseHttps=false;
            reqPolicy.UseSignture=true;
            reqPolicy.AccessPrivateApi=false;
           
            Request request = new Request ();
			APIId apiId = new APIId();
            apiId.Name = "example.family.post";
            apiId.NamespaceValue = "api.example";
            apiId.Version = 1;
			request.ApiId = apiId;
                
            request.RequestEntity=param;            
            request.AccessToken=accessToken;            
            return this.getAPIClient().send<ExampleFamilyPostResult>(request, reqPolicy);
        }
           }
}