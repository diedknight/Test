using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AliexpressTool
{
    class Program
    {
        static void Main(string[] args)
        {
            //DisposableTool.ShopAttribute sa = new DisposableTool.ShopAttribute();
            //sa.Attribute();
            AliexpressTool.ApiTool.AliexpressOrder ali = new ApiTool.AliexpressOrder();
            ali.ShopOrder();
        }
    }
}
