using ExpertReviewIndex.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Analysis;
using Lucene.Net.Util;
using Lucene.Net.Documents;

namespace ExpertReviewIndex
{
    public class BuilderExpertAverageIndex
    {
        IndexWriter indexWriter;
        string indexPath = string.Empty;

        Dictionary<int, ExpertReview> _expertReviewDic;
        List<int> erPidList;
        Dictionary<int, ProductVotesSum> pvsDic;
        List<int> pvsList;
        Dictionary<int, string> fsDic;

        string date = string.Empty;

        public bool Builder(string _date)
        {
            try
            {
                date = _date;

                System.Console.WriteLine("Begig Create IndexWriter.......");
                BuildIndexLog.WriterLog("Begin Builder Expert Average Index...    at: " + DateTime.Now);

                CreateIndexWriter();

                System.Console.WriteLine("Get ReviewAverage.......");
                GetReviewAverage();

                BuildIndexLog.WriterLog("Create Index...    at: " + DateTime.Now);
                
                indexWriter.Dispose();

                BuildIndexLog.WriterLog("End...    at: " + DateTime.Now);

                return true;
            }
            catch (Exception ex)
            {
                BuildIndexLog.WriterLog("Error: " + ex.Message + ex.StackTrace);
            }

            return false;
        }

        private void GetReviewAverage()
        {
            GetExpertReviewData();
            GetUserReviewData();
            GetFeatureScore();

            BuildIndexLog.WriterLog("Calculation Review Average and writer index...   at: " + DateTime.Now);

            List<int> pidList = new List<int>();
            pvsList.AddRange(erPidList);

            BuildIndexLog.WriterLog("Review Average count: " + pvsList.Count);
            foreach (int pid in pvsList)
            {
                if (!pidList.Contains(pid))
                    pidList.Add(pid);
                else
                    continue;

                ReviewAverage ra = new ReviewAverage();
                ra.ProductID = pid;

                float userRatingSum = 3;
                int userRatingVotes = 1;
                double priceMeScore = 0;
                int votesHasScore = 0;
                float userAverageRating = 0;
                int userVotes = 0;

                if (_expertReviewDic.ContainsKey(pid))
                {
                    ra.ExpertReviewCount = _expertReviewDic[pid].Votes;
                    priceMeScore = _expertReviewDic[pid].PriceMeScore;
                    votesHasScore = _expertReviewDic[pid].VotesHasScore;
                    userAverageRating = _expertReviewDic[pid].UserAverageRating;
                    userVotes = _expertReviewDic[pid].UserVotes;
                    ra.TFEAverageRating = priceMeScore;
                    ra.TFUAverageRating = userAverageRating;
                }

                if (pvsDic.ContainsKey(pid))
                {
                    ra.UserReviewCount = pvsDic[pid].ProductRatingVotes - 1;
                    userRatingVotes = pvsDic[pid].ProductRatingVotes;
                    userRatingSum = pvsDic[pid].ProductRatingSum;
                }

                if (ra.ExpertReviewCount > 0 || ra.UserReviewCount > 0 || userVotes > 0)
                    ra.ProductRating = GetAverageRating(userRatingSum, userRatingVotes, priceMeScore, votesHasScore, userAverageRating, userVotes);

                //BuildIndexLog.WriterLog("PID:" + ra.ProductID + "\tExpertReviewCount:" + ra.ExpertReviewCount + "\tPriceMeScore:" + priceMeScore + "\tVotesHasScore:" + votesHasScore + "\tUserAverageRating:" + userAverageRating + "\tUserVotes:" + userVotes + "\tUserReviewCount:" + ra.UserReviewCount + "\tUserRatingVotes:" + userRatingVotes + "\tUserRatingSum:" + userRatingSum);

                //userVotes + ra.ExpertReviewCount = ExpertReviewCount
                ra.ExpertReviewCount = userVotes + ra.ExpertReviewCount;

                if (ra.ExpertReviewCount > 1)
                {
                    if (SqlServerController.DBController.IsExpertReviewBySource(pid, 156))
                        ra.ExpertReviewCount = ra.ExpertReviewCount - 1;
                }

                //如果只有一条 就判断该评论是不是有内容   内容为空就不用显示了
                if (ra.ExpertReviewCount == 1)
                {
                    bool isNull = IsExpertReviewNull(pid);
                    if (isNull)
                        ra.ExpertReviewCount = 0;
                }

                if (fsDic.ContainsKey(pid))
                    ra.FeatureScore = fsDic[pid];

                if (string.IsNullOrEmpty(ra.FeatureScore))
                    ra.FeatureScore = string.Empty;

                WriterIndex(ra);
            }
        }

