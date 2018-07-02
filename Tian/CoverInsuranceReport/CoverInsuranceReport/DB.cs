using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using CoverInsuranceReport.Data;
using System.IO;

namespace CoverInsuranceReport
{
    public class DB
    {

        private static CateCtrl _cate = new CateCtrl();

        public static SqlConnection PricemeDB
        {
            get
            {
                return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString);
            }
        }

        public static SqlConnection EDWDB
        {
            get
            {
                return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EDW"].ConnectionString);
            }
        }


        private static List<T> Get<T>()
        {
            List<T> list = new List<T>();

            var cids = _cate.GetData().Select(item => item.CId).ToList();

            using (var sr = System.IO.File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Sql", typeof(T).Name + ".sql")))
            {
                string sql = sr.ReadToEnd();

                using (var con = PricemeDB)
                {
                    list = con.Query<T>(sql, new { Cids = cids }, null, true, 40000).ToList();
                }
            }

            return list;
        }

        public static List<ActiveData> GetActive()
        {
            return Get<ActiveData>();
        }

        public static List<InactiveData> GetInActive()
        {
            return Get<InactiveData>();
        }

        public static List<UpComingData> GetUpComing()
        {
            return Get<UpComingData>();
        }


        private static List<T> GetPrice<T>(List<int> pids)
        {
            List<T> list = new List<T>();

            SplitIds((ids) =>
            {
                using (var sr = System.IO.File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Sql", typeof(T).Name + ".sql")))
                {
                    string sql = sr.ReadToEnd();

                    using (var con = PricemeDB)
                    {
                        list.AddRange(con.Query<T>(sql, new { Pids = ids }, null, true, 40000).ToList());
                    }
                }
            }, pids);

            return list;
        }

        public static List<ActivePriceData> GetActivePrice(List<int> pids)
        {
            return GetPrice<ActivePriceData>(pids);
        }

        public static List<InactivePriceData> GetInactivePrice(List<int> pids)
        {
            return GetPrice<InactivePriceData>(pids);
        }

        public static List<UpComingPriceData> GetUpComingPrice(List<int> pids)
        {
            return GetPrice<UpComingPriceData>(pids);
        }

        private static List<T> GetHistoricalPrice<T>(List<int> pids)
        {
            List<T> list = new List<T>();

            List<int> rids = new List<int>();
            using (var con = PricemeDB)
            {
                rids = con.Query<int>("select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and RetailerCountry = 3", null, null, true, 40000).ToList();
            }

            SplitIds((ids) =>
            {
                using (var sr = System.IO.File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Sql", typeof(T).Name + ".sql")))
                {
                    string sql = sr.ReadToEnd();

                    using (var con = EDWDB)
                    {
                        list.AddRange(con.Query<T>(sql, new { Pids = ids, Rids = rids }, null, true, 40000).ToList());
                    }
                }
            }, pids);

            return list;
        }

        public static List<HistoricalPriceData> GetHistoricalPrice(List<int> pids)
        {
            List<HistoricalPriceData> list = new List<HistoricalPriceData>();

            var pricelist = GetHistoricalPrice<ActiveHistoricalPriceData>(pids);

            pids.ForEach(pid =>
            {
                DateTime _30Days = DateTime.Now.AddDays(-30);
                DateTime _180Days = DateTime.Now.AddDays(-180);

                var tempList = pricelist.Where(item => item.PId == pid).ToList();
                if (tempList.Count != 0)
                {
                    var _30List = tempList.Where(item => item.CreatedOn > _30Days).OrderBy(item => item.NewPrice).Select(item => item.NewPrice).ToList();
                    var _180List = tempList.Where(item => item.CreatedOn > _180Days).OrderBy(item => item.NewPrice).Select(item => item.NewPrice).ToList();

                    HistoricalPriceData info = new HistoricalPriceData();
                    info.PId = pid;
                    if (_30List.Count != 0)
                    {
                        info.minPrice_30 = _30List.Min();
                        info.maxPrice_30 = _30List.Max();

                        var val1 = _30List[(_30List.Count + 1) / 2 - 1];
                        var val2 = _30List[(_30List.Count + 2) / 2 - 1];

                        info.medianPrice_30 = (val1 + val2) / 2;
                    }

                    if (_180List.Count != 0)
                    {
                        info.minPrice_180 = _180List.Min();
                        info.maxPrice_180 = _180List.Max();
                        info.avgPrice_180 = _180List.Average();

                        var val1 = _180List[(_180List.Count + 1) / 2 - 1];
                        var val2 = _180List[(_180List.Count + 2) / 2 - 1];

                        info.medianPrice_180 = (val1 + val2) / 2;
                    }
                    list.Add(info);
                }
            });

            return list;
        }




        public static List<UpdateOnData> GetUpdate(List<int> pids)
        {
            List<UpdateOnData> list = new List<UpdateOnData>();

            SplitIds((ids) =>
            {
                using (var sr = System.IO.File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Sql", "UpdateOnData.sql")))
                {
                    string sql = sr.ReadToEnd();

                    using (var con = PricemeDB)
                    {
                        list.AddRange(con.Query<UpdateOnData>(sql, new { Pids = ids }, null, true, 40000).ToList());
                    }
                }
            }, pids);

            return list;
        }

        public static List<ImageData> GetImage(List<int> pids)
        {
            List<ImageData> list = new List<ImageData>();


            SplitIds((ids) =>
            {
                using (var sr = System.IO.File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Sql", "ImageData.sql")))
                {
                    string sql = sr.ReadToEnd();

                    using (var con = PricemeDB)
                    {
                        list.AddRange(con.Query<ImageData>(sql, new { Pids = ids }, null, true, 40000).ToList());
                    }
                }
            }, pids);

            list.ForEach(item =>
            {
                if (string.IsNullOrEmpty(item.DefaultImage)) return;

                item.DefaultImage = item.DefaultImage.Insert(item.DefaultImage.LastIndexOf('.'), "_l");
                if (!item.DefaultImage.ToLower().Contains("https://"))
                {
                    item.DefaultImage = "https://images.pricemestatic.com/Large" + item.DefaultImage.Replace("\\", "/");
                }
                else
                {
                    Uri u = new Uri(item.DefaultImage);

                    item.DefaultImage = u.Scheme + "://" + u.Host + "/Large" + u.AbsolutePath;
                }
            });

            return list;
        }

        public static void SplitIds(Action<List<int>> action, List<int> Ids)
        {
            if (Ids.Count <= 1000)
            {
                action(Ids);
            }
            else
            {
                List<int> tempList = new List<int>();

                Ids.ForEach(id =>
                {
                    tempList.Add(id);
                    if (tempList.Count == 1000)
                    {
                        action(tempList);
                        tempList = new List<int>();
                    }
                });

                if (tempList.Count != 0)
                {
                    action(tempList);
                }
            }
        }


    }
}
