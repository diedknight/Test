using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChristmasSite.Code.Ajax
{
    public class AjaxContext
    {


        public IDictionary<string, object> Parameter { get; private set; }
        public IDictionary<string, object> FromUrlParameter { get; private set; }
        public Uri FromUri { get; private set; }
        public string UserId { get; private set; }
        public HttpContext HttpContext { get; private set; }

        public AjaxContext(HttpContext httpContext, IDictionary<string, object> param, Uri uri, IDictionary<string, object> fromUrlParam, string userId = "")
        {
            this.HttpContext = httpContext;
            this.FromUri = uri;
            this.Parameter = param == null ? new Dictionary<string, object>() : param;
            this.FromUrlParameter = fromUrlParam == null ? new Dictionary<string, object>() : fromUrlParam;
            this.UserId = userId;
        }

        public bool GetParameter(string name, bool defVal)
        {
            return this.GetBool(this.Parameter, name, defVal);
        }

        public int GetParameter(string name, int defVal)
        {
            return this.GetInt(this.Parameter, name, defVal);
        }

        public string GetParameter(string name, string defVal)
        {
            return this.GetStr(this.Parameter, name, defVal);
        }

        public bool GetFromUrlParameter(string name, bool defVal)
        {
            return this.GetBool(this.FromUrlParameter, name, defVal);
        }

        public int GetFromUrlParameter(string name, int defVal)
        {
            return this.GetInt(this.FromUrlParameter, name, defVal);
        }

        public string GetFromUrlParameter(string name, string defVal)
        {
            return this.GetStr(this.FromUrlParameter, name, defVal);
        }


        private bool GetBool(IDictionary<string, object> param, string name, bool defVal)
        {
            if (param == null) return defVal;

            if (!param.ContainsKey(name)) return defVal;
            if (param[name] == null) return defVal;

            string result = param[name].ToString().Trim();

            if (string.IsNullOrWhiteSpace(result)) return defVal;

            if (result.Trim().ToLower() == "false") return false;
            if (result.Trim().ToLower() == "true") return true;
            if (result.Trim() == "0") return false;
            if (result.Trim() == "1") return true;

            return defVal;
        }

        private int GetInt(IDictionary<string, object> param, string name, int defVal)
        {
            if (param == null) return defVal;

            if (!param.ContainsKey(name)) return defVal;
            if (param[name] == null) return defVal;

            int result = defVal;
            string text = param[name].ToString().Trim();

            if (string.IsNullOrWhiteSpace(text)) return defVal;

            if (Regex.IsMatch(text, "^-?[0-9\\s]+$"))
            {
                if (Int32.TryParse(text, out result))
                {
                    return result;
                }
            }

            return defVal;
        }

        private string GetStr(IDictionary<string, object> param, string name, string defVal)
        {
            if (param == null) return defVal;

            if (!param.ContainsKey(name)) return defVal;
            if (param[name] == null) return defVal;

            return param[name].ToString();
        }


    }
}
