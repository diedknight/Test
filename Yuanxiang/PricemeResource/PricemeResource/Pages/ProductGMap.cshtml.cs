using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PricemeResource.Data;
using PricemeResource.Logic;

namespace PricemeResource.Pages
{
    public class ProductGMapModel : PageModel
    {
        public bool IsExit = false;
        public string scriptString;
        public string scriptOnclick;

        public int CountryId;
        public int ProductId;

        public string CenterLat = "";
        public string CenterLng = "";
        public string zoom = WebSiteConfig.GMapZoom;

        public PricemeResource.Data.ResourcesData resData;
        int i = 0;

        public void OnGet()
        {
            ProductId = Utility.GetIntParameter("pid", this.Request);
            CountryId = Utility.GetIntParameter("cid", this.Request);

            resData = WebSiteConfig.dicResources[CountryId];
            scriptString = CreateClientScript();
        }

        private string CreateClientScript()
        {
            List<RetailerProductItem> rpis = ProductController.GetRetailerProductItems(ProductId, resData.DbInfo);
            List<int> retailerIDs = new List<int>();
            foreach (RetailerProductItem rp in rpis)
            {
                if (!retailerIDs.Contains(rp.RetailerId))
                {
                    if (rp.RetailerId == 0) continue;
                    retailerIDs.Add(rp.RetailerId);
                }
            }
            if (this.CenterLat == "" || this.CenterLng == "")//my_position 为空时的默认
            {
                CenterLat = "-36.84929745904190";
                CenterLng = "174.76637255743400";
            }

            StringBuilder storesStr = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            if (retailerIDs.Count == 0) IsExit = false;

            foreach (int rid in retailerIDs)
            {
                RetailerCache retailer = RetailerController.GetRetailerDeep(rid);
                if (retailer == null || retailer.RetailerId == 0) { continue; }
                string isShow = "true";
                
                var glats = RetailerController.GetRetailerGLatLng(rid, resData.DbInfo);

                if (glats == null || glats.Count == 0) { continue; }

                List<RetailerProductItem> retailerPList = rpis.Where(r => r.RetailerId == rid).ToList();
                
                string retailerInfoUrl = retailer.RetailerURL;

                string descriptionLogo = "";

                string logo = "", online_store_name = "";
                decimal price = 0;
                string cheapest = "";

                string minPriceStr = "";
                string description = "";
                
                if (RetailerController.IsPPcRetailer(rid))
                {
                    if (!string.IsNullOrEmpty(retailer.LogoFile))
                    {
                        logo = retailer.LogoFile.Insert(retailer.LogoFile.LastIndexOf('.'), "_s");
                        logo = logo.Replace("/images/RetailerImages/", "");
                        logo = WebSiteConfig.ImageWebsite + "/images/RetailerImages/" + logo;
                        logo = "<img src='" + logo + "' alt='" + retailer.RetailerName + "' />";
                    }
                    else
                        logo = "<img src='" + WebSiteConfig.ImageWebsite + "/images/RetailerImages/no_iamge12040.gif' alt='" + retailer.RetailerName + "' />";

                    descriptionLogo += "<div style=' height:30px;'>" + logo + "</div>";
                    logo = "\"" + logo + "\"";
                }
                else
                    descriptionLogo += "<div style=' height:30px;color: #000000;font-weight: bold;font-size:16px;'>" + retailer.RetailerName + "</div>";

                online_store_name = "\"" + retailer.RetailerName + "\"";

                string cheapestRpId = "";

                bool isNoLink = CheckIsNoLink(retailer.RetailerId);
                if (retailerPList.Count > 0)
                {
                    description = setRetailerDesc(retailerPList[0].RpList, out price, out cheapest, description, retailer, isNoLink, descriptionLogo);
                    minPriceStr = Utility.FormatPrice((double)price, CountryId, resData.CurrentCulture);
                    price = 0;
                    cheapestRpId = cheapest;
                    cheapest = "";

                    if (description.EndsWith(", "))
                        description = description.Substring(0, description.LastIndexOf(", "));

                    description += "</span><br />";
                }
                else
                    description += descriptionLogo;

                string markerImg = WebSiteConfig.IframeResource + "/MarkerImages?isNoLink=" + isNoLink + "&page=product&RetailerId=" + retailer.RetailerId + "&RetailerName=" + retailer.RetailerName + "&retailerLogoPath=" + retailer.LogoFile + "&minPrice=" + ProcPriceString(minPriceStr);

                string starImageUrl = "";

                string retailerReviewInfo = "";

                string retailerReviewUrl = "";
                
                foreach (var glatlng in glats)
                {
                    if (!String.IsNullOrEmpty(glatlng.GLat) && !String.IsNullOrEmpty(glatlng.Glng))
                    {

                        string popupDes = GetPopupDes(description, glatlng, isNoLink, rid, starImageUrl, retailerReviewInfo, retailerReviewUrl);

                        if (!string.IsNullOrEmpty(online_store_name) && !string.IsNullOrEmpty(popupDes))
                        {
                            i++;
                            storesStr.Append("\n{");
                            storesStr.Append("\"online_store_name\":" + online_store_name + ",\n");
                            storesStr.Append("\"longitude\":" + glatlng.Glng + ",\n");
                            storesStr.Append("\"latitude\":" + glatlng.GLat + ",\n");
                            storesStr.Append("\"markerImg\":\"" + markerImg + "\",\n");
                            storesStr.Append("\"isShow\":" + isShow + ",\n");
                            storesStr.Append("\"description\":\"");
                            storesStr.Append(popupDes);
                            storesStr.Append("\"},\n");
                        }
                    }
                }
            }

            
            string sStr = storesStr.ToString();
            if (!string.IsNullOrEmpty(sStr))
                sStr = sStr.TrimEnd(',');
            else
                i = 0;
            sb.Append("\nvar data={\n\"isMobile\":false,\n\"isTablet\":false,\n\"my_position\":{\n\"lat\":" + CenterLat + ",\n\"long\":" + CenterLng + "},\n\"physical_stores\":[" + sStr + "],\n\"product_id\":\"" + ProductId + "\"};");

            sb.Append("\n");
            sb.Append("var z=" + zoom + ";");
            sb.Append("\n");
            sb.Append("var map;");
            sb.Append("\n");
            sb.Append("var infowindow;");
            sb.Append("\n");
            sb.Append("var ps_markers = {};");
            sb.Append("\n");
            sb.Append("var marker_ids = [];");
            sb.Append("function map_initialize() {");
            sb.Append("\n");
            sb.Append("var myOptions = {");
            sb.Append("\n");
            sb.Append("zoom: z,");
            sb.Append("mapTypeId: google.maps.MapTypeId.ROADMAP};");
            sb.Append("\n");
            sb.Append("map = new google.maps.Map(document.getElementById('map_canvas'), myOptions);");
            sb.Append("\n");
            // sb.Append("map.setCenter(new google.maps.LatLng(data.my_position.lat, data.my_position.long));");
            sb.Append("\n");
            if (i != 0)
            {
                IsExit = true;
            }
            sb.Append("var nbr_stores = 0;");
            sb.Append("\n");
            sb.Append("var map_bounds = new google.maps.LatLngBounds();");
            sb.Append("\n");
            sb.Append("var my_location = new google.maps.LatLng(data.my_position.lat, data.my_position.long);");
            sb.Append("\n");
            sb.Append(" map_bounds.extend(my_location); ");

            sb.Append(" var min=180;");
            sb.Append("var minLatlng;");
            sb.Append(" var count=0;");
            
            sb.Append("infowindow = new google.maps.InfoWindow({content: 'Init'});");
            sb.Append("\n");
            sb.Append("jQuery.each(data.physical_stores, function(i, store) {");
            sb.Append("\n");
            sb.Append("if (store.latitude && store.isShow==true) {");
            sb.Append("\n");
            sb.Append("nbr_stores++;");
            sb.Append("\n");
            sb.Append("var marker_location = new google.maps.LatLng(store.latitude, store.longitude);");
            sb.Append("\n");
            sb.Append("if(Math.abs(Math.abs(store.longitude)-Math.abs(data.my_position.long))<min){"); sb.Append("\n");
            sb.Append("min =Math.abs(Math.abs(store.longitude)-Math.abs(data.my_position.long)); minLatlng=marker_location;}");

            sb.Append("var icon = new google.maps.MarkerImage(store.markerImg,");
            sb.Append("\n");
            sb.Append("new google.maps.Size(74, 57),");
            sb.Append("\n"); sb.Append("null,");
            sb.Append("\n");
            sb.Append("new google.maps.Point(0, 0));");
            sb.Append("\n");
            sb.Append("var opts = {");
            sb.Append("\n");
            sb.Append("'map': map,");
            sb.Append("\n");
            sb.Append("'position':marker_location,");
            sb.Append("\n");
            sb.Append("'icon': icon,");
            sb.Append("\n");
            sb.Append("'clickable': true,");
            sb.Append("\n");
            sb.Append("'zIndex': -1,");
            sb.Append("\n");
            sb.Append("'title': store.online_store_name");
            sb.Append("\n"); sb.Append("};");
            sb.Append("\n");
            sb.Append("var marker = new google.maps.Marker(opts);");
            sb.Append("\n");
            sb.Append("google.maps.event.addListener(marker, 'click', function() {");
            sb.Append("\n");
            sb.Append("var html = '<div style=\"width:240px; height:160px;\" ><font face=\"sans-serif\" size=\"1\" sytle=\"mergin\">'+store.description+'</font></div>';");
            sb.Append("\n");
            sb.Append("infowindow.setContent(html);");
            sb.Append("\n");
            sb.Append("infowindow.open(map, marker);");
            sb.Append("\n"); sb.Append("});");
            sb.Append("\n");
            sb.Append("ps_markers[store.physical_store_id] = marker;");
            sb.Append("\n");
            sb.Append("marker_ids.push(store.physical_store_id);");
            sb.Append("\n"); sb.Append("}");
            sb.Append("\n"); sb.Append("});");
            sb.Append("\n");

            sb.Append("map_bounds.extend(minLatlng);");

            sb.Append("var sw = map_bounds.getSouthWest();");
            sb.Append("\n");
            sb.Append("var ne = map_bounds.getNorthEast();");
            sb.Append("\n");
            sb.Append("var new_sw = new google.maps.LatLng(sw.lat()-0.005, sw.lng()-0.005);");
            sb.Append("\n");
            sb.Append("var new_ne = new google.maps.LatLng(ne.lat()+0.005, ne.lng()-0.005);");
            sb.Append("\n");
            sb.Append("map_bounds.extend(new_sw);");
            sb.Append("\n");
            sb.Append("map_bounds.extend(new_ne);");
            sb.Append("\n");


            sb.Append("map.fitBounds(map_bounds);");
            sb.Append("map.setCenter(map_bounds.getCenter());");


            sb.Append("\n");
            sb.Append("}");
            sb.Append("\n");

            sb.Append(@"(function pricemeJsIsReady() {
            if (typeof priceCom$ == 'undefined') {
                setTimeout(pricemeJsIsReady, 10);
                return;
            }");

            sb.Append("$(document).ready(function(){");
            sb.Append("\n");
            sb.Append("map_initialize();");
            sb.Append("\n");
            sb.Append("});");

            sb.Append("})();");

            sb.Append("</script>");

            return sb.ToString();
        }

        protected bool CheckIsNoLink(int retailerId)
        {
            bool isNoLink = false;

            if (!RetailerController.IsPPcRetailer(retailerId))
            {
                isNoLink = true;
            }
            return isNoLink;
        }

        private string setRetailerDesc(List<RetailerProductNew> RpList, out decimal price, out string cheapest, string description, RetailerCache retailer, bool isNoLink, string descriptionLogo)
        {
            price = 0; cheapest = "";
            foreach (RetailerProductNew rp in RpList)
            {
                #region
                string VSOnclickScript = string.Format(WebSiteConfig.ScriptFormat_Static, Guid.NewGuid(), "null",
                                                rp.RetailerId, 0, rp.RetailerPrice.ToString("0.00"), retailer.RetailerId, rp.RetailerPrice.ToString("0.00"));

                string functionString = "\nfunction clickrp" + rp.RetailerProductId + "() { " + VSOnclickScript + "};";
                scriptOnclick += functionString;

                if (price == 0)
                {
                    price = rp.RetailerPrice;
                    cheapest = rp.RetailerProductId.ToString();
                }
                if (rp.RetailerPrice < price)
                {
                    price = rp.RetailerPrice;
                    cheapest = rp.RetailerProductId.ToString();
                }
                if (!description.Contains(descriptionLogo))
                    description += descriptionLogo;

                if (!description.Contains(resData.String_Price + ": "))
                    description += "<span  style='font-size:12px;'>" + resData.String_Price + ": ";
                if (isNoLink)
                {
                    description += ProcPriceStringStyle(Utility.FormatPrice((double)rp.RetailerPrice, CountryId, resData.CurrentCulture)) + ", ";
                }
                else
                {

                    string RetailerProductURL = resData.TrackRootUrl + "/rr.aspx?rpid=" + rp.RetailerProductId;

                    description += "<a target='_blank' onclick='clickrp" + rp.RetailerProductId + "()'  href='" + RetailerProductURL + "' rel='nofollow'>" + ProcPriceStringStyle(Utility.FormatPrice((double)rp.RetailerPrice, CountryId, resData.CurrentCulture)) + "</a>" + ", ";
                }
                #endregion
            }
            return description;
        }

        protected string ProcPriceStringStyle(string priceString)
        {
            string newPriceString = priceString.Replace("<span class='priceSymbol'>", "<span class='priceSymbol' style='font-size:12px;'>").Replace("<span class='priceSpan'>", "<span class='priceSpan' style='font-size:12px;font-weight:normal;'>").Trim();
            return newPriceString;
        }

        protected string ProcPriceString(string priceString)
        {
            string newPriceString = priceString.Replace("<span class='priceSymbol'>", "").Replace("</span>", "").Replace("<span class='priceSpan'>", "").Trim();
            return newPriceString;
        }

        private string GetPopupDes(string description, StoreGLatLng glatlng, bool isNoLink, int rid, string starImageUrl, string retailerReviewInfo, string retailerReviewUrl)
        {
            StringBuilder popupDes = new StringBuilder(description);

            if (!string.IsNullOrEmpty(glatlng.LocationName)) popupDes.Append(glatlng.LocationName + "<br />");
            if (!string.IsNullOrEmpty(glatlng.DescriptionNew)) popupDes.Append("<span>" + glatlng.DescriptionNew.Replace("<br />", "<br/>").Replace(" ", "&nbsp;").Replace("\n", "<br/>").Replace("\t", "&nbsp;").Replace("\r", "<br/>") + "</span><br/>");
            if (!string.IsNullOrEmpty(glatlng.PostalCity)) popupDes.Append(glatlng.PostalCity);
            if (!string.IsNullOrEmpty(glatlng.PostalCity) && !string.IsNullOrEmpty(glatlng.Postcode)) popupDes.Append("&nbsp;");
            if (!string.IsNullOrEmpty(glatlng.Postcode)) popupDes.Append(glatlng.Postcode);

            if (!isNoLink)
                popupDes.Append("&nbsp;<a href='" + resData.GoogleMapUrl + "/maps?saddr=" + CenterLat + "," + CenterLng + "&daddr=" + glatlng.GLat + "," + glatlng.Glng + "' target='_blank'> " + resData.GetDirections + " <a/>");

            return popupDes.ToString();
        }
    }
}