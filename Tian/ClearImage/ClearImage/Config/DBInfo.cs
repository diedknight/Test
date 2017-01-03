using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace ClearImage.Config
{
    public class DBInfo
    {
        private ConfigInfo _configInfo = null;

        public ConfigInfo ConfigInfo { get { return this._configInfo; } }

        public DBInfo(ConfigInfo info)
        {
            this._configInfo = info;
        }

        private string GetSelectSql()
        {
            string sql = "select ";

            if (this._configInfo.Top != 0) sql += " top " + this._configInfo.Top;

            sql += " " + this._configInfo.IdCol + " as [Id],";
            sql += " " + this._configInfo.ImageCol + " as [Image]";
            sql += " from";
            sql += " " + this._configInfo.Name;
            sql += " where " + this._configInfo.Where;

            return sql;
        }

        private string GetUpdateSql()
        {
            string sql = "update ";
            sql += " " + this._configInfo.Name;
            sql += " set " + this._configInfo.ImageCol + "=@Image";
            sql += " where " + this._configInfo.IdCol + "=@Id";

            return sql;
        }

        public void ForEach(Action<DataInfo> action)
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["PriceMeTemplate"].ConnectionString;
            string sql = this.GetSelectSql();

            List<DataInfo> list = new List<DataInfo>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                list = con.Query<DataInfo>(sql).ToList();
            }

            list.ForEach(item =>
            {
                //item.DB = this;

                if (string.IsNullOrEmpty(item.Image)) return;
                if (item.Image.ToLower().Contains("s3.pricemestatic.com")) return;

                //item.Image = item.Image.Replace(" ", "");
                action(item);
            });
        }

        public void Update(int Id, string image)
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["PriceMeTemplate"].ConnectionString;
            string sql = this.GetUpdateSql();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Execute(sql, new { Id = Id, Image = image });
            }
        }

    }
}
