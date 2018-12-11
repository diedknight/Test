using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public static string GetProductUrl(string pid, string pname)
        {
            return string.Format("{0}/p-{1}.aspx", FilterInvalidNameChar(pname), pid);
        }

        private static Regex illegalReg = new Regex(@"[^a-z0-9-+]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg1 = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        public static string FilterInvalidNameChar(string name)
        {
            name = illegalReg1.Replace(name, "-");
            name = illegalReg2.Replace(name, "-");
            name = illegalReg3.Replace(name, "");

            return name;
        }

        public static string GetNewRating(string srating)
        {
            string stringrating = string.Empty;
            double rating = 0;
            double.TryParse(srating, out rating);

            for (int i = 1; i < 6; i++)
            {
                string clsname = string.Empty;
                if (rating >= i)
                    clsname = "glyphicon-star";
                else if (rating > (double)(i - 1) && rating < (double)i)
                    clsname = "glyphicon-star half";
                else if (rating < 1 && rating > 0 && i == 1)
                    clsname = "glyphicon-star half";
                else if (rating < (double)i)
                    clsname = "glyphicon-star empty";
                else
                    clsname = "glyphicon-star empty";

                stringrating += "<i class=\"glyphicon " + clsname + "\"></i>";
            }

            return stringrating;
        }
    }
}
