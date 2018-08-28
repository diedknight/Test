using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SiteMap.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace SiteMap
{
    public static class ProductSearcher
    {
        private static Dictionary<string, IndexSearcher> dicSearcher;
        private static Dictionary<int, string> parentCategorys;

        static ProductSearcher()
        {
            string luceneConfigPath = SiteConfig.AppSettings("LuceneConfigPath");
            string indexKey = SiteConfig.AppSettings("LuceneConfigKey");
            string luceneIndexPath = GetLuceneConfigValue(luceneConfigPath, indexKey);
            luceneIndexPath = luceneIndexPath + "AllCategoriesProduct\\";

            dicSearcher = new Dictionary<string, IndexSearcher>();
            if (System.IO.Directory.Exists(luceneIndexPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(luceneIndexPath);
                DirectoryInfo[] subDirInfos = dirInfo.GetDirectories();
                foreach (DirectoryInfo di in subDirInfos)
                {
                    IndexSearcher searcher = GetIndexSearcher(di.FullName);
                    dicSearcher.Add(di.Name, searcher);
                }
            }

            SiteMap.SqlController.MysqlDBController.LoadParentCategory(out parentCategorys);
        }

        public static IndexSearcher GetIndexSearcher(string indexDirectory)
        {
            if (System.IO.Directory.Exists(indexDirectory) && File.Exists(indexDirectory + "\\segments.gen"))
            {
                IndexSearcher indexSearcher = null;
                FSDirectory fsDirectory = FSDirectory.Open(new DirectoryInfo(indexDirectory));
                Lucene.Net.Index.IndexReader indexReader = Lucene.Net.Index.DirectoryReader.Open(fsDirectory);
                indexSearcher = new IndexSearcher(indexReader);

                return indexSearcher;
            }

            return null;
        }

        private static IndexSearcher GetSearcherByCategoryID(int categoryID)
        {
            int rootId = SiteMap.SqlController.MysqlDBController.GetParentCategory(categoryID);

            if (parentCategorys.ContainsKey(rootId))
            {
                string rootCategoryName = parentCategorys[rootId];
                if (dicSearcher.ContainsKey(rootCategoryName))
                    return dicSearcher[rootCategoryName];
            }

            return null;
        }

        public static List<ProductCatalog> GetProducts(int cid)
        {
            List<ProductCatalog> pcs = new List<ProductCatalog>();

            IndexSearcher indexSearcher = GetSearcherByCategoryID(cid);
            if (indexSearcher == null) return pcs;

            Sort sort = new Sort();

            TopFieldDocs topFieldDocs = null;
            BooleanQuery booleanQuery = new BooleanQuery();

            Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
            Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(cid, 0, btRef);
            TermQuery termQuery = new TermQuery(new Term("CategoryID", btRef));
            booleanQuery.Add(termQuery, Occur.MUST);

            topFieldDocs = indexSearcher.Search(booleanQuery, null, int.MaxValue, sort);

            for (int i = 0; i < topFieldDocs.ScoreDocs.Length; i++)
            {
                Document hitDoc = indexSearcher.Doc(topFieldDocs.ScoreDocs[i].Doc);

                ProductCatalog pc = new ProductCatalog();
                pc.ProductID = hitDoc.Get("ProductID");
                pc.ProductName = hitDoc.Get("ProductName");

                pcs.Add(pc);
            }

            return pcs;
        }

        public static string GetLuceneConfigValue(string configPath, string appKey)
        {
            if (!File.Exists(configPath))
                return string.Empty;
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(configPath);

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
    }
}
