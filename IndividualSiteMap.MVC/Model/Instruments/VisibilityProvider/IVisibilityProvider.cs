using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndividualSiteMap.MVC
{
    public interface IVisibilityProvider
    {
        bool CheckRenderType(NavigationNode item, HttpContext context, object renderTarget);
        bool CheckAuthorization(NavigationNode item, HttpContext context);
    }
}