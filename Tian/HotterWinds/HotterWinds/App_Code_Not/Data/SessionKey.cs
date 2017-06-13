using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SessionKey
/// </summary>
namespace PriceMe {
    public enum SessionKey { 
        ForumCaptcha,
        ProductReviewCaptcha,
        ProductReview
    }

    public enum SSNType {
        SsnForum = 1,
        SsnProductReview = 2
    }
}