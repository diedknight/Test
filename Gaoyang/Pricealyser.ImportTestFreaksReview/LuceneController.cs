using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using System.IO;
using System.Xml;

namespace Pricealyser.ImportTestFreaksReview
{
    public static class LuceneController
    {
        const int MAXDOCS = 100000;

        static IndexSearcher expertReviewIndex;

        public static IndexSearcher ExpertReviewIndex
        {
            get { return LuceneController.expertReviewIndex; }
        }

        static IndexSearcher averageIndex;

        public static IndexSearcher AverageIndex
        {
            get { return LuceneController.averageIndex; }
        }

        static string AverageIndexPath;
        static string ExpertReviewIndexPath;

        static LuceneController()
        {
            LoadLucenePath();
            LoadIndexSearcher();
        }

        #region Load Lucene Path
        static void LoadLucenePath()
        {
            string luceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["LuceneConfigPath"].ToString();
            string rootIndexPath = (GetConfigValue(luceneConfigPath, "ExpertReviewIndexPath"));
            ExpertReviewIndexPath = rootIndexPath + "ExpertReview";
            AverageIndexPath = rootIndexPath + "ExpertAverage";
        }

        static string GetConfigValue(string configFilePath, string appKey)
        {
            if (!File.Exists(configFilePath))
                return string.Empty;
            System.Xml.XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(configFilePath);

            XmlNode configNode = null;
            foreach (XmlNode xmlNode in xmlDoc.ChildNodes)
            {
                if (xmlNode.Name.Equals("configuration", StringComparison.InvariantCultureIgnoreCase))
                {
                    configNode = xmlNode;
                    break;
                }
            }

            XmlNode appSettings = null;
            foreach (XmlNode xmlNode in configNode.ChildNodes)
            {
                if (xmlNode.Name.Equals("appsettings", StringComparison.InvariantCultureIgnoreCase))
                {
                    appSettings = xmlNode;
                    break;
                }
            }

            foreach (XmlNode xmlNode in appSettings.ChildNodes)
            {
                if (xmlNode.Attributes[0].Value == appKey)
                {
                    return xmlNode.Attributes[1].Value;
                }
            }
            return string.Empty;
        }
        #endregion

        #region Load Index Searcher
        static void LoadIndexSearcher()
        {
            expertReviewIndex = GetIndexSearcher(ExpertReviewIndexPath);
            averageIndex = GetIndexSearcher(AverageIndexPath);
        }

        static IndexSearcher GetIndexSearcher(string indexDirectory)
        {
            if (System.IO.Directory.Exists(indexDirectory) && System.IO.File.Exists(indexDirectory + "\\segments.gen"))
            {
                IndexSearcher indexSearcher = null;

                Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexDirectory));
                indexSearcher = new IndexSearcher(fsDirectory, true);

                return indexSearcher;
            }
            else
            {
                System.Console.WriteLine(indexDirectory + " have no index file!");
            }
            return null;
        }
        #endregion

        #region Searcher
        //Get FeatureScore
        public static bool GetFeatureScore(int pid)
        {
            bool isFS = false;
            TopFieldDocs docs = null;
            Sort sort = new Sort();
            BooleanQuery query = new BooleanQuery();

            TermQuery termQuery = new TermQuery(new Term("ProductID", pid.ToString()));
            query.Add(termQuery, Occur.MUST);
            
            docs = LuceneController.AverageIndex.Search(query, null, MAXDOCS, sort);

            //for (int i = 0; i < docs.ScoreDocs.Count(); i++)
            //{
            //    Document hitDoc = LuceneController.AverageIndex.Doc(i);
            //    string featureScore = hitDoc.Get("FeatureScore").ToString();
            //    if (!string.IsNullOrEmpty(featureScore))
            //        isFS = true;
            //    break;
            //}

            return isFS;
        }

        //Get ExpertReview
        public static bool GetExpertReview(int pid, int sid)
        {
            bool isER = false;
            TopFieldDocs docs = null;
            Sort sort = new Sort();
            BooleanQuery query = new BooleanQuery();

            TermQuery termQuery = new TermQuery(new Term("ProductID", pid.ToString()));
            query.Add(termQuery, Occur.MUST);

            termQuery = new TermQuery(new Term("SourceID", sid.ToString()));
            query.Add(termQuery, Occur.MUST);

            docs = LuceneController.ExpertReviewIndex.Search(query, null, MAXDOCS, sort);

            //if(docs.ScoreDocs.Count() > 0)
            //    isER = true;

            return isER;
        }
        #endregion
    }
}
