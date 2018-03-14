using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Libraries.Core.Backend.Common
{
    public static class AppDomainExtended
    {
        public static IEnumerable<Type> GetAllTypes(this AppDomain sender)
        {
            return sender.GetAssemblies().SelectMany(a => a.GetTypes());
        }

        public static Type FindController(this AppDomain sender, string controllerName)
        {
            var assemblies = sender.GetAssemblies();
            return FindController(assemblies, typeof(IController), controllerName);
        }

        public static Type FindApiController(this AppDomain sender, string controllerName)
        {
            var assemblies = sender.GetAssemblies();
            return FindController(assemblies, typeof(IHttpController), controllerName);
        }

        private static Type FindController(IEnumerable<Assembly> assemblies, Type interfaceController, string controllerName)
        {
            var r = assemblies.SelectMany(
                a => a.GetTypes().Where(t => t.GetInterface(interfaceController.FullName, true) != null));
            return
                assemblies.SelectMany(
                        a => a.GetTypes().Where(t => t.GetInterface(interfaceController.FullName, true) != null))
                    .First(t => t.FullName.ToLowerInvariant().Contains(controllerName.ToLowerInvariant()));
        }
    }
}
