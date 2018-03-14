using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.Common;
using Microsoft.Practices.Unity;
using Project.Kernel;

namespace Frontend.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ModulesActivator.Start();
            var unityContainer = UnityConfig.GetConfiguredContainer();
            UnityContainerConfig.RegisterTypes(unityContainer);
            ModulesActivator.RegisterTypes(unityContainer);
            var unityControllerFactory = UnityControllerFactory.Create(unityContainer);
            ControllerBuilder.Current.SetControllerFactory(unityControllerFactory);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),
                new UnityHttpControllerSelector(GlobalConfiguration.Configuration));

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModulesActivator.RegisterBundles(unityContainer);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var logger = DependencyResolver.Current.GetService<Wrapper<ILog>>();
            logger.Instance.Fatal(exception);
        }
    }
}
