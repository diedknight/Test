using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedProductsTool
{
    public class RelatedProductsOperator
    {
        int mMaxCount;
        List<ProductRelatedScore> mRelatedProductScoreList;
        List<int> mProductIdList;
        int mCountryId;
        int mInterval;

        public RelatedProductsOperator(int maxCount, int interval, int countryId)
        {
            mMaxCount = maxCount;
            mInterval = interval;
            mRelatedProductScoreList = new List<ProductRelatedScore>();
            mProductIdList = new List<int>();
            mCountryId = countryId;
        }

        public void Add(List<ProductRelatedScore> relatedProductScoreList, int productId)
        {
            mRelatedProductScoreList.AddRange(relatedProductScoreList);
            mProductIdList.Add(productId);

            if(mProductIdList.Count >= mMaxCount)
            {
                SaveToDB();
            }
        }

        private void SaveToDB()
        {
            try
            {
                RelatedProductsController.WriteToDBByProductId(mRelatedProductScoreList, mProductIdList, mCountryId, DateTime.Now, ConfigurationManager.ConnectionStrings["PriceMe_PM"].ConnectionString);
                if (mInterval > 0)
                {
                    System.Threading.Thread.Sleep(mInterval);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Product ids : " + string.Join(",", mProductIdList) + " ex:");
                Console.WriteLine(ex.Message + "\t" + ex.StackTrace);
            }

            mRelatedProductScoreList = new List<ProductRelatedScore>();
            mProductIdList = new List<int>();
        }

        public void Finish()
        {
            if(mProductIdList.Count > 0)
            {
                SaveToDB();
            }
        }
    }
}
