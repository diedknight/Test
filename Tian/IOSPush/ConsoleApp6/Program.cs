using IOSPush;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    class Program
    {
        static void Main(string[] args)
        {
            AppleApns.ProductInfo productInfo = new AppleApns.ProductInfo();
            productInfo.AlertMsg = "Canon Test 5D dropped to $87.60";
            productInfo.CountryId = 3;
            productInfo.ProductBestPrice = 700.60m;
            productInfo.ProductId = 894634543;
            productInfo.ProductImage = "https://s3.pricemestatic.com/Images/ProductImages/201705/636318301642678059_ms.jpg";
            productInfo.ProductName = "Canon Test 5D";


            AppleApns.Push(productInfo, "225f201a2f0dd4dc69d882da25e1d127456837d5bc17d1459ff1e907d74c5df8");

        }
    }
}
