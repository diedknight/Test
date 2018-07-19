using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeCommon;
using System.Text.RegularExpressions;
using IndexBuildCommon.Data;
using PriceMeDBA;
using PriceMeCache;
using PriceMeCommon.BusinessLogic;

namespace IndexBuildCommon
{
    public static class UrlController
    {
        private static Regex illegalReg = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        public static string CatalogUrl(Dictionary<string, string> queryParameters)
        {
            if (!queryParameters.ContainsKey("c"))
            {
                return "";
            }

            int cid = int.Parse(queryParameters["c"]);
            PriceMeCache.CategoryCache category = CategoryController.GetCategoryByCategoryID(cid, AppValue.CountryId);

            string url = "";
            if (category != null)
            {
                if (queryParameters.Count == 1 || (category.IsSiteMap) || (category.IsSiteMapDetail))
                {
                    url = "/" + FilterInvalidUrlPathChar(category.CategoryName) + "/c-" + category.CategoryID +
                            ".aspx";
                }
                else
                {
                    AttributeParameterCollection urlAttributeParameterList =
                        GetAttributeParameterCollectionAndSortAttributesParamters(queryParameters, category);
                    string catalogUrlDescriptionString = GetCatalogUrlDescriptionString(queryParameters,
                                                                                        urlAttributeParameterList,
                                                                                        category);
                    string queryString = GetUrlParameterString(queryParameters);
                    url = "/" + catalogUrlDescriptionString + "/p_" + queryString + ".aspx";
                }
            }
            return url;
        }

        private static string GetUrlParameterString(Dictionary<string, string> queryParameters)
        {
            Dictionary<string, string> oldParameters = new Dictionary<string, string>(queryParameters);
            Dictionary<string, string> newParameters = new Dictionary<string, string>();

            if (queryParameters.ContainsKey("c"))
            {
                newParameters.Add("c", queryParameters["c"]);
                oldParameters.Remove("c");
            }

            if (queryParameters.ContainsKey("m"))
            {
                newParameters.Add("m", queryParameters["m"]);
                oldParameters.Remove("m");
            }

            if (queryParameters.ContainsKey("pr"))
            {
                newParameters.Add("pr", queryParameters["pr"]);
                oldParameters.Remove("pr");
            }

            if (queryParameters.ContainsKey("avs"))
            {
                newParameters.Add("avs", queryParameters["avs"]);
                oldParameters.Remove("avs");
            }

            if (queryParameters.ContainsKey("rid"))
            {
                newParameters.Add("rid", queryParameters["rid"]);
                oldParameters.Remove("rid");
            }

            if (queryParameters.ContainsKey("samp"))
            {
                newParameters.Add("samp", queryParameters["samp"]);
                oldParameters.Remove("samp");
            }

            if (oldParameters.Count > 0)
            {
                List<string> otherParameter = new List<string>();
                foreach (string key in oldParameters.Keys)
                {
                    otherParameter.Add(key);
                }
                otherParameter.Sort();
                foreach (string key in otherParameter)
                {
                    newParameters.Add(key, queryParameters[key]);
                }
            }

            string queryString = "";
            foreach (string pName in newParameters.Keys)
            {
                if (pName == "pg" && (queryParameters[pName] == "1" || queryParameters[pName] == "0"))
                {
                    continue;
                }

                if (pName != "samp")
                {
                    queryString += pName + "-" + FilterInvalidUrlPathChar(queryParameters[pName]) + ",";
                }
                else
                {
                    queryString += pName + "-" + queryParameters[pName] + ",";
                }
            }
            return queryString.TrimEnd(',');
        }

        private static string GetCatalogUrlDescriptionString(Dictionary<string, string> queryParameters, AttributeParameterCollection urlAttributeParameterList, PriceMeCache.CategoryCache category)
        {
            string catalogUrlDescriptionString = "";

            if (queryParameters.ContainsKey("m") && queryParameters["m"].Length > 0)
            {
                int mid = int.Parse(queryParameters["m"]);
                var manufacturer = ManufacturerController.GetManufacturerByID(mid, AppValue.CountryId);
                if (manufacturer != null)
                {
                    //throw new Exception("catalogParameterError! mid : " + mid + " not exist!");
                    catalogUrlDescriptionString = manufacturer.ManufacturerName + " ";
                }
            }

            catalogUrlDescriptionString += category.CategoryName + " ";

            if (urlAttributeParameterList.Count > 0)
            {
                catalogUrlDescriptionString += urlAttributeParameterList.ToURLString();
            }

            catalogUrlDescriptionString = FilterInvalidUrlPathChar(catalogUrlDescriptionString.TrimEnd(' '));

            if (catalogUrlDescriptionString.Length > 100)
            {
                catalogUrlDescriptionString = catalogUrlDescriptionString.Substring(0, 100);
            }
            return catalogUrlDescriptionString;
        }

