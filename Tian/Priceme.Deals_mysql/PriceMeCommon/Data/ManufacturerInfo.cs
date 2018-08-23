using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
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

        public ManufacturerInfo() { }
        public ManufacturerInfo(ManufacturerInfo manufacturerInfo)
        {
            this.BrandsURL = manufacturerInfo.BrandsURL;
            this.Description = manufacturerInfo.Description;
            this.ImagePath = manufacturerInfo.ImagePath;
            this.IsHidden = manufacturerInfo.IsHidden;
            this.IsPopular = manufacturerInfo.IsPopular;
            this.Location = manufacturerInfo.Location;
            this.ManufacturerID = manufacturerInfo.ManufacturerID;
            this.ManufacturerName = manufacturerInfo.ManufacturerName;
            this.ProductCount = manufacturerInfo.ProductCount;
            this.URL = manufacturerInfo.URL;
        }
    }
}