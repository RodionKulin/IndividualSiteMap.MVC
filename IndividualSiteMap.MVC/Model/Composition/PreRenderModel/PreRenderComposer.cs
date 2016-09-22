using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IndividualSiteMap.MVC
{
    public class PreRenderComposer
    {
        //поля
        private CompositionContext _context;


        //свойства
        public List<RenderItem> PreRenderNodes { get; set; }

        public RenderItem CurrentItem { get; set; }



        //инициализация
        public PreRenderComposer(CompositionContext context)
        {
            _context = context;
        }



        //проверка доступности пунктов
        public RenderItem ToPreRenderNode(NavigationNode node)
        {
            bool isVisible = node != null && node.CheckAuthorization(_context);
            if (!isVisible)
            {
                return null;
            }
            
            RenderItem renderItem = new RenderItem(node, _context);
            
            foreach (NavigationNode childNode in node.Children)
            {
                RenderItem childRenderItem = ToPreRenderNode(childNode);
                if (childRenderItem != null)
                {
                    childRenderItem.Parent = renderItem;
                    renderItem.Children.Add(childRenderItem);
                }
            }

            renderItem.Children = renderItem.Children.OrderBy(p => p.Node.Order).ToList();

            return renderItem;
        }

        public List<RenderItem> MoveStartingNode(RenderItem rootNode)
        {
            List<RenderItem> rootList = rootNode == null
                ? new List<RenderItem>()
                : rootNode.Children;

            if (_context.Settings.ShowStartingNode && rootNode != null)
            {
                rootNode.Children = new List<RenderItem>();
                rootList.Insert(0, rootNode);
                _context.RootNode = rootNode;
            }

            return rootList.OrderBy(p => p.Node.Order).ToList();
        }



        //поиск текущего пункта
        public void MatchNodeUrl(List<RenderItem> renderItems)
        {
            foreach (RenderItem renderItem in renderItems)
            {                
                bool isNodeMatched = CurrentItem == null
                    && renderItem.Node.MatchNodeUrl(_context);

                if (isNodeMatched)
                {
                    CurrentItem = renderItem.ItemsProvider == null
                        ? renderItem
                        : renderItem.MatchNavigationItem(_context);
                }

                if (CurrentItem == null)
                {
                    MatchNodeUrl(renderItem.Children);
                }

                if (CurrentItem != null)
                {
                    return;
                }
            }
        }

        public void SetCurrentChain()
        {
            if (CurrentItem != null)
            {
                CurrentItem.IsCurrentItem = true;
                RenderItem parent = CurrentItem;

                while (parent != null)
                {
                    parent.IsChildCurrentItem = true;                    
                    parent = parent.Parent;
                }
            }
        }


        //поиск по ключу
        public RenderItem FindByKey(List<RenderItem> renderItems, string nodeKey)
        {
            foreach (RenderItem renderItem in renderItems)
            {
                if(renderItem.Node.NodeKey == nodeKey)
                {
                    return renderItem;
                }

                RenderItem  matchedItem = FindByKey(renderItem.Children, nodeKey);
                if(matchedItem != null)
                {
                    return matchedItem;
                }
                
            }

            return null;
        }


    }
}