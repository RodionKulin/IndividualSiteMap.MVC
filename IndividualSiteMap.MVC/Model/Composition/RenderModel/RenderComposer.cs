using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualSiteMap.MVC
{
    public class RenderComposer
    {
        //поля
        private CompositionContext _context;


        //инициализация
        public RenderComposer(CompositionContext context)
        {
            _context = context;
        }



        //фильтр видимости
        public List<RenderItem> FilterByRenderTarget(
            List<RenderItem> renderItems, object renderTarget, RenderItem parent)
        {
            List<RenderItem> filtered = new List<RenderItem>();

            foreach (RenderItem renderItem in renderItems)
            {
                bool isVisible = renderItem.Node.CheckRenderTarget(_context, renderTarget);
                if (isVisible)
                {
                    RenderItem filteredItem = new RenderItem(renderItem);  //пересоздание т.к. изменяются Children
                    filteredItem.Children = FilterByRenderTarget(renderItem.Children, renderTarget, filteredItem);
                    filteredItem.Parent = parent;
                    filtered.Add(filteredItem);
                }
            }

            return filtered;
        }
        


        //добавление динамических пунктов
        public List<RenderItem> ExtractProvidedItems(List<RenderItem> renderItems, RenderItem parent)
        {
            List<RenderItem> extractedList = new List<RenderItem>();

            foreach (RenderItem renderItem in renderItems)
            {
                if (renderItem.ItemsProvider == null)
                {
                    extractedList.Add(renderItem);
                    
                }
                else
                {
                    List<RenderItem> extractedItems = ToPreRenderNodes(renderItem, parent);
                    extractedList.AddRange(extractedItems);
                }
            }

            return extractedList;
        }

        private List<RenderItem> ToPreRenderNodes(RenderItem renderItem, RenderItem parent)
        {
            List<RenderItem> extractedList = new List<RenderItem>();
            List<NavigationItem> items = renderItem.ItemsProvider.Provide(renderItem.Node, _context.HttpContext);

            foreach (NavigationItem item in items)
            {
                RenderItem extractedItem = renderItem.FromNavigationItem(item, _context);
                extractedItem.Children = ExtractProvidedItems(renderItem.Children, extractedItem);
                extractedItem.Parent = parent;
                extractedList.Add(extractedItem);
            }

            return extractedList;
        }

    }
}
