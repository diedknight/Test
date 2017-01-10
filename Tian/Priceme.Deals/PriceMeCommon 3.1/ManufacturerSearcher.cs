using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace PriceMeCommon
{
    public class ManufacturerSearcher
    {
        public static List<int> GetAllBrandsIdList()
        {
            Lucene.Net.Search.Searcher allBrandsIndexSearcher = LuceneController.AllBrandsIndexSearcher;

            List<int> allBrandsId = new List<int>();
            if (allBrandsIndexSearcher != null)
            {
                for (int i = 0; i < allBrandsIndexSearcher.MaxDoc; i++)
                {
                    string mid = allBrandsIndexSearcher.Doc(i).Get("ManufacturerID");
                    allBrandsId.Add(int.Parse(mid));
                }
            }

            return allBrandsId;
        }
    }
}