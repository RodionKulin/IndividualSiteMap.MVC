using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace IndividualSiteMap.MVC
{
    public interface IUrlMatcher
    {
        bool Match(NavigationNode node, HttpContext context
            , RouteValueDictionary routeValues, NameValueCollection queryString);
        bool Match(NavigationItem item, HttpContext context
            , RouteValueDictionary routeValues, NameValueCollection queryString);
    }
}