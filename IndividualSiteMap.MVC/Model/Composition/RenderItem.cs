using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace IndividualSiteMap.MVC
{
    public class RenderItem
    {
        //поля
        protected string _href;


        //источники
        public NavigationNode Node { get; set; }
        public NavigationItem Item { get; set; }


        //отображение
        public string Href
        {
            get
            {
                if (string.IsNullOrEmpty(_href))
                    return "javascript:return false;";
                else
                    return _href;
            }
            set
            {
                _href = value;
            }
        }
        public string Title { get; set; }
        public string Description { get; set; }

        
        //иерархия
        public List<RenderItem> Children { get; set; }
        public RenderItem Parent { get; set; }
        public IItemsProvider ItemsProvider { get; set; }
        public bool IsClickable { get; set; }


        //статус активности
        public  bool IsCurrentItem { get; set; }     
        public  bool IsChildCurrentItem { get; set; }

        
        //вычисляемые свойства
        public bool IsChildOrSelfCurrentItem
        {
            get
            {
                return IsCurrentItem || IsChildCurrentItem;
            }
        }

        
        //параметры поиска NavigationItem
        public RouteValueDictionary MatchRouteValues { get; set; }
        public NameValueCollection MatchQueryString { get; set; }



        //инициализация
        public RenderItem()
        {

        }
        public RenderItem(NavigationNode node, CompositionContext context)
        {
            Node = node;
            Children = new List<RenderItem>();
            ItemsProvider = node.ItemsProvider;

            Title = node.GetTitle();
            Description = node.GetDescription();
            IsClickable = node.IsClickable;
            
            MatchQueryString = new NameValueCollection(
                context.HttpContext.Request.QueryString);
            MatchRouteValues = new RouteValueDictionary(
                context.HttpContext.Request.RequestContext.RouteData.Values);

            IUrlBuilder urlBuilder = node.CustomUrlBuilder ?? context.Settings.UrlBuilder;
            Href = urlBuilder.BuildUrl(this, context);
        }
        public RenderItem(RenderItem another)
        {
            Node = another.Node;
            Children = new List<RenderItem>();
            ItemsProvider = another.ItemsProvider;

            Title = another.Title;
            Description = another.Description;
            IsClickable = another.IsClickable;

            MatchQueryString = another.MatchQueryString;
            MatchRouteValues = another.MatchRouteValues;

            Href = another.Href;

            Parent = another.Parent;
            IsCurrentItem = another.IsCurrentItem;
            IsChildCurrentItem = another.IsChildCurrentItem;
        }



        //методы
        internal RenderItem MatchNavigationItem(CompositionContext context)
        {
            if (ItemsProvider == null)
            {
                return null;
            }
            
            List<NavigationItem> items = ItemsProvider.Provide(Node, context.HttpContext);

            NavigationItem matchedItem = items.FirstOrDefault(
                item => Node.MatchItemUrl(item, context, MatchRouteValues, MatchQueryString));

            if(matchedItem == null)
            {
                return null;
            }

            RenderItem renderItem = FromNavigationItem(matchedItem, context);
            renderItem.Parent = Parent;
            renderItem.IsCurrentItem = IsCurrentItem;
            renderItem.IsChildCurrentItem = IsChildCurrentItem;
            
            return renderItem;
        }

        internal RenderItem FromNavigationItem(NavigationItem item, CompositionContext context)
        {
            var navigationItem = new RenderItem()
            {
                Node = Node,
                Children = new List<RenderItem>(),
                Item = item,

                Title = item.GetTitle() ?? Node.GetTitle(),
                Description = item.GetDescription() ?? Node.GetDescription(),
                IsClickable = Node.IsClickable,

                MatchQueryString = new NameValueCollection(MatchQueryString),
                MatchRouteValues = new RouteValueDictionary(MatchRouteValues)
            };

            navigationItem.BuildHref(context);

            return navigationItem;
        }

        internal void BuildHref(CompositionContext context)
        {
            IUrlBuilder urlBuilder = Node.CustomUrlBuilder ?? context.Settings.UrlBuilder;
            Href = urlBuilder.BuildUrl(this, context);
        }

        public RenderItem Ancestor(string nodeKey)
        {
            RenderItem item = this;
            do
            {
                if (item.Node.NodeKey == nodeKey)
                    return item;
                item = item.Parent;
            }
            while (item != null);

            return null;
        }
    }
}