using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductsForPopularSearch
{
    class Utility
    {
        public static string FixKeywords(string kw)
        {
            string[] pNKws = kw.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string newPN = "";
            foreach (string pnKW in pNKws)
            {
                newPN += pnKW + " ";
                //让包含 ' 号的关键字不用 ' 也能搜索到
                if (pnKW.Contains("'"))
                {
                    newPN += pnKW.Replace("'", "") + " ";
                }
                //让包含 - 号的关键字不用 - 也能搜索到
                if (pnKW.Contains("-"))
                {
                    newPN += pnKW.Replace("-", "") + " ";
                    newPN += pnKW.Replace("-", " ") + " ";
                }
            }
            return newPN;
        }
        //static System.Text.RegularExpressions.Regex seachKeywordsRegex = new System.Text.RegularExpressions.Regex(@"(?<kwGroup>[^\s]+-[^\s]+)");
        public static string GetKeywords(string categoryName, string manufacturerName, string keywords, string productName, string otherKeywords)
        {
            string pN = productName.Replace("&", " ").Replace(",", " ").Replace("_", " ").ToLower();
            pN = FixKeywords(pN);
            //System.Text.RegularExpressions.MatchCollection matches = seachKeywordsRegex.Matches(productName);
            //if (matches.Count > 0)
            //{
            //    string newKw = "";
            //    foreach (System.Text.RegularExpressions.Match m in matches)
            //    {
            //        newKw += m.Groups["kwGroup"].Value.Replace("-", "") + " ";
            //    }
            //    pN = pN + " " + newKw.Trim().ToLower();
            //}

            string[] mN = manufacturerName.Replace("&", " ").ToLower().Split(' ');
            string[] others = otherKeywords.Replace("&", " ").Replace(",", " ").Replace("-", " ").ToLower().Split(' ');
            string kw = keywords == null ? "" : keywords.ToLower().Replace("&", " ").Replace(",", " ").Replace(":", " ");

            categoryName = categoryName.ToLower();

            if (categoryName.Equals("Tail Pads", StringComparison.InvariantCultureIgnoreCase)
                || categoryName.Equals("Eyeglasses", StringComparison.InvariantCultureIgnoreCase))
            {
                kw += " " + categoryName;
            }
            else
            {
                string[] cNs = categoryName.Split(new char[] { '&', ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string cn in cNs)
                {
                    string[] subCN = categoryName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in subCN)
                    {
                        if (str.Equals("Accessories", StringComparison.InvariantCultureIgnoreCase)
                        || str.Equals("Product", StringComparison.InvariantCultureIgnoreCase)
                        || str.Equals("Products", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                        kw += " " + str;
                    }
                    string lastName = cNs[cNs.Length - 1];

                    if (lastName.Equals("Jeans", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Gas", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Glasses", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.EndsWith("ss")
                        || lastName.Equals("Accessories", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Product", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Products", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    else if (lastName.EndsWith("ies"))
                    {
                        kw += " " + lastName.Substring(0, lastName.Length - 3) + "y";
                    }
                    else
                    {
                        if (lastName.Equals("Toothbrushes", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Brushes", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Classes", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Watches", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Compasses", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Boxes", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Clothes", StringComparison.InvariantCultureIgnoreCase))
                        {
                            kw += " " + lastName.Substring(0, lastName.Length - 2);
                        }
                        else if (lastName.Equals("Sunglasses", StringComparison.InvariantCultureIgnoreCase))
                        {
                            kw += " sunglasses sun glasses";
                        }
                    }
                }
            }

            foreach (string str in mN)
            {
                if (mN.Equals("NA"))
                {
                    continue;
                }
                kw += " " + str;
            }

            foreach (string str in others)
            {
                kw += " " + str;
            }

            kw += " " + pN;

            return kw.Trim();
        }
    }
}
