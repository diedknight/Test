using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class MembershipInfoController
    {
        public static aspnet_MembershipInfo GetUserInfo(string userID, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return aspnet_MembershipInfo.SingleOrDefault(info => info.UserID == userID);
            }
        }

        public static aspnet_MembershipInfo GetUserInfo(int type, string email, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return aspnet_MembershipInfo.SingleOrDefault(info => info.MembershipType == type && info.OtherEmail == email);
            }
        }

        public static void InsertMembershipInfo(string userKey, string firstname, string lastname, int retailerID, int type, string otherEmail, DateTime? birthday, string location, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
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
}