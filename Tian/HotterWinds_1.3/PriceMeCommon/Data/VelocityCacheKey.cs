using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public enum VelocityCacheKey
    {
        Non = 0,
        CategoryByNameWithClicks,
        MostPopularCategories,
        MostPopularProductByCategoryID,
        RetailerListOrderByNameWithClicks,
        AllCategoryAttributeGroup,
        StatusBarInfo,



        RelatedCategorys,
        AttributeValueRangeCacheList,
        CategoryAttributeTilteMapList,
        AttributeTitlesValuesCollection,
        AttributeTitleCategoryDictionary,
        CategoryAttributeTitleMapDictionary,
        AllExpertReviews,
        ProductVotesSumNZ,
        ProductVotesSumAU,
        CategoryBugingGuideMap,
        CategoryBGMapDic,
        RelatedManufacturerCategories,
        
        ReviewAverages,
        ExpertReviewSource,

        AlatestSources,
        NewExpertReview,
        ReviewCatgory,
        AllUserReviews,
        AllGlatLngs,
        
        RetailerReviewList,
        TopNRetailerTrackProducts,
        AllManufacturer,
        AllManufacturerLetter,
        ManufacturerRelatedCategoryClicks,
        FeaturedProducts,
        CategoryAttributeTitleDictionary,
        EnergyImgs,
        HiddenBrandsCategoryID,
        TreepodiaVideos
    }
}