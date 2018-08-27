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
        }
        public static string FtpTargetLuceneConfigPath { get; set; }
        public static string FtpTargetLuceneIndexRootPath { get; set; }
        public static string FtpLuceneConfigFileCopyDir { get; set; }
        public static string FtpLuceneConfigFileName { get; set; }

        public static string FtpUserID { get; set; }
        public static string FtpPassword { get; set; }
        public static string FtpTargetIP { get; set; }
        public static string FtpTargetPath { get; set; }
        public static bool ToFTP { get; set; }
        public static string LocalLuceneConfigPath { get; set; }
        public static string LuceneKey { get; set; }
        public static int PrevPriceDay { get; set; }
        public static string HiddenManufacturerCategoryIDs { get; set; }
        public static int ThreadCount { get; set; }
        public static string Currencies { get; set; }
        public static bool OnlyPPC { get; set; }
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
    }
}
