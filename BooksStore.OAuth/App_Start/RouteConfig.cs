using System.Web.Mvc;
using System.Web.Routing;

namespace BooksStore.OAuth
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "OAuth", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}