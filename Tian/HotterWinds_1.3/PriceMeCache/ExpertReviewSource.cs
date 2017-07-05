using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    public class ExpertReviewSource
    {
        private int _sourceID;

        public int SourceID
        {
            get { return _sourceID; }
            set { _sourceID = value; }
        }
        private string _webSiteName;
        public string WebSiteName
        {
            get { return _webSiteName; }
            set { _webSiteName = value; }
        }

        private string _logoFile;

        public string LogoFile
        {
            get { return _logoFile; }
            set { _logoFile = value; }
        }
        private int _countryID;
        public int CountryID
        {
            get { return _countryID; }
            set { _countryID = value; }
        }
        private string _homePage;
        public string HomePage
        {
            get { return _homePage; }
            set { _homePage = value; }
        }
    }
}
