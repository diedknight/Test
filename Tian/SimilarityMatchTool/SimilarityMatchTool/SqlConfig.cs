using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarityMatchTool
{
    public class SqlConfig
    {
        public static string cache1 { get; set; }
        public static string cache2 { get; set; }

        public static string InsertSimilarityMatchReport { get; set; }

        static SqlConfig()
        {
            using (var sr = System.IO.File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Sql", "cache1.sql")))
            {
                cache1 = sr.ReadToEnd();
            }

            using (var sr = System.IO.File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Sql", "cache2.sql")))
            {
                cache2 = sr.ReadToEnd();
            }

            using (var sr = System.IO.File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Sql", "InsertSimilarityMatchReport.sql")))
            {
                InsertSimilarityMatchReport = sr.ReadToEnd();
            }

        }

    }
}
