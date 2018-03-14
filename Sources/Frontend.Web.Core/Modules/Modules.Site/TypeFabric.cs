using Libraries.Core.Backend.Common;
using Microsoft.Practices.Unity;
using Modules.Site.Home;

namespace Modules.Site
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IHomeRepository, HomeRepository>();
            container.RegisterType<HomeController, HomeController>();
        }
    }
}
