using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class HitsInfo
    {
        readonly Lucene.Net.Search.IndexSearcher _searcher;
        readonly Lucene.Net.Search.TopDocs _topDocs;
        readonly int _resultCount;
        private readonly string[] _kwList;

        public int ResultCount
        {
            get { return this._resultCount; }
        }

        public string[] KeywordsList
        {
            get { return this._kwList; }
        }

        public Lucene.Net.Documents.Document GetDocument(int index)
        {
            return _searcher.Doc(_topDocs.ScoreDocs[index].Doc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        public Lucene.Net.Documents.Document GetDocument(int index, string[] fieldNames)
        {
            //ISet<string> s = new HashSet<string>();
            //foreach(string fs in fieldNames)
            //{
            //    s.Add(fs);
            //}

            //return _searcher.Doc(_topDocs.ScoreDocs[index].Doc, s);
            return _searcher.Doc(_topDocs.ScoreDocs[index].Doc, new Lucene.Net.Documents.MapFieldSelector(fieldNames));
        }

        public HitsInfo(Lucene.Net.Search.IndexSearcher searcher, Lucene.Net.Search.TopDocs topDocs)
        {
            this._searcher = searcher;
            this._topDocs = topDocs;
            this._resultCount = _topDocs.ScoreDocs.Length;
        }

        public HitsInfo(Lucene.Net.Search.IndexSearcher searcher, Lucene.Net.Search.TopDocs topDocs, string[] kwList)
        {
            this._searcher = searcher;
            this._topDocs = topDocs;
            this._resultCount = _topDocs.ScoreDocs.Length;
            this._kwList = kwList;
        }
    }
}