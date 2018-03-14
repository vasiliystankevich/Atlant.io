using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.WebPages;
using Libraries.Core.Backend.Common;
using Microsoft.Practices.Unity;
using RazorGenerator.Mvc;
using WebGrease.Css.Extensions;

namespace Frontend.Web
{
    public static class ModulesActivator
    {
        public static void Start()
        {
            LoadAssemblies("~/Libraries");
            LoadAssemblies("~/Modules");
            LoadAssemblies("~/Themes");
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            var typeFabrics =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes().Where(t => t.BaseType == typeof(BaseTypeFabric)));
            typeFabrics.ForEach(t =>
            {
                var instance = (ITypeFabric) Activator.CreateInstance(t);
                instance.RegisterTypes(container);
            });
        }

        public static void RegisterBundles(IUnityContainer container)
        {
            RegisterBundles(container, typeof(LibraryBaseBundleConfig));
            RegisterBundles(container, typeof(ComponentBaseBundleConfig));
            RegisterBundles(container, typeof(ModuleBaseBundleConfig));
            RegisterBundles(container, typeof(ThemeBaseBundleConfig));
        }

        private static void RegisterBundles(IUnityContainer container, Type baseBundleType)
        {
            var typeBundles = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.BaseType == baseBundleType));
            typeBundles.ForEach(t =>
            {
                var instance = (IBundleConfig) container.Resolve(t);
                instance.RegisterBundles();
            });
        }

        public static void RazorGeneratorMvcInit()
        {
            var engineAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetTypes().Any(t => t.BaseType == typeof(ModuleTag)));
            engineAssemblies.ForEach(assembly =>
            {
                var engine = new PrecompiledMvcEngine(assembly)
                {
                    UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
                };
                ViewEngines.Engines.Insert(0, engine);
                VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
            });

        }

        public static void LoadAssemblies(string virtualPath)
        {
            var phisicalPath = MapPath(virtualPath);
            var assemblies = Directory.GetFiles(phisicalPath, "*.dll", SearchOption.AllDirectories);
            assemblies.ForEach(assembly => Assembly.LoadFile(assembly));
        }

        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }
    }
}