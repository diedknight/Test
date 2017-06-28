using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.entity
{
    [DataContract(Namespace = "com.alibaba.openapi.client")]
    class AuthorizationToken
    {
        [DataMember(Order = 0)]
        private String access_token;
        [DataMember(Order = 1)]
        private String refresh_token;
        [DataMember(Order = 2)]
        private long expires_in;
        [DataMember(Order = 3)]
        private DateTime expires_time;
        [DataMember(Order = 4)]
        private DateTime refresh_token_timeout;
        [DataMember(Order = 5)]
        private String resource_owner;
        [DataMember(Order = 6)]
        private String uid;
        [DataMember(Order = 7)]
        private long aliId;
        [DataMember(Order = 8)]
        private String memberId;

        /**
       * 获取access_token
       * 
       * @return the accessToken
       */
        public String getAccess_token()
        {
            return access_token;
        }

        /**
         * 获取access_token过期时间
         * 
         * @return the accessTokenTimeout
         */
        public long getExpires_in()
        {
            return expires_in;
        }

        /**
         * 获取refresh_token
         * 
         * @return the refreshToken
         */
        public String getRefresh_token()
        {
            return refresh_token;
        }

        /**
         * 获取refresh_token过期时间
         * 
         * @return the refreshTokenTimeout
         */
        public DateTime getRefresh_token_timeout()
        {
            return refresh_token_timeout;
        }

        public String getMemberId()
        {
            return memberId;
        }

        public void setMemberId(String memberId)
        {
            this.memberId = memberId;
        }

        /**
         * 获取resource_owner
         * 
         * @return the resourceOwnerId
         */
        public String getResource_owner()
        {
            return resource_owner;
        }

        /**
         * 获取uid
         * 
         * @return the uid
         */
        public String getUid()
        {
            return uid;
        }

        /**
         * 获取aliId
         * 
         * @return the aliId
         */
        public long getAliId()
        {
            return aliId;
        }

        public void setAccess_token(String accessToken)
        {
            this.access_token = accessToken;
        }

        public void setRefresh_token(String refreshToken)
        {
            this.refresh_token = refreshToken;
        }

        public void setExpires_in(long accessTokenTimeout)
        {
            this.expires_in = accessTokenTimeout;
            DateTime now = new DateTime();
            this.expires_time = now.AddSeconds(accessTokenTimeout);
        }

        public void setRefresh_token_timeout(DateTime refresh_token_timeout)
        {
            this.refresh_token_timeout = refresh_token_timeout;
        }

        public void setResource_owner(String resourceOwnerId)
        {
            this.resource_owner = resourceOwnerId;
        }

        public void setUid(String uid)
        {
            this.uid = uid;
        }

        public void setAliId(long aliId)
        {
            this.aliId = aliId;
        }

        /**
         * 获取access_token过期时间,Date格式
         * 
         * @return the accessTokenTimeout
         */
        public DateTime getExpires_time()
        {
            return expires_time;
        }
    }
}
