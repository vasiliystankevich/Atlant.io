using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Microsoft.Ajax.Utilities;
using Microsoft.Practices.Unity;

namespace Libraries.Core.Backend.Common
{
    public class UnityControllerFactory : IControllerFactory
    {
        public UnityControllerFactory(IUnityContainer unityContainer)
        {
            UnityContainer = unityContainer;
        }

        public static UnityControllerFactory Create(IUnityContainer unityContainer)
        {
            return new UnityControllerFactory(unityContainer);
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            var targetType = AppDomain.CurrentDomain.FindController(controllerName);                
            return targetType == null ? null : (IController) UnityContainer.Resolve(targetType);
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        { return SessionStateBehavior.Default; }

        public void ReleaseController(IController controller)
        {
            var disposable = controller as IDisposable;
            disposable?.Dispose();
        }

        private IUnityContainer UnityContainer { get; set; }
    }
}
