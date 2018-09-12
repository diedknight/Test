using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ProductSearchIndexBuilder
{
    public static class AppValue
    {
        public static void Init(IConfigurationRoot configuration)
        {
            UpdateDataBase = bool.Parse(configuration["UpdateDataBase"]);
            FixPriceFlag = int.Parse(configuration["FixPriceFlag"]);
            PriceLimitPercent = decimal.Parse(configuration["PriceLimitPercent"]);
            UnlimitedPPC = float.Parse(configuration["UnlimitedPPC"]);

            IndexRootPath = configuration["IndexRootPath"];
            FlagHour = int.Parse(configuration["FlagHour"]);
            FlagVelocityHour = int.Parse(configuration["FlagVelocityHour"]);
            CountryId = int.Parse(configuration["CountryID"]);

            NotCopy = configuration["NotCopy"];
            Email = configuration["Email"];
            InfoEmail = configuration["InfoEmail"];
            WebsiteID = int.Parse(configuration["WebsiteID"]);
            OnlyPPC = bool.Parse(configuration["OnlyPPC"]);
            Currencies = configuration["Currencies"];
            ThreadCount = int.Parse(configuration["ThreadCount"]);
            HiddenManufacturerCategoryIDs = configuration["HiddenManufacturerCategoryIDs"];
            PrevPriceDay = int.Parse(configuration["PrevPriceDay"]);
            LocalLuceneConfigPath = configuration["LocalLuceneConfigPath"];
            LuceneKey = configuration["LuceneKey"];

            ToFTP = bool.Parse(configuration["ToFTP"]);
            FtpUserID = configuration["FtpUserID"];
            FtpPassword = configuration["FtpPassword"];
            FtpTargetIP = configuration["FtpTargetIP"];
            FtpTargetPath = configuration["FtpTargetPath"];

            FtpTargetLuceneConfigPath = configuration["FtpTargetLuceneConfigPath"];
            FtpTargetLuceneIndexRootPath = configuration["FtpTargetLuceneIndexRootPath"];
            FtpLuceneConfigFileCopyDir = configuration["FtpLuceneConfigFileCopyDir"];
            FtpLuceneConfigFileName = configuration["FtpTargetLuceneConfigName"];
            RedisHost = configuration["RedisHost"];
            RedisName = configuration["RedisName"];

            string VersionNoEnglishCountryidString = configuration["VersionNoEnglishCountryid"];
            if (!String.IsNullOrEmpty(VersionNoEnglishCountryidString))
            {
                string[] versionNoEnglishCountryid = VersionNoEnglishCountryidString.Split(',');

                foreach (string countryId in versionNoEnglishCountryid)
                {
                    int cid = 0;
                    int.TryParse(countryId, out cid);
                    if (cid != 0)
                        ListVersionNoEnglishCountryId.Add(cid);
                }
            }

            ReviewStr = configuration["ReviewStr"];

            bool _isDebug;
            if (bool.TryParse(configuration["IsDebug"], out _isDebug))
                IsDebug = _isDebug;
        }

        public static bool IsDebug { get; private set; }
        public static string ReviewStr { get; private set; }
        public static List<int> ListVersionNoEnglishCountryId = new List<int>();
        public static string FtpTargetLuceneConfigPath { get; private set; }
        public static string FtpTargetLuceneIndexRootPath { get; private set; }
        public static string FtpLuceneConfigFileCopyDir { get; private set; }
        public static string FtpLuceneConfigFileName { get; private set; }

        public static string FtpUserID { get; private set; }
        public static string FtpPassword { get; private set; }
        public static string FtpTargetIP { get; private set; }
        public static string FtpTargetPath { get; private set; }
        public static bool ToFTP { get; private set; }
        public static string LocalLuceneConfigPath { get; private set; }
        public static string LuceneKey { get; private set; }
        public static int PrevPriceDay { get; private set; }
        public static string HiddenManufacturerCategoryIDs { get; private set; }
        public static int ThreadCount { get; private set; }
        public static string Currencies { get; private set; }
        public static bool OnlyPPC { get; private set; }
        public static int WebsiteID { get; private set; }
        public static string IndexRootPath { get; private set; }
        public static string NotCopy { get; private set; }
        public static int FlagHour { get; private set; }
        public static int FlagVelocityHour { get; private set; }
        public static int CountryId { get; private set; }

        public static bool UpdateProductCategory { get; private set; }

        public static float UnlimitedPPC { get; private set; }

        public static decimal PriceLimitPercent { get; private set; }

        public static bool UpdateDataBase { get; private set; }

        public static int FixPriceFlag { get; private set; }
        public static string Email { get; private set; }
        public static string InfoEmail { get; private set; }
        public static string RedisHost { get; private set; }
        public static string RedisName { get; private set; }
    }
}
