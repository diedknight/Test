using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ExpertReviewIndex.Data;
using Lucene.Net.Index;
using Lucene.Net.Analysis;
using Lucene.Net.Util;
using Lucene.Net.Documents;
using System.IO;

namespace ExpertReviewIndex
{
    public class BuilderExpertReviewIndex
    {
        IndexWriter indexWriter;
        string indexPath = string.Empty;
        string date = string.Empty;

        public bool Builder(string _date)
        {
            try
            {
                date = _date;
                BuildIndexLog.WriterLog("Begin Builder Expert Review Index...    at: " + DateTime.Now);

                CreateIndexWriter();

                WriterExpertReview();

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

        private void WriterExpertReview()
        {
            BuildIndexLog.WriterLog("Get Expert Review and writer index...   at: " + DateTime.Now);

            using (IDataReader dr = SqlServerController.DBController.GetAllExpertReviewTF())
            {
                int i = 0;

                while (dr.Read())
                {
                    ExpertReview er = new ExpertReview();
                    er.ProductID = int.Parse(dr["ProductID"].ToString());
                    er.CategoryID = int.Parse(dr["CategoryID"].ToString());
                    er.PriceMeScore = double.Parse(dr["PriceMeScore"].ToString());
                    er.Description = dr["Description"].ToString();
                    er.Title = dr["Title"].ToString();
                    er.Pros = dr["Pros"].ToString();
                    er.Cons = dr["Cons"].ToString();
                    er.Verdict = dr["Verdict"].ToString();
                    er.SourceID = dr["SourceID"].ToString();
                    er.ReviewURL = dr["ReviewURL"].ToString();
                    er.ReviewDate = dr["ReviewDate"].ToString();
                    er.ManufacturerID = int.Parse(dr["ManufacturerID"].ToString());
                    er.IsExpertReview = bool.Parse(dr["IsExpertReview"].ToString());

                    if (er.SourceID == "156")
                    {
                        if (!string.IsNullOrEmpty(er.Pros))
                        {
                            string stringPros = string.Empty;
                            string[] temps = er.Pros.Split(',');
                            for (int k = (temps.Length - 1); k >= 0; k--)
                            {
                                stringPros += "<div class=\"pc\"><span class=\"glyphicon glyphicon-plus colorPros\"></span> " + temps[k].Trim() + "</div>";
                            }
                            er.Pros = stringPros;
                        }

                        if (!string.IsNullOrEmpty(er.Cons))
                        {
                            string stringCons = string.Empty;
                            string[] temps = er.Cons.Split(',');
                            for (int k = (temps.Length - 1); k >= 0; k--)
                            {
                                stringCons += "<div class=\"pc\"><span class=\"glyphicon glyphicon-minus colorCros\"></span> " + temps[k].Trim() + "</div>";
                            }
                            er.Cons = stringCons;
                        }
                    }

                    WriterIndex(er, i);
                    i++;
                }
            }
        }

        private void WriterIndex(ExpertReview er, int i)
        {
            Document doc = new Document();

            doc.Add(new Int32Field("ProductID", er.ProductID, Field.Store.YES));
            doc.Add(new Int32Field("CategoryID", er.CategoryID, Field.Store.YES));
            doc.Add(new Int32Field("ManufacturerID", er.ManufacturerID, Field.Store.YES));
            doc.Add(new StringField("Description", er.Description, Field.Store.YES));
            doc.Add(new StringField("Title", er.Title, Field.Store.YES));
            doc.Add(new StringField("Pros", er.Pros, Field.Store.YES));
            doc.Add(new StringField("Cons", er.Cons, Field.Store.YES));
            doc.Add(new StringField("Verdict", er.Verdict, Field.Store.YES));
            doc.Add(new StringField("SourceID", er.SourceID, Field.Store.YES));
            doc.Add(new StringField("ReviewURL", er.ReviewURL, Field.Store.YES));
            doc.Add(new StringField("ReviewDate", er.ReviewDate, Field.Store.YES));
            doc.Add(new StringField("IsExpertReview", er.IsExpertReview.ToString(), Field.Store.YES));

            float rating = 0;
            float.TryParse(er.PriceMeScore.ToString(), out rating);
            doc.Add(new SingleField("PriceMeScore", rating, Field.Store.YES));
            doc.Add(new Int32Field("SortField", i, Field.Store.YES));

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
            indexPath = SiteConfig.AppSettings("IndexRootPath") + @"\" + date + @"\ExpertReview";
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
