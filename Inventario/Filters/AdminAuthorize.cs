using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Filters
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AdminAuthorize : AuthorizeAttribute
    {
        private string[] allowedrole;

        public AdminAuthorize(params string[] role)
        {
            this.allowedrole = role;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                  || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                base.OnAuthorization(filterContext);
            }
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;

            if (httpContext.User.Identity.IsAuthenticated)
            {
                if (this.allowedrole.Count() == 0)
                {
                    return true;
                }
                foreach(var role in allowedrole)
                if (httpContext.User.IsInRole(role))
                {
                    authorize = true;
                }
            }
            return authorize;
        }
    }
}