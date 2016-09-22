using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndividualSiteMap.MVC
{
    public class RequestSiteMap
    {
        //поля
        private PreRenderComposer _preRenderComposer;
        private RenderComposer _renderComposer;
        private BreadCrumbComposer _breadcrumbComposer;
        private CompositionContext _context;


        //вычисляемые свойства
        public List<RenderItem> AuthorizedNodes
        {
            get
            {
                ProcessToStage(CompositionStage.RenderItemsCreated);
                return _preRenderComposer.PreRenderNodes;
            }
        }
        public RenderItem CurrentItem
        {
            get
            {
                ProcessToStage(CompositionStage.CurrentNodeScanComplete);

                return _preRenderComposer.CurrentItem;
            }
        }



        //инициализация
        public RequestSiteMap(IndiSiteMapSettings settings)
        {
            _context = new CompositionContext()
            {
                Settings = settings                
            };
            
            _preRenderComposer = new PreRenderComposer(_context);
            _renderComposer = new RenderComposer(_context);
            _breadcrumbComposer = new BreadCrumbComposer(_context);
        }



        //PreRenderComposer
        private void ProcessToStage(CompositionStage stage)
        {
            while((int)_context.CompositionStage < (int)stage)
            {
                if (_context.CompositionStage == CompositionStage.Initialized)
                {
                    CreateRenderItems();
                    _context.CompositionStage = CompositionStage.RenderItemsCreated;
                }
                else if (_context.CompositionStage == CompositionStage.RenderItemsCreated)
                {
                    FindCurrentItem();
                    _context.CompositionStage = CompositionStage.CurrentNodeScanComplete;
                }
            }
        }

        private void CreateRenderItems()
        {
            if (HttpContext.Current == null)
            {
                throw new NullReferenceException("HttpContext is not ready.");
            }

            _context.HttpContext = HttpContext.Current;
            _context.UrlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            
            RenderItem rootItem = _preRenderComposer.ToPreRenderNode(_context.Settings.Root);
            _preRenderComposer.PreRenderNodes = _preRenderComposer.MoveStartingNode(rootItem);
        }

        private void FindCurrentItem()
        {
            _preRenderComposer.MatchNodeUrl(_preRenderComposer.PreRenderNodes);
            _preRenderComposer.SetCurrentChain();            
        }

        public RenderItem FindByKey(string nodeKey)
        {
            ProcessToStage(CompositionStage.RenderItemsCreated);
            return _preRenderComposer.FindByKey(_preRenderComposer.PreRenderNodes, nodeKey);
        }



        //RenderComposer
        public List<RenderItem> GetMenu(object renderTarget)
        {
            ProcessToStage(CompositionStage.CurrentNodeScanComplete);

            List<RenderItem> fullMenuItems = _renderComposer.FilterByRenderTarget(
                _preRenderComposer.PreRenderNodes, renderTarget, null);

            fullMenuItems = _renderComposer.ExtractProvidedItems(fullMenuItems, null);
            
            return fullMenuItems;
        }

        public List<RenderItem> GetBreadCrumbs(object renderTarget)
        {
            ProcessToStage(CompositionStage.CurrentNodeScanComplete);
            
            List<RenderItem> breadCrumbs = _breadcrumbComposer.Compose(CurrentItem, renderTarget);
            
            return breadCrumbs;
        }
        
        public void WriteSeoMapResponce(object renderTarget)
        {
            ProcessToStage(CompositionStage.RenderItemsCreated);

            List<RenderItem> seoItems = _renderComposer.FilterByRenderTarget(
                _preRenderComposer.PreRenderNodes, renderTarget, null);

            seoItems = _renderComposer.ExtractProvidedItems(seoItems, null);

            var seoComposer = new SeoMapComposer();
            seoComposer.WriteXmlMapResponce(seoItems, _context);
        }
    }
}