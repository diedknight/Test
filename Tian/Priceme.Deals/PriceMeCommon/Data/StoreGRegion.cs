using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class StoreGRegion
    {
        private int _gRegionID;
        public int GRegionID
        {
            get { return _gRegionID; }
            set { _gRegionID = value; }
        }

        private int _regionID;
        public int RegionID
        {
            get { return _regionID; }
            set { _regionID = value; }
        }

        private string _regionCenterGLat = "";
        public string RegionCenterGLat
        {
            get { return _regionCenterGLat; }
            set { _regionCenterGLat = value; }
        }

        private string _regionCenterGLng = "";
        public string RegionCenterGLng
        {
            get { return _regionCenterGLng; }
            set { _regionCenterGLng = value; }
        }

        private int _zoom;
        public int Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        private string _RegionCode = "";

        public string RegionCode
        {
            get { return _RegionCode; }
            set { _RegionCode = value; }
        }
    }
}
