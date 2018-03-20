using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliexpressImport.Data;
using AliexpressDBA;
using System.IO;
using SubSonic.Schema;
using System.Data;
using System.Diagnostics;

namespace AliexpressImport.BusinessLogic
{
    public static class ProductContronller
    {
        public static int ProductMatching(ProductInfoEntity info)
        {
            int type = 0;

            Product product = Product.SingleOrDefault(p => p.AdminComment == info.AdminComment);
            if (product == null)
                product = Product.SingleOrDefault(p => p.Sku == info.Sku);

            if (product != null && product.Price > 0m)
            {
                ProductUpdate(product, info);
                type = 2;
            }

            return type;
        }
        
        private static void ProductUpdate(Product product, ProductInfoEntity info)
        {
            product.AdminComment = info.AdminComment;
            product.Sku = info.Sku;
            product.Price = info.Price * 1.4m + info.Shipping;
            product.ProductCost = info.Price + info.Shipping;
            product.Deleted = false;
            product.UpdatedOnUtc = DateTime.Now;
            product.Save();
        }
        
        public static int GetAllProductCountByCategory(int cid)
        {
            int count = 0;
            string sql = "Select COUNT(Id) as cun From Product Where Deleted = 0 And Id in (Select ProductId From Product_Category_Mapping Where CategoryId = " + cid + ")";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int.TryParse(dr["cun"].ToString(), out count);
            }
            dr.Close();
            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "all count: " + sql);

            return count;
        }

        public static int GetUpdateProductCountByCategory(int cid, DateTime startTime)
        {
            int count = 0;
            string sql = "Select COUNT(Id) as cun From Product Where Deleted = 0 And UpdatedOnUtc > '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' And Id in (Select ProductId From Product_Category_Mapping Where CategoryId = " + cid + ")";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int.TryParse(dr["cun"].ToString(), out count);
            }
            dr.Close();
            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Update count: " + sql);

            return count;
        }

        public static void UpdateProductBySql(string sql)
        {
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            sp.Execute();
        }
    }
}
