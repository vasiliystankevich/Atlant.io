using System.Configuration;
using Libraries.Core.Backend.Common;
using Microsoft.Practices.Unity;
using Topshelf;

namespace Core.Bitcon.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var typeConfiguration = ConfigurationManager.AppSettings["TypeConfiguration"];
            var container = UnityConfig.GetConfiguredContainer();
            Creator<TypeFabric>.Create().RegisterTypes(container);
            var host = HostFactory.New(x =>
            {
                x.Service<ICoreBitconWinServiceInstance>(s =>
                {
                    s.ConstructUsing(name => container.Resolve<ICoreBitconWinServiceInstance>());
                    s.WhenStarted(tc =>
                    {
                        Logger.Extended.Logger.ConfigureAndWatch(".\\log4net.config");
                        tc.Start();
                    });
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription($"Core.Bitcon.Service.{typeConfiguration}");
                x.SetDisplayName($"Core.Bitcon.Service.{typeConfiguration}");
                x.SetServiceName($"Core.Bitcon.Service.{typeConfiguration}");
            });
            host.Run();
        }
    }
}
