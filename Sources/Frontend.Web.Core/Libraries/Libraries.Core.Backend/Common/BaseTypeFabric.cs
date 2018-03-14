using Microsoft.Practices.Unity;

namespace Libraries.Core.Backend.Common
{
    public interface ITypeFabric
    {
        void RegisterTypes(IUnityContainer container);
    }
    public abstract class BaseTypeFabric: ITypeFabric
    {
        public abstract void RegisterTypes(IUnityContainer container);
        public static void RegisterTypes<TTypeFabric>(IUnityContainer container) where TTypeFabric:BaseTypeFabric, new()
        {
            ((ITypeFabric) new TTypeFabric()).RegisterTypes(container);
        }
    }
}
