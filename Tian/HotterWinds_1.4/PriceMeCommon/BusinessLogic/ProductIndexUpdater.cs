using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public class ProductIndexUpdater
    {
        public int UpdatedCount { get; private set; }
        List<ProductCatalog> mProductCatalgoList;
        string mProductIndexRootPath;
        
        public ProductIndexUpdater(List<ProductCatalog> pcList, string productIndexRootPath)
        {
            mProductCatalgoList = pcList;
            mProductIndexRootPath = productIndexRootPath;
        }

        public void Update()
        {
            UpdatedCount = 0;

            List<LuceneUpdateIndexInfo> luceneUpdateIndexInfo = GetAllProductsIndex(mProductIndexRootPath);

            try
            {
                foreach (ProductCatalog pc in mProductCatalgoList)
                {
                    Lucene.Net.Index.Term productIDTerm = new Lucene.Net.Index.Term("ProductID", pc.ProductID);

                    Lucene.Net.Search.TermQuery termQuery = new TermQuery(productIDTerm);
                    bool updated = false;
                    foreach (LuceneUpdateIndexInfo indexInfo in luceneUpdateIndexInfo)
                    {
                        Lucene.Net.Search.TopFieldDocs topFieldDocs = indexInfo.UpdateIndexSearcher.Search(termQuery, null, 1, new Lucene.Net.Search.Sort());
                        if (topFieldDocs.TotalHits > 0)
                        {
                            Document doc = indexInfo.UpdateIndexSearcher.Doc(topFieldDocs.ScoreDocs[0].Doc);
                            doc.RemoveField("BestPrice");
                            //doc.Add(new DoubleField("BestPrice", double.Parse(pc.BestPrice), Field.Store.YES));
                            NumericField bpField = new NumericField("BestPrice", Field.Store.YES, true);
                            bpField.SetDoubleValue(double.Parse(pc.BestPrice));
                            doc.Add(bpField);

                            indexInfo.UpdateIndexWriter.UpdateDocument(productIDTerm, doc);
                            updated = true;
                        }
                    }

                    if (updated)
                    {
                        UpdatedCount++;
                    }
                }
            }
            catch(Exception ex)
            {
                LogController.WriteException("Update index: " + ex.StackTrace);
            }

            foreach (LuceneUpdateIndexInfo indexInfo in luceneUpdateIndexInfo)
            {
                indexInfo.UpdateIndexSearcher.IndexReader.Dispose();
                indexInfo.UpdateIndexWriter.Dispose();
            }
        }

        private List<LuceneUpdateIndexInfo> GetAllProductsIndex(string needTopUpdateIndexPath)
        {
            List<LuceneUpdateIndexInfo> indexList = new List<LuceneUpdateIndexInfo>();

            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(needTopUpdateIndexPath);
            if (dirInfo.Exists)
            {
                System.IO.DirectoryInfo[] subDirInfos = dirInfo.GetDirectories();

                foreach (System.IO.DirectoryInfo dir in subDirInfos)
                {
                    LuceneUpdateIndexInfo luceneUpdateIndexInfo = GetLuceneUpdateIndexInfo(dir.FullName);
                    if (luceneUpdateIndexInfo != null)
                    {
                        indexList.Add(luceneUpdateIndexInfo);
                    }
                }
            }
            else
            {
                //LogWriter.FileLogWriter.WriteLine("", indexDirectory + " have no index file!");
            }
            return indexList;
        }

        private LuceneUpdateIndexInfo GetLuceneUpdateIndexInfo(string indexDirectory)
        {
            if (indexDirectory.EndsWith("AllBrands"))
                return null;
            if (System.IO.Directory.Exists(indexDirectory) && System.IO.File.Exists(indexDirectory + "\\segments.gen"))
            {
                LuceneUpdateIndexInfo luceneUpdateIndexInfo = new LuceneUpdateIndexInfo();

                Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexDirectory));
                Lucene.Net.Index.IndexReader indexReader = DirectoryReader.Open(fsDirectory, true);
                Lucene.Net.Search.IndexSearcher indexSearcher = new IndexSearcher(indexReader);

                //StandardAnalyzer standardAnalyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
                //IndexWriterConfig iwc = new IndexWriterConfig(LuceneVersion.LUCENE_48, standardAnalyzer);
                //iwc.OpenMode = OpenMode.APPEND;
                //Lucene.Net.Index.IndexWriter indexWriter = new Lucene.Net.Index.IndexWriter(fsDirectory, iwc);

                Lucene.Net.Index.IndexWriter indexWriter = new Lucene.Net.Index.IndexWriter(fsDirectory, new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED);

                luceneUpdateIndexInfo.UpdateIndexSearcher = indexSearcher;
                luceneUpdateIndexInfo.UpdateIndexWriter = indexWriter;

                return luceneUpdateIndexInfo;
            }
            else
            {
                //LogWriter.FileLogWriter.WriteLine("", indexDirectory + " have no index file!");
            }
            return null;
        }
    }
}