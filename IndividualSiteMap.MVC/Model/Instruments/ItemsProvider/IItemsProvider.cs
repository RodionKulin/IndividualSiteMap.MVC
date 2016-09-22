using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IndividualSiteMap.MVC
{
    public interface IItemsProvider
    {
        List<NavigationItem> Provide(NavigationNode node, HttpContext context);
    }
}
