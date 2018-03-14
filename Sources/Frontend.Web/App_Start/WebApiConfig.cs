using System.Web.Http;
using Libraries.Core.Backend.Common;
using Unity.WebApi;

namespace Frontend.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API

            // Маршруты веб-API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{action}/{request}",
                new { request = RouteParameter.Optional }
            );

            var container = UnityConfig.GetConfiguredContainer();
            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
