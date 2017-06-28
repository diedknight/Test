using AliexpressTool.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliexpressTool.DisposableTool
{
    public class ShopAttribute
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["CommerceTemplate"].ToString();

        List<ProductData> datas;
        Dictionary<int, string> dicManu;

        public void Attribute()
        {
            GetAllManufacturer();
            GetProducts();

            SetShopAttribute();
        }

        public void GetAllManufacturer()
        {
            dicManu = new Dictionary<int, string>();
            var connectionStr = ConfigurationManager.ConnectionStrings["Pricealyser"].ToString();
            string sql = "Select ManufacturerID, ManufacturerName From CSK_Store_Manufacturer Where ManufacturerID != -1 order by ManufacturerID";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        int mid = 0;
                        int.TryParse(idr["ManufacturerID"].ToString(), out mid);

                        if (!dicManu.ContainsKey(mid))
                            dicManu.Add(mid, idr["ManufacturerName"].ToString());
                    }
                }
            }
        }

        public string GetManufacturer(int mid)
        {
            string manu = string.Empty;
            if (dicManu.ContainsKey(mid))
                manu = dicManu[mid];

            return manu;
        }

        public void GetProducts()
        {
            datas = new List<ProductData>();
            var connectionStr = ConfigurationManager.ConnectionStrings["Pricealyser"].ToString();
            string sql = "select p.ProductName, p.ManufacturerID, temp.* from csk_store_product p "
                       + "inner join (Select p.*, rp.RetailerProductSKU From [WEB].[priceme_nz].dbo.csk_store_RelatedParts p inner join CSK_Store_RetailerProduct rp on p.ShopProductId = rp.ProductId Where RetailerId = 2255) as temp on p.ProductID = temp.productId";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        int spid = 0, pid = 0, skuid = 0, manuid = 0;
                        int.TryParse(idr["ShopProductId"].ToString(), out spid);
                        int.TryParse(idr["productId"].ToString(), out pid);
                        int.TryParse(idr["RetailerProductSKU"].ToString(), out skuid);
                        int.TryParse(idr["ManufacturerID"].ToString(), out manuid);
                        string productname = idr["ProductName"].ToString();
                        
                        ProductData data = datas.SingleOrDefault(d => d.ShopProductId == spid);
                        if (data == null)
                        {
                            data = new ProductData();
                            data.ShopProductId = spid;
                            data.ShopNZProductId = skuid;
                            data.SuitableProducts = productname;
                            data.Manufacturer = GetManufacturer(manuid);
                            List<int> listp = new List<int>();
                            listp.Add(pid);
                            data.ListProduct = listp;

                            datas.Add(data);
                        }
                        else if(!data.ListProduct.Contains(pid))
                        {

                            ProductData temp = data;
                            datas.Remove(data);

                            temp.ListProduct.Add(pid);
                            temp.SuitableProducts = temp.SuitableProducts + ", " + productname;
                            temp.Manufacturer = temp.Manufacturer + ", " + GetManufacturer(manuid);

                            datas.Add(temp);
                        }
                    }
                }
            }
        }

        public void SetShopAttribute()
        {
            foreach (ProductData data in datas)
            {
                SetSuitableforProducts(data);

                SetSuitableforBrand(data);
            }
        }

        private void SetSuitableforProducts(ProductData data)
        {
            string stringAtt = "<ul>";
            string[] temps = data.SuitableProducts.Split(new string[] { ", " }, StringSplitOptions.None);
            foreach (string temp in temps)
            {
                stringAtt += "<li type=\"square\">" + temp + "</li>";
            }
            stringAtt += "</ul>";
            stringAtt = stringAtt.Replace("'", "''");

            int id = GetSuitable("Select Id from dbo.Product_SpecificationAttribute_Mapping Where ProductId = " + data.ShopNZProductId + " And AttributeTypeId = 20 And SpecificationAttributeOptionId = 9");

            string sql = "Insert Product_SpecificationAttribute_Mapping values(" + data.ShopNZProductId + ", 20, 9, '" + stringAtt + "', 0, 1, 0)";
            if (id > 0)
                sql = "Update Product_SpecificationAttribute_Mapping Set CustomValue = '" + stringAtt + "' Where Id = " + id;

            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void SetSuitableforBrand(ProductData data)
        {
            string[] temps = data.Manufacturer.Split(new string[] { ", " }, StringSplitOptions.None);
            foreach (string temp in temps)
            {
                if (string.IsNullOrEmpty(temp)) continue;

                int id = 0;
                string sql = "Select Id From SpecificationAttributeOption Where SpecificationAttributeId = 4 And Name = '" + temp + "'";
                using (SqlConnection conn = new SqlConnection(connectionStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    using (IDataReader idr = cmd.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            int.TryParse(idr["Id"].ToString(), out id);
                        }
                    }
                }

                if (id == 0)
                {
                    sql = "Insert SpecificationAttributeOption Values(4, '" + temp + "', '', 0)Select @@Identity";
                    using (SqlConnection conn = new SqlConnection(connectionStr))
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = 0;
                        conn.Open();
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                int sid = GetSuitable("Select Id from dbo.Product_SpecificationAttribute_Mapping Where ProductId = " + data.ShopNZProductId + " And AttributeTypeId = 0 And SpecificationAttributeOptionId = " + id);

                if (sid == 0)
                {
                    sql = "Insert Product_SpecificationAttribute_Mapping values(" + data.ShopNZProductId + ", 0, " + id + ", '', 1, 0, 0)";
                    using (SqlConnection conn = new SqlConnection(connectionStr))
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = 0;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private int GetSuitable(string sql)
        {
            int id = 0;
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                IDataReader idr = cmd.ExecuteReader();
                while (idr.Read())
                {
                    int.TryParse(idr["Id"].ToString(), out id);
                }
                idr.Close();
            }

            return id;
        }
    }
}
