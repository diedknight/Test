using com.alibaba.openapi.client.policy;
using com.alibaba.openapi.sdk.aeopenapi.param;
using com.alibaba.openapi.sdk.aeopenapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.alibaba.openapi.client.entity;
using System.Data.SqlClient;
using System.Configuration;
using AliexpressTool.Data;

namespace AliexpressTool.ApiTool
{
    public class AliexpressOrder
    {
        private string RefreshToken = ConfigurationManager.AppSettings["RefreshToken"].ToString();
        private string connectionStr = ConfigurationManager.ConnectionStrings["CommerceTemplate"].ToString();

        private List<ShopOrderData> datas;

        public void ShopOrder()
        {
            GetOrderList();

            foreach (ShopOrderData data in datas)
            {
                GetOrder(data);
            }
        }

        private void GetOrderList()
        {
            datas = new List<ShopOrderData>();

            string sql = "Select o.Id, o.ShippingStatusId, n.Note From dbo.[Order] o inner join dbo.OrderNote n On o.Id = n.OrderId Where o.ShippingStatusId != 40 And n.Note like 'AID:%'";
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
                        int id = 0, sid = 0;
                        int.TryParse(idr["Id"].ToString(), out id);
                        int.TryParse(idr["ShippingStatusId"].ToString(), out sid);
                        long aid = 0;
                        long.TryParse(idr["Note"].ToString().Replace("AID:", ""), out aid);

                        ShopOrderData data = new ShopOrderData();
                        data.Id = id;
                        data.ShippingStatusId = sid;
                        data.AliOrderId = aid;
                        datas.Add(data);
                    }
                }
            }
        }

        public void GetOrder(ShopOrderData data)
        {
            ClientPolicy clientPolicy = new ClientPolicy();
            clientPolicy.AppKey = "41375";
            clientPolicy.SecretKey = "K03zzSiA7Lh";
            clientPolicy.ServerHost = "gw.api.alibaba.com";
            clientPolicy.HttpPort = 80;

            ApiFacade facade = new ApiFacade(clientPolicy);
            AuthorizationToken auth = facade.refreshToken(RefreshToken);

            ApiFindOrderBaseInfoParam request = new ApiFindOrderBaseInfoParam();
            request.setOrderId(data.AliOrderId);
            ApiFindOrderBaseInfoResult result = facade.apiFindOrderBaseInfo(request, auth.getAccess_token());

            if (result.getOrderStatus() == "WAIT_BUYER_ACCEPT_GOODS" && data.ShippingStatusId != 30)
            {
                GetShipment(data.Id);

                UpdateShopOrder(data.Id, 20, 30);
            }
            else if (result.getOrderStatus() == "FINISH")
            {
                UpdateShipment(data.Id);

                UpdateShopOrder(data.Id, 30, 40);
            }
        }

        private void GetShipment(int orderid)
        {
            int id = 0;
            string sql = "select Id From dbo.Shipment Where OrderId = " + orderid;
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
                        int.TryParse(idr["Id"].ToString(), out id);
                    }
                }
            }

            if (id == 0)
            {
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sql = "Insert dbo.Shipment values(" + orderid + ", " + orderid + ", 0, '" + datetime + "', '', 'api', '" + datetime + "')";
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

        private void UpdateShipment(int orderid)
        {
            string sql = "Update dbo.Shipment Set DeliveryDateUtc = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' Where OrderId = " + orderid;
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateShopOrder(int id, int osid, int ssid)
        {
            string sql = "Update dbo.[Order] Set OrderStatusId = "+ osid + ", ShippingStatusId = "+ ssid + " Where Id = " + id;
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
