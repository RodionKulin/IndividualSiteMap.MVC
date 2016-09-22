using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IndividualSiteMap.MVC
{
    public class NavigationNode<TController> : NavigationNode
            where TController : Controller
    {

        //инициализация
        public NavigationNode()
            : base()
        {
        }
        public NavigationNode(string action, string controller, string title, object allowedRole = null)
            : base(action, controller, title, allowedRole)
        {
        }
        public NavigationNode(string action, string controller, string title, List<object> allowedRoles)
            : base(action,controller, title, allowedRoles)
        {
        }
        public NavigationNode(Expression<Func<TController, object>> actionMethod)
        {
            var member = actionMethod.Body as MethodCallExpression;
            if (member == null)
            {
                throw new ArgumentException("The parameter property must be a member accessing lambda such as x => x.Index()"
                    , nameof(actionMethod));
            }

            Action = member.Method.Name.ToLower();
            Controller = typeof(TController).Name.ToLower();

            if (Controller.EndsWith("controller"))
            {
                Controller = Controller.Substring(0, Controller.Length - 10);
            }
            
        }


    }
}
