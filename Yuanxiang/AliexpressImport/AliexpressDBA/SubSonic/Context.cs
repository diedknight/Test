


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

namespace AliexpressDBA
{
    public partial class AliexpressDBDB : IQuerySurface
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

        public AliexpressDBDB() 
        {
            if (DefaultDataProvider == null) {
                DataProvider = ProviderFactory.GetProvider("CommerceTemplate");
            }
            else {
                DataProvider = DefaultDataProvider;
            }
            Init();
        }

        public AliexpressDBDB(string connectionStringName)
        {
            DataProvider = ProviderFactory.GetProvider(connectionStringName);
            Init();
        }

		public AliexpressDBDB(string connectionString, string providerName)
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
			
        public Query<ProductAttribute> ProductAttributes { get; set; }
        public Query<ActivityLog> ActivityLogs { get; set; }
        public Query<SS_PR_ProductRibbon> SS_PR_ProductRibbons { get; set; }
        public Query<StockQuantityHistory> StockQuantityHistories { get; set; }
        public Query<Forums_Forum> Forums_Forums { get; set; }
        public Query<ProductAttributeCombination> ProductAttributeCombinations { get; set; }
        public Query<ActivityLogType> ActivityLogTypes { get; set; }
        public Query<SS_PR_RibbonPicture> SS_PR_RibbonPictures { get; set; }
        public Query<Store> Stores { get; set; }
        public Query<Forums_Group> Forums_Groups { get; set; }
        public Query<Product_ProductAttribute_Mapping> Product_ProductAttribute_Mappings { get; set; }
        public Query<Address> Addresses { get; set; }
        public Query<StoreMapping> StoreMappings { get; set; }
        public Query<Forums_Post> Forums_Posts { get; set; }
        public Query<ProductAttributeValue> ProductAttributeValues { get; set; }
        public Query<AddressAttribute> AddressAttributes { get; set; }
        public Query<SS_QT_Tab> SS_QT_Tabs { get; set; }
        public Query<TaxCategory> TaxCategories { get; set; }
        public Query<Forums_PostVote> Forums_PostVotes { get; set; }
        public Query<ProductAvailabilityRange> ProductAvailabilityRanges { get; set; }
        public Query<AddressAttributeValue> AddressAttributeValues { get; set; }
        public Query<SS_RB_Category> SS_RB_Categories { get; set; }
        public Query<TierPrice> TierPrices { get; set; }
        public Query<Forums_Subscription> Forums_Subscriptions { get; set; }
        public Query<Product_Category_Mapping> Product_Category_Mappings { get; set; }
        public Query<Affiliate> Affiliates { get; set; }
        public Query<SS_RB_Post> SS_RB_Posts { get; set; }
        public Query<Topic> Topics { get; set; }
        public Query<Forums_Topic> Forums_Topics { get; set; }
        public Query<Product_Manufacturer_Mapping> Product_Manufacturer_Mappings { get; set; }
        public Query<BackInStockSubscription> BackInStockSubscriptions { get; set; }
        public Query<SS_RB_RichBlogPostCategoryMapping> SS_RB_RichBlogPostCategoryMappings { get; set; }
        public Query<TopicTemplate> TopicTemplates { get; set; }
        public Query<GenericAttribute> GenericAttributes { get; set; }
        public Query<Product_Picture_Mapping> Product_Picture_Mappings { get; set; }
        public Query<BlogComment> BlogComments { get; set; }
        public Query<StorePickupPoint> StorePickupPoints { get; set; }
        public Query<SS_RB_RelatedBlog> SS_RB_RelatedBlogs { get; set; }
        public Query<UrlRecord> UrlRecords { get; set; }
        public Query<GiftCard> GiftCards { get; set; }
        public Query<Product_ProductTag_Mapping> Product_ProductTag_Mappings { get; set; }
        public Query<BlogPost> BlogPosts { get; set; }
        public Query<GoogleProduct> GoogleProducts { get; set; }
        public Query<SS_SPC_ProductsGroup> SS_SPC_ProductsGroups { get; set; }
        public Query<Vendor> Vendors { get; set; }
        public Query<GiftCardUsageHistory> GiftCardUsageHistories { get; set; }
        public Query<ProductReview> ProductReviews { get; set; }
        public Query<Campaign> Campaigns { get; set; }
        public Query<ShippingByWeight> ShippingByWeights { get; set; }
        public Query<SS_SPC_ProductsGroupItem> SS_SPC_ProductsGroupItems { get; set; }
        public Query<VendorNote> VendorNotes { get; set; }
        public Query<Language> Languages { get; set; }
        public Query<ProductReviewHelpfulness> ProductReviewHelpfulnesses { get; set; }
        public Query<Category> Categories { get; set; }
        public Query<TaxRate> TaxRates { get; set; }
        public Query<Warehouse> Warehouses { get; set; }
        public Query<LocaleStringResource> LocaleStringResources { get; set; }
        public Query<Product_SpecificationAttribute_Mapping> Product_SpecificationAttribute_Mappings { get; set; }
        public Query<CategoryTemplate> CategoryTemplates { get; set; }
        public Query<LocalizedProperty> LocalizedProperties { get; set; }
        public Query<ProductTag> ProductTags { get; set; }
        public Query<CheckoutAttribute> CheckoutAttributes { get; set; }
        public Query<Log> Logs { get; set; }
        public Query<ProductTemplate> ProductTemplates { get; set; }
        public Query<CheckoutAttributeValue> CheckoutAttributeValues { get; set; }
        public Query<Manufacturer> Manufacturers { get; set; }
        public Query<ProductWarehouseInventory> ProductWarehouseInventories { get; set; }
        public Query<Country> Countries { get; set; }
        public Query<ManufacturerTemplate> ManufacturerTemplates { get; set; }
        public Query<QueuedEmail> QueuedEmails { get; set; }
        public Query<CrossSellProduct> CrossSellProducts { get; set; }
        public Query<SS_AS_AnywhereSlider> SS_AS_AnywhereSliders { get; set; }
        public Query<MeasureDimension> MeasureDimensions { get; set; }
        public Query<RecurringPayment> RecurringPayments { get; set; }
        public Query<Currency> Currencies { get; set; }
        public Query<SS_AS_SliderImage> SS_AS_SliderImages { get; set; }
        public Query<MeasureWeight> MeasureWeights { get; set; }
        public Query<RecurringPaymentHistory> RecurringPaymentHistories { get; set; }
        public Query<Customer> Customers { get; set; }
        public Query<SS_C_Condition> SS_C_Conditions { get; set; }
        public Query<MessageTemplate> MessageTemplates { get; set; }
        public Query<RelatedProduct> RelatedProducts { get; set; }
        public Query<CustomerAddress> CustomerAddresses { get; set; }
        public Query<SS_C_ConditionGroup> SS_C_ConditionGroups { get; set; }
        public Query<NewsComment> NewsComments { get; set; }
        public Query<ReturnRequest> ReturnRequests { get; set; }
        public Query<CustomerAttribute> CustomerAttributes { get; set; }
        public Query<SS_C_ConditionStatement> SS_C_ConditionStatements { get; set; }
        public Query<News> News { get; set; }
        public Query<ReturnRequestAction> ReturnRequestActions { get; set; }
        public Query<CustomerAttributeValue> CustomerAttributeValues { get; set; }
        public Query<SS_C_CustomerOverride> SS_C_CustomerOverrides { get; set; }
        public Query<NewsLetterSubscription> NewsLetterSubscriptions { get; set; }
        public Query<ReturnRequestReason> ReturnRequestReasons { get; set; }
        public Query<Customer_CustomerRole_Mapping> Customer_CustomerRole_Mappings { get; set; }
        public Query<SS_C_EntityCondition> SS_C_EntityConditions { get; set; }
        public Query<Order> Orders { get; set; }
        public Query<RewardPointsHistory> RewardPointsHistories { get; set; }
        public Query<CustomerPassword> CustomerPasswords { get; set; }
        public Query<SS_C_ProductOverride> SS_C_ProductOverrides { get; set; }
        public Query<OrderItem> OrderItems { get; set; }
        public Query<ScheduleTask> ScheduleTasks { get; set; }
        public Query<CustomerRole> CustomerRoles { get; set; }
        public Query<OrderNote> OrderNotes { get; set; }
        public Query<SearchTerm> SearchTerms { get; set; }
        public Query<DeliveryDate> DeliveryDates { get; set; }
        public Query<PermissionRecord> PermissionRecords { get; set; }
        public Query<SS_MAP_EntityMapping> SS_MAP_EntityMappings { get; set; }
        public Query<Setting> Settings { get; set; }
        public Query<Discount> Discounts { get; set; }
        public Query<PermissionRecord_Role_Mapping> PermissionRecord_Role_Mappings { get; set; }
        public Query<SS_MAP_EntityWidgetMapping> SS_MAP_EntityWidgetMappings { get; set; }
        public Query<Shipment> Shipments { get; set; }
        public Query<Discount_AppliedToCategory> Discount_AppliedToCategories { get; set; }
        public Query<Picture> Pictures { get; set; }
        public Query<SS_S_Schedule> SS_S_Schedules { get; set; }
        public Query<ShipmentItem> ShipmentItems { get; set; }
        public Query<Discount_AppliedToManufacturer> Discount_AppliedToManufacturers { get; set; }
        public Query<Poll> Polls { get; set; }
        public Query<SS_ES_EntitySetting> SS_ES_EntitySettings { get; set; }
        public Query<ShippingMethod> ShippingMethods { get; set; }
        public Query<Discount_AppliedToProduct> Discount_AppliedToProducts { get; set; }
        public Query<PollAnswer> PollAnswers { get; set; }
        public Query<SS_JC_JCarousel> SS_JC_JCarousels { get; set; }
        public Query<ShippingMethodRestriction> ShippingMethodRestrictions { get; set; }
        public Query<DiscountRequirement> DiscountRequirements { get; set; }
        public Query<PollVotingRecord> PollVotingRecords { get; set; }
        public Query<SS_MM_Menu> SS_MM_Menus { get; set; }
        public Query<ShoppingCartItem> ShoppingCartItems { get; set; }
        public Query<DiscountUsageHistory> DiscountUsageHistories { get; set; }
        public Query<PredefinedProductAttributeValue> PredefinedProductAttributeValues { get; set; }
        public Query<SS_MM_MenuItem> SS_MM_MenuItems { get; set; }
        public Query<SpecificationAttribute> SpecificationAttributes { get; set; }
        public Query<Download> Downloads { get; set; }
        public Query<Forums_PrivateMessage> Forums_PrivateMessages { get; set; }
        public Query<SS_PR_CategoryPageRibbon> SS_PR_CategoryPageRibbons { get; set; }
        public Query<SpecificationAttributeOption> SpecificationAttributeOptions { get; set; }
        public Query<EmailAccount> EmailAccounts { get; set; }
        public Query<Product> Products { get; set; }
        public Query<AclRecord> AclRecords { get; set; }
        public Query<SS_PR_ProductPageRibbon> SS_PR_ProductPageRibbons { get; set; }
        public Query<StateProvince> StateProvinces { get; set; }
        public Query<ExternalAuthenticationRecord> ExternalAuthenticationRecords { get; set; }

			

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
            ProductAttributes = new Query<ProductAttribute>(provider);
            ActivityLogs = new Query<ActivityLog>(provider);
            SS_PR_ProductRibbons = new Query<SS_PR_ProductRibbon>(provider);
            StockQuantityHistories = new Query<StockQuantityHistory>(provider);
            Forums_Forums = new Query<Forums_Forum>(provider);
            ProductAttributeCombinations = new Query<ProductAttributeCombination>(provider);
            ActivityLogTypes = new Query<ActivityLogType>(provider);
            SS_PR_RibbonPictures = new Query<SS_PR_RibbonPicture>(provider);
            Stores = new Query<Store>(provider);
            Forums_Groups = new Query<Forums_Group>(provider);
            Product_ProductAttribute_Mappings = new Query<Product_ProductAttribute_Mapping>(provider);
            Addresses = new Query<Address>(provider);
            StoreMappings = new Query<StoreMapping>(provider);
            Forums_Posts = new Query<Forums_Post>(provider);
            ProductAttributeValues = new Query<ProductAttributeValue>(provider);
            AddressAttributes = new Query<AddressAttribute>(provider);
            SS_QT_Tabs = new Query<SS_QT_Tab>(provider);
            TaxCategories = new Query<TaxCategory>(provider);
            Forums_PostVotes = new Query<Forums_PostVote>(provider);
            ProductAvailabilityRanges = new Query<ProductAvailabilityRange>(provider);
            AddressAttributeValues = new Query<AddressAttributeValue>(provider);
            SS_RB_Categories = new Query<SS_RB_Category>(provider);
            TierPrices = new Query<TierPrice>(provider);
            Forums_Subscriptions = new Query<Forums_Subscription>(provider);
            Product_Category_Mappings = new Query<Product_Category_Mapping>(provider);
            Affiliates = new Query<Affiliate>(provider);
            SS_RB_Posts = new Query<SS_RB_Post>(provider);
            Topics = new Query<Topic>(provider);
            Forums_Topics = new Query<Forums_Topic>(provider);
            Product_Manufacturer_Mappings = new Query<Product_Manufacturer_Mapping>(provider);
            BackInStockSubscriptions = new Query<BackInStockSubscription>(provider);
            SS_RB_RichBlogPostCategoryMappings = new Query<SS_RB_RichBlogPostCategoryMapping>(provider);
            TopicTemplates = new Query<TopicTemplate>(provider);
            GenericAttributes = new Query<GenericAttribute>(provider);
            Product_Picture_Mappings = new Query<Product_Picture_Mapping>(provider);
            BlogComments = new Query<BlogComment>(provider);
            StorePickupPoints = new Query<StorePickupPoint>(provider);
            SS_RB_RelatedBlogs = new Query<SS_RB_RelatedBlog>(provider);
            UrlRecords = new Query<UrlRecord>(provider);
            GiftCards = new Query<GiftCard>(provider);
            Product_ProductTag_Mappings = new Query<Product_ProductTag_Mapping>(provider);
            BlogPosts = new Query<BlogPost>(provider);
            GoogleProducts = new Query<GoogleProduct>(provider);
            SS_SPC_ProductsGroups = new Query<SS_SPC_ProductsGroup>(provider);
            Vendors = new Query<Vendor>(provider);
            GiftCardUsageHistories = new Query<GiftCardUsageHistory>(provider);
            ProductReviews = new Query<ProductReview>(provider);
            Campaigns = new Query<Campaign>(provider);
            ShippingByWeights = new Query<ShippingByWeight>(provider);
            SS_SPC_ProductsGroupItems = new Query<SS_SPC_ProductsGroupItem>(provider);
            VendorNotes = new Query<VendorNote>(provider);
            Languages = new Query<Language>(provider);
            ProductReviewHelpfulnesses = new Query<ProductReviewHelpfulness>(provider);
            Categories = new Query<Category>(provider);
            TaxRates = new Query<TaxRate>(provider);
            Warehouses = new Query<Warehouse>(provider);
            LocaleStringResources = new Query<LocaleStringResource>(provider);
            Product_SpecificationAttribute_Mappings = new Query<Product_SpecificationAttribute_Mapping>(provider);
            CategoryTemplates = new Query<CategoryTemplate>(provider);
            LocalizedProperties = new Query<LocalizedProperty>(provider);
            ProductTags = new Query<ProductTag>(provider);
            CheckoutAttributes = new Query<CheckoutAttribute>(provider);
            Logs = new Query<Log>(provider);
            ProductTemplates = new Query<ProductTemplate>(provider);
            CheckoutAttributeValues = new Query<CheckoutAttributeValue>(provider);
            Manufacturers = new Query<Manufacturer>(provider);
            ProductWarehouseInventories = new Query<ProductWarehouseInventory>(provider);
            Countries = new Query<Country>(provider);
            ManufacturerTemplates = new Query<ManufacturerTemplate>(provider);
            QueuedEmails = new Query<QueuedEmail>(provider);
            CrossSellProducts = new Query<CrossSellProduct>(provider);
            SS_AS_AnywhereSliders = new Query<SS_AS_AnywhereSlider>(provider);
            MeasureDimensions = new Query<MeasureDimension>(provider);
            RecurringPayments = new Query<RecurringPayment>(provider);
            Currencies = new Query<Currency>(provider);
            SS_AS_SliderImages = new Query<SS_AS_SliderImage>(provider);
            MeasureWeights = new Query<MeasureWeight>(provider);
            RecurringPaymentHistories = new Query<RecurringPaymentHistory>(provider);
            Customers = new Query<Customer>(provider);
            SS_C_Conditions = new Query<SS_C_Condition>(provider);
            MessageTemplates = new Query<MessageTemplate>(provider);
            RelatedProducts = new Query<RelatedProduct>(provider);
            CustomerAddresses = new Query<CustomerAddress>(provider);
            SS_C_ConditionGroups = new Query<SS_C_ConditionGroup>(provider);
            NewsComments = new Query<NewsComment>(provider);
            ReturnRequests = new Query<ReturnRequest>(provider);
            CustomerAttributes = new Query<CustomerAttribute>(provider);
            SS_C_ConditionStatements = new Query<SS_C_ConditionStatement>(provider);
            News = new Query<News>(provider);
            ReturnRequestActions = new Query<ReturnRequestAction>(provider);
            CustomerAttributeValues = new Query<CustomerAttributeValue>(provider);
            SS_C_CustomerOverrides = new Query<SS_C_CustomerOverride>(provider);
            NewsLetterSubscriptions = new Query<NewsLetterSubscription>(provider);
            ReturnRequestReasons = new Query<ReturnRequestReason>(provider);
            Customer_CustomerRole_Mappings = new Query<Customer_CustomerRole_Mapping>(provider);
            SS_C_EntityConditions = new Query<SS_C_EntityCondition>(provider);
            Orders = new Query<Order>(provider);
            RewardPointsHistories = new Query<RewardPointsHistory>(provider);
            CustomerPasswords = new Query<CustomerPassword>(provider);
            SS_C_ProductOverrides = new Query<SS_C_ProductOverride>(provider);
            OrderItems = new Query<OrderItem>(provider);
            ScheduleTasks = new Query<ScheduleTask>(provider);
            CustomerRoles = new Query<CustomerRole>(provider);
            OrderNotes = new Query<OrderNote>(provider);
            SearchTerms = new Query<SearchTerm>(provider);
            DeliveryDates = new Query<DeliveryDate>(provider);
            PermissionRecords = new Query<PermissionRecord>(provider);
            SS_MAP_EntityMappings = new Query<SS_MAP_EntityMapping>(provider);
            Settings = new Query<Setting>(provider);
            Discounts = new Query<Discount>(provider);
            PermissionRecord_Role_Mappings = new Query<PermissionRecord_Role_Mapping>(provider);
            SS_MAP_EntityWidgetMappings = new Query<SS_MAP_EntityWidgetMapping>(provider);
            Shipments = new Query<Shipment>(provider);
            Discount_AppliedToCategories = new Query<Discount_AppliedToCategory>(provider);
            Pictures = new Query<Picture>(provider);
            SS_S_Schedules = new Query<SS_S_Schedule>(provider);
            ShipmentItems = new Query<ShipmentItem>(provider);
            Discount_AppliedToManufacturers = new Query<Discount_AppliedToManufacturer>(provider);
            Polls = new Query<Poll>(provider);
            SS_ES_EntitySettings = new Query<SS_ES_EntitySetting>(provider);
            ShippingMethods = new Query<ShippingMethod>(provider);
            Discount_AppliedToProducts = new Query<Discount_AppliedToProduct>(provider);
            PollAnswers = new Query<PollAnswer>(provider);
            SS_JC_JCarousels = new Query<SS_JC_JCarousel>(provider);
            ShippingMethodRestrictions = new Query<ShippingMethodRestriction>(provider);
            DiscountRequirements = new Query<DiscountRequirement>(provider);
            PollVotingRecords = new Query<PollVotingRecord>(provider);
            SS_MM_Menus = new Query<SS_MM_Menu>(provider);
            ShoppingCartItems = new Query<ShoppingCartItem>(provider);
            DiscountUsageHistories = new Query<DiscountUsageHistory>(provider);
            PredefinedProductAttributeValues = new Query<PredefinedProductAttributeValue>(provider);
            SS_MM_MenuItems = new Query<SS_MM_MenuItem>(provider);
            SpecificationAttributes = new Query<SpecificationAttribute>(provider);
            Downloads = new Query<Download>(provider);
            Forums_PrivateMessages = new Query<Forums_PrivateMessage>(provider);
            SS_PR_CategoryPageRibbons = new Query<SS_PR_CategoryPageRibbon>(provider);
            SpecificationAttributeOptions = new Query<SpecificationAttributeOption>(provider);
            EmailAccounts = new Query<EmailAccount>(provider);
            Products = new Query<Product>(provider);
            AclRecords = new Query<AclRecord>(provider);
            SS_PR_ProductPageRibbons = new Query<SS_PR_ProductPageRibbon>(provider);
            StateProvinces = new Query<StateProvince>(provider);
            ExternalAuthenticationRecords = new Query<ExternalAuthenticationRecord>(provider);
            #endregion


