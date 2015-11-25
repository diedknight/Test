using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMe.RichAttributeDisplayTool.Process
{
    class DataRank
    {




        /// <summary>
        /// 数据排名算法
        /// </summary>
        /// <param name="proCount"></param>
        /// <returns></returns>
        public List<string> GetRank(Object sender, RankEventArgs e)
        {

            var rankList = new List<string>();

            var process = (ProcessData)sender;

            e.perList.ForEach(f => {
                
                int proCount = process.AttList.Count();

                var disCount = proCount * f;

                var roundCount = disCount.Round();

                var result = proCount - roundCount;

                var rank = process.AttList[result - 1].Value;

                rankList.Add(rank.ToString());
            });

            return rankList;
        }


        


    }
}
