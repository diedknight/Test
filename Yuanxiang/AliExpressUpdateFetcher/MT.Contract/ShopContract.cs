using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Contract
{
    public class ShopContract : IShopContract
    {
        public string Label { get; set; }

        public string Body { get; set; }

        public bool Recoverable { get; set; }
    }
}
