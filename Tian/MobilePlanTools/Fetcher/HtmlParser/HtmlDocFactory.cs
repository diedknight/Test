//===============================================================================
// Pricealyser Crawler
// 
// Copyright (c) 2012 12RMB Ltd. All Rights Reserved.
//
// Author:          TianBJ  
// Date Created:    2015-03-18  (yyyy-MM-dd)
//
// Description:
// 
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser
{
    public class HtmlDocFactory
    {
        public static HtmlDocument Create()
        {
            return new WinistaHtmlDocument();
        }
    }
}
