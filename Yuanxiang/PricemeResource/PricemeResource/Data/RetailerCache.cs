using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Data
{
    [Serializable]
    public class RetailerCache
    {
        #region

        public int RetailerId { get; set; }

        public string RetailerName { get; set; }

        public string TradingName { get; set; }

        public int RetailerCategory { get; set; }

        public int RetailerStatus { get; set; }

        public byte StoreType { get; set; }

        public string RetailerURL { get; set; }

        public string ContactFirstName { get; set; }

        public string YearEstablished { get; set; }

        public string RetailerContactName { get; set; }

        public string ContactLasName { get; set; }

        public string ContactTelephone { get; set; }

        public string ContactEmail { get; set; }

        public string BusinessMobile { get; set; }

        public string RetailerPhone { get; set; }

        public string RetailerFax { get; set; }

        public int RetailerCity { get; set; }

        public string VideoCode { get; set; }

        public int RetailerCountry { get; set; }

        public string RetailerCityDistrict { get; set; }

        public string RetailerShortDescription { get; set; }

        public string RetailerMessage { get; set; }

        public int RetailerImage { get; set; }

        public string Location { get; set; }

        public int RetailerRatingSum { get; set; }//

        public int RetailerTotalRatingVotes { get; set; }//

        public int RetailerCrawlerInfo { get; set; }

        public int RetailerPaymentType { get; set; }

        public string Availability { get; set; }

        public string CurrencyCode { get; set; }

        public string RetailerAffiliates { get; set; }

        public bool IncludeGST { get; set; }

        public string ReturnPolicy { get; set; }

        public string DeliveryInfo { get; set; }

        public bool AustraliaDelivery { get; set; }

        public string Discounts { get; set; }

        public string AdminComments { get; set; }

        public string CreatedBy { get; set; }

        public string CurrencySymbol { get; set; }
        public int ShippingOriginCountry { get; set; }

        private DateTime _createOn = DateTime.Now;
        public DateTime CreatedOn
        {
            get { return _createOn; }
            set { _createOn = value; }
        }

        public string ModifiedBy { get; set; }

        private DateTime _modifiedOn = DateTime.Now;
        public DateTime ModifiedOn
        {
            get { return _modifiedOn; }
            set { _modifiedOn = value; }
        }

        public string LogoFile { get; set; }

        public string Postcode { get; set; }

        public bool IsCreditcard { get; set; }

        public decimal CCFee { get; set; }

        public int FrequencytypeID { get; set; }

        public bool IsParallelImported { get; set; }

        public bool IsPickUp { get; set; }

        public string CompanyEmail { get; set; }

        public string PriceDescriptor { get; set; }

        public string BillingEmail { get; set; }

        public string BusinessTollfreeNumber { get; set; }

        public string Level { get; set; }

        public string Building { get; set; }

        public string Street { get; set; }

        public string Suburb { get; set; }

        public string District { get; set; }

        public string Region { get; set; }

        public string ProductsServices { get; set; }

        public string BrandsCarried { get; set; }

        public string NumberOfEmployees { get; set; }

        public int LocationCategoryId { get; set; }

        public string DeliveryTime { get; set; }

        public string Videourl { get; set; }

        public bool IsCertificated { get; set; }

        public string GSTNumber { get; set; }

        public string CompanyRegNumber { get; set; }

        public string FullcompanyName { get; set; }

        public string Keyword { get; set; }

        public bool IsSetupComplete { get; set; }

        public DateTime ISCUpdateTime { get; set; }

        public int RetailerTypeID { get; set; }

        public string CheckSetupEmailBy { get; set; }

        public int TechnicalAdminID { get; set; }

        public string RegistrationNumber { get; set; }

        public string Ref { get; set; }

        public int FinishLlater { get; set; }

        public bool isCPA { get; set; }

        public bool SecurePayment { get; set; }

        public bool PhoneSupport { get; set; }

        public bool ReturnsAccepted { get; set; }

        public DateTime LastChristmasDevDate { get; set; }

        public string ForeignCurrency { get; set; }

        public int ShoppingRetailerID { get; set; }

        #endregion

        #region

        public int Clicks
        {
            get;
            set;
        }

        public decimal AvRating
        {
            get;
            set;
        }

        public string ReviewString
        {
            get;
            set;
        }

        public string StoreTypeName
        {
            get;
            set;
        }

        public int ReviewsCount
        {
            get;
            set;
        }

        public decimal Rating
        {
            get;
            set;
        }
        #endregion
    }
}
