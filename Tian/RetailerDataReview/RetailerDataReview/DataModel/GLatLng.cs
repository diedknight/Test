using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailerDataReview.DataModel
{
    public class GLatLng : BaseModel
    {
        protected override string ConnectionNameString()
        {
            return "priceMe_OtherCountry";
        }

        public static int GetCount(int retailerId)
        {
            GLatLng info = new GLatLng();

            string sql = "select count(1) from Store_GLatLng where Retailerid=" + retailerId;

            return Convert.ToInt32(info.ExecuteScalar(sql));
        }
    }
}
