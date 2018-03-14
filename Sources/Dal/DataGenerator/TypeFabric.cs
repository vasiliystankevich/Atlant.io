using System.IO;
using Dal;
using log4net;
using log4net.Config;
using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.Common;
using Logs;
using Microsoft.Practices.Unity;
using Modules.Core.Accounts;
using Project.Kernel;

namespace DataGenerator
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IWrapper<ILog>, Wrapper<ILog>>(new InjectionFactory(factory =>
            {
                var logFileConfigPath = "log4net.config";
                XmlConfigurator.Configure(new FileInfo(logFileConfigPath));
                return new Wrapper<ILog>(LogManager.GetLogger(typeof(Wrapper<ILog>)));
            }));

            container.RegisterType<ISaContext, SaContext>(new InjectionFactory(factory => new SaContext()));
            container.RegisterType<IDalContext, DalContext>(new InjectionFactory(factory => new DalContext()));
            container.RegisterType<ILogsContext, LogsContext>(new InjectionFactory(factory => new LogsContext()));
            container.RegisterType<IIdentityRepository, IdentityRepository>();
            container.RegisterType<IDataLoader, DataLoader>();
        }
    }
}
