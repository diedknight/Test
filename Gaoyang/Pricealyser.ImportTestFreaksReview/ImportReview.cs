using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PriceMeDBA;
using PriceMeCommon.BusinessLogic;
using System.Net;
using System.Data;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace Pricealyser.ImportTestFreaksReview
{
    public class ImportReview
    {
        StreamWriter _sw;
        public StreamWriter SW
        {
            get { return _sw; }
            set { _sw = value; }
        }

        string driverPath = System.Configuration.ConfigurationManager.AppSettings["DriverPath"].ToString();
        string imageLogo = System.Configuration.ConfigurationManager.AppSettings["ImageLogo"].ToString();

        Dictionary<int, int> sourceidDic = new Dictionary<int, int>();
        Dictionary<int, string> sourceLogoDic = new Dictionary<int, string>();
        int productCount;
        int count = 0;

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
        string ImportRatio = System.Configuration.ConfigurationManager.AppSettings["ImportRatio"].ToString();
        string EmailReceiver = System.Configuration.ConfigurationManager.AppSettings["EmailReceiver"].ToString();
        public  System.Data.SqlClient.SqlConnection sqlConn;
        List<string> ExistVideos;
        ExpertReviewCache expertCache;
        public void Import()
        {

            Writer("Begin Import Review... " + DateTime.Now);
            string feedFile=string.Empty;
            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["isDevelop"].ToString()))
                feedFile = System.Configuration.ConfigurationManager.AppSettings["FeedFilePath"].ToString();//LoadFeed.GetFTPFile();
            else
                feedFile = LoadFeed.GetFTPFile();

            if (string.IsNullOrEmpty(feedFile))
            {
                Writer("not found feed...");
                return;
            }

            Writer("Load Feed... " + DateTime.Now);
            XmlNodeList nodes = LoadFeed.Load(feedFile);
            if (nodes == null || nodes.Count == 0)
            {
                Writer("Load Feed Error... " + DateTime.Now);
                return;
            }

            ExistVideos = new List<string>();// CSK_Store_ProductVideoSource.All().Select(v => v.SourceUrl).ToList();

            Writer("Bind ReviewSource");
            //BusinessController.GetProductReview();

            //打开数据库连接
            sqlConn = new System.Data.SqlClient.SqlConnection(connectionString);
            sqlConn.Open();

            BindReviewSource();

            string videoSql = "select SourceUrl from CSK_Store_ProductVideoSource";
            using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
            {
                sqlCMD.CommandText = videoSql;
                sqlCMD.CommandTimeout = 0;
                sqlCMD.CommandType = System.Data.CommandType.Text;
                sqlCMD.Connection = sqlConn;

                using (IDataReader dr = sqlCMD.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ExistVideos.Add(dr["SourceUrl"].ToString());
                    }
                }
            }

             expertCache = new ExpertReviewCache();


            //int index = 0;
            //int pageSize = (nodes.Count / 3);
            //int pageCount = (nodes.Count / pageSize) + 1;
            for (int s = 0; s < 1; s++)//pageCount
            {
                //if (s != 0)
                //    nodes = LoadFeed.Load(feedFile);

                //List<XmlNode> readNodes = new List<XmlNode>();
                //for (int i = 0; i < pageSize; i++)
                //{
                //    if (index >= nodes.Count) break;
                //    XmlNode xmlNode = nodes[index];
                //    readNodes.Add(xmlNode);
                //    index++;
                //}
                //nodes = null;
                Writer("Get " + (s + 1) + " XmlNode count: " + nodes.Count);//readNodes
                int reviews = 0; 
                foreach (XmlNode node in nodes)//readNodes
                {
                    List<SourceReview> srList = new List<SourceReview>();
                    OtherScore os = new OtherScore();
                    List<Video> videoList = new List<Video>();
                    int tfId = 0;
                    List<int> pidList = new List<int>();

                    try
                    {
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            string nodeName = childNode.Name;
                            string data = GetContentData(childNode);
                            switch (nodeName)
                            {
                                case "product_id": tfId = int.Parse(data); break;
                                case "your_product_ids": pidList = GetPriceMeProductId(childNode); break;
                                case "scores": GetOtherScore(childNode, os); break;
                                case "region": GetRegionReview(childNode, srList); break;
                                case "videos": GetVideos(childNode, videoList); break;
                                default: break;
                            }
                        }
                    }
                    catch (Exception ex) { Writer("Error: " + ex.Message + ex.StackTrace); }
                    reviews += srList.Count;
                    SaveData(srList, os, pidList, videoList);
                    #region 删除代码
                    ////Test
                    //foreach (int pid in pidList)
                    //{
                    //    count++;
                    //    System.Console.WriteLine(count.ToString());

                    //    if (!BusinessController.productList.Contains(pid))
                    //    {
                    //        bool isP = false;
                    //        bool isProduct = ProductsIndexController.SearchController.SearchIsExistsProduct(pid);
                    //        if (!isProduct)
                    //        {
                    //            int productid = pid;
                    //            while (true)
                    //            {
                    //                System.Console.WriteLine(productid.ToString());
                    //                CSK_Store_ProductIsMerged prm = ProductController.GetProductIdInProductIsMergedByProductId(productid);
                    //                if (prm != null)
                    //                {
                    //                    productid = prm.ToProductID;
                    //                    isProduct = ProductsIndexController.SearchController.SearchIsExistsProduct(productid);
                    //                    if (isProduct)
                    //                    {
                    //                        isP = true;
                    //                        break;
                    //                    }
                    //                }
                    //                else
                    //                    break;
                    //            }
                    //        }
                    //        else
                    //            isP = true;

                    //        if (isP)
                    //            Writer(pid.ToString());
                    //    }
                    //}
                    #endregion
                }
                Writer("total reviewsourse count in file: " + reviews);

                //计算比例，状态即将要变0的总数/数据库中状态为1的总数
                //update where modifyon<当天  zgy 
                string sql_1 = "select count(*) from CSK_Store_ExpertReviewAU where DisplayLinkStatus=0 and ModifiedOn<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                var status_0 = GetCount(sql_1).ToString();

                string sql_2 = "select count(*) from CSK_Store_ExpertReviewAU where DisplayLinkStatus=1";
                var status_1 = GetCount(sql_2).ToString();

                if (!string.IsNullOrEmpty(status_0) && !string.IsNullOrEmpty(status_1) && status_1 != "0")
                {
                    var s_0 = int.Parse(status_0);
                    var s_1 = int.Parse(status_1);
                    var result = (s_0 / (s_1 * 1.0)) * 100.00;
                    if (result > int.Parse(ImportRatio))
                    {
                        string content = "实际比例为" + result + "% 已经超过设定比例" + ImportRatio + "";
                        sendEmail("Channelyser <info@channelyser.com>", content, content, EmailReceiver.Split(',').ToArray());
                        Console.WriteLine("To"+EmailReceiver+"send a email,Time is："+DateTime.Now);
                    }
                    else
                    {
                        string sql = "update CSK_Store_ExpertReviewAU set DisplayLinkStatus=0 where ModifiedOn<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                        SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
                        sp.Command.CommandSql = sql;
                        sp.Command.CommandType = CommandType.Text;
                        sp.Command.CommandTimeout = 0;
                        sp.Execute();
                    }
                }
                
            }

            Console.WriteLine("==========The program execution is completed======== Time is" + DateTime.Now);
            string deleteNullReviewSql = @"delete dbo.CSK_Store_ExpertReviewAU 
                                        WHERE (Title IS NULL OR LEN(Title) = 0) 
                                        AND (Description IS NULL OR LEN(Description) = 0)  
                                        AND (Pros IS NULL OR LEN(Pros) = 0 )
                                        AND (Cons IS NULL OR LEN(Cons) = 0 )
                                        AND (Verdict IS NULL OR LEN(Verdict) = 0)
                                        AND ProductID IN 
                                        (SELECT ProductID FROM CSK_Store_Product WHERE ProductStatus = 1 AND IsDeleted = 0 AND ProductID IN 
                                        (SELECT DISTINCT ProductID FROM CSK_Store_RetailerProduct WHERE RetailerProductStatus=1 AND IsDeleted = 0))";
            SubSonic.Schema.StoredProcedure detsp = new SubSonic.Schema.StoredProcedure("");
            detsp.Command.CommandSql = deleteNullReviewSql;
            detsp.Command.CommandType = CommandType.Text;
            detsp.Command.CommandTimeout = 0;
            detsp.Execute();
            #region 删除代码
            //Writer("Get XmlNode count: " + nodes.Count);
            //foreach (XmlNode node in nodes)
            //{
            //    List<SourceReview> srList = new List<SourceReview>();
            //    OtherScore os = new OtherScore();
            //    List<Video> videoList = new List<Video>();
            //    int tfId = 0;
            //    List<int> pidList = new List<int>();

            //    try
            //    {
            //        foreach (XmlNode childNode in node.ChildNodes)
            //        {
            //            string nodeName = childNode.Name;
            //            string data = GetContentData(childNode);
            //            switch (nodeName)
            //            {
            //                case "product_id": tfId = int.Parse(data); break;
            //                case "your_product_ids": pidList = GetPriceMeProductId(childNode); break;
            //                case "scores": GetOtherScore(childNode, os); break;
            //                case "region": GetRegionReview(childNode, srList); break;
            //                case "videos": GetVideos(childNode, videoList); break;
            //                default: break;
            //            }
            //        }
            //    }
            //    catch (Exception ex) { Writer("Error: " + ex.Message + ex.StackTrace); }

            //    SaveData(srList, os, pidList, videoList);

            //    Writer("Get productid: " + tfId);
            //}
            #endregion
            //关闭数据库连接
            sqlConn.Close();

            Writer("Import " + productCount + " Reviews......" + DateTime.Now);
            Writer("End... " + DateTime.Now);
            Console.WriteLine("==========The program execution is completed======== Time is" + DateTime.Now);
        }

        private int GetCount(string videoSql)
        {
            using (var sqlConn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (var sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = videoSql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = System.Data.CommandType.Text;
                    sqlCMD.Connection = sqlConn;
                    return int.Parse(sqlCMD.ExecuteScalar().ToString());
                }
            }
            
        }

        private void GetVideos(XmlNode node, List<Video> videos)
        {
            if (videos.Count == 0)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    Video vo = new Video();
                
                    foreach (XmlNode subChildNode in childNode.ChildNodes)
                    {
                        string nodeName = subChildNode.Name;
                        string data = GetContentData(subChildNode);
                        switch (nodeName)
                        {
                            case "url": vo.Url = data; break;
                            case "thumbnail": vo.Thumbnail = data; break;
                            default: break;
                        }
                    }
                    videos.Add(vo);
                }
            }
        }

        private void GetOtherScore(XmlNode node, OtherScore os)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                string data = GetContentData(childNode);
                decimal value = 0;
                decimal.TryParse(data, out value);
                if (value > 0)
                {
                    string type = childNode.Attributes["type"].Value;
                    switch (type)
                    {
                        case "design": os.Design = value; break;
                        case "value_for_money": os.Value_for_money = value; break;
                        case "features": os.Features = value; break;
                        case "ease_of_use": os.Ease_of_use = value; break;
                        case "sound_quality": os.Sound_quality = value; break;
                        case "picture_quality": os.Picture_quality = value; break;
                        case "durability": os.Durability = value; break;
                        case "overall_quality": os.Overall_quality = value; break;
                        case "performance": os.Performance = value; break;
                        case "reliability": os.Reliability = value; break;
                        default: break;
                    }
                }
            }
        }

        private void GetRegionReview(XmlNode node, List<SourceReview> srList)
        {
            if (node.Attributes["language"] != null && node.Attributes["language"].InnerText == "en")
            {
                if (srList.Count == 0)
                {
                    SourceReview sr = new SourceReview();
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        string nodeName = childNode.Name;
                        string data = GetContentData(childNode);
                        switch (nodeName)
                        {
                            case "testfreaks_summary": sr.Summary = data; break;
                            case "testfreaks_url": sr.Url = data; break;
                            case "pros": sr.Pros = GetProsOrCons(childNode); break;
                            case "cons": sr.Cons = GetProsOrCons(childNode); break;
                            default: break;
                        }
                        if (!string.IsNullOrEmpty(sr.Cons))
                            break;
                    }

                    sr.Score = 0m;
                    sr.SourceId = 1;
                    sr.SourceName = "TestFreaks";
                    sr.IsExpertReview = true;
                    srList.Add(sr);
                }

                foreach (XmlNode childNode in node.ChildNodes)
                {
                    string nodeName = childNode.Name;
                    switch (nodeName)
                    {
                        case "snippets": GetSnippets(childNode, srList); break;
                        default: break;
                    }
                }
            }
        }

        private void GetSnippets(XmlNode node, List<SourceReview> srList)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                ////暂时只进口专家评论
                //if (childNode.Attributes["type"].Value != "pro")
                //    continue;

                SourceReview sr = new SourceReview();
                if (childNode.Attributes["type"].Value == "pro")
                    sr.IsExpertReview = true;
                else
                    sr.IsExpertReview = false;

                foreach (XmlNode snode in childNode.ChildNodes)
                {
                    string nodeName = snode.Name;
                    string data = GetContentData(snode);
                    switch (nodeName)
                    {
                        case "source_id": sr.SourceId = int.Parse(data); break;
                        case "source_name":
                            {
                                sr.SourceName = data;
                                Writer("read from file: (sr.SourceName = data =>" + sr.SourceName + "=" + data + ")");
                                break;
                            }
                        case "source_logo": sr.SourceLogo = data; break;
                        case "url": sr.Url = data; break;
                        case "title": sr.Summary = data; break;
                        case "extract": sr.Extract = data; break;
                        case "pros": sr.Pros = data; break;
                        case "cons": sr.Cons = data; break;
                        case "author": sr.Author = data; break;
                        case "score": decimal score = 0m; decimal.TryParse(data, out score); sr.Score = score; break;
                        case "date": sr.Date = data; break;
                        default: break;
                    }
                   
                }
                srList.Add(sr);
            }
        }

        private string GetProsOrCons(XmlNode node)
        {
            string temp = string.Empty;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                string data = GetContentData(childNode);
                if (temp != string.Empty)
                    temp += ", " + data;
                else
                    temp += data;
            }

            return temp;
        }

        private List<int> GetPriceMeProductId(XmlNode node)
        {
            List<int> pidList = new List<int>();
            foreach (XmlNode pnode in node)
            {
                string data = GetContentData(pnode);
                int pid = 0;
                int.TryParse(data, out pid);
                if (!pidList.Contains(pid))
                    pidList.Add(pid);
            }

            return pidList;
        }

        private string GetContentData(XmlNode nodeEntity)
        {
            string contentData = nodeEntity.InnerText;

            return contentData;
        }

        private void SaveData(List<SourceReview> srList, OtherScore os, List<int> pidList, List<Video> videoList)
        {


            foreach (int pid in pidList)
            {
                try
                {
                    count++;
                    System.Console.WriteLine(count);
                    int productId = pid;
                    #region 无用代码
                    //bool isP = false;
                    //bool isProduct = ProductsIndexController.SearchController.SearchIsExistsProduct(productId);
                    //if (!isProduct)
                    //{
                    //    int i = 0;
                    //    while (true)
                    //    {
                    //        i++;
                    //        if (i > 3) break;

                    //        System.Console.WriteLine(productId.ToString());
                    //        CSK_Store_ProductIsMerged prm = ProductController.GetProductIdInProductIsMergedByProductId(productId);
                    //        if (prm != null)
                    //        {
                    //            productId = prm.ToProductID;
                    //            isProduct = ProductsIndexController.SearchController.SearchIsExistsProduct(productId);
                    //            if (isProduct)
                    //            {
                    //                isP = true;
                    //                break;
                    //            }
                    //        }
                    //        else
                    //            break;
                    //    }
                    //}
                    //else
                    //    isP = true;

                    //if (!isP)
                    //{
                    //    Writer("Can not find productId: " + productId + DateTime.Now);
                    //    continue;
                    //}

                    #endregion
                    productCount++;

                    if (!expertCache.FeatureScoreList.Contains(productId))
                    {
                        expertCache.FeatureScoreList.Add(productId);
                        try
                        {
                            CSK_Store_ExpertReviewFeatureScore fs = new CSK_Store_ExpertReviewFeatureScore();
                            fs.ProductID = productId;
                            fs.Design = os.Design;
                            fs.Value_for_money = os.Value_for_money;
                            fs.Features = os.Features;
                            fs.Ease_of_use = os.Ease_of_use;
                            fs.Sound_quality = os.Sound_quality;
                            fs.Picture_quality = os.Picture_quality;
                            fs.Durability = os.Durability;
                            fs.Overall_quality = os.Overall_quality;
                            fs.Performance = os.Performance;
                            fs.Reliability = os.Reliability;
                            fs.Save();
                        }
                        catch (Exception ex) { Writer("Add " + productId + " FeatureScore Error: " + ex.Message + ex.StackTrace); }
                    }

                    List<string> needUpdateModifyOn = new List<string>();//<productId|sid>

                    foreach (SourceReview sr in srList)
                    {
                        try
                        {//此两个ID来源于Feed
                            int sid = SaveReviewSource(sr);

                            var urls = sr.Url.Replace("http://", "");
                            if (urls.Contains("/"))
                                urls = urls.Substring(urls.IndexOf('/'));

                            string key = productId + "|" + sid;
                            string keyUp = key + "|" + urls;

                            if (!expertCache.ExpertReviewList.Contains(key))//如果匹配不到执行添加操作
                            {
                                #region
                                expertCache.ExpertReviewList.Add(key);
                                CSK_Store_ExpertReviewAU er = new CSK_Store_ExpertReviewAU();
                                er.SourceID = sid;
                                er.ProductID = productId;
                                er.Title = ReplacementString(sr.Summary);
                                if (er.Title == null)
                                    er.Title = "";
                                if (er.Title.Length > 500)
                                    er.Title = er.Title.Substring(0, 499);
                                er.Pros = ReplacementString(sr.Pros);
                                if (er.Pros != null && er.Pros.Length > 2000)
                                    er.Pros = er.Pros.Substring(0, 1999);
                                er.Cons = ReplacementString(sr.Cons);
                                if (er.Cons != null && er.Cons.Length > 2000)
                                    er.Cons = er.Cons.Substring(0, 1999);
                                er.Verdict = ReplacementString(sr.Extract);
                                if (er.Verdict != null && er.Verdict.Length > 2000)
                                    er.Verdict = er.Verdict.Substring(0, 1999);
                                string url = string.Empty;
                                if (!string.IsNullOrEmpty(sr.Url))
                                {
                                    url = sr.Url.Replace("http://", "");
                                    if (url.Contains("/"))
                                        url = url.Substring(url.IndexOf('/'));
                                }
                                er.ReviewURL = url;
                                er.ReviewBy = sr.Author;
                                if (string.IsNullOrEmpty(er.ReviewBy))
                                    er.ReviewBy = string.Empty;

                                //空的评论不需要加入数据库
                                if (string.IsNullOrEmpty(er.Title) && string.IsNullOrEmpty(er.Pros) && string.IsNullOrEmpty(er.Cons) && string.IsNullOrEmpty(er.Verdict))
                                {
                                    Writer("null expert review... " + key);
                                    continue;
                                }

                                er.Score = (double)sr.Score;
                                decimal pricemeScore = 0m;
                                if (er.Score >= 0&&er.Score<=1)
                                    pricemeScore=1;
                                else
                                    pricemeScore = decimal.Round((sr.Score / 2m), 1);

                                er.PriceMeScore = (double)pricemeScore;

                                if (!string.IsNullOrEmpty(sr.Date))
                                    er.ReviewDate = DateTime.Parse(sr.Date);
                                er.RetailerCountry = 1;
                                er.IsExpertReview = sr.IsExpertReview;

                                //er.Save(); //服务器有问题加title
                                string sql = "Insert CSK_Store_ExpertReviewAU (ProductID,Title,Pros,Cons,Verdict,ReviewURL,SourceID,ReviewBy,ReviewDate,Score,CreatedOn,"
                                            + "CreatedBy,PriceMeScore,RetailerCountry,IsExpertReview,ModifiedOn,DisplayLinkStatus) values(@ProductID,@Title,@Pros,@Cons,@Verdict,@ReviewURL,@SourceID,@ReviewBy,@ReviewDate,@Score,@CreatedOn,"
                                            + "@CreatedBy,@PriceMeScore,@RetailerCountry,@IsExpertReview,@ModifiedOn,@DisplayLinkStatus)";
                                //string sql = "Insert CSK_Store_ExpertReviewAU (ProductID,Title,Description,Pros,Cons,Verdict,ReviewURL,SourceID,ReviewBy,ReviewDate,Score,CreatedOn,"
                                //            + "CreatedBy,ModifiedOn,ModifiedBy,PriceMeScore,RetailerCountry,Alascore,IsExpertReview) values("
                                //            + "" + er.ProductID + ", '" + er.Title + "', '', '" + er.Pros + "', '" + er.Cons + "', '" + er.Verdict + "', "
                                //            + "'" + er.ReviewURL + "', " + er.SourceID + ", '" + er.ReviewBy + "', '" + sr.Date + "', " + er.Score + ", "
                                //            + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'Import', '', '', " + er.PriceMeScore + ", '" + er.RetailerCountry + "', "
                                //            + "0, '" + er.IsExpertReview + "')";
                                //Writer(productId + "  \t  " + sql);

                                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                                {
                                    sqlCMD.CommandText = sql;
                                    sqlCMD.CommandTimeout = 0;
                                    sqlCMD.CommandType = System.Data.CommandType.Text;
                                    sqlCMD.Connection = sqlConn;

                                    System.Data.SqlClient.SqlParameter pidParameter = new System.Data.SqlClient.SqlParameter();
                                    pidParameter.ParameterName = "@ProductID";
                                    pidParameter.SqlDbType = System.Data.SqlDbType.Int;
                                    pidParameter.SqlValue = er.ProductID;
                                    sqlCMD.Parameters.Add(pidParameter);

                                    System.Data.SqlClient.SqlParameter sqlTitleParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlTitleParameter.ParameterName = "@Title";
                                    sqlTitleParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                                    sqlTitleParameter.SqlValue = er.Title;
                                    sqlCMD.Parameters.Add(sqlTitleParameter);

                                    System.Data.SqlClient.SqlParameter sqlProsParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlProsParameter.ParameterName = "@Pros";
                                    sqlProsParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                                    sqlProsParameter.SqlValue = er.Pros;
                                    sqlCMD.Parameters.Add(sqlProsParameter);

                                    System.Data.SqlClient.SqlParameter sqlConsParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlConsParameter.ParameterName = "@Cons";
                                    sqlConsParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                                    sqlConsParameter.SqlValue = er.Cons;
                                    sqlCMD.Parameters.Add(sqlConsParameter);

                                    System.Data.SqlClient.SqlParameter sqlVerdictParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlVerdictParameter.ParameterName = "@Verdict";
                                    sqlVerdictParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                                    sqlVerdictParameter.SqlValue = er.Verdict;
                                    sqlCMD.Parameters.Add(sqlVerdictParameter);

                                    System.Data.SqlClient.SqlParameter sqlReviewURLParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlReviewURLParameter.ParameterName = "@ReviewURL";
                                    sqlReviewURLParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                                    sqlReviewURLParameter.SqlValue = er.ReviewURL;
                                    sqlCMD.Parameters.Add(sqlReviewURLParameter);

                                    System.Data.SqlClient.SqlParameter sqlSourceIDParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlSourceIDParameter.ParameterName = "@SourceID";
                                    sqlSourceIDParameter.SqlDbType = System.Data.SqlDbType.Int;
                                    sqlSourceIDParameter.SqlValue = er.SourceID;
                                    sqlCMD.Parameters.Add(sqlSourceIDParameter);

                                    System.Data.SqlClient.SqlParameter sqlReviewByParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlReviewByParameter.ParameterName = "@ReviewBy";
                                    sqlReviewByParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                                    sqlReviewByParameter.SqlValue = er.ReviewBy;
                                    sqlCMD.Parameters.Add(sqlReviewByParameter);

                                    System.Data.SqlClient.SqlParameter sqlReviewDateParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlReviewDateParameter.ParameterName = "@ReviewDate";
                                    sqlReviewDateParameter.SqlDbType = System.Data.SqlDbType.DateTime;
                                    if (er.ReviewDate == null)
                                        sqlReviewDateParameter.SqlValue = DBNull.Value;
                                    else
                                        sqlReviewDateParameter.SqlValue = er.ReviewDate;
                                    sqlCMD.Parameters.Add(sqlReviewDateParameter);

                                    System.Data.SqlClient.SqlParameter sqlScoreParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlScoreParameter.ParameterName = "@Score";
                                    sqlScoreParameter.SqlDbType = System.Data.SqlDbType.Float;
                                    sqlScoreParameter.SqlValue = er.Score;
                                    sqlCMD.Parameters.Add(sqlScoreParameter);

                                    System.Data.SqlClient.SqlParameter sqlCreatedOnParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlCreatedOnParameter.ParameterName = "@CreatedOn";
                                    sqlCreatedOnParameter.SqlDbType = System.Data.SqlDbType.DateTime;
                                    sqlCreatedOnParameter.SqlValue = DateTime.Now;
                                    sqlCMD.Parameters.Add(sqlCreatedOnParameter);

                                    System.Data.SqlClient.SqlParameter sqlCreatedByParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlCreatedByParameter.ParameterName = "@CreatedBy";
                                    sqlCreatedByParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                                    sqlCreatedByParameter.SqlValue = "Improt";
                                    sqlCMD.Parameters.Add(sqlCreatedByParameter);

                                    System.Data.SqlClient.SqlParameter sqlPriceMeScoreParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlPriceMeScoreParameter.ParameterName = "@PriceMeScore";
                                    sqlPriceMeScoreParameter.SqlDbType = System.Data.SqlDbType.Float;
                                    sqlPriceMeScoreParameter.SqlValue = er.PriceMeScore;
                                    sqlCMD.Parameters.Add(sqlPriceMeScoreParameter);

                                    System.Data.SqlClient.SqlParameter sqlRetailerCountryParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlRetailerCountryParameter.ParameterName = "@RetailerCountry";
                                    sqlRetailerCountryParameter.SqlDbType = System.Data.SqlDbType.Int;
                                    sqlRetailerCountryParameter.SqlValue = er.RetailerCountry;
                                    sqlCMD.Parameters.Add(sqlRetailerCountryParameter);

                                    System.Data.SqlClient.SqlParameter sqlIsExpertReviewParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlIsExpertReviewParameter.ParameterName = "@IsExpertReview";
                                    sqlIsExpertReviewParameter.SqlDbType = System.Data.SqlDbType.Bit;
                                    sqlIsExpertReviewParameter.SqlValue = er.IsExpertReview;
                                    sqlCMD.Parameters.Add(sqlIsExpertReviewParameter);

                                    System.Data.SqlClient.SqlParameter sqlModifyOnParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlModifyOnParameter.ParameterName = "@ModifiedOn";
                                    sqlModifyOnParameter.SqlDbType = System.Data.SqlDbType.DateTime;
                                    sqlModifyOnParameter.SqlValue = DateTime.Now;
                                    sqlCMD.Parameters.Add(sqlModifyOnParameter);

                                    System.Data.SqlClient.SqlParameter sqlDisplayLinkStatusParameter = new System.Data.SqlClient.SqlParameter();
                                    sqlDisplayLinkStatusParameter.ParameterName = "@DisplayLinkStatus";
                                    sqlDisplayLinkStatusParameter.SqlDbType = System.Data.SqlDbType.Bit;
                                    sqlDisplayLinkStatusParameter.SqlValue = true;
                                    sqlCMD.Parameters.Add(sqlDisplayLinkStatusParameter);

                                    try
                                    {
                                        sqlCMD.CommandTimeout = 0;
                                        sqlCMD.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        Writer(productId + "  \t  " + sql);
                                        Writer(ex.Message);
                                        Writer(ex.StackTrace);
                                    }
                                }
                                //SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
                                //sp.Command.CommandSql = sql;
                                //sp.Command.CommandType = CommandType.Text;
                                //sp.Command.CommandTimeout = 0;
                                //sp.Execute();

                                PriceMe_ExpertAverageRatingTF pear = PriceMe_ExpertAverageRatingTF.SingleOrDefault(pe => pe.ProductID == productId);
                                if (pear == null)
                                {
                                    pear = new PriceMe_ExpertAverageRatingTF();
                                    pear.ProductID = productId;
                                    pear.CountryID = 3;
                                    if (sr.IsExpertReview)
                                    {
                                        pear.Votes = 1;
                                        if (er.PriceMeScore != null && er.PriceMeScore != 0)
                                        {
                                            pear.VotesHasScore = 1;
                                            pear.AverageRating = er.PriceMeScore ?? 0;
                                        }
                                        else
                                        {
                                            pear.AverageRating = 0;
                                            pear.VotesHasScore = 0;
                                        }
                                        pear.Votes = 1;
                                    }
                                    else
                                    {
                                        pear.Votes = 0;
                                        pear.VotesHasScore = 0;
                                        pear.AverageRating = 0;
                                        pear.UserAverageRating = er.PriceMeScore ?? 0;
                                        pear.UserVotes = 1;
                                    }
                                }
                                else
                                {
                                    if (er.PriceMeScore != null && er.PriceMeScore != 0)
                                    {
                                        if (sr.IsExpertReview)
                                        {
                                            double score = pear.VotesHasScore * pear.AverageRating;
                                            pear.VotesHasScore += 1;
                                            pear.AverageRating = (score + (er.PriceMeScore ?? 0)) / pear.VotesHasScore;
                                        }
                                        else
                                        {
                                            double score = pear.UserVotes * pear.UserAverageRating;
                                            pear.UserVotes += 1;
                                            pear.UserAverageRating = (score + (er.PriceMeScore ?? 0)) / pear.UserVotes;
                                        }
                                    }
                                    if (sr.IsExpertReview)
                                        pear.Votes += 1;
                                }
                                pear.Save();
                                #endregion
                            }
                            //else if (sr.SourceId == 1)  //TestFreaks 的评论要更新
                            else if (sr.SourceId > 0)  //所有的评论要更新
                            {
                                #region
                                string title = ReplacementString(sr.Summary);
                                if (title.Length > 500)
                                    title = title.Substring(0, 499);

                                string pros = ReplacementString(sr.Pros);
                                if (pros != null && pros.Length > 2000)
                                    pros = pros.Substring(0, 1999);

                                string cons = ReplacementString(sr.Cons);
                                if (cons != null && cons.Length > 2000)
                                    cons = cons.Substring(0, 1999);

                                string verdict = ReplacementString(sr.Extract);
                                if (verdict != null && verdict.Length > 2000)
                                    verdict = verdict.Substring(0, 1999);

                                string url = string.Empty;
                                if (!string.IsNullOrEmpty(sr.Url))
                                {
                                    url = sr.Url.Replace("http://", "");
                                    if (url.Contains("/"))
                                        url = url.Substring(url.IndexOf('/'));
                                }

                                string reviewDate = string.Empty;
                                if (!string.IsNullOrEmpty(sr.Date))
                                    reviewDate = sr.Date;

                                title = title.Replace("'", "''");
                                pros = pros.Replace("'", "''");
                                cons = cons.Replace("'", "''");
                                verdict = verdict.Replace("'", "''");
                                url = url.Replace("'", "''");
                                string sql = "Update CSK_Store_ExpertReviewAU "
                                            + "Set Title = '" + title + "', Pros = '" + pros + "', Cons = '" + cons + "', Verdict = '" + verdict + "', "
                                            + "ReviewURL = '" + url + "', ReviewBy = '" + sr.Author + "', ReviewDate = '" + reviewDate + "', ModifiedOn= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                            + "' , DisplayLinkStatus=1 Where ProductID = " + productId + " And SourceID = " + sid;
                                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
                                sp.Command.CommandSql = sql;
                                sp.Command.CommandType = CommandType.Text;
                                sp.Command.CommandTimeout = 0;
                                sp.Execute();
                                #endregion
                            }
                            else
                            {
                                //此处代表匹配到

                                if (!needUpdateModifyOn.Contains(keyUp))
                                    needUpdateModifyOn.Add(keyUp);
                            }
                        }
                        catch (Exception ex) { Writer("Save Error: " + ex.Message + ex.StackTrace + "\n ReviewURL:" + sr.Url + " \n ReviewBy:" + sr.Author); }
                    }
                    //删除无效评论（即数据库有，Feed里面没有）
                    expertCache.ExpertReviewList.ForEach(f => {
                        var p_id = f.Split('|')[0];
                        var s_id = f.Split('|')[1];
                        var sCount= srList.Where(s => s.SourceId == int.Parse(s_id)&&p_id==productId.ToString()).Count();
                        if (sCount <= 0) {
                            string sql6 = "update CSK_Store_ExpertReviewAU set ModifiedOn= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', DisplayLinkStatus=0 where ProductID=" + pid + " and SourceID=" + s_id + "";
                            var sp6 = new SubSonic.Schema.StoredProcedure("");
                            sp6.Command.CommandSql = sql6;
                            sp6.Command.CommandType = CommandType.Text;
                            sp6.Command.CommandTimeout = 0;
                            sp6.Execute();
                        }

                    });

                    //update modifyon
                    //string updModifyOnSQL = "Update CSK_Store_ExpertReviewAU "
                    //                       + "Set ModifiedOn= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',DisplayLinkStatus=1"
                    //                       + " Where ";//ProductID = " + productId + " And SourceID = " + "sid";
                    for (int i = 0; i < needUpdateModifyOn.Count; i++)
                    {
                        string[] temp = needUpdateModifyOn[i].Split('|');
                        string proid = temp[0];
                        string sid = temp[1];
                        string furl = temp[2];
                        string updModifyOnSQL = "Update CSK_Store_ExpertReviewAU "
                                          + "Set ModifiedOn= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',DisplayLinkStatus=1,ReviewURL='" + furl + "'"
                                          + " Where ";
                        updModifyOnSQL += "(ProductID = " + productId + " And SourceID = " + sid + ")";

                        string sql = updModifyOnSQL;
                        SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
                        sp.Command.CommandSql = sql;
                        sp.Command.CommandType = CommandType.Text;
                        sp.Command.CommandTimeout = 0;
                        sp.Execute();
                        #region 老代码

                        //if (needUpdateModifyOn.Count == 1)
                        //{
                        //    updModifyOnSQL += "(ProductID = " + productId + " And SourceID = " + sid + ")";

                        //    string sql = updModifyOnSQL;
                        //    SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
                        //    sp.Command.CommandSql = sql;
                        //    sp.Command.CommandType = CommandType.Text;
                        //    sp.Command.CommandTimeout = 0;
                        //    sp.Execute();
                        //    continue;
                        //}
                        //if ((i % 50 == 0 || i == needUpdateModifyOn.Count - 1) && i != 0)
                        //{
                        //    updModifyOnSQL += "(ProductID = " + productId + " And SourceID = " + sid + ")";

                        //    string sql = updModifyOnSQL;
                        //    SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
                        //    sp.Command.CommandSql = sql;
                        //    sp.Command.CommandType = CommandType.Text;
                        //    sp.Command.CommandTimeout = 0;
                        //    sp.Execute();
                        //    updModifyOnSQL = "Update CSK_Store_ExpertReviewAU "
                        //                          + "Set ModifiedOn= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',DisplayLinkStatus=1"
                        //                          + " Where ";
                        //}
                        //else
                        //{
                        //    updModifyOnSQL += "(ProductID = " + productId + " And SourceID = " + sid + ") or ";
                        //}

                        #endregion
                    }



                    foreach (Video video in videoList)
                    {
                        if (ExistVideos.Contains(video.Url)) continue;
                        try
                        {
                            CSK_Store_ProductVideoSource videoSource = new CSK_Store_ProductVideoSource();
                            videoSource.SourceUrl = video.Url;
                            videoSource.Type = video.Url.Substring(0, video.Url.IndexOf(".com")).Replace("http://www.", "");
                            videoSource.CreatedBy = "Dawn";
                            videoSource.CreatedOn = DateTime.Now;
                            videoSource.Save();
                            ExistVideos.Add(video.Url);
                            //http://www.youtube.com/watch?v=J5tBKpdsEZI

                            CSK_Store_ProductVideo productVideo = new CSK_Store_ProductVideo();
                            productVideo.ProductID = productId;
                            productVideo.VideoSourceID = videoSource.ID;
                            productVideo.Thumbnail = video.Thumbnail;
                            if (videoSource.Type == "youtube")
                            {
                                if (videoSource.SourceUrl.Contains("watch?"))
                                    productVideo.Url = videoSource.SourceUrl.Replace("watch?v=", "embed/");
                            }
                            productVideo.CreatedBy = "Dawn";
                            productVideo.CreatedOn = DateTime.Now;

                            productVideo.Save();


                        }
                        catch (Exception ex)
                        {
                            Writer("Add " + productId + " ProductVideo Error: " + ex.Message + ex.StackTrace);
                        }
                    }
                }
                catch (Exception e)
                {
                    Writer("SaveData() Error: " + e.Message + e.StackTrace);
                }
            }
        }

        private void BindReviewSource()
        {
            string sql = "select SourceID, SourceID1,LogoFile from dbo.CSK_Store_ReviewSourceAU";
            using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
            {
                sqlCMD.CommandText = sql;
                sqlCMD.CommandTimeout = 0;
                sqlCMD.CommandType = System.Data.CommandType.Text;
                sqlCMD.Connection = sqlConn;

                using (IDataReader dr = sqlCMD.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int sid = 0;
                        int.TryParse(dr["SourceID"].ToString(), out sid);
                        int sid1 = 0;
                        int.TryParse(dr["SourceID1"].ToString(), out sid1);
                        string logo = dr["LogoFile"].ToString();
                        if (!sourceidDic.ContainsKey(sid1))
                            sourceidDic.Add(sid1, sid);
                        if (!sourceLogoDic.ContainsKey(sid1))
                        {
                            //CSK_Store_ReviewSourceAU srau = new CSK_Store_ReviewSourceAU();
                            //srau.SourceID = sid;
                            //srau.SourceID1 = sid1;
                            //srau.LogoFile = logo;
                            sourceLogoDic.Add(sid1, logo);
                        }
                    }
                }
            }
        }

        private int SaveReviewSource(SourceReview sr)
        {
            if (sourceidDic.ContainsKey(sr.SourceId))
            {
                //if sr.SourceLogo exist in feed and 图片有效 ,download
                if (sourceLogoDic.ContainsKey(sr.SourceId) && String.IsNullOrEmpty(sourceLogoDic[sr.SourceId]) && !String.IsNullOrEmpty(sr.SourceLogo))
                {
                    string LogoFile = DownloadImage(sr.SourceLogo);
                    //sourceLogoDic[sr.SourceId].LogoFile = DownloadImage(sr.SourceLogo);
                    //sourceLogoDic[sr.SourceId].Save();
                    string sql = "update CSK_Store_ReviewSourceAU set LogoFile='" + LogoFile + "' where SourceID1 = "+sr.SourceId;
                    var sp = new SubSonic.Schema.StoredProcedure("");
                    sp.Command.CommandSql = sql;
                    sp.Command.CommandType = CommandType.Text;
                    sp.Command.CommandTimeout = 0;
                    sp.Execute();
                    
                }
                
                return sourceidDic[sr.SourceId];
            }
            else
            {
                CSK_Store_ReviewSourceAU srs = new CSK_Store_ReviewSourceAU();
                srs.WebSiteName = sr.SourceName;
                if (srs.WebSiteName.Length > 100)
                    srs.WebSiteName = srs.WebSiteName.Substring(0, 99);
                srs.LogoFile = DownloadImage(sr.SourceLogo);
                srs.SourceID1 = sr.SourceId;
                
                srs.Save();
                Writer("save to CSK_Store_ReviewSourceAU: (srs.WebSiteName = sr.SourceName => " + srs.WebSiteName + "=" + sr.SourceName + ")");
                sourceidDic.Add(sr.SourceId, srs.SourceID);

                return srs.SourceID;
            }
        }

        private string DownloadImage(string imgUrl)
        {
            string logoFile = "";
            try
            {
                if (!Directory.Exists(driverPath))
                    Directory.CreateDirectory(driverPath);

                string imgName = imgUrl.Substring(imgUrl.LastIndexOf('/') + 1);
                WebClient webClient = new WebClient();
                webClient.DownloadFile(imgUrl, (driverPath + imgName));
                logoFile = imageLogo + imgName;
            }
            catch { }

            return logoFile;
        }

        private string ReplacementString(string info)
        {
            if (!string.IsNullOrEmpty(info))
                info = info.Replace("—", "-").Replace("’", "'").Replace("“", "\"").Replace("”", "\"").Replace("�", "");
            else if (info == null)
                info = string.Empty;

            return info;
        }

        private void Writer(string info)
        {
            _sw.WriteLine(info);
            _sw.Flush();
        }


        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件人</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <param name="to">收件人</param>
        public static void sendEmail(string from, string subject, string body, params string[] to)
        {
            string _aWSAccessKey = "0ZY4PSJK9RC3FMPQVB02";
            string _aWSSecretKey = "rRK5GHwelS25Yk5XOLDYLuN8W9I0czKys5n+Tu1D";
            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(_aWSAccessKey, _aWSSecretKey);

            SendEmailRequest seReq = new SendEmailRequest();
            seReq.Source = from;

            Destination det = new Destination();
            det.ToAddresses = to.ToList();

            seReq.Destination = det;

            Message mes = new Message();
            Content con = new Content();
            con.Data = subject;
            mes.Subject = con;

            Body bodyObj = new Body();
            Content conHtml = new Content();
            conHtml.Data = body;
            bodyObj.Text = conHtml;
            bodyObj.Html = conHtml;
            mes.Body = bodyObj;

            seReq.Message = mes;

            //list = new List<string>();
            //list.Add(ConfigAppString.ReplyToEmail);
            //seReq.ReplyToAddresses = list;

            seReq.ReplyToAddresses = new List<string>() { from };

            ses.SendEmail(seReq);
        }
    }
}
