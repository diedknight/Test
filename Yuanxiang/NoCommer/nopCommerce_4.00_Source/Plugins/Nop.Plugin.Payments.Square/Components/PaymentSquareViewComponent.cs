﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Payments.Square.Models;
using Nop.Plugin.Payments.Square.Services;
using Nop.Services.Common;
using Nop.Services.Localization;

namespace Nop.Plugin.Payments.Square.Components
{
    [ViewComponent(Name = "PaymentSquare")]
    public class PaymentSquareViewComponent : ViewComponent
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly SquarePaymentManager _squarePaymentManager;

        #endregion

        #region Ctor

        public PaymentSquareViewComponent(ILocalizationService localizationService,
            IWorkContext workContext,
            SquarePaymentManager squarePaymentManager)
        {
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._squarePaymentManager = squarePaymentManager;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke()
        {
            var model = new PaymentInfoModel
            {

                //whether current customer is guest
                IsGuest = _workContext.CurrentCustomer.IsGuest(),

                //get postal code from the billing address or from the shipping one
                PostalCode = _workContext.CurrentCustomer.BillingAddress?.ZipPostalCode
                ?? _workContext.CurrentCustomer.ShippingAddress?.ZipPostalCode
            };

            //whether customer already has stored cards
            var customerId = _workContext.CurrentCustomer.GetAttribute<string>(SquarePaymentDefaults.CustomerIdAttribute);
            var customer = _squarePaymentManager.GetCustomer(customerId);
            if (customer?.Cards != null)
            {
                var cardNumberMask = _localizationService.GetResource("Plugins.Payments.Square.Fields.StoredCard.Mask");
                model.StoredCards = customer.Cards.Select(card => new SelectListItem { Text = string.Format(cardNumberMask, card.Last4), Value = card.Id }).ToList();
            }

            //add the special item for 'select card' with value 0
            if (model.StoredCards.Any())
            {
                var selectCardText = _localizationService.GetResource("Plugins.Payments.Square.Fields.StoredCard.SelectCard");
                model.StoredCards.Insert(0, new SelectListItem { Text = selectCardText, Value = "0" });
            }

            return View("~/Plugins/Payments.Square/Views/PaymentInfo.cshtml", model);
        }

        #endregion
    }
}