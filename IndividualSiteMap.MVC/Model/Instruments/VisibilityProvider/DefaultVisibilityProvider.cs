using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace IndividualSiteMap.MVC
{
    public class DefaultVisibilityProvider : IVisibilityProvider
    {
        //свойства
        public bool RenderByDefault { get; set; } = true;
        public bool AuthorizeByDefault { get; set; } = true;



        //методы
        public virtual bool CheckRenderType(NavigationNode item, HttpContext context, object renderTarget)
        {
            bool render = (RenderByDefault && item.RenderTargets.Count == 0)
                || (item.RenderTargets.Contains(renderTarget));
            
            return render;
        }

        public virtual bool CheckAuthorization(NavigationNode item, HttpContext context)
        {
            IPrincipal principal = context.User;
            
            if(item.AllowedRoles.Count == 0 && AuthorizeByDefault)
            {
                return true;
            }

            foreach (object role in item.AllowedRoles)
            {
                string roleString = role as string;

                if (roleString == null
                    && role != null && role.GetType().IsEnum)
                {
                    roleString = Enum.GetName(role.GetType(), role);
                }                

                if (principal.IsInRole(roleString))
                {
                    return true;
                }
            }

            return false;
        }
    }
}