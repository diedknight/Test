using FinanceDBA;
using SubSonic.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace FinanceSiteMap
{
    public static class FinancePage
    {
        public static string WebSiteRootUrl_Static = System.Configuration.ConfigurationManager.AppSettings["WebSiteRootUrl"];

        private static Dictionary<int, string> proDic = new Dictionary<int, string>();

        private static Regex illegalReg = new Regex("[^a-z0-9-]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);

        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        public static List<string> GetCategoryProviderInfo()
        {
            List<string> infoList = new List<string>();
            Dictionary<string, string> infoDic = GetAllCreditCardProviderInfo();
            foreach (KeyValuePair<string, string> pair in infoDic)
            {
                string url = GetCategoryProviderInfoUrl(pair.Key, pair.Value, "credit-cards");
                infoList.Add(url);
            }
            infoDic = GetAllSavingsAccountProviderInfo();
            foreach (KeyValuePair<string, string> pair in infoDic)
            {
                string url = GetCategoryProviderInfoUrl(pair.Key, pair.Value, "savings-accounts");
                infoList.Add(url);
            }
            infoDic = GetAllTermDepositProviderInfo();
            foreach (KeyValuePair<string, string> pair in infoDic)
            {
                string url = GetCategoryProviderInfoUrl(pair.Key, pair.Value, "term-deposits");
                infoList.Add(url);
            }
            infoDic = GetAllHomeLoanProviderInfo();
            foreach (KeyValuePair<string, string> pair in infoDic)
            {
                string url = GetCategoryProviderInfoUrl(pair.Key, pair.Value, "home-loan");
                infoList.Add(url);
            }
            return infoList;
        }

        private static Dictionary<string, string> GetAllCreditCardProviderInfo()
        {
            Dictionary<string, string> ccDic = new Dictionary<string, string>();
            Select select = new Select();
            select.From("vw_AllProviderInfo");
            Select subSelect = new Select();
            subSelect.From("CSK_Store_CreditCard");
            subSelect.SelectColumnList = new string[]
            {
                "ProviderId"
            };
            select.Where("ProviderID").In(subSelect).And("Status").IsEqualTo(true);
            IDataReader idr = select.ExecuteReader();
            while (idr.Read())
            {
                string ProviderId = idr["ProviderId"].ToString();
                string ProviderName = idr["Name"].ToString();
                if (!ccDic.ContainsKey(ProviderId))
                {
                    ccDic.Add(ProviderId, ProviderName);
                }
            }
            idr.Close();
            return ccDic;
        }

        private static Dictionary<string, string> GetAllSavingsAccountProviderInfo()
        {
            Dictionary<string, string> saDic = new Dictionary<string, string>();
            Select select = new Select();
            select.From("vw_AllProviderInfo");
            Select subSelect = new Select();
            subSelect.From("CSK_Store_SavingsAccount");
            subSelect.SelectColumnList = new string[]
            {
                "ProviderId"
            };
            select.Where("ProviderID").In(subSelect).And("Status").IsEqualTo(true);
            IDataReader idr = select.ExecuteReader();
            while (idr.Read())
            {
                string ProviderId = idr["ProviderId"].ToString();
                string ProviderName = idr["Name"].ToString();
                if (!saDic.ContainsKey(ProviderId))
                {
                    saDic.Add(ProviderId, ProviderName);
                }
            }
            idr.Close();
            return saDic;
        }

        private static Dictionary<string, string> GetAllTermDepositProviderInfo()
        {
            Dictionary<string, string> tdDic = new Dictionary<string, string>();
            Select select = new Select();
            select.From("vw_AllProviderInfo");
            Select subSelect = new Select();
            subSelect.From("CSK_Store_TermDeposit");
            subSelect.SelectColumnList = new string[]
            {
                "ProviderId"
            };
            select.Where("ProviderID").In(subSelect).And("Status").IsEqualTo(true);
            IDataReader idr = select.ExecuteReader();
            while (idr.Read())
            {
                string ProviderId = idr["ProviderId"].ToString();
                string ProviderName = idr["Name"].ToString();
                if (!tdDic.ContainsKey(ProviderId))
                {
                    tdDic.Add(ProviderId, ProviderName);
                }
            }
            idr.Close();
            return tdDic;
        }

        private static Dictionary<string, string> GetAllHomeLoanProviderInfo()
        {
            Dictionary<string, string> hlDic = new Dictionary<string, string>();
            Select select = new Select();
            select.From("vw_AllProviderInfo");
            Select subSelect = new Select();
            subSelect.From("CSK_Store_HomeLoans");
            subSelect.SelectColumnList = new string[]
            {
                "ProviderId"
            };
            subSelect.Where("Status").IsEqualTo(true);
            select.Where("ProviderID").In(subSelect).And("Status").IsEqualTo(true);
            IDataReader idr = select.ExecuteReader();
            while (idr.Read())
            {
                string ProviderId = idr["ProviderId"].ToString();
                string ProviderName = idr["Name"].ToString();
                if (!hlDic.ContainsKey(ProviderId))
                {
                    hlDic.Add(ProviderId, ProviderName);
                }
            }
            idr.Close();
            return hlDic;
        }

        private static string GetCategoryProviderInfoUrl(string pid, string pname, string key)
        {
            return string.Concat(new string[]
            {
                WebSiteRootUrl_Static,
                FilterInvalidUrlPathChar(pname).ToLower(),
                "-",
                key,
                "_",
                pid
            });
        }

        public static void GetSavingsAccounts(List<string> productList)
        {
            List<CSK_Store_SavingsAccount> sas = (from s in Finance.FinanceDB.CSK_Store_SavingsAccounts
                                                  where s.Status == true
                                                  select s).ToList<CSK_Store_SavingsAccount>();
            foreach (CSK_Store_SavingsAccount sa in sas)
            {
                if (proDic.ContainsKey(sa.ProviderId))
                {
                    string pname = GetProviderName(sa.ProviderId);
                    string url = GetSavingsAccountProductUrl(sa.SavingsAccountID.ToString(), pname, sa.SavingAccountName, WebSiteRootUrl_Static);
                    productList.Add(url);
                }
            }
        }

        public static void GetCreditCards(List<string> productList)
        {
            List<CSK_Store_CreditCard> ccs = (from c in Finance.FinanceDB.CSK_Store_CreditCards
                                              where c.Status == true
                                              select c).ToList<CSK_Store_CreditCard>();
            foreach (CSK_Store_CreditCard cc in ccs)
            {
                if (proDic.ContainsKey(cc.ProviderId ?? 0))
                {
                    string pname = GetProviderName(cc.ProviderId ?? 0);
                    string url = GetCreditCardsProductUrl(cc.ID.ToString(), pname, cc.Name, WebSiteRootUrl_Static);
                    productList.Add(url);
                }
            }
        }

        public static void GetTermDeposits(List<string> productList)
        {
            List<CSK_Store_TermDeposit> tds = (from t in Finance.FinanceDB.CSK_Store_TermDeposits
                                               where t.Status == (bool?)true
                                               select t).ToList<CSK_Store_TermDeposit>();
            foreach (CSK_Store_TermDeposit td in tds)
            {
                if (proDic.ContainsKey(td.ProviderId ?? 0))
                {
                    string pname = GetProviderName(td.ProviderId ?? 0);
                    string url = GetTermDepositProductUrl(td.TermDepositId.ToString(), pname, td.TermDepositName, WebSiteRootUrl_Static);
                    productList.Add(url);
                }
            }
        }

        public static void GetHomeLoans(List<string> productList)
        {
            List<CSK_Store_HomeLoan> hls = (from h in Finance.FinanceDB.CSK_Store_HomeLoans
                                            where h.Status == true
                                            select h).ToList<CSK_Store_HomeLoan>();
            foreach (CSK_Store_HomeLoan hl in hls)
            {
                if (proDic.ContainsKey(hl.ProviderId))
                {
                    string pname = GetProviderName(hl.ProviderId);
                    string url = GetHomeLoanProductUrl(pname, hl.HomeLoanName, hl.Id.ToString(), WebSiteRootUrl_Static);
                    productList.Add(url);
                }
            }
        }

        public static string GetCreditCardsProductUrl(string ccid, string providerName, string cardName, string appPath)
        {
            return GetCreditCardsProductUrl(new Dictionary<string, string>
            {
                {
                    "ccid",
                    ccid
                },
                {
                    "pname",
                    providerName
                },
                {
                    "cname",
                    cardName
                }
            }, appPath);
        }

        public static string GetCreditCardsProductUrl(Dictionary<string, string> ps, string appPath)
        {
            string url = string.Empty;
            if (!appPath.EndsWith("/"))
            {
                appPath += "/";
            }
            if (ps != null && ps.Count > 0)
            {
                if (ps.ContainsKey("pname"))
                {
                    url = appPath + FilterInvalidUrlPathChar(ps["pname"]);
                }
                if (ps.ContainsKey("cname"))
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        url = url + "-" + FilterInvalidUrlPathChar(ps["cname"]);
                    }
                    else
                    {
                        url = appPath + FilterInvalidUrlPathChar(ps["cname"]);
                    }
                }
                if (ps.ContainsKey("ccid"))
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        url = url + "_cc-" + FilterInvalidUrlPathChar(ps["ccid"]);
                    }
                    else
                    {
                        url = appPath + "_cc-" + FilterInvalidUrlPathChar(ps["ccid"]);
                    }
                }
            }
            return url.ToLower();
        }

        public static string GetSavingsAccountProductUrl(string said, string providerName, string saname, string appPath)
        {
            return GetSavingsAccountProductUrl(new Dictionary<string, string>
            {
                {
                    "said",
                    said
                },
                {
                    "pname",
                    providerName
                },
                {
                    "saname",
                    saname
                }
            }, appPath);
        }

        public static string GetSavingsAccountProductUrl(Dictionary<string, string> ps, string appPath)
        {
            string url = string.Empty;
            if (!appPath.EndsWith("/"))
            {
                appPath += "/";
            }
            if (ps != null && ps.Count > 0)
            {
                if (ps.ContainsKey("pname"))
                {
                    url = appPath + FilterInvalidUrlPathChar(ps["pname"]);
                }
                if (ps.ContainsKey("saname"))
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        url = url + "-" + FilterInvalidUrlPathChar(ps["saname"]);
                    }
                    else
                    {
                        url = appPath + FilterInvalidUrlPathChar(ps["saname"]);
                    }
                }
                if (ps.ContainsKey("said"))
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        url = url + "_sa-" + FilterInvalidUrlPathChar(ps["said"]);
                    }
                    else
                    {
                        url = appPath + "_sa-" + FilterInvalidUrlPathChar(ps["said"]);
                    }
                }
            }
            return url.ToLower();
        }

        public static string GetTermDepositProductUrl(string tdid, string providerName, string tdname, string appPath)
        {
            return GetTermDepositProductUrl(new Dictionary<string, string>
            {
                {
                    "tdid",
                    tdid
                },
                {
                    "pname",
                    providerName
                },
                {
                    "tdname",
                    tdname
                }
            }, appPath);
        }

        public static string GetTermDepositProductUrl(Dictionary<string, string> ps, string appPath)
        {
            string url = string.Empty;
            if (!appPath.EndsWith("/"))
            {
                appPath += "/";
            }
            if (ps != null && ps.Count > 0)
            {
                if (ps.ContainsKey("pname"))
                {
                    url = appPath + FilterInvalidUrlPathChar(ps["pname"]);
                }
                if (ps.ContainsKey("tdname"))
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        url = url + "-" + FilterInvalidUrlPathChar(ps["tdname"]);
                    }
                    else
                    {
                        url = appPath + FilterInvalidUrlPathChar(ps["tdname"]);
                    }
                }
                if (ps.ContainsKey("tdid"))
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        url = url + "_td-" + FilterInvalidUrlPathChar(ps["tdid"]);
                    }
                    else
                    {
                        url = appPath + "_td-" + FilterInvalidUrlPathChar(ps["tdid"]);
                    }
                }
            }
            return url.ToLower();
        }

        public static string GetHomeLoanProductUrl(string providerName, string productName, string oid, string appPath)
        {
            string page = FilterInvalidUrlPathChar(providerName + "-" + productName) + "_hl-" + oid;
            return appPath + page;
        }

        public static void GetProviders(List<string> productList)
        {
            List<CSK_Store_Provider> pros = (from p in Finance.FinanceDB.CSK_Store_Providers
                                             where p.Status == (bool?)true
                                             select p).ToList<CSK_Store_Provider>();
            foreach (CSK_Store_Provider pro in pros)
            {
                if (!proDic.ContainsKey(pro.ProviderID))
                {
                    proDic.Add(pro.ProviderID, pro.Name);
                }
                string url = GetProviderUrl(pro.ProviderID.ToString(), pro.Name);
                productList.Add(url);
            }
        }

        private static string GetProviderName(int pid)
        {
            string result;
            if (proDic.ContainsKey(pid))
            {
                result = proDic[pid];
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

        private static string GetProviderUrl(string pid, string pname)
        {
            return WebSiteRootUrl_Static + FilterInvalidUrlPathChar(pname).ToLower() + "_prd-" + pid;
        }

        public static string FilterInvalidUrlPathChar(string sourceString)
        {
            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");
            sourceString = illegalReg3.Replace(sourceString, "");
            return sourceString;
        }
    }
}