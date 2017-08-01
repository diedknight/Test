using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class LuceneUpdateIndexInfo
    {
        public Lucene.Net.Search.IndexSearcher UpdateIndexSearcher { get; set; }
        public Lucene.Net.Index.IndexWriter UpdateIndexWriter { get; set; }
    }
}