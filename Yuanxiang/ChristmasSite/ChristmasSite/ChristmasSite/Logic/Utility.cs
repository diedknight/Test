using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChristmasSite.Logic
{
    public static class Utility
    {
        public static string GetParameter(string sParam, HttpRequest request)
        {
            StringValues sv;
            request.Query.TryGetValue(sParam, out sv);
            return HttpUtility.UrlDecode(sv.ToString());
        }


        public static int GetIntParameter(string sParam, HttpRequest request)
        {
            int iOut = 0;

            string sOut = GetParameter(sParam, request);
            if (!String.IsNullOrEmpty(sOut))
            {
                if (sOut.Contains(','))
                {
                    int.TryParse(sOut.Split(',')[0], out iOut);
                }
                else
                {
                    int.TryParse(sOut, out iOut);
                }
            }

            return iOut;
        }
    }
}
