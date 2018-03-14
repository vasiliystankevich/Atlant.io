using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.Common;
using Microsoft.Practices.Unity;
using Modules.Core.Accounts;

namespace Modules.Core
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IIdentityRepository, IdentityRepository>();
            container.RegisterType<AccountsBundle, AccountsBundle>();
            container.RegisterType<AccountsController, AccountsController>();
        }
    }
}
