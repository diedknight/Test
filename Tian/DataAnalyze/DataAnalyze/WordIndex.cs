using Service.Infrastructure.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyze
{
    public class WordIndex
    {
        private static string _indexPath = "";
        private static Lucene.Net.Search.IndexSearcher _indexSearcher = null;
        private static Lucene.Net.Index.IndexWriter _indexWriter = null;
        private static ISerialize _jsonSerialize = null;

        //public static int Count
        //{
        //    get
        //    {
        //        if (_indexSearcher == null)
        //        {
        //            Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(_indexPath));
        //            _indexSearcher = new Lucene.Net.Search.IndexSearcher(fsDirectory, true);
        //        }
        //        return _indexSearcher.MaxDoc;
        //    }
        //}

        static WordIndex()
        {
            _indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IndexData", "WordIndex");
            if (!Directory.Exists(_indexPath))
            {
                Directory.CreateDirectory(_indexPath);
            }

            _jsonSerialize = new JsonSerialize();
        }

        private static Lucene.Net.Index.IndexWriter CreateIndexWriter(string indexPath)
        {
            Lucene.Net.Analysis.Standard.StandardAnalyzer standardAnalyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30, new HashSet<string>());
            Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexPath));
            Lucene.Net.Index.IndexWriter indexWriter = null;

            if (fsDirectory.FileExists("segments.gen"))
            {
                indexWriter = new Lucene.Net.Index.IndexWriter(fsDirectory, standardAnalyzer, false, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);
            }
            else
            {
                indexWriter = new Lucene.Net.Index.IndexWriter(fsDirectory, standardAnalyzer, true, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);
            }

            return indexWriter;
        }

        public static void Add(string word, Dictionary<int, int> hitPIdCount)
        {
            if (_indexWriter == null)
            {
                _indexWriter = CreateIndexWriter(_indexPath);
            }

            Lucene.Net.Documents.Document document = new Lucene.Net.Documents.Document();
            document.Add(new Lucene.Net.Documents.Field("Word", word, Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Lucene.Net.Documents.Field("Json", _jsonSerialize.Serialize(hitPIdCount), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));

            _indexWriter.AddDocument(document);
        }

        //public static void AddOrUpdate(string word, Dictionary<int, int> hitPIdCount)
        //{
        //    if (_indexWriter == null)
        //    {
        //        _indexWriter = CreateIndexWriter(_indexPath);
        //    }

        //    Lucene.Net.Documents.Document document = new Lucene.Net.Documents.Document();
        //    document.Add(new Lucene.Net.Documents.Field("Word", word, Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            
        //    var dic = GetHitPId(word);
        //    if (dic.Count != 0)
        //    {
        //        foreach (var item in hitPIdCount)
        //        {
        //            if (dic.ContainsKey(item.Key)) dic[item.Key] += item.Value;
        //            else dic.Add(item.Key, item.Value);
        //        }

        //        document.Add(new Lucene.Net.Documents.Field("Json", _jsonSerialize.Serialize(dic), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));

        //        Lucene.Net.Index.Term term = new Lucene.Net.Index.Term("Word", word);
        //        _indexWriter.UpdateDocument(term, document);
        //    }
        //    else
        //    {
        //        document.Add(new Lucene.Net.Documents.Field("Json", _jsonSerialize.Serialize(hitPIdCount), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
        //        _indexWriter.AddDocument(document);                
        //    }
        //}

        public static void Save()
        {
            if (_indexWriter != null)
            {
                _indexWriter.Commit();
                _indexWriter.Optimize();
                _indexWriter.Dispose();
                _indexWriter = null;
            }

            if (_indexSearcher != null)
            {
                _indexSearcher.Dispose();
                _indexSearcher = null;
            }
        }        

        public static Dictionary<int, int> GetHitPId(string word)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();

            if (_indexSearcher == null)
            {
                Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(_indexPath));
                _indexSearcher = new Lucene.Net.Search.IndexSearcher(fsDirectory, true);
            }

            Lucene.Net.Search.TermQuery termQuery = new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("Word", word));

            var doc = _indexSearcher.Search(termQuery, 1);

            if (doc.TotalHits == 0) return dic;

            for (int i = 0; i < doc.ScoreDocs.Length; i++)
            {
                Dictionary<int, int> tempDic = _jsonSerialize.Deserialize<Dictionary<int, int>>(_indexSearcher.Doc(doc.ScoreDocs[i].Doc).Get("Json"));

                foreach (var item in tempDic)
                {
                    if (dic.ContainsKey(item.Key))
                        dic[item.Key] += item.Value;
                    else
                        dic.Add(item.Key, item.Value);
                }
            }            

            return dic;
        }        

    }
}
