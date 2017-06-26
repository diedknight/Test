using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMe
{
    /// <summary>
    /// Summary description for PriceMeRequestValidator
    /// </summary>
    public class PriceMeRequestValidator : System.Web.Util.RequestValidator
    {
        protected override bool IsValidRequestString(System.Web.HttpContext context, string value, System.Web.Util.RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
        {
            if (context.Request.Form.Count == 0)
            {
                return base.IsValidRequestString(context, value, requestValidationSource, collectionKey, out validationFailureIndex);
            }
            else
            {
                validationFailureIndex = -1;
                return true;
            }

        }
    }
}