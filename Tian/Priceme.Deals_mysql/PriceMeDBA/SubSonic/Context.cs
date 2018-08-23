




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

namespace PriceMeDBA
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
                DataProvider = ProviderFactory.GetProvider("CommerceTemplate");
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
        public Query<CSK_Store_RetailerProductAlert> CSK_Store_RetailerProductAlerts { get; set; }
        public Query<CSK_Store_FreightLocation_Map> CSK_Store_FreightLocation_Maps { get; set; }
        public Query<CSK_Store_AutomaticSpecialCharacter> CSK_Store_AutomaticSpecialCharacters { get; set; }
        public Query<CSK_Store_YPricemeSpaceID> CSK_Store_YPricemeSpaceIDs { get; set; }
        public Query<CSK_Store_RetailerCity> CSK_Store_RetailerCities { get; set; }
        public Query<CSK_Store_RetailerStatus> CSK_Store_RetailerStatuses { get; set; }
        public Query<CSK_Store_MostPopularProduct> CSK_Store_MostPopularProducts { get; set; }
        public Query<CSK_Store_Range> CSK_Store_Ranges { get; set; }
        public Query<WidgetAffiliate> WidgetAffiliates { get; set; }
        public Query<CSK_Store_RetailerFreight> CSK_Store_RetailerFreights { get; set; }
        public Query<CSK_Store_RetailerKeyword> CSK_Store_RetailerKeywords { get; set; }
        public Query<MyLog> MyLogs { get; set; }
        public Query<CSK_Store_FeaturedProduct> CSK_Store_FeaturedProducts { get; set; }
        public Query<CSK_Store_SimilarProduct> CSK_Store_SimilarProducts { get; set; }
        public Query<CSK_Store_UserAgent> CSK_Store_UserAgents { get; set; }
        public Query<CSK_Store_NewProduct> CSK_Store_NewProducts { get; set; }
        public Query<CSK_Store_RetailerFreightLocation> CSK_Store_RetailerFreightLocations { get; set; }
        public Query<CSk_Store_RenameProduct_Type> CSk_Store_RenameProduct_Types { get; set; }
        public Query<CSK_Store_NewsLetterUser> CSK_Store_NewsLetterUsers { get; set; }
        public Query<CSK_Store_AttributeType> CSK_Store_AttributeTypes { get; set; }
        public Query<CSK_Store_CrawlerPriority> CSK_Store_CrawlerPriorities { get; set; }
        public Query<CSk_Store_RenameProduct_Setting> CSk_Store_RenameProduct_Settings { get; set; }
        public Query<CSK_Store_PopularBrand> CSK_Store_PopularBrands { get; set; }
        public Query<CSK_Store_OMGRetailers_Affiliate> CSK_Store_OMGRetailers_Affiliates { get; set; }
        public Query<CSK_Store_PPCCreditCard> CSK_Store_PPCCreditCards { get; set; }
        public Query<CSK_Store_ProductReviewComment> CSK_Store_ProductReviewComments { get; set; }
        public Query<CSK_Store_Retailer_Change_Rate> CSK_Store_Retailer_Change_Rates { get; set; }
        public Query<CSK_Store_PPCPaymentOption> CSK_Store_PPCPaymentOptions { get; set; }
        public Query<CSK_Store_PPCAccount> CSK_Store_PPCAccounts { get; set; }
        public Query<CSK_Store_ExceptionCollect> CSK_Store_ExceptionCollects { get; set; }
        public Query<TMPExceptCatIDSTAB> TMPExceptCatIDSTABs { get; set; }
        public Query<TMPCatsTAB> TMPCatsTABs { get; set; }
        public Query<CSK_Store_PPCBudgetHistory> CSK_Store_PPCBudgetHistories { get; set; }
        public Query<TMPHIH> TMPHIHS { get; set; }
        public Query<CSK_Store_PPCCategory> CSK_Store_PPCCategories { get; set; }
        public Query<TMPTB> TMPTBS { get; set; }
        public Query<RetailerNewsletterInfo> RetailerNewsletterInfos { get; set; }
        public Query<CSK_Store_AdvancedMapping> CSK_Store_AdvancedMappings { get; set; }
        public Query<CSK_Store_RetailerAddressType> CSK_Store_RetailerAddressTypes { get; set; }
        public Query<CSK_Store_PPCInvoiceRatio> CSK_Store_PPCInvoiceRatios { get; set; }
        public Query<CSK_Store_RetailerOperatingHour> CSK_Store_RetailerOperatingHours { get; set; }
        public Query<CSK_Store_RetailerAddress> CSK_Store_RetailerAddresses { get; set; }
        public Query<CSK_Util_Currency> CSK_Util_Currencies { get; set; }
        public Query<Log> Logs { get; set; }
        public Query<CSK_Store_UserSearch> CSK_Store_UserSearches { get; set; }
        public Query<CSK_Store_NewOnlyImport> CSK_Store_NewOnlyImports { get; set; }
        public Query<PCM> PCMS { get; set; }
        public Query<CALC_RetailerCategory_Click> CALC_RetailerCategory_Clicks { get; set; }
        public Query<UnitTable> UnitTables { get; set; }
        public Query<CSK_Forum_Topic> CSK_Forum_Topics { get; set; }
        public Query<CSK_Store_DailyDealsRetailer> CSK_Store_DailyDealsRetailers { get; set; }
        public Query<CSK_Store_RetailerCategory> CSK_Store_RetailerCategories { get; set; }
        public Query<CSK_Forum_QPost> CSK_Forum_QPosts { get; set; }
        public Query<CSK_Store_PPCMemberDailyBudget> CSK_Store_PPCMemberDailyBudgets { get; set; }
        public Query<CSK_Store_RetailerReviewComment> CSK_Store_RetailerReviewComments { get; set; }
        public Query<CSK_Store_BannerTracker> CSK_Store_BannerTrackers { get; set; }
        public Query<CSK_Store_TransactionType> CSK_Store_TransactionTypes { get; set; }
        public Query<CSK_Forum_ReportedAbsu> CSK_Forum_ReportedAbsus { get; set; }
        public Query<CSK_Messaging_Mailer> CSK_Messaging_Mailers { get; set; }
        public Query<CSK_Store_Advertising_Banner> CSK_Store_Advertising_Banners { get; set; }
        public Query<CSK_Store_Email_Friend> CSK_Store_Email_Friends { get; set; }
        public Query<CSK_Store_ListingImage> CSK_Store_ListingImages { get; set; }
        public Query<CSK_Store_FreightType> CSK_Store_FreightTypes { get; set; }
        public Query<CSK_Store_Variety> CSK_Store_Varieties { get; set; }
        public Query<CSK_RetaileriContact_Map> CSK_RetaileriContact_Maps { get; set; }
        public Query<CSK_Store_ProductAlertHistory> CSK_Store_ProductAlertHistories { get; set; }
        public Query<CSK_Store_RetailerST> CSK_Store_RetailerSTs { get; set; }
        public Query<Fisher_and_Paykel_Map> Fisher_and_Paykel_Maps { get; set; }
        public Query<RelatedCategory> RelatedCategories { get; set; }
        public Query<PopularSearch> PopularSearches { get; set; }
        public Query<CSK_Store_DailyDealsCategory> CSK_Store_DailyDealsCategories { get; set; }
        public Query<CSK_Store_ZeroProduct_Change_Rate> CSK_Store_ZeroProduct_Change_Rates { get; set; }
        public Query<TradeMeCategoryMap> TradeMeCategoryMaps { get; set; }
        public Query<CSK_Store_MostPopularCategory> CSK_Store_MostPopularCategories { get; set; }
        public Query<CSK_Store_CategoryFeaturedProduct> CSK_Store_CategoryFeaturedProducts { get; set; }
        public Query<BiggestDropPrice> BiggestDropPrices { get; set; }
        public Query<CSK_Store_CategoryMergingKeyword> CSK_Store_CategoryMergingKeywords { get; set; }
        public Query<CSK_Store_DailyDeal> CSK_Store_DailyDeals { get; set; }
        public Query<CSK_Store_AdminInformation> CSK_Store_AdminInformations { get; set; }
        public Query<CSK_Store_FeatureCarousel> CSK_Store_FeatureCarousels { get; set; }
        public Query<CSK_Store_BannerType> CSK_Store_BannerTypes { get; set; }
        public Query<CSK_Store_ProductReviewFeedback> CSK_Store_ProductReviewFeedbacks { get; set; }
        public Query<CSK_Store_ProductStatus> CSK_Store_ProductStatuses { get; set; }
        public Query<CSK_Stats_Tracker> CSK_Stats_Trackers { get; set; }
        public Query<CSK_Store_ReviewUseful> CSK_Store_ReviewUsefuls { get; set; }
        public Query<CSK_Store_ExpertReview> CSK_Store_ExpertReviews { get; set; }
        public Query<CSK_Store_ReviewUsefulType> CSK_Store_ReviewUsefulTypes { get; set; }
        public Query<CSK_Store_ProductType> CSK_Store_ProductTypes { get; set; }
        public Query<CSK_Stats_Behavior> CSK_Stats_Behaviors { get; set; }
        public Query<CSK_Store_ReviewSource> CSK_Store_ReviewSources { get; set; }
        public Query<CSK_Store_PPCMemmberHistory> CSK_Store_PPCMemmberHistories { get; set; }
        public Query<CSK_Store_RetailerCrawlerInfo> CSK_Store_RetailerCrawlerInfos { get; set; }
        public Query<CSK_Store_AttributeMatch> CSK_Store_AttributeMatches { get; set; }
        public Query<CSK_Store_UserLocation> CSK_Store_UserLocations { get; set; }
        public Query<CSK_Store_TreepodiaVideo> CSK_Store_TreepodiaVideos { get; set; }
        public Query<CSK_Store_RetailerProductlibrary> CSK_Store_RetailerProductlibraries { get; set; }
        public Query<Store_CrawlClassType> Store_CrawlClassTypes { get; set; }
        public Query<CSK_Store_EECADatum> CSK_Store_EECAData { get; set; }
        public Query<CSK_Store_GoodSearchKeyword> CSK_Store_GoodSearchKeywords { get; set; }
        public Query<CSK_Store_RelatedCategory_Map> CSK_Store_RelatedCategory_Maps { get; set; }
        public Query<CSK_Store_UserSearch_Google> CSK_Store_UserSearch_Googles { get; set; }
        public Query<CSK_Store_SearchKeywordsRule> CSK_Store_SearchKeywordsRules { get; set; }
        public Query<CSK_Store_CategoryTotal> CSK_Store_CategoryTotals { get; set; }
        public Query<CategoryFooterMap> CategoryFooterMaps { get; set; }
        public Query<CSK_Store_MobileRetailerTracker> CSK_Store_MobileRetailerTrackers { get; set; }
        public Query<CSK_Store_CategoryViewType> CSK_Store_CategoryViewTypes { get; set; }
        public Query<CSK_Store_FreightLocation> CSK_Store_FreightLocations { get; set; }
        public Query<CSK_Content> CSK_Contents { get; set; }
        public Query<CSK_Store_AttributeGroup> CSK_Store_AttributeGroups { get; set; }
        public Query<CSK_Store_AttributeValue> CSK_Store_AttributeValues { get; set; }
        public Query<CSK_Store_AttributeValueRange> CSK_Store_AttributeValueRanges { get; set; }
        public Query<CSK_Store_BuyingGuide> CSK_Store_BuyingGuides { get; set; }
        public Query<CSK_Store_BuyingGuideRelated> CSK_Store_BuyingGuideRelateds { get; set; }
        public Query<CSK_Store_BuyingGuideType> CSK_Store_BuyingGuideTypes { get; set; }
        public Query<CSK_Store_Category> CSK_Store_Categories { get; set; }
        public Query<CSK_Store_Category_AttributeTitle_Map> CSK_Store_Category_AttributeTitle_Maps { get; set; }
        public Query<CSK_Store_CategoryBugingGuide_Map> CSK_Store_CategoryBugingGuide_Maps { get; set; }
        public Query<CSK_Store_CommunityWelContent> CSK_Store_CommunityWelContents { get; set; }
        public Query<CSK_Store_ConsumerPriceMeMapping> CSK_Store_ConsumerPriceMeMappings { get; set; }
        public Query<CSK_Store_ECommercePlatform> CSK_Store_ECommercePlatforms { get; set; }
        public Query<CSK_Store_EmailDatum> CSK_Store_EmailData { get; set; }
        public Query<CSK_Store_EmailSendOut> CSK_Store_EmailSendOuts { get; set; }
        public Query<CSK_Store_Energy> CSK_Store_Energies { get; set; }
        public Query<CSK_Store_ExpertReviewAU> CSK_Store_ExpertReviewAUs { get; set; }
        public Query<CSK_Store_FeaturedTab> CSK_Store_FeaturedTabs { get; set; }
        public Query<CSK_Store_GovernmentBadge> CSK_Store_GovernmentBadges { get; set; }
        public Query<CSK_Store_Image> CSK_Store_Images { get; set; }
        public Query<CSK_Store_JobFunction> CSK_Store_JobFunctions { get; set; }
        public Query<CSK_Store_List> CSK_Store_Lists { get; set; }
        public Query<CSK_Store_ListType> CSK_Store_ListTypes { get; set; }
        public Query<CSK_Store_LocationCategory> CSK_Store_LocationCategories { get; set; }
        public Query<CSK_Store_Manufacturer> CSK_Store_Manufacturers { get; set; }
        public Query<CSK_Store_NewsLetter> CSK_Store_NewsLetters { get; set; }
        public Query<CSK_Store_NewsletterCtx> CSK_Store_NewsletterCtxes { get; set; }
        public Query<CSK_Store_NewsRelease> CSK_Store_NewsReleases { get; set; }
        public Query<CSK_Store_PaymentOption> CSK_Store_PaymentOptions { get; set; }
        public Query<CSK_Store_PPCMember> CSK_Store_PPCMembers { get; set; }
        public Query<CSK_Store_PPCMemberType> CSK_Store_PPCMemberTypes { get; set; }
        public Query<CSK_Store_PreviousNewsletter> CSK_Store_PreviousNewsletters { get; set; }
        public Query<CSK_Store_PriceHistory> CSK_Store_PriceHistories { get; set; }
        public Query<CSK_Store_Product> CSK_Store_Products { get; set; }
        public Query<CSK_Store_ProductAlert> CSK_Store_ProductAlerts { get; set; }
        public Query<CSK_Store_ProductDescriptor> CSK_Store_ProductDescriptors { get; set; }
        public Query<CSK_Store_ProductDescriptorTitle> CSK_Store_ProductDescriptorTitles { get; set; }
        public Query<CSK_Store_ProductIsMerged> CSK_Store_ProductIsMergeds { get; set; }
        public Query<CSK_Store_ProductList> CSK_Store_ProductLists { get; set; }
        public Query<CSK_Store_ProductNew> CSK_Store_ProductNews { get; set; }
        public Query<CSK_Store_ProductRating> CSK_Store_ProductRatings { get; set; }
        public Query<CSK_Store_ProductRetailerCountHistory> CSK_Store_ProductRetailerCountHistories { get; set; }
        public Query<CSK_Store_ProductReview> CSK_Store_ProductReviews { get; set; }
        public Query<CSK_Store_ProductVideoReview> CSK_Store_ProductVideoReviews { get; set; }
        public Query<CSK_Store_ProductVideoSource> CSK_Store_ProductVideoSources { get; set; }
        public Query<CSK_Store_ProductViewTracking> CSK_Store_ProductViewTrackings { get; set; }
        public Query<CSK_Store_ProductVotesSum> CSK_Store_ProductVotesSums { get; set; }
        public Query<csk_store_redirectforretailerproduct> csk_store_redirectforretailerproducts { get; set; }
        public Query<csk_store_RelatedPart> csk_store_RelatedParts { get; set; }
        public Query<CSK_Store_Retailer> CSK_Store_Retailers { get; set; }
        public Query<CSK_Store_RetailerImage> CSK_Store_RetailerImages { get; set; }
        public Query<CSK_Store_RetailerLeadSignUp> CSK_Store_RetailerLeadSignUps { get; set; }
        public Query<CSK_Store_RetailerLeadTracking> CSK_Store_RetailerLeadTrackings { get; set; }
        public Query<CSK_Store_RetailerOffer> CSK_Store_RetailerOffers { get; set; }
        public Query<CSK_Store_RetailerPaymentOption> CSK_Store_RetailerPaymentOptions { get; set; }
        public Query<CSK_Store_RetailerProduct> CSK_Store_RetailerProducts { get; set; }
        public Query<CSK_Store_RetailerProductCondition> CSK_Store_RetailerProductConditions { get; set; }
        public Query<CSK_Store_RetailerProductNew> CSK_Store_RetailerProductNews { get; set; }
        public Query<CSK_Store_RetailerRating> CSK_Store_RetailerRatings { get; set; }
        public Query<CSK_Store_RetailerRedirectClick> CSK_Store_RetailerRedirectClicks { get; set; }
        public Query<CSK_Store_RetailerReview> CSK_Store_RetailerReviews { get; set; }
        public Query<CSK_Store_RetailerStoreType> CSK_Store_RetailerStoreTypes { get; set; }
        public Query<CSK_Store_RetailerVotesSum> CSK_Store_RetailerVotesSums { get; set; }
        public Query<CSK_Store_ReviewSourceAU> CSK_Store_ReviewSourceAUs { get; set; }
        public Query<CSK_Store_ShareWishList> CSK_Store_ShareWishLists { get; set; }
        public Query<CSK_Store_ShoppingTracker> CSK_Store_ShoppingTrackers { get; set; }
        public Query<CSK_Store_Welcontent> CSK_Store_Welcontents { get; set; }
        public Query<CSK_Util_Country> CSK_Util_Countries { get; set; }
        public Query<EMailInfo> EMailInfos { get; set; }
        public Query<FrequencyType> FrequencyTypes { get; set; }
        public Query<Inactive_Retailer_Reason> Inactive_Retailer_Reasons { get; set; }
        public Query<InvalidKeyword> InvalidKeywords { get; set; }
        public Query<Local_CategoryName> Local_CategoryNames { get; set; }
        public Query<MoneyPlan> MoneyPlans { get; set; }
        public Query<PageFavourite> PageFavourites { get; set; }
        public Query<Partner_Topic_Map> Partner_Topic_Maps { get; set; }
        public Query<ProductFavourite> ProductFavourites { get; set; }
        public Query<PurgedProduct> PurgedProducts { get; set; }
        public Query<ResourcesInfo> ResourcesInfos { get; set; }
        public Query<RetailerReviewDetail> RetailerReviewDetails { get; set; }
        public Query<ShortCutCategory_Map> ShortCutCategory_Maps { get; set; }
        public Query<Skykiwi_CategoryName_Translate> Skykiwi_CategoryName_Translates { get; set; }
        public Query<Store_Compare_Attribute_Map> Store_Compare_Attribute_Maps { get; set; }
        public Query<Store_Compare_Attribute> Store_Compare_Attributes { get; set; }
        public Query<Store_GLatLng> Store_GLatLngs { get; set; }
        public Query<Store_GRegion> Store_GRegions { get; set; }
        public Query<WeeklyDealsUser> WeeklyDealsUsers { get; set; }

			

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
            CSK_Store_RetailerProductAlerts = new Query<CSK_Store_RetailerProductAlert>(provider);
            CSK_Store_FreightLocation_Maps = new Query<CSK_Store_FreightLocation_Map>(provider);
            CSK_Store_AutomaticSpecialCharacters = new Query<CSK_Store_AutomaticSpecialCharacter>(provider);
            CSK_Store_YPricemeSpaceIDs = new Query<CSK_Store_YPricemeSpaceID>(provider);
            CSK_Store_RetailerCities = new Query<CSK_Store_RetailerCity>(provider);
            CSK_Store_RetailerStatuses = new Query<CSK_Store_RetailerStatus>(provider);
            CSK_Store_MostPopularProducts = new Query<CSK_Store_MostPopularProduct>(provider);
            CSK_Store_Ranges = new Query<CSK_Store_Range>(provider);
            WidgetAffiliates = new Query<WidgetAffiliate>(provider);
            CSK_Store_RetailerFreights = new Query<CSK_Store_RetailerFreight>(provider);
            CSK_Store_RetailerKeywords = new Query<CSK_Store_RetailerKeyword>(provider);
            MyLogs = new Query<MyLog>(provider);
            CSK_Store_FeaturedProducts = new Query<CSK_Store_FeaturedProduct>(provider);
            CSK_Store_SimilarProducts = new Query<CSK_Store_SimilarProduct>(provider);
            CSK_Store_UserAgents = new Query<CSK_Store_UserAgent>(provider);
            CSK_Store_NewProducts = new Query<CSK_Store_NewProduct>(provider);
            CSK_Store_RetailerFreightLocations = new Query<CSK_Store_RetailerFreightLocation>(provider);
            CSk_Store_RenameProduct_Types = new Query<CSk_Store_RenameProduct_Type>(provider);
            CSK_Store_NewsLetterUsers = new Query<CSK_Store_NewsLetterUser>(provider);
            CSK_Store_AttributeTypes = new Query<CSK_Store_AttributeType>(provider);
            CSK_Store_CrawlerPriorities = new Query<CSK_Store_CrawlerPriority>(provider);
            CSk_Store_RenameProduct_Settings = new Query<CSk_Store_RenameProduct_Setting>(provider);
            CSK_Store_PopularBrands = new Query<CSK_Store_PopularBrand>(provider);
            CSK_Store_OMGRetailers_Affiliates = new Query<CSK_Store_OMGRetailers_Affiliate>(provider);
            CSK_Store_PPCCreditCards = new Query<CSK_Store_PPCCreditCard>(provider);
            CSK_Store_ProductReviewComments = new Query<CSK_Store_ProductReviewComment>(provider);
            CSK_Store_Retailer_Change_Rates = new Query<CSK_Store_Retailer_Change_Rate>(provider);
            CSK_Store_PPCPaymentOptions = new Query<CSK_Store_PPCPaymentOption>(provider);
            CSK_Store_PPCAccounts = new Query<CSK_Store_PPCAccount>(provider);
            CSK_Store_ExceptionCollects = new Query<CSK_Store_ExceptionCollect>(provider);
            TMPExceptCatIDSTABs = new Query<TMPExceptCatIDSTAB>(provider);
            TMPCatsTABs = new Query<TMPCatsTAB>(provider);
            CSK_Store_PPCBudgetHistories = new Query<CSK_Store_PPCBudgetHistory>(provider);
            TMPHIHS = new Query<TMPHIH>(provider);
            CSK_Store_PPCCategories = new Query<CSK_Store_PPCCategory>(provider);
            TMPTBS = new Query<TMPTB>(provider);
            RetailerNewsletterInfos = new Query<RetailerNewsletterInfo>(provider);
            CSK_Store_AdvancedMappings = new Query<CSK_Store_AdvancedMapping>(provider);
            CSK_Store_RetailerAddressTypes = new Query<CSK_Store_RetailerAddressType>(provider);
            CSK_Store_PPCInvoiceRatios = new Query<CSK_Store_PPCInvoiceRatio>(provider);
            CSK_Store_RetailerOperatingHours = new Query<CSK_Store_RetailerOperatingHour>(provider);
            CSK_Store_RetailerAddresses = new Query<CSK_Store_RetailerAddress>(provider);
            CSK_Util_Currencies = new Query<CSK_Util_Currency>(provider);
            Logs = new Query<Log>(provider);
            CSK_Store_UserSearches = new Query<CSK_Store_UserSearch>(provider);
            CSK_Store_NewOnlyImports = new Query<CSK_Store_NewOnlyImport>(provider);
            PCMS = new Query<PCM>(provider);
            CALC_RetailerCategory_Clicks = new Query<CALC_RetailerCategory_Click>(provider);
            UnitTables = new Query<UnitTable>(provider);
            CSK_Forum_Topics = new Query<CSK_Forum_Topic>(provider);
            CSK_Store_DailyDealsRetailers = new Query<CSK_Store_DailyDealsRetailer>(provider);
            CSK_Store_RetailerCategories = new Query<CSK_Store_RetailerCategory>(provider);
            CSK_Forum_QPosts = new Query<CSK_Forum_QPost>(provider);
            CSK_Store_PPCMemberDailyBudgets = new Query<CSK_Store_PPCMemberDailyBudget>(provider);
            CSK_Store_RetailerReviewComments = new Query<CSK_Store_RetailerReviewComment>(provider);
            CSK_Store_BannerTrackers = new Query<CSK_Store_BannerTracker>(provider);
            CSK_Store_TransactionTypes = new Query<CSK_Store_TransactionType>(provider);
            CSK_Forum_ReportedAbsus = new Query<CSK_Forum_ReportedAbsu>(provider);
            CSK_Messaging_Mailers = new Query<CSK_Messaging_Mailer>(provider);
            CSK_Store_Advertising_Banners = new Query<CSK_Store_Advertising_Banner>(provider);
            CSK_Store_Email_Friends = new Query<CSK_Store_Email_Friend>(provider);
            CSK_Store_ListingImages = new Query<CSK_Store_ListingImage>(provider);
            CSK_Store_FreightTypes = new Query<CSK_Store_FreightType>(provider);
            CSK_Store_Varieties = new Query<CSK_Store_Variety>(provider);
            CSK_RetaileriContact_Maps = new Query<CSK_RetaileriContact_Map>(provider);
            CSK_Store_ProductAlertHistories = new Query<CSK_Store_ProductAlertHistory>(provider);
            CSK_Store_RetailerSTs = new Query<CSK_Store_RetailerST>(provider);
            Fisher_and_Paykel_Maps = new Query<Fisher_and_Paykel_Map>(provider);
            RelatedCategories = new Query<RelatedCategory>(provider);
            PopularSearches = new Query<PopularSearch>(provider);
            CSK_Store_DailyDealsCategories = new Query<CSK_Store_DailyDealsCategory>(provider);
            CSK_Store_ZeroProduct_Change_Rates = new Query<CSK_Store_ZeroProduct_Change_Rate>(provider);
            TradeMeCategoryMaps = new Query<TradeMeCategoryMap>(provider);
            CSK_Store_MostPopularCategories = new Query<CSK_Store_MostPopularCategory>(provider);
            CSK_Store_CategoryFeaturedProducts = new Query<CSK_Store_CategoryFeaturedProduct>(provider);
            BiggestDropPrices = new Query<BiggestDropPrice>(provider);
            CSK_Store_CategoryMergingKeywords = new Query<CSK_Store_CategoryMergingKeyword>(provider);
            CSK_Store_DailyDeals = new Query<CSK_Store_DailyDeal>(provider);
            CSK_Store_AdminInformations = new Query<CSK_Store_AdminInformation>(provider);
            CSK_Store_FeatureCarousels = new Query<CSK_Store_FeatureCarousel>(provider);
            CSK_Store_BannerTypes = new Query<CSK_Store_BannerType>(provider);
            CSK_Store_ProductReviewFeedbacks = new Query<CSK_Store_ProductReviewFeedback>(provider);
            CSK_Store_ProductStatuses = new Query<CSK_Store_ProductStatus>(provider);
            CSK_Stats_Trackers = new Query<CSK_Stats_Tracker>(provider);
            CSK_Store_ReviewUsefuls = new Query<CSK_Store_ReviewUseful>(provider);
            CSK_Store_ExpertReviews = new Query<CSK_Store_ExpertReview>(provider);
            CSK_Store_ReviewUsefulTypes = new Query<CSK_Store_ReviewUsefulType>(provider);
            CSK_Store_ProductTypes = new Query<CSK_Store_ProductType>(provider);
            CSK_Stats_Behaviors = new Query<CSK_Stats_Behavior>(provider);
            CSK_Store_ReviewSources = new Query<CSK_Store_ReviewSource>(provider);
            CSK_Store_PPCMemmberHistories = new Query<CSK_Store_PPCMemmberHistory>(provider);
            CSK_Store_RetailerCrawlerInfos = new Query<CSK_Store_RetailerCrawlerInfo>(provider);
            CSK_Store_AttributeMatches = new Query<CSK_Store_AttributeMatch>(provider);
            CSK_Store_UserLocations = new Query<CSK_Store_UserLocation>(provider);
            CSK_Store_TreepodiaVideos = new Query<CSK_Store_TreepodiaVideo>(provider);
            CSK_Store_RetailerProductlibraries = new Query<CSK_Store_RetailerProductlibrary>(provider);
            Store_CrawlClassTypes = new Query<Store_CrawlClassType>(provider);
            CSK_Store_EECAData = new Query<CSK_Store_EECADatum>(provider);
            CSK_Store_GoodSearchKeywords = new Query<CSK_Store_GoodSearchKeyword>(provider);
            CSK_Store_RelatedCategory_Maps = new Query<CSK_Store_RelatedCategory_Map>(provider);
            CSK_Store_UserSearch_Googles = new Query<CSK_Store_UserSearch_Google>(provider);
            CSK_Store_SearchKeywordsRules = new Query<CSK_Store_SearchKeywordsRule>(provider);
            CSK_Store_CategoryTotals = new Query<CSK_Store_CategoryTotal>(provider);
            CategoryFooterMaps = new Query<CategoryFooterMap>(provider);
            CSK_Store_MobileRetailerTrackers = new Query<CSK_Store_MobileRetailerTracker>(provider);
            CSK_Store_CategoryViewTypes = new Query<CSK_Store_CategoryViewType>(provider);
            CSK_Store_FreightLocations = new Query<CSK_Store_FreightLocation>(provider);
            CSK_Contents = new Query<CSK_Content>(provider);
            CSK_Store_AttributeGroups = new Query<CSK_Store_AttributeGroup>(provider);
            CSK_Store_AttributeValues = new Query<CSK_Store_AttributeValue>(provider);
            CSK_Store_AttributeValueRanges = new Query<CSK_Store_AttributeValueRange>(provider);
            CSK_Store_BuyingGuides = new Query<CSK_Store_BuyingGuide>(provider);
            CSK_Store_BuyingGuideRelateds = new Query<CSK_Store_BuyingGuideRelated>(provider);
            CSK_Store_BuyingGuideTypes = new Query<CSK_Store_BuyingGuideType>(provider);
            CSK_Store_Categories = new Query<CSK_Store_Category>(provider);
            CSK_Store_Category_AttributeTitle_Maps = new Query<CSK_Store_Category_AttributeTitle_Map>(provider);
            CSK_Store_CategoryBugingGuide_Maps = new Query<CSK_Store_CategoryBugingGuide_Map>(provider);
            CSK_Store_CommunityWelContents = new Query<CSK_Store_CommunityWelContent>(provider);
            CSK_Store_ConsumerPriceMeMappings = new Query<CSK_Store_ConsumerPriceMeMapping>(provider);
            CSK_Store_ECommercePlatforms = new Query<CSK_Store_ECommercePlatform>(provider);
            CSK_Store_EmailData = new Query<CSK_Store_EmailDatum>(provider);
            CSK_Store_EmailSendOuts = new Query<CSK_Store_EmailSendOut>(provider);
            CSK_Store_Energies = new Query<CSK_Store_Energy>(provider);
            CSK_Store_ExpertReviewAUs = new Query<CSK_Store_ExpertReviewAU>(provider);
            CSK_Store_FeaturedTabs = new Query<CSK_Store_FeaturedTab>(provider);
            CSK_Store_GovernmentBadges = new Query<CSK_Store_GovernmentBadge>(provider);
            CSK_Store_Images = new Query<CSK_Store_Image>(provider);
            CSK_Store_JobFunctions = new Query<CSK_Store_JobFunction>(provider);
            CSK_Store_Lists = new Query<CSK_Store_List>(provider);
            CSK_Store_ListTypes = new Query<CSK_Store_ListType>(provider);
            CSK_Store_LocationCategories = new Query<CSK_Store_LocationCategory>(provider);
            CSK_Store_Manufacturers = new Query<CSK_Store_Manufacturer>(provider);
            CSK_Store_NewsLetters = new Query<CSK_Store_NewsLetter>(provider);
            CSK_Store_NewsletterCtxes = new Query<CSK_Store_NewsletterCtx>(provider);
            CSK_Store_NewsReleases = new Query<CSK_Store_NewsRelease>(provider);
            CSK_Store_PaymentOptions = new Query<CSK_Store_PaymentOption>(provider);
            CSK_Store_PPCMembers = new Query<CSK_Store_PPCMember>(provider);
            CSK_Store_PPCMemberTypes = new Query<CSK_Store_PPCMemberType>(provider);
            CSK_Store_PreviousNewsletters = new Query<CSK_Store_PreviousNewsletter>(provider);
            CSK_Store_PriceHistories = new Query<CSK_Store_PriceHistory>(provider);
            CSK_Store_Products = new Query<CSK_Store_Product>(provider);
            CSK_Store_ProductAlerts = new Query<CSK_Store_ProductAlert>(provider);
            CSK_Store_ProductDescriptors = new Query<CSK_Store_ProductDescriptor>(provider);
            CSK_Store_ProductDescriptorTitles = new Query<CSK_Store_ProductDescriptorTitle>(provider);
            CSK_Store_ProductIsMergeds = new Query<CSK_Store_ProductIsMerged>(provider);
            CSK_Store_ProductLists = new Query<CSK_Store_ProductList>(provider);
            CSK_Store_ProductNews = new Query<CSK_Store_ProductNew>(provider);
            CSK_Store_ProductRatings = new Query<CSK_Store_ProductRating>(provider);
            CSK_Store_ProductRetailerCountHistories = new Query<CSK_Store_ProductRetailerCountHistory>(provider);
            CSK_Store_ProductReviews = new Query<CSK_Store_ProductReview>(provider);
            CSK_Store_ProductVideoReviews = new Query<CSK_Store_ProductVideoReview>(provider);
            CSK_Store_ProductVideoSources = new Query<CSK_Store_ProductVideoSource>(provider);
            CSK_Store_ProductViewTrackings = new Query<CSK_Store_ProductViewTracking>(provider);
            CSK_Store_ProductVotesSums = new Query<CSK_Store_ProductVotesSum>(provider);
            csk_store_redirectforretailerproducts = new Query<csk_store_redirectforretailerproduct>(provider);
            csk_store_RelatedParts = new Query<csk_store_RelatedPart>(provider);
            CSK_Store_Retailers = new Query<CSK_Store_Retailer>(provider);
            CSK_Store_RetailerImages = new Query<CSK_Store_RetailerImage>(provider);
            CSK_Store_RetailerLeadSignUps = new Query<CSK_Store_RetailerLeadSignUp>(provider);
            CSK_Store_RetailerLeadTrackings = new Query<CSK_Store_RetailerLeadTracking>(provider);
            CSK_Store_RetailerOffers = new Query<CSK_Store_RetailerOffer>(provider);
            CSK_Store_RetailerPaymentOptions = new Query<CSK_Store_RetailerPaymentOption>(provider);
            CSK_Store_RetailerProducts = new Query<CSK_Store_RetailerProduct>(provider);
            CSK_Store_RetailerProductConditions = new Query<CSK_Store_RetailerProductCondition>(provider);
            CSK_Store_RetailerProductNews = new Query<CSK_Store_RetailerProductNew>(provider);
            CSK_Store_RetailerRatings = new Query<CSK_Store_RetailerRating>(provider);
            CSK_Store_RetailerRedirectClicks = new Query<CSK_Store_RetailerRedirectClick>(provider);
            CSK_Store_RetailerReviews = new Query<CSK_Store_RetailerReview>(provider);
            CSK_Store_RetailerStoreTypes = new Query<CSK_Store_RetailerStoreType>(provider);
            CSK_Store_RetailerVotesSums = new Query<CSK_Store_RetailerVotesSum>(provider);
            CSK_Store_ReviewSourceAUs = new Query<CSK_Store_ReviewSourceAU>(provider);
            CSK_Store_ShareWishLists = new Query<CSK_Store_ShareWishList>(provider);
            CSK_Store_ShoppingTrackers = new Query<CSK_Store_ShoppingTracker>(provider);
            CSK_Store_Welcontents = new Query<CSK_Store_Welcontent>(provider);
            CSK_Util_Countries = new Query<CSK_Util_Country>(provider);
            EMailInfos = new Query<EMailInfo>(provider);
            FrequencyTypes = new Query<FrequencyType>(provider);
            Inactive_Retailer_Reasons = new Query<Inactive_Retailer_Reason>(provider);
            InvalidKeywords = new Query<InvalidKeyword>(provider);
            Local_CategoryNames = new Query<Local_CategoryName>(provider);
            MoneyPlans = new Query<MoneyPlan>(provider);
            PageFavourites = new Query<PageFavourite>(provider);
            Partner_Topic_Maps = new Query<Partner_Topic_Map>(provider);
            ProductFavourites = new Query<ProductFavourite>(provider);
            PurgedProducts = new Query<PurgedProduct>(provider);
            ResourcesInfos = new Query<ResourcesInfo>(provider);
            RetailerReviewDetails = new Query<RetailerReviewDetail>(provider);
            ShortCutCategory_Maps = new Query<ShortCutCategory_Map>(provider);
            Skykiwi_CategoryName_Translates = new Query<Skykiwi_CategoryName_Translate>(provider);
            Store_Compare_Attribute_Maps = new Query<Store_Compare_Attribute_Map>(provider);
            Store_Compare_Attributes = new Query<Store_Compare_Attribute>(provider);
            Store_GLatLngs = new Query<Store_GLatLng>(provider);
            Store_GRegions = new Query<Store_GRegion>(provider);
            WeeklyDealsUsers = new Query<WeeklyDealsUser>(provider);
            #endregion


            #region ' Schemas '
        	if(DataProvider.Schema.Tables.Count == 0)
			{
            	DataProvider.Schema.Tables.Add(new csk_store_productlibraryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerproductalertTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_freightlocation_mapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_automaticspecialcharacterTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ypricemespaceidsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailercityTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerstatusTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_mostpopularproductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_rangeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new widgetaffiliateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerfreightTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerkeywordTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new mylogTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_featuredproductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_similarproductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_useragentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_newproductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerfreightlocationTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_renameproduct_typeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_newsletteruserTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_attributetypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_crawlerpriorityTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_renameproduct_settingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_popularbrandsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_omgretailers_affiliateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ppccreditcardTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productreviewcommentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailer_change_rateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ppcpaymentoptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ppcaccountTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_exceptioncollectTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new tmpexceptcatidstabTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new tmpcatstabTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ppcbudgethistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new tmphihTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ppccategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new tmptbTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new retailernewsletterinfoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_advancedmappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retaileraddresstypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ppcinvoiceratioTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retaileroperatinghoursTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retaileraddressTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_util_currencyTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new logTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_usersearchTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_newonlyimportTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new pcmTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new calc_retailercategory_clicksTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new unittableTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_forum_topicTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_dailydealsretailerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailercategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_forum_qpostTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ppcmemberdailybudgetTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerreviewcommentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_bannertrackerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_transactiontypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_forum_reportedabsusTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_messaging_mailerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_advertising_bannerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_email_friendTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_listingimageTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_freighttypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_varietyTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_retailericontact_mapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productalerthistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerstTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new fisher_and_paykel_mapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new relatedcategoriesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new popularsearchTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_dailydealscategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_zeroproduct_change_rateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new trademecategorymapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_mostpopularcategoriesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_categoryfeaturedproductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new biggestdroppriceTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_categorymergingkeywordTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_dailydealsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_admininformationTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_featurecarouselTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_bannertypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productreviewfeedbackTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productstatusTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_stats_trackerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_reviewusefulTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_expertreviewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_reviewusefultypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_producttypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_stats_behaviorTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_reviewsourceTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ppcmemmberhistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailercrawlerinfoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_attributematchTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_userlocationTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_treepodiavideoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerproductlibraryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new store_crawlclasstypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_eecadataTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_goodsearchkeywordsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_relatedcategory_mapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_usersearch_googleTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_searchkeywordsruleTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_categorytotalTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new categoryfootermapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_mobileretailertrackerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_categoryviewtypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_freightlocationTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_contentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_attributegroupTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_attributevalueTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_attributevaluerangeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_buyingguideTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_buyingguiderelatedTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_buyingguidetypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_categoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_category_attributetitle_mapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_categorybugingguide_mapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_communitywelcontentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_consumerpricememappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ecommerceplatformTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_emaildatumTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_emailsendoutTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_energyTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_expertreviewauTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_featuredtabTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_governmentbadgeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_imageTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_jobfunctionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_listTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_listtypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_locationcategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_manufacturerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_newsletterTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_newsletterctxTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_newsreleaseTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_paymentoptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ppcmemberTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_ppcmembertypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_previousnewsletterTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_pricehistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productalertTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productdescriptorTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productdescriptortitleTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productismergedTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productlistTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productnewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productratingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productretailercounthistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productreviewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productvideoreviewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productvideosourceTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productviewtrackingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_productvotessumTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_redirectforretailerproductTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_relatedpartsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerimageTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerleadsignupTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerleadtrackingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerofferTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerpaymentoptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerproductTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerproductconditionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerproductnewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerratingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerredirectclickTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerreviewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailerstoretypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_retailervotessumTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_reviewsourceauTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_sharewishlistTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_shoppingtrackerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_store_welcontentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new csk_util_countryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new emailinfoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new frequencytypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new inactive_retailer_reasonTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new invalidkeywordsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new local_categorynameTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new moneyplanTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new pagefavouritesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new partner_topic_mapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new productfavouritesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new purgedproductTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new resourcesinfoTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new retailerreviewdetailTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new shortcutcategory_mapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new skykiwi_categoryname_translateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new store_compare_attribute_mapTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new store_compare_attributesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new store_glatlngTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new store_gregionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new weeklydealsuserTable(DataProvider));
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