        public static string FilterInvalidUrlPathChar(string sourceString)
        {

            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");
            sourceString = illegalReg3.Replace(sourceString, "");
            return sourceString;
        }

        public static AttributeParameterCollection GetAttributeParameterCollectionAndSortAttributesParamters(Dictionary<string, string> queryParameters, PriceMeCache.CategoryCache category)
        {
            AttributeParameterCollection attributeParameterCollection = new AttributeParameterCollection();
            if (queryParameters.ContainsKey("avs"))
            {
                string attributeValuesIDString = queryParameters["avs"];
                string[] avids = attributeValuesIDString.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                int aid;

                List<string> avidsString = new List<string>();
                avidsString.AddRange(avids);
                avidsString.Sort(new AttributeStringSort());

                queryParameters["avs"] = GetAttributeValueString(avidsString);

                foreach (string avid in avids)
                {
                    string avidString = avid;
                    if (avidString.ToLower().EndsWith("r"))
                    {
                        avidString = avid.Substring(0, avidString.Length - 1);
                        int.TryParse(avidString, out aid);
                        if (aid != 0)
                        {
                            AttributeParameter attributeParameter = GetAttributeParameterByAttributeRangeIDAndCategoryID(aid, category.CategoryID);
                            if (attributeParameter != null)
                            {
                                attributeParameterCollection.Add(attributeParameter);
                            }
                        }
                    }
                    else
                    {
                        int.TryParse(avidString, out aid);
                        if (aid != 0)
                        {
                            AttributeParameter attributeParameter = GetAttributeParameterByAttributeValueIDAndCategoryID(aid, category.CategoryID);
                            if (attributeParameter != null)
                            {
                                attributeParameterCollection.Add(attributeParameter);
                            }
                        }
                    }
                }
            }
            return attributeParameterCollection;
        }

        public static AttributeParameter GetAttributeParameterByAttributeValueIDAndCategoryID(int aid, int cid)
        {
            PriceMeCache.AttributeTitleCache productDescriptorTitle = AttributesController.GetAttributeTitleByVauleID(aid);
            if (productDescriptorTitle != null)
            {
                PriceMeCache.AttributeValueCache attributeValue = AttributesController.GetAttributeValueByID(aid);
                if (attributeValue != null)
                {
                    string key = cid + "," + productDescriptorTitle.TypeID;
                    CategoryAttributeTitleMapCache categoryAttributeTitleMap = AttributesController.GetCategoryAttributeTitleMapByKey(key);
                    if (categoryAttributeTitleMap != null)
                    {
                        AttributeParameter attributeParameter = new AttributeParameter();
                        attributeParameter.ListOrder = categoryAttributeTitleMap.AttributeOrder;
                        attributeParameter.AttributeName = productDescriptorTitle.Title;
                        attributeParameter.AttributeValue = attributeValue.Value + (productDescriptorTitle.Unit == null ? "" : productDescriptorTitle.Unit.Trim());
                        return attributeParameter;
                    }
                }
            }
            return null;
        }

        public static AttributeParameter GetAttributeParameterByAttributeRangeIDAndCategoryID(int arid, int cid)
        {
            var attributeValueRange = AttributesController.GetAttributeValueRangeByID(arid);
            if (attributeValueRange != null)
            {
                PriceMeCache.AttributeTitleCache productDescriptorTitle = AttributesController.GetAttributeTitleByID(attributeValueRange.AttributeTitleID);
                string key = cid + "," + productDescriptorTitle.TypeID;

                CategoryAttributeTitleMapCache categoryAttributeTitleMap = AttributesController.GetCategoryAttributeTitleMapByKey(key);
                if (categoryAttributeTitleMap != null)
                {
                    AttributeParameter attributeParameter = new AttributeParameter();
                    attributeParameter.ListOrder = categoryAttributeTitleMap.AttributeOrder;
                    attributeParameter.AttributeName = productDescriptorTitle.Title;
                    attributeParameter.AttributeValue = AttributesController.GetAttributeValueString(attributeValueRange, productDescriptorTitle.Unit);
                    return attributeParameter;
                }
            }
            return null;
        }

        private static string GetAttributeValueString(List<string> avidsString)
        {
            string str = "";
            foreach (string adav in avidsString)
            {
                str += adav + "-";
            }
            return str.TrimEnd('-');
        }
    }
}
