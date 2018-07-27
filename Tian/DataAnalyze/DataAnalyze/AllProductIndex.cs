using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyze
{
    public class AllProductIndex
    {
        private static string _indexPath = "";
        private static Lucene.Net.Search.IndexSearcher _indexSearcher = null;
        private static Lucene.Net.Index.IndexWriter _indexWriter = null;

        static AllProductIndex()
        {
            _indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IndexData", "ProductIndex");
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

        public static void Add(string words, Tuple<int, int, string,int,string> pItem)
        {
            if (_indexWriter == null)
            {
                _indexWriter = CreateIndexWriter(_indexPath);
            }

            Lucene.Net.Documents.Document document = new Lucene.Net.Documents.Document();
            document.Add(new Lucene.Net.Documents.Field("Words", words, Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.ANALYZED));
            document.Add(new Lucene.Net.Documents.Field("PId", pItem.Item1.ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Lucene.Net.Documents.Field("ManufacturerID", pItem.Item2.ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Lucene.Net.Documents.Field("DefaultImage", pItem.Item3.ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Lucene.Net.Documents.Field("CategoryID", pItem.Item4.ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Lucene.Net.Documents.Field("ProductName", pItem.Item5, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));

            _indexWriter.AddDocument(document);
        }

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

        public static Dictionary<int, string> GetPIdScore(List<string> words)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            if (_indexSearcher == null)
            {
                Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(_indexPath));
                _indexSearcher = new Lucene.Net.Search.IndexSearcher(fsDirectory, true);
            }

            Lucene.Net.Search.BooleanQuery query = new Lucene.Net.Search.BooleanQuery();
            words.ForEach(item => { query.Add(new Lucene.Net.Search.TermQuery(new Lucene.Net.Index.Term("Words", item)), Lucene.Net.Search.Occur.SHOULD); });

            var doc = _indexSearcher.Search(query, _indexSearcher.MaxDoc);

            if (doc.TotalHits == 0) return dic;

            for (int i = 0; i < doc.ScoreDocs.Length; i++)
            {
                double score = 1 / (1 + Math.Pow(Math.E, -0.3 * doc.ScoreDocs[i].Score));

                int pid = Convert.ToInt32(_indexSearcher.Doc(doc.ScoreDocs[i].Doc).Get("PId"));
                string manId = _indexSearcher.Doc(doc.ScoreDocs[i].Doc).Get("ManufacturerID");
                string img = _indexSearcher.Doc(doc.ScoreDocs[i].Doc).Get("DefaultImage");
                string cid = _indexSearcher.Doc(doc.ScoreDocs[i].Doc).Get("CategoryID");
                string pName = _indexSearcher.Doc(doc.ScoreDocs[i].Doc).Get("ProductName");

                string value = pid + "|,|" + manId + "|,|" + score.ToString() + "|,|" + img + "|,|" + cid + "|,|" + pName;

                if (dic.ContainsKey(pid))
                {
                    dic[pid] = value;
                }
                else
                {
                    dic.Add(pid, value);
                }
            }

            return dic;
        }


    }
}
