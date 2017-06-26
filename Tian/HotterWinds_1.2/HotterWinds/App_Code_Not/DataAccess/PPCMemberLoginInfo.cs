using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commerce.Common
{
    /// <summary>
    /// Summary description for PPCMemberLoginInfo
    /// </summary>
    public class PPCMemberLoginInfo : IComparable<PPCMemberLoginInfo>
    {
        int retailerID;
        string retailerName;
        int ppcMemberID;
        string userID;
        string userName;
        string password;
        string eMail;

        public int RetailerID
        {
            get { return retailerID; }
            set { retailerID = value; }
        }

        public string RetailerName
        {
            get { return retailerName; }
            set { retailerName = value; }
        }

        public int PPCMemberID
        {
            get { return ppcMemberID; }
            set { ppcMemberID = value; }
        }

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string EMail
        {
            get { return eMail; }
            set { eMail = value; }
        }

        #region IComparable<PPCMemberLoginInfo> Members

        public int CompareTo(PPCMemberLoginInfo other)
        {
            return this.RetailerName.CompareTo(other.RetailerName);
        }

        #endregion
    }
}