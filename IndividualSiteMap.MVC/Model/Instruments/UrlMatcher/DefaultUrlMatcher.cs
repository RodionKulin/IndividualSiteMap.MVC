using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IndividualSiteMap.MVC
{
    public class DefaultUrlMatcher : IUrlMatcher
    {
        public virtual bool Match(NavigationNode node, HttpContext context
            , RouteValueDictionary routeValues, NameValueCollection queryString)
        {
            string action = routeValues["action"].ToString();
            string controller = routeValues["controller"].ToString();

            bool httpMethodMatched = node.HttpMethod == null
                || context.Request.HttpMethod == node.HttpMethod.ToString();

            bool actionMatched = Compare(node.Action, action) && Compare(node.Controller, controller);

            bool routeMatched = MatchRouteValues(routeValues, node.RouteValues);

            bool queryStringMatched = MatchQueryString(queryString, node.QueryStringValues);
            
            return httpMethodMatched && actionMatched && routeMatched && queryStringMatched;
        }
        public virtual bool Match(NavigationItem item, HttpContext context
            , RouteValueDictionary routeValues, NameValueCollection queryString)
        {
            bool routeMatched = MatchRouteValues(routeValues, item.RouteValues);            
            bool queryStringMatched = MatchQueryString(queryString, item.QueryStringValues);

            return routeMatched && queryStringMatched;
        }

        protected virtual bool MatchRouteValues(RouteValueDictionary actual, IDictionary<string, object> expected)
        {
            if (expected != null)
            {
                foreach (KeyValuePair<string, object> keyValue in expected)
                {
                    string actualValue = actual.ContainsKey(keyValue.Key)
                        ? actual[keyValue.Key].ToString()
                        : null;

                    if (!Compare(actualValue, keyValue.Value?.ToString()))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        protected virtual bool MatchQueryString(NameValueCollection actual, IDictionary<string, string> expected)
        {
            if (expected != null)
            {
                foreach (KeyValuePair<string, string> qsPair in expected)
                {
                    string actualValue = actual[qsPair.Key];

                    if (!Compare(actualValue, qsPair.Value))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        protected virtual bool Compare(string actual, string expected)
        {
            if (actual == null && expected == null)
            {
                return true;
            }
            else if (actual != null && expected == "*")
            {
                return true;
            }
            if (actual == null || expected == null)
            {
                return false;
            }
            else
            {
                return actual.ToLowerInvariant() == expected.ToLowerInvariant();
            }
        }
    }
}