using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndividualSiteMap.MVC
{
    public class CompositionContext
    {
        public IndiSiteMapSettings Settings { get; set; }
        public HttpContext HttpContext { get; set; }
        public UrlHelper UrlHelper { get; set; }
        public CompositionStage CompositionStage { get; set; }
        public RenderItem RootNode { get; set; }
    }
}