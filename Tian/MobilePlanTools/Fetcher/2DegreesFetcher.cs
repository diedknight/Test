using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Common;
using Common.Data;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Tags;
using Winista.Text.HtmlParser.Util;
using Pricealyser.Crawler.HtmlParser.Query;
using Newtonsoft.Json;
using Pricealyser.Crawler.Request;

namespace Fetcher
{
    public class PhoneEntity
    {
        public string image { get; set; }
        public string name { get; set; }
        public string upfrontAmount { get; set; }
    }

    public class _2DegreesFetcher : BaseFetcher
    {
        public _2DegreesFetcher()
            : base(2, "2 Degrees")
        { }

        public override List<MobilePlanInfo> GetMobilePlanInfoList()
        {
            StartCrawlingLog();

            XbaiRequest req = new XbaiRequest();
            List<MobilePlanInfo> list = new List<MobilePlanInfo>();

            string planUrl = "https://www.2degreesmobile.co.nz/mobile/pay-monthly/";

            req.Uri = new Uri(planUrl);
            JQuery doc = new JQuery(req.Get(), planUrl);

            doc.find(".size1of5.no-margin-bottom-mobile .flyin-container.flyin-first").each(item =>
            {
                var node = item.ToJQuery();
                MobilePlanInfo info = new MobilePlanInfo();
                info.CarrierName = this.ProviderName;
                info.DataMB = node.find(".plan-inclusions ul li").eq(0).find("strong").val().Trim();
                var minStr = node.find(".plan-inclusions ul li").eq(1).find("strong").val();
                info.Minutes = minStr.Contains("Unlimited") ? -1 : Convert.ToInt32(minStr.ToDecimal());
                info.MobilePlanName = node.find(".plan-price").text().Trim();
                info.MobilePlanURL = planUrl;
                info.Price = info.MobilePlanName.ToDecimal();
                info.Texts = -1;
                info.plus = 0;
                info.Phones = new List<MobilePhoneInfo>();

                list.Add(info);
            });

            string productListUrl = "https://www.2degreesmobile.co.nz/shop?p_p_id=konakart_portlet_WAR_konakart_portlet&p_p_lifecycle=1&p_p_state=normal&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_konakart_portlet_WAR_konakart_portlet__spage=%2FSelectCat.do%3FcatId%3D21&_konakart_portlet_WAR_konakart_portlet_catId=21&_konakart_portlet_WAR_konakart_portlet__sorig=%2FbuildJSON.do";
            string phonesUrl = "https://www.2degreesmobile.co.nz/shop?p_p_id=konakart_portlet_WAR_konakart_portlet&p_p_lifecycle=0&p_p_state=exclusive&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_konakart_portlet_WAR_konakart_portlet__spage=/buildJSON.do";

            string productUrl = "https://www.2degreesmobile.co.nz/shop?p_p_id=konakart_portlet_WAR_konakart_portlet&p_p_lifecycle=1&p_p_state=normal&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_konakart_portlet_WAR_konakart_portlet__spage=%2FSelectProd.do&_konakart_portlet_WAR_konakart_portlet__sorig=%2FSelectCat.do%3FcatId%3D21&productId=";
            string rootUrl = "https://www.2degreesmobile.co.nz";

            //读取产品列表页，获取Cookie等信息
            req.Uri = new Uri(productListUrl);
            req.Get();

            req.Uri = new Uri(phonesUrl);

            //string jsonStr="[{\"475\":{\"custom1\":\"\",\"tags\":[23],\"custom2\":\"\",\"Model\":\"20.52\",\"manufacturerId\":27,\"prodId\":475,\"image\":\"/kk-images/images/9400006010712/catalogue.jpg\",\"sku\":\"9400006010712\",\"isIphone\":true,\"priceExTax\":39,\"name\":\"ALCATEL 20.52 Black\",\"manufacturerName\":\"Alcatel\",\"specialPriceExTax\":\"39\",\"quantity\":10,\"phonePriceWithPlan\":\"0\",\"upfrontAmount\":0}},{\"476\":{\"custom1\":\"\",\"tags\":[23],\"custom2\":\"\",\"Model\":\"20.52\",\"manufacturerId\":27,\"prodId\":476,\"image\":\"/kk-images/images/9400006010729/catalogue.jpg\",\"sku\":\"9400006010729\",\"isIphone\":true,\"priceExTax\":39,\"name\":\"ALCATEL 20.52 White\",\"manufacturerName\":\"Alcatel\",\"specialPriceExTax\":\"39\",\"quantity\":9,\"phonePriceWithPlan\":\"0\",\"upfrontAmount\":0}},{\"637\":{\"custom1\":\"\",\"tags\":[23,24],\"custom2\":\"\",\"Model\":\"onetouch POP C3\",\"manufacturerId\":27,\"prodId\":637,\"image\":\"/kk-images/images/9400022047280/catalogue.jpg\",\"sku\":\"9400022047280\",\"isIphone\":true,\"priceExTax\":79,\"name\":\"ALCATEL onetouch POP C3 White\",\"manufacturerName\":\"Alcatel\",\"specialPriceExTax\":\"79\",\"quantity\":15,\"phonePriceWithPlan\":\"0\",\"upfrontAmount\":0}},{\"539\":{\"custom1\":\"\",\"tags\":[23,24],\"custom2\":\"\",\"Model\":\"Ascend Y520\",\"manufacturerId\":22,\"prodId\":539,\"image\":\"/kk-images/images/9400006010989/catalogue.jpg\",\"sku\":\"9400006010989\",\"isIphone\":true,\"priceExTax\":79,\"name\":\"Huawei Ascend Y520 White\",\"manufacturerName\":\"Huawei\",\"specialPriceExTax\":\"79\",\"quantity\":10,\"phonePriceWithPlan\":\"0\",\"upfrontAmount\":0}},{\"636\":{\"custom1\":\"\",\"tags\":[23,24],\"custom2\":\"\",\"Model\":\"onetouch POP C3\",\"manufacturerId\":27,\"prodId\":636,\"image\":\"/kk-images/images/9400022047297/catalogue.jpg\",\"sku\":\"9400022047297\",\"isIphone\":true,\"priceExTax\":79,\"name\":\"ALCATEL onetouch POP C3 Black\",\"manufacturerName\":\"Alcatel\",\"specialPriceExTax\":\"79\",\"quantity\":15,\"phonePriceWithPlan\":\"0\",\"upfrontAmount\":0}},{\"241\":{\"custom1\":\"\",\"tags\":[23,6,24,9,11,1,10,7,14,18,15,13,17,5,8],\"custom2\":\"\",\"Model\":\"Huawei IDEOS X3\",\"manufacturerId\":22,\"prodId\":241,\"image\":\"/kk-images/images/9400006006210/catalogue.jpg\",\"sku\":\"9400006006210\",\"isIphone\":true,\"priceExTax\":99,\"name\":\"Huawei IDEOS X3 Te Reo Maori\",\"manufacturerName\":\"Huawei\",\"specialPriceExTax\":\"FREE\",\"quantity\":15,\"phonePriceWithPlan\":\"0\",\"upfrontAmount\":0}},{\"638\":{\"custom1\":\"\",\"tags\":[23,24],\"custom2\":\"\",\"Model\":\"Galaxy V Plus\",\"manufacturerId\":15,\"prodId\":638,\"image\":\"/kk-images/images/9400022047341/catalogue.jpg\",\"sku\":\"9400022047341\",\"isIphone\":true,\"priceExTax\":149,\"name\":\"Samsung Galaxy V Plus Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"149\",\"quantity\":14,\"phonePriceWithPlan\":\"0\",\"upfrontAmount\":0}},{\"639\":{\"custom1\":\"\",\"tags\":[23,24],\"custom2\":\"\",\"Model\":\"Galaxy V Plus\",\"manufacturerId\":15,\"prodId\":639,\"image\":\"/kk-images/images/9400022047310/catalogue.jpg\",\"sku\":\"9400022047310\",\"isIphone\":true,\"priceExTax\":149,\"name\":\"Samsung Galaxy V Plus White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"149\",\"quantity\":14,\"phonePriceWithPlan\":\"0\",\"upfrontAmount\":0}},{\"641\":{\"custom1\":\"\",\"tags\":[23,28,24],\"custom2\":\"\",\"Model\":\"Galaxy J1 Ace\",\"manufacturerId\":15,\"prodId\":641,\"image\":\"/kk-images/images/9400022047822/catalogue.jpg\",\"sku\":\"9400022047822\",\"isIphone\":true,\"priceExTax\":199,\"name\":\"Samsung Galaxy J1 Ace White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"199\",\"quantity\":13,\"phonePriceWithPlan\":8,\"upfrontAmount\":29}},{\"640\":{\"custom1\":\"\",\"tags\":[23,28,24],\"custom2\":\"\",\"Model\":\"Galaxy J1 Ace\",\"manufacturerId\":15,\"prodId\":640,\"image\":\"/kk-images/images/9400022047792/catalogue.jpg\",\"sku\":\"9400022047792\",\"isIphone\":true,\"priceExTax\":199,\"name\":\"Samsung Galaxy J1 Ace Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"199\",\"quantity\":14,\"phonePriceWithPlan\":8,\"upfrontAmount\":29}},{\"566\":{\"custom1\":\"\",\"tags\":[23,28,24],\"custom2\":\"\",\"Model\":\"Galaxy Core Prime\",\"manufacturerId\":15,\"prodId\":566,\"image\":\"/kk-images/images/8806086537810/catalogue.jpg\",\"sku\":\"8806086537810\",\"isIphone\":true,\"priceExTax\":249,\"name\":\"Samsung Galaxy Core Prime White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"249\",\"quantity\":12,\"phonePriceWithPlan\":10,\"upfrontAmount\":29}},{\"565\":{\"custom1\":\"\",\"tags\":[23,28,24],\"custom2\":\"\",\"Model\":\"Galaxy Core Prime\",\"manufacturerId\":15,\"prodId\":565,\"image\":\"/kk-images/images/8806086537759/catalogue.jpg\",\"sku\":\"8806086537759\",\"isIphone\":true,\"priceExTax\":249,\"name\":\"Samsung Galaxy Core Prime Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"249\",\"quantity\":12,\"phonePriceWithPlan\":10,\"upfrontAmount\":29}},{\"532\":{\"custom1\":\"\",\"tags\":[23,24],\"custom2\":\"\",\"Model\":\"Moto G Second Gen\",\"manufacturerId\":29,\"prodId\":532,\"image\":\"/kk-images/images/6947681519312/catalogue.jpg\",\"sku\":\"6947681519312\",\"isIphone\":true,\"priceExTax\":329,\"name\":\"Motorola Moto G (2nd Gen)\",\"manufacturerName\":\"Motorola\",\"specialPriceExTax\":\"329\",\"quantity\":14,\"phonePriceWithPlan\":13,\"upfrontAmount\":29}},{\"627\":{\"custom1\":\"\",\"tags\":[23,28,24],\"custom2\":\"\",\"Model\":\"P8 Lite\",\"manufacturerId\":22,\"prodId\":627,\"image\":\"/kk-images/images/9400022046207/catalogue.jpg\",\"sku\":\"9400022046207\",\"isIphone\":true,\"priceExTax\":349,\"name\":\"Huawei P8 Lite Black\",\"manufacturerName\":\"Huawei\",\"specialPriceExTax\":\"349\",\"quantity\":4,\"phonePriceWithPlan\":14,\"upfrontAmount\":29}},{\"626\":{\"custom1\":\"\",\"tags\":[23,28,24],\"custom2\":\"\",\"Model\":\"P8 Lite\",\"manufacturerId\":22,\"prodId\":626,\"image\":\"/kk-images/images/9400022046238/catalogue.jpg\",\"sku\":\"9400022046238\",\"isIphone\":true,\"priceExTax\":349,\"name\":\"Huawei P8 Lite White\",\"manufacturerName\":\"Huawei\",\"specialPriceExTax\":\"349\",\"quantity\":7,\"phonePriceWithPlan\":14,\"upfrontAmount\":29}},{\"622\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy Xcover 3\",\"manufacturerId\":15,\"prodId\":622,\"image\":\"/kk-images/images/8806086997959/catalogue.jpg\",\"sku\":\"8806086997959\",\"isIphone\":true,\"priceExTax\":379,\"name\":\"Samsung Galaxy Xcover 3\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"379\",\"quantity\":4,\"phonePriceWithPlan\":15,\"upfrontAmount\":29}},{\"644\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy J5\",\"manufacturerId\":15,\"prodId\":644,\"image\":\"/kk-images/images/9400022047648/catalogue.jpg\",\"sku\":\"9400022047648\",\"isIphone\":true,\"priceExTax\":399,\"name\":\"Samsung Galaxy J5 Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"399\",\"quantity\":15,\"phonePriceWithPlan\":16,\"upfrontAmount\":29}},{\"645\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy J5\",\"manufacturerId\":15,\"prodId\":645,\"image\":\"/kk-images/images/9400022047679/catalogue.jpg\",\"sku\":\"9400022047679\",\"isIphone\":true,\"priceExTax\":399,\"name\":\"Samsung Galaxy J5 White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"399\",\"quantity\":15,\"phonePriceWithPlan\":16,\"upfrontAmount\":29}},{\"575\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy A3\",\"manufacturerId\":15,\"prodId\":575,\"image\":\"/kk-images/images/8806086639415/catalogue.jpg\",\"sku\":\"8806086639415\",\"isIphone\":true,\"priceExTax\":499,\"name\":\"Samsung Galaxy A3 White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"499\",\"quantity\":15,\"phonePriceWithPlan\":20,\"upfrontAmount\":29}},{\"495\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"SM-G800Y\",\"manufacturerId\":15,\"prodId\":495,\"image\":\"/kk-images/images/9400006010804/catalogue.jpg\",\"sku\":\"9400006010804\",\"isIphone\":true,\"priceExTax\":499,\"name\":\"Samsung Galaxy S5 mini White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"499\",\"quantity\":3,\"phonePriceWithPlan\":20,\"upfrontAmount\":29}},{\"494\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"SM-G800Y\",\"manufacturerId\":15,\"prodId\":494,\"image\":\"/kk-images/images/9400006010798/catalogue.jpg\",\"sku\":\"9400006010798\",\"isIphone\":true,\"priceExTax\":499,\"name\":\"Samsung Galaxy S5 mini Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"499\",\"quantity\":5,\"phonePriceWithPlan\":20,\"upfrontAmount\":29}},{\"579\":{\"custom1\":\"\",\"tags\":[23,28,24],\"custom2\":\"\",\"Model\":\"Galaxy A5\",\"manufacturerId\":15,\"prodId\":579,\"image\":\"/kk-images/images/8806086610131/catalogue.jpg\",\"sku\":\"8806086610131\",\"isIphone\":true,\"priceExTax\":599,\"name\":\"Samsung Galaxy A5 White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"599\",\"quantity\":14,\"phonePriceWithPlan\":24,\"upfrontAmount\":29}},{\"624\":{\"custom1\":\"\",\"tags\":[23,28,24],\"custom2\":\"\",\"Model\":\"P8 \",\"manufacturerId\":22,\"prodId\":624,\"image\":\"/kk-images/images/9400022046177/catalogue.jpg\",\"sku\":\"9400022046177\",\"isIphone\":true,\"priceExTax\":749,\"name\":\"Huawei P8 Mystic Champagne\",\"manufacturerName\":\"Huawei\",\"specialPriceExTax\":\"749\",\"quantity\":11,\"phonePriceWithPlan\":30,\"upfrontAmount\":29}},{\"450\":{\"custom1\":\"\",\"tags\":[23,28,24,29,5],\"custom2\":\"\",\"Model\":\"SM-G900I\",\"manufacturerId\":15,\"prodId\":450,\"image\":\"/kk-images/images/9400006010279/catalogue.jpg\",\"sku\":\"9400006010279\",\"isIphone\":true,\"priceExTax\":799,\"name\":\"Samsung Galaxy S5 SM-900I White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"799\",\"quantity\":15,\"phonePriceWithPlan\":33,\"upfrontAmount\":29}},{\"471\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"SM-G900I\",\"manufacturerId\":15,\"prodId\":471,\"image\":\"/kk-images/images/9400006010668/catalogue.jpg\",\"sku\":\"9400006010668\",\"isIphone\":true,\"priceExTax\":799,\"name\":\"Samsung Galaxy S5 SM-900I Gold\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"799\",\"quantity\":15,\"phonePriceWithPlan\":33,\"upfrontAmount\":29}},{\"470\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"SM-900I\",\"manufacturerId\":15,\"prodId\":470,\"image\":\"/kk-images/images/9400006010286/catalogue.jpg\",\"sku\":\"9400006010286\",\"isIphone\":true,\"priceExTax\":799,\"name\":\"Samsung Galaxy S5 SM-900I Blue\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"799\",\"quantity\":15,\"phonePriceWithPlan\":33,\"upfrontAmount\":29}},{\"449\":{\"custom1\":\"\",\"tags\":[23,28,24,29,5],\"custom2\":\"\",\"Model\":\"SM-900I\",\"manufacturerId\":15,\"prodId\":449,\"image\":\"/kk-images/images/9400006010262/catalogue.jpg\",\"sku\":\"9400006010262\",\"isIphone\":true,\"priceExTax\":799,\"name\":\"Samsung Galaxy S5 SM-900I Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"799\",\"quantity\":13,\"phonePriceWithPlan\":33,\"upfrontAmount\":29}},{\"586\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 32GB Black\",\"manufacturerId\":15,\"prodId\":586,\"image\":\"/kk-images/images/8806086786256/catalogue.jpg\",\"sku\":\"8806086786256\",\"isIphone\":true,\"priceExTax\":999,\"name\":\"Samsung Galaxy S6 32GB Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"999\",\"quantity\":14,\"phonePriceWithPlan\":41,\"upfrontAmount\":29}},{\"514\":{\"custom1\":\"\",\"tags\":[23,28,25],\"custom2\":\"\",\"Model\":\"iPhone 6\",\"manufacturerId\":28,\"prodId\":514,\"image\":\"/kk-images/images/888462062640/catalogue.jpg\",\"sku\":\"888462062640\",\"isIphone\":true,\"priceExTax\":999,\"name\":\"iPhone 6 16GB Silver\",\"manufacturerName\":\"Apple\",\"specialPriceExTax\":\"999\",\"quantity\":15,\"phonePriceWithPlan\":41,\"upfrontAmount\":29}},{\"587\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 32GB Blue\",\"manufacturerId\":15,\"prodId\":587,\"image\":\"/kk-images/images/8806086738637/catalogue.jpg\",\"sku\":\"8806086738637\",\"isIphone\":true,\"priceExTax\":999,\"name\":\"Samsung Galaxy S6 32GB Blue\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"999\",\"quantity\":15,\"phonePriceWithPlan\":41,\"upfrontAmount\":29}},{\"588\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 32GB Gold\",\"manufacturerId\":15,\"prodId\":588,\"image\":\"/kk-images/images/8806086738323/catalogue.jpg\",\"sku\":\"8806086738323\",\"isIphone\":true,\"priceExTax\":999,\"name\":\"Samsung Galaxy S6 32GB Gold\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"999\",\"quantity\":15,\"phonePriceWithPlan\":41,\"upfrontAmount\":29}},{\"516\":{\"custom1\":\"\",\"tags\":[23,28,25],\"custom2\":\"\",\"Model\":\"iPhone 6\",\"manufacturerId\":28,\"prodId\":516,\"image\":\"/kk-images/images/888462062350/catalogue.jpg\",\"sku\":\"888462062350\",\"isIphone\":true,\"priceExTax\":999,\"name\":\"iPhone 6 16GB Space Grey\",\"manufacturerName\":\"Apple\",\"specialPriceExTax\":\"999\",\"quantity\":15,\"phonePriceWithPlan\":41,\"upfrontAmount\":29}},{\"589\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 32GB White\",\"manufacturerId\":15,\"prodId\":589,\"image\":\"/kk-images/images/8806086799935/catalogue.jpg\",\"sku\":\"8806086799935\",\"isIphone\":true,\"priceExTax\":999,\"name\":\"Samsung Galaxy S6 32GB White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"999\",\"quantity\":13,\"phonePriceWithPlan\":41,\"upfrontAmount\":29}},{\"590\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 64GB Black\",\"manufacturerId\":15,\"prodId\":590,\"image\":\"/kk-images/images/8806086800105/catalogue.jpg\",\"sku\":\"8806086800105\",\"isIphone\":true,\"priceExTax\":1149,\"name\":\"Samsung Galaxy S6 64GB Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1149\",\"quantity\":15,\"phonePriceWithPlan\":47,\"upfrontAmount\":29}},{\"593\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 64GB White\",\"manufacturerId\":15,\"prodId\":593,\"image\":\"/kk-images/images/8806086799775/catalogue.jpg\",\"sku\":\"8806086799775\",\"isIphone\":true,\"priceExTax\":1149,\"name\":\"Samsung Galaxy S6 64GB White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1149\",\"quantity\":15,\"phonePriceWithPlan\":47,\"upfrontAmount\":29}},{\"548\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy Note 4\",\"manufacturerId\":15,\"prodId\":548,\"image\":\"/kk-images/images/9400006011016/catalogue.jpg\",\"sku\":\"9400006011016\",\"isIphone\":true,\"priceExTax\":1149,\"name\":\"Samsung Galaxy Note4 Pink\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1149\",\"quantity\":5,\"phonePriceWithPlan\":47,\"upfrontAmount\":29}},{\"592\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 64GB Gold\",\"manufacturerId\":15,\"prodId\":592,\"image\":\"/kk-images/images/8806086697859/catalogue.jpg\",\"sku\":\"8806086697859\",\"isIphone\":true,\"priceExTax\":1149,\"name\":\"Samsung Galaxy S6 64GB Gold\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1149\",\"quantity\":4,\"phonePriceWithPlan\":47,\"upfrontAmount\":29}},{\"595\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 Edge 32GB Gold\",\"manufacturerId\":15,\"prodId\":595,\"image\":\"/kk-images/images/8806086737975/catalogue.jpg\",\"sku\":\"8806086737975\",\"isIphone\":true,\"priceExTax\":1199,\"name\":\"Samsung Galaxy S6 Edge 32GB Gold\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1199\",\"quantity\":15,\"phonePriceWithPlan\":49,\"upfrontAmount\":29}},{\"594\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 Edge 32GB Black\",\"manufacturerId\":15,\"prodId\":594,\"image\":\"/kk-images/images/8806086773409/catalogue.jpg\",\"sku\":\"8806086773409\",\"isIphone\":true,\"priceExTax\":1199,\"name\":\"Samsung Galaxy S6 Edge 32GB Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1199\",\"quantity\":15,\"phonePriceWithPlan\":49,\"upfrontAmount\":29}},{\"517\":{\"custom1\":\"\",\"tags\":[23,28,25],\"custom2\":\"\",\"Model\":\"iPhone 6\",\"manufacturerId\":28,\"prodId\":517,\"image\":\"/kk-images/images/888462064385/catalogue.jpg\",\"sku\":\"888462064385\",\"isIphone\":true,\"priceExTax\":1199,\"name\":\"iPhone 6 64GB Silver\",\"manufacturerName\":\"Apple\",\"specialPriceExTax\":\"1199\",\"quantity\":15,\"phonePriceWithPlan\":49,\"upfrontAmount\":29}},{\"596\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 Edge 32GB Green\",\"manufacturerId\":15,\"prodId\":596,\"image\":\"/kk-images/images/8806086737197/catalogue.jpg\",\"sku\":\"8806086737197\",\"isIphone\":true,\"priceExTax\":1199,\"name\":\"Samsung Galaxy S6 Edge 32GB Green\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1199\",\"quantity\":14,\"phonePriceWithPlan\":49,\"upfrontAmount\":29}},{\"597\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 Edge 32GB White\",\"manufacturerId\":15,\"prodId\":597,\"image\":\"/kk-images/images/8806086773041/catalogue.jpg\",\"sku\":\"8806086773041\",\"isIphone\":true,\"priceExTax\":1199,\"name\":\"Samsung Galaxy S6 Edge 32GB White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1199\",\"quantity\":15,\"phonePriceWithPlan\":49,\"upfrontAmount\":29}},{\"525\":{\"custom1\":\"\",\"tags\":[23,28,25],\"custom2\":\"\",\"Model\":\"iPhone 6 Plus\",\"manufacturerId\":28,\"prodId\":525,\"image\":\"/kk-images/images/888462039239/catalogue.jpg\",\"sku\":\"888462039239\",\"isIphone\":true,\"priceExTax\":1199,\"name\":\"iPhone 6 Plus 16GB Space Grey\",\"manufacturerName\":\"Apple\",\"specialPriceExTax\":\"1199\",\"quantity\":15,\"phonePriceWithPlan\":49,\"upfrontAmount\":29}},{\"519\":{\"custom1\":\"\",\"tags\":[23,28,25],\"custom2\":\"\",\"Model\":\"iPhone 6\",\"manufacturerId\":28,\"prodId\":519,\"image\":\"/kk-images/images/888462064095/catalogue.jpg\",\"sku\":\"888462064095\",\"isIphone\":true,\"priceExTax\":1199,\"name\":\"iPhone 6 64GB Space Grey\",\"manufacturerName\":\"Apple\",\"specialPriceExTax\":\"1199\",\"quantity\":15,\"phonePriceWithPlan\":49,\"upfrontAmount\":29}},{\"634\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy Note5\",\"manufacturerId\":15,\"prodId\":634,\"image\":\"/kk-images/images/9400022047433/catalogue.jpg\",\"sku\":\"9400022047433\",\"isIphone\":true,\"priceExTax\":1299,\"name\":\"Samsung Galaxy Note 5 Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1299\",\"quantity\":12,\"phonePriceWithPlan\":53,\"upfrontAmount\":29}},{\"598\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 Edge 64GB Black\",\"manufacturerId\":15,\"prodId\":598,\"image\":\"/kk-images/images/8806086773140/catalogue.jpg\",\"sku\":\"8806086773140\",\"isIphone\":true,\"priceExTax\":1349,\"name\":\"Samsung Galaxy S6 Edge 64GB Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1349\",\"quantity\":15,\"phonePriceWithPlan\":55,\"upfrontAmount\":29}},{\"601\":{\"custom1\":\"\",\"tags\":[23,28,24],\"custom2\":\"\",\"Model\":\"Galaxy S6 Edge 64GB White\",\"manufacturerId\":15,\"prodId\":601,\"image\":\"/kk-images/images/8806086772860/catalogue.jpg\",\"sku\":\"8806086772860\",\"isIphone\":true,\"priceExTax\":1349,\"name\":\"Samsung Galaxy S6 Edge 64GB White\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1349\",\"quantity\":15,\"phonePriceWithPlan\":55,\"upfrontAmount\":29}},{\"599\":{\"custom1\":\"\",\"tags\":[23,28,24],\"custom2\":\"\",\"Model\":\"Galaxy S6 Edge 64GB Gold\",\"manufacturerId\":15,\"prodId\":599,\"image\":\"/kk-images/images/8806086737623/catalogue.jpg\",\"sku\":\"8806086737623\",\"isIphone\":true,\"priceExTax\":1349,\"name\":\"Samsung Galaxy S6 Edge 64GB Gold\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1349\",\"quantity\":14,\"phonePriceWithPlan\":55,\"upfrontAmount\":29}},{\"526\":{\"custom1\":\"\",\"tags\":[23,28,25],\"custom2\":\"\",\"Model\":\"iPhone 6 Plus\",\"manufacturerId\":28,\"prodId\":526,\"image\":\"/kk-images/images/888462041331/catalogue.jpg\",\"sku\":\"888462041331\",\"isIphone\":true,\"priceExTax\":1399,\"name\":\"iPhone 6 Plus 64GB Silver\",\"manufacturerName\":\"Apple\",\"specialPriceExTax\":\"1399\",\"quantity\":15,\"phonePriceWithPlan\":58,\"upfrontAmount\":29}},{\"630\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy edge+ 32GB\",\"manufacturerId\":15,\"prodId\":630,\"image\":\"/kk-images/images/9400022047495/catalogue.jpg\",\"sku\":\"9400022047495\",\"isIphone\":true,\"priceExTax\":1399,\"name\":\"Samsung Galaxy S6 edge+ 32GB Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1399\",\"quantity\":5,\"phonePriceWithPlan\":58,\"upfrontAmount\":29}},{\"527\":{\"custom1\":\"\",\"tags\":[23,28,25],\"custom2\":\"\",\"Model\":\"iPhone 6 Plus\",\"manufacturerId\":28,\"prodId\":527,\"image\":\"/kk-images/images/888462041638/catalogue.jpg\",\"sku\":\"888462041638\",\"isIphone\":true,\"priceExTax\":1399,\"name\":\"iPhone 6 Plus 64GB Gold\",\"manufacturerName\":\"Apple\",\"specialPriceExTax\":\"1399\",\"quantity\":4,\"phonePriceWithPlan\":58,\"upfrontAmount\":29}},{\"528\":{\"custom1\":\"\",\"tags\":[23,28,25],\"custom2\":\"\",\"Model\":\"iPhone 6 Plus\",\"manufacturerId\":28,\"prodId\":528,\"image\":\"/kk-images/images/888462041034/catalogue.jpg\",\"sku\":\"888462041034\",\"isIphone\":true,\"priceExTax\":1399,\"name\":\"iPhone 6 Plus 64GB Space Grey\",\"manufacturerName\":\"Apple\",\"specialPriceExTax\":\"1399\",\"quantity\":15,\"phonePriceWithPlan\":58,\"upfrontAmount\":29}},{\"617\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy S6 Edge 128GB Gold\",\"manufacturerId\":15,\"prodId\":617,\"image\":\"/kk-images/images/8806086994958/catalogue.jpg\",\"sku\":\"8806086994958\",\"isIphone\":true,\"priceExTax\":1449,\"name\":\"Samsung Galaxy S6 Edge 128GB Gold\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1449\",\"quantity\":5,\"phonePriceWithPlan\":60,\"upfrontAmount\":29}},{\"616\":{\"custom1\":\"\",\"tags\":[23],\"custom2\":\"\",\"Model\":\"Galaxy S6 Edge 128GB Black\",\"manufacturerId\":15,\"prodId\":616,\"image\":\"/kk-images/images/8806086994941/catalogue.jpg\",\"sku\":\"8806086994941\",\"isIphone\":true,\"priceExTax\":1449,\"name\":\"Samsung Galaxy S6 Edge 128GB Black\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1449\",\"quantity\":15,\"phonePriceWithPlan\":60,\"upfrontAmount\":29}},{\"632\":{\"custom1\":\"\",\"tags\":[23,28,24,29],\"custom2\":\"\",\"Model\":\"Galaxy edge+ 64GB\",\"manufacturerId\":15,\"prodId\":632,\"image\":\"/kk-images/images/9400022047587/catalogue.jpg\",\"sku\":\"9400022047587\",\"isIphone\":true,\"priceExTax\":1549,\"name\":\"Samsung Galaxy S6 edge+ 64GB Gold\",\"manufacturerName\":\"Samsung\",\"specialPriceExTax\":\"1549\",\"quantity\":5,\"phonePriceWithPlan\":64,\"upfrontAmount\":29}}]";

            var productList = JsonConvert.DeserializeObject<List<Dictionary<int, PhoneEntity>>>(req.Post());

            list.ForEach(item =>
            {
                productList.ForEach(pro =>
                {
                    pro.ToList().ForEach(phone =>
                    {
                        MobilePhoneInfo info = new MobilePhoneInfo();
                        info.ContractTypeID = 3;
                        info.UpfrontPrice = phone.Value.upfrontAmount.ToDecimal();
                        info.phoneURL = productUrl + phone.Key;
                        info.PhoneName = phone.Value.name;
                        info.PhoneImage = new Uri(new Uri(rootUrl), phone.Value.image).ToString();

                        item.Phones.Add(info);                        
                    });
                });
            });

            FinishCrawlingLog();
            return list;
        }

