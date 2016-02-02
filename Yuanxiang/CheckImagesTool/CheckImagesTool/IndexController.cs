using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckImagesTool
{
    public class IndexController
    {
        private static string _indexPath = "";
        private static Lucene.Net.Search.IndexSearcher _indexSearcher = null;
        private static Lucene.Net.Index.IndexWriter _indexWriter = null;

        static IndexController()
        {
            _indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IndexData");
            if (!Directory.Exists(_indexPath))
            {
                Directory.CreateDirectory(_indexPath);
            }
        }

        private static Lucene.Net.Index.IndexWriter CreateIndexWriter(string indexPath)
        {
            Lucene.Net.Analysis.Standard.StandardAnalyzer standardAnalyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
            Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexPath));
            Lucene.Net.Index.IndexWriter indexWriter = new Lucene.Net.Index.IndexWriter(fsDirectory, standardAnalyzer, true, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);
            indexWriter.SetMergeFactor(5000);
            indexWriter.SetMaxBufferedDocs(4000);
            return indexWriter;
        }

        public static void Add(string data)
        {
            if (_indexWriter == null)
            {
                if (_indexSearcher != null)
                {
                    _indexSearcher.Close();
                    _indexSearcher = null;
                }

                _indexWriter = CreateIndexWriter(_indexPath);
            }

            Lucene.Net.Documents.Document document = new Lucene.Net.Documents.Document();

            var field = new Lucene.Net.Documents.Field("Image", data.ToLower(), Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.NOT_ANALYZED);
            document.Add(field);

            _indexWriter.AddDocument(document);
        }

        public static bool Compare(string fileName)
        {
            if (_indexSearcher == null)
            {
                if (_indexWriter != null)
                {
                    _indexWriter.Optimize();
                    _indexWriter.Close();
                    _indexWriter = null;
                }

                Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(_indexPath));
                _indexSearcher = new Lucene.Net.Search.IndexSearcher(fsDirectory, true);                
            }

            Lucene.Net.Search.TermQuery termQuery = new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("Image", fileName.ToLower()));

            var doc = _indexSearcher.Search(termQuery, 100000);

            return doc.TotalHits > 0;
        }

    }
}
