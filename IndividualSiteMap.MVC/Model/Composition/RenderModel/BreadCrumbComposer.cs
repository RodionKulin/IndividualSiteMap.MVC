using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IndividualSiteMap.MVC
{
    public class BreadCrumbComposer
    {
        //поля
        private CompositionContext _context;
        private Dictionary<object, List<RenderItem>> _cache;


        //инициализация
        public BreadCrumbComposer(CompositionContext context)
        {
            _context = context;
            _cache = new Dictionary<object, List<RenderItem>>();
        }



        //методы
        public List<RenderItem> Compose(RenderItem currentItem, object renderTarget)
        {
            if(!_cache.ContainsKey(renderTarget))
            {
                List<RenderItem> breadCrumbs = BuildList(currentItem, renderTarget);
                _cache[renderTarget] = PickNodeItems(breadCrumbs);
            }

            return _cache[renderTarget];
        }

        private List<RenderItem> BuildList(RenderItem currentItem, object renderTarget)
        {
            if (currentItem == null)
            {
                return new List<RenderItem>();
            }

            List<RenderItem> breadCrumbItems = new List<RenderItem>();
            
            bool isVisible = currentItem.Node.CheckRenderTarget(_context, renderTarget);
            if (isVisible)
            {
                breadCrumbItems.Add(currentItem);
                RenderItem parent = currentItem.Parent;

                while (parent != null)
                {
                    isVisible = parent.Node.CheckRenderTarget(_context, renderTarget);
                    if(!isVisible)
                    {
                        new List<RenderItem>();
                    }
                    
                    breadCrumbItems.Insert(0, parent);
                    parent = parent.Parent;
                }
            }

            return breadCrumbItems;
        }

        private List<RenderItem> PickNodeItems(List<RenderItem> breadCrumbItems)
        {
            List<RenderItem> pickedList = new List<RenderItem>();

            foreach (RenderItem item in breadCrumbItems)
            {
                if (item.ItemsProvider == null)
                {
                    item.BuildHref(_context);
                    pickedList.Add(item);                    
                }
                else
                {
                    RenderItem matchedItem = item.MatchNavigationItem(_context);
                    if(matchedItem == null)
                    {
                        item.IsClickable = false;
                        pickedList.Add(item);
                    }
                    else
                    {
                        pickedList.Add(matchedItem);
                    }    
                }                
            }

            return pickedList;
        }
        

    }
}
