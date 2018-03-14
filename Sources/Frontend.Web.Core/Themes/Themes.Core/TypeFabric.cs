using Libraries.Core.Backend.Common;
using Microsoft.Practices.Unity;

namespace Themes.Core
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<StartBootstrapSbAdmin2, StartBootstrapSbAdmin2>();
        }
    }
}
