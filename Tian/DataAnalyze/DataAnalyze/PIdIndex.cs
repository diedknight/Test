using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyze
{
    public class PIdIndex
    {
        private static string _indexPath = "";
        private static Lucene.Net.Search.IndexSearcher _indexSearcher = null;
        private static Lucene.Net.Index.IndexWriter _indexWriter = null;

        public static int Count
        {
            get
            {
                return ProductCountIndex.Count;
            }
        }

        static PIdIndex()
        {
            _indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IndexData", "PIdIndex");
            if (!Directory.Exists(_indexPath))
            {
                Directory.CreateDirectory(_indexPath);
            }            
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

        public static void Add(int pId, int wordCount)
        {
            if (_indexWriter == null)
            {
                _indexWriter = CreateIndexWriter(_indexPath);
            }

            Lucene.Net.Documents.Document document = new Lucene.Net.Documents.Document();
            document.Add(new Lucene.Net.Documents.Field("PId", pId.ToString(), Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Lucene.Net.Documents.Field("WordCount", wordCount.ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            _indexWriter.AddDocument(document);

        }

        //public static void AddOrUpdate(int pId, int wordCount)
        //{
        //    if (_indexWriter == null)
        //    {
        //        _indexWriter = CreateIndexWriter(_indexPath);
        //    }

        //    Lucene.Net.Documents.Document document = new Lucene.Net.Documents.Document();
        //    document.Add(new Lucene.Net.Documents.Field("PId", pId.ToString(), Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
           
        //    int count = GetWordCount(pId);                      
        //    if (count != 0)
        //    {                                
        //        wordCount += count;
        //        document.Add(new Lucene.Net.Documents.Field("WordCount", wordCount.ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
        //        Lucene.Net.Index.Term term = new Lucene.Net.Index.Term("PId", pId.ToString());
        //        _indexWriter.UpdateDocument(term, document);                
        //    }
        //    else
        //    {
        //        document.Add(new Lucene.Net.Documents.Field("WordCount", wordCount.ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
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

        public static int GetWordCount(int pId)
        {
            if (_indexSearcher == null)
            {
                Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(_indexPath));
                _indexSearcher = new Lucene.Net.Search.IndexSearcher(fsDirectory, true);
            }

            Lucene.Net.Search.TermQuery termQuery = new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("PId", pId.ToString()));

            var doc = _indexSearcher.Search(termQuery, 1);

            if (doc.TotalHits == 0) return 0;

            int count = 0;
            for (int i = 0; i < doc.ScoreDocs.Length; i++)
            {
                count += Convert.ToInt32(_indexSearcher.Doc(doc.ScoreDocs[i].Doc).Get("WordCount"));
            }

            return count;
        }

    }
}
