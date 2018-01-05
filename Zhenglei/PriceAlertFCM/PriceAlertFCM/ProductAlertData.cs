using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlertFCM
{
    public class ProductAlertData
    {

        public int AlertId
        {
            get;set;
        }

        public int ProductId
        {
            get; set;
        }

        public string UserId
        {
            get; set;
        }

        public string Email
        {
            get; set;
        }

        public decimal ProductPrice
        {
            get; set;
        }

        public int Status
        {
            get; set;
        }

        public bool IsActive
        {
            get; set;
        }

        public int AlertType
        {
            get; set;
        }

        public string ExcludedRetailers
        {
            get; set;
        }

        public int PriceType
        {
            get; set;
        }

        public decimal PriceEach
        {
            get; set;
        }

        public string AlertGUID
        {
            get; set;
        }
    }
}
