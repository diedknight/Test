using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;

namespace PriceMeCommon
{
    /// <summary>
    /// MembershipInfoController 的摘要说明
    /// </summary>
    public class MembershipInfoController
    {
        public MembershipInfoController()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static aspnet_MembershipInfo GetUserInfo(string userID)
        {
            return aspnet_MembershipInfo.SingleOrDefault(info => info.UserID == userID);
        }

        public static aspnet_MembershipInfo GetUserInfo(int type, string email)
        {
            return aspnet_MembershipInfo.SingleOrDefault(info => info.MembershipType == type && info.OtherEmail == email);
        }

        public static void InsertMembershipInfo(string userKey, string firstname, string lastname, int retailerID, int type, string otherEmail, DateTime? birthday, string location)
        {
            aspnet_MembershipInfo membershipInfo = new aspnet_MembershipInfo();
            membershipInfo.UserID = userKey;
            membershipInfo.FirstName = firstname;
            membershipInfo.LastName = lastname;
            membershipInfo.RetailerID = retailerID;
            membershipInfo.MembershipType = type;
            membershipInfo.OtherEmail = otherEmail;
            if (birthday != null)
            {
                birthday = birthday.Value.ToLocalTime();
            }
            membershipInfo.Birthday = birthday;
            membershipInfo.Address = location;
            membershipInfo.Save();
        }
    }
}