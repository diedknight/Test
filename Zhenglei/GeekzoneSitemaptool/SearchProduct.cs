using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;
using PriceMeDBA;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using PriceMeCommon;

namespace Pricealyser.SiteMap
{
    public static class SearchProduct
    {
        const int MAXDOCS = 100000;
        static Dictionary<int, Searcher> categoriesProductLuceneIndex;
        static IEnumerable<CSK_Store_Category> AllActiveRootCategories;
        static string indexPath = System.Configuration.ConfigurationManager.AppSettings["IndexPath"].ToString();

        static SearchProduct()
        {
            LoadIndexSearcher();
        }

        public static Dictionary<int, Searcher> CategoriesProductLuceneIndex
        {
            get { return categoriesProductLuceneIndex; }
        }

        public static void LoadIndexSearcher()
        {
            AllActiveRootCategories = CSK_Store_Category.Find(cat => cat.ParentID == new int?(0) && cat.IsActive == true);
            categoriesProductLuceneIndex = GetCategoriesProductLuceneIndex(indexPath);
        }

        private static Dictionary<int, Searcher> GetCategoriesProductLuceneIndex(string indexRootPath)
        {
            Dictionary<int, Searcher> categoriesProductLuceneIndex = new Dictionary<int, Searcher>();
            //List<Searcher> searcherList = new List<Searcher>();
            foreach (CSK_Store_Category pc in AllActiveRootCategories)
            {
                string path = indexRootPath + "AllCategoriesProduct\\" + pc.CategoryName.Trim();
                if (!System.IO.Directory.Exists(path) || string.IsNullOrEmpty(pc.CategoryName) || !System.IO.File.Exists(path + "\\segments.gen"))
                    continue;

                IndexSearcher pCategoryIndexSearcher = GetIndexSearcher(path);
                categoriesProductLuceneIndex.Add(pc.CategoryID, pCategoryIndexSearcher);
            }

            string productsIndexPath = indexRootPath + "AllCategoriesProduct\\Products";

            IndexSearcher productCategoryIndexSearcher = GetIndexSearcher(productsIndexPath);
            categoriesProductLuceneIndex.Add(0, productCategoryIndexSearcher);

            return categoriesProductLuceneIndex;
        }

        public static IndexSearcher GetIndexSearcher(string indexDirectory)
        {
            if (System.IO.Directory.Exists(indexDirectory) && System.IO.File.Exists(indexDirectory + "\\segments.gen"))
            {
                Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexDirectory));
                IndexSearcher indexSearcher = new IndexSearcher(fsDirectory, true);
                return indexSearcher;
            }
            return null;
        }

        public static void GetSiteMapProductByCategoryId(int categoryId, bool isBabyCategory, List<PriceMeCommon.Data.ProductCatalog> products)
        {
            Searcher searcher = GetSearcherByCategoryID(categoryId);
            if (searcher == null)
                return;

            Sort sort = new Sort(SortField.FIELD_SCORE);

            BooleanQuery query = new BooleanQuery();
            TermQuery termQuery = new TermQuery(new Term("CategoryID", categoryId.ToString()));
            query.Add(termQuery, BooleanClause.Occur.MUST);

            if (!isBabyCategory)
            {
                CSK_Store_Category category = CategoryController.GetCategoryByCategoryID(categoryId);
                if (!category.IsDisplayIsMerged ?? false)
                {
                    termQuery = new TermQuery(new Term("IsMerge", "True"));
                    query.Add(termQuery, BooleanClause.Occur.MUST);
                }
                termQuery = new TermQuery(new Term("ManufacturerID", "-1"));
                query.Add(termQuery, BooleanClause.Occur.MUST_NOT);
            }

            TopDocs topDocs = searcher.Search(query, null, MAXDOCS, sort);

            for (int i = 0; i < topDocs.scoreDocs.Length; i++)
            {
                PriceMeCommon.Data.ProductCatalog pc = new PriceMeCommon.Data.ProductCatalog();
                
                Document doc = searcher.Doc(topDocs.scoreDocs[i].doc);
                pc.ProductID = doc.Get("ProductID");
                pc.ProductName = doc.Get("ProductName");

                products.Add(pc);
            }
        }

        public static Searcher GetSearcherByCategoryID(int categoryID)
        {
            int rootCategoryID = 0;
            if (categoryID != 0)
            {
                CSK_Store_Category rootCategory = CategoryController.GetRootCategory(categoryID);
                if (rootCategory == null)
                {
                    return null;
                }
                rootCategoryID = rootCategory.CategoryID;
            }

            if (CategoriesProductLuceneIndex.ContainsKey(rootCategoryID))
                return CategoriesProductLuceneIndex[rootCategoryID];
            else
                return null;
        }
    }
}
