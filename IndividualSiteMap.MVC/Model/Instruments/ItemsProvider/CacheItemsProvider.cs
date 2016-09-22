using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IndividualSiteMap.MVC
{
    public abstract class CacheItemsProvider : IItemsProvider
    {
        //свойства
        public abstract string CacheKey
        {
            get;
        }


        //методы
        public List<NavigationItem> Provide(NavigationNode node, HttpContext context)
        {
            var items = (List<NavigationItem>)context.Cache[CacheKey];

            if(items == null)
            {
                items = GenerateItems();
                context.Cache[CacheKey] = items;
            }

            return items;
        }

        public abstract List<NavigationItem> GenerateItems();
    }
}
