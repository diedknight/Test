


using System;
using SubSonic.Schema;
using System.Collections.Generic;
using SubSonic.DataProviders;
using System.Data;

namespace AliexpressDBA {
	
        /// <summary>
        /// Table: ProductAttribute
        /// Primary Key: Id
        /// </summary>

        public class ProductAttributeTable: DatabaseTable {
            
            public ProductAttributeTable(IDataProvider provider):base("ProductAttribute",provider){
                ClassName = "ProductAttribute";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Description", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Description{
                get{
                    return this.GetColumn("Description");
                }
            }
				
   			public static string DescriptionColumn{
			      get{
        			return "Description";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ActivityLog
        /// Primary Key: Id
        /// </summary>

        public class ActivityLogTable: DatabaseTable {
            
            public ActivityLogTable(IDataProvider provider):base("ActivityLog",provider){
                ClassName = "ActivityLog";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ActivityLogTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Comment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IpAddress", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ActivityLogTypeId{
                get{
                    return this.GetColumn("ActivityLogTypeId");
                }
            }
				
   			public static string ActivityLogTypeIdColumn{
			      get{
        			return "ActivityLogTypeId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn Comment{
                get{
                    return this.GetColumn("Comment");
                }
            }
				
   			public static string CommentColumn{
			      get{
        			return "Comment";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn IpAddress{
                get{
                    return this.GetColumn("IpAddress");
                }
            }
				
   			public static string IpAddressColumn{
			      get{
        			return "IpAddress";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_PR_ProductRibbon
        /// Primary Key: Id
        /// </summary>

        public class SS_PR_ProductRibbonTable: DatabaseTable {
            
            public SS_PR_ProductRibbonTable(IDataProvider provider):base("SS_PR_ProductRibbon",provider){
                ClassName = "SS_PR_ProductRibbon";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Enabled", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("StopAddingRibbonsAftherThisOneIsAdded", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Priority", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("FromDate", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ToDate", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Enabled{
                get{
                    return this.GetColumn("Enabled");
                }
            }
				
   			public static string EnabledColumn{
			      get{
        			return "Enabled";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn StopAddingRibbonsAftherThisOneIsAdded{
                get{
                    return this.GetColumn("StopAddingRibbonsAftherThisOneIsAdded");
                }
            }
				
   			public static string StopAddingRibbonsAftherThisOneIsAddedColumn{
			      get{
        			return "StopAddingRibbonsAftherThisOneIsAdded";
      			}
		    }
            
            public IColumn Priority{
                get{
                    return this.GetColumn("Priority");
                }
            }
				
   			public static string PriorityColumn{
			      get{
        			return "Priority";
      			}
		    }
            
            public IColumn FromDate{
                get{
                    return this.GetColumn("FromDate");
                }
            }
				
   			public static string FromDateColumn{
			      get{
        			return "FromDate";
      			}
		    }
            
            public IColumn ToDate{
                get{
                    return this.GetColumn("ToDate");
                }
            }
				
   			public static string ToDateColumn{
			      get{
        			return "ToDate";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: StockQuantityHistory
        /// Primary Key: Id
        /// </summary>

        public class StockQuantityHistoryTable: DatabaseTable {
            
            public StockQuantityHistoryTable(IDataProvider provider):base("StockQuantityHistory",provider){
                ClassName = "StockQuantityHistory";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("QuantityAdjustment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StockQuantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Message", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CombinationId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WarehouseId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn QuantityAdjustment{
                get{
                    return this.GetColumn("QuantityAdjustment");
                }
            }
				
   			public static string QuantityAdjustmentColumn{
			      get{
        			return "QuantityAdjustment";
      			}
		    }
            
            public IColumn StockQuantity{
                get{
                    return this.GetColumn("StockQuantity");
                }
            }
				
   			public static string StockQuantityColumn{
			      get{
        			return "StockQuantity";
      			}
		    }
            
            public IColumn Message{
                get{
                    return this.GetColumn("Message");
                }
            }
				
   			public static string MessageColumn{
			      get{
        			return "Message";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn CombinationId{
                get{
                    return this.GetColumn("CombinationId");
                }
            }
				
   			public static string CombinationIdColumn{
			      get{
        			return "CombinationId";
      			}
		    }
            
            public IColumn WarehouseId{
                get{
                    return this.GetColumn("WarehouseId");
                }
            }
				
   			public static string WarehouseIdColumn{
			      get{
        			return "WarehouseId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Forums_Forum
        /// Primary Key: Id
        /// </summary>

        public class Forums_ForumTable: DatabaseTable {
            
            public Forums_ForumTable(IDataProvider provider):base("Forums_Forum",provider){
                ClassName = "Forums_Forum";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ForumGroupId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("Description", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("NumTopics", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("NumPosts", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastTopicId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastPostId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastPostCustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastPostTime", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UpdatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ForumGroupId{
                get{
                    return this.GetColumn("ForumGroupId");
                }
            }
				
   			public static string ForumGroupIdColumn{
			      get{
        			return "ForumGroupId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Description{
                get{
                    return this.GetColumn("Description");
                }
            }
				
   			public static string DescriptionColumn{
			      get{
        			return "Description";
      			}
		    }
            
            public IColumn NumTopics{
                get{
                    return this.GetColumn("NumTopics");
                }
            }
				
   			public static string NumTopicsColumn{
			      get{
        			return "NumTopics";
      			}
		    }
            
            public IColumn NumPosts{
                get{
                    return this.GetColumn("NumPosts");
                }
            }
				
   			public static string NumPostsColumn{
			      get{
        			return "NumPosts";
      			}
		    }
            
            public IColumn LastTopicId{
                get{
                    return this.GetColumn("LastTopicId");
                }
            }
				
   			public static string LastTopicIdColumn{
			      get{
        			return "LastTopicId";
      			}
		    }
            
            public IColumn LastPostId{
                get{
                    return this.GetColumn("LastPostId");
                }
            }
				
   			public static string LastPostIdColumn{
			      get{
        			return "LastPostId";
      			}
		    }
            
            public IColumn LastPostCustomerId{
                get{
                    return this.GetColumn("LastPostCustomerId");
                }
            }
				
   			public static string LastPostCustomerIdColumn{
			      get{
        			return "LastPostCustomerId";
      			}
		    }
            
            public IColumn LastPostTime{
                get{
                    return this.GetColumn("LastPostTime");
                }
            }
				
   			public static string LastPostTimeColumn{
			      get{
        			return "LastPostTime";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UpdatedOnUtc{
                get{
                    return this.GetColumn("UpdatedOnUtc");
                }
            }
				
   			public static string UpdatedOnUtcColumn{
			      get{
        			return "UpdatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ProductAttributeCombination
        /// Primary Key: Id
        /// </summary>

        public class ProductAttributeCombinationTable: DatabaseTable {
            
            public ProductAttributeCombinationTable(IDataProvider provider):base("ProductAttributeCombination",provider){
                ClassName = "ProductAttributeCombination";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AttributesXml", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("StockQuantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AllowOutOfStockOrders", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Sku", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ManufacturerPartNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Gtin", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("OverriddenPrice", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("NotifyAdminForQuantityBelow", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn AttributesXml{
                get{
                    return this.GetColumn("AttributesXml");
                }
            }
				
   			public static string AttributesXmlColumn{
			      get{
        			return "AttributesXml";
      			}
		    }
            
            public IColumn StockQuantity{
                get{
                    return this.GetColumn("StockQuantity");
                }
            }
				
   			public static string StockQuantityColumn{
			      get{
        			return "StockQuantity";
      			}
		    }
            
            public IColumn AllowOutOfStockOrders{
                get{
                    return this.GetColumn("AllowOutOfStockOrders");
                }
            }
				
   			public static string AllowOutOfStockOrdersColumn{
			      get{
        			return "AllowOutOfStockOrders";
      			}
		    }
            
            public IColumn Sku{
                get{
                    return this.GetColumn("Sku");
                }
            }
				
   			public static string SkuColumn{
			      get{
        			return "Sku";
      			}
		    }
            
            public IColumn ManufacturerPartNumber{
                get{
                    return this.GetColumn("ManufacturerPartNumber");
                }
            }
				
   			public static string ManufacturerPartNumberColumn{
			      get{
        			return "ManufacturerPartNumber";
      			}
		    }
            
            public IColumn Gtin{
                get{
                    return this.GetColumn("Gtin");
                }
            }
				
   			public static string GtinColumn{
			      get{
        			return "Gtin";
      			}
		    }
            
            public IColumn OverriddenPrice{
                get{
                    return this.GetColumn("OverriddenPrice");
                }
            }
				
   			public static string OverriddenPriceColumn{
			      get{
        			return "OverriddenPrice";
      			}
		    }
            
            public IColumn NotifyAdminForQuantityBelow{
                get{
                    return this.GetColumn("NotifyAdminForQuantityBelow");
                }
            }
				
   			public static string NotifyAdminForQuantityBelowColumn{
			      get{
        			return "NotifyAdminForQuantityBelow";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ActivityLogType
        /// Primary Key: Id
        /// </summary>

        public class ActivityLogTypeTable: DatabaseTable {
            
            public ActivityLogTypeTable(IDataProvider provider):base("ActivityLogType",provider){
                ClassName = "ActivityLogType";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SystemKeyword", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("Enabled", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn SystemKeyword{
                get{
                    return this.GetColumn("SystemKeyword");
                }
            }
				
   			public static string SystemKeywordColumn{
			      get{
        			return "SystemKeyword";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Enabled{
                get{
                    return this.GetColumn("Enabled");
                }
            }
				
   			public static string EnabledColumn{
			      get{
        			return "Enabled";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_PR_RibbonPicture
        /// Primary Key: Id
        /// </summary>

        public class SS_PR_RibbonPictureTable: DatabaseTable {
            
            public SS_PR_RibbonPictureTable(IDataProvider provider):base("SS_PR_RibbonPicture",provider){
                ClassName = "SS_PR_RibbonPicture";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn PictureId{
                get{
                    return this.GetColumn("PictureId");
                }
            }
				
   			public static string PictureIdColumn{
			      get{
        			return "PictureId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Store
        /// Primary Key: Id
        /// </summary>

        public class StoreTable: DatabaseTable {
            
            public StoreTable(IDataProvider provider):base("Store",provider){
                ClassName = "Store";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Url", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("SslEnabled", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SecureUrl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Hosts", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("DefaultLanguageId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CompanyName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("CompanyAddress", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("CompanyPhoneNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("CompanyVat", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Url{
                get{
                    return this.GetColumn("Url");
                }
            }
				
   			public static string UrlColumn{
			      get{
        			return "Url";
      			}
		    }
            
            public IColumn SslEnabled{
                get{
                    return this.GetColumn("SslEnabled");
                }
            }
				
   			public static string SslEnabledColumn{
			      get{
        			return "SslEnabled";
      			}
		    }
            
            public IColumn SecureUrl{
                get{
                    return this.GetColumn("SecureUrl");
                }
            }
				
   			public static string SecureUrlColumn{
			      get{
        			return "SecureUrl";
      			}
		    }
            
            public IColumn Hosts{
                get{
                    return this.GetColumn("Hosts");
                }
            }
				
   			public static string HostsColumn{
			      get{
        			return "Hosts";
      			}
		    }
            
            public IColumn DefaultLanguageId{
                get{
                    return this.GetColumn("DefaultLanguageId");
                }
            }
				
   			public static string DefaultLanguageIdColumn{
			      get{
        			return "DefaultLanguageId";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn CompanyName{
                get{
                    return this.GetColumn("CompanyName");
                }
            }
				
   			public static string CompanyNameColumn{
			      get{
        			return "CompanyName";
      			}
		    }
            
            public IColumn CompanyAddress{
                get{
                    return this.GetColumn("CompanyAddress");
                }
            }
				
   			public static string CompanyAddressColumn{
			      get{
        			return "CompanyAddress";
      			}
		    }
            
            public IColumn CompanyPhoneNumber{
                get{
                    return this.GetColumn("CompanyPhoneNumber");
                }
            }
				
   			public static string CompanyPhoneNumberColumn{
			      get{
        			return "CompanyPhoneNumber";
      			}
		    }
            
            public IColumn CompanyVat{
                get{
                    return this.GetColumn("CompanyVat");
                }
            }
				
   			public static string CompanyVatColumn{
			      get{
        			return "CompanyVat";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Forums_Group
        /// Primary Key: Id
        /// </summary>

        public class Forums_GroupTable: DatabaseTable {
            
            public Forums_GroupTable(IDataProvider provider):base("Forums_Group",provider){
                ClassName = "Forums_Group";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UpdatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UpdatedOnUtc{
                get{
                    return this.GetColumn("UpdatedOnUtc");
                }
            }
				
   			public static string UpdatedOnUtcColumn{
			      get{
        			return "UpdatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Product_ProductAttribute_Mapping
        /// Primary Key: Id
        /// </summary>

        public class Product_ProductAttribute_MappingTable: DatabaseTable {
            
            public Product_ProductAttribute_MappingTable(IDataProvider provider):base("Product_ProductAttribute_Mapping",provider){
                ClassName = "Product_ProductAttribute_Mapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductAttributeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TextPrompt", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IsRequired", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AttributeControlTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ValidationMinLength", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ValidationMaxLength", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ValidationFileAllowedExtensions", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ValidationFileMaximumSize", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DefaultValue", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ConditionAttributeXml", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn ProductAttributeId{
                get{
                    return this.GetColumn("ProductAttributeId");
                }
            }
				
   			public static string ProductAttributeIdColumn{
			      get{
        			return "ProductAttributeId";
      			}
		    }
            
            public IColumn TextPrompt{
                get{
                    return this.GetColumn("TextPrompt");
                }
            }
				
   			public static string TextPromptColumn{
			      get{
        			return "TextPrompt";
      			}
		    }
            
            public IColumn IsRequired{
                get{
                    return this.GetColumn("IsRequired");
                }
            }
				
   			public static string IsRequiredColumn{
			      get{
        			return "IsRequired";
      			}
		    }
            
            public IColumn AttributeControlTypeId{
                get{
                    return this.GetColumn("AttributeControlTypeId");
                }
            }
				
   			public static string AttributeControlTypeIdColumn{
			      get{
        			return "AttributeControlTypeId";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn ValidationMinLength{
                get{
                    return this.GetColumn("ValidationMinLength");
                }
            }
				
   			public static string ValidationMinLengthColumn{
			      get{
        			return "ValidationMinLength";
      			}
		    }
            
            public IColumn ValidationMaxLength{
                get{
                    return this.GetColumn("ValidationMaxLength");
                }
            }
				
   			public static string ValidationMaxLengthColumn{
			      get{
        			return "ValidationMaxLength";
      			}
		    }
            
            public IColumn ValidationFileAllowedExtensions{
                get{
                    return this.GetColumn("ValidationFileAllowedExtensions");
                }
            }
				
   			public static string ValidationFileAllowedExtensionsColumn{
			      get{
        			return "ValidationFileAllowedExtensions";
      			}
		    }
            
            public IColumn ValidationFileMaximumSize{
                get{
                    return this.GetColumn("ValidationFileMaximumSize");
                }
            }
				
   			public static string ValidationFileMaximumSizeColumn{
			      get{
        			return "ValidationFileMaximumSize";
      			}
		    }
            
            public IColumn DefaultValue{
                get{
                    return this.GetColumn("DefaultValue");
                }
            }
				
   			public static string DefaultValueColumn{
			      get{
        			return "DefaultValue";
      			}
		    }
            
            public IColumn ConditionAttributeXml{
                get{
                    return this.GetColumn("ConditionAttributeXml");
                }
            }
				
   			public static string ConditionAttributeXmlColumn{
			      get{
        			return "ConditionAttributeXml";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Address
        /// Primary Key: Id
        /// </summary>

        public class AddressTable: DatabaseTable {
            
            public AddressTable(IDataProvider provider):base("Address",provider){
                ClassName = "Address";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("FirstName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("LastName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Email", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Company", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CountryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StateProvinceId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("City", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Address1", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Address2", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ZipPostalCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("PhoneNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("FaxNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CustomAttributes", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn FirstName{
                get{
                    return this.GetColumn("FirstName");
                }
            }
				
   			public static string FirstNameColumn{
			      get{
        			return "FirstName";
      			}
		    }
            
            public IColumn LastName{
                get{
                    return this.GetColumn("LastName");
                }
            }
				
   			public static string LastNameColumn{
			      get{
        			return "LastName";
      			}
		    }
            
            public IColumn Email{
                get{
                    return this.GetColumn("Email");
                }
            }
				
   			public static string EmailColumn{
			      get{
        			return "Email";
      			}
		    }
            
            public IColumn Company{
                get{
                    return this.GetColumn("Company");
                }
            }
				
   			public static string CompanyColumn{
			      get{
        			return "Company";
      			}
		    }
            
            public IColumn CountryId{
                get{
                    return this.GetColumn("CountryId");
                }
            }
				
   			public static string CountryIdColumn{
			      get{
        			return "CountryId";
      			}
		    }
            
            public IColumn StateProvinceId{
                get{
                    return this.GetColumn("StateProvinceId");
                }
            }
				
   			public static string StateProvinceIdColumn{
			      get{
        			return "StateProvinceId";
      			}
		    }
            
            public IColumn City{
                get{
                    return this.GetColumn("City");
                }
            }
				
   			public static string CityColumn{
			      get{
        			return "City";
      			}
		    }
            
            public IColumn Address1{
                get{
                    return this.GetColumn("Address1");
                }
            }
				
   			public static string Address1Column{
			      get{
        			return "Address1";
      			}
		    }
            
            public IColumn Address2{
                get{
                    return this.GetColumn("Address2");
                }
            }
				
   			public static string Address2Column{
			      get{
        			return "Address2";
      			}
		    }
            
            public IColumn ZipPostalCode{
                get{
                    return this.GetColumn("ZipPostalCode");
                }
            }
				
   			public static string ZipPostalCodeColumn{
			      get{
        			return "ZipPostalCode";
      			}
		    }
            
            public IColumn PhoneNumber{
                get{
                    return this.GetColumn("PhoneNumber");
                }
            }
				
   			public static string PhoneNumberColumn{
			      get{
        			return "PhoneNumber";
      			}
		    }
            
            public IColumn FaxNumber{
                get{
                    return this.GetColumn("FaxNumber");
                }
            }
				
   			public static string FaxNumberColumn{
			      get{
        			return "FaxNumber";
      			}
		    }
            
            public IColumn CustomAttributes{
                get{
                    return this.GetColumn("CustomAttributes");
                }
            }
				
   			public static string CustomAttributesColumn{
			      get{
        			return "CustomAttributes";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: StoreMapping
        /// Primary Key: Id
        /// </summary>

        public class StoreMappingTable: DatabaseTable {
            
            public StoreMappingTable(IDataProvider provider):base("StoreMapping",provider){
                ClassName = "StoreMapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn EntityName{
                get{
                    return this.GetColumn("EntityName");
                }
            }
				
   			public static string EntityNameColumn{
			      get{
        			return "EntityName";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Forums_Post
        /// Primary Key: Id
        /// </summary>

        public class Forums_PostTable: DatabaseTable {
            
            public Forums_PostTable(IDataProvider provider):base("Forums_Post",provider){
                ClassName = "Forums_Post";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TopicId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Text", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IPAddress", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UpdatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("VoteCount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn TopicId{
                get{
                    return this.GetColumn("TopicId");
                }
            }
				
   			public static string TopicIdColumn{
			      get{
        			return "TopicId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn Text{
                get{
                    return this.GetColumn("Text");
                }
            }
				
   			public static string TextColumn{
			      get{
        			return "Text";
      			}
		    }
            
            public IColumn IPAddress{
                get{
                    return this.GetColumn("IPAddress");
                }
            }
				
   			public static string IPAddressColumn{
			      get{
        			return "IPAddress";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UpdatedOnUtc{
                get{
                    return this.GetColumn("UpdatedOnUtc");
                }
            }
				
   			public static string UpdatedOnUtcColumn{
			      get{
        			return "UpdatedOnUtc";
      			}
		    }
            
            public IColumn VoteCount{
                get{
                    return this.GetColumn("VoteCount");
                }
            }
				
   			public static string VoteCountColumn{
			      get{
        			return "VoteCount";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ProductAttributeValue
        /// Primary Key: Id
        /// </summary>

        public class ProductAttributeValueTable: DatabaseTable {
            
            public ProductAttributeValueTable(IDataProvider provider):base("ProductAttributeValue",provider){
                ClassName = "ProductAttributeValue";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductAttributeMappingId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AttributeValueTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AssociatedProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ColorSquaresRgb", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("ImageSquaresPictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PriceAdjustment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WeightAdjustment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Cost", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerEntersQty", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Quantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsPreSelected", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductAttributeMappingId{
                get{
                    return this.GetColumn("ProductAttributeMappingId");
                }
            }
				
   			public static string ProductAttributeMappingIdColumn{
			      get{
        			return "ProductAttributeMappingId";
      			}
		    }
            
            public IColumn AttributeValueTypeId{
                get{
                    return this.GetColumn("AttributeValueTypeId");
                }
            }
				
   			public static string AttributeValueTypeIdColumn{
			      get{
        			return "AttributeValueTypeId";
      			}
		    }
            
            public IColumn AssociatedProductId{
                get{
                    return this.GetColumn("AssociatedProductId");
                }
            }
				
   			public static string AssociatedProductIdColumn{
			      get{
        			return "AssociatedProductId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn ColorSquaresRgb{
                get{
                    return this.GetColumn("ColorSquaresRgb");
                }
            }
				
   			public static string ColorSquaresRgbColumn{
			      get{
        			return "ColorSquaresRgb";
      			}
		    }
            
            public IColumn ImageSquaresPictureId{
                get{
                    return this.GetColumn("ImageSquaresPictureId");
                }
            }
				
   			public static string ImageSquaresPictureIdColumn{
			      get{
        			return "ImageSquaresPictureId";
      			}
		    }
            
            public IColumn PriceAdjustment{
                get{
                    return this.GetColumn("PriceAdjustment");
                }
            }
				
   			public static string PriceAdjustmentColumn{
			      get{
        			return "PriceAdjustment";
      			}
		    }
            
            public IColumn WeightAdjustment{
                get{
                    return this.GetColumn("WeightAdjustment");
                }
            }
				
   			public static string WeightAdjustmentColumn{
			      get{
        			return "WeightAdjustment";
      			}
		    }
            
            public IColumn Cost{
                get{
                    return this.GetColumn("Cost");
                }
            }
				
   			public static string CostColumn{
			      get{
        			return "Cost";
      			}
		    }
            
            public IColumn CustomerEntersQty{
                get{
                    return this.GetColumn("CustomerEntersQty");
                }
            }
				
   			public static string CustomerEntersQtyColumn{
			      get{
        			return "CustomerEntersQty";
      			}
		    }
            
            public IColumn Quantity{
                get{
                    return this.GetColumn("Quantity");
                }
            }
				
   			public static string QuantityColumn{
			      get{
        			return "Quantity";
      			}
		    }
            
            public IColumn IsPreSelected{
                get{
                    return this.GetColumn("IsPreSelected");
                }
            }
				
   			public static string IsPreSelectedColumn{
			      get{
        			return "IsPreSelected";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn PictureId{
                get{
                    return this.GetColumn("PictureId");
                }
            }
				
   			public static string PictureIdColumn{
			      get{
        			return "PictureId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: AddressAttribute
        /// Primary Key: Id
        /// </summary>

        public class AddressAttributeTable: DatabaseTable {
            
            public AddressAttributeTable(IDataProvider provider):base("AddressAttribute",provider){
                ClassName = "AddressAttribute";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("IsRequired", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AttributeControlTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn IsRequired{
                get{
                    return this.GetColumn("IsRequired");
                }
            }
				
   			public static string IsRequiredColumn{
			      get{
        			return "IsRequired";
      			}
		    }
            
            public IColumn AttributeControlTypeId{
                get{
                    return this.GetColumn("AttributeControlTypeId");
                }
            }
				
   			public static string AttributeControlTypeIdColumn{
			      get{
        			return "AttributeControlTypeId";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_QT_Tab
        /// Primary Key: Id
        /// </summary>

        public class SS_QT_TabTable: DatabaseTable {
            
            public SS_QT_TabTable(IDataProvider provider):base("SS_QT_Tab",provider){
                ClassName = "SS_QT_Tab";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SystemName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("DisplayName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Description", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TabMode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn SystemName{
                get{
                    return this.GetColumn("SystemName");
                }
            }
				
   			public static string SystemNameColumn{
			      get{
        			return "SystemName";
      			}
		    }
            
            public IColumn DisplayName{
                get{
                    return this.GetColumn("DisplayName");
                }
            }
				
   			public static string DisplayNameColumn{
			      get{
        			return "DisplayName";
      			}
		    }
            
            public IColumn Description{
                get{
                    return this.GetColumn("Description");
                }
            }
				
   			public static string DescriptionColumn{
			      get{
        			return "Description";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
            public IColumn TabMode{
                get{
                    return this.GetColumn("TabMode");
                }
            }
				
   			public static string TabModeColumn{
			      get{
        			return "TabMode";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: TaxCategory
        /// Primary Key: Id
        /// </summary>

        public class TaxCategoryTable: DatabaseTable {
            
            public TaxCategoryTable(IDataProvider provider):base("TaxCategory",provider){
                ClassName = "TaxCategory";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Forums_PostVote
        /// Primary Key: Id
        /// </summary>

        public class Forums_PostVoteTable: DatabaseTable {
            
            public Forums_PostVoteTable(IDataProvider provider):base("Forums_PostVote",provider){
                ClassName = "Forums_PostVote";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ForumPostId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsUp", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ForumPostId{
                get{
                    return this.GetColumn("ForumPostId");
                }
            }
				
   			public static string ForumPostIdColumn{
			      get{
        			return "ForumPostId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn IsUp{
                get{
                    return this.GetColumn("IsUp");
                }
            }
				
   			public static string IsUpColumn{
			      get{
        			return "IsUp";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ProductAvailabilityRange
        /// Primary Key: Id
        /// </summary>

        public class ProductAvailabilityRangeTable: DatabaseTable {
            
            public ProductAvailabilityRangeTable(IDataProvider provider):base("ProductAvailabilityRange",provider){
                ClassName = "ProductAvailabilityRange";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: AddressAttributeValue
        /// Primary Key: Id
        /// </summary>

        public class AddressAttributeValueTable: DatabaseTable {
            
            public AddressAttributeValueTable(IDataProvider provider):base("AddressAttributeValue",provider){
                ClassName = "AddressAttributeValue";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AddressAttributeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("IsPreSelected", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn AddressAttributeId{
                get{
                    return this.GetColumn("AddressAttributeId");
                }
            }
				
   			public static string AddressAttributeIdColumn{
			      get{
        			return "AddressAttributeId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn IsPreSelected{
                get{
                    return this.GetColumn("IsPreSelected");
                }
            }
				
   			public static string IsPreSelectedColumn{
			      get{
        			return "IsPreSelected";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_RB_Category
        /// Primary Key: Id
        /// </summary>

        public class SS_RB_CategoryTable: DatabaseTable {
            
            public SS_RB_CategoryTable(IDataProvider provider):base("SS_RB_Category",provider){
                ClassName = "SS_RB_Category";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LanguageId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SEOTitle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("SEODescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("SEOKeywords", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("SEName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn LanguageId{
                get{
                    return this.GetColumn("LanguageId");
                }
            }
				
   			public static string LanguageIdColumn{
			      get{
        			return "LanguageId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn SEOTitle{
                get{
                    return this.GetColumn("SEOTitle");
                }
            }
				
   			public static string SEOTitleColumn{
			      get{
        			return "SEOTitle";
      			}
		    }
            
            public IColumn SEODescription{
                get{
                    return this.GetColumn("SEODescription");
                }
            }
				
   			public static string SEODescriptionColumn{
			      get{
        			return "SEODescription";
      			}
		    }
            
            public IColumn SEOKeywords{
                get{
                    return this.GetColumn("SEOKeywords");
                }
            }
				
   			public static string SEOKeywordsColumn{
			      get{
        			return "SEOKeywords";
      			}
		    }
            
            public IColumn SEName{
                get{
                    return this.GetColumn("SEName");
                }
            }
				
   			public static string SENameColumn{
			      get{
        			return "SEName";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: TierPrice
        /// Primary Key: Id
        /// </summary>

        public class TierPriceTable: DatabaseTable {
            
            public TierPriceTable(IDataProvider provider):base("TierPrice",provider){
                ClassName = "TierPrice";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerRoleId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Quantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Price", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StartDateTimeUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EndDateTimeUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn CustomerRoleId{
                get{
                    return this.GetColumn("CustomerRoleId");
                }
            }
				
   			public static string CustomerRoleIdColumn{
			      get{
        			return "CustomerRoleId";
      			}
		    }
            
            public IColumn Quantity{
                get{
                    return this.GetColumn("Quantity");
                }
            }
				
   			public static string QuantityColumn{
			      get{
        			return "Quantity";
      			}
		    }
            
            public IColumn Price{
                get{
                    return this.GetColumn("Price");
                }
            }
				
   			public static string PriceColumn{
			      get{
        			return "Price";
      			}
		    }
            
            public IColumn StartDateTimeUtc{
                get{
                    return this.GetColumn("StartDateTimeUtc");
                }
            }
				
   			public static string StartDateTimeUtcColumn{
			      get{
        			return "StartDateTimeUtc";
      			}
		    }
            
            public IColumn EndDateTimeUtc{
                get{
                    return this.GetColumn("EndDateTimeUtc");
                }
            }
				
   			public static string EndDateTimeUtcColumn{
			      get{
        			return "EndDateTimeUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Forums_Subscription
        /// Primary Key: Id
        /// </summary>

        public class Forums_SubscriptionTable: DatabaseTable {
            
            public Forums_SubscriptionTable(IDataProvider provider):base("Forums_Subscription",provider){
                ClassName = "Forums_Subscription";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SubscriptionGuid", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Guid,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ForumId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TopicId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn SubscriptionGuid{
                get{
                    return this.GetColumn("SubscriptionGuid");
                }
            }
				
   			public static string SubscriptionGuidColumn{
			      get{
        			return "SubscriptionGuid";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn ForumId{
                get{
                    return this.GetColumn("ForumId");
                }
            }
				
   			public static string ForumIdColumn{
			      get{
        			return "ForumId";
      			}
		    }
            
            public IColumn TopicId{
                get{
                    return this.GetColumn("TopicId");
                }
            }
				
   			public static string TopicIdColumn{
			      get{
        			return "TopicId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Product_Category_Mapping
        /// Primary Key: Id
        /// </summary>

        public class Product_Category_MappingTable: DatabaseTable {
            
            public Product_Category_MappingTable(IDataProvider provider):base("Product_Category_Mapping",provider){
                ClassName = "Product_Category_Mapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CategoryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsFeaturedProduct", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn CategoryId{
                get{
                    return this.GetColumn("CategoryId");
                }
            }
				
   			public static string CategoryIdColumn{
			      get{
        			return "CategoryId";
      			}
		    }
            
            public IColumn IsFeaturedProduct{
                get{
                    return this.GetColumn("IsFeaturedProduct");
                }
            }
				
   			public static string IsFeaturedProductColumn{
			      get{
        			return "IsFeaturedProduct";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Affiliate
        /// Primary Key: Id
        /// </summary>

        public class AffiliateTable: DatabaseTable {
            
            public AffiliateTable(IDataProvider provider):base("Affiliate",provider){
                ClassName = "Affiliate";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AddressId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AdminComment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("FriendlyUrlName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Deleted", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Active", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn AddressId{
                get{
                    return this.GetColumn("AddressId");
                }
            }
				
   			public static string AddressIdColumn{
			      get{
        			return "AddressId";
      			}
		    }
            
            public IColumn AdminComment{
                get{
                    return this.GetColumn("AdminComment");
                }
            }
				
   			public static string AdminCommentColumn{
			      get{
        			return "AdminComment";
      			}
		    }
            
            public IColumn FriendlyUrlName{
                get{
                    return this.GetColumn("FriendlyUrlName");
                }
            }
				
   			public static string FriendlyUrlNameColumn{
			      get{
        			return "FriendlyUrlName";
      			}
		    }
            
            public IColumn Deleted{
                get{
                    return this.GetColumn("Deleted");
                }
            }
				
   			public static string DeletedColumn{
			      get{
        			return "Deleted";
      			}
		    }
            
            public IColumn Active{
                get{
                    return this.GetColumn("Active");
                }
            }
				
   			public static string ActiveColumn{
			      get{
        			return "Active";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_RB_Post
        /// Primary Key: Id
        /// </summary>

        public class SS_RB_PostTable: DatabaseTable {
            
            public SS_RB_PostTable(IDataProvider provider):base("SS_RB_Post",provider){
                ClassName = "SS_RB_Post";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("HomePagePictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BlogPostId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn HomePagePictureId{
                get{
                    return this.GetColumn("HomePagePictureId");
                }
            }
				
   			public static string HomePagePictureIdColumn{
			      get{
        			return "HomePagePictureId";
      			}
		    }
            
            public IColumn PictureId{
                get{
                    return this.GetColumn("PictureId");
                }
            }
				
   			public static string PictureIdColumn{
			      get{
        			return "PictureId";
      			}
		    }
            
            public IColumn BlogPostId{
                get{
                    return this.GetColumn("BlogPostId");
                }
            }
				
   			public static string BlogPostIdColumn{
			      get{
        			return "BlogPostId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Topic
        /// Primary Key: Id
        /// </summary>

        public class TopicTable: DatabaseTable {
            
            public TopicTable(IDataProvider provider):base("Topic",provider){
                ClassName = "Topic";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SystemName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IncludeInSitemap", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IncludeInTopMenu", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IncludeInFooterColumn1", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IncludeInFooterColumn2", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IncludeInFooterColumn3", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AccessibleWhenStoreClosed", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsPasswordProtected", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Password", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Title", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Body", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TopicTemplateId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MetaKeywords", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MetaDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MetaTitle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("SubjectToAcl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn SystemName{
                get{
                    return this.GetColumn("SystemName");
                }
            }
				
   			public static string SystemNameColumn{
			      get{
        			return "SystemName";
      			}
		    }
            
            public IColumn IncludeInSitemap{
                get{
                    return this.GetColumn("IncludeInSitemap");
                }
            }
				
   			public static string IncludeInSitemapColumn{
			      get{
        			return "IncludeInSitemap";
      			}
		    }
            
            public IColumn IncludeInTopMenu{
                get{
                    return this.GetColumn("IncludeInTopMenu");
                }
            }
				
   			public static string IncludeInTopMenuColumn{
			      get{
        			return "IncludeInTopMenu";
      			}
		    }
            
            public IColumn IncludeInFooterColumn1{
                get{
                    return this.GetColumn("IncludeInFooterColumn1");
                }
            }
				
   			public static string IncludeInFooterColumn1Column{
			      get{
        			return "IncludeInFooterColumn1";
      			}
		    }
            
            public IColumn IncludeInFooterColumn2{
                get{
                    return this.GetColumn("IncludeInFooterColumn2");
                }
            }
				
   			public static string IncludeInFooterColumn2Column{
			      get{
        			return "IncludeInFooterColumn2";
      			}
		    }
            
            public IColumn IncludeInFooterColumn3{
                get{
                    return this.GetColumn("IncludeInFooterColumn3");
                }
            }
				
   			public static string IncludeInFooterColumn3Column{
			      get{
        			return "IncludeInFooterColumn3";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn AccessibleWhenStoreClosed{
                get{
                    return this.GetColumn("AccessibleWhenStoreClosed");
                }
            }
				
   			public static string AccessibleWhenStoreClosedColumn{
			      get{
        			return "AccessibleWhenStoreClosed";
      			}
		    }
            
            public IColumn IsPasswordProtected{
                get{
                    return this.GetColumn("IsPasswordProtected");
                }
            }
				
   			public static string IsPasswordProtectedColumn{
			      get{
        			return "IsPasswordProtected";
      			}
		    }
            
            public IColumn Password{
                get{
                    return this.GetColumn("Password");
                }
            }
				
   			public static string PasswordColumn{
			      get{
        			return "Password";
      			}
		    }
            
            public IColumn Title{
                get{
                    return this.GetColumn("Title");
                }
            }
				
   			public static string TitleColumn{
			      get{
        			return "Title";
      			}
		    }
            
            public IColumn Body{
                get{
                    return this.GetColumn("Body");
                }
            }
				
   			public static string BodyColumn{
			      get{
        			return "Body";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn TopicTemplateId{
                get{
                    return this.GetColumn("TopicTemplateId");
                }
            }
				
   			public static string TopicTemplateIdColumn{
			      get{
        			return "TopicTemplateId";
      			}
		    }
            
            public IColumn MetaKeywords{
                get{
                    return this.GetColumn("MetaKeywords");
                }
            }
				
   			public static string MetaKeywordsColumn{
			      get{
        			return "MetaKeywords";
      			}
		    }
            
            public IColumn MetaDescription{
                get{
                    return this.GetColumn("MetaDescription");
                }
            }
				
   			public static string MetaDescriptionColumn{
			      get{
        			return "MetaDescription";
      			}
		    }
            
            public IColumn MetaTitle{
                get{
                    return this.GetColumn("MetaTitle");
                }
            }
				
   			public static string MetaTitleColumn{
			      get{
        			return "MetaTitle";
      			}
		    }
            
            public IColumn SubjectToAcl{
                get{
                    return this.GetColumn("SubjectToAcl");
                }
            }
				
   			public static string SubjectToAclColumn{
			      get{
        			return "SubjectToAcl";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Forums_Topic
        /// Primary Key: Id
        /// </summary>

        public class Forums_TopicTable: DatabaseTable {
            
            public Forums_TopicTable(IDataProvider provider):base("Forums_Topic",provider){
                ClassName = "Forums_Topic";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ForumId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TopicTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Subject", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 450
                });

                Columns.Add(new DatabaseColumn("NumPosts", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Views", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastPostId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastPostCustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastPostTime", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UpdatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ForumId{
                get{
                    return this.GetColumn("ForumId");
                }
            }
				
   			public static string ForumIdColumn{
			      get{
        			return "ForumId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn TopicTypeId{
                get{
                    return this.GetColumn("TopicTypeId");
                }
            }
				
   			public static string TopicTypeIdColumn{
			      get{
        			return "TopicTypeId";
      			}
		    }
            
            public IColumn Subject{
                get{
                    return this.GetColumn("Subject");
                }
            }
				
   			public static string SubjectColumn{
			      get{
        			return "Subject";
      			}
		    }
            
            public IColumn NumPosts{
                get{
                    return this.GetColumn("NumPosts");
                }
            }
				
   			public static string NumPostsColumn{
			      get{
        			return "NumPosts";
      			}
		    }
            
            public IColumn Views{
                get{
                    return this.GetColumn("Views");
                }
            }
				
   			public static string ViewsColumn{
			      get{
        			return "Views";
      			}
		    }
            
            public IColumn LastPostId{
                get{
                    return this.GetColumn("LastPostId");
                }
            }
				
   			public static string LastPostIdColumn{
			      get{
        			return "LastPostId";
      			}
		    }
            
            public IColumn LastPostCustomerId{
                get{
                    return this.GetColumn("LastPostCustomerId");
                }
            }
				
   			public static string LastPostCustomerIdColumn{
			      get{
        			return "LastPostCustomerId";
      			}
		    }
            
            public IColumn LastPostTime{
                get{
                    return this.GetColumn("LastPostTime");
                }
            }
				
   			public static string LastPostTimeColumn{
			      get{
        			return "LastPostTime";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UpdatedOnUtc{
                get{
                    return this.GetColumn("UpdatedOnUtc");
                }
            }
				
   			public static string UpdatedOnUtcColumn{
			      get{
        			return "UpdatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Product_Manufacturer_Mapping
        /// Primary Key: Id
        /// </summary>

        public class Product_Manufacturer_MappingTable: DatabaseTable {
            
            public Product_Manufacturer_MappingTable(IDataProvider provider):base("Product_Manufacturer_Mapping",provider){
                ClassName = "Product_Manufacturer_Mapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ManufacturerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsFeaturedProduct", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn ManufacturerId{
                get{
                    return this.GetColumn("ManufacturerId");
                }
            }
				
   			public static string ManufacturerIdColumn{
			      get{
        			return "ManufacturerId";
      			}
		    }
            
            public IColumn IsFeaturedProduct{
                get{
                    return this.GetColumn("IsFeaturedProduct");
                }
            }
				
   			public static string IsFeaturedProductColumn{
			      get{
        			return "IsFeaturedProduct";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: BackInStockSubscription
        /// Primary Key: Id
        /// </summary>

        public class BackInStockSubscriptionTable: DatabaseTable {
            
            public BackInStockSubscriptionTable(IDataProvider provider):base("BackInStockSubscription",provider){
                ClassName = "BackInStockSubscription";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_RB_RichBlogPostCategoryMapping
        /// Primary Key: Id
        /// </summary>

        public class SS_RB_RichBlogPostCategoryMappingTable: DatabaseTable {
            
            public SS_RB_RichBlogPostCategoryMappingTable(IDataProvider provider):base("SS_RB_RichBlogPostCategoryMapping",provider){
                ClassName = "SS_RB_RichBlogPostCategoryMapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BlogPostId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CategoryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn BlogPostId{
                get{
                    return this.GetColumn("BlogPostId");
                }
            }
				
   			public static string BlogPostIdColumn{
			      get{
        			return "BlogPostId";
      			}
		    }
            
            public IColumn CategoryId{
                get{
                    return this.GetColumn("CategoryId");
                }
            }
				
   			public static string CategoryIdColumn{
			      get{
        			return "CategoryId";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: TopicTemplate
        /// Primary Key: Id
        /// </summary>

        public class TopicTemplateTable: DatabaseTable {
            
            public TopicTemplateTable(IDataProvider provider):base("TopicTemplate",provider){
                ClassName = "TopicTemplate";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ViewPath", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn ViewPath{
                get{
                    return this.GetColumn("ViewPath");
                }
            }
				
   			public static string ViewPathColumn{
			      get{
        			return "ViewPath";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: GenericAttribute
        /// Primary Key: Id
        /// </summary>

        public class GenericAttributeTable: DatabaseTable {
            
            public GenericAttributeTable(IDataProvider provider):base("GenericAttribute",provider){
                ClassName = "GenericAttribute";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("KeyGroup", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Key", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Value", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn KeyGroup{
                get{
                    return this.GetColumn("KeyGroup");
                }
            }
				
   			public static string KeyGroupColumn{
			      get{
        			return "KeyGroup";
      			}
		    }
            
            public IColumn Key{
                get{
                    return this.GetColumn("Key");
                }
            }
				
   			public static string KeyColumn{
			      get{
        			return "Key";
      			}
		    }
            
            public IColumn Value{
                get{
                    return this.GetColumn("Value");
                }
            }
				
   			public static string ValueColumn{
			      get{
        			return "Value";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Product_Picture_Mapping
        /// Primary Key: Id
        /// </summary>

        public class Product_Picture_MappingTable: DatabaseTable {
            
            public Product_Picture_MappingTable(IDataProvider provider):base("Product_Picture_Mapping",provider){
                ClassName = "Product_Picture_Mapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn PictureId{
                get{
                    return this.GetColumn("PictureId");
                }
            }
				
   			public static string PictureIdColumn{
			      get{
        			return "PictureId";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: BlogComment
        /// Primary Key: Id
        /// </summary>

        public class BlogCommentTable: DatabaseTable {
            
            public BlogCommentTable(IDataProvider provider):base("BlogComment",provider){
                ClassName = "BlogComment";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CommentText", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IsApproved", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BlogPostId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn CommentText{
                get{
                    return this.GetColumn("CommentText");
                }
            }
				
   			public static string CommentTextColumn{
			      get{
        			return "CommentText";
      			}
		    }
            
            public IColumn IsApproved{
                get{
                    return this.GetColumn("IsApproved");
                }
            }
				
   			public static string IsApprovedColumn{
			      get{
        			return "IsApproved";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn BlogPostId{
                get{
                    return this.GetColumn("BlogPostId");
                }
            }
				
   			public static string BlogPostIdColumn{
			      get{
        			return "BlogPostId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: StorePickupPoint
        /// Primary Key: Id
        /// </summary>

        public class StorePickupPointTable: DatabaseTable {
            
            public StorePickupPointTable(IDataProvider provider):base("StorePickupPoint",provider){
                ClassName = "StorePickupPoint";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Description", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AddressId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PickupFee", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OpeningHours", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Description{
                get{
                    return this.GetColumn("Description");
                }
            }
				
   			public static string DescriptionColumn{
			      get{
        			return "Description";
      			}
		    }
            
            public IColumn AddressId{
                get{
                    return this.GetColumn("AddressId");
                }
            }
				
   			public static string AddressIdColumn{
			      get{
        			return "AddressId";
      			}
		    }
            
            public IColumn PickupFee{
                get{
                    return this.GetColumn("PickupFee");
                }
            }
				
   			public static string PickupFeeColumn{
			      get{
        			return "PickupFee";
      			}
		    }
            
            public IColumn OpeningHours{
                get{
                    return this.GetColumn("OpeningHours");
                }
            }
				
   			public static string OpeningHoursColumn{
			      get{
        			return "OpeningHours";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_RB_RelatedBlog
        /// Primary Key: Id
        /// </summary>

        public class SS_RB_RelatedBlogTable: DatabaseTable {
            
            public SS_RB_RelatedBlogTable(IDataProvider provider):base("SS_RB_RelatedBlog",provider){
                ClassName = "SS_RB_RelatedBlog";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BlogPostId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RelatedBlogPostId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn BlogPostId{
                get{
                    return this.GetColumn("BlogPostId");
                }
            }
				
   			public static string BlogPostIdColumn{
			      get{
        			return "BlogPostId";
      			}
		    }
            
            public IColumn RelatedBlogPostId{
                get{
                    return this.GetColumn("RelatedBlogPostId");
                }
            }
				
   			public static string RelatedBlogPostIdColumn{
			      get{
        			return "RelatedBlogPostId";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: UrlRecord
        /// Primary Key: Id
        /// </summary>

        public class UrlRecordTable: DatabaseTable {
            
            public UrlRecordTable(IDataProvider provider):base("UrlRecord",provider){
                ClassName = "UrlRecord";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Slug", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("IsActive", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LanguageId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn EntityName{
                get{
                    return this.GetColumn("EntityName");
                }
            }
				
   			public static string EntityNameColumn{
			      get{
        			return "EntityName";
      			}
		    }
            
            public IColumn Slug{
                get{
                    return this.GetColumn("Slug");
                }
            }
				
   			public static string SlugColumn{
			      get{
        			return "Slug";
      			}
		    }
            
            public IColumn IsActive{
                get{
                    return this.GetColumn("IsActive");
                }
            }
				
   			public static string IsActiveColumn{
			      get{
        			return "IsActive";
      			}
		    }
            
            public IColumn LanguageId{
                get{
                    return this.GetColumn("LanguageId");
                }
            }
				
   			public static string LanguageIdColumn{
			      get{
        			return "LanguageId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: GiftCard
        /// Primary Key: Id
        /// </summary>

        public class GiftCardTable: DatabaseTable {
            
            public GiftCardTable(IDataProvider provider):base("GiftCard",provider){
                ClassName = "GiftCard";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PurchasedWithOrderItemId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("GiftCardTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Amount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsGiftCardActivated", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("GiftCardCouponCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("RecipientName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("RecipientEmail", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("SenderName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("SenderEmail", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Message", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IsRecipientNotified", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn PurchasedWithOrderItemId{
                get{
                    return this.GetColumn("PurchasedWithOrderItemId");
                }
            }
				
   			public static string PurchasedWithOrderItemIdColumn{
			      get{
        			return "PurchasedWithOrderItemId";
      			}
		    }
            
            public IColumn GiftCardTypeId{
                get{
                    return this.GetColumn("GiftCardTypeId");
                }
            }
				
   			public static string GiftCardTypeIdColumn{
			      get{
        			return "GiftCardTypeId";
      			}
		    }
            
            public IColumn Amount{
                get{
                    return this.GetColumn("Amount");
                }
            }
				
   			public static string AmountColumn{
			      get{
        			return "Amount";
      			}
		    }
            
            public IColumn IsGiftCardActivated{
                get{
                    return this.GetColumn("IsGiftCardActivated");
                }
            }
				
   			public static string IsGiftCardActivatedColumn{
			      get{
        			return "IsGiftCardActivated";
      			}
		    }
            
            public IColumn GiftCardCouponCode{
                get{
                    return this.GetColumn("GiftCardCouponCode");
                }
            }
				
   			public static string GiftCardCouponCodeColumn{
			      get{
        			return "GiftCardCouponCode";
      			}
		    }
            
            public IColumn RecipientName{
                get{
                    return this.GetColumn("RecipientName");
                }
            }
				
   			public static string RecipientNameColumn{
			      get{
        			return "RecipientName";
      			}
		    }
            
            public IColumn RecipientEmail{
                get{
                    return this.GetColumn("RecipientEmail");
                }
            }
				
   			public static string RecipientEmailColumn{
			      get{
        			return "RecipientEmail";
      			}
		    }
            
            public IColumn SenderName{
                get{
                    return this.GetColumn("SenderName");
                }
            }
				
   			public static string SenderNameColumn{
			      get{
        			return "SenderName";
      			}
		    }
            
            public IColumn SenderEmail{
                get{
                    return this.GetColumn("SenderEmail");
                }
            }
				
   			public static string SenderEmailColumn{
			      get{
        			return "SenderEmail";
      			}
		    }
            
            public IColumn Message{
                get{
                    return this.GetColumn("Message");
                }
            }
				
   			public static string MessageColumn{
			      get{
        			return "Message";
      			}
		    }
            
            public IColumn IsRecipientNotified{
                get{
                    return this.GetColumn("IsRecipientNotified");
                }
            }
				
   			public static string IsRecipientNotifiedColumn{
			      get{
        			return "IsRecipientNotified";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Product_ProductTag_Mapping
        /// Primary Key: Product_Id
        /// </summary>

        public class Product_ProductTag_MappingTable: DatabaseTable {
            
            public Product_ProductTag_MappingTable(IDataProvider provider):base("Product_ProductTag_Mapping",provider){
                ClassName = "Product_ProductTag_Mapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Product_Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductTag_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Product_Id{
                get{
                    return this.GetColumn("Product_Id");
                }
            }
				
   			public static string Product_IdColumn{
			      get{
        			return "Product_Id";
      			}
		    }
            
            public IColumn ProductTag_Id{
                get{
                    return this.GetColumn("ProductTag_Id");
                }
            }
				
   			public static string ProductTag_IdColumn{
			      get{
        			return "ProductTag_Id";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: BlogPost
        /// Primary Key: Id
        /// </summary>

        public class BlogPostTable: DatabaseTable {
            
            public BlogPostTable(IDataProvider provider):base("BlogPost",provider){
                ClassName = "BlogPost";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LanguageId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Title", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Body", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("BodyOverview", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AllowComments", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Tags", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("StartDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EndDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MetaKeywords", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("MetaDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MetaTitle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn LanguageId{
                get{
                    return this.GetColumn("LanguageId");
                }
            }
				
   			public static string LanguageIdColumn{
			      get{
        			return "LanguageId";
      			}
		    }
            
            public IColumn Title{
                get{
                    return this.GetColumn("Title");
                }
            }
				
   			public static string TitleColumn{
			      get{
        			return "Title";
      			}
		    }
            
            public IColumn Body{
                get{
                    return this.GetColumn("Body");
                }
            }
				
   			public static string BodyColumn{
			      get{
        			return "Body";
      			}
		    }
            
            public IColumn BodyOverview{
                get{
                    return this.GetColumn("BodyOverview");
                }
            }
				
   			public static string BodyOverviewColumn{
			      get{
        			return "BodyOverview";
      			}
		    }
            
            public IColumn AllowComments{
                get{
                    return this.GetColumn("AllowComments");
                }
            }
				
   			public static string AllowCommentsColumn{
			      get{
        			return "AllowComments";
      			}
		    }
            
            public IColumn Tags{
                get{
                    return this.GetColumn("Tags");
                }
            }
				
   			public static string TagsColumn{
			      get{
        			return "Tags";
      			}
		    }
            
            public IColumn StartDateUtc{
                get{
                    return this.GetColumn("StartDateUtc");
                }
            }
				
   			public static string StartDateUtcColumn{
			      get{
        			return "StartDateUtc";
      			}
		    }
            
            public IColumn EndDateUtc{
                get{
                    return this.GetColumn("EndDateUtc");
                }
            }
				
   			public static string EndDateUtcColumn{
			      get{
        			return "EndDateUtc";
      			}
		    }
            
            public IColumn MetaKeywords{
                get{
                    return this.GetColumn("MetaKeywords");
                }
            }
				
   			public static string MetaKeywordsColumn{
			      get{
        			return "MetaKeywords";
      			}
		    }
            
            public IColumn MetaDescription{
                get{
                    return this.GetColumn("MetaDescription");
                }
            }
				
   			public static string MetaDescriptionColumn{
			      get{
        			return "MetaDescription";
      			}
		    }
            
            public IColumn MetaTitle{
                get{
                    return this.GetColumn("MetaTitle");
                }
            }
				
   			public static string MetaTitleColumn{
			      get{
        			return "MetaTitle";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: GoogleProduct
        /// Primary Key: Id
        /// </summary>

        public class GoogleProductTable: DatabaseTable {
            
            public GoogleProductTable(IDataProvider provider):base("GoogleProduct",provider){
                ClassName = "GoogleProduct";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Taxonomy", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CustomGoods", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Gender", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AgeGroup", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Color", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Size", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Material", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Pattern", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ItemGroupId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn Taxonomy{
                get{
                    return this.GetColumn("Taxonomy");
                }
            }
				
   			public static string TaxonomyColumn{
			      get{
        			return "Taxonomy";
      			}
		    }
            
            public IColumn CustomGoods{
                get{
                    return this.GetColumn("CustomGoods");
                }
            }
				
   			public static string CustomGoodsColumn{
			      get{
        			return "CustomGoods";
      			}
		    }
            
            public IColumn Gender{
                get{
                    return this.GetColumn("Gender");
                }
            }
				
   			public static string GenderColumn{
			      get{
        			return "Gender";
      			}
		    }
            
            public IColumn AgeGroup{
                get{
                    return this.GetColumn("AgeGroup");
                }
            }
				
   			public static string AgeGroupColumn{
			      get{
        			return "AgeGroup";
      			}
		    }
            
            public IColumn Color{
                get{
                    return this.GetColumn("Color");
                }
            }
				
   			public static string ColorColumn{
			      get{
        			return "Color";
      			}
		    }
            
            public IColumn Size{
                get{
                    return this.GetColumn("Size");
                }
            }
				
   			public static string SizeColumn{
			      get{
        			return "Size";
      			}
		    }
            
            public IColumn Material{
                get{
                    return this.GetColumn("Material");
                }
            }
				
   			public static string MaterialColumn{
			      get{
        			return "Material";
      			}
		    }
            
            public IColumn Pattern{
                get{
                    return this.GetColumn("Pattern");
                }
            }
				
   			public static string PatternColumn{
			      get{
        			return "Pattern";
      			}
		    }
            
            public IColumn ItemGroupId{
                get{
                    return this.GetColumn("ItemGroupId");
                }
            }
				
   			public static string ItemGroupIdColumn{
			      get{
        			return "ItemGroupId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_SPC_ProductsGroup
        /// Primary Key: Id
        /// </summary>

        public class SS_SPC_ProductsGroupTable: DatabaseTable {
            
            public SS_SPC_ProductsGroupTable(IDataProvider provider):base("SS_SPC_ProductsGroup",provider){
                ClassName = "SS_SPC_ProductsGroup";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Title", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("WidgetZone", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Store", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("NumberOfProductsPerItem", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn Title{
                get{
                    return this.GetColumn("Title");
                }
            }
				
   			public static string TitleColumn{
			      get{
        			return "Title";
      			}
		    }
            
            public IColumn WidgetZone{
                get{
                    return this.GetColumn("WidgetZone");
                }
            }
				
   			public static string WidgetZoneColumn{
			      get{
        			return "WidgetZone";
      			}
		    }
            
            public IColumn Store{
                get{
                    return this.GetColumn("Store");
                }
            }
				
   			public static string StoreColumn{
			      get{
        			return "Store";
      			}
		    }
            
            public IColumn NumberOfProductsPerItem{
                get{
                    return this.GetColumn("NumberOfProductsPerItem");
                }
            }
				
   			public static string NumberOfProductsPerItemColumn{
			      get{
        			return "NumberOfProductsPerItem";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Vendor
        /// Primary Key: Id
        /// </summary>

        public class VendorTable: DatabaseTable {
            
            public VendorTable(IDataProvider provider):base("Vendor",provider){
                ClassName = "Vendor";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Email", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Description", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("PictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AddressId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AdminComment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Active", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Deleted", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MetaKeywords", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("MetaDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MetaTitle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("PageSize", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AllowCustomersToSelectPageSize", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PageSizeOptions", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Email{
                get{
                    return this.GetColumn("Email");
                }
            }
				
   			public static string EmailColumn{
			      get{
        			return "Email";
      			}
		    }
            
            public IColumn Description{
                get{
                    return this.GetColumn("Description");
                }
            }
				
   			public static string DescriptionColumn{
			      get{
        			return "Description";
      			}
		    }
            
            public IColumn PictureId{
                get{
                    return this.GetColumn("PictureId");
                }
            }
				
   			public static string PictureIdColumn{
			      get{
        			return "PictureId";
      			}
		    }
            
            public IColumn AddressId{
                get{
                    return this.GetColumn("AddressId");
                }
            }
				
   			public static string AddressIdColumn{
			      get{
        			return "AddressId";
      			}
		    }
            
            public IColumn AdminComment{
                get{
                    return this.GetColumn("AdminComment");
                }
            }
				
   			public static string AdminCommentColumn{
			      get{
        			return "AdminComment";
      			}
		    }
            
            public IColumn Active{
                get{
                    return this.GetColumn("Active");
                }
            }
				
   			public static string ActiveColumn{
			      get{
        			return "Active";
      			}
		    }
            
            public IColumn Deleted{
                get{
                    return this.GetColumn("Deleted");
                }
            }
				
   			public static string DeletedColumn{
			      get{
        			return "Deleted";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn MetaKeywords{
                get{
                    return this.GetColumn("MetaKeywords");
                }
            }
				
   			public static string MetaKeywordsColumn{
			      get{
        			return "MetaKeywords";
      			}
		    }
            
            public IColumn MetaDescription{
                get{
                    return this.GetColumn("MetaDescription");
                }
            }
				
   			public static string MetaDescriptionColumn{
			      get{
        			return "MetaDescription";
      			}
		    }
            
            public IColumn MetaTitle{
                get{
                    return this.GetColumn("MetaTitle");
                }
            }
				
   			public static string MetaTitleColumn{
			      get{
        			return "MetaTitle";
      			}
		    }
            
            public IColumn PageSize{
                get{
                    return this.GetColumn("PageSize");
                }
            }
				
   			public static string PageSizeColumn{
			      get{
        			return "PageSize";
      			}
		    }
            
            public IColumn AllowCustomersToSelectPageSize{
                get{
                    return this.GetColumn("AllowCustomersToSelectPageSize");
                }
            }
				
   			public static string AllowCustomersToSelectPageSizeColumn{
			      get{
        			return "AllowCustomersToSelectPageSize";
      			}
		    }
            
            public IColumn PageSizeOptions{
                get{
                    return this.GetColumn("PageSizeOptions");
                }
            }
				
   			public static string PageSizeOptionsColumn{
			      get{
        			return "PageSizeOptions";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: GiftCardUsageHistory
        /// Primary Key: Id
        /// </summary>

        public class GiftCardUsageHistoryTable: DatabaseTable {
            
            public GiftCardUsageHistoryTable(IDataProvider provider):base("GiftCardUsageHistory",provider){
                ClassName = "GiftCardUsageHistory";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("GiftCardId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UsedWithOrderId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UsedValue", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn GiftCardId{
                get{
                    return this.GetColumn("GiftCardId");
                }
            }
				
   			public static string GiftCardIdColumn{
			      get{
        			return "GiftCardId";
      			}
		    }
            
            public IColumn UsedWithOrderId{
                get{
                    return this.GetColumn("UsedWithOrderId");
                }
            }
				
   			public static string UsedWithOrderIdColumn{
			      get{
        			return "UsedWithOrderId";
      			}
		    }
            
            public IColumn UsedValue{
                get{
                    return this.GetColumn("UsedValue");
                }
            }
				
   			public static string UsedValueColumn{
			      get{
        			return "UsedValue";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ProductReview
        /// Primary Key: Id
        /// </summary>

        public class ProductReviewTable: DatabaseTable {
            
            public ProductReviewTable(IDataProvider provider):base("ProductReview",provider){
                ClassName = "ProductReview";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsApproved", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Title", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ReviewText", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ReplyText", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Rating", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("HelpfulYesTotal", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("HelpfulNoTotal", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn IsApproved{
                get{
                    return this.GetColumn("IsApproved");
                }
            }
				
   			public static string IsApprovedColumn{
			      get{
        			return "IsApproved";
      			}
		    }
            
            public IColumn Title{
                get{
                    return this.GetColumn("Title");
                }
            }
				
   			public static string TitleColumn{
			      get{
        			return "Title";
      			}
		    }
            
            public IColumn ReviewText{
                get{
                    return this.GetColumn("ReviewText");
                }
            }
				
   			public static string ReviewTextColumn{
			      get{
        			return "ReviewText";
      			}
		    }
            
            public IColumn ReplyText{
                get{
                    return this.GetColumn("ReplyText");
                }
            }
				
   			public static string ReplyTextColumn{
			      get{
        			return "ReplyText";
      			}
		    }
            
            public IColumn Rating{
                get{
                    return this.GetColumn("Rating");
                }
            }
				
   			public static string RatingColumn{
			      get{
        			return "Rating";
      			}
		    }
            
            public IColumn HelpfulYesTotal{
                get{
                    return this.GetColumn("HelpfulYesTotal");
                }
            }
				
   			public static string HelpfulYesTotalColumn{
			      get{
        			return "HelpfulYesTotal";
      			}
		    }
            
            public IColumn HelpfulNoTotal{
                get{
                    return this.GetColumn("HelpfulNoTotal");
                }
            }
				
   			public static string HelpfulNoTotalColumn{
			      get{
        			return "HelpfulNoTotal";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Campaign
        /// Primary Key: Id
        /// </summary>

        public class CampaignTable: DatabaseTable {
            
            public CampaignTable(IDataProvider provider):base("Campaign",provider){
                ClassName = "Campaign";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Subject", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Body", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerRoleId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DontSendBeforeDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Subject{
                get{
                    return this.GetColumn("Subject");
                }
            }
				
   			public static string SubjectColumn{
			      get{
        			return "Subject";
      			}
		    }
            
            public IColumn Body{
                get{
                    return this.GetColumn("Body");
                }
            }
				
   			public static string BodyColumn{
			      get{
        			return "Body";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn CustomerRoleId{
                get{
                    return this.GetColumn("CustomerRoleId");
                }
            }
				
   			public static string CustomerRoleIdColumn{
			      get{
        			return "CustomerRoleId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn DontSendBeforeDateUtc{
                get{
                    return this.GetColumn("DontSendBeforeDateUtc");
                }
            }
				
   			public static string DontSendBeforeDateUtcColumn{
			      get{
        			return "DontSendBeforeDateUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ShippingByWeight
        /// Primary Key: Id
        /// </summary>

        public class ShippingByWeightTable: DatabaseTable {
            
            public ShippingByWeightTable(IDataProvider provider):base("ShippingByWeight",provider){
                ClassName = "ShippingByWeight";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WarehouseId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CountryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StateProvinceId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Zip", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ShippingMethodId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("From", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("To", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AdditionalFixedCost", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PercentageRateOfSubtotal", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RatePerWeightUnit", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LowerWeightLimit", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn WarehouseId{
                get{
                    return this.GetColumn("WarehouseId");
                }
            }
				
   			public static string WarehouseIdColumn{
			      get{
        			return "WarehouseId";
      			}
		    }
            
            public IColumn CountryId{
                get{
                    return this.GetColumn("CountryId");
                }
            }
				
   			public static string CountryIdColumn{
			      get{
        			return "CountryId";
      			}
		    }
            
            public IColumn StateProvinceId{
                get{
                    return this.GetColumn("StateProvinceId");
                }
            }
				
   			public static string StateProvinceIdColumn{
			      get{
        			return "StateProvinceId";
      			}
		    }
            
            public IColumn Zip{
                get{
                    return this.GetColumn("Zip");
                }
            }
				
   			public static string ZipColumn{
			      get{
        			return "Zip";
      			}
		    }
            
            public IColumn ShippingMethodId{
                get{
                    return this.GetColumn("ShippingMethodId");
                }
            }
				
   			public static string ShippingMethodIdColumn{
			      get{
        			return "ShippingMethodId";
      			}
		    }
            
            public IColumn From{
                get{
                    return this.GetColumn("From");
                }
            }
				
   			public static string FromColumn{
			      get{
        			return "From";
      			}
		    }
            
            public IColumn To{
                get{
                    return this.GetColumn("To");
                }
            }
				
   			public static string ToColumn{
			      get{
        			return "To";
      			}
		    }
            
            public IColumn AdditionalFixedCost{
                get{
                    return this.GetColumn("AdditionalFixedCost");
                }
            }
				
   			public static string AdditionalFixedCostColumn{
			      get{
        			return "AdditionalFixedCost";
      			}
		    }
            
            public IColumn PercentageRateOfSubtotal{
                get{
                    return this.GetColumn("PercentageRateOfSubtotal");
                }
            }
				
   			public static string PercentageRateOfSubtotalColumn{
			      get{
        			return "PercentageRateOfSubtotal";
      			}
		    }
            
            public IColumn RatePerWeightUnit{
                get{
                    return this.GetColumn("RatePerWeightUnit");
                }
            }
				
   			public static string RatePerWeightUnitColumn{
			      get{
        			return "RatePerWeightUnit";
      			}
		    }
            
            public IColumn LowerWeightLimit{
                get{
                    return this.GetColumn("LowerWeightLimit");
                }
            }
				
   			public static string LowerWeightLimitColumn{
			      get{
        			return "LowerWeightLimit";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_SPC_ProductsGroupItem
        /// Primary Key: Id
        /// </summary>

        public class SS_SPC_ProductsGroupItemTable: DatabaseTable {
            
            public SS_SPC_ProductsGroupItemTable(IDataProvider provider):base("SS_SPC_ProductsGroupItem",provider){
                ClassName = "SS_SPC_ProductsGroupItem";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Active", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Title", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("SourceType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SortMethod", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("GroupId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Active{
                get{
                    return this.GetColumn("Active");
                }
            }
				
   			public static string ActiveColumn{
			      get{
        			return "Active";
      			}
		    }
            
            public IColumn Title{
                get{
                    return this.GetColumn("Title");
                }
            }
				
   			public static string TitleColumn{
			      get{
        			return "Title";
      			}
		    }
            
            public IColumn SourceType{
                get{
                    return this.GetColumn("SourceType");
                }
            }
				
   			public static string SourceTypeColumn{
			      get{
        			return "SourceType";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn SortMethod{
                get{
                    return this.GetColumn("SortMethod");
                }
            }
				
   			public static string SortMethodColumn{
			      get{
        			return "SortMethod";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn GroupId{
                get{
                    return this.GetColumn("GroupId");
                }
            }
				
   			public static string GroupIdColumn{
			      get{
        			return "GroupId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: VendorNote
        /// Primary Key: Id
        /// </summary>

        public class VendorNoteTable: DatabaseTable {
            
            public VendorNoteTable(IDataProvider provider):base("VendorNote",provider){
                ClassName = "VendorNote";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("VendorId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Note", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn VendorId{
                get{
                    return this.GetColumn("VendorId");
                }
            }
				
   			public static string VendorIdColumn{
			      get{
        			return "VendorId";
      			}
		    }
            
            public IColumn Note{
                get{
                    return this.GetColumn("Note");
                }
            }
				
   			public static string NoteColumn{
			      get{
        			return "Note";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Language
        /// Primary Key: Id
        /// </summary>

        public class LanguageTable: DatabaseTable {
            
            public LanguageTable(IDataProvider provider):base("Language",provider){
                ClassName = "Language";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("LanguageCulture", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 20
                });

                Columns.Add(new DatabaseColumn("UniqueSeoCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 2
                });

                Columns.Add(new DatabaseColumn("FlagImageFileName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 50
                });

                Columns.Add(new DatabaseColumn("Rtl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DefaultCurrencyId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn LanguageCulture{
                get{
                    return this.GetColumn("LanguageCulture");
                }
            }
				
   			public static string LanguageCultureColumn{
			      get{
        			return "LanguageCulture";
      			}
		    }
            
            public IColumn UniqueSeoCode{
                get{
                    return this.GetColumn("UniqueSeoCode");
                }
            }
				
   			public static string UniqueSeoCodeColumn{
			      get{
        			return "UniqueSeoCode";
      			}
		    }
            
            public IColumn FlagImageFileName{
                get{
                    return this.GetColumn("FlagImageFileName");
                }
            }
				
   			public static string FlagImageFileNameColumn{
			      get{
        			return "FlagImageFileName";
      			}
		    }
            
            public IColumn Rtl{
                get{
                    return this.GetColumn("Rtl");
                }
            }
				
   			public static string RtlColumn{
			      get{
        			return "Rtl";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
            public IColumn DefaultCurrencyId{
                get{
                    return this.GetColumn("DefaultCurrencyId");
                }
            }
				
   			public static string DefaultCurrencyIdColumn{
			      get{
        			return "DefaultCurrencyId";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ProductReviewHelpfulness
        /// Primary Key: Id
        /// </summary>

        public class ProductReviewHelpfulnessTable: DatabaseTable {
            
            public ProductReviewHelpfulnessTable(IDataProvider provider):base("ProductReviewHelpfulness",provider){
                ClassName = "ProductReviewHelpfulness";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductReviewId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WasHelpful", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductReviewId{
                get{
                    return this.GetColumn("ProductReviewId");
                }
            }
				
   			public static string ProductReviewIdColumn{
			      get{
        			return "ProductReviewId";
      			}
		    }
            
            public IColumn WasHelpful{
                get{
                    return this.GetColumn("WasHelpful");
                }
            }
				
   			public static string WasHelpfulColumn{
			      get{
        			return "WasHelpful";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Category
        /// Primary Key: Id
        /// </summary>

        public class CategoryTable: DatabaseTable {
            
            public CategoryTable(IDataProvider provider):base("Category",provider){
                ClassName = "Category";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Description", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CategoryTemplateId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MetaKeywords", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("MetaDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MetaTitle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ParentCategoryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PageSize", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AllowCustomersToSelectPageSize", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PageSizeOptions", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("PriceRanges", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ShowOnHomePage", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IncludeInTopMenu", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SubjectToAcl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Deleted", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UpdatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Description{
                get{
                    return this.GetColumn("Description");
                }
            }
				
   			public static string DescriptionColumn{
			      get{
        			return "Description";
      			}
		    }
            
            public IColumn CategoryTemplateId{
                get{
                    return this.GetColumn("CategoryTemplateId");
                }
            }
				
   			public static string CategoryTemplateIdColumn{
			      get{
        			return "CategoryTemplateId";
      			}
		    }
            
            public IColumn MetaKeywords{
                get{
                    return this.GetColumn("MetaKeywords");
                }
            }
				
   			public static string MetaKeywordsColumn{
			      get{
        			return "MetaKeywords";
      			}
		    }
            
            public IColumn MetaDescription{
                get{
                    return this.GetColumn("MetaDescription");
                }
            }
				
   			public static string MetaDescriptionColumn{
			      get{
        			return "MetaDescription";
      			}
		    }
            
            public IColumn MetaTitle{
                get{
                    return this.GetColumn("MetaTitle");
                }
            }
				
   			public static string MetaTitleColumn{
			      get{
        			return "MetaTitle";
      			}
		    }
            
            public IColumn ParentCategoryId{
                get{
                    return this.GetColumn("ParentCategoryId");
                }
            }
				
   			public static string ParentCategoryIdColumn{
			      get{
        			return "ParentCategoryId";
      			}
		    }
            
            public IColumn PictureId{
                get{
                    return this.GetColumn("PictureId");
                }
            }
				
   			public static string PictureIdColumn{
			      get{
        			return "PictureId";
      			}
		    }
            
            public IColumn PageSize{
                get{
                    return this.GetColumn("PageSize");
                }
            }
				
   			public static string PageSizeColumn{
			      get{
        			return "PageSize";
      			}
		    }
            
            public IColumn AllowCustomersToSelectPageSize{
                get{
                    return this.GetColumn("AllowCustomersToSelectPageSize");
                }
            }
				
   			public static string AllowCustomersToSelectPageSizeColumn{
			      get{
        			return "AllowCustomersToSelectPageSize";
      			}
		    }
            
            public IColumn PageSizeOptions{
                get{
                    return this.GetColumn("PageSizeOptions");
                }
            }
				
   			public static string PageSizeOptionsColumn{
			      get{
        			return "PageSizeOptions";
      			}
		    }
            
            public IColumn PriceRanges{
                get{
                    return this.GetColumn("PriceRanges");
                }
            }
				
   			public static string PriceRangesColumn{
			      get{
        			return "PriceRanges";
      			}
		    }
            
            public IColumn ShowOnHomePage{
                get{
                    return this.GetColumn("ShowOnHomePage");
                }
            }
				
   			public static string ShowOnHomePageColumn{
			      get{
        			return "ShowOnHomePage";
      			}
		    }
            
            public IColumn IncludeInTopMenu{
                get{
                    return this.GetColumn("IncludeInTopMenu");
                }
            }
				
   			public static string IncludeInTopMenuColumn{
			      get{
        			return "IncludeInTopMenu";
      			}
		    }
            
            public IColumn SubjectToAcl{
                get{
                    return this.GetColumn("SubjectToAcl");
                }
            }
				
   			public static string SubjectToAclColumn{
			      get{
        			return "SubjectToAcl";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn Deleted{
                get{
                    return this.GetColumn("Deleted");
                }
            }
				
   			public static string DeletedColumn{
			      get{
        			return "Deleted";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UpdatedOnUtc{
                get{
                    return this.GetColumn("UpdatedOnUtc");
                }
            }
				
   			public static string UpdatedOnUtcColumn{
			      get{
        			return "UpdatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: TaxRate
        /// Primary Key: Id
        /// </summary>

        public class TaxRateTable: DatabaseTable {
            
            public TaxRateTable(IDataProvider provider):base("TaxRate",provider){
                ClassName = "TaxRate";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TaxCategoryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CountryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StateProvinceId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Zip", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Percentage", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn TaxCategoryId{
                get{
                    return this.GetColumn("TaxCategoryId");
                }
            }
				
   			public static string TaxCategoryIdColumn{
			      get{
        			return "TaxCategoryId";
      			}
		    }
            
            public IColumn CountryId{
                get{
                    return this.GetColumn("CountryId");
                }
            }
				
   			public static string CountryIdColumn{
			      get{
        			return "CountryId";
      			}
		    }
            
            public IColumn StateProvinceId{
                get{
                    return this.GetColumn("StateProvinceId");
                }
            }
				
   			public static string StateProvinceIdColumn{
			      get{
        			return "StateProvinceId";
      			}
		    }
            
            public IColumn Zip{
                get{
                    return this.GetColumn("Zip");
                }
            }
				
   			public static string ZipColumn{
			      get{
        			return "Zip";
      			}
		    }
            
            public IColumn Percentage{
                get{
                    return this.GetColumn("Percentage");
                }
            }
				
   			public static string PercentageColumn{
			      get{
        			return "Percentage";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Warehouse
        /// Primary Key: Id
        /// </summary>

        public class WarehouseTable: DatabaseTable {
            
            public WarehouseTable(IDataProvider provider):base("Warehouse",provider){
                ClassName = "Warehouse";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("AdminComment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AddressId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn AdminComment{
                get{
                    return this.GetColumn("AdminComment");
                }
            }
				
   			public static string AdminCommentColumn{
			      get{
        			return "AdminComment";
      			}
		    }
            
            public IColumn AddressId{
                get{
                    return this.GetColumn("AddressId");
                }
            }
				
   			public static string AddressIdColumn{
			      get{
        			return "AddressId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: LocaleStringResource
        /// Primary Key: Id
        /// </summary>

        public class LocaleStringResourceTable: DatabaseTable {
            
            public LocaleStringResourceTable(IDataProvider provider):base("LocaleStringResource",provider){
                ClassName = "LocaleStringResource";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LanguageId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ResourceName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("ResourceValue", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn LanguageId{
                get{
                    return this.GetColumn("LanguageId");
                }
            }
				
   			public static string LanguageIdColumn{
			      get{
        			return "LanguageId";
      			}
		    }
            
            public IColumn ResourceName{
                get{
                    return this.GetColumn("ResourceName");
                }
            }
				
   			public static string ResourceNameColumn{
			      get{
        			return "ResourceName";
      			}
		    }
            
            public IColumn ResourceValue{
                get{
                    return this.GetColumn("ResourceValue");
                }
            }
				
   			public static string ResourceValueColumn{
			      get{
        			return "ResourceValue";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Product_SpecificationAttribute_Mapping
        /// Primary Key: Id
        /// </summary>

        public class Product_SpecificationAttribute_MappingTable: DatabaseTable {
            
            public Product_SpecificationAttribute_MappingTable(IDataProvider provider):base("Product_SpecificationAttribute_Mapping",provider){
                ClassName = "Product_SpecificationAttribute_Mapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AttributeTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SpecificationAttributeOptionId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomValue", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 4000
                });

                Columns.Add(new DatabaseColumn("AllowFiltering", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShowOnProductPage", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn AttributeTypeId{
                get{
                    return this.GetColumn("AttributeTypeId");
                }
            }
				
   			public static string AttributeTypeIdColumn{
			      get{
        			return "AttributeTypeId";
      			}
		    }
            
            public IColumn SpecificationAttributeOptionId{
                get{
                    return this.GetColumn("SpecificationAttributeOptionId");
                }
            }
				
   			public static string SpecificationAttributeOptionIdColumn{
			      get{
        			return "SpecificationAttributeOptionId";
      			}
		    }
            
            public IColumn CustomValue{
                get{
                    return this.GetColumn("CustomValue");
                }
            }
				
   			public static string CustomValueColumn{
			      get{
        			return "CustomValue";
      			}
		    }
            
            public IColumn AllowFiltering{
                get{
                    return this.GetColumn("AllowFiltering");
                }
            }
				
   			public static string AllowFilteringColumn{
			      get{
        			return "AllowFiltering";
      			}
		    }
            
            public IColumn ShowOnProductPage{
                get{
                    return this.GetColumn("ShowOnProductPage");
                }
            }
				
   			public static string ShowOnProductPageColumn{
			      get{
        			return "ShowOnProductPage";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: CategoryTemplate
        /// Primary Key: Id
        /// </summary>

        public class CategoryTemplateTable: DatabaseTable {
            
            public CategoryTemplateTable(IDataProvider provider):base("CategoryTemplate",provider){
                ClassName = "CategoryTemplate";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ViewPath", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn ViewPath{
                get{
                    return this.GetColumn("ViewPath");
                }
            }
				
   			public static string ViewPathColumn{
			      get{
        			return "ViewPath";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: LocalizedProperty
        /// Primary Key: Id
        /// </summary>

        public class LocalizedPropertyTable: DatabaseTable {
            
            public LocalizedPropertyTable(IDataProvider provider):base("LocalizedProperty",provider){
                ClassName = "LocalizedProperty";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LanguageId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LocaleKeyGroup", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("LocaleKey", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("LocaleValue", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn LanguageId{
                get{
                    return this.GetColumn("LanguageId");
                }
            }
				
   			public static string LanguageIdColumn{
			      get{
        			return "LanguageId";
      			}
		    }
            
            public IColumn LocaleKeyGroup{
                get{
                    return this.GetColumn("LocaleKeyGroup");
                }
            }
				
   			public static string LocaleKeyGroupColumn{
			      get{
        			return "LocaleKeyGroup";
      			}
		    }
            
            public IColumn LocaleKey{
                get{
                    return this.GetColumn("LocaleKey");
                }
            }
				
   			public static string LocaleKeyColumn{
			      get{
        			return "LocaleKey";
      			}
		    }
            
            public IColumn LocaleValue{
                get{
                    return this.GetColumn("LocaleValue");
                }
            }
				
   			public static string LocaleValueColumn{
			      get{
        			return "LocaleValue";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ProductTag
        /// Primary Key: Id
        /// </summary>

        public class ProductTagTable: DatabaseTable {
            
            public ProductTagTable(IDataProvider provider):base("ProductTag",provider){
                ClassName = "ProductTag";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: CheckoutAttribute
        /// Primary Key: Id
        /// </summary>

        public class CheckoutAttributeTable: DatabaseTable {
            
            public CheckoutAttributeTable(IDataProvider provider):base("CheckoutAttribute",provider){
                ClassName = "CheckoutAttribute";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("TextPrompt", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IsRequired", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShippableProductRequired", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsTaxExempt", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TaxCategoryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AttributeControlTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ValidationMinLength", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ValidationMaxLength", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ValidationFileAllowedExtensions", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ValidationFileMaximumSize", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DefaultValue", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ConditionAttributeXml", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn TextPrompt{
                get{
                    return this.GetColumn("TextPrompt");
                }
            }
				
   			public static string TextPromptColumn{
			      get{
        			return "TextPrompt";
      			}
		    }
            
            public IColumn IsRequired{
                get{
                    return this.GetColumn("IsRequired");
                }
            }
				
   			public static string IsRequiredColumn{
			      get{
        			return "IsRequired";
      			}
		    }
            
            public IColumn ShippableProductRequired{
                get{
                    return this.GetColumn("ShippableProductRequired");
                }
            }
				
   			public static string ShippableProductRequiredColumn{
			      get{
        			return "ShippableProductRequired";
      			}
		    }
            
            public IColumn IsTaxExempt{
                get{
                    return this.GetColumn("IsTaxExempt");
                }
            }
				
   			public static string IsTaxExemptColumn{
			      get{
        			return "IsTaxExempt";
      			}
		    }
            
            public IColumn TaxCategoryId{
                get{
                    return this.GetColumn("TaxCategoryId");
                }
            }
				
   			public static string TaxCategoryIdColumn{
			      get{
        			return "TaxCategoryId";
      			}
		    }
            
            public IColumn AttributeControlTypeId{
                get{
                    return this.GetColumn("AttributeControlTypeId");
                }
            }
				
   			public static string AttributeControlTypeIdColumn{
			      get{
        			return "AttributeControlTypeId";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
            public IColumn ValidationMinLength{
                get{
                    return this.GetColumn("ValidationMinLength");
                }
            }
				
   			public static string ValidationMinLengthColumn{
			      get{
        			return "ValidationMinLength";
      			}
		    }
            
            public IColumn ValidationMaxLength{
                get{
                    return this.GetColumn("ValidationMaxLength");
                }
            }
				
   			public static string ValidationMaxLengthColumn{
			      get{
        			return "ValidationMaxLength";
      			}
		    }
            
            public IColumn ValidationFileAllowedExtensions{
                get{
                    return this.GetColumn("ValidationFileAllowedExtensions");
                }
            }
				
   			public static string ValidationFileAllowedExtensionsColumn{
			      get{
        			return "ValidationFileAllowedExtensions";
      			}
		    }
            
            public IColumn ValidationFileMaximumSize{
                get{
                    return this.GetColumn("ValidationFileMaximumSize");
                }
            }
				
   			public static string ValidationFileMaximumSizeColumn{
			      get{
        			return "ValidationFileMaximumSize";
      			}
		    }
            
            public IColumn DefaultValue{
                get{
                    return this.GetColumn("DefaultValue");
                }
            }
				
   			public static string DefaultValueColumn{
			      get{
        			return "DefaultValue";
      			}
		    }
            
            public IColumn ConditionAttributeXml{
                get{
                    return this.GetColumn("ConditionAttributeXml");
                }
            }
				
   			public static string ConditionAttributeXmlColumn{
			      get{
        			return "ConditionAttributeXml";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Log
        /// Primary Key: Id
        /// </summary>

        public class LogTable: DatabaseTable {
            
            public LogTable(IDataProvider provider):base("Log",provider){
                ClassName = "Log";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LogLevelId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShortMessage", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("FullMessage", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IpAddress", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PageUrl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ReferrerUrl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn LogLevelId{
                get{
                    return this.GetColumn("LogLevelId");
                }
            }
				
   			public static string LogLevelIdColumn{
			      get{
        			return "LogLevelId";
      			}
		    }
            
            public IColumn ShortMessage{
                get{
                    return this.GetColumn("ShortMessage");
                }
            }
				
   			public static string ShortMessageColumn{
			      get{
        			return "ShortMessage";
      			}
		    }
            
            public IColumn FullMessage{
                get{
                    return this.GetColumn("FullMessage");
                }
            }
				
   			public static string FullMessageColumn{
			      get{
        			return "FullMessage";
      			}
		    }
            
            public IColumn IpAddress{
                get{
                    return this.GetColumn("IpAddress");
                }
            }
				
   			public static string IpAddressColumn{
			      get{
        			return "IpAddress";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn PageUrl{
                get{
                    return this.GetColumn("PageUrl");
                }
            }
				
   			public static string PageUrlColumn{
			      get{
        			return "PageUrl";
      			}
		    }
            
            public IColumn ReferrerUrl{
                get{
                    return this.GetColumn("ReferrerUrl");
                }
            }
				
   			public static string ReferrerUrlColumn{
			      get{
        			return "ReferrerUrl";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ProductTemplate
        /// Primary Key: Id
        /// </summary>

        public class ProductTemplateTable: DatabaseTable {
            
            public ProductTemplateTable(IDataProvider provider):base("ProductTemplate",provider){
                ClassName = "ProductTemplate";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ViewPath", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IgnoredProductTypes", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn ViewPath{
                get{
                    return this.GetColumn("ViewPath");
                }
            }
				
   			public static string ViewPathColumn{
			      get{
        			return "ViewPath";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn IgnoredProductTypes{
                get{
                    return this.GetColumn("IgnoredProductTypes");
                }
            }
				
   			public static string IgnoredProductTypesColumn{
			      get{
        			return "IgnoredProductTypes";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: CheckoutAttributeValue
        /// Primary Key: Id
        /// </summary>

        public class CheckoutAttributeValueTable: DatabaseTable {
            
            public CheckoutAttributeValueTable(IDataProvider provider):base("CheckoutAttributeValue",provider){
                ClassName = "CheckoutAttributeValue";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CheckoutAttributeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ColorSquaresRgb", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("PriceAdjustment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WeightAdjustment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsPreSelected", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CheckoutAttributeId{
                get{
                    return this.GetColumn("CheckoutAttributeId");
                }
            }
				
   			public static string CheckoutAttributeIdColumn{
			      get{
        			return "CheckoutAttributeId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn ColorSquaresRgb{
                get{
                    return this.GetColumn("ColorSquaresRgb");
                }
            }
				
   			public static string ColorSquaresRgbColumn{
			      get{
        			return "ColorSquaresRgb";
      			}
		    }
            
            public IColumn PriceAdjustment{
                get{
                    return this.GetColumn("PriceAdjustment");
                }
            }
				
   			public static string PriceAdjustmentColumn{
			      get{
        			return "PriceAdjustment";
      			}
		    }
            
            public IColumn WeightAdjustment{
                get{
                    return this.GetColumn("WeightAdjustment");
                }
            }
				
   			public static string WeightAdjustmentColumn{
			      get{
        			return "WeightAdjustment";
      			}
		    }
            
            public IColumn IsPreSelected{
                get{
                    return this.GetColumn("IsPreSelected");
                }
            }
				
   			public static string IsPreSelectedColumn{
			      get{
        			return "IsPreSelected";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Manufacturer
        /// Primary Key: Id
        /// </summary>

        public class ManufacturerTable: DatabaseTable {
            
            public ManufacturerTable(IDataProvider provider):base("Manufacturer",provider){
                ClassName = "Manufacturer";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Description", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ManufacturerTemplateId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MetaKeywords", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("MetaDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MetaTitle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("PictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PageSize", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AllowCustomersToSelectPageSize", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PageSizeOptions", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("PriceRanges", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("SubjectToAcl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Deleted", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UpdatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Description{
                get{
                    return this.GetColumn("Description");
                }
            }
				
   			public static string DescriptionColumn{
			      get{
        			return "Description";
      			}
		    }
            
            public IColumn ManufacturerTemplateId{
                get{
                    return this.GetColumn("ManufacturerTemplateId");
                }
            }
				
   			public static string ManufacturerTemplateIdColumn{
			      get{
        			return "ManufacturerTemplateId";
      			}
		    }
            
            public IColumn MetaKeywords{
                get{
                    return this.GetColumn("MetaKeywords");
                }
            }
				
   			public static string MetaKeywordsColumn{
			      get{
        			return "MetaKeywords";
      			}
		    }
            
            public IColumn MetaDescription{
                get{
                    return this.GetColumn("MetaDescription");
                }
            }
				
   			public static string MetaDescriptionColumn{
			      get{
        			return "MetaDescription";
      			}
		    }
            
            public IColumn MetaTitle{
                get{
                    return this.GetColumn("MetaTitle");
                }
            }
				
   			public static string MetaTitleColumn{
			      get{
        			return "MetaTitle";
      			}
		    }
            
            public IColumn PictureId{
                get{
                    return this.GetColumn("PictureId");
                }
            }
				
   			public static string PictureIdColumn{
			      get{
        			return "PictureId";
      			}
		    }
            
            public IColumn PageSize{
                get{
                    return this.GetColumn("PageSize");
                }
            }
				
   			public static string PageSizeColumn{
			      get{
        			return "PageSize";
      			}
		    }
            
            public IColumn AllowCustomersToSelectPageSize{
                get{
                    return this.GetColumn("AllowCustomersToSelectPageSize");
                }
            }
				
   			public static string AllowCustomersToSelectPageSizeColumn{
			      get{
        			return "AllowCustomersToSelectPageSize";
      			}
		    }
            
            public IColumn PageSizeOptions{
                get{
                    return this.GetColumn("PageSizeOptions");
                }
            }
				
   			public static string PageSizeOptionsColumn{
			      get{
        			return "PageSizeOptions";
      			}
		    }
            
            public IColumn PriceRanges{
                get{
                    return this.GetColumn("PriceRanges");
                }
            }
				
   			public static string PriceRangesColumn{
			      get{
        			return "PriceRanges";
      			}
		    }
            
            public IColumn SubjectToAcl{
                get{
                    return this.GetColumn("SubjectToAcl");
                }
            }
				
   			public static string SubjectToAclColumn{
			      get{
        			return "SubjectToAcl";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn Deleted{
                get{
                    return this.GetColumn("Deleted");
                }
            }
				
   			public static string DeletedColumn{
			      get{
        			return "Deleted";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UpdatedOnUtc{
                get{
                    return this.GetColumn("UpdatedOnUtc");
                }
            }
				
   			public static string UpdatedOnUtcColumn{
			      get{
        			return "UpdatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ProductWarehouseInventory
        /// Primary Key: Id
        /// </summary>

        public class ProductWarehouseInventoryTable: DatabaseTable {
            
            public ProductWarehouseInventoryTable(IDataProvider provider):base("ProductWarehouseInventory",provider){
                ClassName = "ProductWarehouseInventory";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WarehouseId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StockQuantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ReservedQuantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn WarehouseId{
                get{
                    return this.GetColumn("WarehouseId");
                }
            }
				
   			public static string WarehouseIdColumn{
			      get{
        			return "WarehouseId";
      			}
		    }
            
            public IColumn StockQuantity{
                get{
                    return this.GetColumn("StockQuantity");
                }
            }
				
   			public static string StockQuantityColumn{
			      get{
        			return "StockQuantity";
      			}
		    }
            
            public IColumn ReservedQuantity{
                get{
                    return this.GetColumn("ReservedQuantity");
                }
            }
				
   			public static string ReservedQuantityColumn{
			      get{
        			return "ReservedQuantity";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Country
        /// Primary Key: Id
        /// </summary>

        public class CountryTable: DatabaseTable {
            
            public CountryTable(IDataProvider provider):base("Country",provider){
                ClassName = "Country";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("AllowsBilling", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AllowsShipping", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TwoLetterIsoCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 2
                });

                Columns.Add(new DatabaseColumn("ThreeLetterIsoCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 3
                });

                Columns.Add(new DatabaseColumn("NumericIsoCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SubjectToVat", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn AllowsBilling{
                get{
                    return this.GetColumn("AllowsBilling");
                }
            }
				
   			public static string AllowsBillingColumn{
			      get{
        			return "AllowsBilling";
      			}
		    }
            
            public IColumn AllowsShipping{
                get{
                    return this.GetColumn("AllowsShipping");
                }
            }
				
   			public static string AllowsShippingColumn{
			      get{
        			return "AllowsShipping";
      			}
		    }
            
            public IColumn TwoLetterIsoCode{
                get{
                    return this.GetColumn("TwoLetterIsoCode");
                }
            }
				
   			public static string TwoLetterIsoCodeColumn{
			      get{
        			return "TwoLetterIsoCode";
      			}
		    }
            
            public IColumn ThreeLetterIsoCode{
                get{
                    return this.GetColumn("ThreeLetterIsoCode");
                }
            }
				
   			public static string ThreeLetterIsoCodeColumn{
			      get{
        			return "ThreeLetterIsoCode";
      			}
		    }
            
            public IColumn NumericIsoCode{
                get{
                    return this.GetColumn("NumericIsoCode");
                }
            }
				
   			public static string NumericIsoCodeColumn{
			      get{
        			return "NumericIsoCode";
      			}
		    }
            
            public IColumn SubjectToVat{
                get{
                    return this.GetColumn("SubjectToVat");
                }
            }
				
   			public static string SubjectToVatColumn{
			      get{
        			return "SubjectToVat";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ManufacturerTemplate
        /// Primary Key: Id
        /// </summary>

        public class ManufacturerTemplateTable: DatabaseTable {
            
            public ManufacturerTemplateTable(IDataProvider provider):base("ManufacturerTemplate",provider){
                ClassName = "ManufacturerTemplate";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ViewPath", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn ViewPath{
                get{
                    return this.GetColumn("ViewPath");
                }
            }
				
   			public static string ViewPathColumn{
			      get{
        			return "ViewPath";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: QueuedEmail
        /// Primary Key: Id
        /// </summary>

        public class QueuedEmailTable: DatabaseTable {
            
            public QueuedEmailTable(IDataProvider provider):base("QueuedEmail",provider){
                ClassName = "QueuedEmail";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PriorityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("From", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 500
                });

                Columns.Add(new DatabaseColumn("FromName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 500
                });

                Columns.Add(new DatabaseColumn("To", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 500
                });

                Columns.Add(new DatabaseColumn("ToName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 500
                });

                Columns.Add(new DatabaseColumn("ReplyTo", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 500
                });

                Columns.Add(new DatabaseColumn("ReplyToName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 500
                });

                Columns.Add(new DatabaseColumn("CC", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 500
                });

                Columns.Add(new DatabaseColumn("Bcc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 500
                });

                Columns.Add(new DatabaseColumn("Subject", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("Body", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AttachmentFilePath", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AttachmentFileName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AttachedDownloadId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DontSendBeforeDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SentTries", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SentOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EmailAccountId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn PriorityId{
                get{
                    return this.GetColumn("PriorityId");
                }
            }
				
   			public static string PriorityIdColumn{
			      get{
        			return "PriorityId";
      			}
		    }
            
            public IColumn From{
                get{
                    return this.GetColumn("From");
                }
            }
				
   			public static string FromColumn{
			      get{
        			return "From";
      			}
		    }
            
            public IColumn FromName{
                get{
                    return this.GetColumn("FromName");
                }
            }
				
   			public static string FromNameColumn{
			      get{
        			return "FromName";
      			}
		    }
            
            public IColumn To{
                get{
                    return this.GetColumn("To");
                }
            }
				
   			public static string ToColumn{
			      get{
        			return "To";
      			}
		    }
            
            public IColumn ToName{
                get{
                    return this.GetColumn("ToName");
                }
            }
				
   			public static string ToNameColumn{
			      get{
        			return "ToName";
      			}
		    }
            
            public IColumn ReplyTo{
                get{
                    return this.GetColumn("ReplyTo");
                }
            }
				
   			public static string ReplyToColumn{
			      get{
        			return "ReplyTo";
      			}
		    }
            
            public IColumn ReplyToName{
                get{
                    return this.GetColumn("ReplyToName");
                }
            }
				
   			public static string ReplyToNameColumn{
			      get{
        			return "ReplyToName";
      			}
		    }
            
            public IColumn CC{
                get{
                    return this.GetColumn("CC");
                }
            }
				
   			public static string CCColumn{
			      get{
        			return "CC";
      			}
		    }
            
            public IColumn Bcc{
                get{
                    return this.GetColumn("Bcc");
                }
            }
				
   			public static string BccColumn{
			      get{
        			return "Bcc";
      			}
		    }
            
            public IColumn Subject{
                get{
                    return this.GetColumn("Subject");
                }
            }
				
   			public static string SubjectColumn{
			      get{
        			return "Subject";
      			}
		    }
            
            public IColumn Body{
                get{
                    return this.GetColumn("Body");
                }
            }
				
   			public static string BodyColumn{
			      get{
        			return "Body";
      			}
		    }
            
            public IColumn AttachmentFilePath{
                get{
                    return this.GetColumn("AttachmentFilePath");
                }
            }
				
   			public static string AttachmentFilePathColumn{
			      get{
        			return "AttachmentFilePath";
      			}
		    }
            
            public IColumn AttachmentFileName{
                get{
                    return this.GetColumn("AttachmentFileName");
                }
            }
				
   			public static string AttachmentFileNameColumn{
			      get{
        			return "AttachmentFileName";
      			}
		    }
            
            public IColumn AttachedDownloadId{
                get{
                    return this.GetColumn("AttachedDownloadId");
                }
            }
				
   			public static string AttachedDownloadIdColumn{
			      get{
        			return "AttachedDownloadId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn DontSendBeforeDateUtc{
                get{
                    return this.GetColumn("DontSendBeforeDateUtc");
                }
            }
				
   			public static string DontSendBeforeDateUtcColumn{
			      get{
        			return "DontSendBeforeDateUtc";
      			}
		    }
            
            public IColumn SentTries{
                get{
                    return this.GetColumn("SentTries");
                }
            }
				
   			public static string SentTriesColumn{
			      get{
        			return "SentTries";
      			}
		    }
            
            public IColumn SentOnUtc{
                get{
                    return this.GetColumn("SentOnUtc");
                }
            }
				
   			public static string SentOnUtcColumn{
			      get{
        			return "SentOnUtc";
      			}
		    }
            
            public IColumn EmailAccountId{
                get{
                    return this.GetColumn("EmailAccountId");
                }
            }
				
   			public static string EmailAccountIdColumn{
			      get{
        			return "EmailAccountId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: CrossSellProduct
        /// Primary Key: Id
        /// </summary>

        public class CrossSellProductTable: DatabaseTable {
            
            public CrossSellProductTable(IDataProvider provider):base("CrossSellProduct",provider){
                ClassName = "CrossSellProduct";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId1", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId2", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId1{
                get{
                    return this.GetColumn("ProductId1");
                }
            }
				
   			public static string ProductId1Column{
			      get{
        			return "ProductId1";
      			}
		    }
            
            public IColumn ProductId2{
                get{
                    return this.GetColumn("ProductId2");
                }
            }
				
   			public static string ProductId2Column{
			      get{
        			return "ProductId2";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_AS_AnywhereSlider
        /// Primary Key: Id
        /// </summary>

        public class SS_AS_AnywhereSliderTable: DatabaseTable {
            
            public SS_AS_AnywhereSliderTable(IDataProvider provider):base("SS_AS_AnywhereSlider",provider){
                ClassName = "SS_AS_AnywhereSlider";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SystemName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("SliderType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LanguageId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn SystemName{
                get{
                    return this.GetColumn("SystemName");
                }
            }
				
   			public static string SystemNameColumn{
			      get{
        			return "SystemName";
      			}
		    }
            
            public IColumn SliderType{
                get{
                    return this.GetColumn("SliderType");
                }
            }
				
   			public static string SliderTypeColumn{
			      get{
        			return "SliderType";
      			}
		    }
            
            public IColumn LanguageId{
                get{
                    return this.GetColumn("LanguageId");
                }
            }
				
   			public static string LanguageIdColumn{
			      get{
        			return "LanguageId";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: MeasureDimension
        /// Primary Key: Id
        /// </summary>

        public class MeasureDimensionTable: DatabaseTable {
            
            public MeasureDimensionTable(IDataProvider provider):base("MeasureDimension",provider){
                ClassName = "MeasureDimension";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("SystemKeyword", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("Ratio", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn SystemKeyword{
                get{
                    return this.GetColumn("SystemKeyword");
                }
            }
				
   			public static string SystemKeywordColumn{
			      get{
        			return "SystemKeyword";
      			}
		    }
            
            public IColumn Ratio{
                get{
                    return this.GetColumn("Ratio");
                }
            }
				
   			public static string RatioColumn{
			      get{
        			return "Ratio";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: RecurringPayment
        /// Primary Key: Id
        /// </summary>

        public class RecurringPaymentTable: DatabaseTable {
            
            public RecurringPaymentTable(IDataProvider provider):base("RecurringPayment",provider){
                ClassName = "RecurringPayment";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CycleLength", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CyclePeriodId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TotalCycles", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StartDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsActive", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastPaymentFailed", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Deleted", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("InitialOrderId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CycleLength{
                get{
                    return this.GetColumn("CycleLength");
                }
            }
				
   			public static string CycleLengthColumn{
			      get{
        			return "CycleLength";
      			}
		    }
            
            public IColumn CyclePeriodId{
                get{
                    return this.GetColumn("CyclePeriodId");
                }
            }
				
   			public static string CyclePeriodIdColumn{
			      get{
        			return "CyclePeriodId";
      			}
		    }
            
            public IColumn TotalCycles{
                get{
                    return this.GetColumn("TotalCycles");
                }
            }
				
   			public static string TotalCyclesColumn{
			      get{
        			return "TotalCycles";
      			}
		    }
            
            public IColumn StartDateUtc{
                get{
                    return this.GetColumn("StartDateUtc");
                }
            }
				
   			public static string StartDateUtcColumn{
			      get{
        			return "StartDateUtc";
      			}
		    }
            
            public IColumn IsActive{
                get{
                    return this.GetColumn("IsActive");
                }
            }
				
   			public static string IsActiveColumn{
			      get{
        			return "IsActive";
      			}
		    }
            
            public IColumn LastPaymentFailed{
                get{
                    return this.GetColumn("LastPaymentFailed");
                }
            }
				
   			public static string LastPaymentFailedColumn{
			      get{
        			return "LastPaymentFailed";
      			}
		    }
            
            public IColumn Deleted{
                get{
                    return this.GetColumn("Deleted");
                }
            }
				
   			public static string DeletedColumn{
			      get{
        			return "Deleted";
      			}
		    }
            
            public IColumn InitialOrderId{
                get{
                    return this.GetColumn("InitialOrderId");
                }
            }
				
   			public static string InitialOrderIdColumn{
			      get{
        			return "InitialOrderId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Currency
        /// Primary Key: Id
        /// </summary>

        public class CurrencyTable: DatabaseTable {
            
            public CurrencyTable(IDataProvider provider):base("Currency",provider){
                ClassName = "Currency";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 50
                });

                Columns.Add(new DatabaseColumn("CurrencyCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 5
                });

                Columns.Add(new DatabaseColumn("Rate", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayLocale", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 50
                });

                Columns.Add(new DatabaseColumn("CustomFormatting", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 50
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UpdatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RoundingTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn CurrencyCode{
                get{
                    return this.GetColumn("CurrencyCode");
                }
            }
				
   			public static string CurrencyCodeColumn{
			      get{
        			return "CurrencyCode";
      			}
		    }
            
            public IColumn Rate{
                get{
                    return this.GetColumn("Rate");
                }
            }
				
   			public static string RateColumn{
			      get{
        			return "Rate";
      			}
		    }
            
            public IColumn DisplayLocale{
                get{
                    return this.GetColumn("DisplayLocale");
                }
            }
				
   			public static string DisplayLocaleColumn{
			      get{
        			return "DisplayLocale";
      			}
		    }
            
            public IColumn CustomFormatting{
                get{
                    return this.GetColumn("CustomFormatting");
                }
            }
				
   			public static string CustomFormattingColumn{
			      get{
        			return "CustomFormatting";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UpdatedOnUtc{
                get{
                    return this.GetColumn("UpdatedOnUtc");
                }
            }
				
   			public static string UpdatedOnUtcColumn{
			      get{
        			return "UpdatedOnUtc";
      			}
		    }
            
            public IColumn RoundingTypeId{
                get{
                    return this.GetColumn("RoundingTypeId");
                }
            }
				
   			public static string RoundingTypeIdColumn{
			      get{
        			return "RoundingTypeId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_AS_SliderImage
        /// Primary Key: Id
        /// </summary>

        public class SS_AS_SliderImageTable: DatabaseTable {
            
            public SS_AS_SliderImageTable(IDataProvider provider):base("SS_AS_SliderImage",provider){
                ClassName = "SS_AS_SliderImage";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayText", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Url", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Alt", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Visible", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SliderId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn DisplayText{
                get{
                    return this.GetColumn("DisplayText");
                }
            }
				
   			public static string DisplayTextColumn{
			      get{
        			return "DisplayText";
      			}
		    }
            
            public IColumn Url{
                get{
                    return this.GetColumn("Url");
                }
            }
				
   			public static string UrlColumn{
			      get{
        			return "Url";
      			}
		    }
            
            public IColumn Alt{
                get{
                    return this.GetColumn("Alt");
                }
            }
				
   			public static string AltColumn{
			      get{
        			return "Alt";
      			}
		    }
            
            public IColumn Visible{
                get{
                    return this.GetColumn("Visible");
                }
            }
				
   			public static string VisibleColumn{
			      get{
        			return "Visible";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn PictureId{
                get{
                    return this.GetColumn("PictureId");
                }
            }
				
   			public static string PictureIdColumn{
			      get{
        			return "PictureId";
      			}
		    }
            
            public IColumn SliderId{
                get{
                    return this.GetColumn("SliderId");
                }
            }
				
   			public static string SliderIdColumn{
			      get{
        			return "SliderId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: MeasureWeight
        /// Primary Key: Id
        /// </summary>

        public class MeasureWeightTable: DatabaseTable {
            
            public MeasureWeightTable(IDataProvider provider):base("MeasureWeight",provider){
                ClassName = "MeasureWeight";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("SystemKeyword", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("Ratio", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn SystemKeyword{
                get{
                    return this.GetColumn("SystemKeyword");
                }
            }
				
   			public static string SystemKeywordColumn{
			      get{
        			return "SystemKeyword";
      			}
		    }
            
            public IColumn Ratio{
                get{
                    return this.GetColumn("Ratio");
                }
            }
				
   			public static string RatioColumn{
			      get{
        			return "Ratio";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: RecurringPaymentHistory
        /// Primary Key: Id
        /// </summary>

        public class RecurringPaymentHistoryTable: DatabaseTable {
            
            public RecurringPaymentHistoryTable(IDataProvider provider):base("RecurringPaymentHistory",provider){
                ClassName = "RecurringPaymentHistory";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RecurringPaymentId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn RecurringPaymentId{
                get{
                    return this.GetColumn("RecurringPaymentId");
                }
            }
				
   			public static string RecurringPaymentIdColumn{
			      get{
        			return "RecurringPaymentId";
      			}
		    }
            
            public IColumn OrderId{
                get{
                    return this.GetColumn("OrderId");
                }
            }
				
   			public static string OrderIdColumn{
			      get{
        			return "OrderId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Customer
        /// Primary Key: Id
        /// </summary>

        public class CustomerTable: DatabaseTable {
            
            public CustomerTable(IDataProvider provider):base("Customer",provider){
                ClassName = "Customer";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerGuid", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Guid,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Username", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("Email", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("EmailToRevalidate", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("AdminComment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IsTaxExempt", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AffiliateId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("VendorId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("HasShoppingCartItems", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RequireReLogin", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("FailedLoginAttempts", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CannotLoginUntilDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Active", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Deleted", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsSystemAccount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SystemName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("LastIpAddress", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastLoginDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastActivityDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RegisteredInStoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BillingAddress_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShippingAddress_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CustomerGuid{
                get{
                    return this.GetColumn("CustomerGuid");
                }
            }
				
   			public static string CustomerGuidColumn{
			      get{
        			return "CustomerGuid";
      			}
		    }
            
            public IColumn Username{
                get{
                    return this.GetColumn("Username");
                }
            }
				
   			public static string UsernameColumn{
			      get{
        			return "Username";
      			}
		    }
            
            public IColumn Email{
                get{
                    return this.GetColumn("Email");
                }
            }
				
   			public static string EmailColumn{
			      get{
        			return "Email";
      			}
		    }
            
            public IColumn EmailToRevalidate{
                get{
                    return this.GetColumn("EmailToRevalidate");
                }
            }
				
   			public static string EmailToRevalidateColumn{
			      get{
        			return "EmailToRevalidate";
      			}
		    }
            
            public IColumn AdminComment{
                get{
                    return this.GetColumn("AdminComment");
                }
            }
				
   			public static string AdminCommentColumn{
			      get{
        			return "AdminComment";
      			}
		    }
            
            public IColumn IsTaxExempt{
                get{
                    return this.GetColumn("IsTaxExempt");
                }
            }
				
   			public static string IsTaxExemptColumn{
			      get{
        			return "IsTaxExempt";
      			}
		    }
            
            public IColumn AffiliateId{
                get{
                    return this.GetColumn("AffiliateId");
                }
            }
				
   			public static string AffiliateIdColumn{
			      get{
        			return "AffiliateId";
      			}
		    }
            
            public IColumn VendorId{
                get{
                    return this.GetColumn("VendorId");
                }
            }
				
   			public static string VendorIdColumn{
			      get{
        			return "VendorId";
      			}
		    }
            
            public IColumn HasShoppingCartItems{
                get{
                    return this.GetColumn("HasShoppingCartItems");
                }
            }
				
   			public static string HasShoppingCartItemsColumn{
			      get{
        			return "HasShoppingCartItems";
      			}
		    }
            
            public IColumn RequireReLogin{
                get{
                    return this.GetColumn("RequireReLogin");
                }
            }
				
   			public static string RequireReLoginColumn{
			      get{
        			return "RequireReLogin";
      			}
		    }
            
            public IColumn FailedLoginAttempts{
                get{
                    return this.GetColumn("FailedLoginAttempts");
                }
            }
				
   			public static string FailedLoginAttemptsColumn{
			      get{
        			return "FailedLoginAttempts";
      			}
		    }
            
            public IColumn CannotLoginUntilDateUtc{
                get{
                    return this.GetColumn("CannotLoginUntilDateUtc");
                }
            }
				
   			public static string CannotLoginUntilDateUtcColumn{
			      get{
        			return "CannotLoginUntilDateUtc";
      			}
		    }
            
            public IColumn Active{
                get{
                    return this.GetColumn("Active");
                }
            }
				
   			public static string ActiveColumn{
			      get{
        			return "Active";
      			}
		    }
            
            public IColumn Deleted{
                get{
                    return this.GetColumn("Deleted");
                }
            }
				
   			public static string DeletedColumn{
			      get{
        			return "Deleted";
      			}
		    }
            
            public IColumn IsSystemAccount{
                get{
                    return this.GetColumn("IsSystemAccount");
                }
            }
				
   			public static string IsSystemAccountColumn{
			      get{
        			return "IsSystemAccount";
      			}
		    }
            
            public IColumn SystemName{
                get{
                    return this.GetColumn("SystemName");
                }
            }
				
   			public static string SystemNameColumn{
			      get{
        			return "SystemName";
      			}
		    }
            
            public IColumn LastIpAddress{
                get{
                    return this.GetColumn("LastIpAddress");
                }
            }
				
   			public static string LastIpAddressColumn{
			      get{
        			return "LastIpAddress";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn LastLoginDateUtc{
                get{
                    return this.GetColumn("LastLoginDateUtc");
                }
            }
				
   			public static string LastLoginDateUtcColumn{
			      get{
        			return "LastLoginDateUtc";
      			}
		    }
            
            public IColumn LastActivityDateUtc{
                get{
                    return this.GetColumn("LastActivityDateUtc");
                }
            }
				
   			public static string LastActivityDateUtcColumn{
			      get{
        			return "LastActivityDateUtc";
      			}
		    }
            
            public IColumn RegisteredInStoreId{
                get{
                    return this.GetColumn("RegisteredInStoreId");
                }
            }
				
   			public static string RegisteredInStoreIdColumn{
			      get{
        			return "RegisteredInStoreId";
      			}
		    }
            
            public IColumn BillingAddress_Id{
                get{
                    return this.GetColumn("BillingAddress_Id");
                }
            }
				
   			public static string BillingAddress_IdColumn{
			      get{
        			return "BillingAddress_Id";
      			}
		    }
            
            public IColumn ShippingAddress_Id{
                get{
                    return this.GetColumn("ShippingAddress_Id");
                }
            }
				
   			public static string ShippingAddress_IdColumn{
			      get{
        			return "ShippingAddress_Id";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_C_Condition
        /// Primary Key: Id
        /// </summary>

        public class SS_C_ConditionTable: DatabaseTable {
            
            public SS_C_ConditionTable(IDataProvider provider):base("SS_C_Condition",provider){
                ClassName = "SS_C_Condition";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Active", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Active{
                get{
                    return this.GetColumn("Active");
                }
            }
				
   			public static string ActiveColumn{
			      get{
        			return "Active";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: MessageTemplate
        /// Primary Key: Id
        /// </summary>

        public class MessageTemplateTable: DatabaseTable {
            
            public MessageTemplateTable(IDataProvider provider):base("MessageTemplate",provider){
                ClassName = "MessageTemplate";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("BccEmailAddresses", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("Subject", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("Body", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IsActive", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DelayBeforeSend", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DelayPeriodId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AttachedDownloadId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EmailAccountId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn BccEmailAddresses{
                get{
                    return this.GetColumn("BccEmailAddresses");
                }
            }
				
   			public static string BccEmailAddressesColumn{
			      get{
        			return "BccEmailAddresses";
      			}
		    }
            
            public IColumn Subject{
                get{
                    return this.GetColumn("Subject");
                }
            }
				
   			public static string SubjectColumn{
			      get{
        			return "Subject";
      			}
		    }
            
            public IColumn Body{
                get{
                    return this.GetColumn("Body");
                }
            }
				
   			public static string BodyColumn{
			      get{
        			return "Body";
      			}
		    }
            
            public IColumn IsActive{
                get{
                    return this.GetColumn("IsActive");
                }
            }
				
   			public static string IsActiveColumn{
			      get{
        			return "IsActive";
      			}
		    }
            
            public IColumn DelayBeforeSend{
                get{
                    return this.GetColumn("DelayBeforeSend");
                }
            }
				
   			public static string DelayBeforeSendColumn{
			      get{
        			return "DelayBeforeSend";
      			}
		    }
            
            public IColumn DelayPeriodId{
                get{
                    return this.GetColumn("DelayPeriodId");
                }
            }
				
   			public static string DelayPeriodIdColumn{
			      get{
        			return "DelayPeriodId";
      			}
		    }
            
            public IColumn AttachedDownloadId{
                get{
                    return this.GetColumn("AttachedDownloadId");
                }
            }
				
   			public static string AttachedDownloadIdColumn{
			      get{
        			return "AttachedDownloadId";
      			}
		    }
            
            public IColumn EmailAccountId{
                get{
                    return this.GetColumn("EmailAccountId");
                }
            }
				
   			public static string EmailAccountIdColumn{
			      get{
        			return "EmailAccountId";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: RelatedProduct
        /// Primary Key: Id
        /// </summary>

        public class RelatedProductTable: DatabaseTable {
            
            public RelatedProductTable(IDataProvider provider):base("RelatedProduct",provider){
                ClassName = "RelatedProduct";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId1", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId2", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductId1{
                get{
                    return this.GetColumn("ProductId1");
                }
            }
				
   			public static string ProductId1Column{
			      get{
        			return "ProductId1";
      			}
		    }
            
            public IColumn ProductId2{
                get{
                    return this.GetColumn("ProductId2");
                }
            }
				
   			public static string ProductId2Column{
			      get{
        			return "ProductId2";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: CustomerAddresses
        /// Primary Key: Address_Id
        /// </summary>

        public class CustomerAddressesTable: DatabaseTable {
            
            public CustomerAddressesTable(IDataProvider provider):base("CustomerAddresses",provider){
                ClassName = "CustomerAddress";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Customer_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Address_Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Customer_Id{
                get{
                    return this.GetColumn("Customer_Id");
                }
            }
				
   			public static string Customer_IdColumn{
			      get{
        			return "Customer_Id";
      			}
		    }
            
            public IColumn Address_Id{
                get{
                    return this.GetColumn("Address_Id");
                }
            }
				
   			public static string Address_IdColumn{
			      get{
        			return "Address_Id";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_C_ConditionGroup
        /// Primary Key: Id
        /// </summary>

        public class SS_C_ConditionGroupTable: DatabaseTable {
            
            public SS_C_ConditionGroupTable(IDataProvider provider):base("SS_C_ConditionGroup",provider){
                ClassName = "SS_C_ConditionGroup";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ConditionId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ConditionId{
                get{
                    return this.GetColumn("ConditionId");
                }
            }
				
   			public static string ConditionIdColumn{
			      get{
        			return "ConditionId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: NewsComment
        /// Primary Key: Id
        /// </summary>

        public class NewsCommentTable: DatabaseTable {
            
            public NewsCommentTable(IDataProvider provider):base("NewsComment",provider){
                ClassName = "NewsComment";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CommentTitle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CommentText", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("NewsItemId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsApproved", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CommentTitle{
                get{
                    return this.GetColumn("CommentTitle");
                }
            }
				
   			public static string CommentTitleColumn{
			      get{
        			return "CommentTitle";
      			}
		    }
            
            public IColumn CommentText{
                get{
                    return this.GetColumn("CommentText");
                }
            }
				
   			public static string CommentTextColumn{
			      get{
        			return "CommentText";
      			}
		    }
            
            public IColumn NewsItemId{
                get{
                    return this.GetColumn("NewsItemId");
                }
            }
				
   			public static string NewsItemIdColumn{
			      get{
        			return "NewsItemId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn IsApproved{
                get{
                    return this.GetColumn("IsApproved");
                }
            }
				
   			public static string IsApprovedColumn{
			      get{
        			return "IsApproved";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ReturnRequest
        /// Primary Key: Id
        /// </summary>

        public class ReturnRequestTable: DatabaseTable {
            
            public ReturnRequestTable(IDataProvider provider):base("ReturnRequest",provider){
                ClassName = "ReturnRequest";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderItemId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Quantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ReasonForReturn", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("RequestedAction", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CustomerComments", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("UploadedFileId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StaffNotes", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ReturnRequestStatusId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UpdatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CustomNumber{
                get{
                    return this.GetColumn("CustomNumber");
                }
            }
				
   			public static string CustomNumberColumn{
			      get{
        			return "CustomNumber";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn OrderItemId{
                get{
                    return this.GetColumn("OrderItemId");
                }
            }
				
   			public static string OrderItemIdColumn{
			      get{
        			return "OrderItemId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn Quantity{
                get{
                    return this.GetColumn("Quantity");
                }
            }
				
   			public static string QuantityColumn{
			      get{
        			return "Quantity";
      			}
		    }
            
            public IColumn ReasonForReturn{
                get{
                    return this.GetColumn("ReasonForReturn");
                }
            }
				
   			public static string ReasonForReturnColumn{
			      get{
        			return "ReasonForReturn";
      			}
		    }
            
            public IColumn RequestedAction{
                get{
                    return this.GetColumn("RequestedAction");
                }
            }
				
   			public static string RequestedActionColumn{
			      get{
        			return "RequestedAction";
      			}
		    }
            
            public IColumn CustomerComments{
                get{
                    return this.GetColumn("CustomerComments");
                }
            }
				
   			public static string CustomerCommentsColumn{
			      get{
        			return "CustomerComments";
      			}
		    }
            
            public IColumn UploadedFileId{
                get{
                    return this.GetColumn("UploadedFileId");
                }
            }
				
   			public static string UploadedFileIdColumn{
			      get{
        			return "UploadedFileId";
      			}
		    }
            
            public IColumn StaffNotes{
                get{
                    return this.GetColumn("StaffNotes");
                }
            }
				
   			public static string StaffNotesColumn{
			      get{
        			return "StaffNotes";
      			}
		    }
            
            public IColumn ReturnRequestStatusId{
                get{
                    return this.GetColumn("ReturnRequestStatusId");
                }
            }
				
   			public static string ReturnRequestStatusIdColumn{
			      get{
        			return "ReturnRequestStatusId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UpdatedOnUtc{
                get{
                    return this.GetColumn("UpdatedOnUtc");
                }
            }
				
   			public static string UpdatedOnUtcColumn{
			      get{
        			return "UpdatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: CustomerAttribute
        /// Primary Key: Id
        /// </summary>

        public class CustomerAttributeTable: DatabaseTable {
            
            public CustomerAttributeTable(IDataProvider provider):base("CustomerAttribute",provider){
                ClassName = "CustomerAttribute";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("IsRequired", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AttributeControlTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn IsRequired{
                get{
                    return this.GetColumn("IsRequired");
                }
            }
				
   			public static string IsRequiredColumn{
			      get{
        			return "IsRequired";
      			}
		    }
            
            public IColumn AttributeControlTypeId{
                get{
                    return this.GetColumn("AttributeControlTypeId");
                }
            }
				
   			public static string AttributeControlTypeIdColumn{
			      get{
        			return "AttributeControlTypeId";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_C_ConditionStatement
        /// Primary Key: Id
        /// </summary>

        public class SS_C_ConditionStatementTable: DatabaseTable {
            
            public SS_C_ConditionStatementTable(IDataProvider provider):base("SS_C_ConditionStatement",provider){
                ClassName = "SS_C_ConditionStatement";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ConditionType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ConditionProperty", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OperatorType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Value", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ConditionGroupId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ConditionType{
                get{
                    return this.GetColumn("ConditionType");
                }
            }
				
   			public static string ConditionTypeColumn{
			      get{
        			return "ConditionType";
      			}
		    }
            
            public IColumn ConditionProperty{
                get{
                    return this.GetColumn("ConditionProperty");
                }
            }
				
   			public static string ConditionPropertyColumn{
			      get{
        			return "ConditionProperty";
      			}
		    }
            
            public IColumn OperatorType{
                get{
                    return this.GetColumn("OperatorType");
                }
            }
				
   			public static string OperatorTypeColumn{
			      get{
        			return "OperatorType";
      			}
		    }
            
            public IColumn Value{
                get{
                    return this.GetColumn("Value");
                }
            }
				
   			public static string ValueColumn{
			      get{
        			return "Value";
      			}
		    }
            
            public IColumn ConditionGroupId{
                get{
                    return this.GetColumn("ConditionGroupId");
                }
            }
				
   			public static string ConditionGroupIdColumn{
			      get{
        			return "ConditionGroupId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: News
        /// Primary Key: Id
        /// </summary>

        public class NewsTable: DatabaseTable {
            
            public NewsTable(IDataProvider provider):base("News",provider){
                ClassName = "News";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LanguageId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Title", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Short", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Full", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StartDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EndDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AllowComments", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MetaKeywords", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("MetaDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MetaTitle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn LanguageId{
                get{
                    return this.GetColumn("LanguageId");
                }
            }
				
   			public static string LanguageIdColumn{
			      get{
        			return "LanguageId";
      			}
		    }
            
            public IColumn Title{
                get{
                    return this.GetColumn("Title");
                }
            }
				
   			public static string TitleColumn{
			      get{
        			return "Title";
      			}
		    }
            
            public IColumn Short{
                get{
                    return this.GetColumn("Short");
                }
            }
				
   			public static string ShortColumn{
			      get{
        			return "Short";
      			}
		    }
            
            public IColumn Full{
                get{
                    return this.GetColumn("Full");
                }
            }
				
   			public static string FullColumn{
			      get{
        			return "Full";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn StartDateUtc{
                get{
                    return this.GetColumn("StartDateUtc");
                }
            }
				
   			public static string StartDateUtcColumn{
			      get{
        			return "StartDateUtc";
      			}
		    }
            
            public IColumn EndDateUtc{
                get{
                    return this.GetColumn("EndDateUtc");
                }
            }
				
   			public static string EndDateUtcColumn{
			      get{
        			return "EndDateUtc";
      			}
		    }
            
            public IColumn AllowComments{
                get{
                    return this.GetColumn("AllowComments");
                }
            }
				
   			public static string AllowCommentsColumn{
			      get{
        			return "AllowComments";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
            public IColumn MetaKeywords{
                get{
                    return this.GetColumn("MetaKeywords");
                }
            }
				
   			public static string MetaKeywordsColumn{
			      get{
        			return "MetaKeywords";
      			}
		    }
            
            public IColumn MetaDescription{
                get{
                    return this.GetColumn("MetaDescription");
                }
            }
				
   			public static string MetaDescriptionColumn{
			      get{
        			return "MetaDescription";
      			}
		    }
            
            public IColumn MetaTitle{
                get{
                    return this.GetColumn("MetaTitle");
                }
            }
				
   			public static string MetaTitleColumn{
			      get{
        			return "MetaTitle";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ReturnRequestAction
        /// Primary Key: Id
        /// </summary>

        public class ReturnRequestActionTable: DatabaseTable {
            
            public ReturnRequestActionTable(IDataProvider provider):base("ReturnRequestAction",provider){
                ClassName = "ReturnRequestAction";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: CustomerAttributeValue
        /// Primary Key: Id
        /// </summary>

        public class CustomerAttributeValueTable: DatabaseTable {
            
            public CustomerAttributeValueTable(IDataProvider provider):base("CustomerAttributeValue",provider){
                ClassName = "CustomerAttributeValue";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerAttributeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("IsPreSelected", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CustomerAttributeId{
                get{
                    return this.GetColumn("CustomerAttributeId");
                }
            }
				
   			public static string CustomerAttributeIdColumn{
			      get{
        			return "CustomerAttributeId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn IsPreSelected{
                get{
                    return this.GetColumn("IsPreSelected");
                }
            }
				
   			public static string IsPreSelectedColumn{
			      get{
        			return "IsPreSelected";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_C_CustomerOverride
        /// Primary Key: Id
        /// </summary>

        public class SS_C_CustomerOverrideTable: DatabaseTable {
            
            public SS_C_CustomerOverrideTable(IDataProvider provider):base("SS_C_CustomerOverride",provider){
                ClassName = "SS_C_CustomerOverride";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ConditionId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OverrideState", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ConditionId{
                get{
                    return this.GetColumn("ConditionId");
                }
            }
				
   			public static string ConditionIdColumn{
			      get{
        			return "ConditionId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn OverrideState{
                get{
                    return this.GetColumn("OverrideState");
                }
            }
				
   			public static string OverrideStateColumn{
			      get{
        			return "OverrideState";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: NewsLetterSubscription
        /// Primary Key: Id
        /// </summary>

        public class NewsLetterSubscriptionTable: DatabaseTable {
            
            public NewsLetterSubscriptionTable(IDataProvider provider):base("NewsLetterSubscription",provider){
                ClassName = "NewsLetterSubscription";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("NewsLetterSubscriptionGuid", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Guid,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Email", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 255
                });

                Columns.Add(new DatabaseColumn("Active", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn NewsLetterSubscriptionGuid{
                get{
                    return this.GetColumn("NewsLetterSubscriptionGuid");
                }
            }
				
   			public static string NewsLetterSubscriptionGuidColumn{
			      get{
        			return "NewsLetterSubscriptionGuid";
      			}
		    }
            
            public IColumn Email{
                get{
                    return this.GetColumn("Email");
                }
            }
				
   			public static string EmailColumn{
			      get{
        			return "Email";
      			}
		    }
            
            public IColumn Active{
                get{
                    return this.GetColumn("Active");
                }
            }
				
   			public static string ActiveColumn{
			      get{
        			return "Active";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ReturnRequestReason
        /// Primary Key: Id
        /// </summary>

        public class ReturnRequestReasonTable: DatabaseTable {
            
            public ReturnRequestReasonTable(IDataProvider provider):base("ReturnRequestReason",provider){
                ClassName = "ReturnRequestReason";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Customer_CustomerRole_Mapping
        /// Primary Key: Customer_Id
        /// </summary>

        public class Customer_CustomerRole_MappingTable: DatabaseTable {
            
            public Customer_CustomerRole_MappingTable(IDataProvider provider):base("Customer_CustomerRole_Mapping",provider){
                ClassName = "Customer_CustomerRole_Mapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Customer_Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerRole_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Customer_Id{
                get{
                    return this.GetColumn("Customer_Id");
                }
            }
				
   			public static string Customer_IdColumn{
			      get{
        			return "Customer_Id";
      			}
		    }
            
            public IColumn CustomerRole_Id{
                get{
                    return this.GetColumn("CustomerRole_Id");
                }
            }
				
   			public static string CustomerRole_IdColumn{
			      get{
        			return "CustomerRole_Id";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_C_EntityCondition
        /// Primary Key: Id
        /// </summary>

        public class SS_C_EntityConditionTable: DatabaseTable {
            
            public SS_C_EntityConditionTable(IDataProvider provider):base("SS_C_EntityCondition",provider){
                ClassName = "SS_C_EntityCondition";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ConditionId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ConditionId{
                get{
                    return this.GetColumn("ConditionId");
                }
            }
				
   			public static string ConditionIdColumn{
			      get{
        			return "ConditionId";
      			}
		    }
            
            public IColumn EntityType{
                get{
                    return this.GetColumn("EntityType");
                }
            }
				
   			public static string EntityTypeColumn{
			      get{
        			return "EntityType";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Order
        /// Primary Key: Id
        /// </summary>

        public class OrderTable: DatabaseTable {
            
            public OrderTable(IDataProvider provider):base("Order",provider){
                ClassName = "Order";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderGuid", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Guid,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BillingAddressId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShippingAddressId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PickupAddressId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PickUpInStore", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderStatusId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShippingStatusId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PaymentStatusId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PaymentMethodSystemName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CustomerCurrencyCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CurrencyRate", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerTaxDisplayTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("VatNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("OrderSubtotalInclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderSubtotalExclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderSubTotalDiscountInclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderSubTotalDiscountExclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderShippingInclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderShippingExclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PaymentMethodAdditionalFeeInclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PaymentMethodAdditionalFeeExclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TaxRates", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("OrderTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderDiscount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderTotal", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RefundedAmount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RewardPointsHistoryEntryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CheckoutAttributeDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CheckoutAttributesXml", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CustomerLanguageId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AffiliateId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerIp", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AllowStoringCreditCardNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CardType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CardName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CardNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MaskedCreditCardNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CardCvv2", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CardExpirationMonth", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CardExpirationYear", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AuthorizationTransactionId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AuthorizationTransactionCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AuthorizationTransactionResult", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CaptureTransactionId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CaptureTransactionResult", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("SubscriptionTransactionId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("PaidDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShippingMethod", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ShippingRateComputationMethodSystemName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CustomValuesXml", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Deleted", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomOrderNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn OrderGuid{
                get{
                    return this.GetColumn("OrderGuid");
                }
            }
				
   			public static string OrderGuidColumn{
			      get{
        			return "OrderGuid";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn BillingAddressId{
                get{
                    return this.GetColumn("BillingAddressId");
                }
            }
				
   			public static string BillingAddressIdColumn{
			      get{
        			return "BillingAddressId";
      			}
		    }
            
            public IColumn ShippingAddressId{
                get{
                    return this.GetColumn("ShippingAddressId");
                }
            }
				
   			public static string ShippingAddressIdColumn{
			      get{
        			return "ShippingAddressId";
      			}
		    }
            
            public IColumn PickupAddressId{
                get{
                    return this.GetColumn("PickupAddressId");
                }
            }
				
   			public static string PickupAddressIdColumn{
			      get{
        			return "PickupAddressId";
      			}
		    }
            
            public IColumn PickUpInStore{
                get{
                    return this.GetColumn("PickUpInStore");
                }
            }
				
   			public static string PickUpInStoreColumn{
			      get{
        			return "PickUpInStore";
      			}
		    }
            
            public IColumn OrderStatusId{
                get{
                    return this.GetColumn("OrderStatusId");
                }
            }
				
   			public static string OrderStatusIdColumn{
			      get{
        			return "OrderStatusId";
      			}
		    }
            
            public IColumn ShippingStatusId{
                get{
                    return this.GetColumn("ShippingStatusId");
                }
            }
				
   			public static string ShippingStatusIdColumn{
			      get{
        			return "ShippingStatusId";
      			}
		    }
            
            public IColumn PaymentStatusId{
                get{
                    return this.GetColumn("PaymentStatusId");
                }
            }
				
   			public static string PaymentStatusIdColumn{
			      get{
        			return "PaymentStatusId";
      			}
		    }
            
            public IColumn PaymentMethodSystemName{
                get{
                    return this.GetColumn("PaymentMethodSystemName");
                }
            }
				
   			public static string PaymentMethodSystemNameColumn{
			      get{
        			return "PaymentMethodSystemName";
      			}
		    }
            
            public IColumn CustomerCurrencyCode{
                get{
                    return this.GetColumn("CustomerCurrencyCode");
                }
            }
				
   			public static string CustomerCurrencyCodeColumn{
			      get{
        			return "CustomerCurrencyCode";
      			}
		    }
            
            public IColumn CurrencyRate{
                get{
                    return this.GetColumn("CurrencyRate");
                }
            }
				
   			public static string CurrencyRateColumn{
			      get{
        			return "CurrencyRate";
      			}
		    }
            
            public IColumn CustomerTaxDisplayTypeId{
                get{
                    return this.GetColumn("CustomerTaxDisplayTypeId");
                }
            }
				
   			public static string CustomerTaxDisplayTypeIdColumn{
			      get{
        			return "CustomerTaxDisplayTypeId";
      			}
		    }
            
            public IColumn VatNumber{
                get{
                    return this.GetColumn("VatNumber");
                }
            }
				
   			public static string VatNumberColumn{
			      get{
        			return "VatNumber";
      			}
		    }
            
            public IColumn OrderSubtotalInclTax{
                get{
                    return this.GetColumn("OrderSubtotalInclTax");
                }
            }
				
   			public static string OrderSubtotalInclTaxColumn{
			      get{
        			return "OrderSubtotalInclTax";
      			}
		    }
            
            public IColumn OrderSubtotalExclTax{
                get{
                    return this.GetColumn("OrderSubtotalExclTax");
                }
            }
				
   			public static string OrderSubtotalExclTaxColumn{
			      get{
        			return "OrderSubtotalExclTax";
      			}
		    }
            
            public IColumn OrderSubTotalDiscountInclTax{
                get{
                    return this.GetColumn("OrderSubTotalDiscountInclTax");
                }
            }
				
   			public static string OrderSubTotalDiscountInclTaxColumn{
			      get{
        			return "OrderSubTotalDiscountInclTax";
      			}
		    }
            
            public IColumn OrderSubTotalDiscountExclTax{
                get{
                    return this.GetColumn("OrderSubTotalDiscountExclTax");
                }
            }
				
   			public static string OrderSubTotalDiscountExclTaxColumn{
			      get{
        			return "OrderSubTotalDiscountExclTax";
      			}
		    }
            
            public IColumn OrderShippingInclTax{
                get{
                    return this.GetColumn("OrderShippingInclTax");
                }
            }
				
   			public static string OrderShippingInclTaxColumn{
			      get{
        			return "OrderShippingInclTax";
      			}
		    }
            
            public IColumn OrderShippingExclTax{
                get{
                    return this.GetColumn("OrderShippingExclTax");
                }
            }
				
   			public static string OrderShippingExclTaxColumn{
			      get{
        			return "OrderShippingExclTax";
      			}
		    }
            
            public IColumn PaymentMethodAdditionalFeeInclTax{
                get{
                    return this.GetColumn("PaymentMethodAdditionalFeeInclTax");
                }
            }
				
   			public static string PaymentMethodAdditionalFeeInclTaxColumn{
			      get{
        			return "PaymentMethodAdditionalFeeInclTax";
      			}
		    }
            
            public IColumn PaymentMethodAdditionalFeeExclTax{
                get{
                    return this.GetColumn("PaymentMethodAdditionalFeeExclTax");
                }
            }
				
   			public static string PaymentMethodAdditionalFeeExclTaxColumn{
			      get{
        			return "PaymentMethodAdditionalFeeExclTax";
      			}
		    }
            
            public IColumn TaxRates{
                get{
                    return this.GetColumn("TaxRates");
                }
            }
				
   			public static string TaxRatesColumn{
			      get{
        			return "TaxRates";
      			}
		    }
            
            public IColumn OrderTax{
                get{
                    return this.GetColumn("OrderTax");
                }
            }
				
   			public static string OrderTaxColumn{
			      get{
        			return "OrderTax";
      			}
		    }
            
            public IColumn OrderDiscount{
                get{
                    return this.GetColumn("OrderDiscount");
                }
            }
				
   			public static string OrderDiscountColumn{
			      get{
        			return "OrderDiscount";
      			}
		    }
            
            public IColumn OrderTotal{
                get{
                    return this.GetColumn("OrderTotal");
                }
            }
				
   			public static string OrderTotalColumn{
			      get{
        			return "OrderTotal";
      			}
		    }
            
            public IColumn RefundedAmount{
                get{
                    return this.GetColumn("RefundedAmount");
                }
            }
				
   			public static string RefundedAmountColumn{
			      get{
        			return "RefundedAmount";
      			}
		    }
            
            public IColumn RewardPointsHistoryEntryId{
                get{
                    return this.GetColumn("RewardPointsHistoryEntryId");
                }
            }
				
   			public static string RewardPointsHistoryEntryIdColumn{
			      get{
        			return "RewardPointsHistoryEntryId";
      			}
		    }
            
            public IColumn CheckoutAttributeDescription{
                get{
                    return this.GetColumn("CheckoutAttributeDescription");
                }
            }
				
   			public static string CheckoutAttributeDescriptionColumn{
			      get{
        			return "CheckoutAttributeDescription";
      			}
		    }
            
            public IColumn CheckoutAttributesXml{
                get{
                    return this.GetColumn("CheckoutAttributesXml");
                }
            }
				
   			public static string CheckoutAttributesXmlColumn{
			      get{
        			return "CheckoutAttributesXml";
      			}
		    }
            
            public IColumn CustomerLanguageId{
                get{
                    return this.GetColumn("CustomerLanguageId");
                }
            }
				
   			public static string CustomerLanguageIdColumn{
			      get{
        			return "CustomerLanguageId";
      			}
		    }
            
            public IColumn AffiliateId{
                get{
                    return this.GetColumn("AffiliateId");
                }
            }
				
   			public static string AffiliateIdColumn{
			      get{
        			return "AffiliateId";
      			}
		    }
            
            public IColumn CustomerIp{
                get{
                    return this.GetColumn("CustomerIp");
                }
            }
				
   			public static string CustomerIpColumn{
			      get{
        			return "CustomerIp";
      			}
		    }
            
            public IColumn AllowStoringCreditCardNumber{
                get{
                    return this.GetColumn("AllowStoringCreditCardNumber");
                }
            }
				
   			public static string AllowStoringCreditCardNumberColumn{
			      get{
        			return "AllowStoringCreditCardNumber";
      			}
		    }
            
            public IColumn CardType{
                get{
                    return this.GetColumn("CardType");
                }
            }
				
   			public static string CardTypeColumn{
			      get{
        			return "CardType";
      			}
		    }
            
            public IColumn CardName{
                get{
                    return this.GetColumn("CardName");
                }
            }
				
   			public static string CardNameColumn{
			      get{
        			return "CardName";
      			}
		    }
            
            public IColumn CardNumber{
                get{
                    return this.GetColumn("CardNumber");
                }
            }
				
   			public static string CardNumberColumn{
			      get{
        			return "CardNumber";
      			}
		    }
            
            public IColumn MaskedCreditCardNumber{
                get{
                    return this.GetColumn("MaskedCreditCardNumber");
                }
            }
				
   			public static string MaskedCreditCardNumberColumn{
			      get{
        			return "MaskedCreditCardNumber";
      			}
		    }
            
            public IColumn CardCvv2{
                get{
                    return this.GetColumn("CardCvv2");
                }
            }
				
   			public static string CardCvv2Column{
			      get{
        			return "CardCvv2";
      			}
		    }
            
            public IColumn CardExpirationMonth{
                get{
                    return this.GetColumn("CardExpirationMonth");
                }
            }
				
   			public static string CardExpirationMonthColumn{
			      get{
        			return "CardExpirationMonth";
      			}
		    }
            
            public IColumn CardExpirationYear{
                get{
                    return this.GetColumn("CardExpirationYear");
                }
            }
				
   			public static string CardExpirationYearColumn{
			      get{
        			return "CardExpirationYear";
      			}
		    }
            
            public IColumn AuthorizationTransactionId{
                get{
                    return this.GetColumn("AuthorizationTransactionId");
                }
            }
				
   			public static string AuthorizationTransactionIdColumn{
			      get{
        			return "AuthorizationTransactionId";
      			}
		    }
            
            public IColumn AuthorizationTransactionCode{
                get{
                    return this.GetColumn("AuthorizationTransactionCode");
                }
            }
				
   			public static string AuthorizationTransactionCodeColumn{
			      get{
        			return "AuthorizationTransactionCode";
      			}
		    }
            
            public IColumn AuthorizationTransactionResult{
                get{
                    return this.GetColumn("AuthorizationTransactionResult");
                }
            }
				
   			public static string AuthorizationTransactionResultColumn{
			      get{
        			return "AuthorizationTransactionResult";
      			}
		    }
            
            public IColumn CaptureTransactionId{
                get{
                    return this.GetColumn("CaptureTransactionId");
                }
            }
				
   			public static string CaptureTransactionIdColumn{
			      get{
        			return "CaptureTransactionId";
      			}
		    }
            
            public IColumn CaptureTransactionResult{
                get{
                    return this.GetColumn("CaptureTransactionResult");
                }
            }
				
   			public static string CaptureTransactionResultColumn{
			      get{
        			return "CaptureTransactionResult";
      			}
		    }
            
            public IColumn SubscriptionTransactionId{
                get{
                    return this.GetColumn("SubscriptionTransactionId");
                }
            }
				
   			public static string SubscriptionTransactionIdColumn{
			      get{
        			return "SubscriptionTransactionId";
      			}
		    }
            
            public IColumn PaidDateUtc{
                get{
                    return this.GetColumn("PaidDateUtc");
                }
            }
				
   			public static string PaidDateUtcColumn{
			      get{
        			return "PaidDateUtc";
      			}
		    }
            
            public IColumn ShippingMethod{
                get{
                    return this.GetColumn("ShippingMethod");
                }
            }
				
   			public static string ShippingMethodColumn{
			      get{
        			return "ShippingMethod";
      			}
		    }
            
            public IColumn ShippingRateComputationMethodSystemName{
                get{
                    return this.GetColumn("ShippingRateComputationMethodSystemName");
                }
            }
				
   			public static string ShippingRateComputationMethodSystemNameColumn{
			      get{
        			return "ShippingRateComputationMethodSystemName";
      			}
		    }
            
            public IColumn CustomValuesXml{
                get{
                    return this.GetColumn("CustomValuesXml");
                }
            }
				
   			public static string CustomValuesXmlColumn{
			      get{
        			return "CustomValuesXml";
      			}
		    }
            
            public IColumn Deleted{
                get{
                    return this.GetColumn("Deleted");
                }
            }
				
   			public static string DeletedColumn{
			      get{
        			return "Deleted";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn CustomOrderNumber{
                get{
                    return this.GetColumn("CustomOrderNumber");
                }
            }
				
   			public static string CustomOrderNumberColumn{
			      get{
        			return "CustomOrderNumber";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: RewardPointsHistory
        /// Primary Key: Id
        /// </summary>

        public class RewardPointsHistoryTable: DatabaseTable {
            
            public RewardPointsHistoryTable(IDataProvider provider):base("RewardPointsHistory",provider){
                ClassName = "RewardPointsHistory";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Points", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PointsBalance", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UsedAmount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Message", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UsedWithOrder_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn Points{
                get{
                    return this.GetColumn("Points");
                }
            }
				
   			public static string PointsColumn{
			      get{
        			return "Points";
      			}
		    }
            
            public IColumn PointsBalance{
                get{
                    return this.GetColumn("PointsBalance");
                }
            }
				
   			public static string PointsBalanceColumn{
			      get{
        			return "PointsBalance";
      			}
		    }
            
            public IColumn UsedAmount{
                get{
                    return this.GetColumn("UsedAmount");
                }
            }
				
   			public static string UsedAmountColumn{
			      get{
        			return "UsedAmount";
      			}
		    }
            
            public IColumn Message{
                get{
                    return this.GetColumn("Message");
                }
            }
				
   			public static string MessageColumn{
			      get{
        			return "Message";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UsedWithOrder_Id{
                get{
                    return this.GetColumn("UsedWithOrder_Id");
                }
            }
				
   			public static string UsedWithOrder_IdColumn{
			      get{
        			return "UsedWithOrder_Id";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: CustomerPassword
        /// Primary Key: Id
        /// </summary>

        public class CustomerPasswordTable: DatabaseTable {
            
            public CustomerPasswordTable(IDataProvider provider):base("CustomerPassword",provider){
                ClassName = "CustomerPassword";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Password", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("PasswordFormatId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PasswordSalt", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn Password{
                get{
                    return this.GetColumn("Password");
                }
            }
				
   			public static string PasswordColumn{
			      get{
        			return "Password";
      			}
		    }
            
            public IColumn PasswordFormatId{
                get{
                    return this.GetColumn("PasswordFormatId");
                }
            }
				
   			public static string PasswordFormatIdColumn{
			      get{
        			return "PasswordFormatId";
      			}
		    }
            
            public IColumn PasswordSalt{
                get{
                    return this.GetColumn("PasswordSalt");
                }
            }
				
   			public static string PasswordSaltColumn{
			      get{
        			return "PasswordSalt";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_C_ProductOverride
        /// Primary Key: Id
        /// </summary>

        public class SS_C_ProductOverrideTable: DatabaseTable {
            
            public SS_C_ProductOverrideTable(IDataProvider provider):base("SS_C_ProductOverride",provider){
                ClassName = "SS_C_ProductOverride";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ConditionId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductState", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ConditionId{
                get{
                    return this.GetColumn("ConditionId");
                }
            }
				
   			public static string ConditionIdColumn{
			      get{
        			return "ConditionId";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn ProductState{
                get{
                    return this.GetColumn("ProductState");
                }
            }
				
   			public static string ProductStateColumn{
			      get{
        			return "ProductState";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: OrderItem
        /// Primary Key: Id
        /// </summary>

        public class OrderItemTable: DatabaseTable {
            
            public OrderItemTable(IDataProvider provider):base("OrderItem",provider){
                ClassName = "OrderItem";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderItemGuid", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Guid,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Quantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UnitPriceInclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UnitPriceExclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PriceInclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PriceExclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DiscountAmountInclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DiscountAmountExclTax", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OriginalProductCost", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AttributeDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AttributesXml", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("DownloadCount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsDownloadActivated", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LicenseDownloadId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ItemWeight", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RentalStartDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RentalEndDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn OrderItemGuid{
                get{
                    return this.GetColumn("OrderItemGuid");
                }
            }
				
   			public static string OrderItemGuidColumn{
			      get{
        			return "OrderItemGuid";
      			}
		    }
            
            public IColumn OrderId{
                get{
                    return this.GetColumn("OrderId");
                }
            }
				
   			public static string OrderIdColumn{
			      get{
        			return "OrderId";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn Quantity{
                get{
                    return this.GetColumn("Quantity");
                }
            }
				
   			public static string QuantityColumn{
			      get{
        			return "Quantity";
      			}
		    }
            
            public IColumn UnitPriceInclTax{
                get{
                    return this.GetColumn("UnitPriceInclTax");
                }
            }
				
   			public static string UnitPriceInclTaxColumn{
			      get{
        			return "UnitPriceInclTax";
      			}
		    }
            
            public IColumn UnitPriceExclTax{
                get{
                    return this.GetColumn("UnitPriceExclTax");
                }
            }
				
   			public static string UnitPriceExclTaxColumn{
			      get{
        			return "UnitPriceExclTax";
      			}
		    }
            
            public IColumn PriceInclTax{
                get{
                    return this.GetColumn("PriceInclTax");
                }
            }
				
   			public static string PriceInclTaxColumn{
			      get{
        			return "PriceInclTax";
      			}
		    }
            
            public IColumn PriceExclTax{
                get{
                    return this.GetColumn("PriceExclTax");
                }
            }
				
   			public static string PriceExclTaxColumn{
			      get{
        			return "PriceExclTax";
      			}
		    }
            
            public IColumn DiscountAmountInclTax{
                get{
                    return this.GetColumn("DiscountAmountInclTax");
                }
            }
				
   			public static string DiscountAmountInclTaxColumn{
			      get{
        			return "DiscountAmountInclTax";
      			}
		    }
            
            public IColumn DiscountAmountExclTax{
                get{
                    return this.GetColumn("DiscountAmountExclTax");
                }
            }
				
   			public static string DiscountAmountExclTaxColumn{
			      get{
        			return "DiscountAmountExclTax";
      			}
		    }
            
            public IColumn OriginalProductCost{
                get{
                    return this.GetColumn("OriginalProductCost");
                }
            }
				
   			public static string OriginalProductCostColumn{
			      get{
        			return "OriginalProductCost";
      			}
		    }
            
            public IColumn AttributeDescription{
                get{
                    return this.GetColumn("AttributeDescription");
                }
            }
				
   			public static string AttributeDescriptionColumn{
			      get{
        			return "AttributeDescription";
      			}
		    }
            
            public IColumn AttributesXml{
                get{
                    return this.GetColumn("AttributesXml");
                }
            }
				
   			public static string AttributesXmlColumn{
			      get{
        			return "AttributesXml";
      			}
		    }
            
            public IColumn DownloadCount{
                get{
                    return this.GetColumn("DownloadCount");
                }
            }
				
   			public static string DownloadCountColumn{
			      get{
        			return "DownloadCount";
      			}
		    }
            
            public IColumn IsDownloadActivated{
                get{
                    return this.GetColumn("IsDownloadActivated");
                }
            }
				
   			public static string IsDownloadActivatedColumn{
			      get{
        			return "IsDownloadActivated";
      			}
		    }
            
            public IColumn LicenseDownloadId{
                get{
                    return this.GetColumn("LicenseDownloadId");
                }
            }
				
   			public static string LicenseDownloadIdColumn{
			      get{
        			return "LicenseDownloadId";
      			}
		    }
            
            public IColumn ItemWeight{
                get{
                    return this.GetColumn("ItemWeight");
                }
            }
				
   			public static string ItemWeightColumn{
			      get{
        			return "ItemWeight";
      			}
		    }
            
            public IColumn RentalStartDateUtc{
                get{
                    return this.GetColumn("RentalStartDateUtc");
                }
            }
				
   			public static string RentalStartDateUtcColumn{
			      get{
        			return "RentalStartDateUtc";
      			}
		    }
            
            public IColumn RentalEndDateUtc{
                get{
                    return this.GetColumn("RentalEndDateUtc");
                }
            }
				
   			public static string RentalEndDateUtcColumn{
			      get{
        			return "RentalEndDateUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ScheduleTask
        /// Primary Key: Id
        /// </summary>

        public class ScheduleTaskTable: DatabaseTable {
            
            public ScheduleTaskTable(IDataProvider provider):base("ScheduleTask",provider){
                ClassName = "ScheduleTask";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Seconds", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Type", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Enabled", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StopOnError", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LeasedByMachineName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("LeasedUntilUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastStartUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastEndUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LastSuccessUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Seconds{
                get{
                    return this.GetColumn("Seconds");
                }
            }
				
   			public static string SecondsColumn{
			      get{
        			return "Seconds";
      			}
		    }
            
            public IColumn Type{
                get{
                    return this.GetColumn("Type");
                }
            }
				
   			public static string TypeColumn{
			      get{
        			return "Type";
      			}
		    }
            
            public IColumn Enabled{
                get{
                    return this.GetColumn("Enabled");
                }
            }
				
   			public static string EnabledColumn{
			      get{
        			return "Enabled";
      			}
		    }
            
            public IColumn StopOnError{
                get{
                    return this.GetColumn("StopOnError");
                }
            }
				
   			public static string StopOnErrorColumn{
			      get{
        			return "StopOnError";
      			}
		    }
            
            public IColumn LeasedByMachineName{
                get{
                    return this.GetColumn("LeasedByMachineName");
                }
            }
				
   			public static string LeasedByMachineNameColumn{
			      get{
        			return "LeasedByMachineName";
      			}
		    }
            
            public IColumn LeasedUntilUtc{
                get{
                    return this.GetColumn("LeasedUntilUtc");
                }
            }
				
   			public static string LeasedUntilUtcColumn{
			      get{
        			return "LeasedUntilUtc";
      			}
		    }
            
            public IColumn LastStartUtc{
                get{
                    return this.GetColumn("LastStartUtc");
                }
            }
				
   			public static string LastStartUtcColumn{
			      get{
        			return "LastStartUtc";
      			}
		    }
            
            public IColumn LastEndUtc{
                get{
                    return this.GetColumn("LastEndUtc");
                }
            }
				
   			public static string LastEndUtcColumn{
			      get{
        			return "LastEndUtc";
      			}
		    }
            
            public IColumn LastSuccessUtc{
                get{
                    return this.GetColumn("LastSuccessUtc");
                }
            }
				
   			public static string LastSuccessUtcColumn{
			      get{
        			return "LastSuccessUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: CustomerRole
        /// Primary Key: Id
        /// </summary>

        public class CustomerRoleTable: DatabaseTable {
            
            public CustomerRoleTable(IDataProvider provider):base("CustomerRole",provider){
                ClassName = "CustomerRole";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 255
                });

                Columns.Add(new DatabaseColumn("FreeShipping", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TaxExempt", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Active", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsSystemRole", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SystemName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 255
                });

                Columns.Add(new DatabaseColumn("EnablePasswordLifetime", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PurchasedWithProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn FreeShipping{
                get{
                    return this.GetColumn("FreeShipping");
                }
            }
				
   			public static string FreeShippingColumn{
			      get{
        			return "FreeShipping";
      			}
		    }
            
            public IColumn TaxExempt{
                get{
                    return this.GetColumn("TaxExempt");
                }
            }
				
   			public static string TaxExemptColumn{
			      get{
        			return "TaxExempt";
      			}
		    }
            
            public IColumn Active{
                get{
                    return this.GetColumn("Active");
                }
            }
				
   			public static string ActiveColumn{
			      get{
        			return "Active";
      			}
		    }
            
            public IColumn IsSystemRole{
                get{
                    return this.GetColumn("IsSystemRole");
                }
            }
				
   			public static string IsSystemRoleColumn{
			      get{
        			return "IsSystemRole";
      			}
		    }
            
            public IColumn SystemName{
                get{
                    return this.GetColumn("SystemName");
                }
            }
				
   			public static string SystemNameColumn{
			      get{
        			return "SystemName";
      			}
		    }
            
            public IColumn EnablePasswordLifetime{
                get{
                    return this.GetColumn("EnablePasswordLifetime");
                }
            }
				
   			public static string EnablePasswordLifetimeColumn{
			      get{
        			return "EnablePasswordLifetime";
      			}
		    }
            
            public IColumn PurchasedWithProductId{
                get{
                    return this.GetColumn("PurchasedWithProductId");
                }
            }
				
   			public static string PurchasedWithProductIdColumn{
			      get{
        			return "PurchasedWithProductId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: OrderNote
        /// Primary Key: Id
        /// </summary>

        public class OrderNoteTable: DatabaseTable {
            
            public OrderNoteTable(IDataProvider provider):base("OrderNote",provider){
                ClassName = "OrderNote";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Note", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("DownloadId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayToCustomer", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn OrderId{
                get{
                    return this.GetColumn("OrderId");
                }
            }
				
   			public static string OrderIdColumn{
			      get{
        			return "OrderId";
      			}
		    }
            
            public IColumn Note{
                get{
                    return this.GetColumn("Note");
                }
            }
				
   			public static string NoteColumn{
			      get{
        			return "Note";
      			}
		    }
            
            public IColumn DownloadId{
                get{
                    return this.GetColumn("DownloadId");
                }
            }
				
   			public static string DownloadIdColumn{
			      get{
        			return "DownloadId";
      			}
		    }
            
            public IColumn DisplayToCustomer{
                get{
                    return this.GetColumn("DisplayToCustomer");
                }
            }
				
   			public static string DisplayToCustomerColumn{
			      get{
        			return "DisplayToCustomer";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SearchTerm
        /// Primary Key: Id
        /// </summary>

        public class SearchTermTable: DatabaseTable {
            
            public SearchTermTable(IDataProvider provider):base("SearchTerm",provider){
                ClassName = "SearchTerm";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Keyword", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Count", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Keyword{
                get{
                    return this.GetColumn("Keyword");
                }
            }
				
   			public static string KeywordColumn{
			      get{
        			return "Keyword";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn Count{
                get{
                    return this.GetColumn("Count");
                }
            }
				
   			public static string CountColumn{
			      get{
        			return "Count";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: DeliveryDate
        /// Primary Key: Id
        /// </summary>

        public class DeliveryDateTable: DatabaseTable {
            
            public DeliveryDateTable(IDataProvider provider):base("DeliveryDate",provider){
                ClassName = "DeliveryDate";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: PermissionRecord
        /// Primary Key: Id
        /// </summary>

        public class PermissionRecordTable: DatabaseTable {
            
            public PermissionRecordTable(IDataProvider provider):base("PermissionRecord",provider){
                ClassName = "PermissionRecord";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("SystemName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 255
                });

                Columns.Add(new DatabaseColumn("Category", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 255
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn SystemName{
                get{
                    return this.GetColumn("SystemName");
                }
            }
				
   			public static string SystemNameColumn{
			      get{
        			return "SystemName";
      			}
		    }
            
            public IColumn Category{
                get{
                    return this.GetColumn("Category");
                }
            }
				
   			public static string CategoryColumn{
			      get{
        			return "Category";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_MAP_EntityMapping
        /// Primary Key: Id
        /// </summary>

        public class SS_MAP_EntityMappingTable: DatabaseTable {
            
            public SS_MAP_EntityMappingTable(IDataProvider provider):base("SS_MAP_EntityMapping",provider){
                ClassName = "SS_MAP_EntityMapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MappedEntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MappingType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn EntityType{
                get{
                    return this.GetColumn("EntityType");
                }
            }
				
   			public static string EntityTypeColumn{
			      get{
        			return "EntityType";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn MappedEntityId{
                get{
                    return this.GetColumn("MappedEntityId");
                }
            }
				
   			public static string MappedEntityIdColumn{
			      get{
        			return "MappedEntityId";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn MappingType{
                get{
                    return this.GetColumn("MappingType");
                }
            }
				
   			public static string MappingTypeColumn{
			      get{
        			return "MappingType";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Setting
        /// Primary Key: Id
        /// </summary>

        public class SettingTable: DatabaseTable {
            
            public SettingTable(IDataProvider provider):base("Setting",provider){
                ClassName = "Setting";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("Value", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 2000
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Value{
                get{
                    return this.GetColumn("Value");
                }
            }
				
   			public static string ValueColumn{
			      get{
        			return "Value";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Discount
        /// Primary Key: Id
        /// </summary>

        public class DiscountTable: DatabaseTable {
            
            public DiscountTable(IDataProvider provider):base("Discount",provider){
                ClassName = "Discount";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("DiscountTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UsePercentage", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DiscountPercentage", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DiscountAmount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MaximumDiscountAmount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StartDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EndDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RequiresCouponCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CouponCode", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("IsCumulative", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DiscountLimitationId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitationTimes", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MaximumDiscountedQuantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AppliedToSubCategories", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn DiscountTypeId{
                get{
                    return this.GetColumn("DiscountTypeId");
                }
            }
				
   			public static string DiscountTypeIdColumn{
			      get{
        			return "DiscountTypeId";
      			}
		    }
            
            public IColumn UsePercentage{
                get{
                    return this.GetColumn("UsePercentage");
                }
            }
				
   			public static string UsePercentageColumn{
			      get{
        			return "UsePercentage";
      			}
		    }
            
            public IColumn DiscountPercentage{
                get{
                    return this.GetColumn("DiscountPercentage");
                }
            }
				
   			public static string DiscountPercentageColumn{
			      get{
        			return "DiscountPercentage";
      			}
		    }
            
            public IColumn DiscountAmount{
                get{
                    return this.GetColumn("DiscountAmount");
                }
            }
				
   			public static string DiscountAmountColumn{
			      get{
        			return "DiscountAmount";
      			}
		    }
            
            public IColumn MaximumDiscountAmount{
                get{
                    return this.GetColumn("MaximumDiscountAmount");
                }
            }
				
   			public static string MaximumDiscountAmountColumn{
			      get{
        			return "MaximumDiscountAmount";
      			}
		    }
            
            public IColumn StartDateUtc{
                get{
                    return this.GetColumn("StartDateUtc");
                }
            }
				
   			public static string StartDateUtcColumn{
			      get{
        			return "StartDateUtc";
      			}
		    }
            
            public IColumn EndDateUtc{
                get{
                    return this.GetColumn("EndDateUtc");
                }
            }
				
   			public static string EndDateUtcColumn{
			      get{
        			return "EndDateUtc";
      			}
		    }
            
            public IColumn RequiresCouponCode{
                get{
                    return this.GetColumn("RequiresCouponCode");
                }
            }
				
   			public static string RequiresCouponCodeColumn{
			      get{
        			return "RequiresCouponCode";
      			}
		    }
            
            public IColumn CouponCode{
                get{
                    return this.GetColumn("CouponCode");
                }
            }
				
   			public static string CouponCodeColumn{
			      get{
        			return "CouponCode";
      			}
		    }
            
            public IColumn IsCumulative{
                get{
                    return this.GetColumn("IsCumulative");
                }
            }
				
   			public static string IsCumulativeColumn{
			      get{
        			return "IsCumulative";
      			}
		    }
            
            public IColumn DiscountLimitationId{
                get{
                    return this.GetColumn("DiscountLimitationId");
                }
            }
				
   			public static string DiscountLimitationIdColumn{
			      get{
        			return "DiscountLimitationId";
      			}
		    }
            
            public IColumn LimitationTimes{
                get{
                    return this.GetColumn("LimitationTimes");
                }
            }
				
   			public static string LimitationTimesColumn{
			      get{
        			return "LimitationTimes";
      			}
		    }
            
            public IColumn MaximumDiscountedQuantity{
                get{
                    return this.GetColumn("MaximumDiscountedQuantity");
                }
            }
				
   			public static string MaximumDiscountedQuantityColumn{
			      get{
        			return "MaximumDiscountedQuantity";
      			}
		    }
            
            public IColumn AppliedToSubCategories{
                get{
                    return this.GetColumn("AppliedToSubCategories");
                }
            }
				
   			public static string AppliedToSubCategoriesColumn{
			      get{
        			return "AppliedToSubCategories";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: PermissionRecord_Role_Mapping
        /// Primary Key: CustomerRole_Id
        /// </summary>

        public class PermissionRecord_Role_MappingTable: DatabaseTable {
            
            public PermissionRecord_Role_MappingTable(IDataProvider provider):base("PermissionRecord_Role_Mapping",provider){
                ClassName = "PermissionRecord_Role_Mapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("PermissionRecord_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerRole_Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn PermissionRecord_Id{
                get{
                    return this.GetColumn("PermissionRecord_Id");
                }
            }
				
   			public static string PermissionRecord_IdColumn{
			      get{
        			return "PermissionRecord_Id";
      			}
		    }
            
            public IColumn CustomerRole_Id{
                get{
                    return this.GetColumn("CustomerRole_Id");
                }
            }
				
   			public static string CustomerRole_IdColumn{
			      get{
        			return "CustomerRole_Id";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_MAP_EntityWidgetMapping
        /// Primary Key: Id
        /// </summary>

        public class SS_MAP_EntityWidgetMappingTable: DatabaseTable {
            
            public SS_MAP_EntityWidgetMappingTable(IDataProvider provider):base("SS_MAP_EntityWidgetMapping",provider){
                ClassName = "SS_MAP_EntityWidgetMapping";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WidgetZone", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn EntityType{
                get{
                    return this.GetColumn("EntityType");
                }
            }
				
   			public static string EntityTypeColumn{
			      get{
        			return "EntityType";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn WidgetZone{
                get{
                    return this.GetColumn("WidgetZone");
                }
            }
				
   			public static string WidgetZoneColumn{
			      get{
        			return "WidgetZone";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Shipment
        /// Primary Key: Id
        /// </summary>

        public class ShipmentTable: DatabaseTable {
            
            public ShipmentTable(IDataProvider provider):base("Shipment",provider){
                ClassName = "Shipment";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TrackingNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("TotalWeight", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShippedDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DeliveryDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AdminComment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn OrderId{
                get{
                    return this.GetColumn("OrderId");
                }
            }
				
   			public static string OrderIdColumn{
			      get{
        			return "OrderId";
      			}
		    }
            
            public IColumn TrackingNumber{
                get{
                    return this.GetColumn("TrackingNumber");
                }
            }
				
   			public static string TrackingNumberColumn{
			      get{
        			return "TrackingNumber";
      			}
		    }
            
            public IColumn TotalWeight{
                get{
                    return this.GetColumn("TotalWeight");
                }
            }
				
   			public static string TotalWeightColumn{
			      get{
        			return "TotalWeight";
      			}
		    }
            
            public IColumn ShippedDateUtc{
                get{
                    return this.GetColumn("ShippedDateUtc");
                }
            }
				
   			public static string ShippedDateUtcColumn{
			      get{
        			return "ShippedDateUtc";
      			}
		    }
            
            public IColumn DeliveryDateUtc{
                get{
                    return this.GetColumn("DeliveryDateUtc");
                }
            }
				
   			public static string DeliveryDateUtcColumn{
			      get{
        			return "DeliveryDateUtc";
      			}
		    }
            
            public IColumn AdminComment{
                get{
                    return this.GetColumn("AdminComment");
                }
            }
				
   			public static string AdminCommentColumn{
			      get{
        			return "AdminComment";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Discount_AppliedToCategories
        /// Primary Key: Category_Id
        /// </summary>

        public class Discount_AppliedToCategoriesTable: DatabaseTable {
            
            public Discount_AppliedToCategoriesTable(IDataProvider provider):base("Discount_AppliedToCategories",provider){
                ClassName = "Discount_AppliedToCategory";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Discount_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Category_Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Discount_Id{
                get{
                    return this.GetColumn("Discount_Id");
                }
            }
				
   			public static string Discount_IdColumn{
			      get{
        			return "Discount_Id";
      			}
		    }
            
            public IColumn Category_Id{
                get{
                    return this.GetColumn("Category_Id");
                }
            }
				
   			public static string Category_IdColumn{
			      get{
        			return "Category_Id";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Picture
        /// Primary Key: Id
        /// </summary>

        public class PictureTable: DatabaseTable {
            
            public PictureTable(IDataProvider provider):base("Picture",provider){
                ClassName = "Picture";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PictureBinary", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Binary,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MimeType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 40
                });

                Columns.Add(new DatabaseColumn("SeoFilename", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 300
                });

                Columns.Add(new DatabaseColumn("AltAttribute", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("TitleAttribute", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IsNew", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn PictureBinary{
                get{
                    return this.GetColumn("PictureBinary");
                }
            }
				
   			public static string PictureBinaryColumn{
			      get{
        			return "PictureBinary";
      			}
		    }
            
            public IColumn MimeType{
                get{
                    return this.GetColumn("MimeType");
                }
            }
				
   			public static string MimeTypeColumn{
			      get{
        			return "MimeType";
      			}
		    }
            
            public IColumn SeoFilename{
                get{
                    return this.GetColumn("SeoFilename");
                }
            }
				
   			public static string SeoFilenameColumn{
			      get{
        			return "SeoFilename";
      			}
		    }
            
            public IColumn AltAttribute{
                get{
                    return this.GetColumn("AltAttribute");
                }
            }
				
   			public static string AltAttributeColumn{
			      get{
        			return "AltAttribute";
      			}
		    }
            
            public IColumn TitleAttribute{
                get{
                    return this.GetColumn("TitleAttribute");
                }
            }
				
   			public static string TitleAttributeColumn{
			      get{
        			return "TitleAttribute";
      			}
		    }
            
            public IColumn IsNew{
                get{
                    return this.GetColumn("IsNew");
                }
            }
				
   			public static string IsNewColumn{
			      get{
        			return "IsNew";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_S_Schedule
        /// Primary Key: Id
        /// </summary>

        public class SS_S_ScheduleTable: DatabaseTable {
            
            public SS_S_ScheduleTable(IDataProvider provider):base("SS_S_Schedule",provider){
                ClassName = "SS_S_Schedule";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityFromDate", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityToDate", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SchedulePatternType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SchedulePatternFromTime", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.AnsiString,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SchedulePatternToTime", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.AnsiString,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ExactDayValue", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EveryMonthFromDayValue", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EveryMonthToDayValue", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn EntityType{
                get{
                    return this.GetColumn("EntityType");
                }
            }
				
   			public static string EntityTypeColumn{
			      get{
        			return "EntityType";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn EntityFromDate{
                get{
                    return this.GetColumn("EntityFromDate");
                }
            }
				
   			public static string EntityFromDateColumn{
			      get{
        			return "EntityFromDate";
      			}
		    }
            
            public IColumn EntityToDate{
                get{
                    return this.GetColumn("EntityToDate");
                }
            }
				
   			public static string EntityToDateColumn{
			      get{
        			return "EntityToDate";
      			}
		    }
            
            public IColumn SchedulePatternType{
                get{
                    return this.GetColumn("SchedulePatternType");
                }
            }
				
   			public static string SchedulePatternTypeColumn{
			      get{
        			return "SchedulePatternType";
      			}
		    }
            
            public IColumn SchedulePatternFromTime{
                get{
                    return this.GetColumn("SchedulePatternFromTime");
                }
            }
				
   			public static string SchedulePatternFromTimeColumn{
			      get{
        			return "SchedulePatternFromTime";
      			}
		    }
            
            public IColumn SchedulePatternToTime{
                get{
                    return this.GetColumn("SchedulePatternToTime");
                }
            }
				
   			public static string SchedulePatternToTimeColumn{
			      get{
        			return "SchedulePatternToTime";
      			}
		    }
            
            public IColumn ExactDayValue{
                get{
                    return this.GetColumn("ExactDayValue");
                }
            }
				
   			public static string ExactDayValueColumn{
			      get{
        			return "ExactDayValue";
      			}
		    }
            
            public IColumn EveryMonthFromDayValue{
                get{
                    return this.GetColumn("EveryMonthFromDayValue");
                }
            }
				
   			public static string EveryMonthFromDayValueColumn{
			      get{
        			return "EveryMonthFromDayValue";
      			}
		    }
            
            public IColumn EveryMonthToDayValue{
                get{
                    return this.GetColumn("EveryMonthToDayValue");
                }
            }
				
   			public static string EveryMonthToDayValueColumn{
			      get{
        			return "EveryMonthToDayValue";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ShipmentItem
        /// Primary Key: Id
        /// </summary>

        public class ShipmentItemTable: DatabaseTable {
            
            public ShipmentItemTable(IDataProvider provider):base("ShipmentItem",provider){
                ClassName = "ShipmentItem";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShipmentId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderItemId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Quantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WarehouseId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ShipmentId{
                get{
                    return this.GetColumn("ShipmentId");
                }
            }
				
   			public static string ShipmentIdColumn{
			      get{
        			return "ShipmentId";
      			}
		    }
            
            public IColumn OrderItemId{
                get{
                    return this.GetColumn("OrderItemId");
                }
            }
				
   			public static string OrderItemIdColumn{
			      get{
        			return "OrderItemId";
      			}
		    }
            
            public IColumn Quantity{
                get{
                    return this.GetColumn("Quantity");
                }
            }
				
   			public static string QuantityColumn{
			      get{
        			return "Quantity";
      			}
		    }
            
            public IColumn WarehouseId{
                get{
                    return this.GetColumn("WarehouseId");
                }
            }
				
   			public static string WarehouseIdColumn{
			      get{
        			return "WarehouseId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Discount_AppliedToManufacturers
        /// Primary Key: Discount_Id
        /// </summary>

        public class Discount_AppliedToManufacturersTable: DatabaseTable {
            
            public Discount_AppliedToManufacturersTable(IDataProvider provider):base("Discount_AppliedToManufacturers",provider){
                ClassName = "Discount_AppliedToManufacturer";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Discount_Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Manufacturer_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Discount_Id{
                get{
                    return this.GetColumn("Discount_Id");
                }
            }
				
   			public static string Discount_IdColumn{
			      get{
        			return "Discount_Id";
      			}
		    }
            
            public IColumn Manufacturer_Id{
                get{
                    return this.GetColumn("Manufacturer_Id");
                }
            }
				
   			public static string Manufacturer_IdColumn{
			      get{
        			return "Manufacturer_Id";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Poll
        /// Primary Key: Id
        /// </summary>

        public class PollTable: DatabaseTable {
            
            public PollTable(IDataProvider provider):base("Poll",provider){
                ClassName = "Poll";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LanguageId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("SystemKeyword", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShowOnHomePage", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AllowGuestsToVote", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StartDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EndDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn LanguageId{
                get{
                    return this.GetColumn("LanguageId");
                }
            }
				
   			public static string LanguageIdColumn{
			      get{
        			return "LanguageId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn SystemKeyword{
                get{
                    return this.GetColumn("SystemKeyword");
                }
            }
				
   			public static string SystemKeywordColumn{
			      get{
        			return "SystemKeyword";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn ShowOnHomePage{
                get{
                    return this.GetColumn("ShowOnHomePage");
                }
            }
				
   			public static string ShowOnHomePageColumn{
			      get{
        			return "ShowOnHomePage";
      			}
		    }
            
            public IColumn AllowGuestsToVote{
                get{
                    return this.GetColumn("AllowGuestsToVote");
                }
            }
				
   			public static string AllowGuestsToVoteColumn{
			      get{
        			return "AllowGuestsToVote";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn StartDateUtc{
                get{
                    return this.GetColumn("StartDateUtc");
                }
            }
				
   			public static string StartDateUtcColumn{
			      get{
        			return "StartDateUtc";
      			}
		    }
            
            public IColumn EndDateUtc{
                get{
                    return this.GetColumn("EndDateUtc");
                }
            }
				
   			public static string EndDateUtcColumn{
			      get{
        			return "EndDateUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_ES_EntitySetting
        /// Primary Key: Id
        /// </summary>

        public class SS_ES_EntitySettingTable: DatabaseTable {
            
            public SS_ES_EntitySettingTable(IDataProvider provider):base("SS_ES_EntitySetting",provider){
                ClassName = "SS_ES_EntitySetting";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Key", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Value", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn EntityType{
                get{
                    return this.GetColumn("EntityType");
                }
            }
				
   			public static string EntityTypeColumn{
			      get{
        			return "EntityType";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn Key{
                get{
                    return this.GetColumn("Key");
                }
            }
				
   			public static string KeyColumn{
			      get{
        			return "Key";
      			}
		    }
            
            public IColumn Value{
                get{
                    return this.GetColumn("Value");
                }
            }
				
   			public static string ValueColumn{
			      get{
        			return "Value";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ShippingMethod
        /// Primary Key: Id
        /// </summary>

        public class ShippingMethodTable: DatabaseTable {
            
            public ShippingMethodTable(IDataProvider provider):base("ShippingMethod",provider){
                ClassName = "ShippingMethod";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Description", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Description{
                get{
                    return this.GetColumn("Description");
                }
            }
				
   			public static string DescriptionColumn{
			      get{
        			return "Description";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Discount_AppliedToProducts
        /// Primary Key: Discount_Id
        /// </summary>

        public class Discount_AppliedToProductsTable: DatabaseTable {
            
            public Discount_AppliedToProductsTable(IDataProvider provider):base("Discount_AppliedToProducts",provider){
                ClassName = "Discount_AppliedToProduct";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Discount_Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Product_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Discount_Id{
                get{
                    return this.GetColumn("Discount_Id");
                }
            }
				
   			public static string Discount_IdColumn{
			      get{
        			return "Discount_Id";
      			}
		    }
            
            public IColumn Product_Id{
                get{
                    return this.GetColumn("Product_Id");
                }
            }
				
   			public static string Product_IdColumn{
			      get{
        			return "Product_Id";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: PollAnswer
        /// Primary Key: Id
        /// </summary>

        public class PollAnswerTable: DatabaseTable {
            
            public PollAnswerTable(IDataProvider provider):base("PollAnswer",provider){
                ClassName = "PollAnswer";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PollId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("NumberOfVotes", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn PollId{
                get{
                    return this.GetColumn("PollId");
                }
            }
				
   			public static string PollIdColumn{
			      get{
        			return "PollId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn NumberOfVotes{
                get{
                    return this.GetColumn("NumberOfVotes");
                }
            }
				
   			public static string NumberOfVotesColumn{
			      get{
        			return "NumberOfVotes";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_JC_JCarousel
        /// Primary Key: Id
        /// </summary>

        public class SS_JC_JCarouselTable: DatabaseTable {
            
            public SS_JC_JCarouselTable(IDataProvider provider):base("SS_JC_JCarousel",provider){
                ClassName = "SS_JC_JCarousel";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 200
                });

                Columns.Add(new DatabaseColumn("Title", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 250
                });

                Columns.Add(new DatabaseColumn("DataSourceType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("DataSourceEntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CarouselType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Title{
                get{
                    return this.GetColumn("Title");
                }
            }
				
   			public static string TitleColumn{
			      get{
        			return "Title";
      			}
		    }
            
            public IColumn DataSourceType{
                get{
                    return this.GetColumn("DataSourceType");
                }
            }
				
   			public static string DataSourceTypeColumn{
			      get{
        			return "DataSourceType";
      			}
		    }
            
            public IColumn DataSourceEntityId{
                get{
                    return this.GetColumn("DataSourceEntityId");
                }
            }
				
   			public static string DataSourceEntityIdColumn{
			      get{
        			return "DataSourceEntityId";
      			}
		    }
            
            public IColumn CarouselType{
                get{
                    return this.GetColumn("CarouselType");
                }
            }
				
   			public static string CarouselTypeColumn{
			      get{
        			return "CarouselType";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ShippingMethodRestrictions
        /// Primary Key: Country_Id
        /// </summary>

        public class ShippingMethodRestrictionsTable: DatabaseTable {
            
            public ShippingMethodRestrictionsTable(IDataProvider provider):base("ShippingMethodRestrictions",provider){
                ClassName = "ShippingMethodRestriction";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("ShippingMethod_Id", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Country_Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn ShippingMethod_Id{
                get{
                    return this.GetColumn("ShippingMethod_Id");
                }
            }
				
   			public static string ShippingMethod_IdColumn{
			      get{
        			return "ShippingMethod_Id";
      			}
		    }
            
            public IColumn Country_Id{
                get{
                    return this.GetColumn("Country_Id");
                }
            }
				
   			public static string Country_IdColumn{
			      get{
        			return "Country_Id";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: DiscountRequirement
        /// Primary Key: Id
        /// </summary>

        public class DiscountRequirementTable: DatabaseTable {
            
            public DiscountRequirementTable(IDataProvider provider):base("DiscountRequirement",provider){
                ClassName = "DiscountRequirement";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DiscountId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DiscountRequirementRuleSystemName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ParentId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("InteractionTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsGroup", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn DiscountId{
                get{
                    return this.GetColumn("DiscountId");
                }
            }
				
   			public static string DiscountIdColumn{
			      get{
        			return "DiscountId";
      			}
		    }
            
            public IColumn DiscountRequirementRuleSystemName{
                get{
                    return this.GetColumn("DiscountRequirementRuleSystemName");
                }
            }
				
   			public static string DiscountRequirementRuleSystemNameColumn{
			      get{
        			return "DiscountRequirementRuleSystemName";
      			}
		    }
            
            public IColumn ParentId{
                get{
                    return this.GetColumn("ParentId");
                }
            }
				
   			public static string ParentIdColumn{
			      get{
        			return "ParentId";
      			}
		    }
            
            public IColumn InteractionTypeId{
                get{
                    return this.GetColumn("InteractionTypeId");
                }
            }
				
   			public static string InteractionTypeIdColumn{
			      get{
        			return "InteractionTypeId";
      			}
		    }
            
            public IColumn IsGroup{
                get{
                    return this.GetColumn("IsGroup");
                }
            }
				
   			public static string IsGroupColumn{
			      get{
        			return "IsGroup";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: PollVotingRecord
        /// Primary Key: Id
        /// </summary>

        public class PollVotingRecordTable: DatabaseTable {
            
            public PollVotingRecordTable(IDataProvider provider):base("PollVotingRecord",provider){
                ClassName = "PollVotingRecord";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PollAnswerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn PollAnswerId{
                get{
                    return this.GetColumn("PollAnswerId");
                }
            }
				
   			public static string PollAnswerIdColumn{
			      get{
        			return "PollAnswerId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_MM_Menu
        /// Primary Key: Id
        /// </summary>

        public class SS_MM_MenuTable: DatabaseTable {
            
            public SS_MM_MenuTable(IDataProvider provider):base("SS_MM_Menu",provider){
                ClassName = "SS_MM_Menu";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Enabled", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CssClass", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ShowDropdownsOnClick", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Enabled{
                get{
                    return this.GetColumn("Enabled");
                }
            }
				
   			public static string EnabledColumn{
			      get{
        			return "Enabled";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn CssClass{
                get{
                    return this.GetColumn("CssClass");
                }
            }
				
   			public static string CssClassColumn{
			      get{
        			return "CssClass";
      			}
		    }
            
            public IColumn ShowDropdownsOnClick{
                get{
                    return this.GetColumn("ShowDropdownsOnClick");
                }
            }
				
   			public static string ShowDropdownsOnClickColumn{
			      get{
        			return "ShowDropdownsOnClick";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ShoppingCartItem
        /// Primary Key: Id
        /// </summary>

        public class ShoppingCartItemTable: DatabaseTable {
            
            public ShoppingCartItemTable(IDataProvider provider):base("ShoppingCartItem",provider){
                ClassName = "ShoppingCartItem";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShoppingCartTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AttributesXml", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("CustomerEnteredPrice", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Quantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RentalStartDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RentalEndDateUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UpdatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn ShoppingCartTypeId{
                get{
                    return this.GetColumn("ShoppingCartTypeId");
                }
            }
				
   			public static string ShoppingCartTypeIdColumn{
			      get{
        			return "ShoppingCartTypeId";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn ProductId{
                get{
                    return this.GetColumn("ProductId");
                }
            }
				
   			public static string ProductIdColumn{
			      get{
        			return "ProductId";
      			}
		    }
            
            public IColumn AttributesXml{
                get{
                    return this.GetColumn("AttributesXml");
                }
            }
				
   			public static string AttributesXmlColumn{
			      get{
        			return "AttributesXml";
      			}
		    }
            
            public IColumn CustomerEnteredPrice{
                get{
                    return this.GetColumn("CustomerEnteredPrice");
                }
            }
				
   			public static string CustomerEnteredPriceColumn{
			      get{
        			return "CustomerEnteredPrice";
      			}
		    }
            
            public IColumn Quantity{
                get{
                    return this.GetColumn("Quantity");
                }
            }
				
   			public static string QuantityColumn{
			      get{
        			return "Quantity";
      			}
		    }
            
            public IColumn RentalStartDateUtc{
                get{
                    return this.GetColumn("RentalStartDateUtc");
                }
            }
				
   			public static string RentalStartDateUtcColumn{
			      get{
        			return "RentalStartDateUtc";
      			}
		    }
            
            public IColumn RentalEndDateUtc{
                get{
                    return this.GetColumn("RentalEndDateUtc");
                }
            }
				
   			public static string RentalEndDateUtcColumn{
			      get{
        			return "RentalEndDateUtc";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UpdatedOnUtc{
                get{
                    return this.GetColumn("UpdatedOnUtc");
                }
            }
				
   			public static string UpdatedOnUtcColumn{
			      get{
        			return "UpdatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: DiscountUsageHistory
        /// Primary Key: Id
        /// </summary>

        public class DiscountUsageHistoryTable: DatabaseTable {
            
            public DiscountUsageHistoryTable(IDataProvider provider):base("DiscountUsageHistory",provider){
                ClassName = "DiscountUsageHistory";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DiscountId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn DiscountId{
                get{
                    return this.GetColumn("DiscountId");
                }
            }
				
   			public static string DiscountIdColumn{
			      get{
        			return "DiscountId";
      			}
		    }
            
            public IColumn OrderId{
                get{
                    return this.GetColumn("OrderId");
                }
            }
				
   			public static string OrderIdColumn{
			      get{
        			return "OrderId";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: PredefinedProductAttributeValue
        /// Primary Key: Id
        /// </summary>

        public class PredefinedProductAttributeValueTable: DatabaseTable {
            
            public PredefinedProductAttributeValueTable(IDataProvider provider):base("PredefinedProductAttributeValue",provider){
                ClassName = "PredefinedProductAttributeValue";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductAttributeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("PriceAdjustment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WeightAdjustment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Cost", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsPreSelected", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductAttributeId{
                get{
                    return this.GetColumn("ProductAttributeId");
                }
            }
				
   			public static string ProductAttributeIdColumn{
			      get{
        			return "ProductAttributeId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn PriceAdjustment{
                get{
                    return this.GetColumn("PriceAdjustment");
                }
            }
				
   			public static string PriceAdjustmentColumn{
			      get{
        			return "PriceAdjustment";
      			}
		    }
            
            public IColumn WeightAdjustment{
                get{
                    return this.GetColumn("WeightAdjustment");
                }
            }
				
   			public static string WeightAdjustmentColumn{
			      get{
        			return "WeightAdjustment";
      			}
		    }
            
            public IColumn Cost{
                get{
                    return this.GetColumn("Cost");
                }
            }
				
   			public static string CostColumn{
			      get{
        			return "Cost";
      			}
		    }
            
            public IColumn IsPreSelected{
                get{
                    return this.GetColumn("IsPreSelected");
                }
            }
				
   			public static string IsPreSelectedColumn{
			      get{
        			return "IsPreSelected";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_MM_MenuItem
        /// Primary Key: Id
        /// </summary>

        public class SS_MM_MenuItemTable: DatabaseTable {
            
            public SS_MM_MenuItemTable(IDataProvider provider):base("SS_MM_MenuItem",provider){
                ClassName = "SS_MM_MenuItem";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Type", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Title", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Url", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("OpenInNewWindow", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CssClass", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MaximumNumberOfEntities", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("NumberOfBoxesPerRow", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CatalogTemplate", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ImageSize", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WidgetZone", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Width", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ParentMenuItemId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MenuId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SubjectToAcl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Type{
                get{
                    return this.GetColumn("Type");
                }
            }
				
   			public static string TypeColumn{
			      get{
        			return "Type";
      			}
		    }
            
            public IColumn Title{
                get{
                    return this.GetColumn("Title");
                }
            }
				
   			public static string TitleColumn{
			      get{
        			return "Title";
      			}
		    }
            
            public IColumn Url{
                get{
                    return this.GetColumn("Url");
                }
            }
				
   			public static string UrlColumn{
			      get{
        			return "Url";
      			}
		    }
            
            public IColumn OpenInNewWindow{
                get{
                    return this.GetColumn("OpenInNewWindow");
                }
            }
				
   			public static string OpenInNewWindowColumn{
			      get{
        			return "OpenInNewWindow";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn CssClass{
                get{
                    return this.GetColumn("CssClass");
                }
            }
				
   			public static string CssClassColumn{
			      get{
        			return "CssClass";
      			}
		    }
            
            public IColumn MaximumNumberOfEntities{
                get{
                    return this.GetColumn("MaximumNumberOfEntities");
                }
            }
				
   			public static string MaximumNumberOfEntitiesColumn{
			      get{
        			return "MaximumNumberOfEntities";
      			}
		    }
            
            public IColumn NumberOfBoxesPerRow{
                get{
                    return this.GetColumn("NumberOfBoxesPerRow");
                }
            }
				
   			public static string NumberOfBoxesPerRowColumn{
			      get{
        			return "NumberOfBoxesPerRow";
      			}
		    }
            
            public IColumn CatalogTemplate{
                get{
                    return this.GetColumn("CatalogTemplate");
                }
            }
				
   			public static string CatalogTemplateColumn{
			      get{
        			return "CatalogTemplate";
      			}
		    }
            
            public IColumn ImageSize{
                get{
                    return this.GetColumn("ImageSize");
                }
            }
				
   			public static string ImageSizeColumn{
			      get{
        			return "ImageSize";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn WidgetZone{
                get{
                    return this.GetColumn("WidgetZone");
                }
            }
				
   			public static string WidgetZoneColumn{
			      get{
        			return "WidgetZone";
      			}
		    }
            
            public IColumn Width{
                get{
                    return this.GetColumn("Width");
                }
            }
				
   			public static string WidthColumn{
			      get{
        			return "Width";
      			}
		    }
            
            public IColumn ParentMenuItemId{
                get{
                    return this.GetColumn("ParentMenuItemId");
                }
            }
				
   			public static string ParentMenuItemIdColumn{
			      get{
        			return "ParentMenuItemId";
      			}
		    }
            
            public IColumn MenuId{
                get{
                    return this.GetColumn("MenuId");
                }
            }
				
   			public static string MenuIdColumn{
			      get{
        			return "MenuId";
      			}
		    }
            
            public IColumn SubjectToAcl{
                get{
                    return this.GetColumn("SubjectToAcl");
                }
            }
				
   			public static string SubjectToAclColumn{
			      get{
        			return "SubjectToAcl";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SpecificationAttribute
        /// Primary Key: Id
        /// </summary>

        public class SpecificationAttributeTable: DatabaseTable {
            
            public SpecificationAttributeTable(IDataProvider provider):base("SpecificationAttribute",provider){
                ClassName = "SpecificationAttribute";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Download
        /// Primary Key: Id
        /// </summary>

        public class DownloadTable: DatabaseTable {
            
            public DownloadTable(IDataProvider provider):base("Download",provider){
                ClassName = "Download";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DownloadGuid", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Guid,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UseDownloadUrl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DownloadUrl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("DownloadBinary", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Binary,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ContentType", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Filename", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Extension", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IsNew", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn DownloadGuid{
                get{
                    return this.GetColumn("DownloadGuid");
                }
            }
				
   			public static string DownloadGuidColumn{
			      get{
        			return "DownloadGuid";
      			}
		    }
            
            public IColumn UseDownloadUrl{
                get{
                    return this.GetColumn("UseDownloadUrl");
                }
            }
				
   			public static string UseDownloadUrlColumn{
			      get{
        			return "UseDownloadUrl";
      			}
		    }
            
            public IColumn DownloadUrl{
                get{
                    return this.GetColumn("DownloadUrl");
                }
            }
				
   			public static string DownloadUrlColumn{
			      get{
        			return "DownloadUrl";
      			}
		    }
            
            public IColumn DownloadBinary{
                get{
                    return this.GetColumn("DownloadBinary");
                }
            }
				
   			public static string DownloadBinaryColumn{
			      get{
        			return "DownloadBinary";
      			}
		    }
            
            public IColumn ContentType{
                get{
                    return this.GetColumn("ContentType");
                }
            }
				
   			public static string ContentTypeColumn{
			      get{
        			return "ContentType";
      			}
		    }
            
            public IColumn Filename{
                get{
                    return this.GetColumn("Filename");
                }
            }
				
   			public static string FilenameColumn{
			      get{
        			return "Filename";
      			}
		    }
            
            public IColumn Extension{
                get{
                    return this.GetColumn("Extension");
                }
            }
				
   			public static string ExtensionColumn{
			      get{
        			return "Extension";
      			}
		    }
            
            public IColumn IsNew{
                get{
                    return this.GetColumn("IsNew");
                }
            }
				
   			public static string IsNewColumn{
			      get{
        			return "IsNew";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Forums_PrivateMessage
        /// Primary Key: Id
        /// </summary>

        public class Forums_PrivateMessageTable: DatabaseTable {
            
            public Forums_PrivateMessageTable(IDataProvider provider):base("Forums_PrivateMessage",provider){
                ClassName = "Forums_PrivateMessage";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StoreId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("FromCustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ToCustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Subject", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 450
                });

                Columns.Add(new DatabaseColumn("Text", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IsRead", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsDeletedByAuthor", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsDeletedByRecipient", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn StoreId{
                get{
                    return this.GetColumn("StoreId");
                }
            }
				
   			public static string StoreIdColumn{
			      get{
        			return "StoreId";
      			}
		    }
            
            public IColumn FromCustomerId{
                get{
                    return this.GetColumn("FromCustomerId");
                }
            }
				
   			public static string FromCustomerIdColumn{
			      get{
        			return "FromCustomerId";
      			}
		    }
            
            public IColumn ToCustomerId{
                get{
                    return this.GetColumn("ToCustomerId");
                }
            }
				
   			public static string ToCustomerIdColumn{
			      get{
        			return "ToCustomerId";
      			}
		    }
            
            public IColumn Subject{
                get{
                    return this.GetColumn("Subject");
                }
            }
				
   			public static string SubjectColumn{
			      get{
        			return "Subject";
      			}
		    }
            
            public IColumn Text{
                get{
                    return this.GetColumn("Text");
                }
            }
				
   			public static string TextColumn{
			      get{
        			return "Text";
      			}
		    }
            
            public IColumn IsRead{
                get{
                    return this.GetColumn("IsRead");
                }
            }
				
   			public static string IsReadColumn{
			      get{
        			return "IsRead";
      			}
		    }
            
            public IColumn IsDeletedByAuthor{
                get{
                    return this.GetColumn("IsDeletedByAuthor");
                }
            }
				
   			public static string IsDeletedByAuthorColumn{
			      get{
        			return "IsDeletedByAuthor";
      			}
		    }
            
            public IColumn IsDeletedByRecipient{
                get{
                    return this.GetColumn("IsDeletedByRecipient");
                }
            }
				
   			public static string IsDeletedByRecipientColumn{
			      get{
        			return "IsDeletedByRecipient";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_PR_CategoryPageRibbon
        /// Primary Key: Id
        /// </summary>

        public class SS_PR_CategoryPageRibbonTable: DatabaseTable {
            
            public SS_PR_CategoryPageRibbonTable(IDataProvider provider):base("SS_PR_CategoryPageRibbon",provider){
                ClassName = "SS_PR_CategoryPageRibbon";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductRibbonId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Enabled", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Text", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Position", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("TextStyle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ImageStyle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ContainerStyle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductRibbonId{
                get{
                    return this.GetColumn("ProductRibbonId");
                }
            }
				
   			public static string ProductRibbonIdColumn{
			      get{
        			return "ProductRibbonId";
      			}
		    }
            
            public IColumn PictureId{
                get{
                    return this.GetColumn("PictureId");
                }
            }
				
   			public static string PictureIdColumn{
			      get{
        			return "PictureId";
      			}
		    }
            
            public IColumn Enabled{
                get{
                    return this.GetColumn("Enabled");
                }
            }
				
   			public static string EnabledColumn{
			      get{
        			return "Enabled";
      			}
		    }
            
            public IColumn Text{
                get{
                    return this.GetColumn("Text");
                }
            }
				
   			public static string TextColumn{
			      get{
        			return "Text";
      			}
		    }
            
            public IColumn Position{
                get{
                    return this.GetColumn("Position");
                }
            }
				
   			public static string PositionColumn{
			      get{
        			return "Position";
      			}
		    }
            
            public IColumn TextStyle{
                get{
                    return this.GetColumn("TextStyle");
                }
            }
				
   			public static string TextStyleColumn{
			      get{
        			return "TextStyle";
      			}
		    }
            
            public IColumn ImageStyle{
                get{
                    return this.GetColumn("ImageStyle");
                }
            }
				
   			public static string ImageStyleColumn{
			      get{
        			return "ImageStyle";
      			}
		    }
            
            public IColumn ContainerStyle{
                get{
                    return this.GetColumn("ContainerStyle");
                }
            }
				
   			public static string ContainerStyleColumn{
			      get{
        			return "ContainerStyle";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SpecificationAttributeOption
        /// Primary Key: Id
        /// </summary>

        public class SpecificationAttributeOptionTable: DatabaseTable {
            
            public SpecificationAttributeOptionTable(IDataProvider provider):base("SpecificationAttributeOption",provider){
                ClassName = "SpecificationAttributeOption";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SpecificationAttributeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ColorSquaresRgb", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn SpecificationAttributeId{
                get{
                    return this.GetColumn("SpecificationAttributeId");
                }
            }
				
   			public static string SpecificationAttributeIdColumn{
			      get{
        			return "SpecificationAttributeId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn ColorSquaresRgb{
                get{
                    return this.GetColumn("ColorSquaresRgb");
                }
            }
				
   			public static string ColorSquaresRgbColumn{
			      get{
        			return "ColorSquaresRgb";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: EmailAccount
        /// Primary Key: Id
        /// </summary>

        public class EmailAccountTable: DatabaseTable {
            
            public EmailAccountTable(IDataProvider provider):base("EmailAccount",provider){
                ClassName = "EmailAccount";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Email", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 255
                });

                Columns.Add(new DatabaseColumn("DisplayName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 255
                });

                Columns.Add(new DatabaseColumn("Host", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 255
                });

                Columns.Add(new DatabaseColumn("Port", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Username", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 255
                });

                Columns.Add(new DatabaseColumn("Password", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 255
                });

                Columns.Add(new DatabaseColumn("EnableSsl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UseDefaultCredentials", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn Email{
                get{
                    return this.GetColumn("Email");
                }
            }
				
   			public static string EmailColumn{
			      get{
        			return "Email";
      			}
		    }
            
            public IColumn DisplayName{
                get{
                    return this.GetColumn("DisplayName");
                }
            }
				
   			public static string DisplayNameColumn{
			      get{
        			return "DisplayName";
      			}
		    }
            
            public IColumn Host{
                get{
                    return this.GetColumn("Host");
                }
            }
				
   			public static string HostColumn{
			      get{
        			return "Host";
      			}
		    }
            
            public IColumn Port{
                get{
                    return this.GetColumn("Port");
                }
            }
				
   			public static string PortColumn{
			      get{
        			return "Port";
      			}
		    }
            
            public IColumn Username{
                get{
                    return this.GetColumn("Username");
                }
            }
				
   			public static string UsernameColumn{
			      get{
        			return "Username";
      			}
		    }
            
            public IColumn Password{
                get{
                    return this.GetColumn("Password");
                }
            }
				
   			public static string PasswordColumn{
			      get{
        			return "Password";
      			}
		    }
            
            public IColumn EnableSsl{
                get{
                    return this.GetColumn("EnableSsl");
                }
            }
				
   			public static string EnableSslColumn{
			      get{
        			return "EnableSsl";
      			}
		    }
            
            public IColumn UseDefaultCredentials{
                get{
                    return this.GetColumn("UseDefaultCredentials");
                }
            }
				
   			public static string UseDefaultCredentialsColumn{
			      get{
        			return "UseDefaultCredentials";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: Product
        /// Primary Key: Id
        /// </summary>

        public class ProductTable: DatabaseTable {
            
            public ProductTable(IDataProvider provider):base("Product",provider){
                ClassName = "Product";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ParentGroupedProductId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("VisibleIndividually", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ShortDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("FullDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("AdminComment", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ProductTemplateId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("VendorId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShowOnHomePage", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MetaKeywords", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("MetaDescription", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("MetaTitle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("AllowCustomerReviews", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ApprovedRatingSum", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("NotApprovedRatingSum", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ApprovedTotalReviews", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("NotApprovedTotalReviews", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SubjectToAcl", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LimitedToStores", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Sku", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("ManufacturerPartNumber", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("Gtin", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("IsGiftCard", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("GiftCardTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OverriddenGiftCardAmount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RequireOtherProducts", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RequiredProductIds", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("AutomaticallyAddRequiredProducts", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsDownload", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DownloadId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UnlimitedDownloads", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MaxNumberOfDownloads", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DownloadExpirationDays", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DownloadActivationTypeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("HasSampleDownload", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("SampleDownloadId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("HasUserAgreement", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UserAgreementText", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("IsRecurring", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RecurringCycleLength", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RecurringCyclePeriodId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RecurringTotalCycles", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsRental", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RentalPriceLength", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("RentalPricePeriodId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsShipEnabled", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsFreeShipping", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ShipSeparately", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AdditionalShippingCharge", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DeliveryDateId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsTaxExempt", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("TaxCategoryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("IsTelecommunicationsOrBroadcastingOrElectronicServices", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ManageInventoryMethodId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductAvailabilityRangeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UseMultipleWarehouses", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("WarehouseId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("StockQuantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayStockAvailability", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayStockQuantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MinStockQuantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("LowStockActivityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("NotifyAdminForQuantityBelow", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BackorderModeId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AllowBackInStockSubscriptions", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderMinimumQuantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OrderMaximumQuantity", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AllowedQuantities", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 1000
                });

                Columns.Add(new DatabaseColumn("AllowAddingOnlyExistingAttributeCombinations", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("NotReturnable", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisableBuyButton", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisableWishlistButton", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AvailableForPreOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PreOrderAvailabilityStartDateTimeUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CallForPrice", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Price", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("OldPrice", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductCost", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerEntersPrice", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MinimumCustomerEnteredPrice", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MaximumCustomerEnteredPrice", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BasepriceEnabled", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BasepriceAmount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BasepriceUnitId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BasepriceBaseAmount", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("BasepriceBaseUnitId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MarkAsNew", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MarkAsNewStartDateTimeUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("MarkAsNewEndDateTimeUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("HasTierPrices", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("HasDiscountsApplied", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Weight", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Length", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Width", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Height", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Decimal,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AvailableStartDateTimeUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("AvailableEndDateTimeUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Deleted", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CreatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("UpdatedOnUtc", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.DateTime,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductTypeId{
                get{
                    return this.GetColumn("ProductTypeId");
                }
            }
				
   			public static string ProductTypeIdColumn{
			      get{
        			return "ProductTypeId";
      			}
		    }
            
            public IColumn ParentGroupedProductId{
                get{
                    return this.GetColumn("ParentGroupedProductId");
                }
            }
				
   			public static string ParentGroupedProductIdColumn{
			      get{
        			return "ParentGroupedProductId";
      			}
		    }
            
            public IColumn VisibleIndividually{
                get{
                    return this.GetColumn("VisibleIndividually");
                }
            }
				
   			public static string VisibleIndividuallyColumn{
			      get{
        			return "VisibleIndividually";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn ShortDescription{
                get{
                    return this.GetColumn("ShortDescription");
                }
            }
				
   			public static string ShortDescriptionColumn{
			      get{
        			return "ShortDescription";
      			}
		    }
            
            public IColumn FullDescription{
                get{
                    return this.GetColumn("FullDescription");
                }
            }
				
   			public static string FullDescriptionColumn{
			      get{
        			return "FullDescription";
      			}
		    }
            
            public IColumn AdminComment{
                get{
                    return this.GetColumn("AdminComment");
                }
            }
				
   			public static string AdminCommentColumn{
			      get{
        			return "AdminComment";
      			}
		    }
            
            public IColumn ProductTemplateId{
                get{
                    return this.GetColumn("ProductTemplateId");
                }
            }
				
   			public static string ProductTemplateIdColumn{
			      get{
        			return "ProductTemplateId";
      			}
		    }
            
            public IColumn VendorId{
                get{
                    return this.GetColumn("VendorId");
                }
            }
				
   			public static string VendorIdColumn{
			      get{
        			return "VendorId";
      			}
		    }
            
            public IColumn ShowOnHomePage{
                get{
                    return this.GetColumn("ShowOnHomePage");
                }
            }
				
   			public static string ShowOnHomePageColumn{
			      get{
        			return "ShowOnHomePage";
      			}
		    }
            
            public IColumn MetaKeywords{
                get{
                    return this.GetColumn("MetaKeywords");
                }
            }
				
   			public static string MetaKeywordsColumn{
			      get{
        			return "MetaKeywords";
      			}
		    }
            
            public IColumn MetaDescription{
                get{
                    return this.GetColumn("MetaDescription");
                }
            }
				
   			public static string MetaDescriptionColumn{
			      get{
        			return "MetaDescription";
      			}
		    }
            
            public IColumn MetaTitle{
                get{
                    return this.GetColumn("MetaTitle");
                }
            }
				
   			public static string MetaTitleColumn{
			      get{
        			return "MetaTitle";
      			}
		    }
            
            public IColumn AllowCustomerReviews{
                get{
                    return this.GetColumn("AllowCustomerReviews");
                }
            }
				
   			public static string AllowCustomerReviewsColumn{
			      get{
        			return "AllowCustomerReviews";
      			}
		    }
            
            public IColumn ApprovedRatingSum{
                get{
                    return this.GetColumn("ApprovedRatingSum");
                }
            }
				
   			public static string ApprovedRatingSumColumn{
			      get{
        			return "ApprovedRatingSum";
      			}
		    }
            
            public IColumn NotApprovedRatingSum{
                get{
                    return this.GetColumn("NotApprovedRatingSum");
                }
            }
				
   			public static string NotApprovedRatingSumColumn{
			      get{
        			return "NotApprovedRatingSum";
      			}
		    }
            
            public IColumn ApprovedTotalReviews{
                get{
                    return this.GetColumn("ApprovedTotalReviews");
                }
            }
				
   			public static string ApprovedTotalReviewsColumn{
			      get{
        			return "ApprovedTotalReviews";
      			}
		    }
            
            public IColumn NotApprovedTotalReviews{
                get{
                    return this.GetColumn("NotApprovedTotalReviews");
                }
            }
				
   			public static string NotApprovedTotalReviewsColumn{
			      get{
        			return "NotApprovedTotalReviews";
      			}
		    }
            
            public IColumn SubjectToAcl{
                get{
                    return this.GetColumn("SubjectToAcl");
                }
            }
				
   			public static string SubjectToAclColumn{
			      get{
        			return "SubjectToAcl";
      			}
		    }
            
            public IColumn LimitedToStores{
                get{
                    return this.GetColumn("LimitedToStores");
                }
            }
				
   			public static string LimitedToStoresColumn{
			      get{
        			return "LimitedToStores";
      			}
		    }
            
            public IColumn Sku{
                get{
                    return this.GetColumn("Sku");
                }
            }
				
   			public static string SkuColumn{
			      get{
        			return "Sku";
      			}
		    }
            
            public IColumn ManufacturerPartNumber{
                get{
                    return this.GetColumn("ManufacturerPartNumber");
                }
            }
				
   			public static string ManufacturerPartNumberColumn{
			      get{
        			return "ManufacturerPartNumber";
      			}
		    }
            
            public IColumn Gtin{
                get{
                    return this.GetColumn("Gtin");
                }
            }
				
   			public static string GtinColumn{
			      get{
        			return "Gtin";
      			}
		    }
            
            public IColumn IsGiftCard{
                get{
                    return this.GetColumn("IsGiftCard");
                }
            }
				
   			public static string IsGiftCardColumn{
			      get{
        			return "IsGiftCard";
      			}
		    }
            
            public IColumn GiftCardTypeId{
                get{
                    return this.GetColumn("GiftCardTypeId");
                }
            }
				
   			public static string GiftCardTypeIdColumn{
			      get{
        			return "GiftCardTypeId";
      			}
		    }
            
            public IColumn OverriddenGiftCardAmount{
                get{
                    return this.GetColumn("OverriddenGiftCardAmount");
                }
            }
				
   			public static string OverriddenGiftCardAmountColumn{
			      get{
        			return "OverriddenGiftCardAmount";
      			}
		    }
            
            public IColumn RequireOtherProducts{
                get{
                    return this.GetColumn("RequireOtherProducts");
                }
            }
				
   			public static string RequireOtherProductsColumn{
			      get{
        			return "RequireOtherProducts";
      			}
		    }
            
            public IColumn RequiredProductIds{
                get{
                    return this.GetColumn("RequiredProductIds");
                }
            }
				
   			public static string RequiredProductIdsColumn{
			      get{
        			return "RequiredProductIds";
      			}
		    }
            
            public IColumn AutomaticallyAddRequiredProducts{
                get{
                    return this.GetColumn("AutomaticallyAddRequiredProducts");
                }
            }
				
   			public static string AutomaticallyAddRequiredProductsColumn{
			      get{
        			return "AutomaticallyAddRequiredProducts";
      			}
		    }
            
            public IColumn IsDownload{
                get{
                    return this.GetColumn("IsDownload");
                }
            }
				
   			public static string IsDownloadColumn{
			      get{
        			return "IsDownload";
      			}
		    }
            
            public IColumn DownloadId{
                get{
                    return this.GetColumn("DownloadId");
                }
            }
				
   			public static string DownloadIdColumn{
			      get{
        			return "DownloadId";
      			}
		    }
            
            public IColumn UnlimitedDownloads{
                get{
                    return this.GetColumn("UnlimitedDownloads");
                }
            }
				
   			public static string UnlimitedDownloadsColumn{
			      get{
        			return "UnlimitedDownloads";
      			}
		    }
            
            public IColumn MaxNumberOfDownloads{
                get{
                    return this.GetColumn("MaxNumberOfDownloads");
                }
            }
				
   			public static string MaxNumberOfDownloadsColumn{
			      get{
        			return "MaxNumberOfDownloads";
      			}
		    }
            
            public IColumn DownloadExpirationDays{
                get{
                    return this.GetColumn("DownloadExpirationDays");
                }
            }
				
   			public static string DownloadExpirationDaysColumn{
			      get{
        			return "DownloadExpirationDays";
      			}
		    }
            
            public IColumn DownloadActivationTypeId{
                get{
                    return this.GetColumn("DownloadActivationTypeId");
                }
            }
				
   			public static string DownloadActivationTypeIdColumn{
			      get{
        			return "DownloadActivationTypeId";
      			}
		    }
            
            public IColumn HasSampleDownload{
                get{
                    return this.GetColumn("HasSampleDownload");
                }
            }
				
   			public static string HasSampleDownloadColumn{
			      get{
        			return "HasSampleDownload";
      			}
		    }
            
            public IColumn SampleDownloadId{
                get{
                    return this.GetColumn("SampleDownloadId");
                }
            }
				
   			public static string SampleDownloadIdColumn{
			      get{
        			return "SampleDownloadId";
      			}
		    }
            
            public IColumn HasUserAgreement{
                get{
                    return this.GetColumn("HasUserAgreement");
                }
            }
				
   			public static string HasUserAgreementColumn{
			      get{
        			return "HasUserAgreement";
      			}
		    }
            
            public IColumn UserAgreementText{
                get{
                    return this.GetColumn("UserAgreementText");
                }
            }
				
   			public static string UserAgreementTextColumn{
			      get{
        			return "UserAgreementText";
      			}
		    }
            
            public IColumn IsRecurring{
                get{
                    return this.GetColumn("IsRecurring");
                }
            }
				
   			public static string IsRecurringColumn{
			      get{
        			return "IsRecurring";
      			}
		    }
            
            public IColumn RecurringCycleLength{
                get{
                    return this.GetColumn("RecurringCycleLength");
                }
            }
				
   			public static string RecurringCycleLengthColumn{
			      get{
        			return "RecurringCycleLength";
      			}
		    }
            
            public IColumn RecurringCyclePeriodId{
                get{
                    return this.GetColumn("RecurringCyclePeriodId");
                }
            }
				
   			public static string RecurringCyclePeriodIdColumn{
			      get{
        			return "RecurringCyclePeriodId";
      			}
		    }
            
            public IColumn RecurringTotalCycles{
                get{
                    return this.GetColumn("RecurringTotalCycles");
                }
            }
				
   			public static string RecurringTotalCyclesColumn{
			      get{
        			return "RecurringTotalCycles";
      			}
		    }
            
            public IColumn IsRental{
                get{
                    return this.GetColumn("IsRental");
                }
            }
				
   			public static string IsRentalColumn{
			      get{
        			return "IsRental";
      			}
		    }
            
            public IColumn RentalPriceLength{
                get{
                    return this.GetColumn("RentalPriceLength");
                }
            }
				
   			public static string RentalPriceLengthColumn{
			      get{
        			return "RentalPriceLength";
      			}
		    }
            
            public IColumn RentalPricePeriodId{
                get{
                    return this.GetColumn("RentalPricePeriodId");
                }
            }
				
   			public static string RentalPricePeriodIdColumn{
			      get{
        			return "RentalPricePeriodId";
      			}
		    }
            
            public IColumn IsShipEnabled{
                get{
                    return this.GetColumn("IsShipEnabled");
                }
            }
				
   			public static string IsShipEnabledColumn{
			      get{
        			return "IsShipEnabled";
      			}
		    }
            
            public IColumn IsFreeShipping{
                get{
                    return this.GetColumn("IsFreeShipping");
                }
            }
				
   			public static string IsFreeShippingColumn{
			      get{
        			return "IsFreeShipping";
      			}
		    }
            
            public IColumn ShipSeparately{
                get{
                    return this.GetColumn("ShipSeparately");
                }
            }
				
   			public static string ShipSeparatelyColumn{
			      get{
        			return "ShipSeparately";
      			}
		    }
            
            public IColumn AdditionalShippingCharge{
                get{
                    return this.GetColumn("AdditionalShippingCharge");
                }
            }
				
   			public static string AdditionalShippingChargeColumn{
			      get{
        			return "AdditionalShippingCharge";
      			}
		    }
            
            public IColumn DeliveryDateId{
                get{
                    return this.GetColumn("DeliveryDateId");
                }
            }
				
   			public static string DeliveryDateIdColumn{
			      get{
        			return "DeliveryDateId";
      			}
		    }
            
            public IColumn IsTaxExempt{
                get{
                    return this.GetColumn("IsTaxExempt");
                }
            }
				
   			public static string IsTaxExemptColumn{
			      get{
        			return "IsTaxExempt";
      			}
		    }
            
            public IColumn TaxCategoryId{
                get{
                    return this.GetColumn("TaxCategoryId");
                }
            }
				
   			public static string TaxCategoryIdColumn{
			      get{
        			return "TaxCategoryId";
      			}
		    }
            
            public IColumn IsTelecommunicationsOrBroadcastingOrElectronicServices{
                get{
                    return this.GetColumn("IsTelecommunicationsOrBroadcastingOrElectronicServices");
                }
            }
				
   			public static string IsTelecommunicationsOrBroadcastingOrElectronicServicesColumn{
			      get{
        			return "IsTelecommunicationsOrBroadcastingOrElectronicServices";
      			}
		    }
            
            public IColumn ManageInventoryMethodId{
                get{
                    return this.GetColumn("ManageInventoryMethodId");
                }
            }
				
   			public static string ManageInventoryMethodIdColumn{
			      get{
        			return "ManageInventoryMethodId";
      			}
		    }
            
            public IColumn ProductAvailabilityRangeId{
                get{
                    return this.GetColumn("ProductAvailabilityRangeId");
                }
            }
				
   			public static string ProductAvailabilityRangeIdColumn{
			      get{
        			return "ProductAvailabilityRangeId";
      			}
		    }
            
            public IColumn UseMultipleWarehouses{
                get{
                    return this.GetColumn("UseMultipleWarehouses");
                }
            }
				
   			public static string UseMultipleWarehousesColumn{
			      get{
        			return "UseMultipleWarehouses";
      			}
		    }
            
            public IColumn WarehouseId{
                get{
                    return this.GetColumn("WarehouseId");
                }
            }
				
   			public static string WarehouseIdColumn{
			      get{
        			return "WarehouseId";
      			}
		    }
            
            public IColumn StockQuantity{
                get{
                    return this.GetColumn("StockQuantity");
                }
            }
				
   			public static string StockQuantityColumn{
			      get{
        			return "StockQuantity";
      			}
		    }
            
            public IColumn DisplayStockAvailability{
                get{
                    return this.GetColumn("DisplayStockAvailability");
                }
            }
				
   			public static string DisplayStockAvailabilityColumn{
			      get{
        			return "DisplayStockAvailability";
      			}
		    }
            
            public IColumn DisplayStockQuantity{
                get{
                    return this.GetColumn("DisplayStockQuantity");
                }
            }
				
   			public static string DisplayStockQuantityColumn{
			      get{
        			return "DisplayStockQuantity";
      			}
		    }
            
            public IColumn MinStockQuantity{
                get{
                    return this.GetColumn("MinStockQuantity");
                }
            }
				
   			public static string MinStockQuantityColumn{
			      get{
        			return "MinStockQuantity";
      			}
		    }
            
            public IColumn LowStockActivityId{
                get{
                    return this.GetColumn("LowStockActivityId");
                }
            }
				
   			public static string LowStockActivityIdColumn{
			      get{
        			return "LowStockActivityId";
      			}
		    }
            
            public IColumn NotifyAdminForQuantityBelow{
                get{
                    return this.GetColumn("NotifyAdminForQuantityBelow");
                }
            }
				
   			public static string NotifyAdminForQuantityBelowColumn{
			      get{
        			return "NotifyAdminForQuantityBelow";
      			}
		    }
            
            public IColumn BackorderModeId{
                get{
                    return this.GetColumn("BackorderModeId");
                }
            }
				
   			public static string BackorderModeIdColumn{
			      get{
        			return "BackorderModeId";
      			}
		    }
            
            public IColumn AllowBackInStockSubscriptions{
                get{
                    return this.GetColumn("AllowBackInStockSubscriptions");
                }
            }
				
   			public static string AllowBackInStockSubscriptionsColumn{
			      get{
        			return "AllowBackInStockSubscriptions";
      			}
		    }
            
            public IColumn OrderMinimumQuantity{
                get{
                    return this.GetColumn("OrderMinimumQuantity");
                }
            }
				
   			public static string OrderMinimumQuantityColumn{
			      get{
        			return "OrderMinimumQuantity";
      			}
		    }
            
            public IColumn OrderMaximumQuantity{
                get{
                    return this.GetColumn("OrderMaximumQuantity");
                }
            }
				
   			public static string OrderMaximumQuantityColumn{
			      get{
        			return "OrderMaximumQuantity";
      			}
		    }
            
            public IColumn AllowedQuantities{
                get{
                    return this.GetColumn("AllowedQuantities");
                }
            }
				
   			public static string AllowedQuantitiesColumn{
			      get{
        			return "AllowedQuantities";
      			}
		    }
            
            public IColumn AllowAddingOnlyExistingAttributeCombinations{
                get{
                    return this.GetColumn("AllowAddingOnlyExistingAttributeCombinations");
                }
            }
				
   			public static string AllowAddingOnlyExistingAttributeCombinationsColumn{
			      get{
        			return "AllowAddingOnlyExistingAttributeCombinations";
      			}
		    }
            
            public IColumn NotReturnable{
                get{
                    return this.GetColumn("NotReturnable");
                }
            }
				
   			public static string NotReturnableColumn{
			      get{
        			return "NotReturnable";
      			}
		    }
            
            public IColumn DisableBuyButton{
                get{
                    return this.GetColumn("DisableBuyButton");
                }
            }
				
   			public static string DisableBuyButtonColumn{
			      get{
        			return "DisableBuyButton";
      			}
		    }
            
            public IColumn DisableWishlistButton{
                get{
                    return this.GetColumn("DisableWishlistButton");
                }
            }
				
   			public static string DisableWishlistButtonColumn{
			      get{
        			return "DisableWishlistButton";
      			}
		    }
            
            public IColumn AvailableForPreOrder{
                get{
                    return this.GetColumn("AvailableForPreOrder");
                }
            }
				
   			public static string AvailableForPreOrderColumn{
			      get{
        			return "AvailableForPreOrder";
      			}
		    }
            
            public IColumn PreOrderAvailabilityStartDateTimeUtc{
                get{
                    return this.GetColumn("PreOrderAvailabilityStartDateTimeUtc");
                }
            }
				
   			public static string PreOrderAvailabilityStartDateTimeUtcColumn{
			      get{
        			return "PreOrderAvailabilityStartDateTimeUtc";
      			}
		    }
            
            public IColumn CallForPrice{
                get{
                    return this.GetColumn("CallForPrice");
                }
            }
				
   			public static string CallForPriceColumn{
			      get{
        			return "CallForPrice";
      			}
		    }
            
            public IColumn Price{
                get{
                    return this.GetColumn("Price");
                }
            }
				
   			public static string PriceColumn{
			      get{
        			return "Price";
      			}
		    }
            
            public IColumn OldPrice{
                get{
                    return this.GetColumn("OldPrice");
                }
            }
				
   			public static string OldPriceColumn{
			      get{
        			return "OldPrice";
      			}
		    }
            
            public IColumn ProductCost{
                get{
                    return this.GetColumn("ProductCost");
                }
            }
				
   			public static string ProductCostColumn{
			      get{
        			return "ProductCost";
      			}
		    }
            
            public IColumn CustomerEntersPrice{
                get{
                    return this.GetColumn("CustomerEntersPrice");
                }
            }
				
   			public static string CustomerEntersPriceColumn{
			      get{
        			return "CustomerEntersPrice";
      			}
		    }
            
            public IColumn MinimumCustomerEnteredPrice{
                get{
                    return this.GetColumn("MinimumCustomerEnteredPrice");
                }
            }
				
   			public static string MinimumCustomerEnteredPriceColumn{
			      get{
        			return "MinimumCustomerEnteredPrice";
      			}
		    }
            
            public IColumn MaximumCustomerEnteredPrice{
                get{
                    return this.GetColumn("MaximumCustomerEnteredPrice");
                }
            }
				
   			public static string MaximumCustomerEnteredPriceColumn{
			      get{
        			return "MaximumCustomerEnteredPrice";
      			}
		    }
            
            public IColumn BasepriceEnabled{
                get{
                    return this.GetColumn("BasepriceEnabled");
                }
            }
				
   			public static string BasepriceEnabledColumn{
			      get{
        			return "BasepriceEnabled";
      			}
		    }
            
            public IColumn BasepriceAmount{
                get{
                    return this.GetColumn("BasepriceAmount");
                }
            }
				
   			public static string BasepriceAmountColumn{
			      get{
        			return "BasepriceAmount";
      			}
		    }
            
            public IColumn BasepriceUnitId{
                get{
                    return this.GetColumn("BasepriceUnitId");
                }
            }
				
   			public static string BasepriceUnitIdColumn{
			      get{
        			return "BasepriceUnitId";
      			}
		    }
            
            public IColumn BasepriceBaseAmount{
                get{
                    return this.GetColumn("BasepriceBaseAmount");
                }
            }
				
   			public static string BasepriceBaseAmountColumn{
			      get{
        			return "BasepriceBaseAmount";
      			}
		    }
            
            public IColumn BasepriceBaseUnitId{
                get{
                    return this.GetColumn("BasepriceBaseUnitId");
                }
            }
				
   			public static string BasepriceBaseUnitIdColumn{
			      get{
        			return "BasepriceBaseUnitId";
      			}
		    }
            
            public IColumn MarkAsNew{
                get{
                    return this.GetColumn("MarkAsNew");
                }
            }
				
   			public static string MarkAsNewColumn{
			      get{
        			return "MarkAsNew";
      			}
		    }
            
            public IColumn MarkAsNewStartDateTimeUtc{
                get{
                    return this.GetColumn("MarkAsNewStartDateTimeUtc");
                }
            }
				
   			public static string MarkAsNewStartDateTimeUtcColumn{
			      get{
        			return "MarkAsNewStartDateTimeUtc";
      			}
		    }
            
            public IColumn MarkAsNewEndDateTimeUtc{
                get{
                    return this.GetColumn("MarkAsNewEndDateTimeUtc");
                }
            }
				
   			public static string MarkAsNewEndDateTimeUtcColumn{
			      get{
        			return "MarkAsNewEndDateTimeUtc";
      			}
		    }
            
            public IColumn HasTierPrices{
                get{
                    return this.GetColumn("HasTierPrices");
                }
            }
				
   			public static string HasTierPricesColumn{
			      get{
        			return "HasTierPrices";
      			}
		    }
            
            public IColumn HasDiscountsApplied{
                get{
                    return this.GetColumn("HasDiscountsApplied");
                }
            }
				
   			public static string HasDiscountsAppliedColumn{
			      get{
        			return "HasDiscountsApplied";
      			}
		    }
            
            public IColumn Weight{
                get{
                    return this.GetColumn("Weight");
                }
            }
				
   			public static string WeightColumn{
			      get{
        			return "Weight";
      			}
		    }
            
            public IColumn Length{
                get{
                    return this.GetColumn("Length");
                }
            }
				
   			public static string LengthColumn{
			      get{
        			return "Length";
      			}
		    }
            
            public IColumn Width{
                get{
                    return this.GetColumn("Width");
                }
            }
				
   			public static string WidthColumn{
			      get{
        			return "Width";
      			}
		    }
            
            public IColumn Height{
                get{
                    return this.GetColumn("Height");
                }
            }
				
   			public static string HeightColumn{
			      get{
        			return "Height";
      			}
		    }
            
            public IColumn AvailableStartDateTimeUtc{
                get{
                    return this.GetColumn("AvailableStartDateTimeUtc");
                }
            }
				
   			public static string AvailableStartDateTimeUtcColumn{
			      get{
        			return "AvailableStartDateTimeUtc";
      			}
		    }
            
            public IColumn AvailableEndDateTimeUtc{
                get{
                    return this.GetColumn("AvailableEndDateTimeUtc");
                }
            }
				
   			public static string AvailableEndDateTimeUtcColumn{
			      get{
        			return "AvailableEndDateTimeUtc";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn Deleted{
                get{
                    return this.GetColumn("Deleted");
                }
            }
				
   			public static string DeletedColumn{
			      get{
        			return "Deleted";
      			}
		    }
            
            public IColumn CreatedOnUtc{
                get{
                    return this.GetColumn("CreatedOnUtc");
                }
            }
				
   			public static string CreatedOnUtcColumn{
			      get{
        			return "CreatedOnUtc";
      			}
		    }
            
            public IColumn UpdatedOnUtc{
                get{
                    return this.GetColumn("UpdatedOnUtc");
                }
            }
				
   			public static string UpdatedOnUtcColumn{
			      get{
        			return "UpdatedOnUtc";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: AclRecord
        /// Primary Key: Id
        /// </summary>

        public class AclRecordTable: DatabaseTable {
            
            public AclRecordTable(IDataProvider provider):base("AclRecord",provider){
                ClassName = "AclRecord";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("EntityName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 400
                });

                Columns.Add(new DatabaseColumn("CustomerRoleId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn EntityId{
                get{
                    return this.GetColumn("EntityId");
                }
            }
				
   			public static string EntityIdColumn{
			      get{
        			return "EntityId";
      			}
		    }
            
            public IColumn EntityName{
                get{
                    return this.GetColumn("EntityName");
                }
            }
				
   			public static string EntityNameColumn{
			      get{
        			return "EntityName";
      			}
		    }
            
            public IColumn CustomerRoleId{
                get{
                    return this.GetColumn("CustomerRoleId");
                }
            }
				
   			public static string CustomerRoleIdColumn{
			      get{
        			return "CustomerRoleId";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: SS_PR_ProductPageRibbon
        /// Primary Key: Id
        /// </summary>

        public class SS_PR_ProductPageRibbonTable: DatabaseTable {
            
            public SS_PR_ProductPageRibbonTable(IDataProvider provider):base("SS_PR_ProductPageRibbon",provider){
                ClassName = "SS_PR_ProductPageRibbon";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("ProductRibbonId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("PictureId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Enabled", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Text", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("Position", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("TextStyle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ImageStyle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ContainerStyle", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn ProductRibbonId{
                get{
                    return this.GetColumn("ProductRibbonId");
                }
            }
				
   			public static string ProductRibbonIdColumn{
			      get{
        			return "ProductRibbonId";
      			}
		    }
            
            public IColumn PictureId{
                get{
                    return this.GetColumn("PictureId");
                }
            }
				
   			public static string PictureIdColumn{
			      get{
        			return "PictureId";
      			}
		    }
            
            public IColumn Enabled{
                get{
                    return this.GetColumn("Enabled");
                }
            }
				
   			public static string EnabledColumn{
			      get{
        			return "Enabled";
      			}
		    }
            
            public IColumn Text{
                get{
                    return this.GetColumn("Text");
                }
            }
				
   			public static string TextColumn{
			      get{
        			return "Text";
      			}
		    }
            
            public IColumn Position{
                get{
                    return this.GetColumn("Position");
                }
            }
				
   			public static string PositionColumn{
			      get{
        			return "Position";
      			}
		    }
            
            public IColumn TextStyle{
                get{
                    return this.GetColumn("TextStyle");
                }
            }
				
   			public static string TextStyleColumn{
			      get{
        			return "TextStyle";
      			}
		    }
            
            public IColumn ImageStyle{
                get{
                    return this.GetColumn("ImageStyle");
                }
            }
				
   			public static string ImageStyleColumn{
			      get{
        			return "ImageStyle";
      			}
		    }
            
            public IColumn ContainerStyle{
                get{
                    return this.GetColumn("ContainerStyle");
                }
            }
				
   			public static string ContainerStyleColumn{
			      get{
        			return "ContainerStyle";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: StateProvince
        /// Primary Key: Id
        /// </summary>

        public class StateProvinceTable: DatabaseTable {
            
            public StateProvinceTable(IDataProvider provider):base("StateProvince",provider){
                ClassName = "StateProvince";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CountryId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Name", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("Abbreviation", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 100
                });

                Columns.Add(new DatabaseColumn("Published", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Boolean,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("DisplayOrder", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = 0
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CountryId{
                get{
                    return this.GetColumn("CountryId");
                }
            }
				
   			public static string CountryIdColumn{
			      get{
        			return "CountryId";
      			}
		    }
            
            public IColumn Name{
                get{
                    return this.GetColumn("Name");
                }
            }
				
   			public static string NameColumn{
			      get{
        			return "Name";
      			}
		    }
            
            public IColumn Abbreviation{
                get{
                    return this.GetColumn("Abbreviation");
                }
            }
				
   			public static string AbbreviationColumn{
			      get{
        			return "Abbreviation";
      			}
		    }
            
            public IColumn Published{
                get{
                    return this.GetColumn("Published");
                }
            }
				
   			public static string PublishedColumn{
			      get{
        			return "Published";
      			}
		    }
            
            public IColumn DisplayOrder{
                get{
                    return this.GetColumn("DisplayOrder");
                }
            }
				
   			public static string DisplayOrderColumn{
			      get{
        			return "DisplayOrder";
      			}
		    }
            
                    
        }
        
        /// <summary>
        /// Table: ExternalAuthenticationRecord
        /// Primary Key: Id
        /// </summary>

        public class ExternalAuthenticationRecordTable: DatabaseTable {
            
            public ExternalAuthenticationRecordTable(IDataProvider provider):base("ExternalAuthenticationRecord",provider){
                ClassName = "ExternalAuthenticationRecord";
                SchemaName = "dbo";
                

                Columns.Add(new DatabaseColumn("Id", this)
                {
	                IsPrimaryKey = true,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = true,
	                IsForeignKey = false,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("CustomerId", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.Int32,
	                IsNullable = false,
	                AutoIncrement = false,
	                IsForeignKey = true,
	                MaxLength = 0
                });

                Columns.Add(new DatabaseColumn("Email", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ExternalIdentifier", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ExternalDisplayIdentifier", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("OAuthToken", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("OAuthAccessToken", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });

                Columns.Add(new DatabaseColumn("ProviderSystemName", this)
                {
	                IsPrimaryKey = false,
	                DataType = DbType.String,
	                IsNullable = true,
	                AutoIncrement = false,
	                IsForeignKey = false,
	                MaxLength = -1
                });
                    
                
                
            }

            public IColumn Id{
                get{
                    return this.GetColumn("Id");
                }
            }
				
   			public static string IdColumn{
			      get{
        			return "Id";
      			}
		    }
            
            public IColumn CustomerId{
                get{
                    return this.GetColumn("CustomerId");
                }
            }
				
   			public static string CustomerIdColumn{
			      get{
        			return "CustomerId";
      			}
		    }
            
            public IColumn Email{
                get{
                    return this.GetColumn("Email");
                }
            }
				
   			public static string EmailColumn{
			      get{
        			return "Email";
      			}
		    }
            
            public IColumn ExternalIdentifier{
                get{
                    return this.GetColumn("ExternalIdentifier");
                }
            }
				
   			public static string ExternalIdentifierColumn{
			      get{
        			return "ExternalIdentifier";
      			}
		    }
            
            public IColumn ExternalDisplayIdentifier{
                get{
                    return this.GetColumn("ExternalDisplayIdentifier");
                }
            }
				
   			public static string ExternalDisplayIdentifierColumn{
			      get{
        			return "ExternalDisplayIdentifier";
      			}
		    }
            
            public IColumn OAuthToken{
                get{
                    return this.GetColumn("OAuthToken");
                }
            }
				
   			public static string OAuthTokenColumn{
			      get{
        			return "OAuthToken";
      			}
		    }
            
            public IColumn OAuthAccessToken{
                get{
                    return this.GetColumn("OAuthAccessToken");
                }
            }
				
   			public static string OAuthAccessTokenColumn{
			      get{
        			return "OAuthAccessToken";
      			}
		    }
            
            public IColumn ProviderSystemName{
                get{
                    return this.GetColumn("ProviderSystemName");
                }
            }
				
   			public static string ProviderSystemNameColumn{
			      get{
        			return "ProviderSystemName";
      			}
		    }
            
                    
        }
        
}