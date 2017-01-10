using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Priceme.Deals.Code.Ajax
{
    public class AjaxBuilder
    {
        private static List<Type> typeList = null;
        static AjaxBuilder()
        {
            typeList = typeof(AjaxBuilder).Assembly.GetTypes().Where(type => typeof(IAjaxable).IsAssignableFrom(type) && (!type.IsInterface) && (!type.IsAbstract)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="param">没有参数传Null</param>
        /// <returns></returns>
        public static object Ajax(System.Web.HttpContext httpContext,string className, string methodName, IDictionary<string, object> param, string fromUrl, string userId = "")
        {
            if (string.IsNullOrEmpty(className)) return "0";
            if (string.IsNullOrEmpty(methodName)) return "0";

            if (param == null) param = new Dictionary<string, object>();

            Uri fromUri = string.IsNullOrWhiteSpace(fromUrl) ? null : new UriBuilder(fromUrl).Uri;
            Dictionary<string, object> fromUrlParam = new Dictionary<string, object>();

            if (fromUri != null)
            {
                var querys = System.Web.HttpUtility.ParseQueryString(fromUri.Query);
                for (int i = 0; i < querys.AllKeys.Length; i++)
                {
                    if (fromUrlParam.ContainsKey(querys.AllKeys[i])) continue;

                    fromUrlParam.Add(querys.AllKeys[i], querys.Get(querys.AllKeys[i]));
                }
            }

            var classInfo = typeList.SingleOrDefault(item => item.Name == className.Trim());
            if (classInfo == null) return "0";

            MethodInfo methedInfo = classInfo.GetMethod(methodName);

            if (methedInfo == null) return "0";
            if (methedInfo.GetParameters().Length != 1) return "0";
            if (methedInfo.GetParameters()[0].ParameterType.Name != "AjaxContext") return "0";

            try
            {
                return methedInfo.Invoke(Activator.CreateInstance(classInfo), new object[] { new AjaxContext(httpContext, param, fromUri, fromUrlParam, userId) });
            }
            catch { return "0"; }

        }

    }
}
