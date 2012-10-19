using System;
using System.Web.Mvc;
using System.Web.Routing;
using AjaxLogIn.Controllers;

namespace AjaxLogIn.Infrastructure
{
    public class ManualControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == typeof(AccountController))
            {
                return new AccountController(new InMemoryUserService(), new AuthService());
            }

            return base.GetControllerInstance(requestContext, controllerType);
        }
    }
}
