using ImportAttrs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //ImportAttrs.ImportController.Init();

            string filePath = System.Configuration.ConfigurationManager.AppSettings["TestFeedPath"];

            List<ImportProductInfo> list = GetFileText(filePath);
            foreach (var importProductInfo in list)
            {
                ImportAttrs.ImportController.ImportAttr(importProductInfo);
            }
        }

        private static List<ImportProductInfo> GetFileText(string filePath)
        {
            List<ImportProductInfo> list = new List<ImportProductInfo>();
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(filePath))
            {
                string line = streamReader.ReadLine();
                while(!string.IsNullOrEmpty(line))
                {
                    ImportProductInfo importProductInfo = new ImportProductInfo();
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.JsonConvert.DeserializeObject(line) as Newtonsoft.Json.Linq.JObject;
                    Newtonsoft.Json.Linq.JObject attrsObj = obj["Attribute"] as Newtonsoft.Json.Linq.JObject;
                   
                    foreach(var attr in attrsObj.Cast<KeyValuePair<string, Newtonsoft.Json.Linq.JToken>>().ToList())
                    {
                        string key = attr.Key;
                        string value = attr.Value.ToString();
                        if(!string.IsNullOrEmpty(value))
                        {
                            dic.Add(key, value);
                        }
                    }
                    line = streamReader.ReadLine();
                    importProductInfo.AllAttributesDic = dic;

                    importProductInfo.ProductId = int.Parse(obj["PID"].ToString());
                    importProductInfo.RetailerId = int.Parse(obj["RID"].ToString());
                    importProductInfo.CategoryId = int.Parse(obj["CID"].ToString());
                    list.Add(importProductInfo);

                }
            }
            return list;
        }
    }
}
