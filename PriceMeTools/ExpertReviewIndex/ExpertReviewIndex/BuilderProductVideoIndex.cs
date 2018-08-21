using ExpertReviewIndex.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Lucene.Net.Index;
using Lucene.Net.Analysis;
using Lucene.Net.Util;
using Lucene.Net.Documents;
using System.IO;

namespace ExpertReviewIndex
{
    public class BuilderProductVideoIndex
    {
        IndexWriter indexWriter;
        string indexPath = string.Empty;
        string date = string.Empty;

        public bool Builder(string _date)
        {
            try
            {
                date = _date;
                BuildIndexLog.WriterLog("Begin Build Product Video Index...    at: " + DateTime.Now);

                CreateIndexWriter();

                WriterProductVideo();

                BuildIndexLog.WriterLog("Create Index...    at: " + DateTime.Now);
                
                indexWriter.Dispose();

                BuildIndexLog.WriterLog("End...    at: " + DateTime.Now);

                return true;
            }
            catch (Exception ex)
            {
                BuildIndexLog.WriterLog("Error: " + ex.Message + ex.StackTrace);
            }

            return false;
        }

        private void WriterProductVideo()
        {
            BuildIndexLog.WriterLog("Get Product Video and writer index...   at: " + DateTime.Now);

            using (IDataReader dr = SqlServerController.DBController.GetProductVideo())
            {
                int i = 0;

                while (dr.Read())
                {
                    ProductVideo video = new ProductVideo();
                    video.ProductID = int.Parse(dr["ProductID"].ToString());
                    video.Url = dr["Url"].ToString();
                    video.Thumbnail = dr["Thumbnail"].ToString();
                    WriterIndex(video, i);
                    i++;
                }
            }
        }

        private void WriterIndex(ProductVideo er, int i)
        {
            Document doc = new Document();

            doc.Add(new Int32Field("ProductID", er.ProductID, Field.Store.YES));
            doc.Add(new StringField("Url", er.Url, Field.Store.YES));
            doc.Add(new StringField("Thumbnail", er.Thumbnail, Field.Store.YES));

            while (true)
            {
                try
                {
                    indexWriter.AddDocument(doc);
                    break;
                }
                catch { }
            }
        }

        private void CreateIndexWriter()
        {
            indexPath = SiteConfig.AppSettings("IndexRootPath") + @"\" + date + @"\ProductVideo";
            if (!System.IO.Directory.Exists(indexPath))
                System.IO.Directory.CreateDirectory(indexPath);
            Analyzer analyzer = new Lucene.Net.Analysis.Core.WhitespaceAnalyzer(LuceneVersion.LUCENE_48);
            Lucene.Net.Store.FSDirectory ramDir = Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo(indexPath));

            IndexWriterConfig iwc = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
            iwc.OpenMode = OpenMode.CREATE;
            iwc.RAMBufferSizeMB = 300;
            iwc.MaxBufferedDocs = 2000;
            iwc.MaxThreadStates = IndexWriterConfig.DEFAULT_MAX_THREAD_STATES;

            indexWriter = new IndexWriter(ramDir, iwc);
        }
    }
}
