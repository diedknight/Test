using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarityMatchTool
{
    public class Score
    {
        public static double Similarity(string name, decimal price, string compareName, decimal comparePrice)
        {
            var cos = new Cosine();

            double priceDiff = 1;
            if (price != 0)
            {
                priceDiff = (double)(Math.Abs(price - comparePrice) / price);
            }

            //w1 * (1-a*(price diff %) )/100
            double similarityPrice = AppConfig.W1 * (1 - AppConfig.A * priceDiff) / 100;

            //w2 * Cosine similarity
            double similarityCosine = AppConfig.W2 * cos.Similarity(name.ToLower(), compareName.ToLower());

            double totalSimilarity = similarityPrice + similarityCosine;

            return totalSimilarity;
        }
    }
}
