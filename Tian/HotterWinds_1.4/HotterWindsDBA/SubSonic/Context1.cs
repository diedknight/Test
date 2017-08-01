


using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using SubSonic.DataProviders;
using SubSonic.Extensions;
using SubSonic.Linq.Structure;
using SubSonic.Query;
using SubSonic.Schema;
using System.Data.Common;
using System.Collections.Generic;

namespace HotterWindsDBA
{
    public partial class PriceMeDBDB : IQuerySurface
    {

        public IDataProvider DataProvider;
        public DbQueryProvider provider;
        
        public static IDataProvider DefaultDataProvider { get; set; }

        public bool TestMode
		{
            get
			{
                return DataProvider.ConnectionString.Equals("test", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public PriceMeDBDB() 
        {
            if (DefaultDataProvider == null) {
                DataProvider = ProviderFactory.GetProvider("HotterWinds");
            }
            else {
                DataProvider = DefaultDataProvider;
            }
            Init();
        }

        public PriceMeDBDB(string connectionStringName)
        {
            DataProvider = ProviderFactory.GetProvider(connectionStringName);
            Init();
        }

		public PriceMeDBDB(string connectionString, string providerName)
        {
            DataProvider = ProviderFactory.GetProvider(connectionString,providerName);
            Init();
        }

		public ITable FindByPrimaryKey(string pkName)
        {
            return DataProvider.Schema.Tables.SingleOrDefault(x => x.PrimaryKey.Name.Equals(pkName, StringComparison.InvariantCultureIgnoreCase));
        }

        public Query<T> GetQuery<T>()
        {
            return new Query<T>(provider);
        }
        
        public ITable FindTable(string tableName)
        {
            return DataProvider.FindTable(tableName);
        }
               
        public IDataProvider Provider
        {
            get { return DataProvider; }
            set {DataProvider=value;}
        }
        
        public DbQueryProvider QueryProvider
        {
            get { return provider; }
        }
        
        BatchQuery _batch = null;
        public void Queue<T>(IQueryable<T> qry)
        {
            if (_batch == null)
                _batch = new BatchQuery(Provider, QueryProvider);
            _batch.Queue(qry);
        }

        public void Queue(ISqlQuery qry)
        {
            if (_batch == null)
                _batch = new BatchQuery(Provider, QueryProvider);
            _batch.Queue(qry);
        }

        public void ExecuteTransaction(IList<DbCommand> commands)
		{
            if(!TestMode)
			{
                using(var connection = commands[0].Connection)
				{
                   if (connection.State == ConnectionState.Closed)
                        connection.Open();
                   
                   using (var trans = connection.BeginTransaction()) 
				   {
                        foreach (var cmd in commands) 
						{
                            cmd.Transaction = trans;
                            cmd.Connection = connection;
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    connection.Close();
                }
            }
        }

        public IDataReader ExecuteBatch()
        {
            if (_batch == null)
                throw new InvalidOperationException("There's nothing in the queue");
            if(!TestMode)
                return _batch.ExecuteReader();
            return null;
        }
			
        public Query<CSK_Store_ProductLibrary> CSK_Store_ProductLibraries { get; set; }
        public Query<Partner_Topic_Map> Partner_Topic_Maps { get; set; }
        public Query<CSK_Store_RetailerFreightLocation> CSK_Store_RetailerFreightLocations { get; set; }
        public Query<CSK_Store_DailyDeal> CSK_Store_DailyDeals { get; set; }
        public Query<CSK_Store_ProductList> CSK_Store_ProductLists { get; set; }
        public Query<aspnet_Profile> aspnet_Profiles { get; set; }
        public Query<CSK_Store_ShareWishList> CSK_Store_ShareWishLists { get; set; }
        public Query<CSK_Store_Import_MapNotSupportedCategory> CSK_Store_Import_MapNotSupportedCategories { get; set; }
        public Query<CSK_Store_AttributeValueRange> CSK_Store_AttributeValueRanges { get; set; }
        public Query<CSK_Store_FreightLocation_Map> CSK_Store_FreightLocation_Maps { get; set; }
        public Query<PurgedProduct> PurgedProducts { get; set; }
        public Query<CSK_Store_AutomaticSpecialCharacter> CSK_Store_AutomaticSpecialCharacters { get; set; }
        public Query<Skykiwi_CategoryName_Translate> Skykiwi_CategoryName_Translates { get; set; }
        public Query<Store_GLatLng> Store_GLatLngs { get; set; }
        public Query<RetailerReviewDetail> RetailerReviewDetails { get; set; }
        public Query<CSK_Store_PPCStat> CSK_Store_PPCStats { get; set; }
        public Query<CSK_Store_RetailerOffer> CSK_Store_RetailerOffers { get; set; }
        public Query<CSK_Store_RetailerStatus> CSK_Store_RetailerStatuses { get; set; }
        public Query<CSK_Store_MostPopularProduct> CSK_Store_MostPopularProducts { get; set; }
        public Query<CSK_Store_BuyingGuide> CSK_Store_BuyingGuides { get; set; }
        public Query<CSK_Store_ExpertReview> CSK_Store_ExpertReviews { get; set; }
        public Query<CSK_Store_RetailerImage> CSK_Store_RetailerImages { get; set; }
        public Query<CSK_Store_Welcontent> CSK_Store_Welcontents { get; set; }
        public Query<CSK_Store_FeedType> CSK_Store_FeedTypes { get; set; }
        public Query<CSK_Store_Range> CSK_Store_Ranges { get; set; }
        public Query<UnitTable> UnitTables { get; set; }
        public Query<aspnet_Role> aspnet_Roles { get; set; }
        public Query<WidgetAffiliate> WidgetAffiliates { get; set; }
        public Query<CSK_Store_ReviewSource> CSK_Store_ReviewSources { get; set; }
        public Query<CSK_Store_RetailerPaymentOption> CSK_Store_RetailerPaymentOptions { get; set; }
        public Query<HW_Newsletter_Email> HW_Newsletter_Emails { get; set; }
        public Query<CSK_Store_OMGRetailers_Affiliate> CSK_Store_OMGRetailers_Affiliates { get; set; }
        public Query<MyLog> MyLogs { get; set; }
        public Query<CSK_Store_BuyingGuideRelated> CSK_Store_BuyingGuideRelateds { get; set; }
        public Query<CSK_Store_TreepodiaVideo> CSK_Store_TreepodiaVideos { get; set; }
        public Query<CSK_Store_FeaturedProduct> CSK_Store_FeaturedProducts { get; set; }
        public Query<aspnet_UsersInRole> aspnet_UsersInRoles { get; set; }
        public Query<CSK_Store_SimilarProduct> CSK_Store_SimilarProducts { get; set; }
        public Query<CSK_Store_JobFunction> CSK_Store_JobFunctions { get; set; }
        public Query<CSK_Store_UserAgent> CSK_Store_UserAgents { get; set; }
        public Query<CSK_Store_RetailerProductNew> CSK_Store_RetailerProductNews { get; set; }
        public Query<CSK_Store_Product_Category_Map> CSK_Store_Product_Category_Maps { get; set; }
        public Query<CSK_Store_RetailerLeadSignUp> CSK_Store_RetailerLeadSignUps { get; set; }
        public Query<CSk_Store_RenameProduct_Type> CSk_Store_RenameProduct_Types { get; set; }
        public Query<CSK_Store_NewsLetterUser> CSK_Store_NewsLetterUsers { get; set; }
        public Query<CSK_Store_BuyingGuideType> CSK_Store_BuyingGuideTypes { get; set; }
        public Query<CSK_Store_RetailerLeadTracking> CSK_Store_RetailerLeadTrackings { get; set; }
        public Query<CSK_Store_CrawlerPriority> CSK_Store_CrawlerPriorities { get; set; }
        public Query<CSk_Store_RenameProduct_Setting> CSk_Store_RenameProduct_Settings { get; set; }
        public Query<CSK_Store_RetailerProductCondition> CSK_Store_RetailerProductConditions { get; set; }
        public Query<CSK_Store_PopularBrand> CSK_Store_PopularBrands { get; set; }
        public Query<CSK_Store_PPCCreditCard> CSK_Store_PPCCreditCards { get; set; }
        public Query<CSK_Store_ProductReviewComment> CSK_Store_ProductReviewComments { get; set; }
        public Query<CSK_Store_Category> CSK_Store_Categories { get; set; }
        public Query<CSK_Store_RetailerProduct> CSK_Store_RetailerProducts { get; set; }
        public Query<CSK_Forum_Topic> CSK_Forum_Topics { get; set; }
        public Query<CSK_Store_Retailer_Change_Rate> CSK_Store_Retailer_Change_Rates { get; set; }
        public Query<CSK_Forum_QPost> CSK_Forum_QPosts { get; set; }
        public Query<CSK_Store_PPCPaymentOption> CSK_Store_PPCPaymentOptions { get; set; }
        public Query<CSK_Forum_ReportedAbsu> CSK_Forum_ReportedAbsus { get; set; }
        public Query<CSK_Store_ManufacturerMap> CSK_Store_ManufacturerMaps { get; set; }
        public Query<CSK_Store_ReviewSourceAU> CSK_Store_ReviewSourceAUs { get; set; }
        public Query<CSK_Store_CommunityWelContent> CSK_Store_CommunityWelContents { get; set; }
        public Query<CSK_Store_NewProduct> CSK_Store_NewProducts { get; set; }
        public Query<aspnet_Path> aspnet_Paths { get; set; }
        public Query<CSK_Store_ExceptionCollect> CSK_Store_ExceptionCollects { get; set; }
        public Query<CSK_Store_RetailerKeyword> CSK_Store_RetailerKeywords { get; set; }
        public Query<CSK_Store_Category_AttributeTitle_Map> CSK_Store_Category_AttributeTitle_Maps { get; set; }
        public Query<CSK_Store_ProductReview> CSK_Store_ProductReviews { get; set; }
        public Query<Local_CategoryName> Local_CategoryNames { get; set; }
        public Query<CSK_Store_PPCCategory> CSK_Store_PPCCategories { get; set; }
        public Query<CSK_Store_DirectoryCategoryMap> CSK_Store_DirectoryCategoryMaps { get; set; }
        public Query<aspnet_PersonalizationAllUser> aspnet_PersonalizationAllUsers { get; set; }
        public Query<ShortCutCategory_Map> ShortCutCategory_Maps { get; set; }
        public Query<CSK_Store_CategoryBugingGuide_Map> CSK_Store_CategoryBugingGuide_Maps { get; set; }
        public Query<CSK_Store_LocationCategory> CSK_Store_LocationCategories { get; set; }
        public Query<CSK_Store_UserSearch_Google> CSK_Store_UserSearch_Googles { get; set; }
        public Query<aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUsers { get; set; }
        public Query<CSK_Store_AdvancedMapping> CSK_Store_AdvancedMappings { get; set; }
        public Query<CSK_Store_ProductAlertHistory> CSK_Store_ProductAlertHistories { get; set; }
        public Query<CSK_Store_RetailerAddressType> CSK_Store_RetailerAddressTypes { get; set; }
        public Query<PriceMe_ExpertAverageRatingTF> PriceMe_ExpertAverageRatingTFs { get; set; }
        public Query<CSK_Store_RetailerOperatingHour> CSK_Store_RetailerOperatingHours { get; set; }
        public Query<CSK_Store_PPCAccount> CSK_Store_PPCAccounts { get; set; }
        public Query<CSK_Store_ECommercePlatform> CSK_Store_ECommercePlatforms { get; set; }
        public Query<CSK_Store_RetailerAddress> CSK_Store_RetailerAddresses { get; set; }
        public Query<CSK_Util_Currency> CSK_Util_Currencies { get; set; }
        public Query<PopularSearch> PopularSearches { get; set; }
        public Query<CSK_Store_ExpertReviewAU> CSK_Store_ExpertReviewAUs { get; set; }
        public Query<CSK_Util_Country> CSK_Util_Countries { get; set; }
        public Query<CSK_Store_EmailDatum> CSK_Store_EmailData { get; set; }
        public Query<RetailerNewsletterInfo> RetailerNewsletterInfos { get; set; }
        public Query<CSK_Store_IP_Blacklist> CSK_Store_IP_Blacklists { get; set; }
        public Query<CSK_Store_NewsletterCtx> CSK_Store_NewsletterCtxes { get; set; }
        public Query<Log> Logs { get; set; }
        public Query<CSK_Store_CategoryViewType> CSK_Store_CategoryViewTypes { get; set; }
        public Query<Store_Compare_Attribute_Map> Store_Compare_Attribute_Maps { get; set; }
        public Query<CSK_Store_UserSearch> CSK_Store_UserSearches { get; set; }
        public Query<CSK_Store_PPCBudgetHistory> CSK_Store_PPCBudgetHistories { get; set; }
        public Query<CSK_Store_NewOnlyImport> CSK_Store_NewOnlyImports { get; set; }
        public Query<CSK_Store_Image> CSK_Store_Images { get; set; }
        public Query<Inactive_Retailer_Reason> Inactive_Retailer_Reasons { get; set; }
        public Query<PCM> PCMS { get; set; }
        public Query<CSK_Store_RetailerCategory> CSK_Store_RetailerCategories { get; set; }
        public Query<CSK_Store_DailyDealsTracker> CSK_Store_DailyDealsTrackers { get; set; }
        public Query<CSK_Store_RetailerReviewComment> CSK_Store_RetailerReviewComments { get; set; }
        public Query<CSK_Store_BannerTracker> CSK_Store_BannerTrackers { get; set; }
        public Query<Store_Compare_Attribute> Store_Compare_Attributes { get; set; }
        public Query<CSK_Store_TransactionType> CSK_Store_TransactionTypes { get; set; }
        public Query<ResourcesInfo> ResourcesInfos { get; set; }
        public Query<CSK_Store_PPCInvoiceRatio> CSK_Store_PPCInvoiceRatios { get; set; }
        public Query<CSK_Messaging_Mailer> CSK_Messaging_Mailers { get; set; }
        public Query<CSK_Store_RetailerReview> CSK_Store_RetailerReviews { get; set; }
        public Query<CSK_Store_Advertising_Banner> CSK_Store_Advertising_Banners { get; set; }
        public Query<CSK_Store_Manufacturer> CSK_Store_Manufacturers { get; set; }
        public Query<CSK_Store_ManufacturerLibrary> CSK_Store_ManufacturerLibraries { get; set; }
        public Query<CSK_Store_Email_Friend> CSK_Store_Email_Friends { get; set; }
        public Query<CSK_Store_RetailerTracker_AffiliateID> CSK_Store_RetailerTracker_AffiliateIDs { get; set; }
        public Query<Csk_Store_FileFormat> Csk_Store_FileFormats { get; set; }
        public Query<CSK_Store_MostPopularCategory> CSK_Store_MostPopularCategories { get; set; }
        public Query<aspnet_WebEvent_Event> aspnet_WebEvent_Events { get; set; }
        public Query<CSK_Store_ListingImage> CSK_Store_ListingImages { get; set; }
        public Query<CSK_Store_PM_ProductDescription> CSK_Store_PM_ProductDescriptions { get; set; }
        public Query<RelatedCategory> RelatedCategories { get; set; }
        public Query<aspnet_MembershipInfo> aspnet_MembershipInfos { get; set; }
        public Query<Csk_Store_FeedField> Csk_Store_FeedFields { get; set; }
        public Query<CSK_Store_NewsRelease> CSK_Store_NewsReleases { get; set; }
        public Query<Csk_Store_FeedFormat> Csk_Store_FeedFormats { get; set; }
        public Query<CSK_Store_PaymentOption> CSK_Store_PaymentOptions { get; set; }
        public Query<CSK_Store_FreightType> CSK_Store_FreightTypes { get; set; }
        public Query<CSK_Store_Variety> CSK_Store_Varieties { get; set; }
        public Query<CSK_Store_ProductRating> CSK_Store_ProductRatings { get; set; }
        public Query<EMailInfo> EMailInfos { get; set; }
        public Query<CSK_Store_PPCMemberDailyBudget> CSK_Store_PPCMemberDailyBudgets { get; set; }
        public Query<CSK_Store_EmailSendOut> CSK_Store_EmailSendOuts { get; set; }
        public Query<CSK_Store_PPCMember> CSK_Store_PPCMembers { get; set; }
        public Query<PageFavourite> PageFavourites { get; set; }
        public Query<ProductFavourite> ProductFavourites { get; set; }
        public Query<CSK_Store_Product404> CSK_Store_Product404s { get; set; }
        public Query<CSK_Content> CSK_Contents { get; set; }
        public Query<AutomatedMerging> AutomatedMergings { get; set; }
        public Query<CSK_Store_PriceHistory> CSK_Store_PriceHistories { get; set; }
        public Query<CSK_Store_RetailerRedirectClick> CSK_Store_RetailerRedirectClicks { get; set; }
        public Query<CSK_Store_ZeroProduct_Change_Rate> CSK_Store_ZeroProduct_Change_Rates { get; set; }
        public Query<CSK_Store_PPCMemberType> CSK_Store_PPCMemberTypes { get; set; }
        public Query<CSK_Store_Energy> CSK_Store_Energies { get; set; }
        public Query<TradeMeCategoryMap> TradeMeCategoryMaps { get; set; }
        public Query<CSK_Store_PPCPlan> CSK_Store_PPCPlans { get; set; }
        public Query<CSK_Store_ProductNew> CSK_Store_ProductNews { get; set; }
        public Query<CSK_RetaileriContact_Map> CSK_RetaileriContact_Maps { get; set; }
        public Query<CSK_Store_ProductAlert> CSK_Store_ProductAlerts { get; set; }
        public Query<CSK_Store_AutomaticMergingOptionSetting> CSK_Store_AutomaticMergingOptionSettings { get; set; }
        public Query<CSK_Store_CategoryFeaturedProduct> CSK_Store_CategoryFeaturedProducts { get; set; }
        public Query<BiggestDropPrice> BiggestDropPrices { get; set; }
        public Query<CSK_Store_FeaturedTab> CSK_Store_FeaturedTabs { get; set; }
        public Query<Csk_Store_FeedFormatMap> Csk_Store_FeedFormatMaps { get; set; }
        public Query<CSK_Store_CategoryMergingKeyword> CSK_Store_CategoryMergingKeywords { get; set; }
        public Query<CSK_Store_PPCTransaction> CSK_Store_PPCTransactions { get; set; }
        public Query<CSK_Store_ProductDescriptor> CSK_Store_ProductDescriptors { get; set; }
        public Query<CSK_Store_Import_Map> CSK_Store_Import_Maps { get; set; }
        public Query<CSK_Store_Product> CSK_Store_Products { get; set; }
        public Query<CSK_Store_AdminInformation> CSK_Store_AdminInformations { get; set; }
        public Query<CSK_Store_ProductViewTracking> CSK_Store_ProductViewTrackings { get; set; }
        public Query<aspnet_Application> aspnet_Applications { get; set; }
        public Query<FrequencyType> FrequencyTypes { get; set; }
        public Query<CSK_Store_ProductDescriptorTitle> CSK_Store_ProductDescriptorTitles { get; set; }
        public Query<CSK_Store_RetailerVotesSum> CSK_Store_RetailerVotesSums { get; set; }
        public Query<InvalidKeyword> InvalidKeywords { get; set; }
        public Query<CSK_Store_FeatureCarousel> CSK_Store_FeatureCarousels { get; set; }
        public Query<Store_GRegion> Store_GRegions { get; set; }
        public Query<CSK_Store_BannerType> CSK_Store_BannerTypes { get; set; }
        public Query<CSK_Store_ProductReviewFeedback> CSK_Store_ProductReviewFeedbacks { get; set; }
        public Query<aspnet_User> aspnet_Users { get; set; }
        public Query<CSK_Store_AutomaticRetailerNoModel> CSK_Store_AutomaticRetailerNoModels { get; set; }
        public Query<CSK_Store_ProductIsMerged> CSK_Store_ProductIsMergeds { get; set; }
        public Query<CSK_Store_TempAuto> CSK_Store_TempAutos { get; set; }
        public Query<CSK_Stats_Tracker> CSK_Stats_Trackers { get; set; }
        public Query<CSK_Store_ProductVotesSum> CSK_Store_ProductVotesSums { get; set; }
        public Query<aspnet_SchemaVersion> aspnet_SchemaVersions { get; set; }
        public Query<CSK_Store_Tool> CSK_Store_Tools { get; set; }
        public Query<CSK_Store_RetailerProductAlert> CSK_Store_RetailerProductAlerts { get; set; }
        public Query<CSK_Store_ReviewUseful> CSK_Store_ReviewUsefuls { get; set; }
        public Query<CSK_Store_ProductStatus> CSK_Store_ProductStatuses { get; set; }
        public Query<CSK_Store_RetailerST> CSK_Store_RetailerSTs { get; set; }
        public Query<CSK_Store_NewsLetter> CSK_Store_NewsLetters { get; set; }
        public Query<CSK_Stats_Behavior> CSK_Stats_Behaviors { get; set; }
        public Query<CSK_Store_ReviewUsefulType> CSK_Store_ReviewUsefulTypes { get; set; }
        public Query<CSK_Store_ShoppingTracker> CSK_Store_ShoppingTrackers { get; set; }
        public Query<CSK_Store_AttributeMatch> CSK_Store_AttributeMatches { get; set; }
        public Query<CSK_Store_RetailerStoreType> CSK_Store_RetailerStoreTypes { get; set; }
        public Query<CSK_Store_ProductType> CSK_Store_ProductTypes { get; set; }
        public Query<CSK_Store_AutomaticRemoveSpace> CSK_Store_AutomaticRemoveSpaces { get; set; }
        public Query<CSK_Store_PPCMemmberHistory> CSK_Store_PPCMemmberHistories { get; set; }
        public Query<CSK_Store_DailyDealsCategory> CSK_Store_DailyDealsCategories { get; set; }
        public Query<CSk_Store_AutomaticCategorySeries> CSk_Store_AutomaticCategorySeries { get; set; }
        public Query<CSK_Store_ProductRetailerCountHistory> CSK_Store_ProductRetailerCountHistories { get; set; }
        public Query<aspnet_Membership> aspnet_Memberships { get; set; }
        public Query<CSK_Store_ProductVideo> CSK_Store_ProductVideos { get; set; }
        public Query<CSK_Store_DailyDealsRetailer> CSK_Store_DailyDealsRetailers { get; set; }
        public Query<CSK_Store_UserLocation> CSK_Store_UserLocations { get; set; }
        public Query<CSK_Store_RetailerProductlibrary> CSK_Store_RetailerProductlibraries { get; set; }
        public Query<CSK_Store_AttributeGroup> CSK_Store_AttributeGroups { get; set; }
        public Query<Store_CrawlClassType> Store_CrawlClassTypes { get; set; }
        public Query<CSK_Store_FreightLocation> CSK_Store_FreightLocations { get; set; }
        public Query<CSK_Store_ConsumerPriceMeMapping> CSK_Store_ConsumerPriceMeMappings { get; set; }
        public Query<CSK_Store_RetailerTrackerlibrary> CSK_Store_RetailerTrackerlibraries { get; set; }
        public Query<CSK_Store_RetailerTracker> CSK_Store_RetailerTrackers { get; set; }
        public Query<CSK_Store_GoodSearchKeyword> CSK_Store_GoodSearchKeywords { get; set; }
        public Query<CSK_Store_RetailerCrawlerInfo> CSK_Store_RetailerCrawlerInfos { get; set; }
        public Query<CategoryFooterMap> CategoryFooterMaps { get; set; }
        public Query<CSK_Store_PreviousNewsletter> CSK_Store_PreviousNewsletters { get; set; }
        public Query<CALC_RetailerCategory_Click> CALC_RetailerCategory_Clicks { get; set; }
        public Query<CSK_Store_RelatedCategory_Map> CSK_Store_RelatedCategory_Maps { get; set; }
        public Query<CSK_Store_Retailer> CSK_Store_Retailers { get; set; }
        public Query<CSK_Store_AttributeType> CSK_Store_AttributeTypes { get; set; }
        public Query<CSK_Store_RetailerCity> CSK_Store_RetailerCities { get; set; }
        public Query<CSK_Store_SearchKeywordsRule> CSK_Store_SearchKeywordsRules { get; set; }
        public Query<CSK_Store_EECADatum> CSK_Store_EECAData { get; set; }
        public Query<CSK_Store_GovernmentBadge> CSK_Store_GovernmentBadges { get; set; }
        public Query<CSK_Store_ProductVideoReview> CSK_Store_ProductVideoReviews { get; set; }
        public Query<CSK_Store_RetailerFreight> CSK_Store_RetailerFreights { get; set; }
        public Query<CSK_Store_ProductVideoSource> CSK_Store_ProductVideoSources { get; set; }
        public Query<CSK_Store_YPricemeSpaceID> CSK_Store_YPricemeSpaceIDs { get; set; }
        public Query<CSK_Store_RetailerRating> CSK_Store_RetailerRatings { get; set; }
        public Query<CSK_Store_AttributeValue> CSK_Store_AttributeValues { get; set; }
        public Query<CSK_Store_List> CSK_Store_Lists { get; set; }
        public Query<CSK_Store_CategoryTotal> CSK_Store_CategoryTotals { get; set; }
        public Query<CSK_Store_AutomaticMappingNoModel> CSK_Store_AutomaticMappingNoModels { get; set; }
        public Query<CSK_Store_MobileRetailerTracker> CSK_Store_MobileRetailerTrackers { get; set; }
        public Query<Fisher_and_Paykel_Map> Fisher_and_Paykel_Maps { get; set; }
        public Query<CSK_Store_ListType> CSK_Store_ListTypes { get; set; }
        public Query<Csk_store_redirectforRetailerproduct> Csk_store_redirectforRetailerproducts { get; set; }

			

        #region ' Aggregates and SubSonic Queries '
        public Select SelectColumns(params string[] columns)
        {
            return new Select(DataProvider, columns);
        }

        public Select Select
        {
            get { return new Select(this.Provider); }
        }

        public Insert Insert
		{
            get { return new Insert(this.Provider); }
        }

        public Update<T> Update<T>() where T:new()
		{
            return new Update<T>(this.Provider);
        }

        public SqlQuery Delete<T>(Expression<Func<T,bool>> column) where T:new()
        {
            LambdaExpression lamda = column;
            SqlQuery result = new Delete<T>(this.Provider);
            result = result.From<T>();
            result.Constraints=lamda.ParseConstraints().ToList();
            return result;
        }

        public SqlQuery Max<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = DataProvider.FindTable(objectName).Name;
            return new Select(DataProvider, new Aggregate(colName, AggregateFunction.Max)).From(tableName);
        }

        public SqlQuery Min<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.Min)).From(tableName);
        }

        public SqlQuery Sum<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.Sum)).From(tableName);
        }

        public SqlQuery Avg<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.Avg)).From(tableName);
        }

        public SqlQuery Count<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.Count)).From(tableName);
        }

        public SqlQuery Variance<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.Var)).From(tableName);
        }

        public SqlQuery StandardDeviation<T>(Expression<Func<T,object>> column)
        {
            LambdaExpression lamda = column;
            string colName = lamda.ParseObjectValue();
            string objectName = typeof(T).Name;
            string tableName = this.Provider.FindTable(objectName).Name;
            return new Select(this.Provider, new Aggregate(colName, AggregateFunction.StDev)).From(tableName);
        }

        #endregion

        void Init()
        {
            provider = new DbQueryProvider(this.Provider);

            #region ' Query Defs '
            CSK_Store_ProductLibraries = new Query<CSK_Store_ProductLibrary>(provider);
            Partner_Topic_Maps = new Query<Partner_Topic_Map>(provider);
            CSK_Store_RetailerFreightLocations = new Query<CSK_Store_RetailerFreightLocation>(provider);
            CSK_Store_DailyDeals = new Query<CSK_Store_DailyDeal>(provider);
            CSK_Store_ProductLists = new Query<CSK_Store_ProductList>(provider);
            aspnet_Profiles = new Query<aspnet_Profile>(provider);
            CSK_Store_ShareWishLists = new Query<CSK_Store_ShareWishList>(provider);
            CSK_Store_Import_MapNotSupportedCategories = new Query<CSK_Store_Import_MapNotSupportedCategory>(provider);
            CSK_Store_AttributeValueRanges = new Query<CSK_Store_AttributeValueRange>(provider);
            CSK_Store_FreightLocation_Maps = new Query<CSK_Store_FreightLocation_Map>(provider);
            PurgedProducts = new Query<PurgedProduct>(provider);
            CSK_Store_AutomaticSpecialCharacters = new Query<CSK_Store_AutomaticSpecialCharacter>(provider);
            Skykiwi_CategoryName_Translates = new Query<Skykiwi_CategoryName_Translate>(provider);
            Store_GLatLngs = new Query<Store_GLatLng>(provider);
            RetailerReviewDetails = new Query<RetailerReviewDetail>(provider);
            CSK_Store_PPCStats = new Query<CSK_Store_PPCStat>(provider);
            CSK_Store_RetailerOffers = new Query<CSK_Store_RetailerOffer>(provider);
            CSK_Store_RetailerStatuses = new Query<CSK_Store_RetailerStatus>(provider);
            CSK_Store_MostPopularProducts = new Query<CSK_Store_MostPopularProduct>(provider);
            CSK_Store_BuyingGuides = new Query<CSK_Store_BuyingGuide>(provider);
            CSK_Store_ExpertReviews = new Query<CSK_Store_ExpertReview>(provider);
            CSK_Store_RetailerImages = new Query<CSK_Store_RetailerImage>(provider);
            CSK_Store_Welcontents = new Query<CSK_Store_Welcontent>(provider);
            CSK_Store_FeedTypes = new Query<CSK_Store_FeedType>(provider);
            CSK_Store_Ranges = new Query<CSK_Store_Range>(provider);
            UnitTables = new Query<UnitTable>(provider);
            aspnet_Roles = new Query<aspnet_Role>(provider);
            WidgetAffiliates = new Query<WidgetAffiliate>(provider);
            CSK_Store_ReviewSources = new Query<CSK_Store_ReviewSource>(provider);
            CSK_Store_RetailerPaymentOptions = new Query<CSK_Store_RetailerPaymentOption>(provider);
            HW_Newsletter_Emails = new Query<HW_Newsletter_Email>(provider);
            CSK_Store_OMGRetailers_Affiliates = new Query<CSK_Store_OMGRetailers_Affiliate>(provider);
            MyLogs = new Query<MyLog>(provider);
            CSK_Store_BuyingGuideRelateds = new Query<CSK_Store_BuyingGuideRelated>(provider);
            CSK_Store_TreepodiaVideos = new Query<CSK_Store_TreepodiaVideo>(provider);
            CSK_Store_FeaturedProducts = new Query<CSK_Store_FeaturedProduct>(provider);
            aspnet_UsersInRoles = new Query<aspnet_UsersInRole>(provider);
            CSK_Store_SimilarProducts = new Query<CSK_Store_SimilarProduct>(provider);
            CSK_Store_JobFunctions = new Query<CSK_Store_JobFunction>(provider);
            CSK_Store_UserAgents = new Query<CSK_Store_UserAgent>(provider);
            CSK_Store_RetailerProductNews = new Query<CSK_Store_RetailerProductNew>(provider);
            CSK_Store_Product_Category_Maps = new Query<CSK_Store_Product_Category_Map>(provider);
            CSK_Store_RetailerLeadSignUps = new Query<CSK_Store_RetailerLeadSignUp>(provider);
            CSk_Store_RenameProduct_Types = new Query<CSk_Store_RenameProduct_Type>(provider);
            CSK_Store_NewsLetterUsers = new Query<CSK_Store_NewsLetterUser>(provider);
            CSK_Store_BuyingGuideTypes = new Query<CSK_Store_BuyingGuideType>(provider);
            CSK_Store_RetailerLeadTrackings = new Query<CSK_Store_RetailerLeadTracking>(provider);
            CSK_Store_CrawlerPriorities = new Query<CSK_Store_CrawlerPriority>(provider);
            CSk_Store_RenameProduct_Settings = new Query<CSk_Store_RenameProduct_Setting>(provider);
            CSK_Store_RetailerProductConditions = new Query<CSK_Store_RetailerProductCondition>(provider);
            CSK_Store_PopularBrands = new Query<CSK_Store_PopularBrand>(provider);
            CSK_Store_PPCCreditCards = new Query<CSK_Store_PPCCreditCard>(provider);
            CSK_Store_ProductReviewComments = new Query<CSK_Store_ProductReviewComment>(provider);
            CSK_Store_Categories = new Query<CSK_Store_Category>(provider);
            CSK_Store_RetailerProducts = new Query<CSK_Store_RetailerProduct>(provider);
            CSK_Forum_Topics = new Query<CSK_Forum_Topic>(provider);
            CSK_Store_Retailer_Change_Rates = new Query<CSK_Store_Retailer_Change_Rate>(provider);
            CSK_Forum_QPosts = new Query<CSK_Forum_QPost>(provider);
            CSK_Store_PPCPaymentOptions = new Query<CSK_Store_PPCPaymentOption>(provider);
            CSK_Forum_ReportedAbsus = new Query<CSK_Forum_ReportedAbsu>(provider);
            CSK_Store_ManufacturerMaps = new Query<CSK_Store_ManufacturerMap>(provider);
            CSK_Store_ReviewSourceAUs = new Query<CSK_Store_ReviewSourceAU>(provider);
            CSK_Store_CommunityWelContents = new Query<CSK_Store_CommunityWelContent>(provider);
            CSK_Store_NewProducts = new Query<CSK_Store_NewProduct>(provider);
            aspnet_Paths = new Query<aspnet_Path>(provider);
            CSK_Store_ExceptionCollects = new Query<CSK_Store_ExceptionCollect>(provider);
            CSK_Store_RetailerKeywords = new Query<CSK_Store_RetailerKeyword>(provider);
            CSK_Store_Category_AttributeTitle_Maps = new Query<CSK_Store_Category_AttributeTitle_Map>(provider);
            CSK_Store_ProductReviews = new Query<CSK_Store_ProductReview>(provider);
            Local_CategoryNames = new Query<Local_CategoryName>(provider);
            CSK_Store_PPCCategories = new Query<CSK_Store_PPCCategory>(provider);
            CSK_Store_DirectoryCategoryMaps = new Query<CSK_Store_DirectoryCategoryMap>(provider);
            aspnet_PersonalizationAllUsers = new Query<aspnet_PersonalizationAllUser>(provider);
            ShortCutCategory_Maps = new Query<ShortCutCategory_Map>(provider);
            CSK_Store_CategoryBugingGuide_Maps = new Query<CSK_Store_CategoryBugingGuide_Map>(provider);
            CSK_Store_LocationCategories = new Query<CSK_Store_LocationCategory>(provider);
            CSK_Store_UserSearch_Googles = new Query<CSK_Store_UserSearch_Google>(provider);
            aspnet_PersonalizationPerUsers = new Query<aspnet_PersonalizationPerUser>(provider);
            CSK_Store_AdvancedMappings = new Query<CSK_Store_AdvancedMapping>(provider);
            CSK_Store_ProductAlertHistories = new Query<CSK_Store_ProductAlertHistory>(provider);
            CSK_Store_RetailerAddressTypes = new Query<CSK_Store_RetailerAddressType>(provider);
            PriceMe_ExpertAverageRatingTFs = new Query<PriceMe_ExpertAverageRatingTF>(provider);
            CSK_Store_RetailerOperatingHours = new Query<CSK_Store_RetailerOperatingHour>(provider);
            CSK_Store_PPCAccounts = new Query<CSK_Store_PPCAccount>(provider);
            CSK_Store_ECommercePlatforms = new Query<CSK_Store_ECommercePlatform>(provider);
            CSK_Store_RetailerAddresses = new Query<CSK_Store_RetailerAddress>(provider);
            CSK_Util_Currencies = new Query<CSK_Util_Currency>(provider);
            PopularSearches = new Query<PopularSearch>(provider);
            CSK_Store_ExpertReviewAUs = new Query<CSK_Store_ExpertReviewAU>(provider);
            CSK_Util_Countries = new Query<CSK_Util_Country>(provider);
            CSK_Store_EmailData = new Query<CSK_Store_EmailDatum>(provider);
            RetailerNewsletterInfos = new Query<RetailerNewsletterInfo>(provider);
            CSK_Store_IP_Blacklists = new Query<CSK_Store_IP_Blacklist>(provider);
            CSK_Store_NewsletterCtxes = new Query<CSK_Store_NewsletterCtx>(provider);
            Logs = new Query<Log>(provider);
            CSK_Store_CategoryViewTypes = new Query<CSK_Store_CategoryViewType>(provider);
            Store_Compare_Attribute_Maps = new Query<Store_Compare_Attribute_Map>(provider);
            CSK_Store_UserSearches = new Query<CSK_Store_UserSearch>(provider);
            CSK_Store_PPCBudgetHistories = new Query<CSK_Store_PPCBudgetHistory>(provider);
            CSK_Store_NewOnlyImports = new Query<CSK_Store_NewOnlyImport>(provider);
            CSK_Store_Images = new Query<CSK_Store_Image>(provider);
            Inactive_Retailer_Reasons = new Query<Inactive_Retailer_Reason>(provider);
            PCMS = new Query<PCM>(provider);
            CSK_Store_RetailerCategories = new Query<CSK_Store_RetailerCategory>(provider);
            CSK_Store_DailyDealsTrackers = new Query<CSK_Store_DailyDealsTracker>(provider);
            CSK_Store_RetailerReviewComments = new Query<CSK_Store_RetailerReviewComment>(provider);
            CSK_Store_BannerTrackers = new Query<CSK_Store_BannerTracker>(provider);
            Store_Compare_Attributes = new Query<Store_Compare_Attribute>(provider);
            CSK_Store_TransactionTypes = new Query<CSK_Store_TransactionType>(provider);
            ResourcesInfos = new Query<ResourcesInfo>(provider);
            CSK_Store_PPCInvoiceRatios = new Query<CSK_Store_PPCInvoiceRatio>(provider);
            CSK_Messaging_Mailers = new Query<CSK_Messaging_Mailer>(provider);
            CSK_Store_RetailerReviews = new Query<CSK_Store_RetailerReview>(provider);
            CSK_Store_Advertising_Banners = new Query<CSK_Store_Advertising_Banner>(provider);
            CSK_Store_Manufacturers = new Query<CSK_Store_Manufacturer>(provider);
            CSK_Store_ManufacturerLibraries = new Query<CSK_Store_ManufacturerLibrary>(provider);
            CSK_Store_Email_Friends = new Query<CSK_Store_Email_Friend>(provider);
            CSK_Store_RetailerTracker_AffiliateIDs = new Query<CSK_Store_RetailerTracker_AffiliateID>(provider);
            Csk_Store_FileFormats = new Query<Csk_Store_FileFormat>(provider);
            CSK_Store_MostPopularCategories = new Query<CSK_Store_MostPopularCategory>(provider);
            aspnet_WebEvent_Events = new Query<aspnet_WebEvent_Event>(provider);
            CSK_Store_ListingImages = new Query<CSK_Store_ListingImage>(provider);
            CSK_Store_PM_ProductDescriptions = new Query<CSK_Store_PM_ProductDescription>(provider);
            RelatedCategories = new Query<RelatedCategory>(provider);
            aspnet_MembershipInfos = new Query<aspnet_MembershipInfo>(provider);
            Csk_Store_FeedFields = new Query<Csk_Store_FeedField>(provider);
            CSK_Store_NewsReleases = new Query<CSK_Store_NewsRelease>(provider);
            Csk_Store_FeedFormats = new Query<Csk_Store_FeedFormat>(provider);
            CSK_Store_PaymentOptions = new Query<CSK_Store_PaymentOption>(provider);
            CSK_Store_FreightTypes = new Query<CSK_Store_FreightType>(provider);
            CSK_Store_Varieties = new Query<CSK_Store_Variety>(provider);
            CSK_Store_ProductRatings = new Query<CSK_Store_ProductRating>(provider);
            EMailInfos = new Query<EMailInfo>(provider);
            CSK_Store_PPCMemberDailyBudgets = new Query<CSK_Store_PPCMemberDailyBudget>(provider);
            CSK_Store_EmailSendOuts = new Query<CSK_Store_EmailSendOut>(provider);
            CSK_Store_PPCMembers = new Query<CSK_Store_PPCMember>(provider);
            PageFavourites = new Query<PageFavourite>(provider);
            ProductFavourites = new Query<ProductFavourite>(provider);
            CSK_Store_Product404s = new Query<CSK_Store_Product404>(provider);
            CSK_Contents = new Query<CSK_Content>(provider);
            AutomatedMergings = new Query<AutomatedMerging>(provider);
            CSK_Store_PriceHistories = new Query<CSK_Store_PriceHistory>(provider);
            CSK_Store_RetailerRedirectClicks = new Query<CSK_Store_RetailerRedirectClick>(provider);
            CSK_Store_ZeroProduct_Change_Rates = new Query<CSK_Store_ZeroProduct_Change_Rate>(provider);
            CSK_Store_PPCMemberTypes = new Query<CSK_Store_PPCMemberType>(provider);
            CSK_Store_Energies = new Query<CSK_Store_Energy>(provider);
            TradeMeCategoryMaps = new Query<TradeMeCategoryMap>(provider);
            CSK_Store_PPCPlans = new Query<CSK_Store_PPCPlan>(provider);
            CSK_Store_ProductNews = new Query<CSK_Store_ProductNew>(provider);
            CSK_RetaileriContact_Maps = new Query<CSK_RetaileriContact_Map>(provider);
            CSK_Store_ProductAlerts = new Query<CSK_Store_ProductAlert>(provider);
            CSK_Store_AutomaticMergingOptionSettings = new Query<CSK_Store_AutomaticMergingOptionSetting>(provider);
            CSK_Store_CategoryFeaturedProducts = new Query<CSK_Store_CategoryFeaturedProduct>(provider);
            BiggestDropPrices = new Query<BiggestDropPrice>(provider);
            CSK_Store_FeaturedTabs = new Query<CSK_Store_FeaturedTab>(provider);
            Csk_Store_FeedFormatMaps = new Query<Csk_Store_FeedFormatMap>(provider);
            CSK_Store_CategoryMergingKeywords = new Query<CSK_Store_CategoryMergingKeyword>(provider);
            CSK_Store_PPCTransactions = new Query<CSK_Store_PPCTransaction>(provider);
            CSK_Store_ProductDescriptors = new Query<CSK_Store_ProductDescriptor>(provider);
            CSK_Store_Import_Maps = new Query<CSK_Store_Import_Map>(provider);
            CSK_Store_Products = new Query<CSK_Store_Product>(provider);
            CSK_Store_AdminInformations = new Query<CSK_Store_AdminInformation>(provider);
            CSK_Store_ProductViewTrackings = new Query<CSK_Store_ProductViewTracking>(provider);
            aspnet_Applications = new Query<aspnet_Application>(provider);
            FrequencyTypes = new Query<FrequencyType>(provider);
            CSK_Store_ProductDescriptorTitles = new Query<CSK_Store_ProductDescriptorTitle>(provider);
            CSK_Store_RetailerVotesSums = new Query<CSK_Store_RetailerVotesSum>(provider);
            InvalidKeywords = new Query<InvalidKeyword>(provider);
            CSK_Store_FeatureCarousels = new Query<CSK_Store_FeatureCarousel>(provider);
            Store_GRegions = new Query<Store_GRegion>(provider);
            CSK_Store_BannerTypes = new Query<CSK_Store_BannerType>(provider);
            CSK_Store_ProductReviewFeedbacks = new Query<CSK_Store_ProductReviewFeedback>(provider);
            aspnet_Users = new Query<aspnet_User>(provider);
            CSK_Store_AutomaticRetailerNoModels = new Query<CSK_Store_AutomaticRetailerNoModel>(provider);
            CSK_Store_ProductIsMergeds = new Query<CSK_Store_ProductIsMerged>(provider);
            CSK_Store_TempAutos = new Query<CSK_Store_TempAuto>(provider);
            CSK_Stats_Trackers = new Query<CSK_Stats_Tracker>(provider);
            CSK_Store_ProductVotesSums = new Query<CSK_Store_ProductVotesSum>(provider);
            aspnet_SchemaVersions = new Query<aspnet_SchemaVersion>(provider);
            CSK_Store_Tools = new Query<CSK_Store_Tool>(provider);
            CSK_Store_RetailerProductAlerts = new Query<CSK_Store_RetailerProductAlert>(provider);
            CSK_Store_ReviewUsefuls = new Query<CSK_Store_ReviewUseful>(provider);
            CSK_Store_ProductStatuses = new Query<CSK_Store_ProductStatus>(provider);
            CSK_Store_RetailerSTs = new Query<CSK_Store_RetailerST>(provider);
            CSK_Store_NewsLetters = new Query<CSK_Store_NewsLetter>(provider);
            CSK_Stats_Behaviors = new Query<CSK_Stats_Behavior>(provider);
            CSK_Store_ReviewUsefulTypes = new Query<CSK_Store_ReviewUsefulType>(provider);
            CSK_Store_ShoppingTrackers = new Query<CSK_Store_ShoppingTracker>(provider);
            CSK_Store_AttributeMatches = new Query<CSK_Store_AttributeMatch>(provider);
            CSK_Store_RetailerStoreTypes = new Query<CSK_Store_RetailerStoreType>(provider);
            CSK_Store_ProductTypes = new Query<CSK_Store_ProductType>(provider);
            CSK_Store_AutomaticRemoveSpaces = new Query<CSK_Store_AutomaticRemoveSpace>(provider);
            CSK_Store_PPCMemmberHistories = new Query<CSK_Store_PPCMemmberHistory>(provider);
            CSK_Store_DailyDealsCategories = new Query<CSK_Store_DailyDealsCategory>(provider);
            CSk_Store_AutomaticCategorySeries = new Query<CSk_Store_AutomaticCategorySeries>(provider);
            CSK_Store_ProductRetailerCountHistories = new Query<CSK_Store_ProductRetailerCountHistory>(provider);
            aspnet_Memberships = new Query<aspnet_Membership>(provider);
            CSK_Store_ProductVideos = new Query<CSK_Store_ProductVideo>(provider);
            CSK_Store_DailyDealsRetailers = new Query<CSK_Store_DailyDealsRetailer>(provider);
            CSK_Store_UserLocations = new Query<CSK_Store_UserLocation>(provider);
            CSK_Store_RetailerProductlibraries = new Query<CSK_Store_RetailerProductlibrary>(provider);
            CSK_Store_AttributeGroups = new Query<CSK_Store_AttributeGroup>(provider);
            Store_CrawlClassTypes = new Query<Store_CrawlClassType>(provider);
            CSK_Store_FreightLocations = new Query<CSK_Store_FreightLocation>(provider);
            CSK_Store_ConsumerPriceMeMappings = new Query<CSK_Store_ConsumerPriceMeMapping>(provider);
            CSK_Store_RetailerTrackerlibraries = new Query<CSK_Store_RetailerTrackerlibrary>(provider);
            CSK_Store_RetailerTrackers = new Query<CSK_Store_RetailerTracker>(provider);
            CSK_Store_GoodSearchKeywords = new Query<CSK_Store_GoodSearchKeyword>(provider);
            CSK_Store_RetailerCrawlerInfos = new Query<CSK_Store_RetailerCrawlerInfo>(provider);
            CategoryFooterMaps = new Query<CategoryFooterMap>(provider);
            CSK_Store_PreviousNewsletters = new Query<CSK_Store_PreviousNewsletter>(provider);
            CALC_RetailerCategory_Clicks = new Query<CALC_RetailerCategory_Click>(provider);
            CSK_Store_RelatedCategory_Maps = new Query<CSK_Store_RelatedCategory_Map>(provider);
            CSK_Store_Retailers = new Query<CSK_Store_Retailer>(provider);
            CSK_Store_AttributeTypes = new Query<CSK_Store_AttributeType>(provider);
            CSK_Store_RetailerCities = new Query<CSK_Store_RetailerCity>(provider);
            CSK_Store_SearchKeywordsRules = new Query<CSK_Store_SearchKeywordsRule>(provider);
            CSK_Store_EECAData = new Query<CSK_Store_EECADatum>(provider);
            CSK_Store_GovernmentBadges = new Query<CSK_Store_GovernmentBadge>(provider);
            CSK_Store_ProductVideoReviews = new Query<CSK_Store_ProductVideoReview>(provider);
            CSK_Store_RetailerFreights = new Query<CSK_Store_RetailerFreight>(provider);
            CSK_Store_ProductVideoSources = new Query<CSK_Store_ProductVideoSource>(provider);
            CSK_Store_YPricemeSpaceIDs = new Query<CSK_Store_YPricemeSpaceID>(provider);
            CSK_Store_RetailerRatings = new Query<CSK_Store_RetailerRating>(provider);
            CSK_Store_AttributeValues = new Query<CSK_Store_AttributeValue>(provider);
            CSK_Store_Lists = new Query<CSK_Store_List>(provider);
            CSK_Store_CategoryTotals = new Query<CSK_Store_CategoryTotal>(provider);
            CSK_Store_AutomaticMappingNoModels = new Query<CSK_Store_AutomaticMappingNoModel>(provider);
            CSK_Store_MobileRetailerTrackers = new Query<CSK_Store_MobileRetailerTracker>(provider);
            Fisher_and_Paykel_Maps = new Query<Fisher_and_Paykel_Map>(provider);
            CSK_Store_ListTypes = new Query<CSK_Store_ListType>(provider);
            Csk_store_redirectforRetailerproducts = new Query<Csk_store_redirectforRetailerproduct>(provider);
            #endregion


            #region ' Schemas '
        	if(DataProvider.Schema.Tables.Count == 0)
			{
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductLibraryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Partner_Topic_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerFreightLocationTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_DailyDealsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductListTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_ProfileTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ShareWishListTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_Import_MapNotSupportedCategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AttributeValueRangeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_FreightLocation_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PurgedProductTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AutomaticSpecialCharacterTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Skykiwi_CategoryName_TranslateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Store_GLatLngTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new RetailerReviewDetailTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCStatsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerOfferTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerStatusTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_MostPopularProductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_BuyingGuideTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ExpertReviewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerImageTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_WelcontentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_FeedTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RangeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new UnitTableTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_RolesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new WidgetAffiliateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ReviewSourceTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerPaymentOptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new HW_Newsletter_EmailTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_OMGRetailers_AffiliateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new MyLogTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_BuyingGuideRelatedTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_TreepodiaVideoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_FeaturedProductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_UsersInRolesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_SimilarProductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_JobFunctionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_UserAgentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerProductNewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_Product_Category_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerLeadSignUpTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSk_Store_RenameProduct_TypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_NewsLetterUserTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_BuyingGuideTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerLeadTrackingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_CrawlerPriorityTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSk_Store_RenameProduct_SettingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerProductConditionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PopularBrandsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCCreditCardTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductReviewCommentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_CategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerProductTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Forum_TopicTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_Retailer_Change_RateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Forum_QPostTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCPaymentOptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Forum_ReportedAbsusTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ManufacturerMapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ReviewSourceAUTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_CommunityWelContentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_NewProductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_PathsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ExceptionCollectTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerKeywordTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_Category_AttributeTitle_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductReviewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Local_CategoryNameTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCCategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_DirectoryCategoryMapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_PersonalizationAllUsersTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ShortCutCategory_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_CategoryBugingGuide_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_LocationCategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_UserSearch_GoogleTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_PersonalizationPerUserTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AdvancedMappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductAlertHistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerAddressTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PriceMe_ExpertAverageRatingTFTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerOperatingHoursTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCAccountTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ECommercePlatformTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerAddressTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Util_CurrencyTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PopularSearchTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ExpertReviewAUTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Util_CountryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_EmailDatumTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new RetailerNewsletterInfoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_IP_BlacklistTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_NewsletterCtxTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new LogTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_CategoryViewTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Store_Compare_Attribute_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_UserSearchTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCBudgetHistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_NewOnlyImportTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ImageTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Inactive_Retailer_ReasonTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PCMTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerCategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_DailyDealsTrackerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerReviewCommentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_BannerTrackerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Store_Compare_AttributesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_TransactionTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ResourcesInfoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCInvoiceRatioTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Messaging_MailerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerReviewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_Advertising_BannerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ManufacturerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ManufacturerLibraryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_Email_FriendTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerTracker_AffiliateIDTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Csk_Store_FileFormatTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_MostPopularCategoriesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_WebEvent_EventsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ListingImageTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PM_ProductDescriptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new RelatedCategoriesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_MembershipInfoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Csk_Store_FeedFieldTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_NewsReleaseTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Csk_Store_FeedFormatTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PaymentOptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_FreightTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_VarietyTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductRatingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new EMailInfoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCMemberDailyBudgetTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_EmailSendOutTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCMemberTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PageFavouritesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ProductFavouritesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_Product404Table(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_ContentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new AutomatedMergingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PriceHistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerRedirectClickTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ZeroProduct_Change_RateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCMemberTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_EnergyTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new TradeMeCategoryMapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCPlanTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductNewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_RetaileriContact_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductAlertTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AutomaticMergingOptionSettingsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_CategoryFeaturedProductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new BiggestDropPriceTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_FeaturedTabTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Csk_Store_FeedFormatMapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_CategoryMergingKeywordTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCTransactionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductDescriptorTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_Import_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AdminInformationTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductViewTrackingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_ApplicationsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new FrequencyTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductDescriptorTitleTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerVotesSumTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new InvalidKeywordsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_FeatureCarouselTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Store_GRegionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_BannerTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductReviewFeedbackTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_UsersTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AutomaticRetailerNoModelTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductIsMergedTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_TempAutoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Stats_TrackerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductVotesSumTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_SchemaVersionsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ToolsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerProductAlertTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ReviewUsefulTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductStatusTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerSTTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_NewsLetterTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Stats_BehaviorTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ReviewUsefulTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ShoppingTrackerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AttributeMatchTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerStoreTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AutomaticRemoveSpaceTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PPCMemmberHistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_DailyDealsCategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSk_Store_AutomaticCategorySeriesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductRetailerCountHistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new aspnet_MembershipTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductVideoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_DailyDealsRetailerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_UserLocationTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerProductlibraryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AttributeGroupTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Store_CrawlClassTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_FreightLocationTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ConsumerPriceMeMappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerTrackerlibraryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerTrackerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_GoodSearchKeywordsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerCrawlerInfoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CategoryFooterMapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_PreviousNewsletterTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CALC_RetailerCategory_ClicksTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RelatedCategory_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AttributeTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerCityTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_SearchKeywordsRuleTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_EECADataTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_GovernmentBadgeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductVideoReviewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerFreightTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ProductVideoSourceTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_YPricemeSpaceIDsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_RetailerRatingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AttributeValueTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ListTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_CategoryTotalTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_AutomaticMappingNoModelTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_MobileRetailerTrackerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Fisher_and_Paykel_MapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CSK_Store_ListTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Csk_store_redirectforRetailerproductTable(DataProvider));
            }
            #endregion
        }
        

        #region ' Helpers '
            
        internal static DateTime DateTimeNowTruncatedDownToSecond() {
            var now = DateTime.Now;
            return now.AddTicks(-now.Ticks % TimeSpan.TicksPerSecond);
        }

        #endregion

    }
}