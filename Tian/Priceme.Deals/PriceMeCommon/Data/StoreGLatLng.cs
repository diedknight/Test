using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class StoreGLatLng
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _retailerid;
        public int Retailerid
        {
            get { return _retailerid; }
            set { _retailerid = value; }
        }

        private string _gLat;
        public string GLat
        {
            get { return _gLat; }
            set { _gLat = value; }
        }

        private string _glng;
        public string Glng
        {
            get { return _glng; }
            set { _glng = value; }
        }

        private int _region;
        public int Region
        {
            get { return _region; }
            set { _region = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        private string _Postcode;

        public string Postcode
        {
            get { return _Postcode; }
            set { _Postcode = value; }
        }
        private string _PostalCity;

        public string PostalCity
        {
            get { return _PostalCity; }
            set { _PostalCity = value; }
        }
        private string _email;

        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _Phone;

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        private string _LocationName;

        public string LocationName
        {
            get { return _LocationName; }
            set { _LocationName = value; }
        }

        private string _DescriptionNew;

        public string DescriptionNew
        {
            get { return _DescriptionNew; }
            set { _DescriptionNew = value; }
        }
 }
}
