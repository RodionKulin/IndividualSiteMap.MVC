using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace IndividualSiteMap.MVC
{
    public class IndiSiteMapSettings
    {
        //свойства
        public NavigationNode Root { get; set; }
        public IVisibilityProvider VisibilityProvider { get; set; }
        public IUrlMatcher UrlMatcher { get; set; }
        public IUrlBuilder UrlBuilder { get; set; }
        public bool ShowStartingNode { get; set; }



        //инициализация
        public IndiSiteMapSettings()
        {
            VisibilityProvider = new DefaultVisibilityProvider();
            UrlMatcher = new DefaultUrlMatcher();
            UrlBuilder = new DefaultUrlBuilder();
            ShowStartingNode = true;
        }

        
    }
}