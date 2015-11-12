﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SubSonic.Schema;
using System.Data;
using System.Text.RegularExpressions;

namespace MergeReportTool
{
    public class MergeReport
    {
        StreamWriter _sw;
        public StreamWriter SW
        {
            get { return _sw; }
            set { _sw = value; }
        }

        private List<int> listRids;
        private List<ReportKeywordData> listKeyword;
        private List<MergingOptionSettingData> listSetting;
        private List<string> listSpecial;
        private List<MappingNoModelData> listNoModel;
        private List<int> listCountry;
        private int CountryId;
        private decimal PriceRateJudge;

        public void Tools()
        {
            Write("Begin......" + DateTime.Now);
            
            string stringrids = ConfigurationManager.AppSettings["RetailerId"].ToString();
            string stringcids = ConfigurationManager.AppSettings["CategoryId"].ToString();
            int.TryParse(ConfigurationManager.AppSettings["CountryId"].ToString(), out CountryId);

            listCountry = new List<int>();
            string stringCountry = ConfigurationManager.AppSettings["CountryId"].ToString();
            string[] countrys = stringCountry.Split(',');
            foreach (string temp in countrys)
            {
                int cid = 0;
                int.TryParse(temp, out cid);
                listCountry.Add(cid);
            }

            foreach (int countryid in listCountry)
            {
                CountryId = countryid;
                Write("Get " + countryid + " Country......" + DateTime.Now);

                listRids = new List<int>();
                if (stringrids == "0")
                    GetAllRetailers();
                else
                {
                    string[] temps = stringrids.Split(',');
                    foreach (string temp in temps)
                    {
                        int rid = 0;
                        int.TryParse(temp, out rid);
                        listRids.Add(rid);
                    }
                }
                Write("Get " + listRids.Count + " Retailers......" + DateTime.Now);

                if (stringcids == "0")
                    stringcids = GetAllCategorys();

                decimal.TryParse(ConfigurationManager.AppSettings["PriceRateJudge"].ToString().Replace("%", ""), out PriceRateJudge);
                PriceRateJudge = decimal.Round(PriceRateJudge / 100, 2);

                GetAllMergeReportKeyword();
                GetMergingOptionSettings();
                GetSpecialCharacter();
                GetNoModel();


                foreach (int rid in listRids)
                {
                    Write("Check " + rid + " product......." + DateTime.Now);

                    List<ProductData> products = new List<ProductData>();
                    Write("Get all product by retailerid......." + DateTime.Now);
                    GetAllProductByRetailerId(rid, stringcids, products);

                    Write("Get " + products.Count + " product by retailerid......." + DateTime.Now);
                    foreach (ProductData data in products)
                    {
                        bool isError = CheckPrice(data);
                        if (!isError)
                            isError = CheckKeyword(data);
                        if (!isError)
                            CheckProductName(data);
                    }
                }

            }

            Write("End......" + DateTime.Now);
        }

        private bool CheckPrice(ProductData data)
        {
            bool isError = false;
            List<decimal> listPrice = new List<decimal>();
            string sql = "Select RetailerPrice From CSK_Store_RetailerProduct rp inner join CSK_Store_Retailer r "
                        + "On rp.RetailerId = r.RetailerId Where rp.ProductId = " + data.ProductId + " "
                        + "And rp.RetailerProductStatus = 1 And IsDeleted = 0 And r.RetailerCountry = " + CountryId;
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                decimal price = 0m;
                decimal.TryParse(dr["RetailerPrice"].ToString(), out price);
                listPrice.Add(price);
            }
            dr.Close();

            decimal total = 0;
            int count = 0;
            if (listPrice.Count < 2)
                return isError;
            else if (listPrice.Count == 2)
            {
                foreach (decimal price in listPrice)
                {
                    total += price;
                }
                count = 2;
            }
            else
            {
                listPrice.Sort();
                for (int i = 1; i < listPrice.Count - 1; i++)
                {
                    total += listPrice[i];
                }
                count = listPrice.Count - 2;
            }

