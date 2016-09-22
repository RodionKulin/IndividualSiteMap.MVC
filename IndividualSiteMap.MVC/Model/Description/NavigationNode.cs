using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IndividualSiteMap.MVC
{
    public class NavigationNode : NavigationContent
    {
        //идентификатор
        public string NodeKey { get; set; }


        //адрес
        public string Action { get; set; }
        public string Controller { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public IDictionary<string, object> RouteValues { get; set; }
        public IDictionary<string, string> QueryStringValues { get; set; }


        //содержание
        public SeoDescription Seo { get; set; }
        public bool IsClickable { get; set; }
        public IDictionary<string, object> Viewbag { get; set; }


        //уровень доступа        
        public List<object> AllowedRoles { get; set; }
        public List<object> RenderTargets { get; set; }


        //иерархия
        public int Order { get; set; }
        public List<NavigationNode> Children { get; set; }
        public IItemsProvider ItemsProvider { get; set; }


        //альтернативные реализации
        public IUrlMatcher CustomUrlMatcher { get; set; }
        public IUrlBuilder CustomUrlBuilder { get; set; }
        public IVisibilityProvider CustomVisibilityProvider { get; set; }


        //инициализация
        public NavigationNode()            
        {
            Children = new List<NavigationNode>();
            AllowedRoles = new List<object>();
            RenderTargets = new List<object>();
            Seo = new SeoDescription();
            Viewbag = new Dictionary<string, object>();
            IsClickable = true;
        }
        public NavigationNode(string action, string controller, string title, object allowedRole = null)
            : this()
        {
            Action = action;
            Controller = controller;
            Title = title;
            
            if (allowedRole != null)
                AllowedRoles.Add(allowedRole);
        }
        public NavigationNode(string action, string controller, string title, List<object> allowedRoles)
            : this()
        {
            Action = action;
            Controller = controller;
            Title = title;

            AllowedRoles = allowedRoles ?? new List<object>();
        }


        //методы
        public NavigationNode SetTitle(Expression<Func<object, object>> resourceKey)
        {
            SetTitleResource(resourceKey);
            return this;
        }

        public NavigationNode SetDescription(Expression<Func<object, object>> resourceKey)
        {
            SetDescriptionResource(resourceKey);
            return this;
        }

        internal bool CheckRenderTarget(CompositionContext context, object renderTarget)
        {
            IVisibilityProvider visibilityProvider = CustomVisibilityProvider
                ?? context.Settings.VisibilityProvider;
            return visibilityProvider.CheckRenderType(this, context.HttpContext, renderTarget);
        }

        internal bool CheckAuthorization(CompositionContext context)
        {
            IVisibilityProvider visibilityProvider = CustomVisibilityProvider
                ?? context.Settings.VisibilityProvider;
            return visibilityProvider.CheckAuthorization(this, context.HttpContext);
        }

        internal bool MatchNodeUrl(CompositionContext context
            , RouteValueDictionary itemRouteValues = null, NameValueCollection itemQueryString = null)
        {
            IUrlMatcher urlMatcher = CustomUrlMatcher ?? context.Settings.UrlMatcher;
            
            itemRouteValues = itemRouteValues ?? context.HttpContext.Request.RequestContext.RouteData.Values;
            itemQueryString = itemQueryString ?? context.HttpContext.Request.QueryString;

            return urlMatcher.Match(this, context.HttpContext, itemRouteValues, itemQueryString);
        }

        internal bool MatchItemUrl(NavigationItem item, CompositionContext context
            , RouteValueDictionary itemRouteValues = null, NameValueCollection itemQueryString = null)
        {
            IUrlMatcher urlMatcher = CustomUrlMatcher ?? context.Settings.UrlMatcher;

            itemRouteValues = itemRouteValues ?? context.HttpContext.Request.RequestContext.RouteData.Values;
            itemQueryString = itemQueryString ?? context.HttpContext.Request.QueryString;

            return urlMatcher.Match(item, context.HttpContext, itemRouteValues, itemQueryString);
        }
    }

}
