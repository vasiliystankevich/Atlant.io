using Dal;
using Libraries.Core.Backend.Common;
using Microsoft.Practices.Unity;

namespace Core.Bitcon.Service
{
    public class TypeFabric: BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            Logger.Extended.Logger.RegisterTypes(container, ".\\log4net.config");
            container.RegisterType<IExecutor, Executor>();
            container.RegisterType<IDalContext, DalContext>(new InjectionFactory(factory => new DalContext()));
            container.RegisterType<ICoreServiceRepository, CoreServiceRepository>();
            container.RegisterType<ICoreBitconWinServiceInstance, CoreBitconWinServiceInstance>();
        }
    }
}