            #region ' Schemas '
        	if(DataProvider.Schema.Tables.Count == 0)
			{
            	DataProvider.Schema.Tables.Add(new ProductAttributeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ActivityLogTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_PR_ProductRibbonTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new StockQuantityHistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Forums_ForumTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ProductAttributeCombinationTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ActivityLogTypeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_PR_RibbonPictureTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new StoreTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Forums_GroupTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Product_ProductAttribute_MappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new AddressTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new StoreMappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Forums_PostTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ProductAttributeValueTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new AddressAttributeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_QT_TabTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new TaxCategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Forums_PostVoteTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ProductAvailabilityRangeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new AddressAttributeValueTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_RB_CategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new TierPriceTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Forums_SubscriptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Product_Category_MappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new AffiliateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_RB_PostTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new TopicTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Forums_TopicTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Product_Manufacturer_MappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new BackInStockSubscriptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_RB_RichBlogPostCategoryMappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new TopicTemplateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new GenericAttributeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Product_Picture_MappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new BlogCommentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new StorePickupPointTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_RB_RelatedBlogTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new UrlRecordTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new GiftCardTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Product_ProductTag_MappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new BlogPostTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new GoogleProductTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_SPC_ProductsGroupTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new VendorTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new GiftCardUsageHistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ProductReviewTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CampaignTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ShippingByWeightTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_SPC_ProductsGroupItemTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new VendorNoteTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new LanguageTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ProductReviewHelpfulnessTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CategoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new TaxRateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new WarehouseTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new LocaleStringResourceTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Product_SpecificationAttribute_MappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CategoryTemplateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new LocalizedPropertyTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ProductTagTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CheckoutAttributeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new LogTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ProductTemplateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CheckoutAttributeValueTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ManufacturerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ProductWarehouseInventoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CountryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ManufacturerTemplateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new QueuedEmailTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CrossSellProductTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_AS_AnywhereSliderTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new MeasureDimensionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new RecurringPaymentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CurrencyTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_AS_SliderImageTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new MeasureWeightTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new RecurringPaymentHistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CustomerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_C_ConditionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new MessageTemplateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new RelatedProductTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CustomerAddressesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_C_ConditionGroupTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new NewsCommentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ReturnRequestTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CustomerAttributeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_C_ConditionStatementTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new NewsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ReturnRequestActionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CustomerAttributeValueTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_C_CustomerOverrideTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new NewsLetterSubscriptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ReturnRequestReasonTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Customer_CustomerRole_MappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_C_EntityConditionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new OrderTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new RewardPointsHistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CustomerPasswordTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_C_ProductOverrideTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new OrderItemTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ScheduleTaskTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new CustomerRoleTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new OrderNoteTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SearchTermTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new DeliveryDateTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PermissionRecordTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_MAP_EntityMappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SettingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new DiscountTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PermissionRecord_Role_MappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_MAP_EntityWidgetMappingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ShipmentTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Discount_AppliedToCategoriesTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PictureTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_S_ScheduleTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ShipmentItemTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Discount_AppliedToManufacturersTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PollTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_ES_EntitySettingTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ShippingMethodTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Discount_AppliedToProductsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PollAnswerTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_JC_JCarouselTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ShippingMethodRestrictionsTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new DiscountRequirementTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PollVotingRecordTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_MM_MenuTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ShoppingCartItemTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new DiscountUsageHistoryTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new PredefinedProductAttributeValueTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_MM_MenuItemTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SpecificationAttributeTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new DownloadTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new Forums_PrivateMessageTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_PR_CategoryPageRibbonTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SpecificationAttributeOptionTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new EmailAccountTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ProductTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new AclRecordTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new SS_PR_ProductPageRibbonTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new StateProvinceTable(DataProvider));
            	DataProvider.Schema.Tables.Add(new ExternalAuthenticationRecordTable(DataProvider));
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