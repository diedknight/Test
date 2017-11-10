﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyze
{
    public class Term2
    {
        public static void Add(string words, int pId)
        {
            AllProductIndex.Add(words, pId);
        }


        public static void Save()
        {
            AllProductIndex.Save();
        }

        public static List<string> GetTerms(string str)
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

        public static List<WordScore> GetScore(string str, int top = 10)
        {
            Dictionary<int, WordScore> dic = new Dictionary<int, WordScore>();

            if (string.IsNullOrWhiteSpace(str)) return new List<WordScore>();

            AllProductIndex.GetPIdScore(GetTerms(str)).ToList().ForEach(item =>
            {
                if (dic.ContainsKey(item.Key))
                    dic[item.Key].Score += item.Value;
                else
                    dic.Add(item.Key, new WordScore() { PId = item.Key, Score = item.Value });
            });

            //GetTerms(str).ForEach(word =>
            //{
            //    AllProductIndex.GetPIdScore(word).ToList().ForEach(item => {
            //        if (dic.ContainsKey(item.Key))
            //            dic[item.Key].Score += item.Value;
            //        else
            //            dic.Add(item.Key, new WordScore() { PId = item.Key, Score = item.Value });
            //    });
            //});

            return dic.OrderByDescending(item => item.Value.Score).Take(top).Select(item => item.Value).ToList();
        }

        public static List<WordScore> GetScore(string retailerProductName, int productId, decimal price, int top = 10)
        {
            Dictionary<int, WordScore> dic = new Dictionary<int, WordScore>();

            if (string.IsNullOrWhiteSpace(retailerProductName)) return new List<WordScore>();

            string str = string.Format("{0} Price_{1}", retailerProductName, price.ToString("0.00"));

            return GetScore(str, top + 1).Where(item => item.PId != productId).Take(top).ToList();
        }

    }
}