using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyze
{
    public class ProductCountIndex
    {
        private static string _indexPath = "";
        private static Lucene.Net.Search.IndexSearcher _indexSearcher = null;
        private static Lucene.Net.Index.IndexWriter _indexWriter = null;

        private static HashSet<int> _set = null;

        private static int _count = 0;

        public static int Count
        {
            get
            {
                if (_set != null) _set.Clear();

                if (_count == 0)
                {
                    if (_indexSearcher == null)
                    {
                        Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(_indexPath));
                        _indexSearcher = new Lucene.Net.Search.IndexSearcher(fsDirectory, true);
                    }

                    _count = _indexSearcher.MaxDoc;

                    _indexSearcher.Dispose();
                    _indexSearcher = null;
                }

                return _count;
            }
        }

        static ProductCountIndex()
        {
            _indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IndexData", "ProductCountIndex");
            if (!Directory.Exists(_indexPath))
            {
                Directory.CreateDirectory(_indexPath);
            }

            _set = new HashSet<int>();
        }


        private static Lucene.Net.Index.IndexWriter CreateIndexWriter(string indexPath)
        {
            Lucene.Net.Analysis.Standard.StandardAnalyzer standardAnalyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexPath));
            Lucene.Net.Index.IndexWriter indexWriter = new Lucene.Net.Index.IndexWriter(fsDirectory, standardAnalyzer, true, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);

            return indexWriter;
        }

        public static void Add(int pId)
        {
            _set.Add(pId);
        }

        public static void Save()
        {
            if (_indexWriter == null)
            {
                _indexWriter = CreateIndexWriter(_indexPath);
            }

            foreach (var item in _set)
            {
                Lucene.Net.Documents.Document document = new Lucene.Net.Documents.Document();
                document.Add(new Lucene.Net.Documents.Field("PId", item.ToString(), Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));                
                _indexWriter.AddDocument(document);
            }

            _indexWriter.Commit();
            _indexWriter.Optimize();
            _indexWriter.Dispose();
            _indexWriter = null;
        }

    }
}
