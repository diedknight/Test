using com.alibaba.openapi.client.entity;
using com.alibaba.openapi.client.http;
using com.alibaba.openapi.client.policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client
{
    class SyncAPIClient
    {
        private ClientPolicy clientPolicy;

        public SyncAPIClient(ClientPolicy clientPolicy)
        {
            this.clientPolicy = clientPolicy;
        }

        public T send<T>(Request request, RequestPolicy policy)
        {
            HttpClient httpClient = new HttpClient(clientPolicy);
            T result = httpClient.request<T>(request, policy);
            return result;
        }

        public AuthorizationToken getToken(String code)
        {

            APIId apiId = new APIId();
            apiId.Name = "getToken";
            apiId.NamespaceValue = "system.oauth2";
            apiId.Version = 1;

            Request request = new Request();
            request.ApiId = apiId;

            request.AddtionalParams["code"] = code;
            request.AddtionalParams["grant_type"] = "authorization_code";
            request.AddtionalParams["need_refresh_token"] = true;
            request.AddtionalParams["client_id"] = clientPolicy.AppKey;
            request.AddtionalParams["client_secret"] = clientPolicy.SecretKey;
            request.AddtionalParams["redirect_uri"] = "default";
            RequestPolicy oauthPolicy = new RequestPolicy();
            oauthPolicy.UseHttps = true;

            return this.send<AuthorizationToken>(request, oauthPolicy);
        }

        public AuthorizationToken refreshToken(String refreshToken)
        {

            APIId apiId = new APIId();
            apiId.Name = "getToken";
            apiId.NamespaceValue = "system.oauth2";
            apiId.Version = 1;
        
            Request request = new Request();
            request.ApiId = apiId;

            request.AddtionalParams["refreshToken"] = refreshToken;
            request.AddtionalParams["grant_type"] = "refresh_token";
            request.AddtionalParams["client_id"] = clientPolicy.AppKey;
            request.AddtionalParams["client_secret"] = clientPolicy.SecretKey;
            request.AddtionalParams["redirect_uri"] = "default";
            RequestPolicy oauthPolicy = new RequestPolicy();
            oauthPolicy.HttpMethod = "GET";
            oauthPolicy.UseHttps = true;
            return this.send<AuthorizationToken>(request, oauthPolicy);


        }
    }
}

