﻿using ImportAttrs.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs
{
    public static class ImportController
    {
        static readonly string DefaultCategoryUnit = "cm";
        static string DBConnectionStr_Static;
        static Dictionary<int, Dictionary<int, List<AttributeRetailerMap>>> AttributeRetailerMapCategoryDic_Static;
        static Dictionary<int, string> CategoryUnitDic_Static;
        static AttributeRetailerGeneralHelper AttributeRetailerGeneralHelper_Static;
        static Dictionary<int, AttributeTitleInfo> AttributeTitleDic_Static;
        static Dictionary<int, List<AttributeValueInfo>> AttributeValueListDic_Static;
        static Dictionary<int, CompareAttributeInfo> CompareAttributeDic_Static;
        static Dictionary<int, List<CompareAttributeValueMap>> CategoryCompareAttributeValueMapDic_Static;
        static LogWriter MatchLogWriter_Static;
        static LogWriter UnMatchLogWriter_Static;
        static ImportController()
        {
            DBConnectionStr_Static = System.Configuration.ConfigurationManager.ConnectionStrings["PriceMe_DB"].ConnectionString;
            string importAttrLogDir = System.Configuration.ConfigurationManager.AppSettings["ImportAttrLogDir"];
            DateTime nowTime = DateTime.Now;
            string matchLogPath = System.IO.Path.Combine(importAttrLogDir, "Match_" + nowTime.ToString("yyyy-MM-dd_HH_mm") + ".txt");
            string unMatchLogPath = System.IO.Path.Combine(importAttrLogDir, "UnMatch_" + nowTime.ToString("yyyy-MM-dd_HH_mm") + ".txt");
            MatchLogWriter_Static = new LogWriter(matchLogPath);
            UnMatchLogWriter_Static = new LogWriter(unMatchLogPath);

            Init();
        }

        public static void Init()
        {
            AttributeRetailerMapCategoryDic_Static = new Dictionary<int, Dictionary<int, List<AttributeRetailerMap>>>();
            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();
                string selectAttrsRetailerMapSql = @"SELECT [RetailerId]
                                                    ,[CategoryId]
                                                    ,[Unit]
                                                    ,[PM_AttributeID]
                                                    ,[AttributeType]
                                                    ,[IsRemove]
                                                    ,[RemoveKeword]
                                                    ,[IsSplit]
                                                    ,[SplitKeyword]
                                                    ,[KeepBefore]
                                                    ,[IsCombine]
                                                    ,[CombineAttribute]
                                                    ,[KeepCombineAttributeBefore]
                                                    ,[RetailerAttributeName]
                                                    ,[AttributeRetailerMapID]
                                                    FROM [dbo].[CSK_Store_AttributeRetailerMap]";
                using (SqlCommand sqlCmd = new SqlCommand(selectAttrsRetailerMapSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            AttributeRetailerMap arm = new AttributeRetailerMap();
                            arm.RetailerId = sqlDr.GetInt32(0);
                            arm.CategoryId = sqlDr.GetInt32(1);
                            if (!sqlDr.IsDBNull(2))
                            {
                                arm.Unit = sqlDr.GetString(2);
                            }
                            if (!sqlDr.IsDBNull(3))
                            {
                                arm.PM_AttributeID = sqlDr.GetString(3);
                            }
                            arm.AttributeType = sqlDr.GetInt32(4);
                            arm.IsRemove = sqlDr.GetBoolean(5);
                            if (!sqlDr.IsDBNull(6))
                            {
                                arm.RemoveKeword = sqlDr.GetString(6);
                            }
                            arm.IsSplit = sqlDr.GetBoolean(7);
                            if (!sqlDr.IsDBNull(8))
                            {
                                arm.SplitKeyword = sqlDr.GetString(8);
                            }
                            arm.KeepBefore = sqlDr.GetBoolean(9);
                            arm.IsCombine = sqlDr.GetBoolean(10);
                            if (!sqlDr.IsDBNull(11))
                            {
                                arm.CombineAttribute = sqlDr.GetString(11);
                            }
                            if (!sqlDr.IsDBNull(12))
                            {
                                arm.KeepCombineAttributeBefore = sqlDr.GetBoolean(12);
                            }
                            else
                            {
                                arm.KeepCombineAttributeBefore = false;
                            }
                            arm.RetailerAttributeName = sqlDr.GetString(13);
                            arm.Id = sqlDr.GetInt32(14);

                            if (AttributeRetailerMapCategoryDic_Static.ContainsKey(arm.RetailerId))
                            {
                                if (AttributeRetailerMapCategoryDic_Static[arm.RetailerId].ContainsKey(arm.CategoryId))
                                {
                                    AttributeRetailerMapCategoryDic_Static[arm.RetailerId][arm.CategoryId].Add(arm);
                                }
                                else
                                {
                                    List<AttributeRetailerMap> list = new List<AttributeRetailerMap>();
                                    list.Add(arm);
                                    AttributeRetailerMapCategoryDic_Static[arm.RetailerId].Add(arm.CategoryId, list);
                                }
                            }
                            else
                            {
                                List<AttributeRetailerMap> list = new List<AttributeRetailerMap>();
                                list.Add(arm);
                                Dictionary<int, List<AttributeRetailerMap>> dic = new Dictionary<int, List<AttributeRetailerMap>>();
                                dic.Add(arm.CategoryId, list);
                                AttributeRetailerMapCategoryDic_Static.Add(arm.RetailerId, dic);
                            }
                        }
                    }
                }

                CategoryUnitDic_Static = new Dictionary<int, string>();
                string selectCategoryUnitSql = "select CategoryID, SizeUnit from CSK_Store_CategoryUnit where SizeUnit <> 'cm'";
                using (SqlCommand sqlCmd = new SqlCommand(selectCategoryUnitSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            CategoryUnitDic_Static.Add(sqlDr.GetInt32(0), sqlDr.GetString(1));
                        }
                    }
                }

                CategoryCompareAttributeValueMapDic_Static = new Dictionary<int, List<CompareAttributeValueMap>>();
                string selectCompareAttributeValueSql = "SELECT [CompareAttributeID],[Value],[Skeywords],[CategoryID] FROM [dbo].[AT_CompareAttributeValue_Map]";
                using (SqlCommand sqlCmd = new SqlCommand(selectCompareAttributeValueSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            CompareAttributeValueMap cavm = new CompareAttributeValueMap();
                            cavm.CompareAttributeID = sqlDr.GetInt32(0);
                            cavm.Value = sqlDr.GetString(1);
                            if (!sqlDr.IsDBNull(2))
                            {
                                cavm.Skeywords = sqlDr.GetString(2);
                            }
                            cavm.CategoryID = sqlDr.GetInt32(3);
                            cavm.SkeywordList = new List<string>();
                            if (!string.IsNullOrEmpty(cavm.Skeywords))
                            {
                                cavm.SkeywordList.AddRange(cavm.Skeywords.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
                            }

                            if (CategoryCompareAttributeValueMapDic_Static.ContainsKey(cavm.CategoryID))
                            {
                                CategoryCompareAttributeValueMapDic_Static[cavm.CategoryID].Add(cavm);
                            }
                            else
                            {
                                List<CompareAttributeValueMap> list = new List<CompareAttributeValueMap>();
                                list.Add(cavm);
                                CategoryCompareAttributeValueMapDic_Static.Add(cavm.CategoryID, list);
                            }

                        }
                    }
                }

                AttributeRetailerGeneralHelper_Static = new AttributeRetailerGeneralHelper();
                string selectAttributeRetailerGeneralMapSql = "SELECT [RetailerID],[CategoryID],[IsRemoveKeyword],[Keyword] FROM [dbo].[CSK_Store_AttributeRetailerGeneralMap]";
                using (SqlCommand sqlCmd = new SqlCommand(selectAttributeRetailerGeneralMapSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            AttributeRetailerGeneralMap argm = new AttributeRetailerGeneralMap();
                            if (!sqlDr.IsDBNull(0))
                            {
                                argm.RetailerId = sqlDr.GetInt32(0);
                            }
                            if (!sqlDr.IsDBNull(1))
                            {
                                argm.CategoryId = sqlDr.GetInt32(1);
                            }
                            argm.IsRemoveKeyword = sqlDr.GetBoolean(2);
                            argm.Keyword = sqlDr.GetString(3);
                            AttributeRetailerGeneralHelper_Static.Add(argm);
                        }
                    }
                }

                AttributeTitleDic_Static = new Dictionary<int, AttributeTitleInfo>();
                string selectPriceMeAttributeTitleSql = "SELECT [TypeID],[Title],[Unit],[AttributeTypeID],[CatelogAttributes] FROM [Priceme].[dbo].[CSK_Store_ProductDescriptorTitle]";
                using (SqlCommand sqlCmd = new SqlCommand(selectPriceMeAttributeTitleSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            AttributeTitleInfo ati = new AttributeTitleInfo();
                            ati.TitleId = sqlDr.GetInt32(0);
                            ati.Title = sqlDr.GetString(1);
                            if (!sqlDr.IsDBNull(2))
                            {
                                ati.Unit = sqlDr.GetString(2);
                            }
                            ati.AttributeTypeID = sqlDr.GetInt32(3);
                            if (!sqlDr.IsDBNull(4))
                            {
                                ati.CatelogAttributes = sqlDr.GetBoolean(4);
                            }

                            AttributeTitleDic_Static.Add(ati.TitleId, ati);
                        }
                    }
                }

                AttributeValueListDic_Static = new Dictionary<int, List<AttributeValueInfo>>();
                string selectPriceMeAttributeValueSql = "SELECT [AttributeValueID],[AttributeTitleID],[Value],[PopularAttribute],[ListOrder] FROM [Priceme].[dbo].[CSK_Store_AttributeValue]";
                using (SqlCommand sqlCmd = new SqlCommand(selectPriceMeAttributeValueSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            AttributeValueInfo avi = new AttributeValueInfo();
                            avi.AttributeValueId = sqlDr.GetInt32(0);
                            avi.TitleId = sqlDr.GetInt32(1);
                            if (!sqlDr.IsDBNull(2))
                            {
                                avi.Value = sqlDr.GetString(2);
                            }
                            if (!sqlDr.IsDBNull(3))
                            {
                                avi.PopularAttribute = sqlDr.GetBoolean(3);
                            }
                            if (!sqlDr.IsDBNull(4))
                            {
                                avi.ListOrder = sqlDr.GetInt32(4);
                            }

                            if (AttributeValueListDic_Static.ContainsKey(avi.TitleId))
                            {
                                AttributeValueListDic_Static[avi.TitleId].Add(avi);
                            }
                            else
                            {
                                List<AttributeValueInfo> list = new List<AttributeValueInfo>();
                                list.Add(avi);
                                AttributeValueListDic_Static.Add(avi.TitleId, list);
                            }
                        }
                    }
                }

                CompareAttributeDic_Static = new Dictionary<int, CompareAttributeInfo>();
                string selectCompareAttributesSql = "SELECT [CompareAttributeID],[CategoryID],[Name],[IsNumeric],[Unit],[AttributeTypeID] FROM [dbo].[Store_Compare_Attributes]";
                using (SqlCommand sqlCmd = new SqlCommand(selectCompareAttributesSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            CompareAttributeInfo cai = new CompareAttributeInfo();
                            cai.CompareAttributeId = sqlDr.GetInt32(0);
                            cai.CategoryId = sqlDr.GetInt32(1);
                            cai.Name = sqlDr.GetString(2);

                            if (!sqlDr.IsDBNull(3))
                            {
                                cai.IsNumeric = sqlDr.GetBoolean(3);
                            }
                            if (!sqlDr.IsDBNull(4))
                            {
                                cai.Unit = sqlDr.GetString(4);
                            }
                            cai.AttributeTypeID = sqlDr.GetInt32(5);
                            CompareAttributeDic_Static.Add(cai.CompareAttributeId, cai);
                        }
                    }
                }
            }
        }

        static SqlConnection GetDBConnection()
        {
            return new SqlConnection(DBConnectionStr_Static);
        }

        static List<AttributeRetailerMap> GetAttributeRetailerMaps(int retailerId, int categoryId)
        {
            if (AttributeRetailerMapCategoryDic_Static.ContainsKey(retailerId))
            {
                if (AttributeRetailerMapCategoryDic_Static[retailerId].ContainsKey(categoryId))
                {
                    return AttributeRetailerMapCategoryDic_Static[retailerId][categoryId];
                }
            }

            return null;
        }

        static string GetCategoryUnit(int categoryId)
        {
            if (CategoryUnitDic_Static.ContainsKey(categoryId))
            {
                return CategoryUnitDic_Static[categoryId];
            }
            return DefaultCategoryUnit;
        }

        public static void ImportAttr(ImportProductInfo importProductInfo)
        {
            int productId = importProductInfo.ProductId;
            int categoryId = importProductInfo.CategoryId;
            int retailerId = importProductInfo.RetailerId;
            var attrDic = importProductInfo.AllAttributesDic;
            if (productId <= 0 || categoryId <= 0 || retailerId <= 0 || attrDic == null || attrDic.Count == 0)
            {
                UnMatchLogWriter_Static.WriteLine("Invalid product info.");
                UnMatchLogWriter_Static.WriteLine("ProductId : " + productId + " CategoryId : " + categoryId + " RetailerId : " + retailerId);
                return;
            }

            string lengthUnit = GetCategoryUnit(categoryId);
            ProductAllAttributesInfo productAllAttributesInfo = GetProductAllAttributesInfo(productId);
            List<AttributeRetailerMap> attributeRetailerMapList = null;
            if (AttributeRetailerMapCategoryDic_Static.ContainsKey(retailerId))
            {
                Dictionary<int, List<AttributeRetailerMap>> dic = AttributeRetailerMapCategoryDic_Static[retailerId];
                if (dic.ContainsKey(categoryId))
                {
                    attributeRetailerMapList = dic[categoryId];
                }
            }

            if (attributeRetailerMapList != null)
            {
                foreach (string attrTitle in attrDic.Keys)
                {
                    try
                    {
                        AttributeRetailerMap arm = attributeRetailerMapList.Where(a => a.RetailerAttributeName.Equals(attrTitle, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (arm != null)
                        {
                            var attrValue = attrDic[attrTitle];
                            attrValue = FixValue(attrValue, arm, retailerId, categoryId, attrDic);

                            if (string.IsNullOrEmpty(attrValue))
                            {
                                UnMatchLogWriter_Static.WriteLine("PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + arm.PM_AttributeID + " AttributeName : " + arm.RetailerAttributeName + " AttributeValue : " + attrDic[attrTitle] + " AttributeValue(Fixed) : null.");
                                int titleId = 0;
                                string titleName = "";
                                if (int.TryParse(arm.PM_AttributeID, out titleId))
                                {
                                    if (arm.AttributeType == 2)
                                    {
                                        titleName = AttributeTitleDic_Static[titleId].Title;
                                    }
                                    else
                                    {
                                        titleName = CompareAttributeDic_Static[titleId].Name;
                                    }

                                }
                                AttributeUnmatchedReport attributeUnmatchedReport = new AttributeUnmatchedReport(retailerId, categoryId, productId, arm.AttributeType, titleId, titleName, arm.RetailerAttributeName, attrDic[attrTitle], "", true);
                                WriteUnMatchedReport(attributeUnmatchedReport);
                                continue;
                            }

                            //宽度
                            if (arm.AttributeType == 99)
                            {
                                if (!string.IsNullOrEmpty(arm.Unit))
                                {
                                    attrValue = attrValue.Replace(arm.Unit, "").Trim();
                                }
                                float widthFloat = float.Parse(attrValue);

                                if (!string.IsNullOrEmpty(arm.Unit) && !arm.Unit.Equals(lengthUnit, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    widthFloat = ChangeUnit(widthFloat, arm.Unit, lengthUnit);
                                }

                                UpdateProductWidth(productId, widthFloat);

                                MatchLogWriter_Static.WriteLine("PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + "Width" + " AttributeName : " + widthFloat.ToString("0.00") + " AttributeValue : " + widthFloat.ToString("0.00"));

                                AttributeMatchedReport attributeMatchedReport = new AttributeMatchedReport(retailerId, categoryId, productId, arm.AttributeType, arm.AttributeType, "Width", arm.RetailerAttributeName, attrDic[attrTitle], widthFloat.ToString("0.00"), true, true);
                                WriteMatchedReport(attributeMatchedReport);
                            }
                            //高度
                            else if (arm.AttributeType == 999)
                            {
                                if (!string.IsNullOrEmpty(arm.Unit))
                                {
                                    attrValue = attrValue.Replace(arm.Unit, "").Trim();
                                }
                                float heightFloat = float.Parse(attrValue);

                                if (!string.IsNullOrEmpty(arm.Unit) && !arm.Unit.Equals(lengthUnit, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    heightFloat = ChangeUnit(heightFloat, arm.Unit, lengthUnit);
                                }

                                UpdateProductHeight(productId, heightFloat);

                                MatchLogWriter_Static.WriteLine("PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + "Height" + " AttributeName : " + heightFloat.ToString("0.00") + " AttributeValue : " + heightFloat.ToString("0.00"));

                                AttributeMatchedReport attributeMatchedReport = new AttributeMatchedReport(retailerId, categoryId, productId, arm.AttributeType, arm.AttributeType, "Height", arm.RetailerAttributeName, attrDic[attrTitle], heightFloat.ToString("0.00"), true, true);
                                WriteMatchedReport(attributeMatchedReport);
                            }
                            //深度
                            else if (arm.AttributeType == 9)
                            {
                                if (!string.IsNullOrEmpty(arm.Unit))
                                {
                                    attrValue = attrValue.Replace(arm.Unit, "").Trim();
                                }
                                float lengthFloat = float.Parse(attrValue);

                                if (!string.IsNullOrEmpty(arm.Unit) && !arm.Unit.Equals(lengthUnit, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    lengthFloat = ChangeUnit(lengthFloat, arm.Unit, lengthUnit);
                                }

                                UpdateProductLength(productId, lengthFloat);

                                MatchLogWriter_Static.WriteLine("PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + "Length" + " AttributeName : " + lengthFloat.ToString("0.00") + " AttributeValue : " + lengthFloat.ToString("0.00"));

                                AttributeMatchedReport attributeMatchedReport = new AttributeMatchedReport(retailerId, categoryId, productId, arm.AttributeType, arm.AttributeType, "Length", arm.RetailerAttributeName, attrDic[attrTitle], lengthFloat.ToString("0.00"), true, true);
                                WriteMatchedReport(attributeMatchedReport);
                            }
                            //重量
                            else if (arm.AttributeType == 1000)
                            {
                                if (!string.IsNullOrEmpty(arm.Unit))
                                {
                                    attrValue = attrValue.Replace(arm.Unit, "").Trim();
                                }
                                float weightFloat = float.Parse(attrValue);
                                if (!string.IsNullOrEmpty(arm.Unit) && !arm.Unit.Equals("kg", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    weightFloat = ChangeUnit(weightFloat, arm.Unit, "kg");
                                }

                                UpdateProductWeight(productId, weightFloat);

                                MatchLogWriter_Static.WriteLine("PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + "Weight" + " AttributeName : " + weightFloat.ToString("0.00") + " AttributeValue : " + weightFloat.ToString("0.00"));

                                AttributeMatchedReport attributeMatchedReport = new AttributeMatchedReport(retailerId, categoryId, productId, arm.AttributeType, arm.AttributeType, "Weight", arm.RetailerAttributeName, attrDic[attrTitle], weightFloat.ToString("0.00"), true, true);
                                WriteMatchedReport(attributeMatchedReport);
                            }
                            //长宽高在一起
                            else if (arm.AttributeType == 9999)
                            {
                                if (!string.IsNullOrEmpty(arm.Unit))
                                {
                                    attrValue = attrValue.Replace(arm.Unit, "").Trim();
                                }
                                string[] valueFormatStrs = arm.PM_AttributeID.ToLower().Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
                                string[] valueStrs = attrValue.ToLower().Split(new string[] { "x", "×" }, StringSplitOptions.RemoveEmptyEntries);

                                float width = 0, height = 0, length = 0;
                                for (int i = 0; i < 3; i++)
                                {
                                    string format = valueFormatStrs[i];
                                    float value = float.Parse(valueStrs[i].Trim());
                                    if (format.Equals("h"))
                                    {
                                        height = value;
                                    }
                                    else if (format.Equals("w"))
                                    {
                                        width = value;
                                    }
                                    else if (format.Equals("l"))
                                    {
                                        length = value;
                                    }
                                }
                                if (!string.IsNullOrEmpty(arm.Unit) && !arm.Unit.Equals(lengthUnit, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    width = ChangeUnit(width, arm.Unit, lengthUnit);
                                    height = ChangeUnit(height, arm.Unit, lengthUnit);
                                    length = ChangeUnit(length, arm.Unit, lengthUnit);
                                }

                                UpdateProductWHL(productId, width, height, length);

                                MatchLogWriter_Static.WriteLine("PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + arm.PM_AttributeID);

                                AttributeMatchedReport attributeMatchedReport = new AttributeMatchedReport(retailerId, categoryId, productId, arm.AttributeType, arm.AttributeType, "WHL", arm.RetailerAttributeName, attrDic[attrTitle], "W:" + width.ToString("0.00") + " H:" + height.ToString("0.00") + " L:" + length.ToString("0.00"), true, true);
                                WriteMatchedReport(attributeMatchedReport);
                            }
                            else
                            {
                                int titleId = 0;
                                if (int.TryParse(arm.PM_AttributeID, out titleId))
                                {
                                    //一般Attributes
                                    if (arm.AttributeType == 2)
                                    {
                                        var priceMeAttrTitle = AttributeTitleDic_Static[titleId];
                                        List<AttributeValueInfo> priceMeAttrValues = new List<AttributeValueInfo>();
                                        if (AttributeValueListDic_Static.ContainsKey(titleId))
                                        {
                                            priceMeAttrValues = AttributeValueListDic_Static[titleId];
                                        }

                                        float floatValue = 0f;
                                        if (!string.IsNullOrEmpty(arm.Unit))
                                        {
                                            attrValue = attrValue.Replace(arm.Unit, "").Trim();
                                            if (priceMeAttrTitle.AttributeTypeID == 6)
                                            {
                                                if (float.TryParse(attrValue, out floatValue))
                                                {
                                                    if (!string.IsNullOrEmpty(priceMeAttrTitle.Unit))
                                                    {
                                                        floatValue = ChangeUnit(floatValue, arm.Unit, priceMeAttrTitle.Unit);
                                                    }
                                                    attrValue = floatValue.ToString("0.00");
                                                }
                                            }
                                            else if (priceMeAttrTitle.AttributeTypeID == 4)
                                            {
                                                int intValue = 0;
                                                if (int.TryParse(attrValue, out intValue))
                                                {
                                                    if (!string.IsNullOrEmpty(priceMeAttrTitle.Unit))
                                                    {
                                                        intValue = (int)ChangeUnit((float)intValue, arm.Unit, priceMeAttrTitle.Unit);
                                                    }
                                                    attrValue = intValue.ToString();
                                                }
                                            }
                                        }

                                        ProductAttributeInfo productAttributeInfo = productAllAttributesInfo.ProductAttributeList.Where(p => p.AttributeTitleId == titleId).FirstOrDefault();

                                        AttributeValueInfo newAttributeValue = priceMeAttrValues.Where(av => av.Value.Equals(attrValue)).FirstOrDefault();
                                        if (newAttributeValue == null && newAttributeValue == null)
                                        {
                                            attrValue = floatValue.ToString("0.0");
                                            newAttributeValue = priceMeAttrValues.Where(av => av.Value.Equals(attrValue)).FirstOrDefault();
                                        }

                                        //如果Attribute的Value存在
                                        if (newAttributeValue != null)
                                        {
                                            //如果产品未加该Attribute
                                            if (productAttributeInfo == null)
                                            {
                                                InsertProductAttribute(productId, priceMeAttrTitle, newAttributeValue);

                                                AttributeMatchedReport attributeMatchedReport = new AttributeMatchedReport(retailerId, categoryId, productId, arm.AttributeType, priceMeAttrTitle.TitleId, priceMeAttrTitle.Title, arm.RetailerAttributeName, attrDic[attrTitle], attrValue, true, true);
                                                WriteMatchedReport(attributeMatchedReport);
                                            }
                                            else
                                            {
                                                //如果产品的Attribute和新跑到的Attribute不一致
                                                //if (newAttributeValue.AttributeValueId != productAttributeInfo.AttributeValueId)
                                                //{
                                                //    UpdateProductAttribute(productId, productAttributeInfo, newAttributeValue);
                                                //}

                                                AttributeMatchedReport attributeMatchedReport = new AttributeMatchedReport(retailerId, categoryId, productId, arm.AttributeType, priceMeAttrTitle.TitleId, priceMeAttrTitle.Title, arm.RetailerAttributeName, attrDic[attrTitle], attrValue, false, true);
                                                WriteMatchedReport(attributeMatchedReport);
                                            }

                                            MatchLogWriter_Static.WriteLine("PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + arm.PM_AttributeID + " AttributeName : " + arm.RetailerAttributeName + " AttributeValue : " + newAttributeValue.Value);
                                        }
                                        //如果Attribute的Value不存在
                                        else
                                        {
                                            //newAttributeValue = InsertAttributeValue(priceMeAttrTitle, attrValue);
                                            //if (productAttributeInfo == null)
                                            //{
                                            //    InsertProductAttribute(productId, priceMeAttrTitle, newAttributeValue);
                                            //}
                                            //else
                                            //{
                                            //    UpdateProductAttribute(productId, productAttributeInfo, newAttributeValue);
                                            //}

                                            UnMatchLogWriter_Static.WriteLine("PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + arm.PM_AttributeID + " AttributeName : " + arm.RetailerAttributeName + " AttributeValue : " + attrDic[attrTitle] + " AttributeValue(Fixed) : " + attrValue);

                                            AttributeUnmatchedReport attributeUnmatchedReport = new AttributeUnmatchedReport(retailerId, categoryId, productId, arm.AttributeType, titleId, priceMeAttrTitle.Title, arm.RetailerAttributeName, attrDic[attrTitle], attrValue, true);
                                            WriteUnMatchedReport(attributeUnmatchedReport);
                                        }
                                    }
                                    //比较用的Attributes
                                    else if (arm.AttributeType == 3)
                                    {
                                        var priceMeCompareAttrTitle = CompareAttributeDic_Static[titleId];
                                        bool isFloatValue = false;
                                        float floatValue = 0f;
                                        if (!string.IsNullOrEmpty(arm.Unit))
                                        {
                                            attrValue = attrValue.Replace(arm.Unit, "").Trim();
                                            if (priceMeCompareAttrTitle.AttributeTypeID == 6)
                                            {
                                                if (float.TryParse(attrValue, out floatValue))
                                                {
                                                    if (!string.IsNullOrEmpty(priceMeCompareAttrTitle.Unit))
                                                    {
                                                        floatValue = ChangeUnit(floatValue, arm.Unit, priceMeCompareAttrTitle.Unit);
                                                    }
                                                    attrValue = floatValue.ToString("0.00");
                                                    isFloatValue = true;
                                                }
                                            }
                                            else if (priceMeCompareAttrTitle.AttributeTypeID == 4)
                                            {
                                                int intValue = 0;
                                                if (int.TryParse(attrValue, out intValue))
                                                {
                                                    if (!string.IsNullOrEmpty(priceMeCompareAttrTitle.Unit))
                                                    {
                                                        intValue = (int)ChangeUnit((float)intValue, arm.Unit, priceMeCompareAttrTitle.Unit);
                                                    }
                                                    attrValue = intValue.ToString();
                                                }
                                            }
                                        }

                                        if (CategoryCompareAttributeValueMapDic_Static.ContainsKey(categoryId))
                                        {
                                            bool isValid = false;
                                            var ccm = CategoryCompareAttributeValueMapDic_Static[categoryId].Where(ccvm => ccvm.SkeywordList.Contains(attrValue) && ccvm.CompareAttributeID == titleId).FirstOrDefault();

                                            if (ccm != null)
                                            {
                                                attrValue = ccm.Value;
                                                isValid = true;
                                            }
                                            else
                                            {
                                                var ccm2 = CategoryCompareAttributeValueMapDic_Static[categoryId].Where(ccvm => ccvm.Value.Equals(attrValue, StringComparison.InvariantCultureIgnoreCase) && ccvm.CompareAttributeID == titleId).FirstOrDefault();
                                                if (ccm2 != null)
                                                {
                                                    isValid = true;
                                                }
                                            }

                                            if (isFloatValue && !isValid)
                                            {
                                                attrValue = floatValue.ToString("0.0");
                                                ccm = CategoryCompareAttributeValueMapDic_Static[categoryId].Where(ccvm => ccvm.SkeywordList.Contains(attrValue) && ccvm.CompareAttributeID == titleId).FirstOrDefault();

                                                if (ccm != null)
                                                {
                                                    attrValue = ccm.Value;
                                                    isValid = true;
                                                }
                                                else
                                                {
                                                    var ccm2 = CategoryCompareAttributeValueMapDic_Static[categoryId].Where(ccvm => ccvm.Value.Equals(attrValue, StringComparison.InvariantCultureIgnoreCase) && ccvm.CompareAttributeID == titleId).FirstOrDefault();
                                                    if (ccm2 != null)
                                                    {
                                                        isValid = true;
                                                    }
                                                }
                                            }

                                            if (isValid)
                                            {
                                                ProductCompareAttributeInfo productCompareAttributeInfo = productAllAttributesInfo.ProductCompareAttributeList.Where(pc => pc.CompareAttributeID == titleId).FirstOrDefault();
                                                if (productCompareAttributeInfo == null)
                                                {
                                                    InsertProductCompareAttribute(productId, priceMeCompareAttrTitle, attrValue);

                                                    AttributeMatchedReport attributeMatchedReport = new AttributeMatchedReport(retailerId, categoryId, productId, arm.AttributeType, priceMeCompareAttrTitle.CompareAttributeId, priceMeCompareAttrTitle.Name, arm.RetailerAttributeName, attrDic[attrTitle], attrValue, true, true);
                                                    WriteMatchedReport(attributeMatchedReport);
                                                }
                                                else
                                                {
                                                    //UpdateProductCompareAttribute(productId, priceMeCompareAttrTitle, attrValue);

                                                    AttributeMatchedReport attributeMatchedReport = new AttributeMatchedReport(retailerId, categoryId, productId, arm.AttributeType, priceMeCompareAttrTitle.CompareAttributeId, priceMeCompareAttrTitle.Name, arm.RetailerAttributeName, attrDic[attrTitle], attrValue, false, true);
                                                    WriteMatchedReport(attributeMatchedReport);
                                                }

                                                MatchLogWriter_Static.WriteLine("PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + arm.PM_AttributeID + " AttributeName : " + arm.RetailerAttributeName + " AttributeValue : " + attrValue);
                                            }
                                            else
                                            {
                                                UnMatchLogWriter_Static.WriteLine("Not in CompareAttributeValueMap. PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + arm.PM_AttributeID + " AttributeName : " + arm.RetailerAttributeName + " AttributeValue : " + attrDic[attrTitle] + " AttributeValue(Fixed) : " + attrValue);

                                                AttributeUnmatchedReport attributeUnmatchedReport = new AttributeUnmatchedReport(retailerId, categoryId, productId, arm.AttributeType, titleId, priceMeCompareAttrTitle.Name, arm.RetailerAttributeName, attrDic[attrTitle], attrValue, true);
                                                WriteUnMatchedReport(attributeUnmatchedReport);
                                            }
                                        }
                                        else
                                        {
                                            UnMatchLogWriter_Static.WriteLine("Not in CompareAttributeValueMap. PId : " + productId + " CId : " + categoryId + " RId : " + retailerId + " AttributeType : " + arm.AttributeType + " AttributeTitle : " + arm.PM_AttributeID + " AttributeName : " + arm.RetailerAttributeName + " AttributeValue : " + attrDic[attrTitle] + " AttributeValue(Fixed) : " + attrValue);

                                            AttributeUnmatchedReport attributeUnmatchedReport = new AttributeUnmatchedReport(retailerId, categoryId, productId, arm.AttributeType, titleId, priceMeCompareAttrTitle.Name, arm.RetailerAttributeName, attrDic[attrTitle], attrValue, true);
                                            WriteUnMatchedReport(attributeUnmatchedReport);
                                        }
                                    }
                                    else
                                    {
                                        UnMatchLogWriter_Static.WriteLine("Unknow attribute type : " + arm.AttributeType + " ProductId : " + productId);
                                    }
                                }
                                else
                                {
                                    UnMatchLogWriter_Static.WriteLine("Invalid is PM_AttributeID. PM_AttributeID : " + arm.PM_AttributeID + " ProductId : " + productId);
                                }
                            }
                        }
                        else
                        {
                            UnMatchLogWriter_Static.WriteLine("AttributeRetailerMap is null. Tilte : " + attrTitle + " ProductId : " + productId);
                        }
                    }
                    catch (Exception ex)
                    {
                        UnMatchLogWriter_Static.WriteLine("Exception... ProductId : " + productId);
                        UnMatchLogWriter_Static.WriteLine(ex.Message + " \t " + ex.StackTrace);
                    }
                }
            }
            else
            {
                UnMatchLogWriter_Static.WriteLine("AttributeRetailerMap is null. RetailerId : " + retailerId + " ProductId : " + productId);
            }
        }

        private static void UpdateProductWHL(int productId, float width, float height, float length)
        {
            string updateSql = "UPDATE [dbo].[CSK_Store_Product] SET [Width] = " + width.ToString("0.00") + ",[Height] = " + height.ToString("0.00") + ",[Length] = " + length.ToString("0.00") + " WHERE [ProductID] = " + productId;
            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(updateSql, sqlConn))
                {
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateProductWidth(int productId, float widthFloat)
        {
            string updateSql = "UPDATE [dbo].[CSK_Store_Product] SET [Width] = " + widthFloat.ToString("0.00") + " WHERE [ProductID] = " + productId;
            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(updateSql, sqlConn))
                {
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateProductHeight(int productId, float heightFloat)
        {
            string updateSql = "UPDATE [dbo].[CSK_Store_Product] SET [Height] = " + heightFloat.ToString("0.00") + " WHERE [ProductID] = " + productId;
            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(updateSql, sqlConn))
                {
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateProductLength(int productId, float lengthFloat)
        {
            string updateSql = "UPDATE [dbo].[CSK_Store_Product] SET [Length] = " + lengthFloat.ToString("0.00") + " WHERE [ProductID] = " + productId;
            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(updateSql, sqlConn))
                {
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateProductWeight(int productId, float weightFloat)
        {
            string updateSql = "UPDATE [dbo].[CSK_Store_Product] SET [Weight] = " + weightFloat.ToString("0.00") + " WHERE [ProductID] = " + productId;
            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(updateSql, sqlConn))
                {
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private static float ChangeUnit(float weightFloat, string fromUnit, string toUnit)
        {
            if (fromUnit.Equals("g", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("kg", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat / 1000f;
            }
            else if (fromUnit.Equals("kg", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("g", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat * 1000f;
            }
            else if (fromUnit.Equals("mm", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("cm", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat / 10f;
            }
            else if (fromUnit.Equals("cm", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("mm", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat * 10f;
            }
            else if (fromUnit.Equals("cm", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("m", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat / 100f;
            }
            else if (fromUnit.Equals("m", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("cm", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat * 100f;
            }
            else if (fromUnit.Equals("mm", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("m", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat / 1000f;
            }
            else if (fromUnit.Equals("m", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("mm", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat * 1000f;
            }
            else if (fromUnit.Equals("ml", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("l", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat / 1000f;
            }
            else if (fromUnit.Equals("l", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("ml", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat * 1000f;
            }
            else if (fromUnit.Equals("kb", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("mb", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat / 1024f;
            }
            else if (fromUnit.Equals("mb", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("kb", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat * 1024f;
            }
            else if (fromUnit.Equals("mb", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("gb", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat / 1024f;
            }
            else if (fromUnit.Equals("gb", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("mb", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat * 1024f;
            }
            else if (fromUnit.Equals("gb", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("tb", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat / 1024f;
            }
            else if (fromUnit.Equals("tb", StringComparison.InvariantCultureIgnoreCase) && toUnit.Equals("gb", StringComparison.InvariantCultureIgnoreCase))
            {
                return weightFloat * 1024f;
            }
            else
            {
                Console.WriteLine("No change unit from : " + fromUnit + " to :" + toUnit);
                return weightFloat;
            }
        }

        /// <summary>
        /// 处理value的值
        /// </summary>
        /// <param name="attrValue"></param>
        /// <param name="arm"></param>
        /// <returns></returns>
        private static string FixValue(string attrValue, AttributeRetailerMap arm, int retailerId, int categoryId, Dictionary<string, string> attrDic)
        {
            if (arm.IsRemove)
            {
                attrValue = attrValue.Replace(arm.RemoveKeword, "").Replace("  ", " ").Trim();
            }
            if (arm.IsSplit)
            {
                string[] vs = attrValue.Split(new string[] { arm.SplitKeyword }, StringSplitOptions.RemoveEmptyEntries);
                if (arm.KeepBefore)
                {
                    attrValue = vs[0].Trim();
                }
                else
                {
                    if (vs.Length == 1)
                    {
                        return null;
                    }
                    attrValue = vs[1].Trim();
                }
            }
            if (arm.IsCombine)
            {
                if (attrDic.ContainsKey(arm.CombineAttribute))
                {
                    string combineValue = FixValue2(attrDic[arm.CombineAttribute], arm, retailerId, categoryId);
                    if (arm.KeepCombineAttributeBefore)
                    {
                        attrValue = combineValue + " " + attrValue;
                    }
                    else
                    {
                        attrValue = attrValue + " " + combineValue;
                    }
                }
                else
                {
                    return null;
                }
            }

            attrValue = AttributeRetailerGeneralHelper_Static.FixAttributeValue(attrValue, retailerId, categoryId).Trim();
            return attrValue;
        }

        /// <summary>
        /// 处理value的值,不考虑IsSplit和IsCombine
        /// </summary>
        /// <param name="attrValue"></param>
        /// <param name="arm"></param>
        /// <returns></returns>
        private static string FixValue2(string attrValue, AttributeRetailerMap arm, int retailerId, int categoryId)
        {
            if (arm.IsRemove)
            {
                attrValue = attrValue.Replace(arm.RemoveKeword, "").Replace("  ", " ").Trim();
            }

            attrValue = AttributeRetailerGeneralHelper_Static.FixAttributeValue(attrValue, retailerId, categoryId).Trim();
            return attrValue;
        }

        private static void UpdateProductCompareAttribute(int productId, CompareAttributeInfo priceMeCompareAttrTitle, string attrValue)
        {
            string updateSql = @"UPDATE [dbo].[Store_Compare_Attribute_Map]
                                 SET[CompareValue] = @valueName
                                 ,[Modifiedon] = @dateNow
                                 ,[Modifiedby] = 'ImportAttributeTool'
                                  WHERE [ProductID] = " + productId + " and [CompareAttributeID] = " + priceMeCompareAttrTitle.CompareAttributeId;

            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(updateSql, sqlConn))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@valueName", attrValue));
                    sqlCmd.Parameters.Add(new SqlParameter("@dateNow", DateTime.Now));
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private static void InsertProductCompareAttribute(int productId, CompareAttributeInfo priceMeCompareAttrTitle, string attrValue)
        {
            string insertSql = @"INSERT INTO [dbo].[Store_Compare_Attribute_Map]
                                   ([CompareAttributeID]
                                   ,[CompareValue]
                                   ,[ProductID]
                                   ,[Createdon]
                                   ,[Createdby])
                                    VALUES
                                   (@cAttrId
                                   ,@valueName
                                   ,@productId
                                   ,@dateNow
                                   ,'ImportAttributeTool')";

            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(insertSql, sqlConn))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@cAttrId", priceMeCompareAttrTitle.CompareAttributeId));
                    sqlCmd.Parameters.Add(new SqlParameter("@valueName", attrValue));
                    sqlCmd.Parameters.Add(new SqlParameter("@productId", productId));
                    sqlCmd.Parameters.Add(new SqlParameter("@dateNow", DateTime.Now));
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private static AttributeValueInfo InsertAttributeValue(AttributeTitleInfo priceMeAttrTitle, string attrValue)
        {
            AttributeValueInfo attributeValueInfo = null;
            int newValueId = 0;
            string insertSql = @"INSERT INTO [dbo].[CSK_Store_AttributeValue]
                                   ([AttributeTitleID]
                                   ,[Value]
                                   ,[CreatedOn]
                                   ,[CreatedBy]
                                   ,[PopularAttribute]
                                   ,[ListOrder])
                                    VALUES
                                   (@titleId
                                   ,@valueName
                                   ,@dateNow
                                   ,'ImportAttributeTool'
                                   ,0
                                   ,0)
                                    select ident_current('CSK_Store_AttributeValue')";

            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();

                using (SqlCommand sqlCmd = new SqlCommand(insertSql, sqlConn))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@titleId", priceMeAttrTitle.TitleId));
                    sqlCmd.Parameters.Add(new SqlParameter("@valueName", attrValue));
                    sqlCmd.Parameters.Add(new SqlParameter("@dateNow", DateTime.Now));

                    newValueId = Convert.ToInt32(sqlCmd.ExecuteScalar());
                }

                string selectPriceMeAttributeValueSql = "SELECT [AttributeValueID],[AttributeTitleID],[Value],[PopularAttribute],[ListOrder] FROM [dbo].[CSK_Store_AttributeValue] where AttributeValueID = " + newValueId;
                using (SqlCommand sqlCmd = new SqlCommand(selectPriceMeAttributeValueSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.Read())
                        {
                            attributeValueInfo = new AttributeValueInfo();
                            attributeValueInfo.AttributeValueId = sqlDr.GetInt32(0);
                            attributeValueInfo.TitleId = sqlDr.GetInt32(1);
                            if (!sqlDr.IsDBNull(2))
                            {
                                attributeValueInfo.Value = sqlDr.GetString(2);
                            }
                            if (!sqlDr.IsDBNull(3))
                            {
                                attributeValueInfo.PopularAttribute = sqlDr.GetBoolean(3);
                            }
                            if (!sqlDr.IsDBNull(4))
                            {
                                attributeValueInfo.ListOrder = sqlDr.GetInt32(4);
                            }

                            if (AttributeValueListDic_Static.ContainsKey(attributeValueInfo.TitleId))
                            {
                                AttributeValueListDic_Static[attributeValueInfo.TitleId].Add(attributeValueInfo);
                            }
                            else
                            {
                                List<AttributeValueInfo> list = new List<AttributeValueInfo>();
                                list.Add(attributeValueInfo);
                                AttributeValueListDic_Static.Add(attributeValueInfo.TitleId, list);
                            }
                        }
                    }
                }
            }
            return attributeValueInfo;
        }

        private static void InsertProductAttribute(int productId, AttributeTitleInfo priceMeAttrTitle, AttributeValueInfo newAttributeValue)
        {
            string insertSql = @"INSERT INTO [dbo].[CSK_Store_ProductDescriptor]
                                ([ProductID]
                                ,[Title]
                                ,[ProductDescriptorName]
                                ,[IsBulletedList]
                                ,[ListOrder]
                                ,[TypeID]
                                ,[AttributeValueID]
                                ,[ModifiedBy])
                                VALUES
                                (@productId
                                ,@titleName
                                ,@valueName
                                ,0
                                ,0
                                ,@titleId
                                ,@valueId
                                ,'ImportAttributeTool')";

            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();

                using (SqlCommand sqlCmd = new SqlCommand(insertSql, sqlConn))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@productId", productId));
                    sqlCmd.Parameters.Add(new SqlParameter("@titleName", priceMeAttrTitle.Title));
                    sqlCmd.Parameters.Add(new SqlParameter("@valueName", newAttributeValue.Value));
                    sqlCmd.Parameters.Add(new SqlParameter("@titleId", priceMeAttrTitle.TitleId));
                    sqlCmd.Parameters.Add(new SqlParameter("@valueId", newAttributeValue.AttributeValueId));
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateProductAttribute(int productId, ProductAttributeInfo productAttributeInfo, AttributeValueInfo newAttributeValue)
        {
            string updateSql = @"UPDATE [dbo].[CSK_Store_ProductDescriptor]
                                 SET [ProductDescriptorName] = @valueName
                                    ,[AttributeValueID] = @valueId
                                    ,[ModifiedBy] = 'ImportAttributeTool' WHERE ProductID = " + productId + " and ProductDescriptorID = " + productAttributeInfo.Id;

            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(updateSql, sqlConn))
                {
                    sqlCmd.Parameters.Add(new SqlParameter("@valueName", newAttributeValue.Value));
                    sqlCmd.Parameters.Add(new SqlParameter("@valueId", newAttributeValue.AttributeValueId));
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private static ProductAllAttributesInfo GetProductAllAttributesInfo(int productId)
        {
            ProductAllAttributesInfo paai = new ProductAllAttributesInfo();
            paai.ProductId = productId;

            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();

                List<ProductAttributeInfo> list = new List<ProductAttributeInfo>();
                string selectProductAttributesSql = "SELECT [ProductDescriptorID],[TypeID],[AttributeValueID] FROM [dbo].[CSK_Store_ProductDescriptor] where [ProductID] = " + productId;
                using (SqlCommand sqlCmd = new SqlCommand(selectProductAttributesSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            ProductAttributeInfo pai = new ProductAttributeInfo();
                            pai.Id = sqlDr.GetInt32(0);
                            pai.AttributeTitleId = sqlDr.GetInt32(1);
                            pai.AttributeValueId = sqlDr.GetInt32(2);
                            list.Add(pai);
                        }
                    }
                    paai.ProductAttributeList = list;
                }

                List<ProductCompareAttributeInfo> list2 = new List<ProductCompareAttributeInfo>();
                string selectProductCompareAttributesSql = "SELECT [ID],[CompareAttributeID],[CompareValue] FROM [dbo].[Store_Compare_Attribute_Map] where ProductID = " + productId;
                using (SqlCommand sqlCmd = new SqlCommand(selectProductCompareAttributesSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            ProductCompareAttributeInfo pcai = new ProductCompareAttributeInfo();
                            pcai.Id = sqlDr.GetInt32(0);
                            pcai.CompareAttributeID = sqlDr.GetInt32(1);
                            pcai.CompareValue = sqlDr.GetString(2);
                            list2.Add(pcai);
                        }
                    }
                    paai.ProductCompareAttributeList = list2;
                }
            }


            return paai;
        }

        static void WriteMatchedReport(AttributeMatchedReport attributeMatchedReport)
        {
            string selectSql = @"SELECT [ID] FROM [dbo].[AttributeMatchedReport]
                                 where RID = " + attributeMatchedReport.RID + " and PID = " + attributeMatchedReport.PID + " and CID = " + attributeMatchedReport.CID + " and AttType = " + attributeMatchedReport.AttType + " and AttTitleID = " + attributeMatchedReport.AttTitleID;

            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();

                bool exist = false;
                using (SqlCommand sqlCmd = new SqlCommand(selectSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if (sqlDr.Read())
                        {
                            exist = true;
                        }
                    }
                }

                if (!exist)
                {
                    string insertSql = @"INSERT INTO [dbo].[AttributeMatchedReport]
                               ([RID]
                               ,[CID]
                               ,[PID]
                               ,[AttType]
                               ,[AttTitleID]
                               ,[PM_AttName]
                               ,[DR_AttName]
                               ,[DR_AttValue_Orignal]
                               ,[DR_AttValue_Changed]
                               ,[AutoImport]
                               ,[SaveToDB]
                               ,[CreatedBy]
                               ,[CreatedOn])
                                VALUES
                               (@rId
                               ,@cId
                               ,@pId
                               ,@attType
                               ,@attTitleId
                               ,@pmAttName
                               ,@drAttName
                               ,@drAttValueOrignal
                               ,@drAttValueChanged
                               ,@autoImport
                               ,@saveToDB
                               ,'ImportAttributeTool'
                               ,@createdOn)";


                    using (SqlCommand sqlCmd = new SqlCommand(insertSql, sqlConn))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("@rId", attributeMatchedReport.RID));
                        sqlCmd.Parameters.Add(new SqlParameter("@cId", attributeMatchedReport.CID));
                        sqlCmd.Parameters.Add(new SqlParameter("@pId", attributeMatchedReport.PID));
                        sqlCmd.Parameters.Add(new SqlParameter("@attType", attributeMatchedReport.AttType));
                        sqlCmd.Parameters.Add(new SqlParameter("@attTitleId", attributeMatchedReport.AttTitleID));
                        sqlCmd.Parameters.Add(new SqlParameter("@pmAttName", attributeMatchedReport.PM_AttName));
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttName", attributeMatchedReport.DR_AttName));
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttValueOrignal", attributeMatchedReport.DR_AttValue_Orignal));
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttValueChanged", attributeMatchedReport.DR_AttValue_Changed));
                        sqlCmd.Parameters.Add(new SqlParameter("@autoImport", attributeMatchedReport.AutoImport));
                        sqlCmd.Parameters.Add(new SqlParameter("@saveToDB", attributeMatchedReport.SaveToDB));
                        sqlCmd.Parameters.Add(new SqlParameter("@createdOn", DateTime.Now));
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string updateSql = @"UPDATE [dbo].[AttributeMatchedReport]
                                         SET [DR_AttName] = @drAttName
                                            ,[DR_AttValue_Orignal] = @drAttValueOrignal
                                            ,[DR_AttValue_Changed] = @drAttValueChanged
                                            where RID = " + attributeMatchedReport.RID + " and PID = " + attributeMatchedReport.PID + " and CID = " + attributeMatchedReport.CID + " and AttType = " + attributeMatchedReport.AttType + " and AttTitleID = " + attributeMatchedReport.AttTitleID;

                    using (SqlCommand sqlCmd = new SqlCommand(updateSql, sqlConn))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttName", attributeMatchedReport.DR_AttName));
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttValueOrignal", attributeMatchedReport.DR_AttValue_Orignal));
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttValueChanged", attributeMatchedReport.DR_AttValue_Changed));
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        static void WriteUnMatchedReport(AttributeUnmatchedReport attributeUnmatchedReport)
        {
            string selectSql = @"SELECT [ID] FROM [dbo].[AttributeUnmatchedReport]
                                 where RID = " + attributeUnmatchedReport.RID + " and PID = " + attributeUnmatchedReport.PID + " and CID = " + attributeUnmatchedReport.CID + " and AttType = " + attributeUnmatchedReport.AttType + " and AttTitleID = " + attributeUnmatchedReport.AttTitleID;

            using (SqlConnection sqlConn = GetDBConnection())
            {
                sqlConn.Open();

                bool exist = false;
                using (SqlCommand sqlCmd = new SqlCommand(selectSql, sqlConn))
                {
                    using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                    {
                        if(sqlDr.Read())
                        {
                            exist = true;
                        }
                    }
                }

                if (!exist)
                {
                    string insertSql = @"INSERT INTO [dbo].[AttributeUnmatchedReport]
                               ([RID]
                               ,[CID]
                               ,[PID]
                               ,[AttType]
                               ,[AttTitleID]
                               ,[PM_AttName]
                               ,[DR_AttName]
                               ,[DR_AttValue_Orignal]
                               ,[DR_AttValue_Changed]
                               ,[Status])
                                VALUES
                               (@rId
                               ,@cId
                               ,@pId
                               ,@attType
                               ,@attTitleId
                               ,@pmAttName
                               ,@drAttName
                               ,@drAttValueOrignal
                               ,@drAttValueChanged
                               ,@status)";


                    using (SqlCommand sqlCmd = new SqlCommand(insertSql, sqlConn))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("@rId", attributeUnmatchedReport.RID));
                        sqlCmd.Parameters.Add(new SqlParameter("@cId", attributeUnmatchedReport.CID));
                        sqlCmd.Parameters.Add(new SqlParameter("@pId", attributeUnmatchedReport.PID));
                        sqlCmd.Parameters.Add(new SqlParameter("@attType", attributeUnmatchedReport.AttType));
                        sqlCmd.Parameters.Add(new SqlParameter("@attTitleId", attributeUnmatchedReport.AttTitleID));
                        sqlCmd.Parameters.Add(new SqlParameter("@pmAttName", attributeUnmatchedReport.PM_AttName));
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttName", attributeUnmatchedReport.DR_AttName));
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttValueOrignal", attributeUnmatchedReport.DR_AttValue_Orignal));
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttValueChanged", attributeUnmatchedReport.DR_AttValue_Changed));
                        sqlCmd.Parameters.Add(new SqlParameter("@status", attributeUnmatchedReport.Status));
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string updateSql = @"UPDATE [dbo].[AttributeUnmatchedReport]
                                         SET [DR_AttName] = @drAttName
                                            ,[DR_AttValue_Orignal] = @drAttValueOrignal
                                            ,[DR_AttValue_Changed] = @drAttValueChanged
                                            where RID = " + attributeUnmatchedReport.RID + " and PID = " + attributeUnmatchedReport.PID + " and CID = " + attributeUnmatchedReport.CID + " and AttType = " + attributeUnmatchedReport.AttType + " and AttTitleID = " + attributeUnmatchedReport.AttTitleID;

                    using (SqlCommand sqlCmd = new SqlCommand(updateSql, sqlConn))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttName", attributeUnmatchedReport.DR_AttName));
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttValueOrignal", attributeUnmatchedReport.DR_AttValue_Orignal));
                        sqlCmd.Parameters.Add(new SqlParameter("@drAttValueChanged", attributeUnmatchedReport.DR_AttValue_Changed));
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
        }
    
    }
}