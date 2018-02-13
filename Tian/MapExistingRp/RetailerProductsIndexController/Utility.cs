using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Text;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

/// <summary>
/// Summary description for Utility
/// </summary>
/// 
namespace RetailerProductsIndexController
{
    public static class Utility
    {
        public static int DeleteAllChar(string str)
        {
            string temp = "";
            string number = "";
            for (int i = 0; i < str.Length; i++)
            {
                temp = str.Substring(i, 1);
                if (Encoding.ASCII.GetBytes(temp)[0] >= 48 && Encoding.ASCII.GetBytes(temp)[0] <= 57)
                {
                    number += temp;
                }
            }
            if (number == "")
            {
                return 0;
            }
            else
            {
                return int.Parse(number);
            }
        }

        public static List<string> AnalyseKeywords(string keywords)
        {
            List<string> list = new List<string>();

            string[] tmp = keywords.Split(new string[] { " ", ",", ";", "prices", "price", "buy", "cheap", "compare", "-" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in tmp)
            {
                if (s.Length > 0)
                {
                    list.Add(s.Replace("-", " ").ToLower());
                }
            }
            return list;
        }

        public static string GetString(string str, int length)
        {
            if (str.Length > length)
            {
                return str.Substring(0, length - 3) + "...";
            }

            return str;
        }

        static Hashtable categoryTotalClicks = new Hashtable();
  
        public static string FilterIllegalChar(string sourceString)
        {
            string sOut = "";
            string[] illegalChar ={ "/", "\\", "&", "?", " ", "<br/>", ":", "-", ",", ";", "'", "\"" };
            string[] result = sourceString.Split(illegalChar, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in result)
            {
                sOut += s + "-";
            }
            if (sOut.Length > 0) sOut = sOut.Substring(0, sOut.Length - 1);
            return sOut;
        }

        public static string GetParamFromQueryString(string queryString, string paramName)
        {
            string sOut = "";
            string paraStr = paramName + "=";

            if (queryString.IndexOf(paraStr) != -1)
            {
                int startIndex = queryString.IndexOf(paraStr);
                int endIndex = queryString.IndexOf("&", queryString.IndexOf(paraStr));

                if (endIndex >= 0)
                {
                    sOut = queryString.Substring(startIndex, endIndex - startIndex);
                }
                else
                {
                    sOut = queryString.Substring(startIndex);
                }

                sOut = sOut.Substring(sOut.IndexOf("=") + 1);
            }
            return sOut;
        }

        public static string GetSiteRoot()
        {
            //string Port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            //if (Port == null || Port == "80" || Port == "443")
            //    Port = "";
            //else
            //    Port = ":" + Port;

            //string Protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            //if (Protocol == null || Protocol == "0")
            //    Protocol = "http://";
            //else
            //    Protocol = "https://";

            //string appPath = System.Web.HttpContext.Current.Request.ApplicationPath;
            //if (appPath == "/")
            //    appPath = "";

            //string sOut = Protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
            return "http://www.priceme.co.nz";
        }

        public static string RemoveUrlParam(string queryString, string paramName)
        {
            string paraStr = paramName + "=";

            if (queryString.IndexOf(paraStr) != -1)
            {
                int startIndex = queryString.IndexOf(paraStr);
                int endIndex = queryString.IndexOf("&", queryString.IndexOf(paraStr));

                if (endIndex >= 0)
                {
                    queryString = queryString.Remove(startIndex, endIndex - startIndex + 1);
                }
                else
                {
                    queryString = queryString.Remove(startIndex);
                }

                //if queryString's last char is '&' then remove '&'
                if (queryString.EndsWith("&"))
                {
                    queryString = queryString.Remove(queryString.Length - 1);
                }
            }
            return queryString;
        }

        public static double GetAverageRating(int productRatingSum, int productRatingVotes, int productid)
        {
            double returnFloat = 0.0;
            if (productRatingVotes > 1 )
            {
                productRatingSum = productRatingSum - 3;
                productRatingVotes = productRatingVotes - 1;
                returnFloat = (productRatingSum * 1.0d ) / (productRatingVotes * 1.0d);
                returnFloat = double.Parse(returnFloat.ToString("0.0"));
            }
            return returnFloat;
        }

    }
}