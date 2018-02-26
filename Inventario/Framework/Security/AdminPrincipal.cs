using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Runtime.InteropServices;
using Inventario.Services;
using Inventario.ServiceContracts;

namespace Inventario.Framework.Security
{
    public class AdminPrincipal:IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public string Users { get; set; }
        public string RoleType { get; set; }

        public List<string> Roles { get; set; }
            
          public AdminPrincipal()
        {

        }
        public AdminPrincipal(string username, FormsAuthenticationTicket authTicket)
        {
            this.Users = username;
            LoginService loginService = DependencyResolver.Current.GetService<LoginService>();
            LoginViewModel user = loginService.getUser(username);
            Identity = new FormsIdentity(authTicket);
            Users = user.username;
          
            RoleType = user.Role.Replace(" ", String.Empty);
        }
        public bool IsInRole(string role)
        {
            if (RoleType == role)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}