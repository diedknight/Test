//===============================================================================
// Pricealyser Crawler
// 
// Copyright (c) 2012 12RMB Ltd. All Rights Reserved.
//
// Author:          TianBJ  
// Date Created:    2015-03-27  (yyyy-MM-dd)
//
// Description:
// 
//===============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.Request
{
    internal abstract class Part
    {
        protected abstract long WriteHeader(StreamWriter writer);
        protected abstract long WriteBody(StreamWriter writer);
        public abstract long GetByteLength();
        public abstract string Read();
        public long Write(StreamWriter writer)
        {
            long num = this.WriteHeader(writer);
            return num + this.WriteBody(writer);
        }
    }
}
