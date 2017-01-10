using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using PriceMeDBA;

namespace PriceMeCommon.BusinessLogic
{
    public class MapController
    {
        private static List<CSK_Store_UserLocation> locations;

        static MapController()
        {
            
        }

        public static List<CSK_Store_UserLocation> Locations
        {
            get { return locations; }
        }

        public static void Load()
        {
            locations = CSK_Store_UserLocation.All().ToList();
        }

        public static List<Store_GLatLng> GetRegionRetailersGMapInfo(int region, List<int> retailerIDList)
        {
            List<Store_GLatLng> glats =
                Store_GLatLng.Find(g => g.Region == region).Where(g => retailerIDList.Contains(g.Retailerid ?? 0)).
                    ToList();

            return glats;
        }

        public static List<Store_GLatLng> GetRegionRetailersGMapInfo(int retailerId)
        {
            List<Store_GLatLng> glats = Store_GLatLng.Find(g => (g.Retailerid ?? 0) == retailerId).ToList();

            return glats;
        }

        public static string GetColor(CSK_Store_Retailer retailer)
        {
            string color = "";
            switch (retailer.StoreType)
            {
                case 1:
                    color = "red";
                    break;
                case 2:
                    color = "orange";
                    break;
                case 3:
                    color = "green";
                    break;
                case 4:
                    color = "blue";
                    break;
            }
            return color;
        }
    }
}