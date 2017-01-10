using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceMeCommon.Convert
{
    public class ConvertMap
    {
        Dictionary<string, string> _maps;

        public ConvertMap()
        {
            _maps = new Dictionary<string, string>();
        }

        public void AddMap(string targetProperty, string convertProperty)
        {
            _maps.Add(targetProperty, convertProperty);
        }

        public string GetMap(string targetProperty)
        {
            if (_maps.ContainsKey(targetProperty))
            {
                return _maps[targetProperty];
            }
            return null;
        }
    }
}