using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PricemeResource.Data;
using PricemeResource.Logic;

namespace PricemeResource.Pages
{
    public class CompareReviewModel : PageModel
    {
        public ResourcesData resData;
        public string pids;
        public int countryId;

        public List<ProductNewData> products;
        public string datasets;
        public string tip;
        public int filterCount;
        public string stringjs;
        public List<int> listPids;

        public void OnGet()
        {
            pids = Utility.GetParameter("pids", this.Request);
            countryId = Utility.GetIntParameter("cid", this.Request);
            resData = WebSiteConfig.dicResources[countryId];

            BindData();
            products = ProductController.GetRealProductSimplified(listPids, resData.DbInfo);

            GetReviewChartJson();
        }

        private void BindData()
        {
            List<string> listpid = pids.Split(',').ToList();
            listPids = new List<int>();

            if (listpid.Count > 0)
            {
                foreach (string pid in listpid)
                {
                    int id = 0;
                    int.TryParse(pid, out id);
                    if (id > 0)
                        listPids.Add(id);
                }
            }
        }

        public void GetReviewChartJson()
        {
            int arg = 0;
            string[] colorArr = { "rgba(60,141,188,0.9)", "#cb4b4b", "green" };

            var allReviewList = ProductController.GetExpertReviewResult(listPids, resData.DbInfo);

            foreach (var proid in listPids)
            {
                if (string.IsNullOrEmpty(proid.ToString())) continue;

                string productName = products.SingleOrDefault(s => s.ProductID == proid).ProductName;
                tip += "<span style='width:40px;height:12px;  display:inline-block;background-color:" + colorArr[arg] + ";'></span> " + productName + "&nbsp;&nbsp;";

                var reviewList = allReviewList.Where(r => r.ProductId == proid && r.PriceMeScore > 0).ToList();
                var reviewCount = reviewList.Count() * 1.0;

                var isZero = reviewList.Count() <= 0;
                filterCount = filterCount + reviewList.Count();
                
                datasets = datasets + "{label:\"el\",fillColor:\"" + colorArr[arg] + "\",";
                datasets = datasets + "strokeColor:\"" + colorArr[arg] + "\",";
                datasets = datasets + "pointColor:\"" + colorArr[arg] + "\",";
                datasets = datasets + "pointStrokeColor:\"" + colorArr[arg] + "\",";
                datasets = datasets + "pointHighlightFill:\"" + colorArr[arg] + "\",";
                datasets = datasets + "pointHighlightStroke:\"" + colorArr[arg] + "\",";
                
                var oneCount = reviewList.Where(f => f.PriceMeScore >= 1 && f.PriceMeScore < 2).Count();
                var twoCount = reviewList.Where(f => f.PriceMeScore >= 2 && f.PriceMeScore < 3).Count();
                var threeCount = reviewList.Where(f => f.PriceMeScore >= 3 && f.PriceMeScore < 4).Count();
                var fourCount = reviewList.Where(f => f.PriceMeScore >= 4 && f.PriceMeScore < 5).Count();
                var fiveCount = reviewList.Where(f => f.PriceMeScore >= 5).Count();

                var perOne = isZero ? "0" : ((oneCount / reviewCount) * 100).ToString("0");
                var perTwo = isZero ? "0" : ((twoCount / reviewCount) * 100).ToString("0");
                var perThree = isZero ? "0" : ((threeCount / reviewCount) * 100).ToString("0");
                var perFour = isZero ? "0" : ((fourCount / reviewCount) * 100).ToString("0");
                var perFive = isZero ? "0" : ((fiveCount / reviewCount) * 100).ToString("0");

                int one = int.Parse(perOne), two = int.Parse(perTwo), three = int.Parse(perThree), four = int.Parse(perFour), five = int.Parse(perFive);

                var count = one + five + two + three + four;

                var num = 0;
                if (count > 100)
                {
                    num = count - 100;

                    if (one > num)
                        one = one - num;
                    else if (two > num)
                        two = two - num;
                    else if (three > num)
                        three = three - num;
                    else if (four > num)
                        four = four - num;
                    else if (five > num)
                        five = five - num;

                }

                datasets = datasets + "data:[" + one + "," + two + "," + three + "," + four + "," + five + "]";

                datasets = datasets + "},";
                arg++;
            }

            stringjs = "(function jqIsReady() {if (typeof $ == \"undefined\") {setTimeout(jqIsReady, 10); return;}"
                    + " $(function() {var barChartCanvas = $(\"#barChart\").get(0).getContext(\"2d\");var barChart = new Chart(barChartCanvas);"
                    + " var barChartData = areaChartDatas; barChartOptions.datasetFill = false; barChart.Bar(barChartData, barChartOptions); })})();";
        }
    }
}