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
    public class RetailerGMapModel : PageModel
    {
        public int CountryId;
        public int RetailerId;
        public PricemeResource.Data.ResourcesData resData;

        public bool IsExist = false;
        public bool isDes = false;
        public string CenterLat = "";
        public string CenterLng = "";

        public string scriptString;

        public List<StoreGLatLng> glatlngC = null;
        public RetailerCache retailer;


        public void OnGet()
        {
            RetailerId = Utility.GetIntParameter("rid", this.Request);
            CountryId = Utility.GetIntParameter("cid", this.Request);
            resData = WebSiteConfig.dicResources[CountryId];

            retailer = RetailerController.GetRetailerDeep(RetailerId);
            glatlngC = RetailerController.GetRetailerGLatLng(RetailerId, resData.DbInfo);
            glatlngC = glatlngC.OrderBy(gc => gc.PostalCity).ToList();
            if (glatlngC != null && glatlngC.Count > 0)
            {
                IsExist = true;

                scriptString = CreateClientScript();
            }
        }

        private string CreateClientScript()
        {
            if (this.CenterLat == "" || this.CenterLng == "")//my_position 为空时的默认
            {
                CenterLat = "-36.84929745904190";
                CenterLng = "174.76637255743400";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");

            string retailerInfoUrl = Utility.GetRetailerInfoUrl(retailer.RetailerId, retailer.RetailerName);
            string storesStr = "";
            if (glatlngC != null && glatlngC.Count > 0)
            {
                foreach (StoreGLatLng glatlng in glatlngC)
                {
                    if (!string.IsNullOrEmpty(glatlng.Glng) && !string.IsNullOrEmpty(glatlng.GLat))
                    {
                        string description = "";
                        if (retailer != null)
                        {
                            bool isNoLink = CheckIsNoLink(retailer.RetailerId);
                            string descriptionLogo = "";
                            string descLogo = "";
                            if (RetailerController.IsPPcRetailer(glatlng.Retailerid))
                            {
                                if (!string.IsNullOrEmpty(retailer.LogoFile))
                                {
                                    descLogo = retailer.LogoFile.Insert(retailer.LogoFile.LastIndexOf('.'), "_s");
                                    descLogo = descLogo.Replace("/images/RetailerImages/", "");
                                    descLogo = WebSiteConfig.ImageWebsite + "/images/RetailerImages/" + descLogo;
                                    descLogo = "<img src='" + descLogo + "' />";
                                }
                                else
                                {
                                    descLogo = "<img src='" + WebSiteConfig.ImageWebsite + "/images/RetailerImages/no_iamge12040.gif'  />";
                                }
                                descriptionLogo += "<div style=' height:30px; margin-bottom: 5px;'>" + descLogo + "</div>";
                            }
                            else
                            {
                                descriptionLogo += retailer.RetailerName;
                            }

                            description += "<div style='color: #3074d3;font-weight: bold;'>" + descriptionLogo + "</div>";
                            //256   1449
                            if (!String.IsNullOrEmpty(glatlng.LocationName)) description += glatlng.LocationName + "<br />";
                            if (!String.IsNullOrEmpty(glatlng.DescriptionNew)) description += "<span>" + glatlng.DescriptionNew.Replace("<br />", "<br/>").Replace(" ", "&nbsp;").Replace("\n", "<br/>").Replace("\t", "&nbsp;").Replace("\r", "<br/>") + "</span><br/>";

                            if (!String.IsNullOrEmpty(glatlng.PostalCity)) description += glatlng.PostalCity;
                            if (!String.IsNullOrEmpty(glatlng.PostalCity) && !String.IsNullOrEmpty(glatlng.Postcode)) description += "&nbsp;";
                            if (!String.IsNullOrEmpty(glatlng.Postcode)) description += glatlng.Postcode;
                            if (!String.IsNullOrEmpty(glatlng.PostalCity) || !String.IsNullOrEmpty(glatlng.Postcode)) description += "<br />";

                            description += ("<a href='" + resData.GoogleMapUrl + "/maps?saddr=" + CenterLat + "," + CenterLng
                                + "&daddr=" + glatlng.GLat + "," + glatlng.Glng + "' target='_blank'> " + resData.GetDirections + " <a/>");

                            string markerImg = WebSiteConfig.IframeResource + "/MarkerImages?isNoLink=" + CheckIsNoLink(retailer.RetailerId) + "&page=retailer&RetailerId=" + retailer.RetailerId + "&RetailerName=" + retailer.RetailerName + "&retailerLogoPath=" + retailer.LogoFile;

                            storesStr += "{";
                            storesStr += "\"online_store_name\":\"" + retailer.RetailerName + "\",";
                            storesStr += "\"longitude\":" + glatlng.Glng + ",";
                            storesStr += "\"latitude\":" + glatlng.GLat + ",";
                            storesStr += "\"markerImg\":\"" + markerImg + "\",";
                            storesStr += "\"description\":\"" + description + "\"";
                            storesStr += "},";
                        }
                    }
                }
            }
            if (storesStr != "")
                storesStr = storesStr.Substring(0, storesStr.LastIndexOf(","));

            sb.Append("var data={\"isMobile\":false,\"isTablet\":false,\"my_position\":{\"lat\":" + CenterLat + ",\"long\":" + CenterLng + "},\"physical_stores\":[" + storesStr + "]};");

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
            sb.Append("zoom: 12,");
            sb.Append("mapTypeId: google.maps.MapTypeId.ROADMAP};");
            sb.Append("\n");
            sb.Append("map = new google.maps.Map(document.getElementById('map_canvas'), myOptions);");
            sb.Append("\n");

            sb.Append("var nbr_stores = 0;");
            sb.Append("\n");
            sb.Append("var map_bounds = new google.maps.LatLngBounds();");
            sb.Append("\n");
            sb.Append("var my_location = new google.maps.LatLng(data.my_position.lat, data.my_position.long);");
            sb.Append("\n");
            sb.Append("map_bounds.extend(my_location);");
            sb.Append("\n");
            sb.Append("infowindow = new google.maps.InfoWindow({content: 'Init'});");
            sb.Append("\n");
            sb.Append("jQuery.each(data.physical_stores, function(i, store) {");
            sb.Append("\n");
            sb.Append("if (store.latitude) {");
            sb.Append("\n");
            sb.Append("nbr_stores++;");
            sb.Append("\n");
            sb.Append("var marker_location = new google.maps.LatLng(store.latitude, store.longitude);");
            sb.Append("\n");
            sb.Append("var icon = new google.maps.MarkerImage(store.markerImg,");
            sb.Append("\n");
            sb.Append("new google.maps.Size(74, 57),");
            sb.Append("\n"); sb.Append("null,");
            sb.Append("\n");
            sb.Append("new google.maps.Point(1, 57));");
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
            sb.Append("if (nbr_stores<=3) map_bounds.extend(marker_location); ");
            sb.Append("\n");
            sb.Append("google.maps.event.addListener(marker, 'click', function() {");
            sb.Append("\n");
            sb.Append("var html = '<div style=\"width:210px; height:150px;\" ><font face=\"sans-serif\" size=\"1\" sytle=\"mergin\">'+store.description+'</font></div>';");
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
            sb.Append("var sw = map_bounds.getSouthWest();");
            sb.Append("\n");
            sb.Append("var ne = map_bounds.getNorthEast();");
            sb.Append("\n");
            sb.Append("var new_sw = new google.maps.LatLng(sw.lat()-0.1, sw.lng()-0.1);");
            sb.Append("\n");
            sb.Append("var new_ne = new google.maps.LatLng(ne.lat()+0.1, ne.lng()-0.1);");
            sb.Append("\n");
            sb.Append("map_bounds.extend(new_sw);");
            sb.Append("\n");
            sb.Append("map_bounds.extend(new_ne);");
            sb.Append("\n");
            sb.Append("map.fitBounds(map_bounds);");
            sb.Append("\n"); sb.Append("}");
            sb.Append("\n");
            sb.Append("$(document).ready(function(){");
            sb.Append("\n");
            sb.Append("map_initialize();");
            sb.Append("\n"); sb.Append("});");

            sb.Append(" </script>");
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
    }
}