        private void WriterIndex(ReviewAverage ra)
        {
            //BuildIndexLog.WriterLog("WriterIndex...");
            Document doc = new Document();

            doc.Add(new Int32Field("ProductID", ra.ProductID, Field.Store.YES));
            doc.Add(new Int32Field("ExpertReviewCount", ra.ExpertReviewCount, Field.Store.YES));
            doc.Add(new Int32Field("UserReviewCount", ra.UserReviewCount, Field.Store.YES));
            doc.Add(new StringField("FeatureScore", ra.FeatureScore, Field.Store.YES));

            float rating = 0;
            float.TryParse(ra.ProductRating.ToString(), out rating);
            doc.Add(new SingleField("ProductRating", rating, Field.Store.YES));

            rating = 0;
            float.TryParse(ra.TFEAverageRating.ToString(), out rating);
            doc.Add(new SingleField("TFEAverageRating", rating, Field.Store.YES));

            rating = 0;
            float.TryParse(ra.TFUAverageRating.ToString(), out rating);
            doc.Add(new SingleField("TFUAverageRating", rating, Field.Store.YES));

            while (true)
            {
                try
                {
                    indexWriter.AddDocument(doc);
                    break;
                }
                catch (Exception ex) { BuildIndexLog.WriterLog("Writer Error: " + ex.Message + ex.StackTrace); }
            }
        }

        private double GetAverageRating(float userRatingSum, int userRatingVotes, double priceMeScore, int votesHasScore, float userAverageRating, int userVotes)
        {
            double score = 0f;

            userRatingSum = userRatingSum - 3;
            userRatingVotes = userRatingVotes - 1;
            score = (userRatingSum * 1.0d + (priceMeScore * votesHasScore) + (userAverageRating * userVotes)) / (userRatingVotes * 1.0d + votesHasScore + userVotes);
            score = double.Parse(score.ToString("0.0"));

            return score;
        }

        private void CreateIndexWriter()
        {
            indexPath = SiteConfig.AppSettings("IndexRootPath") + @"\" + date + @"\ExpertAverage";
            if (!System.IO.Directory.Exists(indexPath))
                System.IO.Directory.CreateDirectory(indexPath);
            Analyzer analyzer = new Lucene.Net.Analysis.Core.WhitespaceAnalyzer(LuceneVersion.LUCENE_48);
            Lucene.Net.Store.FSDirectory ramDir = Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo(indexPath));

            IndexWriterConfig iwc = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
            iwc.OpenMode = OpenMode.CREATE;
            iwc.RAMBufferSizeMB = 300;
            iwc.MaxBufferedDocs = 2000;
            iwc.MaxThreadStates = IndexWriterConfig.DEFAULT_MAX_THREAD_STATES;

            indexWriter = new IndexWriter(ramDir, iwc);
        }

        private void GetExpertReviewData()
        {
            BuildIndexLog.WriterLog("Get ExpertReview Data...   at: " + DateTime.Now);

            SqlServerController.DBController.GetExpertReviewData(out _expertReviewDic, out erPidList);
        }

        private void GetUserReviewData()
        {
            BuildIndexLog.WriterLog("Get UserReview Data...   at: " + DateTime.Now);
            MySqlController.DBController.GetUserReviewData(out pvsDic, out pvsList);
        }

        private void GetFeatureScore()
        {
            SqlServerController.DBController.GetFeatureScore(out fsDic);
        }

        private bool IsExpertReviewNull(int pid)
        {
            return SqlServerController.DBController.IsExpertReviewNull(pid);
        }
    }
}
