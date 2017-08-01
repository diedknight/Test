using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    [Serializable]
    public class GLatLngCache
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _Retailerid;

        public int Retailerid
        {
            get { return _Retailerid; }
            set { _Retailerid = value; }
        }
        private string _GLat;

        public string GLat
        {
            get { return _GLat; }
            set { _GLat = value; }
        }
        private string _Glng;

        public string Glng
        {
            get { return _Glng; }
            set { _Glng = value; }
        }
        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private string _Location;

        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
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
