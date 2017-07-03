using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class StoreUserLocation
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _userLocation;
        public string UserLocation
        {
            get { return _userLocation; }
            set { _userLocation = value; }
        }

        private bool _isNorthIsland;
        public bool IsNorthIsland
        {
            get { return _isNorthIsland; }
            set { _isNorthIsland = value; }
        }
    }
}
