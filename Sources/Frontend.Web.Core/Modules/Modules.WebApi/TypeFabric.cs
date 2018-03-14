using Libraries.Core.Backend.Common;
using Libraries.Core.Backend.WebApi.Repositories;
using Microsoft.Practices.Unity;
using Modules.WebApi.BitcoinNet;
using Modules.WebApi.Accounts;

namespace Modules.WebApi
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IVersionRepository, VersionRepository>();
            container.RegisterType<ITcpClientPoolRepository, TcpClientPoolRepository>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IAccountsApiRepository, AccountsApiRepository>();
            container.RegisterType<IBitcoinNetApiRepository, BitcoinNetApiRepository>();

            container.RegisterType<IAccountsApiController, AccountsApiController>();
            container.RegisterType<IBitcoinNetApiController, BitcoinNetApiController>();
        }
    }
}
