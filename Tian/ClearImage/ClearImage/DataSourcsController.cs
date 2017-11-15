using ClearImage.Config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearImage
{
    public class DataSourcsController
    {
        private static string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["PriceMeTemplate"].ConnectionString;

        public static List<SourcsData> GetSourcsData(string sql)
        {
            List<SourcsData> list = new List<SourcsData>();

            using (SqlConnection conn = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        int id = 0;
                        int.TryParse(idr["Id"].ToString(), out id);
                        SourcsData data = new SourcsData();
                        data.Id = id;
                        data.FilePath = idr["DefaultImage"].ToString().ToLower();

                        list.Add(data);
                    }
                }
            }

            return list;
        }

        private static void UpdateSql(string sql)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdataSourcsData(string imagefile, string url)
        {
            if (Program.key == "product")
            {
                List<SourcsData> rplist = Program.listRetailerproduct.Where(l => l.FilePath == imagefile).ToList();
                if (rplist != null && rplist.Count > 0)
                {
                    string sql = "update CSK_Store_retailerProduct set DefaultImage = '" + url + "' where RetailerProductId ";
                    sql += GetWhere(rplist);
                    UpdateSql(sql);
                }

                List<SourcsData> ilist = Program.listImage.Where(l => l.FilePath == imagefile).ToList();
                if (ilist != null && ilist.Count > 0)
                {
                    string sql = "update CSK_Store_Image set ImageFile = '" + url + "' where ImageId ";
                    sql += GetWhere(ilist);
                    UpdateSql(sql);
                }
            }
            else if (Program.key == "retailerproduct")
            {
                List<SourcsData> plist = Program.listProduct.Where(l => l.FilePath == imagefile).ToList();
                if (plist != null && plist.Count > 0)
                {
                    string sql = "update CSK_Store_Product set DefaultImage = '" + url + "' where ProductId ";
                    sql += GetWhere(plist);
                    UpdateSql(sql);
                }

                List<SourcsData> ilist = Program.listImage.Where(l => l.FilePath == imagefile).ToList();
                if (ilist != null && ilist.Count > 0)
                {
                    string sql = "update CSK_Store_Image set ImageFile = '" + url + "' where ImageId ";
                    sql += GetWhere(ilist);
                    UpdateSql(sql);
                }
            }
            else if (Program.key == "image")
            {
                List<SourcsData> plist = Program.listProduct.Where(l => l.FilePath == imagefile).ToList();
                if (plist != null && plist.Count > 0)
                {
                    string sql = "update CSK_Store_Product set DefaultImage = '" + url + "' where ProductId ";
                    sql += GetWhere(plist);
                    UpdateSql(sql);
                }

                List<SourcsData> rplist = Program.listRetailerproduct.Where(l => l.FilePath == imagefile).ToList();
                if (rplist != null && rplist.Count > 0)
                {
                    string sql = "update CSK_Store_retailerProduct set DefaultImage = '" + url + "' where RetailerProductId ";
                    sql += GetWhere(rplist);
                    UpdateSql(sql);
                }
            }
        }

        private static string GetWhere(List<SourcsData> list)
        {
            string stringwhere = string.Empty;

            if (list.Count == 1)
                stringwhere = " = " + list[0].Id;
            else
            {
                string ids = string.Empty;
                foreach (SourcsData data in list)
                {
                    ids += data.Id + ", ";
                }
                ids = ids.Substring(0, ids.LastIndexOf(',')).Trim();

                stringwhere = " in (" + ids + ")";
            }

            return stringwhere;
        }
    }
}
