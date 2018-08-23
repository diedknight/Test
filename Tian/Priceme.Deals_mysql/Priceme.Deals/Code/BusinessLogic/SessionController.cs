using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// Summary description for SessionController
/// </summary>
namespace PriceMe {
    public class SessionController {
        public static int sesscount = 0;

        public static HttpSessionState Session = HttpContext.Current.Session;

        public static object Get(SessionKey key) {
            string _key = key.ToString();
            return Session[_key];
        }

        public static void Set( SessionKey key, object value ) {
            string _key = key.ToString();
            if (Session[_key] != null)
                Session[_key] = value;
            else
                Session.Add(_key, value);
        }

        public static void Remove(SessionKey key) {
            Session.Remove(key.ToString());
        }
    }
}