using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackFriday.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;

namespace BlackFriday.Pages
{
    public class IndexModel : PageModel
    {
        private AppConfig _appConfig = null;

        public string Description { get; set; }

        public IndexModel(AppConfig appConfig)
        {
            this._appConfig = appConfig;
        }


        public void OnGet()
        {
            using (var con = new MySqlConnection(this._appConfig.PricemeDBConnection.ConnectionString))
            {
                string sql = "select Ctx from CSK_Content where PageId='Black Friday information'";

                this.Description = con.ExecuteScalar<string>(sql);
            }
        }

        public void OnPost(string txtEmail)
        {            
            using (var con = new MySqlConnection(this._appConfig.PricemeDBConnection.ConnectionString))
            {
                string sql = "INSERT INTO WeeklyDealsUser (emailaddress) VALUES (@email)";

                con.Execute(sql, new { email = txtEmail });
            }

            this.ViewData["signup"] = "Thank you for signing up";
            OnGet();
        }

    }
}
