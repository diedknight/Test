using PriceMeDBA;
using Service.Infrastructure;
using Service.Infrastructure.Compress;
using Service.Infrastructure.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyze
{
    public class Term
    {        
        private static Dictionary<string, WordInfo> _wordDic = null;
        private static Dictionary<int, int> _productDic = null;

        public static int WordCount { get { return _wordDic.Count; } }

        static Term()
        {
            _wordDic = new Dictionary<string, WordInfo>();
            _productDic = new Dictionary<int, int>();
        }

        private static void Clear()
        {
            _wordDic.Clear();
            _productDic.Clear();

            _wordDic = new Dictionary<string, WordInfo>();
            _productDic = new Dictionary<int, int>();
        }

        public static void Save()
        {
            foreach (var item in _productDic)
            {
                PIdIndex.Add(item.Key, item.Value);
                ProductCountIndex.Add(item.Key);
            }

            foreach (var item in _wordDic)
            {
                WordIndex.Add(item.Key, item.Value.HitPIdCount);
            }

            Clear();

            WordIndex.Save();
            PIdIndex.Save();
            ProductCountIndex.Save();
        }

        public static void Add(string rpName, int pId)
        {
            if (string.IsNullOrWhiteSpace(rpName)) return;
            if (pId == 0) return;

            List<string> list = GetTerms(rpName);

            list.ForEach(word =>
            {
                if (_wordDic.ContainsKey(word))
                {
                    WordInfo wordInfo = _wordDic[word];

                    if (wordInfo.HitPIdCount.ContainsKey(pId))
                    {
                        wordInfo.HitPIdCount[pId] = wordInfo.HitPIdCount[pId] + 1;
                    }
                    else
                    {
                        wordInfo.HitPIdCount.Add(pId, 1);
                    }
                }
                else
                {
                    WordInfo wordInfo = new WordInfo();
                    //wordInfo.Word = word;
                    wordInfo.HitPIdCount.Add(pId, 1);

                    _wordDic.Add(word, wordInfo);
                }
            });

            if (_productDic.ContainsKey(pId))
            {
                _productDic[pId] = _productDic[pId] + list.Count;
            }
            else
            {
                _productDic.Add(pId, list.Count);
            }
        }

        private static List<string> GetTerms(string str)
        {
            List<string> list = new List<string>();

            using (Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30, new HashSet<string>()))
            {
                using (var tokenStream = analyzer.TokenStream("", new StringReader(str)))
                {
                    var termAttribute = tokenStream.AddAttribute<Lucene.Net.Analysis.Tokenattributes.ITermAttribute>();

                    while (tokenStream.IncrementToken())
                    {
                        list.Add(termAttribute.Term);
                    }
                }
            }

            HashSet<string> tempSet = new HashSet<string>();
            list.ForEach(item => { tempSet.Add(item); });

            return tempSet.ToList();
        }

        public static List<WordScore> GetScore(string retailerProductName, int productId, decimal price, int top = 10)
        {
            Dictionary<int, WordScore> dic = new Dictionary<int, WordScore>();

            if (string.IsNullOrWhiteSpace(retailerProductName)) return new List<WordScore>();

            string str = string.Format("{0} Price_{1}", retailerProductName, price.ToString("0.00"));

            return GetScore(str, top + 1).Where(item => item.PId != productId).Take(top).ToList();
        }

        public static List<WordScore> GetScore(string str, int top = 10)
        {
            Dictionary<int, WordScore> dic = new Dictionary<int, WordScore>();

            if (string.IsNullOrWhiteSpace(str)) return new List<WordScore>();

            GetTerms(str).ForEach(word =>
            {
                var wordDic = WordIndex.GetHitPId(word);
                if (wordDic.Count == 0) return;

                foreach (var item in wordDic)
                {
                    int pId = item.Key;

                    double hitCount = Convert.ToDouble(item.Value);

                    if (word.Contains("price_") && hitCount < 2d) continue; //Product内，价格命中次数小于2次的不考虑
                    //if (hitCount < 2d) continue;   //Product内，单词命中次数小于2次的不考虑
                    
                    var productWordCount = PIdIndex.GetWordCount(pId);                    
                    //if (productWordCount < 50) continue;   //产品单词数小于50的不考虑

                    double weight = Math.Log(Convert.ToDouble(PIdIndex.Count) / wordDic.Count, 2);

                    double score = hitCount / productWordCount * weight;

                    if (dic.ContainsKey(pId))
                        dic[pId].Score += score;
                    else
                        dic.Add(pId, new WordScore() { PId = pId, Score = score });
                }
            });

            return dic.OrderByDescending(item => item.Value.Score).Take(top).Select(item => item.Value).ToList();
        }

        //private class WordWeight
        //{
        //    public int HitCount { get; set; }
        //    public double TermFrequency { get; set; }
        //}

        private class WordInfo
        {
            public WordInfo()
            {
                this.HitPIdCount = new Dictionary<int, int>();
            }

            //public string Word { get; set; }
            public Dictionary<int, int> HitPIdCount { get; set; }
            
            public double Weight { get; set; }
        }

        
    }
}
