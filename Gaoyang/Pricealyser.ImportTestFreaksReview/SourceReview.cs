using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pricealyser.ImportTestFreaksReview
{
    public class SourceReview
    {
        //private int _testFreaksId;
        //public int TestFreaksId
        //{
        //    get { return _testFreaksId; }
        //    set { _testFreaksId = value; }
        //}

        private int _sourceId;
        public int SourceId
        {
            get { return _sourceId; }
            set { _sourceId = value; }
        }

        //private List<int> _pidList;
        //public List<int> PidList
        //{
        //    get { return _pidList; }
        //    set { _pidList = value; }
        //}

        private string _sourceName;
        public string SourceName
        {
            get { return _sourceName; }
            set { _sourceName = value; }
        }

        private string _sourceLogo;
        public string SourceLogo
        {
            get { return _sourceLogo; }
            set { _sourceLogo = value; }
        }

        private string _summary;
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        private decimal _score;
        public decimal Score
        {
            get { return _score; }
            set { _score = value; }
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        private string _pros;
        public string Pros
        {
            get { return _pros; }
            set { _pros = value; }
        }

        private string _cons;
        public string Cons
        {
            get { return _cons; }
            set { _cons = value; }
        }

        private string _date;
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private string _extract;
        public string Extract
        {
            get { return _extract; }
            set { _extract = value; }
        }

        private string _author;

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        private bool _IsExpertReview;

        public bool IsExpertReview
        {
            get { return _IsExpertReview; }
            set { _IsExpertReview = value; }
        }
    }
}