            decimal avgPrice = decimal.Round(total / count, 2);
            if (avgPrice > 0)
            {
                decimal ratePrice = decimal.Round(Math.Abs(data.RetailerPrice - avgPrice) / avgPrice, 2);
                if (ratePrice > PriceRateJudge && data.RetailerProductCondition != 4)
                {
                    InsertMergeErrorReport(data.RetailerProductId, data.ProductId, string.Empty, string.Empty, ratePrice.ToString(), string.Empty);
                    isError = true;
                }
            }

            return isError;
        }

        private bool CheckKeyword(ProductData data)
        {
            bool isError = false;
            string stringError = string.Empty;
            List<ReportKeywordData> ks = listKeyword.Where(k => k.CategoryId == data.CategoryID).ToList();
            if (ks != null && ks.Count > 0)
            {
                foreach (ReportKeywordData k in ks)
                {
                    if (k.keywordType == 1)
                    {
                        #region keywordtype = 1
                        string pvalue = string.Empty;
                        string pkey = string.Empty;
                        bool isCheck = false;
                        string[] temps = k.Keyword.Split(',');
                        foreach (string temp in temps)
                        {
                            Regex reg = new Regex(@" (?<value>\d+)" + temp + " ", RegexOptions.IgnoreCase);
                            if (string.IsNullOrEmpty(pkey))
                            {
                                Match ma = reg.Match(data.ProductName);
                                if (ma.Success)
                                {
                                    pvalue = ma.Groups["value"].Value;
                                    pkey = temp;
                                }
                            }

                            if (!isCheck)
                            {
                                Match ma = reg.Match(data.RetailerProductName);
                                if (ma.Success)
                                {
                                    isCheck = true;
                                }
                            }

                            if (string.IsNullOrEmpty(pkey))
                            {
                                reg = new Regex(@" (?<value>\d+) " + temp + " ", RegexOptions.IgnoreCase);
                                Match ma = reg.Match(data.ProductName);
                                if (ma.Success)
                                {
                                    pvalue = ma.Groups["value"].Value;
                                    pkey = temp;
                                }
                            }

                            if (!isCheck)
                            {
                                Match ma = reg.Match(data.RetailerProductName);
                                if (ma.Success)
                                {
                                    isCheck = true;
                                }
                            }
                        }

                        if (isCheck && !string.IsNullOrEmpty(pkey) && !string.IsNullOrEmpty(pvalue))
                        {
                            bool isTrue = false;
                            foreach (string temp in temps)
                            {
                                string key = " " + pvalue + temp + " ";
                                string key1 = " " + pvalue + " " + temp + " ";
                                if (pkey.ToLower() == "gb" && temp.ToLower() == "tb")
                                {
                                    int value = 0;
                                    int.TryParse(pvalue, out value);

                                    key = " " + (value / 1024) + temp + " ";
                                    key1 = " " + (value / 1024) + " " + temp + " ";
                                    if (data.RetailerProductName.ToLower().Contains(key.ToLower()))
                                    {
                                        isTrue = true;
                                        break;
                                    }
                                    else
                                    {
                                        key = " " + (value / 1000) + temp + " ";
                                        key1 = " " + (value / 1000) + " " + temp + " ";
                                    }
                                }
                                else if (pkey.ToLower() == "tb" && temp.ToLower() == "gb")
                                {
                                    int value = 0;
                                    int.TryParse(pvalue, out value);

                                    key = " " + (value * 1024) + temp + " ";
                                    key1 = " " + (value * 1024) + " " + temp + " ";
                                    if (data.RetailerProductName.ToLower().Contains(key.ToLower()))
                                    {
                                        isTrue = true;
                                        break;
                                    }
                                    else
                                    {
                                        key = " " + (value * 1000) + temp + " ";
                                        key1 = " " + (value * 1000) + " " + temp + " ";
                                    }
                                }

                                if (data.RetailerProductName.ToLower().Contains(key.ToLower()))
                                {
                                    isTrue = true;
                                    break;
                                }
                                else if(data.RetailerProductName.ToLower().Contains(key1.ToLower()))
                                {
                                    isTrue = true;
                                    break;
                                }
                            }

                            if (!isTrue)
                            {
                                isError = true;
                                stringError = "type 1:" + pvalue + pkey;
                            }
                        }
                        #endregion
                    }
                    else if (k.keywordType == 2)
                    {
                        string pkey = string.Empty;
                        string[] temps = k.Keyword.Split(',');
                        foreach (string temp in temps)
                        {
                            if (data.ProductName.Contains(" " + temp + " "))
                            {
                                pkey = temp;
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(pkey) && !data.RetailerProductName.Contains(" " + pkey + " "))
                        {
                            isError = true;
                            stringError = "type 2:" + pkey + "|" + data.RetailerProductName;
                        }
                    }
                    else if (k.keywordType == 3)
                    {
                        string pkey = string.Empty;
                        string[] temps = k.Keyword.Split(',');
                        foreach (string temp in temps)
                        {
                            if (data.ProductName.Contains(" " + temp + " "))
                            {
                                pkey = temp;
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(pkey))
                        {
                            string[] ntemps = k.Keyword.Replace(pkey, "").Split(',');
                            foreach (string ntemp in ntemps)
                            {
                                if (string.IsNullOrEmpty(ntemp)) continue;
                                if (data.RetailerProductName.Contains(" " + ntemp + " "))
                                {
                                    isError = true;
                                    stringError = "type 3:" + pkey + "|" + ntemp;
                                    break;
                                }
                            }
                        }
                    }

                    if (isError)
                        break;
                }

                if (isError)
                    InsertMergeErrorReport(data.RetailerProductId, data.ProductId, string.Empty, string.Empty, string.Empty, stringError);
            }

            return isError;
        }

        private void CheckProductName(ProductData data)
        {
            if (data.ManufacturerID == -1) return;

            try
            {
                string manuname = GetManufacturer(data.ManufacturerID);
                if (!data.RetailerProductName.ToLower().Contains(manuname.ToLower()))
                {
                    List<MappingNoModelData> listTemp = listNoModel.Where(m => m.CategoryId == data.CategoryID).ToList();
                    List<MergingOptionSettingData> listOption = listSetting.Where(s => s.CategoryId == data.CategoryID).ToList();
                    foreach (MergingOptionSettingData item in listOption)
                    {
                        Regex reg = null;
                        if (item.FirstCharacterIsLetter && item.IncludeCharacterAndLetter)
                            reg = new Regex(@"^[a-z]+\S*\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        else if (item.FirstCharacterIsLetter && !item.IncludeCharacterAndLetter)
                            reg = new Regex(@"^[a-z]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        else if (!item.FirstCharacterIsLetter && item.IncludeCharacterAndLetter)
                            reg = new Regex(@"(^\d+[\s\S]*?[a-z]+)|(^[a-z]+[\s\S]*?\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        else if (!item.FirstCharacterIsLetter && !item.IncludeCharacterAndLetter)
                        {
                            if (data.CategoryID == 2588 || data.CategoryID == 3016)
                                reg = new Regex(@"^[0-9]+\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            else
                                reg = new Regex(@"^[a-z0-9]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        }

                        string pkeyword = string.Empty;
                        string[] keyWords = SpecialCharacter(data.ProductName);
                        for (int i = 0; i < keyWords.Length; i++)
                        {
                            string key = keyWords[i];
                            if (reg.IsMatch(key))
                            {
                                if (key.Length >= item.LengthModel)
                                {
                                    MappingNoModelData nom = listTemp.SingleOrDefault(m => m.Keyword.ToLower() == key.ToLower());
                                    if (nom != null)
                                        continue;
                                    else
                                    {
                                        pkeyword = key.ToLower();
                                        break;
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(pkeyword))
                        {
                            string rpkeyword = string.Empty;
                            string[] rkeyWords = SpecialCharacter(data.RetailerProductName);
                            for (int i = 0; i < rkeyWords.Length; i++)
                            {
                                string key = rkeyWords[i];
                                if (reg.IsMatch(key))
                                {
                                    if (key.Length >= item.LengthModel)
                                    {
                                        MappingNoModelData nom = listTemp.SingleOrDefault(m => m.Keyword.ToLower() == key.ToLower());
                                        if (nom != null)
                                            continue;
                                        else
                                        {
                                            rpkeyword = key.ToLower();
                                            break;
                                        }
                                    }
                                }
                            }

                            if (pkeyword != rpkeyword)
                                InsertMergeErrorReport(data.RetailerProductId, data.ProductId, manuname, pkeyword + "|" + rpkeyword, string.Empty, string.Empty);
                        }
                    }
                }
            }
            catch (Exception ex) { Write("check productname error....... " + ex.Message + ex.StackTrace + DateTime.Now); }
        }

        private string[] SpecialCharacter(string productName)
        {
            foreach (string item in listSpecial)
                productName = productName.Replace(item, " ");
            string[] temp = productName.ToLower().Split(' ');
            if (temp.Length > 3 && temp[1] == "&")
            {
                string info = temp[0] + " " + temp[1] + " " + temp[2];
                productName = productName.ToLower().Replace(info, "");
                string[] infos = productName.Split(' ');
                string[] m = { info };
                m.CopyTo(infos, 0);
                return infos;
            }
            else
                return temp;
        }

        private void InsertMergeErrorReport(int rpid, int pid, string SameBrand, string SameModel, string PriceRate, string keyword)
        {
            Write("Insert merge error report......." + DateTime.Now);
            bool isUpdate = false;
            string sql = "Select * From CSK_Store_MergeErrorReport Where PID = " + pid + " And RPID = " + rpid;
            try
            {
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    isUpdate = true;
                }
                dr.Close();

                sql = "Insert CSK_Store_MergeErrorReport values(" + pid + ", " + rpid + ", "
                    + "'" + SameBrand + "', '" + SameModel + "', '" + PriceRate + "', '" + keyword + "', 1, 0, 0, '', "
                    + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                if (isUpdate)
                {
                    sql = "Update CSK_Store_MergeErrorReport Set SameBrand = '" + SameBrand + "', SameModel = '" + SameModel + "', "
                        + "PriceRate = '" + PriceRate + "', ModifiedOn = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', "
                        + "Keyword = '" + keyword + "' Where PID = " + pid + " And RPID = " + rpid;
                }

                sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                sp.Execute();
            }
            catch (Exception ex) { Write("Insert merge error report error.......  sql: " + sql + ex.Message + ex.StackTrace + DateTime.Now); }
        }

        private void GetAllProductByRetailerId(int rid, string stringcids, List<ProductData> products)
        {
            try
            {
                string sql = "Select rp.RetailerProductId, rp.RetailerProductName, rp.RetailerPrice, rp.RetailerProductCondition, p.ProductId, "
                            + "p.ProductName, p.ManufacturerID, p.CategoryID From CSK_Store_RetailerProduct rp inner join CSK_Store_Product p "
                            + "On rp.ProductId = p.ProductID Where rp.RetailerId = " + rid + " And rp.RetailerProductStatus = 1 And rp.IsDeleted = 0 "
                            + "And p.IsMerge = 1 And p.CategoryID in (" + stringcids + ")";
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    int rpid = 0, pid = 0, mid = 0, cid = 0, rcid = 0;
                    int.TryParse(dr["RetailerProductId"].ToString(), out rpid);
                    int.TryParse(dr["ProductId"].ToString(), out pid);
                    int.TryParse(dr["ManufacturerID"].ToString(), out mid);
                    int.TryParse(dr["RetailerProductCondition"].ToString(), out rcid);
                    int.TryParse(dr["CategoryID"].ToString(), out cid);
                    decimal price = 0m;
                    decimal.TryParse(dr["RetailerPrice"].ToString(), out price);

                    ProductData product = new ProductData();
                    product.RetailerProductId = rpid;
                    product.RetailerProductName = dr["RetailerProductName"].ToString();
                    product.RetailerPrice = price;
                    product.RetailerProductCondition = rcid;
                    product.ProductId = pid;
                    product.ProductName = dr["ProductName"].ToString();
                    product.ManufacturerID = mid;
                    product.CategoryID = cid;
                    products.Add(product);
                }
                dr.Close();
            }
            catch (Exception ex) { Write("Get all product by retailerid error......." + ex.Message + ex.StackTrace + DateTime.Now); }
        }

        private void GetAllRetailers()
        {
            try
            {
                Write("Get all retailerid......." + DateTime.Now);
                string sql = "Select RetailerId From CSK_Store_Retailer Where RetailerStatus = 1 And RetailerCountry " + CountryId;
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    int rid = 0;
                    int.TryParse(dr["RetailerId"].ToString(), out rid);

                    listRids.Add(rid);
                }
                dr.Close();
            }
            catch (Exception ex) { Write("Get all retailerid error......." + ex.Message + ex.StackTrace + DateTime.Now); }
        }

        private string GetAllCategorys()
        {
            Write("Get all categoryid......." + DateTime.Now);
            string stringcids = string.Empty;
            try
            {
                string sql = "Select Categoryid From csk_store_helptopcategory";
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    int cid = 0;
                    int.TryParse(dr["Categoryid"].ToString(), out cid);

                    stringcids += cid + ",";
                }
                dr.Close();
                if (!string.IsNullOrEmpty(stringcids))
                    stringcids = stringcids.Substring(0, stringcids.LastIndexOf(','));
            }
            catch (Exception ex) { Write("Get all categoryid error......." + ex.Message + ex.StackTrace + DateTime.Now); }
            return stringcids;
        }

        private void GetAllMergeReportKeyword()
        {
            try
            {
                Write("Get all keyword......." + DateTime.Now);
                listKeyword = new List<ReportKeywordData>();
                string sql = "Select * From CSK_Store_MergeReportKeyword";
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    int id = 0, typeid = 0, cid = 0;
                    int.TryParse(dr["Id"].ToString(), out id);
                    int.TryParse(dr["KeywordType"].ToString(), out typeid);
                    int.TryParse(dr["CategoryId"].ToString(), out cid);

                    ReportKeywordData data = new ReportKeywordData();
                    data.Id = id;
                    data.Keyword = dr["Keyword"].ToString();
                    data.keywordType = typeid;
                    data.CategoryId = cid;
                    listKeyword.Add(data);
                }
                dr.Close();
            }
            catch (Exception ex) { Write("Get all keyword error......." + ex.Message + ex.StackTrace + DateTime.Now); }
        }

        private void GetMergingOptionSettings()
        {
            listSetting = new List<MergingOptionSettingData>();
            string sql = "select Categoryid, FirstCharacterIsLetter, IncludeCharacterAndLetter, "
                        + "LengthModel, isColourMatch from CSK_Store_AutomaticMergingOptionSettings";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int cid = 0, length = 0;
                int.TryParse(dr["Categoryid"].ToString(), out cid);
                int.TryParse(dr["LengthModel"].ToString(), out length);
                bool first = false, character = false, iscolour = false;
                bool.TryParse(dr["FirstCharacterIsLetter"].ToString(), out first);
                bool.TryParse(dr["IncludeCharacterAndLetter"].ToString(), out character);
                bool.TryParse(dr["isColourMatch"].ToString(), out iscolour);

                MergingOptionSettingData data = new MergingOptionSettingData();
                data.CategoryId = cid;
                data.FirstCharacterIsLetter = first;
                data.IncludeCharacterAndLetter = character;
                data.LengthModel = length;
                data.isColourMatch = iscolour;
                listSetting.Add(data);
            }
            dr.Close();
        }

        private void GetSpecialCharacter()
        {
            listSpecial = new List<string>();
            string sql = "select SpecialCharacter from CSK_Store_AutomaticSpecialCharacter";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                listSpecial.Add(dr["SpecialCharacter"].ToString());
            }
            dr.Close();
        }

        private void GetNoModel()
        {
            listNoModel = new List<MappingNoModelData>();
            string sql = "select CategoryId, Keyword, BlackListKeyword from CSK_Store_AutomaticMappingNoModel";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int cid = 0;
                int.TryParse(dr["CategoryId"].ToString(), out cid);
                bool blacklist = false;
                bool.TryParse(dr["BlackListKeyword"].ToString(), out blacklist);

                MappingNoModelData data = new MappingNoModelData();
                data.CategoryId = cid;
                data.Keyword = dr["Keyword"].ToString();
                data.BlackList = blacklist;
                listNoModel.Add(data);
            }
            dr.Close();
        }
        
        private string GetManufacturer(int mid)
        {
            string manuname = string.Empty;
            string sql = "select ManufacturerName from CSK_Store_Manufacturer where ManufacturerID = " + mid;
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                manuname = dr["ManufacturerName"].ToString();
            }
            dr.Close();

            return manuname;
        }

        private void Write(string info)
        {
            System.Console.WriteLine(info);

            _sw.WriteLine(info);
            _sw.WriteLine(_sw.NewLine);
            _sw.Flush();
        }
    }
}
