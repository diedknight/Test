using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportAttrs.Data;

namespace ImportAttrs
{
    public class AttributeRetailerGeneralHelper
    {
        List<AttributeRetailerGeneralMap> myGeneralAttributeMaps = new List<AttributeRetailerGeneralMap>();
        Dictionary<int, List<AttributeRetailerGeneralMap>> myRetailerAttributeMaps = new Dictionary<int, List<AttributeRetailerGeneralMap>>();
        Dictionary<int, List<AttributeRetailerGeneralMap>> myCategoryAttributeMaps = new Dictionary<int, List<AttributeRetailerGeneralMap>>();
        Dictionary<string, List<AttributeRetailerGeneralMap>> myRetailerCategoryAttributeMaps = new Dictionary<string, List<AttributeRetailerGeneralMap>>();
        internal void Add(AttributeRetailerGeneralMap argm)
        {
            if(argm.CategoryId == 0 && argm.RetailerId == 0)
            {
                myGeneralAttributeMaps.Add(argm);
            }
            else if (argm.CategoryId == 0)
            {
                if(myRetailerAttributeMaps.ContainsKey(argm.RetailerId))
                {
                    myRetailerAttributeMaps[argm.RetailerId].Add(argm);
                }
                else
                {
                    List<AttributeRetailerGeneralMap> list = new List<AttributeRetailerGeneralMap>();
                    list.Add(argm);
                    myRetailerAttributeMaps.Add(argm.RetailerId, list);
                }
            }
            else if(argm.RetailerId == 0)
            {
                if (myCategoryAttributeMaps.ContainsKey(argm.CategoryId))
                {
                    myCategoryAttributeMaps[argm.CategoryId].Add(argm);
                }
                else
                {
                    List<AttributeRetailerGeneralMap> list = new List<AttributeRetailerGeneralMap>();
                    list.Add(argm);
                    myCategoryAttributeMaps.Add(argm.CategoryId, list);
                }
            }
            else
            {
                string key = argm.RetailerId + "-" + argm.CategoryId;

                if (myRetailerCategoryAttributeMaps.ContainsKey(key))
                {
                    myRetailerCategoryAttributeMaps[key].Add(argm);
                }
                else
                {
                    List<AttributeRetailerGeneralMap> list = new List<AttributeRetailerGeneralMap>();
                    list.Add(argm);
                    myRetailerCategoryAttributeMaps.Add(key, list);
                }
            }
        }

        internal string FixAttributeValue(string attrValue, int retailerId, int categoryId)
        {
            string key = retailerId + "-" + categoryId;
            if(myRetailerCategoryAttributeMaps.ContainsKey(key))
            {
                List<AttributeRetailerGeneralMap> list = myRetailerCategoryAttributeMaps[key];
                foreach(var arm in list)
                {
                    if (arm.IsRemoveKeyword)
                    {
                        attrValue = attrValue.Replace(arm.Keyword, "");
                    }
                }
            }
            if (myRetailerAttributeMaps.ContainsKey(retailerId))
            {
                List<AttributeRetailerGeneralMap> list = myRetailerAttributeMaps[retailerId];
                foreach (var arm in list)
                {
                    if (arm.IsRemoveKeyword)
                    {
                        attrValue = attrValue.Replace(arm.Keyword, "");
                    }
                }
            }
            if (myCategoryAttributeMaps.ContainsKey(categoryId))
            {
                List<AttributeRetailerGeneralMap> list = myCategoryAttributeMaps[categoryId];
                foreach (var arm in list)
                {
                    if (arm.IsRemoveKeyword)
                    {
                        attrValue = attrValue.Replace(arm.Keyword, "");
                    }
                }
            }
            foreach (var arm in myGeneralAttributeMaps)
            {
                if (arm.IsRemoveKeyword)
                {
                    attrValue = attrValue.Replace(arm.Keyword, "");
                }
            }
            return attrValue;
        }
    }
}
