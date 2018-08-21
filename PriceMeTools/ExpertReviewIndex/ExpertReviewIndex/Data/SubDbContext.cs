using ExpertReviewIndex.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExpertReviewIndex.Data
{
    public class SubDbContext : DbContext
    {
        //public DbSet<CskStoreCategory> CSK_Store_Category { get; set; }

        //public SubDbContext(DbContextOptions<SubDbContext> options) : base(options)
        //{

        //}
    }
}


//class DBController {
//    public static SubDbContext SubDb;

//    public static void LoadDBConnection()
//    {
//        DbContextOptionsBuilder<SubDbContext> optionsBuilder = new DbContextOptionsBuilder<SubDbContext>();
//        optionsBuilder.UseMySql(SiteConfig.AppSettings("conn_mysql"));

//        SubDb = new SubDbContext(optionsBuilder.Options);
//        var test = SubDb.CSK_Store_Category.Where(c=>c.ToString)
//    }
//}