        //CookieCollection cookieColle;
        //string location;
        //string value;

        //public override List<MobilePlanInfo> GetMobilePlanInfoList()
        //{
        //    StartCrawlingLog();
        //    List<MobilePlanInfo> mobilePlanList = new List<MobilePlanInfo>();
        //    //return mobilePlanList;
        //    try
        //    {
        //        string url = "http://www.2degreesmobile.co.nz/paymonthly/plans";
        //        Parser parser = this.GetParser(url);
        //        StarlCrawlPlansLinkLog(url);

        //        var plan_obj = parser.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "container-fixed"))[0];

        //        var plan_data = plan_obj.Children.SearchFor(typeof(Div), false).ToNodeArray();

        //        foreach (var plan in plan_data) {
        //            var Data = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "second-row"), true);

        //            var str_arr = Data[0].ToPlainTextString().Split(' ');
        //            var min =Data[0].ToPlainTextString().Contains("Unlimited")?"-1": str_arr[5];
        //            var PlanName = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "first-row"), true)[0].ToPlainTextString();
        //            var a_obj = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "third-row"), true)[0];

        //            var PlanUrl = (a_obj.Children.SearchFor(typeof(ATag),false)[0] as ATag).Link;

        //            var info = new MobilePlanInfo();
        //            info.CarrierName = this.ProviderName;
        //            info.DataMB = str_arr[0];
        //            info.Minutes = int.Parse(min);
        //            info.MobilePlanName = PlanName;
        //            //info.MobilePlanURL = PlanUrl;
        //            info.MobilePlanURL = "http://www.2degreesmobile.co.nz/paymonthly/plans";
        //            info.Price = int.Parse(PlanName.Replace("$","").Trim());
        //            info.Texts = -1;
        //            info.plus = 0;

