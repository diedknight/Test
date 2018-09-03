using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace ImportAttrs.Data
{
    public class UnmatchReportData
    {
        public int ID { get; set; }
        public int RID { get; set; }
        public int CID { get; set; }
        public int PID { get; set; }
        public int AttType { get; set; }
        public int AttTitleID { get; set; }
        public string PM_AttName { get; set; }
        public string DR_AttName { get; set; }
        public string DR_AttValue_Orignal { get; set; }
        public string DR_AttValue_Changed { get; set; }
        public bool Status { get; set; }

        public void Update(int status, string DR_AttValue_Orignal)
        {
            var tempStatus = status != 0;
            if (tempStatus == this.Status && DR_AttValue_Orignal == this.DR_AttValue_Orignal) return;
            
            string sql = "update AttributeUnmatchedReport set Status=" + status + ", DR_AttValue_Orignal='" + DR_AttValue_Orignal + "' where ID=" + ID;

            using (var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PriceMe_DB"].ConnectionString))
            {
                con.Execute(sql);
            }
        }


    }
}
