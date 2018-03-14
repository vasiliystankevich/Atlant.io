using Dal;
using Microsoft.Practices.Unity;
using Project.Kernel;
using log4net;
using log4net.Config;
using System.IO;
using System.Web;

namespace Frontend.Web
{
    public class UnityContainerConfig 
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IWrapper<ILog>, Wrapper<ILog>>(new InjectionFactory(factory =>
            {
                var logFileConfigPath = HttpContext.Current.Server.MapPath("~/log4net.config");
                XmlConfigurator.Configure(new FileInfo(logFileConfigPath));
                return new Wrapper<ILog>(LogManager.GetLogger(typeof(Wrapper<ILog>)));
            }));
            container.RegisterType<IDalContext, DalContext>(new InjectionFactory(factory => new DalContext()));
        }
    }
}
