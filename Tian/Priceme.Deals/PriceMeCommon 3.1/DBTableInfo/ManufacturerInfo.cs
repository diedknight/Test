using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.DBTableInfo
{
    public class ManufacturerInfo
    {
        public int ManufacturerID { get; set; }
        public string ManufacturerName { get; set; }
        public string Description { get; set; }
        public bool IsPopular { get; set; }
        public string ImagePath { get; set; }
        public bool IsHidden { get; set; }
        public int ProductCount { get; set; }
        public string Location { get; set; }

        public string URL { get; set; }
        public string BrandsURL { get; set; }
        public string BrandsURLAU { get; set; }
        public string BrandsURLPH { get; set; }
        public string BrandsURLHK { get; set; }
        public string BrandsURLSG { get; set; }
        public string BrandsURLID { get; set; }
        public string BrandsURLMY { get; set; }
    }
}