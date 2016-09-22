using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IndividualSiteMap.MVC
{
    public class NavigationContent
    {
        //поля
        private ResourceManager _titleResourceManager;
        private Type _titleResource;
        private ResourceManager _descResourceManager;
        private Type _descResource;


        //свойства
        public string Image { get; set; }
        public string CssClass { get; set; }
        public IDictionary<string, string> Attributes { get; set; }

        public string Title { get; set; }
        public Type TitleResource
        {
            get { return _titleResource; }
            set
            {
                _titleResourceManager = null;
                _titleResource = value;
            }
        }
        public string TitleResourceKey { get; set; }


        public string Description { get; set; }
        public Type DescriptionResource
        {
            get { return _descResource; }
            set
            {
                _descResourceManager = null;
                _descResource = value;
            }
        }
        public string DescriptionResourceKey { get; set; }


        //методы
        protected ResourceManager GetResourceManager(Type resourceType)
        {
            BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            PropertyInfo resourceManagerProp = resourceType.GetProperty("ResourceManager", flags);
            return (ResourceManager)resourceManagerProp.GetValue(null);
        }

        protected virtual void SetTitleResource(Expression<Func<object, object>> resourceKey)
        {
            var member = resourceKey.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException("The parameter property must be a member accessing lambda such as x => x.ResourceKey"
                    , "resourceKey");
            }

            TitleResourceKey = member.Member.Name;
            TitleResource = member.Member.DeclaringType;

            _titleResourceManager = GetResourceManager(TitleResource);
        }
        public virtual string GetTitle()
        {
            if (TitleResource == null || TitleResourceKey == null)
            {
                return Title;
            }
            else
            {
                if (_titleResourceManager == null)
                {
                    _titleResourceManager = GetResourceManager(TitleResource);
                }

                CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
                ResourceSet resourceSet = _titleResourceManager.GetResourceSet(culture, true, true);
                return resourceSet.GetString(TitleResourceKey);
            }
        }

        protected virtual void SetDescriptionResource(Expression<Func<object, object>> resourceKey)
        {
            var member = resourceKey.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException("The parameter property must be a member accessing lambda such as x => x.ResourceKey"
                    , "resourceKey");
            }

            DescriptionResourceKey = member.Member.Name;
            DescriptionResource = member.Member.DeclaringType;

            _descResourceManager = GetResourceManager(DescriptionResource);
        }
        public virtual string GetDescription()
        {
            if (DescriptionResource == null || DescriptionResourceKey == null)
            {
                return Title;
            }
            else
            {
                if (_descResourceManager == null)
                {
                    _descResourceManager = GetResourceManager(TitleResource);
                }

                CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
                ResourceSet resourceSet = _descResourceManager.GetResourceSet(culture, true, true);
                return resourceSet.GetString(DescriptionResourceKey);
            }
        }
    }
}
