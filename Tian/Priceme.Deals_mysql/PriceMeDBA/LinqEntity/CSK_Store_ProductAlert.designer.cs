﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PriceMeDBA.LinqEntity
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Priceme_NZ3")]
	public partial class CSK_Store_ProductAlertDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertCSK_Store_ProductAlert(CSK_Store_ProductAlert instance);
    partial void UpdateCSK_Store_ProductAlert(CSK_Store_ProductAlert instance);
    partial void DeleteCSK_Store_ProductAlert(CSK_Store_ProductAlert instance);
    #endregion
		
		public CSK_Store_ProductAlertDataContext() : 
				base(global::PriceMeDBA.Properties.Settings.Default.Priceme_NZ3ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public CSK_Store_ProductAlertDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CSK_Store_ProductAlertDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CSK_Store_ProductAlertDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CSK_Store_ProductAlertDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<CSK_Store_ProductAlert> CSK_Store_ProductAlerts
		{
			get
			{
				return this.GetTable<CSK_Store_ProductAlert>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.CSK_Store_ProductAlert")]
	public partial class CSK_Store_ProductAlert : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _AlertId;
		
		private string _AlertGUID;
		
		private int _ProductId;
		
		private string _UserId;
		
		private string _Email;
		
		private System.Nullable<decimal> _ProductPrice;
		
		private System.Nullable<int> _Status;
		
		private System.Nullable<bool> _IsActive;
		
		private string _CreatedBy;
		
		private System.Nullable<System.DateTime> _CreatedOn;
		
		private System.Nullable<int> _AlertType;
		
		private string _ExcludedRetailers;
		
		private System.Nullable<int> _PriceType;
		
		private System.Nullable<decimal> _PriceEach;
		
		private System.Nullable<decimal> _OriginalPrice;
		
		private System.Nullable<System.DateTime> _UnsubscribeDate;
		
		private string _ParseID;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnAlertIdChanging(int value);
    partial void OnAlertIdChanged();
    partial void OnAlertGUIDChanging(string value);
    partial void OnAlertGUIDChanged();
    partial void OnProductIdChanging(int value);
    partial void OnProductIdChanged();
    partial void OnUserIdChanging(string value);
    partial void OnUserIdChanged();
    partial void OnEmailChanging(string value);
    partial void OnEmailChanged();
    partial void OnProductPriceChanging(System.Nullable<decimal> value);
    partial void OnProductPriceChanged();
    partial void OnStatusChanging(System.Nullable<int> value);
    partial void OnStatusChanged();
    partial void OnIsActiveChanging(System.Nullable<bool> value);
    partial void OnIsActiveChanged();
    partial void OnCreatedByChanging(string value);
    partial void OnCreatedByChanged();
    partial void OnCreatedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnCreatedOnChanged();
    partial void OnAlertTypeChanging(System.Nullable<int> value);
    partial void OnAlertTypeChanged();
    partial void OnExcludedRetailersChanging(string value);
    partial void OnExcludedRetailersChanged();
    partial void OnPriceTypeChanging(System.Nullable<int> value);
    partial void OnPriceTypeChanged();
    partial void OnPriceEachChanging(System.Nullable<decimal> value);
    partial void OnPriceEachChanged();
    partial void OnOriginalPriceChanging(System.Nullable<decimal> value);
    partial void OnOriginalPriceChanged();
    partial void OnUnsubscribeDateChanging(System.Nullable<System.DateTime> value);
    partial void OnUnsubscribeDateChanged();
    partial void OnParseIDChanging(string value);
    partial void OnParseIDChanged();
    #endregion
		
		public CSK_Store_ProductAlert()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AlertId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true, IsVersion=true)]
		public int AlertId
		{
			get
			{
				return this._AlertId;
			}
			set
			{
				if ((this._AlertId != value))
				{
					this.OnAlertIdChanging(value);
					this.SendPropertyChanging();
					this._AlertId = value;
					this.SendPropertyChanged("AlertId");
					this.OnAlertIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AlertGUID", DbType="VarChar(50)", UpdateCheck=UpdateCheck.Never)]
		public string AlertGUID
		{
			get
			{
				return this._AlertGUID;
			}
			set
			{
				if ((this._AlertGUID != value))
				{
					this.OnAlertGUIDChanging(value);
					this.SendPropertyChanging();
					this._AlertGUID = value;
					this.SendPropertyChanged("AlertGUID");
					this.OnAlertGUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProductId", DbType="Int NOT NULL", UpdateCheck=UpdateCheck.Never)]
		public int ProductId
		{
			get
			{
				return this._ProductId;
			}
			set
			{
				if ((this._ProductId != value))
				{
					this.OnProductIdChanging(value);
					this.SendPropertyChanging();
					this._ProductId = value;
					this.SendPropertyChanged("ProductId");
					this.OnProductIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="VarChar(100)", UpdateCheck=UpdateCheck.Never)]
		public string UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Email", DbType="VarChar(100)", UpdateCheck=UpdateCheck.Never)]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this.OnEmailChanging(value);
					this.SendPropertyChanging();
					this._Email = value;
					this.SendPropertyChanged("Email");
					this.OnEmailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProductPrice", DbType="Money", UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<decimal> ProductPrice
		{
			get
			{
				return this._ProductPrice;
			}
			set
			{
				if ((this._ProductPrice != value))
				{
					this.OnProductPriceChanging(value);
					this.SendPropertyChanging();
					this._ProductPrice = value;
					this.SendPropertyChanged("ProductPrice");
					this.OnProductPriceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="Int", UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<int> Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsActive", DbType="Bit", UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<bool> IsActive
		{
			get
			{
				return this._IsActive;
			}
			set
			{
				if ((this._IsActive != value))
				{
					this.OnIsActiveChanging(value);
					this.SendPropertyChanging();
					this._IsActive = value;
					this.SendPropertyChanged("IsActive");
					this.OnIsActiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedBy", DbType="VarChar(50)", UpdateCheck=UpdateCheck.Never)]
		public string CreatedBy
		{
			get
			{
				return this._CreatedBy;
			}
			set
			{
				if ((this._CreatedBy != value))
				{
					this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedOn", DbType="DateTime", UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<System.DateTime> CreatedOn
		{
			get
			{
				return this._CreatedOn;
			}
			set
			{
				if ((this._CreatedOn != value))
				{
					this.OnCreatedOnChanging(value);
					this.SendPropertyChanging();
					this._CreatedOn = value;
					this.SendPropertyChanged("CreatedOn");
					this.OnCreatedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AlertType", DbType="Int", UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<int> AlertType
		{
			get
			{
				return this._AlertType;
			}
			set
			{
				if ((this._AlertType != value))
				{
					this.OnAlertTypeChanging(value);
					this.SendPropertyChanging();
					this._AlertType = value;
					this.SendPropertyChanged("AlertType");
					this.OnAlertTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExcludedRetailers", DbType="NVarChar(1000)", UpdateCheck=UpdateCheck.Never)]
		public string ExcludedRetailers
		{
			get
			{
				return this._ExcludedRetailers;
			}
			set
			{
				if ((this._ExcludedRetailers != value))
				{
					this.OnExcludedRetailersChanging(value);
					this.SendPropertyChanging();
					this._ExcludedRetailers = value;
					this.SendPropertyChanged("ExcludedRetailers");
					this.OnExcludedRetailersChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PriceType", DbType="Int", UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<int> PriceType
		{
			get
			{
				return this._PriceType;
			}
			set
			{
				if ((this._PriceType != value))
				{
					this.OnPriceTypeChanging(value);
					this.SendPropertyChanging();
					this._PriceType = value;
					this.SendPropertyChanged("PriceType");
					this.OnPriceTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PriceEach", DbType="Money", UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<decimal> PriceEach
		{
			get
			{
				return this._PriceEach;
			}
			set
			{
				if ((this._PriceEach != value))
				{
					this.OnPriceEachChanging(value);
					this.SendPropertyChanging();
					this._PriceEach = value;
					this.SendPropertyChanged("PriceEach");
					this.OnPriceEachChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OriginalPrice", DbType="Money", UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<decimal> OriginalPrice
		{
			get
			{
				return this._OriginalPrice;
			}
			set
			{
				if ((this._OriginalPrice != value))
				{
					this.OnOriginalPriceChanging(value);
					this.SendPropertyChanging();
					this._OriginalPrice = value;
					this.SendPropertyChanged("OriginalPrice");
					this.OnOriginalPriceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UnsubscribeDate", DbType="Date", UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<System.DateTime> UnsubscribeDate
		{
			get
			{
				return this._UnsubscribeDate;
			}
			set
			{
				if ((this._UnsubscribeDate != value))
				{
					this.OnUnsubscribeDateChanging(value);
					this.SendPropertyChanging();
					this._UnsubscribeDate = value;
					this.SendPropertyChanged("UnsubscribeDate");
					this.OnUnsubscribeDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ParseID", DbType="NVarChar(50)", UpdateCheck=UpdateCheck.Never)]
		public string ParseID
		{
			get
			{
				return this._ParseID;
			}
			set
			{
				if ((this._ParseID != value))
				{
					this.OnParseIDChanging(value);
					this.SendPropertyChanging();
					this._ParseID = value;
					this.SendPropertyChanged("ParseID");
					this.OnParseIDChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
