using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndividualSiteMap.MVC
{
    public class IndiSiteMap
    {
        //свойства
        public static IndiSiteMapSettings Settings { get; set; }

        public static RequestSiteMap Request
        {
            get
            {
                RequestSiteMap current = (RequestSiteMap)HttpContext.Current.Items[Constants.PER_REQUEST_CACHE_KEY];

                if (current == null)
                {
                    current = new RequestSiteMap(Settings);
                    HttpContext.Current.Items[Constants.PER_REQUEST_CACHE_KEY] = current;
                }

                return current;
            }        
        }

     
        //инициализация
        static IndiSiteMap()
        {
            Settings = new IndiSiteMapSettings();
        }
    }
}