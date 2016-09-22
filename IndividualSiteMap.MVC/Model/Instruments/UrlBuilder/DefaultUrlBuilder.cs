using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace IndividualSiteMap.MVC
{
    public class DefaultUrlBuilder : IUrlBuilder
    {

        //методы
        public virtual string BuildUrl(RenderItem renderItem, CompositionContext context)
        {
            IDictionary<string, object> routeValues = renderItem.Item == null
                ? renderItem.Node.RouteValues
                : Combine(renderItem.Node.RouteValues, renderItem.Item.RouteValues);

            string url = routeValues == null
                ? context.UrlHelper.Action(renderItem.Node.Action, renderItem.Node.Controller)
                : context.UrlHelper.Action(renderItem.Node.Action, renderItem.Node.Controller
                    , ToRouteValueDictionary(renderItem, routeValues));

            IDictionary<string, string> queryValues = renderItem.Item == null
                ? renderItem.Node.QueryStringValues
                : Combine(renderItem.Node.QueryStringValues, renderItem.Item.QueryStringValues);

            return queryValues == null
                ? url
                : string.Format("{0}?{1}", url, ToQueryString(renderItem, queryValues));
        }

        
        protected virtual IDictionary<string, T> Combine<T>(
            IDictionary<string, T> source1, IDictionary<string, T> source2)
        {
            if (source1 != null && source2 == null)
            {
                return source1;
            }
            else if (source1 == null && source2 != null)
            {
                return source2;
            }
            else if (source1 != null && source2 != null)
            {
                return source1
                    .Union(source2)
                    .ToDictionary(p => p.Key, p => p.Value);
            }
            else
            {
                return null;
            }
        }

        protected virtual RouteValueDictionary ToRouteValueDictionary(
            RenderItem renderItem, IDictionary<string, object> routeValues)
        {
            RouteValueDictionary currentRvd = renderItem.MatchRouteValues;
            RouteValueDictionary buildRvd = new RouteValueDictionary();

            foreach (string nodeKey in routeValues.Keys.ToList())
            {
                object value = routeValues[nodeKey];

                if (value?.ToString() == "*")
                {
                    bool currentUrlHasKey = currentRvd != null && currentRvd.ContainsKey(nodeKey) && currentRvd[nodeKey] != null;
                    value = currentUrlHasKey
                        ? currentRvd[nodeKey]
                        : null;
                }

                buildRvd.Add(nodeKey, value);
            }

            return buildRvd;
        }

        protected virtual string ToQueryString(
            RenderItem renderItem, IDictionary<string, string> queryValues)
        {
           NameValueCollection currentQueryString = renderItem.MatchQueryString;
           Dictionary<string,string> buildQueryString = new Dictionary<string, string>();

            foreach (string nodeKey in queryValues.Keys)
            {
                string value = queryValues[nodeKey];

                if (value == "*")
                {
                    value = currentQueryString != null
                        ? currentQueryString[nodeKey]
                        : null;
                }

                buildQueryString.Add(HttpUtility.UrlEncode(nodeKey), HttpUtility.UrlEncode(value));
            }

            string qs = string.Join("&",
                buildQueryString.Select(kvp =>
                string.Format("{0}={1}", kvp.Key, kvp.Value)));
            return qs;
        }
        
    }
}
