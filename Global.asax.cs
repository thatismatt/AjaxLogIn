using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AjaxLogIn.Infrastructure;

namespace AjaxLogIn
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );

            ControllerBuilder.Current.SetControllerFactory(new ManualControllerFactory());
        }
    }
}