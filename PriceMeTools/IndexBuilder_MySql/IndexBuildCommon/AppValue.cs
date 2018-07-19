using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndexBuildCommon
{
    public static class AppValue
    {
        static string _updateDataBase = System.Configuration.ConfigurationManager.AppSettings["UpdateDataBase"];
        static int _fixPriceFlag = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FixPriceFlag"]);
        static decimal _priceLimitPercent = decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["PriceLimitPercent"]);
        static int _clickBoost = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ClickBoost"]);
        static float _unlimitedPPC = float.Parse(System.Configuration.ConfigurationManager.AppSettings["UnlimitedPPC"]);

        private static bool _updateProductCategory = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UpdateProductCategory"]);

        public static string IndexRootPath { get; private set; }
        public static string NotCopy { get; private set; }
        public static int FlagHour { get; private set; }
        public static int FlagVelocityHour { get; private set; }
        public static int CountryId { get; private set; }

        static AppValue()
        {
            IndexRootPath = System.Configuration.ConfigurationManager.AppSettings["IndexRootPath"];
            FlagHour = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FlagHour"]);
            FlagVelocityHour = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FlagVelocityHour"]);
            CountryId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CountryID"]);

            NotCopy = System.Configuration.ConfigurationManager.AppSettings["NotCopy"];
        }

        #region Value Get Property
        public static bool UpdateProductCategory
        {
            get { return AppValue._updateProductCategory; }
        }

        public static float UnlimitedPPC
        {
            get { return AppValue._unlimitedPPC; }
        }

        public static int ClickBoost
        {
            get { return AppValue._clickBoost; }
        }

        public static decimal PriceLimitPercent
        {
            get { return AppValue._priceLimitPercent; }
        }

        public static string UpdateDataBase
        {
            get { return AppValue._updateDataBase; }
        }

        public static int FixPriceFlag
        {
            get { return AppValue._fixPriceFlag; }
        }
        #endregion
    }
}