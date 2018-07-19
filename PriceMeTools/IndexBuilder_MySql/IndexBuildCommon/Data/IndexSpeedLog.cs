using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndexBuildCommon.Data
{
    public class IndexSpeedLog
    {
        public Dictionary<string, BulidIndexSpeedInfo> CategoriesProductBuildSpeedInfoDictionary = new Dictionary<string, BulidIndexSpeedInfo>();
        public BulidIndexSpeedInfo CategoriesBulidIndexSpeedInfo;
        public BulidIndexSpeedInfo RetailerProductsBulidIndexSpeedInfo;
        public BulidIndexSpeedInfo ProductRetailerMapBulidIndexSpeedInfo;
        public BulidIndexSpeedInfo ProductsAttributesBulidIndexSpeedInfo;
        public BulidIndexSpeedInfo RetailerBulidIndexSpeedInfo;
        public BulidIndexSpeedInfo UpdateRetailerTrackerSpeedInfo;
        public BulidIndexSpeedInfo UpdateProductRatingSpeedInfo;
        public BulidIndexSpeedInfo UpdateProductCategorySpeedInfo;
        public BulidIndexSpeedInfo BuildVelocitySpeedInfo;
        public BulidIndexSpeedInfo UpdateRelatedManufacturerCategoriesSpeedInfo;

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();

            if (UpdateRetailerTrackerSpeedInfo != null)
            {
                strBuilder.AppendLine("--- UpdateRetailerTrackerSpeedInfo ---");
                strBuilder.AppendLine("start : " + UpdateRetailerTrackerSpeedInfo.StartReadDBTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("end : " + UpdateRetailerTrackerSpeedInfo.EndReadDBTime.ToString("HH:mm:ss"));
                var span = UpdateRetailerTrackerSpeedInfo.EndReadDBTime - UpdateRetailerTrackerSpeedInfo.StartReadDBTime;
                strBuilder.AppendLine(span.ToString());
            }

            if (UpdateProductRatingSpeedInfo != null)
            {
                strBuilder.AppendLine("--- UpdateProductRatingSpeedInfo ---");
                strBuilder.AppendLine("start : " + UpdateProductRatingSpeedInfo.StartReadDBTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("end : " + UpdateProductRatingSpeedInfo.EndReadDBTime.ToString("HH:mm:ss"));
                var span = UpdateProductRatingSpeedInfo.EndReadDBTime - UpdateProductRatingSpeedInfo.StartReadDBTime;
                strBuilder.AppendLine(span.ToString());
            }

            if (UpdateProductCategorySpeedInfo != null)
            {
                strBuilder.AppendLine("--- UpdateProductCategorySpeedInfo ---");
                strBuilder.AppendLine("start : " + UpdateProductCategorySpeedInfo.StartReadDBTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("end : " + UpdateProductCategorySpeedInfo.EndReadDBTime.ToString("HH:mm:ss"));
                var span = UpdateProductCategorySpeedInfo.EndReadDBTime - UpdateProductCategorySpeedInfo.StartReadDBTime;
                strBuilder.AppendLine(span.ToString());
            }

            if (CategoriesProductBuildSpeedInfoDictionary != null && CategoriesProductBuildSpeedInfoDictionary.Count > 0)
            {
                strBuilder.AppendLine("------------------------------------------");
                TimeSpan dbTimeSpan = new TimeSpan();
                TimeSpan indexTimeSpan = new TimeSpan();
                foreach (string key in CategoriesProductBuildSpeedInfoDictionary.Keys)
                {
                    strBuilder.AppendLine("--- " + key + "---");
                    BulidIndexSpeedInfo bulidIndexSpeedInfo = CategoriesProductBuildSpeedInfoDictionary[key];

                    strBuilder.AppendLine("DB start : " + bulidIndexSpeedInfo.StartReadDBTime.ToString("HH:mm:ss"));
                    strBuilder.AppendLine("DB end : " + bulidIndexSpeedInfo.EndReadDBTime.ToString("HH:mm:ss"));
                    var dbSpan = bulidIndexSpeedInfo.EndReadDBTime - bulidIndexSpeedInfo.StartReadDBTime;
                    dbTimeSpan += dbSpan;

                    strBuilder.AppendLine("Index start : " + bulidIndexSpeedInfo.StartWriteIndexTime.ToString("HH:mm:ss"));
                    strBuilder.AppendLine("Index end : " + bulidIndexSpeedInfo.EndWriteIndexTime.ToString("HH:mm:ss"));
                    var indexSpan = bulidIndexSpeedInfo.EndWriteIndexTime - bulidIndexSpeedInfo.StartWriteIndexTime;
                    indexTimeSpan += indexSpan;

                    strBuilder.AppendLine("DB : " + dbSpan.ToString());
                    strBuilder.AppendLine("Index : " + indexSpan.ToString());
                }

                strBuilder.AppendLine("---");
                strBuilder.AppendLine("DB : " + dbTimeSpan.ToString());
                strBuilder.AppendLine("Index : " + indexTimeSpan.ToString());
                strBuilder.AppendLine("------------------------------------------");
            }

            if (CategoriesBulidIndexSpeedInfo != null)
            {
                strBuilder.AppendLine("--- CategoriesBulidIndexSpeedInfo ---");
                strBuilder.AppendLine("DB start : " + CategoriesBulidIndexSpeedInfo.StartReadDBTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("DB end : " + CategoriesBulidIndexSpeedInfo.EndReadDBTime.ToString("HH:mm:ss"));
                var dbSpan = CategoriesBulidIndexSpeedInfo.EndReadDBTime - CategoriesBulidIndexSpeedInfo.StartReadDBTime;

                strBuilder.AppendLine("Index start : " + CategoriesBulidIndexSpeedInfo.StartWriteIndexTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("Index end : " + CategoriesBulidIndexSpeedInfo.EndWriteIndexTime.ToString("HH:mm:ss"));
                var indexSpan = CategoriesBulidIndexSpeedInfo.EndWriteIndexTime - CategoriesBulidIndexSpeedInfo.StartWriteIndexTime;

                strBuilder.AppendLine("DB : " + dbSpan.ToString());
                strBuilder.AppendLine("Index : " + indexSpan.ToString());
            }

            if (RetailerProductsBulidIndexSpeedInfo != null)
            {
                strBuilder.AppendLine("--- RetailerProductsBulidIndexSpeedInfo ---");

                strBuilder.AppendLine("DB start : " + RetailerProductsBulidIndexSpeedInfo.StartReadDBTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("DB end : " + RetailerProductsBulidIndexSpeedInfo.EndReadDBTime.ToString("HH:mm:ss"));
                var dbSpan = RetailerProductsBulidIndexSpeedInfo.EndReadDBTime - RetailerProductsBulidIndexSpeedInfo.StartReadDBTime;

                strBuilder.AppendLine("Index start : " + RetailerProductsBulidIndexSpeedInfo.StartWriteIndexTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("Index end : " + RetailerProductsBulidIndexSpeedInfo.EndWriteIndexTime.ToString("HH:mm:ss"));
                var indexSpan = RetailerProductsBulidIndexSpeedInfo.EndWriteIndexTime - RetailerProductsBulidIndexSpeedInfo.StartWriteIndexTime;

                strBuilder.AppendLine("DB : " + dbSpan.ToString());
                strBuilder.AppendLine("Index : " + indexSpan.ToString());
            }

            if (ProductRetailerMapBulidIndexSpeedInfo != null)
            {
                strBuilder.AppendLine("--- ProductRetailerMapBulidIndexSpeedInfo ---");

                strBuilder.AppendLine("DB start : " + ProductRetailerMapBulidIndexSpeedInfo.StartReadDBTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("DB end : " + ProductRetailerMapBulidIndexSpeedInfo.EndReadDBTime.ToString("HH:mm:ss"));
                var dbSpan = ProductRetailerMapBulidIndexSpeedInfo.EndReadDBTime - ProductRetailerMapBulidIndexSpeedInfo.StartReadDBTime;

                strBuilder.AppendLine("Index start : " + ProductRetailerMapBulidIndexSpeedInfo.StartWriteIndexTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("Index end : " + ProductRetailerMapBulidIndexSpeedInfo.EndWriteIndexTime.ToString("HH:mm:ss"));
                var indexSpan = ProductRetailerMapBulidIndexSpeedInfo.EndWriteIndexTime - ProductRetailerMapBulidIndexSpeedInfo.StartWriteIndexTime;

                strBuilder.AppendLine("DB : " + dbSpan.ToString());
                strBuilder.AppendLine("Index : " + indexSpan.ToString());
            }

            if (ProductsAttributesBulidIndexSpeedInfo != null)
            {
                strBuilder.AppendLine("--- ProductsAttributesBulidIndexSpeedInfo ---");

                strBuilder.AppendLine("DB start : " + ProductsAttributesBulidIndexSpeedInfo.StartReadDBTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("DB end : " + ProductsAttributesBulidIndexSpeedInfo.EndReadDBTime.ToString("HH:mm:ss"));
                var dbSpan = ProductsAttributesBulidIndexSpeedInfo.EndReadDBTime - ProductsAttributesBulidIndexSpeedInfo.StartReadDBTime;

                strBuilder.AppendLine("Index start : " + ProductsAttributesBulidIndexSpeedInfo.StartWriteIndexTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("Index end : " + ProductsAttributesBulidIndexSpeedInfo.EndWriteIndexTime.ToString("HH:mm:ss"));
                var indexSpan = ProductsAttributesBulidIndexSpeedInfo.EndWriteIndexTime - ProductsAttributesBulidIndexSpeedInfo.StartWriteIndexTime;

                strBuilder.AppendLine("DB : " + dbSpan.ToString());
                strBuilder.AppendLine("Index : " + indexSpan.ToString());
            }

            if (BuildVelocitySpeedInfo != null)
            {
                strBuilder.AppendLine("--- BuildVelocitySpeedInfo ---");
                strBuilder.AppendLine("start : " + BuildVelocitySpeedInfo.StartReadDBTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("end : " + BuildVelocitySpeedInfo.EndReadDBTime.ToString("HH:mm:ss"));
                var span = BuildVelocitySpeedInfo.EndReadDBTime - BuildVelocitySpeedInfo.StartReadDBTime;
                strBuilder.AppendLine(span.ToString());
            }

            if (UpdateRelatedManufacturerCategoriesSpeedInfo != null)
            {
                strBuilder.AppendLine("--- UpdateRelatedManufacturerCategoriesSpeedInfo ---");
                strBuilder.AppendLine("start : " + UpdateRelatedManufacturerCategoriesSpeedInfo.StartReadDBTime.ToString("HH:mm:ss"));
                strBuilder.AppendLine("end : " + UpdateRelatedManufacturerCategoriesSpeedInfo.EndReadDBTime.ToString("HH:mm:ss"));
                var span = UpdateRelatedManufacturerCategoriesSpeedInfo.EndReadDBTime - UpdateRelatedManufacturerCategoriesSpeedInfo.StartReadDBTime;
                strBuilder.AppendLine(span.ToString());
            }

            return strBuilder.ToString();
        }
    }
}
