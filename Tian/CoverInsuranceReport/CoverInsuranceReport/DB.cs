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