        //            var PhoneList = new List<MobilePhoneInfo>();
        //            foreach (var phone in GetProDataList())
        //            {
        //                var ph = new MobilePhoneInfo();
        //                ph.ContractTypeID = phone.ContractTypeID;
        //                ph.UpfrontPrice = phone.Price;
        //                ph.phoneURL = phone.ProUrl;
        //                ph.PhoneName = phone.Name;
        //                ph.PhoneImage = phone.ImageUrl;
        //                PhoneList.Add(ph);
        //            }
        //            info.Phones = PhoneList;
        //            mobilePlanList.Add(info);
        //        }
        //        //Dictionary<string, Dictionary<int, int>> bonus = GetMobilePlanBonus(parser);
        //        //Dictionary<string, Dictionary<string, string>> phoneDic = new Dictionary<string, Dictionary<string, string>>();
        //        //Dictionary<string, string> phoneImg = new Dictionary<string, string>();
        //        //Dictionary<string, string> phoneUrl = new Dictionary<string, string>();
        //        //GetMobilePlanPhoneMap(phoneDic, phoneImg, phoneUrl);

                

                
        //    }
        //    catch (Exception ex)
        //    {
        //        GenerateLog(string.Format("2DegreesFetcher crawling exception, at {0}:\r\n{1}\r\n{2}",
        //            DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), ex.Message, ex.Source));
        //    }

