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

            if (this._configInfo.Name.ToLower().Contains("csk_store_product"))
            {
                sql += " and " + this._configInfo.IdCol + " not in (select id from (select pid as id from CreateImageForS3_RecordHandledPIDAndRPID) as a where " + System.Configuration.ConfigurationManager.AppSettings["IDRange"] + ")";
            }

            if (this._configInfo.Name.ToLower().Contains("csk_store_retailerproduct"))
            {
                sql += " and " + this._configInfo.IdCol + " not in (select id from (select rpid as id from CreateImageForS3_RecordHandledPIDAndRPID) as a where " + System.Configuration.ConfigurationManager.AppSettings["IDRange"] + ")";
            }

            sql += " and " + this._configInfo.ImageCol + " not in (select ImagePath from CreateImageForS3_RecordHandledPIDAndRPID)";

            return sql;
        }

        private string GetUpdateSql(string tableName)
        {
            string sql = " INSERT INTO [dbo].[CreateImageForS3_RecordHandledPIDAndRPID] ([PID],[RPID],[ImagePath]) VALUES";

            if (tableName.ToLower().Contains("csk_store_product"))
            {
                sql += " (@Id,0,@Image)";
            }

            if (tableName.ToLower().Contains("csk_store_retailerproduct"))
            {
                sql += " (0,@Id,@Image)";
            }

            return sql;
        }

        public void ForEach(Action<DataInfo, string> action)
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
                if (!item.Image.ToLower().Contains("s3.pricemestatic.com")) return;

                //item.Image = item.Image.Replace(" ", "");
                action(item, this.ConfigInfo.Name);
            });
        }

        public void Update(int Id, string image)
        {
            if (this._configInfo.Name.ToLower().Contains("csk_store_product") || this._configInfo.Name.ToLower().Contains("csk_store_retailerproduct"))
            {

                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["PriceMeTemplate"].ConnectionString;
                string sql = this.GetUpdateSql(this._configInfo.Name);

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Execute(sql, new { Id = Id, Image = image });
                }
            }
        }

    }
}
