


  
using System;
using SubSonic;
using SubSonic.Schema;
using SubSonic.DataProviders;
using System.Data;

namespace PriceMeDBA{
	public partial class PriceMeDBDB{

        public StoredProcedure AddCategory(){
            StoredProcedure sp=new StoredProcedure("AddCategory",this.Provider);
            return sp;
        }
        public StoredProcedure AddFieldFromNameToSKU(){
            StoredProcedure sp=new StoredProcedure("AddFieldFromNameToSKU",this.Provider);
            return sp;
        }
        public StoredProcedure AddNewGlatLngByCountry(){
            StoredProcedure sp=new StoredProcedure("AddNewGlatLngByCountry",this.Provider);
            return sp;
        }
        public StoredProcedure AdminCategoriesIndex(){
            StoredProcedure sp=new StoredProcedure("AdminCategoriesIndex",this.Provider);
            return sp;
        }
        public StoredProcedure AdminIndex(){
            StoredProcedure sp=new StoredProcedure("AdminIndex",this.Provider);
            return sp;
        }
        public StoredProcedure AdminIndexSP(){
            StoredProcedure sp=new StoredProcedure("AdminIndexSP",this.Provider);
            return sp;
        }
        public StoredProcedure APIFeedByMPN(){
            StoredProcedure sp=new StoredProcedure("APIFeedByMPN",this.Provider);
            return sp;
        }
        public StoredProcedure APIFeedBySKU(){
            StoredProcedure sp=new StoredProcedure("APIFeedBySKU",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_AnyDataInTables(){
            StoredProcedure sp=new StoredProcedure("aspnet_AnyDataInTables",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Applications_CreateApplication(){
            StoredProcedure sp=new StoredProcedure("aspnet_Applications_CreateApplication",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_CheckSchemaVersion(){
            StoredProcedure sp=new StoredProcedure("aspnet_CheckSchemaVersion",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_ChangePasswordQuestionAndAnswer(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_ChangePasswordQuestionAndAnswer",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_CreateUser(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_CreateUser",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_FindUsersByEmail(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_FindUsersByEmail",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_FindUsersByName(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_FindUsersByName",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_GetAllUsers(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_GetAllUsers",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_GetNumberOfUsersOnline(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_GetNumberOfUsersOnline",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_GetPassword(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_GetPassword",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_GetPasswordWithFormat(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_GetPasswordWithFormat",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_GetUserByEmail(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_GetUserByEmail",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_GetUserByName(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_GetUserByName",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_GetUserByUserId(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_GetUserByUserId",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_ResetPassword(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_ResetPassword",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_SetPassword(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_SetPassword",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_UnlockUser(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_UnlockUser",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_UpdateUser(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_UpdateUser",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Membership_UpdateUserInfo(){
            StoredProcedure sp=new StoredProcedure("aspnet_Membership_UpdateUserInfo",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Paths_CreatePath(){
            StoredProcedure sp=new StoredProcedure("aspnet_Paths_CreatePath",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Personalization_GetApplicationId(){
            StoredProcedure sp=new StoredProcedure("aspnet_Personalization_GetApplicationId",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationAdministration_DeleteAllState(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationAdministration_DeleteAllState",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationAdministration_FindState(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationAdministration_FindState",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationAdministration_GetCountOfState(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationAdministration_GetCountOfState",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationAdministration_ResetSharedState(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationAdministration_ResetSharedState",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationAdministration_ResetUserState(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationAdministration_ResetUserState",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationAllUsers_GetPageSettings(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationAllUsers_GetPageSettings",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationAllUsers_ResetPageSettings(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationAllUsers_ResetPageSettings",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationAllUsers_SetPageSettings(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationAllUsers_SetPageSettings",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationPerUser_GetPageSettings(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationPerUser_GetPageSettings",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationPerUser_ResetPageSettings(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationPerUser_ResetPageSettings",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_PersonalizationPerUser_SetPageSettings(){
            StoredProcedure sp=new StoredProcedure("aspnet_PersonalizationPerUser_SetPageSettings",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Profile_DeleteInactiveProfiles(){
            StoredProcedure sp=new StoredProcedure("aspnet_Profile_DeleteInactiveProfiles",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Profile_DeleteProfiles(){
            StoredProcedure sp=new StoredProcedure("aspnet_Profile_DeleteProfiles",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Profile_GetNumberOfInactiveProfiles(){
            StoredProcedure sp=new StoredProcedure("aspnet_Profile_GetNumberOfInactiveProfiles",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Profile_GetProfiles(){
            StoredProcedure sp=new StoredProcedure("aspnet_Profile_GetProfiles",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Profile_GetProperties(){
            StoredProcedure sp=new StoredProcedure("aspnet_Profile_GetProperties",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Profile_SetProperties(){
            StoredProcedure sp=new StoredProcedure("aspnet_Profile_SetProperties",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_RegisterSchemaVersion(){
            StoredProcedure sp=new StoredProcedure("aspnet_RegisterSchemaVersion",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Roles_CreateRole(){
            StoredProcedure sp=new StoredProcedure("aspnet_Roles_CreateRole",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Roles_DeleteRole(){
            StoredProcedure sp=new StoredProcedure("aspnet_Roles_DeleteRole",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Roles_GetAllRoles(){
            StoredProcedure sp=new StoredProcedure("aspnet_Roles_GetAllRoles",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Roles_RoleExists(){
            StoredProcedure sp=new StoredProcedure("aspnet_Roles_RoleExists",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Setup_RemoveAllRoleMembers(){
            StoredProcedure sp=new StoredProcedure("aspnet_Setup_RemoveAllRoleMembers",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Setup_RestorePermissions(){
            StoredProcedure sp=new StoredProcedure("aspnet_Setup_RestorePermissions",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_UnRegisterSchemaVersion(){
            StoredProcedure sp=new StoredProcedure("aspnet_UnRegisterSchemaVersion",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Users_CreateUser(){
            StoredProcedure sp=new StoredProcedure("aspnet_Users_CreateUser",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Users_DeleteUser(){
            StoredProcedure sp=new StoredProcedure("aspnet_Users_DeleteUser",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Users_GetEmail(){
            StoredProcedure sp=new StoredProcedure("aspnet_Users_GetEmail",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_Users_GetFirstName(){
            StoredProcedure sp=new StoredProcedure("aspnet_Users_GetFirstName",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_UsersInRoles_AddUsersToRoles(){
            StoredProcedure sp=new StoredProcedure("aspnet_UsersInRoles_AddUsersToRoles",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_UsersInRoles_FindUsersInRole(){
            StoredProcedure sp=new StoredProcedure("aspnet_UsersInRoles_FindUsersInRole",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_UsersInRoles_GetRolesForUser(){
            StoredProcedure sp=new StoredProcedure("aspnet_UsersInRoles_GetRolesForUser",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_UsersInRoles_GetUsersInRoles(){
            StoredProcedure sp=new StoredProcedure("aspnet_UsersInRoles_GetUsersInRoles",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_UsersInRoles_IsUserInRole(){
            StoredProcedure sp=new StoredProcedure("aspnet_UsersInRoles_IsUserInRole",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_UsersInRoles_RemoveUsersFromRoles(){
            StoredProcedure sp=new StoredProcedure("aspnet_UsersInRoles_RemoveUsersFromRoles",this.Provider);
            return sp;
        }
        public StoredProcedure aspnet_WebEvent_LogEvent(){
            StoredProcedure sp=new StoredProcedure("aspnet_WebEvent_LogEvent",this.Provider);
            return sp;
        }
        public StoredProcedure bbbb(){
            StoredProcedure sp=new StoredProcedure("bbbb",this.Provider);
            return sp;
        }
        public StoredProcedure bbbbb(){
            StoredProcedure sp=new StoredProcedure("bbbbb",this.Provider);
            return sp;
        }
        public StoredProcedure bbbbb_ProductDescriptor(){
            StoredProcedure sp=new StoredProcedure("bbbbb_ProductDescriptor",this.Provider);
            return sp;
        }
        public StoredProcedure CalculateRetailerStatus(){
            StoredProcedure sp=new StoredProcedure("CalculateRetailerStatus",this.Provider);
            return sp;
        }
        public StoredProcedure ChangeProductCategory(){
            StoredProcedure sp=new StoredProcedure("ChangeProductCategory",this.Provider);
            return sp;
        }
        public StoredProcedure ClearLogs(){
            StoredProcedure sp=new StoredProcedure("ClearLogs",this.Provider);
            return sp;
        }
        public StoredProcedure CreateNewProduct(){
            StoredProcedure sp=new StoredProcedure("CreateNewProduct",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Content_Get(){
            StoredProcedure sp=new StoredProcedure("CSK_Content_Get",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Content_Insert(){
            StoredProcedure sp=new StoredProcedure("CSK_Content_Insert",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Content_Save(){
            StoredProcedure sp=new StoredProcedure("CSK_Content_Save",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Content_Update(){
            StoredProcedure sp=new StoredProcedure("CSK_Content_Update",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Coupons_GetAllCouponTypes(){
            StoredProcedure sp=new StoredProcedure("CSK_Coupons_GetAllCouponTypes",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Coupons_GetCoupon(){
            StoredProcedure sp=new StoredProcedure("CSK_Coupons_GetCoupon",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Coupons_GetCouponType(){
            StoredProcedure sp=new StoredProcedure("CSK_Coupons_GetCouponType",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Coupons_SaveCoupon(){
            StoredProcedure sp=new StoredProcedure("CSK_Coupons_SaveCoupon",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Coupons_SaveCouponType(){
            StoredProcedure sp=new StoredProcedure("CSK_Coupons_SaveCouponType",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Forum_AbuseList(){
            StoredProcedure sp=new StoredProcedure("CSK_Forum_AbuseList",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Forum_CategorySummary(){
            StoredProcedure sp=new StoredProcedure("CSK_Forum_CategorySummary",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Forum_QPostList(){
            StoredProcedure sp=new StoredProcedure("CSK_Forum_QPostList",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Forum_TopicInfo(){
            StoredProcedure sp=new StoredProcedure("CSK_Forum_TopicInfo",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Forum_TopicList(){
            StoredProcedure sp=new StoredProcedure("CSK_Forum_TopicList",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Forum_TopicListShowAllCat(){
            StoredProcedure sp=new StoredProcedure("CSK_Forum_TopicListShowAllCat",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Image_IsEnlarge(){
            StoredProcedure sp=new StoredProcedure("CSK_Image_IsEnlarge",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Product_Category_Map(){
            StoredProcedure sp=new StoredProcedure("CSK_Product_Category_Map",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Product_DescAndAttr(){
            StoredProcedure sp=new StoredProcedure("CSK_Product_DescAndAttr",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_productCategoryMap(){
            StoredProcedure sp=new StoredProcedure("CSK_productCategoryMap",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Promo_AddProduct(){
            StoredProcedure sp=new StoredProcedure("CSK_Promo_AddProduct",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Promo_Bundle_AddProduct(){
            StoredProcedure sp=new StoredProcedure("CSK_Promo_Bundle_AddProduct",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Promo_Bundle_GetAvailableProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Promo_Bundle_GetAvailableProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Promo_Bundle_GetByProductID(){
            StoredProcedure sp=new StoredProcedure("CSK_Promo_Bundle_GetByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Promo_Bundle_GetSelectedProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Promo_Bundle_GetSelectedProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Promo_EnsureOrderCoupon(){
            StoredProcedure sp=new StoredProcedure("CSK_Promo_EnsureOrderCoupon",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Promo_GetProductList(){
            StoredProcedure sp=new StoredProcedure("CSK_Promo_GetProductList",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Promo_ProductMatrix(){
            StoredProcedure sp=new StoredProcedure("CSK_Promo_ProductMatrix",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Promo_RemoveProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Promo_RemoveProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Shipping_GetRates(){
            StoredProcedure sp=new StoredProcedure("CSK_Shipping_GetRates",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Shipping_GetRates_Air(){
            StoredProcedure sp=new StoredProcedure("CSK_Shipping_GetRates_Air",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Shipping_GetRates_Ground(){
            StoredProcedure sp=new StoredProcedure("CSK_Shipping_GetRates_Ground",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Sotre_RetailerProduct_GetAllCountByRetailerId(){
            StoredProcedure sp=new StoredProcedure("CSK_Sotre_RetailerProduct_GetAllCountByRetailerId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Stats_FavoriteCategories(){
            StoredProcedure sp=new StoredProcedure("CSK_Stats_FavoriteCategories",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Stats_FavoriteProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Stats_FavoriteProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Stats_Tracker_GetByBehaviorID(){
            StoredProcedure sp=new StoredProcedure("CSK_Stats_Tracker_GetByBehaviorID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Stats_Tracker_GetByProductAndBehavior(){
            StoredProcedure sp=new StoredProcedure("CSK_Stats_Tracker_GetByProductAndBehavior",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Stats_Tracker_SynchTrackingCookie(){
            StoredProcedure sp=new StoredProcedure("CSK_Stats_Tracker_SynchTrackingCookie",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetBestPriceByProductID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetBestPriceByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetBestPriceRetailerProduct(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetBestPriceRetailerProduct",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetCategoryFeaturedProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetCategoryFeaturedProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetMarketPrice(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetMarketPrice",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetMostFocusedProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetMostFocusedProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetMostPopularProductsByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetMostPopularProductsByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetMostPopularProductsByGroupList(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetMostPopularProductsByGroupList",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetMostPopularProductsBySubCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetMostPopularProductsBySubCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetNewProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetNewProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetNewProductsByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetNewProductsByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetRelatedProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetRelatedProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetRelatedProductsByParents(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetRelatedProductsByParents",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetRetailerProductByProductIdSmp(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetRetailerProductByProductIdSmp",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetRetailerProductIdByProductID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetRetailerProductIdByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetRetailerRating(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetRetailerRating",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetRetailerReviewsById(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetRetailerReviewsById",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetStatusBarData(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetStatusBarData",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetTopRatingProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetTopRatingProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_GetTopRatingProductsByCategoryId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_GetTopRatingProductsByCategoryId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetAllProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetAllProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetAllRetailerProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetAllRetailerProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetCategoryProductsByCID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetCategoryProductsByCID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetCategoryProductsByCID_2(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetCategoryProductsByCID_2",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetHistory(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetHistory",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetParentIDZero(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetParentIDZero",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetProductRetailerMap(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetProductRetailerMap",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetProductsByBigCategoryId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetProductsByBigCategoryId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetProductsByRID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetProductsByRID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetProductsDescriptor(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetProductsDescriptor",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetRetailerClicks(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetRetailerClicks",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetRetailerProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetRetailerProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetRetailerProductsByCountry(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetRetailerProductsByCountry",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetRetailerProductsByCountryId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetRetailerProductsByCountryId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetShoppingRetailerProductsByCountryId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetShoppingRetailerProductsByCountryId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Index_GetWines(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Index_GetWines",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_IndexPam_GetRetailerProductsByCountryId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_IndexPam_GetRetailerProductsByCountryId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_PamIndex_GetRetailerProductsByCountryId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_PamIndex_GetRetailerProductsByCountryId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_GetAll(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_GetAll",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_GetByProductName(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_GetByProductName",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_GetFeaturedProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_GetFeaturedProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_GetProductIsMeraed(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_GetProductIsMeraed",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_GetRetailerProductCount(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_GetRetailerProductCount",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_MergeProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_MergeProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_MergeProducts_ExIsMerge(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_MergeProducts_ExIsMerge",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_MergeProducts123(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_MergeProducts123",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_MergeProducts2(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_MergeProducts2",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_MergeTempProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_MergeTempProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_MergeTempProducts_ExIsmerge(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_MergeTempProducts_ExIsmerge",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Product_MergeTempProducts2(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Product_MergeTempProducts2",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Retailer_GetByProductId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Retailer_GetByProductId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Retailer_GetByProductIdOrderByRating(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Retailer_GetByProductIdOrderByRating",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Retailer_GetByProductIdOrderByTotalPrice(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Retailer_GetByProductIdOrderByTotalPrice",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Retailer_GetByProductIdSmp(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Retailer_GetByProductIdSmp",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Retailer_GetCityByProductId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Retailer_GetCityByProductId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Retailer_GetRetailerAmountByProductId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Retailer_GetRetailerAmountByProductId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_RetailerReview_GetAll(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_RetailerReview_GetAll",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Wine_GetCountryByCategoryId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Wine_GetCountryByCategoryId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_12RMB_Wine_GetVarietyByCategoryId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_12RMB_Wine_GetVarietyByCategoryId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_AddItemToCart(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_AddItemToCart",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_AttributeByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_AttributeByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAllBrands(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAllBrands",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAllCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAllCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAllCategoryTotal(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAllCategoryTotal",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAllSubCategoryByParentID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAllSubCategoryByParentID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAllSubs(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAllSubs",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAllTopBrandsGroupByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAllTopBrandsGroupByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAttributeCategories(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAttributeCategories",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAttributeCategoriesID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAttributeCategoriesID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAttributeRange(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAttributeRange",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAttributeRangeID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAttributeRangeID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAttributeRangeValue(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAttributeRangeValue",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAttributeTitleID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAttributeTitleID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetAttributeValue(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetAttributeValue",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetBabySubCategory(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetBabySubCategory",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetBrandsByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetBrandsByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetByProductID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetCategoryByParentID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetCategoryByParentID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetCategoryByRetaulerID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetCategoryByRetaulerID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetCategoryTobBrandsByManID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetCategoryTobBrandsByManID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetCategoryTopBrands(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetCategoryTopBrands",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetCategoryTotal(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetCategoryTotal",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetChildrenID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetChildrenID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetCrumbs(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetCrumbs",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetCrumbsByProductID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetCrumbsByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetPageByGUIDMulti(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetPageByGUIDMulti",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetPageByNameMulti(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetPageByNameMulti",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetPageMulti(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetPageMulti",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetParentCategoryName(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetParentCategoryName",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetParentID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetParentID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetPriceRange(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetPriceRange",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetProductCategoryTotal(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetProductCategoryTotal",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetRelatedPorductByPrice(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetRelatedPorductByPrice",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetSubCategory(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetSubCategory",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_GetTopBrandsByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_GetTopBrandsByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Category_UpdateCategoryCount(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Category_UpdateCategoryCount",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_CategoryGetActive(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_CategoryGetActive",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_CategoryTree(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_CategoryTree",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Config_GetList(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Config_GetList",this.Provider);
            return sp;
        }
        public StoredProcedure csk_store_CreateSitemap(){
            StoredProcedure sp=new StoredProcedure("csk_store_CreateSitemap",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_ExpertReview_GetAllExpertReview(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_ExpertReview_GetAllExpertReview",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_ExpertReview_GetAllExpertReviewTF(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_ExpertReview_GetAllExpertReviewTF",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_FeatureProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_FeatureProducts",this.Provider);
            return sp;
        }
        public StoredProcedure csk_store_FreightLocation_City(){
            StoredProcedure sp=new StoredProcedure("csk_store_FreightLocation_City",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetBestRetailerForNewsletter(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetBestRetailerForNewsletter",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetClickReffrral(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetClickReffrral",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetDuplicateRetailerProduct(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetDuplicateRetailerProduct",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetPPCMemberByCountry(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetPPCMemberByCountry",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetPPCMemberByCreditCard(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetPPCMemberByCreditCard",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetPriceDropInLastNDays(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetPriceDropInLastNDays",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetPriceDropInLastNDays2(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetPriceDropInLastNDays2",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetPriceDropInLastNDays3(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetPriceDropInLastNDays3",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetProductDescriptorTitle(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetProductDescriptorTitle",this.Provider);
            return sp;
        }
        public StoredProcedure csk_store_GetProductsByRetailerID(){
            StoredProcedure sp=new StoredProcedure("csk_store_GetProductsByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetProductsImage(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetProductsImage",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetRetailerProductCountByCategory(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetRetailerProductCountByCategory",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_GetRetailerProductsByProductID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_GetRetailerProductsByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_MostPopularCategoriesByTime(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_MostPopularCategoriesByTime",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Order_Query(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Order_Query",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_PopularSearch(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_PopularSearch",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_PopularSearch_For_Map(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_PopularSearch_For_Map",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_PPCCategory_GetMinMaxCostPerClick(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_PPCCategory_GetMinMaxCostPerClick",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_PPCMemberGetAll(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_PPCMemberGetAll",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_PPCMemberGetAllByTypeID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_PPCMemberGetAllByTypeID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_PPCStatsByCreatedOn(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_PPCStatsByCreatedOn",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_PPCTransactionByPPCMemberId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_PPCTransactionByPPCMemberId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_AddRating(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_AddRating",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_CountProductGetByManufacturerID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_CountProductGetByManufacturerID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_DeletePermanent(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_DeletePermanent",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetAllProductsCountsByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetAllProductsCountsByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetByCategoryName(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetByCategoryName",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetByManufacturerID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetByManufacturerID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetByManufacturerIDAndPriceRange(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetByManufacturerIDAndPriceRange",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetByPriceRange(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetByPriceRange",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetByRetailerID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure csk_store_product_GetCategoryCount_ByRetailerID(){
            StoredProcedure sp=new StoredProcedure("csk_store_product_GetCategoryCount_ByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetDescriptor(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetDescriptor",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetDescriptorByProductID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetDescriptorByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetExpertReviewByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetExpertReviewByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetFeaturesByProductID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetFeaturesByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetIsMergedByProductId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetIsMergedByProductId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetMostPopular(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetMostPopular",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetMostPopularByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetMostPopularByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetPostAddMulti(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetPostAddMulti",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetProductAccessories(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetProductAccessories",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetProductPositionRangeByRetailerAndCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetProductPositionRangeByRetailerAndCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetProductsCountByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetProductsCountByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetSameProductByModifiedOn(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetSameProductByModifiedOn",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetSameProductByProductName(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetSameProductByProductName",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_GetUserReviewByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_GetUserReviewByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_ModifyProductName(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_ModifyProductName",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_SmartSearch(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_SmartSearch",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_SmartSearchByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_SmartSearchByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_Wine(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_Wine",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Product_Wine_GetByCategoryId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Product_Wine_GetByCategoryId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_ProductCategoryMapUpdate(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_ProductCategoryMapUpdate",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_ProductDescriptorTitleAndAttributeTitleMap(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_ProductDescriptorTitleAndAttributeTitleMap",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_ProductIsMergedByCreatedOn(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_ProductIsMergedByCreatedOn",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_ProductVideo_GetAllVideo(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_ProductVideo_GetAllVideo",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Reassign_Product_Category(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Reassign_Product_Category",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Reassign_Product_Category2(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Reassign_Product_Category2",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Reassign_Product_Manufacturer(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Reassign_Product_Manufacturer",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_ReAssignManufacturerID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_ReAssignManufacturerID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RelatedCategory_GetByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RelatedCategory_GetByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_DeletePermanent(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_DeletePermanent",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_DeleteUserIP(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_DeleteUserIP",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetAllValidRetailers(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetAllValidRetailers",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetClickTotal(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetClickTotal",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetIdByRetailerCatID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetIdByRetailerCatID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetProductStock(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetProductStock",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetRetailerCategory(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetRetailerCategory",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetRetailerCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetRetailerCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetRetailerCategoryTotal(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetRetailerCategoryTotal",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetRetailerCrumbs(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetRetailerCrumbs",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetRetailerDailyTrackerByRetailerID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetRetailerDailyTrackerByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetRetailerDailyTrackerByRetailerIDTest(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetRetailerDailyTrackerByRetailerIDTest",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetRetailerProductTracker(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetRetailerProductTracker",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetRetailerTrackerByRetailerID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetRetailerTrackerByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_GetTotalRetailerTracker(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_GetTotalRetailerTracker",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Retailer_Transation(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Retailer_Transation",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerId_Transation(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerId_Transation",this.Provider);
            return sp;
        }
        public StoredProcedure csk_store_retailerproduct_DuplicateNewProducts(){
            StoredProcedure sp=new StoredProcedure("csk_store_retailerproduct_DuplicateNewProducts",this.Provider);
            return sp;
        }
        public StoredProcedure csk_store_retailerproduct_DuplicateNewProductsByRetailerId(){
            StoredProcedure sp=new StoredProcedure("csk_store_retailerproduct_DuplicateNewProductsByRetailerId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerProduct_Freight(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerProduct_Freight",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerProduct_FreightLocation(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerProduct_FreightLocation",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerProduct_GetBestRankByCategoryid(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerProduct_GetBestRankByCategoryid",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerProduct_GetMaxRankByCategoryid(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerProduct_GetMaxRankByCategoryid",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerProduct_GetRankByPara(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerProduct_GetRankByPara",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerProductsChangeRate(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerProductsChangeRate",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerTrackerDeleteByTrackerId(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerTrackerDeleteByTrackerId",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerTrackerDeleteByTrackerIdTime(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerTrackerDeleteByTrackerIdTime",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerTraker_GetTopProductRank(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerTraker_GetTopProductRank",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerTraker_ProductRank_ByCategoryID(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerTraker_ProductRank_ByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_RetailerUserEmailGetBillEmail(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_RetailerUserEmailGetBillEmail",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_SearchProductByCategoryIDAndKeywords(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_SearchProductByCategoryIDAndKeywords",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_TopNRetailerTrackProducts(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_TopNRetailerTrackProducts",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_TopNRetailerTrackProductsByCategory(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_TopNRetailerTrackProductsByCategory",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_Upadate_ProductViewTracking(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_Upadate_ProductViewTracking",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_UPDATE_ProductRating(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_UPDATE_ProductRating",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_UPDATE_RetailerTracker(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_UPDATE_RetailerTracker",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_ZeroProductsChangeRate(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_ZeroProductsChangeRate",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Store_ZeroProductsCount(){
            StoredProcedure sp=new StoredProcedure("CSK_Store_ZeroProductsCount",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Tax_CalculateAmountByState(){
            StoredProcedure sp=new StoredProcedure("CSK_Tax_CalculateAmountByState",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Tax_CalculateAmountByZIP(){
            StoredProcedure sp=new StoredProcedure("CSK_Tax_CalculateAmountByZIP",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Tax_GetTaxRate(){
            StoredProcedure sp=new StoredProcedure("CSK_Tax_GetTaxRate",this.Provider);
            return sp;
        }
        public StoredProcedure CSK_Tax_SaveZipRate(){
            StoredProcedure sp=new StoredProcedure("CSK_Tax_SaveZipRate",this.Provider);
            return sp;
        }
        public StoredProcedure DeleteAttribute(){
            StoredProcedure sp=new StoredProcedure("DeleteAttribute",this.Provider);
            return sp;
        }
        public StoredProcedure DeleteGlatLngByCountry(){
            StoredProcedure sp=new StoredProcedure("DeleteGlatLngByCountry",this.Provider);
            return sp;
        }
        public StoredProcedure DeleteHelpSetIsMergeHistory(){
            StoredProcedure sp=new StoredProcedure("DeleteHelpSetIsMergeHistory",this.Provider);
            return sp;
        }
        public StoredProcedure DeleteProductByID(){
            StoredProcedure sp=new StoredProcedure("DeleteProductByID",this.Provider);
            return sp;
        }
        public StoredProcedure DeleteRelatedTableWithProductId(){
            StoredProcedure sp=new StoredProcedure("DeleteRelatedTableWithProductId",this.Provider);
            return sp;
        }
        public StoredProcedure DeLeteRetailerProductByProductID(){
            StoredProcedure sp=new StoredProcedure("DeLeteRetailerProductByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure DeLeteRetailerProductByRetailerProductID(){
            StoredProcedure sp=new StoredProcedure("DeLeteRetailerProductByRetailerProductID",this.Provider);
            return sp;
        }
        public StoredProcedure DELRetailerProduct(){
            StoredProcedure sp=new StoredProcedure("DELRetailerProduct",this.Provider);
            return sp;
        }
        public StoredProcedure DoUnmerge(){
            StoredProcedure sp=new StoredProcedure("DoUnmerge",this.Provider);
            return sp;
        }
        public StoredProcedure Get_Category_AttributeTitle_MapAndCSK_Store_ProductDescriptorTitle(){
            StoredProcedure sp=new StoredProcedure("Get_Category_AttributeTitle_MapAndCSK_Store_ProductDescriptorTitle",this.Provider);
            return sp;
        }
        public StoredProcedure GetAlatestFeedFileProducts(){
            StoredProcedure sp=new StoredProcedure("GetAlatestFeedFileProducts",this.Provider);
            return sp;
        }
        public StoredProcedure GetALLActiveDirectory(){
            StoredProcedure sp=new StoredProcedure("GetALLActiveDirectory",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllActiveRetailers(){
            StoredProcedure sp=new StoredProcedure("GetAllActiveRetailers",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllAdvancedMappingCategoryID(){
            StoredProcedure sp=new StoredProcedure("GetAllAdvancedMappingCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllCateRetailerTracker(){
            StoredProcedure sp=new StoredProcedure("GetAllCateRetailerTracker",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllExpertReview(){
            StoredProcedure sp=new StoredProcedure("GetAllExpertReview",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllExpertReviews(){
            StoredProcedure sp=new StoredProcedure("GetAllExpertReviews",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllExpertReviewSource(){
            StoredProcedure sp=new StoredProcedure("GetAllExpertReviewSource",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllExpertReviewSourceTF(){
            StoredProcedure sp=new StoredProcedure("GetAllExpertReviewSourceTF",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllGlatLng(){
            StoredProcedure sp=new StoredProcedure("GetAllGlatLng",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllGlatLngByCountryID(){
            StoredProcedure sp=new StoredProcedure("GetAllGlatLngByCountryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllGlatLngByCountyID(){
            StoredProcedure sp=new StoredProcedure("GetAllGlatLngByCountyID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllImageProductByRetaielrID(){
            StoredProcedure sp=new StoredProcedure("GetAllImageProductByRetaielrID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllManufacturer(){
            StoredProcedure sp=new StoredProcedure("GetAllManufacturer",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllMatchCategories(){
            StoredProcedure sp=new StoredProcedure("GetAllMatchCategories",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllNotSupportCategoriesByAdmin(){
            StoredProcedure sp=new StoredProcedure("GetAllNotSupportCategoriesByAdmin",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllOperatingHours(){
            StoredProcedure sp=new StoredProcedure("GetAllOperatingHours",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductAttributeInfo(){
            StoredProcedure sp=new StoredProcedure("GetAllProductAttributeInfo",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductReview(){
            StoredProcedure sp=new StoredProcedure("GetAllProductReview",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductReview2(){
            StoredProcedure sp=new StoredProcedure("GetAllProductReview2",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductReviewCountByAuthorName(){
            StoredProcedure sp=new StoredProcedure("GetAllProductReviewCountByAuthorName",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductReviewsByAuthorName(){
            StoredProcedure sp=new StoredProcedure("GetAllProductReviewsByAuthorName",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductsByCategoryIDAndAttributeTitleID(){
            StoredProcedure sp=new StoredProcedure("GetAllProductsByCategoryIDAndAttributeTitleID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductsByCategoryIDAndAttributeTitleIDAdmin(){
            StoredProcedure sp=new StoredProcedure("GetAllProductsByCategoryIDAndAttributeTitleIDAdmin",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductsByCategoryIDAndCompareAttributeID(){
            StoredProcedure sp=new StoredProcedure("GetAllProductsByCategoryIDAndCompareAttributeID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductsByCategoryIDAndHeightEqualZero(){
            StoredProcedure sp=new StoredProcedure("GetAllProductsByCategoryIDAndHeightEqualZero",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductsByCategoryIDAndLengthEqualZero(){
            StoredProcedure sp=new StoredProcedure("GetAllProductsByCategoryIDAndLengthEqualZero",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductsByCategoryIDAndWeightEqualZero(){
            StoredProcedure sp=new StoredProcedure("GetAllProductsByCategoryIDAndWeightEqualZero",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllProductsByCategoryIDAndWidthEqualZero(){
            StoredProcedure sp=new StoredProcedure("GetAllProductsByCategoryIDAndWidthEqualZero",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRelatedCategories(){
            StoredProcedure sp=new StoredProcedure("GetAllRelatedCategories",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerCrawlerInfoOrderByPriority(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerCrawlerInfoOrderByPriority",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerReview(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerReview",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerReviewCountByAuthorName(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerReviewCountByAuthorName",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerTrackerByAlid(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerTrackerByAlid",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerTrackerByCategoryID(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerTrackerByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerTrackerCost(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerTrackerCost",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerTrackerCostByCateID(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerTrackerCostByCateID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerTrackProduct(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerTrackProduct",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerTrackProductByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerTrackProductByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerTrackUserIP(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerTrackUserIP",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerTrackUserIPByCataID(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerTrackUserIPByCataID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllRetailerTrackUserIPByID(){
            StoredProcedure sp=new StoredProcedure("GetAllRetailerTrackUserIPByID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllSubAllCateRetailerTrackerByCateID(){
            StoredProcedure sp=new StoredProcedure("GetAllSubAllCateRetailerTrackerByCateID",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllSubCategorysByCategoryId(){
            StoredProcedure sp=new StoredProcedure("GetAllSubCategorysByCategoryId",this.Provider);
            return sp;
        }
        public StoredProcedure GetAllSubRetailerTrackUserIPByCataID(){
            StoredProcedure sp=new StoredProcedure("GetAllSubRetailerTrackUserIPByCataID",this.Provider);
            return sp;
        }
        public StoredProcedure getAttributesTitleByProductID(){
            StoredProcedure sp=new StoredProcedure("getAttributesTitleByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure GetCategoriesIndex(){
            StoredProcedure sp=new StoredProcedure("GetCategoriesIndex",this.Provider);
            return sp;
        }
        public StoredProcedure GetCompareAttributeByCategoryId(){
            StoredProcedure sp=new StoredProcedure("GetCompareAttributeByCategoryId",this.Provider);
            return sp;
        }
        public StoredProcedure GetConsumerPriceMeMappingWithImageEmpty(){
            StoredProcedure sp=new StoredProcedure("GetConsumerPriceMeMappingWithImageEmpty",this.Provider);
            return sp;
        }
        public StoredProcedure GetConsumerPriceMeMappingWithLongDescriptionEmpty(){
            StoredProcedure sp=new StoredProcedure("GetConsumerPriceMeMappingWithLongDescriptionEmpty",this.Provider);
            return sp;
        }
        public StoredProcedure GetConsumerPriceMeMappingWithNoActiveRetailerProduct(){
            StoredProcedure sp=new StoredProcedure("GetConsumerPriceMeMappingWithNoActiveRetailerProduct",this.Provider);
            return sp;
        }
        public StoredProcedure GetCreditPPCTransactionByRetailerId(){
            StoredProcedure sp=new StoredProcedure("GetCreditPPCTransactionByRetailerId",this.Provider);
            return sp;
        }
        public StoredProcedure GetDailyDealsTrackerByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetDailyDealsTrackerByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetDatabaseSize(){
            StoredProcedure sp=new StoredProcedure("GetDatabaseSize",this.Provider);
            return sp;
        }
        public StoredProcedure GetDuplicatesProductsByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetDuplicatesProductsByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetEnergyMap(){
            StoredProcedure sp=new StoredProcedure("GetEnergyMap",this.Provider);
            return sp;
        }
        public StoredProcedure GetExpertReview(){
            StoredProcedure sp=new StoredProcedure("GetExpertReview",this.Provider);
            return sp;
        }
        public StoredProcedure GetFeaturedProductFromMostPopularProducts(){
            StoredProcedure sp=new StoredProcedure("GetFeaturedProductFromMostPopularProducts",this.Provider);
            return sp;
        }
        public StoredProcedure GetFeedRetailerCrawlerInfo(){
            StoredProcedure sp=new StoredProcedure("GetFeedRetailerCrawlerInfo",this.Provider);
            return sp;
        }
        public StoredProcedure GetFeedRetailerCrawlerInfoByCountryId(){
            StoredProcedure sp=new StoredProcedure("GetFeedRetailerCrawlerInfoByCountryId",this.Provider);
            return sp;
        }
        public StoredProcedure GetFinishingCategoryImages(){
            StoredProcedure sp=new StoredProcedure("GetFinishingCategoryImages",this.Provider);
            return sp;
        }
        public StoredProcedure GetFinishingManufacturerImages(){
            StoredProcedure sp=new StoredProcedure("GetFinishingManufacturerImages",this.Provider);
            return sp;
        }
        public StoredProcedure GetFinishingProductImages(){
            StoredProcedure sp=new StoredProcedure("GetFinishingProductImages",this.Provider);
            return sp;
        }
        public StoredProcedure GetFinishingRetailerImages(){
            StoredProcedure sp=new StoredProcedure("GetFinishingRetailerImages",this.Provider);
            return sp;
        }
        public StoredProcedure GetFinishingRetailerProductImages(){
            StoredProcedure sp=new StoredProcedure("GetFinishingRetailerProductImages",this.Provider);
            return sp;
        }
        public StoredProcedure GetFixProductIDs(){
            StoredProcedure sp=new StoredProcedure("GetFixProductIDs",this.Provider);
            return sp;
        }
        public StoredProcedure GetGlatLngByCountryAndID(){
            StoredProcedure sp=new StoredProcedure("GetGlatLngByCountryAndID",this.Provider);
            return sp;
        }
        public StoredProcedure GetGoogleAdSuggest(){
            StoredProcedure sp=new StoredProcedure("GetGoogleAdSuggest",this.Provider);
            return sp;
        }
        public StoredProcedure GetHotSearch(){
            StoredProcedure sp=new StoredProcedure("GetHotSearch",this.Provider);
            return sp;
        }
        public StoredProcedure GetHotSearch2(){
            StoredProcedure sp=new StoredProcedure("GetHotSearch2",this.Provider);
            return sp;
        }
        public StoredProcedure GetImportMapNotSupportedCategory(){
            StoredProcedure sp=new StoredProcedure("GetImportMapNotSupportedCategory",this.Provider);
            return sp;
        }
        public StoredProcedure GetLatestExpertReview(){
            StoredProcedure sp=new StoredProcedure("GetLatestExpertReview",this.Provider);
            return sp;
        }
        public StoredProcedure GetLatestExpertReviewAU(){
            StoredProcedure sp=new StoredProcedure("GetLatestExpertReviewAU",this.Provider);
            return sp;
        }
        public StoredProcedure GetList(){
            StoredProcedure sp=new StoredProcedure("GetList",this.Provider);
            return sp;
        }
        public StoredProcedure GetMembershipInfoByRetailerId(){
            StoredProcedure sp=new StoredProcedure("GetMembershipInfoByRetailerId",this.Provider);
            return sp;
        }
        public StoredProcedure GetMonthInvoice(){
            StoredProcedure sp=new StoredProcedure("GetMonthInvoice",this.Provider);
            return sp;
        }
        public StoredProcedure GetMostPopularCategoriesByTime(){
            StoredProcedure sp=new StoredProcedure("GetMostPopularCategoriesByTime",this.Provider);
            return sp;
        }
        public StoredProcedure GetMostPopularProduct(){
            StoredProcedure sp=new StoredProcedure("GetMostPopularProduct",this.Provider);
            return sp;
        }
        public StoredProcedure GetMostPopularProductsGroupList(){
            StoredProcedure sp=new StoredProcedure("GetMostPopularProductsGroupList",this.Provider);
            return sp;
        }
        public StoredProcedure GetNewsletterUserLists(){
            StoredProcedure sp=new StoredProcedure("GetNewsletterUserLists",this.Provider);
            return sp;
        }
        public StoredProcedure GetOnlineRetailerProductCountByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetOnlineRetailerProductCountByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetPPCAccountByPPCTransactionId(){
            StoredProcedure sp=new StoredProcedure("GetPPCAccountByPPCTransactionId",this.Provider);
            return sp;
        }
        public StoredProcedure GetPPCBudgetHistoryByPPCMemberId(){
            StoredProcedure sp=new StoredProcedure("GetPPCBudgetHistoryByPPCMemberId",this.Provider);
            return sp;
        }
        public StoredProcedure GetPPCPPCMemberByStartdate(){
            StoredProcedure sp=new StoredProcedure("GetPPCPPCMemberByStartdate",this.Provider);
            return sp;
        }
        public StoredProcedure GetPPCTransactionByCreatedon(){
            StoredProcedure sp=new StoredProcedure("GetPPCTransactionByCreatedon",this.Provider);
            return sp;
        }
        public StoredProcedure GetPriceMeExpertAverageRating(){
            StoredProcedure sp=new StoredProcedure("GetPriceMeExpertAverageRating",this.Provider);
            return sp;
        }
        public StoredProcedure GetPriceMeExpertAverageRatingTF(){
            StoredProcedure sp=new StoredProcedure("GetPriceMeExpertAverageRatingTF",this.Provider);
            return sp;
        }
        public StoredProcedure GetPriceMeUserAverageRating(){
            StoredProcedure sp=new StoredProcedure("GetPriceMeUserAverageRating",this.Provider);
            return sp;
        }
        public StoredProcedure getProductAccessoriesByCategoryID(){
            StoredProcedure sp=new StoredProcedure("getProductAccessoriesByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductAttributeIDAndTitleByCategoryID(){
            StoredProcedure sp=new StoredProcedure("GetProductAttributeIDAndTitleByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductByCategoryID(){
            StoredProcedure sp=new StoredProcedure("GetProductByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductByCategoryIDAndRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetProductByCategoryIDAndRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductByRetailerIDAndCategoryID(){
            StoredProcedure sp=new StoredProcedure("GetProductByRetailerIDAndCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductCountByCategoryIdAtt(){
            StoredProcedure sp=new StoredProcedure("GetProductCountByCategoryIdAtt",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductCountByCategoryIdTypeId(){
            StoredProcedure sp=new StoredProcedure("GetProductCountByCategoryIdTypeId",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductForIcecat(){
            StoredProcedure sp=new StoredProcedure("GetProductForIcecat",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductForIcecatByCateAndBrand(){
            StoredProcedure sp=new StoredProcedure("GetProductForIcecatByCateAndBrand",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductFromRetailerTrackByUserIP(){
            StoredProcedure sp=new StoredProcedure("GetProductFromRetailerTrackByUserIP",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductImagesByPageIndex(){
            StoredProcedure sp=new StoredProcedure("GetProductImagesByPageIndex",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductReview(){
            StoredProcedure sp=new StoredProcedure("GetProductReview",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductReviewByReviewID(){
            StoredProcedure sp=new StoredProcedure("GetProductReviewByReviewID",this.Provider);
            return sp;
        }
        public StoredProcedure GetProductUserReviewsByCountryID(){
            StoredProcedure sp=new StoredProcedure("GetProductUserReviewsByCountryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRecentReviews(){
            StoredProcedure sp=new StoredProcedure("GetRecentReviews",this.Provider);
            return sp;
        }
        public StoredProcedure getRetailerBooksByRetailerID(){
            StoredProcedure sp=new StoredProcedure("getRetailerBooksByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerByPPCMemberId(){
            StoredProcedure sp=new StoredProcedure("GetRetailerByPPCMemberId",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerCostReport(){
            StoredProcedure sp=new StoredProcedure("GetRetailerCostReport",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerCrawlerInfoByCountry(){
            StoredProcedure sp=new StoredProcedure("GetRetailerCrawlerInfoByCountry",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerCrawlerInfoByFrequencytypeID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerCrawlerInfoByFrequencytypeID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerCrawlerInfoByModifyOn(){
            StoredProcedure sp=new StoredProcedure("GetRetailerCrawlerInfoByModifyOn",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerIdByProductId(){
            StoredProcedure sp=new StoredProcedure("GetRetailerIdByProductId",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerIdByProductIdCountryId(){
            StoredProcedure sp=new StoredProcedure("GetRetailerIdByProductIdCountryId",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerIdByRetailerProductId(){
            StoredProcedure sp=new StoredProcedure("GetRetailerIdByRetailerProductId",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerIdByRetailerProductIdCountryId(){
            StoredProcedure sp=new StoredProcedure("GetRetailerIdByRetailerProductIdCountryId",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductByProductId(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductByProductId",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductByProductIdTF(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductByProductIdTF",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductCategoryCount(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductCategoryCount",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductCount(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductCount",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductCountByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductCountByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductForURLCheckedByRetaierID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductForURLCheckedByRetaierID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductInPriceMeReportF(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductInPriceMeReportF",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductInPriceMeReportS(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductInPriceMeReportS",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductInPriceMeReportT(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductInPriceMeReportT",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerProductModifiedon(){
            StoredProcedure sp=new StoredProcedure("GetRetailerProductModifiedon",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerReviewByCountryID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerReviewByCountryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerReviewDetailByCountryID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerReviewDetailByCountryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailersByCountryCrawlType(){
            StoredProcedure sp=new StoredProcedure("GetRetailersByCountryCrawlType",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailersFetcherByCountry(){
            StoredProcedure sp=new StoredProcedure("GetRetailersFetcherByCountry",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerByAlid(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerByAlid",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerByCategoryID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerByCategoryIDTest(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerByCategoryIDTest",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerClicksByRetailerId(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerClicksByRetailerId",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerCost(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerCost",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerCostByCateID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerCostByCateID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerCostByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerCostByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerInvoiceByAlid(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerInvoiceByAlid",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerInvoiceByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerInvoiceByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerTrackerInvoiceByRetailerIDAndAlid(){
            StoredProcedure sp=new StoredProcedure("GetRetailerTrackerInvoiceByRetailerIDAndAlid",this.Provider);
            return sp;
        }
        public StoredProcedure GetRetailerUserName(){
            StoredProcedure sp=new StoredProcedure("GetRetailerUserName",this.Provider);
            return sp;
        }
        public StoredProcedure GetReviewsCatgory(){
            StoredProcedure sp=new StoredProcedure("GetReviewsCatgory",this.Provider);
            return sp;
        }
        public StoredProcedure GetReviewSourceIDNameByProductID(){
            StoredProcedure sp=new StoredProcedure("GetReviewSourceIDNameByProductID",this.Provider);
            return sp;
        }
        public StoredProcedure GetSameProductNameCountByCategoryID(){
            StoredProcedure sp=new StoredProcedure("GetSameProductNameCountByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetSubAllCateRetailerTracker(){
            StoredProcedure sp=new StoredProcedure("GetSubAllCateRetailerTracker",this.Provider);
            return sp;
        }
        public StoredProcedure GetSubAllCateRetailerTrackerByCateID(){
            StoredProcedure sp=new StoredProcedure("GetSubAllCateRetailerTrackerByCateID",this.Provider);
            return sp;
        }
        public StoredProcedure GetSubAllCateRetailerTrackerTest(){
            StoredProcedure sp=new StoredProcedure("GetSubAllCateRetailerTrackerTest",this.Provider);
            return sp;
        }
        public StoredProcedure GetToCrawlRetailerCount(){
            StoredProcedure sp=new StoredProcedure("GetToCrawlRetailerCount",this.Provider);
            return sp;
        }
        public StoredProcedure GetTop5RetailerTrackerByCategoryID(){
            StoredProcedure sp=new StoredProcedure("GetTop5RetailerTrackerByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure GetTop5RetailerTrackProduct(){
            StoredProcedure sp=new StoredProcedure("GetTop5RetailerTrackProduct",this.Provider);
            return sp;
        }
        public StoredProcedure GetTopAllRetailerTrackUserIP(){
            StoredProcedure sp=new StoredProcedure("GetTopAllRetailerTrackUserIP",this.Provider);
            return sp;
        }
        public StoredProcedure GetTopRecentReviews(){
            StoredProcedure sp=new StoredProcedure("GetTopRecentReviews",this.Provider);
            return sp;
        }
        public StoredProcedure GetTopRetailerTrackCategory(){
            StoredProcedure sp=new StoredProcedure("GetTopRetailerTrackCategory",this.Provider);
            return sp;
        }
        public StoredProcedure GetTopRetailerTrackCategoryByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetTopRetailerTrackCategoryByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetTopRetailerTrackProduct(){
            StoredProcedure sp=new StoredProcedure("GetTopRetailerTrackProduct",this.Provider);
            return sp;
        }
        public StoredProcedure GetTopRetailerTrackProductByRetailerID(){
            StoredProcedure sp=new StoredProcedure("GetTopRetailerTrackProductByRetailerID",this.Provider);
            return sp;
        }
        public StoredProcedure GetTopRetailerTrackUserIP(){
            StoredProcedure sp=new StoredProcedure("GetTopRetailerTrackUserIP",this.Provider);
            return sp;
        }
        public StoredProcedure GetWelcomeContent(){
            StoredProcedure sp=new StoredProcedure("GetWelcomeContent",this.Provider);
            return sp;
        }
        public StoredProcedure HelpGetAllRetailerProudcts(){
            StoredProcedure sp=new StoredProcedure("HelpGetAllRetailerProudcts",this.Provider);
            return sp;
        }
        public StoredProcedure HelpGetRetailerProudcts(){
            StoredProcedure sp=new StoredProcedure("HelpGetRetailerProudcts",this.Provider);
            return sp;
        }
        public StoredProcedure ImportFeedImages(){
            StoredProcedure sp=new StoredProcedure("ImportFeedImages",this.Provider);
            return sp;
        }
        public StoredProcedure InsertCategoryLog(){
            StoredProcedure sp=new StoredProcedure("InsertCategoryLog",this.Provider);
            return sp;
        }
        public StoredProcedure InsertTreepodiaVideo(){
            StoredProcedure sp=new StoredProcedure("InsertTreepodiaVideo",this.Provider);
            return sp;
        }
        public StoredProcedure MergeDeleteProductByID(){
            StoredProcedure sp=new StoredProcedure("MergeDeleteProductByID",this.Provider);
            return sp;
        }
        public StoredProcedure New_GetCategoryProductsByCID(){
            StoredProcedure sp=new StoredProcedure("New_GetCategoryProductsByCID",this.Provider);
            return sp;
        }
        public StoredProcedure New_GetCategoryProductsByCID2(){
            StoredProcedure sp=new StoredProcedure("New_GetCategoryProductsByCID2",this.Provider);
            return sp;
        }
        public StoredProcedure New_GetCategoryProductsByCID3(){
            StoredProcedure sp=new StoredProcedure("New_GetCategoryProductsByCID3",this.Provider);
            return sp;
        }
        public StoredProcedure p_compdb(){
            StoredProcedure sp=new StoredProcedure("p_compdb",this.Provider);
            return sp;
        }
        public StoredProcedure RedirectforRetailerproduct(){
            StoredProcedure sp=new StoredProcedure("RedirectforRetailerproduct",this.Provider);
            return sp;
        }
        public StoredProcedure RedirectforRetailerproduct_AU(){
            StoredProcedure sp=new StoredProcedure("RedirectforRetailerproduct_AU",this.Provider);
            return sp;
        }
        public StoredProcedure RedirectforRetailerproduct_HK(){
            StoredProcedure sp=new StoredProcedure("RedirectforRetailerproduct_HK",this.Provider);
            return sp;
        }
        public StoredProcedure RedirectforRetailerproduct_ID(){
            StoredProcedure sp=new StoredProcedure("RedirectforRetailerproduct_ID",this.Provider);
            return sp;
        }
        public StoredProcedure RedirectforRetailerproduct_MY(){
            StoredProcedure sp=new StoredProcedure("RedirectforRetailerproduct_MY",this.Provider);
            return sp;
        }
        public StoredProcedure RedirectforRetailerproduct_NZ(){
            StoredProcedure sp=new StoredProcedure("RedirectforRetailerproduct_NZ",this.Provider);
            return sp;
        }
        public StoredProcedure RedirectforRetailerproduct_PH(){
            StoredProcedure sp=new StoredProcedure("RedirectforRetailerproduct_PH",this.Provider);
            return sp;
        }
        public StoredProcedure RedirectforRetailerproduct_SG(){
            StoredProcedure sp=new StoredProcedure("RedirectforRetailerproduct_SG",this.Provider);
            return sp;
        }
        public StoredProcedure RedirectforRetailerproduct_TH(){
            StoredProcedure sp=new StoredProcedure("RedirectforRetailerproduct_TH",this.Provider);
            return sp;
        }
        public StoredProcedure RedirectforRetailerproduct_VN(){
            StoredProcedure sp=new StoredProcedure("RedirectforRetailerproduct_VN",this.Provider);
            return sp;
        }
        public StoredProcedure selectIndexProductByCategotyID(){
            StoredProcedure sp=new StoredProcedure("selectIndexProductByCategotyID",this.Provider);
            return sp;
        }
        public StoredProcedure selectProductByCategotyIDAndProductNameFuzzy(){
            StoredProcedure sp=new StoredProcedure("selectProductByCategotyIDAndProductNameFuzzy",this.Provider);
            return sp;
        }
        public StoredProcedure selectProductByProductNameFuzzy(){
            StoredProcedure sp=new StoredProcedure("selectProductByProductNameFuzzy",this.Provider);
            return sp;
        }
        public StoredProcedure selectProductImages(){
            StoredProcedure sp=new StoredProcedure("selectProductImages",this.Provider);
            return sp;
        }
        public StoredProcedure selectProductOnAccessoiesByProductNameFuzzy(){
            StoredProcedure sp=new StoredProcedure("selectProductOnAccessoiesByProductNameFuzzy",this.Provider);
            return sp;
        }
        public StoredProcedure selectRetailerProductInTempAutoByCategoryID(){
            StoredProcedure sp=new StoredProcedure("selectRetailerProductInTempAutoByCategoryID",this.Provider);
            return sp;
        }
        public StoredProcedure SetDefaultImage(){
            StoredProcedure sp=new StoredProcedure("SetDefaultImage",this.Provider);
            return sp;
        }
        public StoredProcedure spDeleteProduct(){
            StoredProcedure sp=new StoredProcedure("spDeleteProduct",this.Provider);
            return sp;
        }
        public StoredProcedure spDeleteRetailerProduct(){
            StoredProcedure sp=new StoredProcedure("spDeleteRetailerProduct",this.Provider);
            return sp;
        }
        public StoredProcedure spGetProductCount(){
            StoredProcedure sp=new StoredProcedure("spGetProductCount",this.Provider);
            return sp;
        }
        public StoredProcedure spGetProductDescriptorByCategory(){
            StoredProcedure sp=new StoredProcedure("spGetProductDescriptorByCategory",this.Provider);
            return sp;
        }
        public StoredProcedure spGetUserIsAdministrator(){
            StoredProcedure sp=new StoredProcedure("spGetUserIsAdministrator",this.Provider);
            return sp;
        }
        public StoredProcedure Test_PartnersFeedGetCategory(){
            StoredProcedure sp=new StoredProcedure("Test_PartnersFeedGetCategory",this.Provider);
            return sp;
        }
        public StoredProcedure Test_PartnersFeedGetCategoryByCategoryId(){
            StoredProcedure sp=new StoredProcedure("Test_PartnersFeedGetCategoryByCategoryId",this.Provider);
            return sp;
        }
        public StoredProcedure TestDuplicateProNameInSameCate(){
            StoredProcedure sp=new StoredProcedure("TestDuplicateProNameInSameCate",this.Provider);
            return sp;
        }
        public StoredProcedure Update_Buildinginde_needdata(){
            StoredProcedure sp=new StoredProcedure("Update_Buildinginde_needdata",this.Provider);
            return sp;
        }
        public StoredProcedure Update_PriceMe_ExpertAverageRating(){
            StoredProcedure sp=new StoredProcedure("Update_PriceMe_ExpertAverageRating",this.Provider);
            return sp;
        }
        public StoredProcedure Update_PriceMe_ExpertAverageRating_PricemeScoreis0(){
            StoredProcedure sp=new StoredProcedure("Update_PriceMe_ExpertAverageRating_PricemeScoreis0",this.Provider);
            return sp;
        }
        public StoredProcedure Update_PriceMe_ExpertAverageRating_PricemeScoreis0AU(){
            StoredProcedure sp=new StoredProcedure("Update_PriceMe_ExpertAverageRating_PricemeScoreis0AU",this.Provider);
            return sp;
        }
        public StoredProcedure Update_PriceMe_ExpertAverageRatingAU(){
            StoredProcedure sp=new StoredProcedure("Update_PriceMe_ExpertAverageRatingAU",this.Provider);
            return sp;
        }
        public StoredProcedure Update_Product_DescAndAttr(){
            StoredProcedure sp=new StoredProcedure("Update_Product_DescAndAttr",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateAUReplication(){
            StoredProcedure sp=new StoredProcedure("UpdateAUReplication",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateGlatLngByCountry(){
            StoredProcedure sp=new StoredProcedure("UpdateGlatLngByCountry",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateHKReplication(){
            StoredProcedure sp=new StoredProcedure("UpdateHKReplication",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateIDReplication(){
            StoredProcedure sp=new StoredProcedure("UpdateIDReplication",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateMYReplication(){
            StoredProcedure sp=new StoredProcedure("UpdateMYReplication",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateNZReplication(){
            StoredProcedure sp=new StoredProcedure("UpdateNZReplication",this.Provider);
            return sp;
        }
        public StoredProcedure UpdatePHReplication(){
            StoredProcedure sp=new StoredProcedure("UpdatePHReplication",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateProductCategory(){
            StoredProcedure sp=new StoredProcedure("UpdateProductCategory",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateProductClickTemp(){
            StoredProcedure sp=new StoredProcedure("UpdateProductClickTemp",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateRetailerProductCategory(){
            StoredProcedure sp=new StoredProcedure("UpdateRetailerProductCategory",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateSGReplication(){
            StoredProcedure sp=new StoredProcedure("UpdateSGReplication",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateTHReplication(){
            StoredProcedure sp=new StoredProcedure("UpdateTHReplication",this.Provider);
            return sp;
        }
        public StoredProcedure UpdateVNReplication(){
            StoredProcedure sp=new StoredProcedure("UpdateVNReplication",this.Provider);
            return sp;
        }
        public StoredProcedure WriteLog(){
            StoredProcedure sp=new StoredProcedure("WriteLog",this.Provider);
            return sp;
        }
	
	}
	
}
 