﻿using System;
using FluentValidation.Attributes;
using Nop.Web.Areas.Admin.Validators.Catalog;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Web.Areas.Admin.Models.Catalog
{
    [Validator(typeof(ProductReviewValidator))]
    public partial class ProductReviewModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Store")]
        public string StoreName { get; set; }
        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Product")]
        public int ProductId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Product")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Customer")]
        public int CustomerId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Customer")]
        public string CustomerInfo { get; set; }
        
        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Title")]
        public string Title { get; set; }
        
        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.ReviewText")]
        public string ReviewText { get; set; }
        
        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.ReplyText")]
        public string ReplyText { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Rating")]
        public int Rating { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.IsApproved")]
        public bool IsApproved { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        //vendor
        public bool IsLoggedInAsVendor { get; set; }
    }
}