using IndividualSiteMap.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Datapecker.Panel
{
    public class AppendIfExistUrlBuilder : DefaultUrlBuilder
    {
        protected IEnumerable<string> _routeKeys;
        protected IEnumerable<string> _queryStringKeys;

        //инициализация
        public AppendIfExistUrlBuilder(IEnumerable<string> routeKeys, IEnumerable<string> queryStringKeys)
        {
            _routeKeys = routeKeys;
            _queryStringKeys = queryStringKeys;
        }


        //методы
        public override string BuildUrl(RenderItem renderItem, CompositionContext context)
        {
            //route values
            IDictionary<string, object> routeValues = renderItem.Item == null
                ? renderItem.Node.RouteValues
                : Combine(renderItem.Node.RouteValues, renderItem.Item.RouteValues);

            if (_routeKeys != null)
            {
                foreach (string qsKey in _routeKeys)
                {
                    if (renderItem.MatchQueryString[qsKey] != null)
                    {
                        routeValues = routeValues ?? new Dictionary<string, object>();
                        routeValues[qsKey] = "*";
                    }
                }
            }

            string url = routeValues == null
                ? context.UrlHelper.Action(renderItem.Node.Action, renderItem.Node.Controller)
                : context.UrlHelper.Action(renderItem.Node.Action, renderItem.Node.Controller
                    , ToRouteValueDictionary(renderItem, routeValues));


            //queryString values
            IDictionary<string, string> queryValues = renderItem.Item == null
                ? renderItem.Node.QueryStringValues
                : Combine(renderItem.Node.QueryStringValues, renderItem.Item.QueryStringValues);

            if(_queryStringKeys != null)
            {
                foreach (string qsKey in _queryStringKeys)
                {
                    if (renderItem.MatchQueryString[qsKey] != null)
                    {
                        queryValues = queryValues ?? new Dictionary<string, string>();
                        queryValues[qsKey] = "*";
                    }
                }
            }

            return queryValues == null
                ? url
                : string.Format("{0}?{1}", url, ToQueryString(renderItem, queryValues));
        }
    }
}