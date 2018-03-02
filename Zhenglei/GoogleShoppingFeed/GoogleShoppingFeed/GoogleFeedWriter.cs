using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleShoppingFeed
{
    public class GoogleFeedWriter
    {
        List<GoogleFeedProduct> mGoogleProducts;
        string mFeedPath;

        public GoogleFeedWriter(List<GoogleFeedProduct> gProducts, string feedPath)
        {
            mGoogleProducts = gProducts;
            mFeedPath = feedPath;
        }

        public void WriteFile()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(mFeedPath, false))
            {
                sw.WriteLine("<rss xmlns:g=\"http://base.google.com/ns/1.0\" version=\"2.0\" encoding=\"utf-8\">");
                sw.WriteLine("<config><g:store>PriceMe</g:store><g:url>https://www.priceme.co.nz/</g:url></config>");
                sw.WriteLine("<channel>");
                foreach(var p in mGoogleProducts)
                {
                    sw.WriteLine(p.ToXmlString());
                }
                sw.WriteLine("</channel>");
                sw.WriteLine("</rss>");
            }
        }
    }
}