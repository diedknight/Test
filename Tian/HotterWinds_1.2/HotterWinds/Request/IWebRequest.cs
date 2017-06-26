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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.Request
{
    public interface IWebRequest
    {
        void AddOrSetPara(string name, string value);
        void AddOrSetPara(string name, byte[] value);
        bool IsExistPara(string name);
        void RemovePara(string name);
        void ClearPara();
        string GetTextPara(string name);
        byte[] GetDataPara(string name);
        string Upload();
        string Post();
        string Get();
        byte[] GetFile();

        string GetCookie(string name);
        void SetCookie(string name, string value);

    }
}
