using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace IndividualSiteMap.MVC
{
    public class NavigationItem : NavigationContent
    {
        //свойства
        public IDictionary<string, object> RouteValues { get; set; }
        public IDictionary<string, string> QueryStringValues { get; set; }


        //инициализация
        public NavigationItem()
        {
        }
        public NavigationItem(string title, string description
            , IDictionary<string, object> routeValues, IDictionary<string, string> queryStringValues)
        {
            Title = title;
            Description = description;
            RouteValues = routeValues;
            QueryStringValues = queryStringValues;
        }


        //методы
        public NavigationItem SetTitle(Expression<Func<object, object>> resourceKey)
        {
            SetTitleResource(resourceKey);
            return this;
        }
        public NavigationItem SetDescription(Expression<Func<object, object>> resourceKey)
        {
            SetDescriptionResource(resourceKey);
            return this;
        }
    }
}
