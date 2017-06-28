using com.alibaba.openapi.client.policy;
using com.alibaba.openapi.client.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using com.alibaba.openapi.client.serialize;
using com.alibaba.openapi.client.entity;
using System.Collections;
using System.Web;

namespace com.alibaba.openapi.client.http
{
    class HttpClient
    {
        private ClientPolicy clientPolicy;

        public HttpClient(ClientPolicy clientPolicy)
        {
            this.clientPolicy = clientPolicy;
        }

        public T request<T>(Request request, RequestPolicy requestPolicy)
        {
            StringBuilder path = createProtocolRequestPath(requestPolicy, request);
            Dictionary<String, Object> parameters = createParameterDictionary(requestPolicy, request);
            StringBuilder queryBuilder = new StringBuilder();
            signature(path.ToString(), parameters, requestPolicy, clientPolicy);
            if ("GET".Equals(requestPolicy.HttpMethod))
            {

                String queryString = createParameterStr(parameters);
                String uriStr = buildRequestUri(requestPolicy, request);
                uriStr = uriStr + "?" + queryString;
                Uri uri = new Uri(uriStr);
                HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

                httpWebRequest.Method = "GET";
                httpWebRequest.KeepAlive = false;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.UserAgent = "Ocean/NET-SDKClient";

                HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();

                DeSerializer deSerializer = SerializerProvider.getInstance().getDeSerializer(requestPolicy.ResponseProtocol);
                ResponseWrapper rw = deSerializer.deSerialize(responseStream, typeof(T), Encoding.UTF8.EncodingName);
                return (T)rw.Result;
            }
            else
            {
                String postString = createParameterStr(parameters);
                byte[] postData = Encoding.UTF8.GetBytes(postString);
                String uriStr = buildRequestUri(requestPolicy, request);
                Uri uri = new Uri(uriStr);
                HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

                httpWebRequest.Method = "POST";
                httpWebRequest.KeepAlive = false;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.UserAgent = "Ocean/NET-SDKClient";
                httpWebRequest.ContentLength = postData.Length;

                System.IO.Stream outputStream = httpWebRequest.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();
                try
                {
                    HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();

                    
                    DeSerializer deSerializer = SerializerProvider.getInstance().getDeSerializer(requestPolicy.ResponseProtocol);
                    ResponseWrapper rw = deSerializer.deSerialize(responseStream, typeof(T), Encoding.UTF8.EncodingName);
                    return (T)rw.Result;
                }
                catch (System.Net.WebException webException)
                {
                    HttpWebResponse response = webException.Response as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();
                    DeSerializer deSerializer = SerializerProvider.getInstance().getDeSerializer(requestPolicy.ResponseProtocol);
                    Exception rw = deSerializer.buildException(responseStream,500, Encoding.UTF8.EncodingName);
                    throw rw;
                }
            }
        }

        private String buildRequestUri(RequestPolicy requestPolicy, Request request)
        {
            String schema = "http";
            int port = clientPolicy.HttpPort;
            if (requestPolicy.UseHttps)
            {
                schema = "https";
                port = clientPolicy.HttpsPort;
            }
            StringBuilder relativeBuilder = new StringBuilder(schema);
            relativeBuilder.Append("://");
            relativeBuilder.Append(clientPolicy.ServerHost);
            if (port != 80 && port != 443)
            {
                relativeBuilder.Append(":");
                relativeBuilder.Append(port);
            }

            if (requestPolicy.AccessPrivateApi)
            {
                relativeBuilder.Append("/api");
            }
            else
            {
                relativeBuilder.Append("/openapi");
            }

            relativeBuilder.Append("/");
            relativeBuilder.Append(createProtocolRequestPath(requestPolicy, request));
            return relativeBuilder.ToString();
        }

        private StringBuilder createProtocolRequestPath(RequestPolicy requestPolicy, Request request)
        {

            StringBuilder relativeBuilder = new StringBuilder();

            relativeBuilder.Append(requestPolicy.RequestProtocol);
            relativeBuilder.Append("/").Append(request.ApiId.Version);
            relativeBuilder.Append("/").Append(request.ApiId.NamespaceValue);
            relativeBuilder.Append("/").Append(request.ApiId.Name);
            relativeBuilder.Append("/").Append(clientPolicy.AppKey);
            return relativeBuilder;

        }

        private String createParameterStr(Dictionary<String, Object> parameters)
        {
            StringBuilder paramBuilder = new StringBuilder();
            foreach (KeyValuePair<string, object> kvp in parameters)
            {
                String encodedValue = null;
                if (kvp.Value != null)
                {
                    String tempValue = kvp.Value.ToString();
                    byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(tempValue);
                    encodedValue = HttpUtility.UrlEncode(byteArray, 0, byteArray.Length);   
                }
                paramBuilder.Append(kvp.Key).Append("=").Append(encodedValue);
                paramBuilder.Append("&");
            }
            return paramBuilder.ToString();
        }

        private Dictionary<String, Object> createParameterDictionary(RequestPolicy requestPolicy, Request request)
        {

            Serializer serializer = SerializerProvider.getInstance().getSerializer(requestPolicy.RequestProtocol);

            Dictionary<String, Object> parameters = serializer.serialize(request.RequestEntity);
            if (!requestPolicy.RequestProtocol.Equals(requestPolicy.ResponseProtocol))
            {
                parameters.Add("_aop_responseFormat", requestPolicy.ResponseProtocol);
            }
            if (requestPolicy.RequestSendTimestamp)
            {
                parameters.Add("_aop_timestamp", DateUtil.currentTimeMillis());
            }
            parameters.Add("_aop_datePattern", DateUtil.getDatePattern());
            foreach (KeyValuePair<string, object> kvp in request.AddtionalParams)
            {
                parameters.Add(kvp.Key, kvp.Value);
            }
            if (request.AccessToken != null)
            {
                parameters.Add("access_token", request.AccessToken);
            }
            return parameters;
        }

        private void signature(String path, Dictionary<String, Object> parameters, RequestPolicy requestPolicy, ClientPolicy clientPolicy)
        {
            if (!requestPolicy.UseSignture)
            {
                return;
            }
            if (clientPolicy.AppKey == null
                    || clientPolicy.SecretKey == null)
            {
                return;
            }
            byte[] sign = SignatureUtil.hmacSha1(path, parameters, clientPolicy.SecretKey);
            String signStr = SignatureUtil.toHex(sign);
            if (signStr != null)
            {
                parameters.Add("_aop_signature", signStr);
            }
        }


        private WebClient createWebClient()
        {

            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Ocean/SDK Client");

            return client;
        }
    }
}