        //    FinishCrawlingLog();
            
        //    return mobilePlanList;
        //}

        //public List<pro_list> GetProDataList()
        //{

        //    string pro_url = "https://www.2degreesmobile.co.nz/shop?p_p_id=konakart_portlet_WAR_konakart_portlet&p_p_lifecycle=1&p_p_state=normal&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_konakart_portlet_WAR_konakart_portlet__spage=%2FSelectProd.do&_konakart_portlet_WAR_konakart_portlet__sorig=%2FSelectCat.do%3FcatId%3D21&productId=";
        //    var pro_list = new List<pro_list>();
        //    #region 第一页
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 128GB Gold", ImageUrl = ImgUrl("888462040730"), ProUrl = pro_url + "530", Price = 95, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 128GB Silver", ImageUrl = ImgUrl("888462040433"), ProUrl = pro_url + "529", Price = 95, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 128GB Space Grey", ImageUrl = ImgUrl("888462040136"), ProUrl = pro_url + "531", Price = 95, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 128GB Gold", ImageUrl = ImgUrl("888462040730"), ProUrl = pro_url + "521", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 128GB Silver", ImageUrl = ImgUrl("888462063517"), ProUrl = pro_url + "520", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 128GB Space Grey", ImageUrl = ImgUrl("888462063227"), ProUrl = pro_url + "522", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 64GB Gold", ImageUrl = ImgUrl("888462041638"), ProUrl = pro_url + "527", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 64GB Silver", ImageUrl = ImgUrl("888462041331"), ProUrl = pro_url + "526", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 64GB Space Grey", ImageUrl = ImgUrl("888462041034"), ProUrl = pro_url + "528", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 Edge 32GB White", ImageUrl = ImgUrl("8806086773041"), ProUrl = pro_url + "597", Price = 84, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 64GB Gold", ImageUrl = ImgUrl("888462064675"), ProUrl = pro_url + "518", Price = 80, ContractTypeID = 3 });
        //    #endregion
        //    #region 第二页
        //    pro_list.Add(new pro_list { Name = "iPhone 6 64GB Silver", ImageUrl = ImgUrl("888462064385"), ProUrl = pro_url + "517", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 64GB Space Grey", ImageUrl = ImgUrl("888462064095"), ProUrl = pro_url + "519", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 16GB Gold", ImageUrl = ImgUrl("888462039833"), ProUrl = pro_url + "524", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 16GB Silver", ImageUrl = ImgUrl("888462039536"), ProUrl = pro_url + "523", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 16GB Space Grey", ImageUrl = ImgUrl("888462039239"), ProUrl = pro_url + "525", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 64GB Black", ImageUrl = ImgUrl("8806086800105"), ProUrl = pro_url + "590", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 64GB White", ImageUrl = ImgUrl("8806086799775"), ProUrl = pro_url + "593", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Motorola Nexus 6", ImageUrl = ImgUrl("9400022044944"), ProUrl = pro_url + "559", Price = 76, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy Note4 Gold", ImageUrl = ImgUrl("9400006011047"), ProUrl = pro_url + "549", Price = 76, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy Note4 Pink", ImageUrl = ImgUrl("9400006011016"), ProUrl = pro_url + "548", Price = 76, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "HTC One M9", ImageUrl = ImgUrl("9400022045170"), ProUrl = pro_url + "604", Price = 74, ContractTypeID = 3 });
        //    #endregion
        //    #region 第三页
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 32GB Black", ImageUrl = ImgUrl("8806086786256"), ProUrl = pro_url + "586", Price = 74, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 32GB Gold", ImageUrl = ImgUrl("8806086738323"), ProUrl = pro_url + "588", Price = 74, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 32GB White", ImageUrl = ImgUrl("8806086799935"), ProUrl = pro_url + "589", Price = 74, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 16GB Gold", ImageUrl = ImgUrl("888462062930"), ProUrl = pro_url + "515", Price = 73, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 16GB Silver", ImageUrl = ImgUrl("888462062640"), ProUrl = pro_url + "514", Price = 73, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 16GB Space Grey", ImageUrl = ImgUrl("888462062350"), ProUrl = pro_url + "516", Price = 73, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 32GB Gold", ImageUrl = ImgUrl("885909844999"), ProUrl = pro_url + "394", Price = 70, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 32GB Silver", ImageUrl = ImgUrl("885909844845"), ProUrl = pro_url + "395", Price = 70, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 32GB Space Grey", ImageUrl = ImgUrl("885909844692"), ProUrl = pro_url + "396", Price = 70, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Motorola Moto X (2nd Gen) Black", ImageUrl = ImgUrl("9400022044432"), ProUrl = pro_url + "546", Price = 68, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Motorola Moto X (2nd Gen) White", ImageUrl = ImgUrl("9400022044425"), ProUrl = pro_url + "547", Price = 68, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 16GB Gold", ImageUrl = ImgUrl("885909844531"), ProUrl = pro_url + "391", Price = 67, ContractTypeID = 3 });
        //    #endregion
        //    #region 第四页
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 16GB Silver", ImageUrl = ImgUrl("885909844388"), ProUrl = pro_url + "392", Price = 67, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 16GB Space Grey", ImageUrl = ImgUrl("885909844234"), ProUrl = pro_url + "393", Price = 67, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 SM-900I Black", ImageUrl = ImgUrl("9400006010262"), ProUrl = pro_url + "449", Price = 66, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 SM-900I Blue", ImageUrl = ImgUrl("9400006010286"), ProUrl = pro_url + "470", Price = 66, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 SM-900I Gold", ImageUrl = ImgUrl("9400006010668"), ProUrl = pro_url + "471", Price = 66, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 SM-900I White", ImageUrl = ImgUrl("9400006010279"), ProUrl = pro_url + "450", Price = 66, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy K Zoom", ImageUrl = ImgUrl("9400006010873"), ProUrl = pro_url + "477", Price = 62, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A5 Black", ImageUrl = ImgUrl("8806086609968"), ProUrl = pro_url + "576", Price = 57, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A5 Gold", ImageUrl = ImgUrl("8806086609647"), ProUrl = pro_url + "577", Price = 57, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A5 Silver", ImageUrl = ImgUrl("8806086610063"), ProUrl = pro_url + "578", Price = 57, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A5 White", ImageUrl = ImgUrl("8806086610131"), ProUrl = pro_url + "579", Price = 57, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5c 16GB Pink", ImageUrl = ImgUrl("885909837830"), ProUrl = pro_url + "383", Price = 54, ContractTypeID = 3 });
        //    #endregion
        //    #region 第五页
        //    pro_list.Add(new pro_list { Name = "iPhone 5c 8GB Blue", ImageUrl = ImgUrl("885909939664"), ProUrl = pro_url + "509", Price = 54, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5c 8GB Green", ImageUrl = ImgUrl("885909939671"), ProUrl = pro_url + "510", Price = 54, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5c 8GB White", ImageUrl = ImgUrl("885909939640"), ProUrl = pro_url + "512", Price = 54, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5c 8GB Yellow", ImageUrl = ImgUrl("885909939657"), ProUrl = pro_url + "513", Price = 54, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "HTC Desire 820", ImageUrl = ImgUrl("9400022045163"), ProUrl = pro_url + "610", Price = 53, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 mini Black", ImageUrl = ImgUrl("9400006010798"), ProUrl = pro_url + "494", Price = 53, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 mini White", ImageUrl = ImgUrl("9400006010804"), ProUrl = pro_url + "495", Price = 53, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A3 Black", ImageUrl = ImgUrl("8806086639491"), ProUrl = pro_url + "572", Price = 49, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A3 Gold", ImageUrl = ImgUrl("8806086639590"), ProUrl = pro_url + "573", Price = 49, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A3 White", ImageUrl = ImgUrl("8806086639415"), ProUrl = pro_url + "575", Price = 49, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "LG G3 Beat", ImageUrl = ImgUrl("9400022044678"), ProUrl = pro_url + "552", Price = 45, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S4 mini Black", ImageUrl = ImgUrl("9400006008207"), ProUrl = pro_url + "347", Price = 45, ContractTypeID = 3 });
        //    #endregion
        //    #region 第六页
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S4 mini Red", ImageUrl = ImgUrl("9400006011061"), ProUrl = pro_url + "542", Price = 45, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S4 mini White", ImageUrl = ImgUrl("9400006011078"), ProUrl = pro_url + "540", Price = 45, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Motorola Moto G (2nd Gen)", ImageUrl = ImgUrl("6947681519312"), ProUrl = pro_url + "532", Price = 42, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "HTC Desire 510", ImageUrl = ImgUrl("4718487657537"), ProUrl = pro_url + "535", Price = 41, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Nokia Lumia 635", ImageUrl = ImgUrl("9400006010736"), ProUrl = pro_url + "472", Price = 39, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy Core Prime White", ImageUrl = ImgUrl("8806086537810"), ProUrl = pro_url + "566", Price = 39, ContractTypeID = 3 });
        //    #endregion
        //    return pro_list;
        //}


        //string ImgUrl(string markID)
        //{
        //    string url = "https://www.2degreesmobile.co.nz/kk-images/images/" + markID + "/catalogue.jpg";
        //    return url;
        //}

       




        //#region 原来的代码

        //private MobilePlanMoreInfo GetMobilePlanMoreInfo(Parser parser)
        //{
        //    MobilePlanMoreInfo info = new MobilePlanMoreInfo();

        //    parser.Reset();
        //    RegexFilter rFilter = new RegexFilter("Other pricing stuff...");
        //    NodeList rNode = parser.ExtractAllNodesThatMatch(rFilter);
        //    if (rNode == null || rNode.Count == 0) return info;

        //    TableTag tbl = rNode[0].Parent.NextSibling as TableTag;
        //    TableRow[] tr = tbl.Rows;

        //    Regex regex = new Regex(@"(?<value>[\d+]*)c");
        //    Match mat = regex.Match(tr[0].Columns[1].ToPlainTextString());
        //    string minutes = mat.Groups["value"].Value;
        //    mat = regex.Match(tr[1].Columns[1].ToPlainTextString());
        //    string texts = mat.Groups["value"].Value;
        //    mat = regex.Match(tr[4].Columns[1].ToPlainTextString());
        //    string data = mat.Groups["value"].Value;

        //    info.CallRate = minutes;
        //    info.TextRate = texts;
        //    info.DataRate = data;

        //    return info;
        //}

        ///// <summary>
        ///// Get Plan Bonus
        ///// </summary>
        ///// <param name="parser"></param>
        ///// <returns></returns>
        //private Dictionary<string, Dictionary<int, int>> GetMobilePlanBonus(Parser parser)
        //{
        //    parser.Reset();
        //    RegexFilter rFilter = new RegexFilter("Get a Plan Bonus");
        //    NodeList rNode = parser.ExtractAllNodesThatMatch(rFilter);
        //    if (rNode == null || rNode.Count == 0) return null;

        //    TableTag tbl = rNode[0].Parent.NextSibling.NextSibling.NextSibling.NextSibling as TableTag;
        //    Dictionary<string, Dictionary<int, int>> dic = new Dictionary<string, Dictionary<int, int>>();
            
        //    foreach (TableRow row in tbl.Rows)
        //    {
        //        if (row.ColumnCount < 3) continue;

        //        var planName = row.Columns[0].ToPlainTextString().Trim();
        //        int bonus12 = int.Parse(row.Columns[1].ToPlainTextString().Replace("$", "").Trim());
        //        int bonus24 = int.Parse(row.Columns[2].ToPlainTextString().Replace("$", "").Trim());

        //        Dictionary<int, int> bonus = new Dictionary<int, int>();
        //        bonus.Add(12, bonus12);
        //        bonus.Add(24, bonus24);
        //        dic.Add(planName, bonus);
        //    }

        //    return dic;
        //}

        //private void GetMobilePlanPhoneMap(Dictionary<string, Dictionary<string, string>> phoneDic, Dictionary<string, string> phoneImg, Dictionary<string, string> phoneUrl)
        //{
        //    try
        //    {
        //        GetHttpContentHeard();

        //        var url_ = "https://www.2degreesmobile.co.nz/shop?p_p_id=konakart_portlet_WAR_konakart_portlet&p_p_lifecycle=0&p_p_state=exclusive&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_konakart_portlet_WAR_konakart_portlet__spage=%2FbuildJSON.do";
        //        string httpString = GetPostHttpContent(url_);
        //        StarlCrawlPhonesLinkLog(url_);
                 
        //        Regex reg = new Regex(@"{[\s\S]*?""prodId"":(?<id>[\d+]*),[\s\S]*?""image"":""(?<image>[\s\S]*?)"",[\s\S]*?""name"":""(?<name>[\s\S]*?)"",[\s\S]*?""specialPriceExTax"":""(?<price>[\s\S]*?)""}},");
        //        MatchCollection mths = reg.Matches(httpString);
        //        var phonePlanUrl = string.Empty;
        //        foreach (Match mth in mths)
        //        {
        //            string name = mth.Groups["name"].Value.Replace("&trade;", "").Trim();
        //            name = name.Replace("&nbsp;", " ");
        //            while (name.Contains("  "))
        //            {
        //                name = name.Replace("  ", " ");
        //            } name = name.Trim();
        //            string id = mth.Groups["id"].Value;
        //            string url = location;
        //            string price = mth.Groups["price"].Value;
        //            string image = "https://www.2degreesmobile.co.nz" + mth.Groups["image"].Value;
        //            if (price.ToUpper().Contains("FREE"))
        //                price = string.Empty;

        //            string html = GetPostHttpContent(url, id, price);
        //            Lexer lexer = new Lexer(html);
        //            Parser pParser = new Parser(lexer);
        //            pParser.Reset();

        //            //HasAttributeFilter hFilter = new HasAttributeFilter("id", "planGrids");
        //            //NodeList nodeList = pParser.ExtractAllNodesThatMatch(hFilter);
        //            //if (nodeList == null || nodeList.Count == 0) continue;

        //            //TableTag tbl = nodeList[0] as TableTag;
        //            //TableRow[] tr = tbl.Rows;

        //            Regex regexFilter = new Regex(@"javascript\:setPlanDetails\((?<value>[\d+]*),(?<aa>[\s\S]*?);");
        //            RegexFilter pReg = new RegexFilter("Select Plan");
        //            NodeList nodeList = pParser.ExtractAllNodesThatMatch(pReg);
        //            if (nodeList == null || nodeList.Count <= 3) continue;
        //            Dictionary<string, string> valueDic = new Dictionary<string, string>();

        //            for (int j = 1; j < nodeList.Count; j++)
        //            {
        //                Match mat = regexFilter.Match(nodeList[j].Parent.Parent.ToHtml());
        //                if (mat.Success)
        //                {
        //                    string value = mat.Groups["value"].Value;

        //                    pParser.Reset();
        //                    HasAttributeFilter nFilter = new HasAttributeFilter("id", "planName" + value);
        //                    NodeList nNode = pParser.ExtractAllNodesThatMatch(nFilter);
        //                    string planName = string.Empty;
        //                    if (nNode != null && nNode.Count > 0 && !string.IsNullOrEmpty(nNode[0].ToPlainTextString()))
        //                        planName = nNode[0].ToPlainTextString().ToLower();

        //                    pParser.Reset();
        //                    nFilter = new HasAttributeFilter("id", "planDesc" + value);
        //                    nNode = pParser.ExtractAllNodesThatMatch(nFilter);
        //                    if (nNode != null && nNode.Count > 0 && !string.IsNullOrEmpty(nNode[0].ToPlainTextString()))
        //                        planName += (" " + nNode[0].ToPlainTextString().ToLower());

        //                    pParser.Reset();
        //                    string code = value + 0;
        //                    HasAttributeFilter vFilter = new HasAttributeFilter("id", "termWisePrice" + code);
        //                    NodeList vNode = pParser.ExtractAllNodesThatMatch(vFilter);
        //                    string price0 = "0";
        //                    if (vNode != null && vNode.Count > 0 && !string.IsNullOrEmpty(vNode[0].ToPlainTextString()))
        //                        price0 = vNode[0].ToPlainTextString();

        //                    pParser.Reset();
        //                    code = value + 1;
        //                    vFilter = new HasAttributeFilter("id", "termWisePrice" + code);
        //                    vNode = pParser.ExtractAllNodesThatMatch(vFilter);
        //                    string price12 = "0";
        //                    if (vNode != null && vNode.Count > 0 && !string.IsNullOrEmpty(vNode[0].ToPlainTextString()))
        //                        price12 = vNode[0].ToPlainTextString();

        //                    pParser.Reset();
        //                    code = value + 2;
        //                    vFilter = new HasAttributeFilter("id", "termWisePrice" + code);
        //                    vNode = pParser.ExtractAllNodesThatMatch(vFilter);
        //                    string price24 = "0";
        //                    if (vNode != null && vNode.Count > 0 && !string.IsNullOrEmpty(vNode[0].ToPlainTextString()))
        //                        price24 = vNode[0].ToPlainTextString();

        //                    valueDic.Add(planName, price0 + "|" + price12 + "|" + price24);
        //                }
        //            }
        //            if (phoneDic.ContainsKey(name))
        //            {
        //                Dictionary<string, string> valueDic0 = phoneDic[name];
        //                foreach (string key in valueDic.Keys)
        //                {
        //                    if (!valueDic0.ContainsKey(key))
        //                        valueDic0.Add(key, valueDic[key]);
        //                }
        //                phoneDic[name] = valueDic0;
        //            }
        //            else
        //                phoneDic.Add(name, valueDic);

        //            if (!phoneImg.ContainsKey(name))
        //                phoneImg.Add(name, image);
        //            phonePlanUrl = url + "&productId=" + id;
        //            if (!phoneUrl.ContainsKey(name))
        //                phoneUrl.Add(name, phonePlanUrl);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        GenerateLog(string.Format("2DegreesFetcher GetMobilePlanPhoneMap exception, at {0}:\r\n{1}\r\n{2}",
        //            DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), ex.Message, ex.Source));
        //    }   
        //}

        //private void GetHttpContentHeard()
        //{
        //    try
        //    {

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.2degreesmobile.co.nz/shop?p_p_id=konakart_portlet_WAR_konakart_portlet&p_p_lifecycle=1&p_p_state=normal&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_konakart_portlet_WAR_konakart_portlet__spage=%2FSelectCat.do%3FcatId%3D21&_konakart_portlet_WAR_konakart_portlet_catId=21");
        //        request.Method = "GET";
        //        request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.8.1.6) Gecko/20070725 Firefox/2.0.0.6";
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.CookieContainer = new CookieContainer();
        //        request.KeepAlive = false;
        //        request.ProtocolVersion = HttpVersion.Version10;
        //        HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
        //        if (cookieColle == null || cookieColle.Count == 0)
        //            cookieColle = response1.Cookies;
        //        StreamReader stIn1 = new StreamReader(response1.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));                
        //        string strResponse1 = stIn1.ReadToEnd();
        //        Lexer lexer = new Lexer(strResponse1);
        //        Parser parser = new Parser(lexer);
        //        parser.Reset();

        //        HasAttributeFilter fromFilter = new HasAttributeFilter("id", "ShowProductDetailsForm");
        //        NodeList fromNode = parser.ExtractAllNodesThatMatch(fromFilter);
        //        FormTag fTag = fromNode[0] as FormTag;
        //        location = fTag.FormLocation;

        //        NodeClassFilter inFilter = new NodeClassFilter(typeof(InputTag));
        //        NodeList inNode = fTag.Children.ExtractAllNodesThatMatch(inFilter, true);
        //        InputTag inTag = inNode[0] as InputTag;
        //        value = inTag.Attributes["VALUE"].ToString();
        //    }
        //    catch (Exception ex) { }
        //}

        //private string GetPostHttpContent(string url)
        //{
        //    try
        //    {
        //        string strResponse;

        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //        req.Method = "POST";
        //        req.ContentType = "application/x-www-form-urlencoded";
        //        req.CookieContainer = new CookieContainer();
        //        if (cookieColle != null && cookieColle.Count > 0)
        //            req.CookieContainer.Add(cookieColle);
                
        //        // Do the request to get the response 
        //        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        //        StreamReader stIn = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
        //        strResponse = stIn.ReadToEnd();

        //        return strResponse;
        //    }
        //    catch (Exception ex) { }
        //    return "";
        //}

        //private string GetPostHttpContent(string url, string id, string price)
        //{
        //    try
        //    {
        //        string strResponse;
        //        string strNewValue = "org.apache.struts.taglib.html.TOKEN=" + value + "&productId=" + id + "&productSpecialPrice=" + price;

        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //        req.Method = "POST";
        //        req.ContentType = "application/x-www-form-urlencoded";
        //        req.ContentLength = strNewValue.Length;
        //        req.CookieContainer = new CookieContainer();
        //        if (cookieColle != null && cookieColle.Count > 0)
        //            req.CookieContainer.Add(cookieColle);
        //        req.KeepAlive = false;
        //        req.ProtocolVersion = HttpVersion.Version10;

        //        // Write the request 
        //        StreamWriter stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.GetEncoding("gb2312"));
        //        stOut.Write(strNewValue);
        //        stOut.Close();

        //        // Do the request to get the response 
        //        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        //        StreamReader stIn = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
        //        strResponse = stIn.ReadToEnd();

        //        return strResponse;
        //    }
        //    catch (Exception ex) { }
        //    return "";
        //}
        //#endregion
    }

    //public class pro_list
    //{
    //    public string Name { get; set; }
    //    public string Plan { get; set; }

    //    public int Price { get; set; }

    //    public int ContractTypeID { get; set; }

    //    public string ImageUrl { get; set; }
    //    public string ProUrl { get; set; }
    //}
}
