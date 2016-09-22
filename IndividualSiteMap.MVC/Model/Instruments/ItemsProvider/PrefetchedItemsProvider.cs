using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IndividualSiteMap.MVC
{
    public class PrefetchedItemsProvider : IItemsProvider
    {
        //свойства
        public List<NavigationItem> Items { get; set; }


        //инициализция
        public PrefetchedItemsProvider()
        {

        }
        public PrefetchedItemsProvider(List<NavigationItem> items)
        {
            Items = items;
        }


        //методы
        public virtual List<NavigationItem> Provide(NavigationNode node, HttpContext context)
        {
            return Items;
        }


        //cast
        public static implicit operator PrefetchedItemsProvider(List<NavigationItem> items)
        {
            return new PrefetchedItemsProvider(items);
        }        
    }
}
