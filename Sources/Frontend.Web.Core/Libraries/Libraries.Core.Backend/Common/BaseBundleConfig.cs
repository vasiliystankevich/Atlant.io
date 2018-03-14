using System.Collections.Generic;
using System.Web.Optimization;
using System.Web.UI;

namespace Libraries.Core.Backend.Common
{
    public enum EBundleType
    {
        Style,
        Script
    }

    public interface IBundleConfig
    {
        void RegisterBundles();
    }

    public abstract class BaseBundleConfig : IBundleConfig
    {
        protected BaseBundleConfig()
        {
            Bundles = BundleTable.Bundles;
            ModuleName = GetType().Assembly.GetName().Name;
        }
        protected void RegisterBundle(EBundleType bundleType, string virtualPath, string[] cdnPaths)
        {
            Bundle registerBundle = null;
            switch (bundleType)
            {
                case EBundleType.Style: registerBundle = new StyleBundle(virtualPath).Include(cdnPaths); break;
                case EBundleType.Script: registerBundle = new ScriptBundle(virtualPath).Include(cdnPaths); break;
            }
            Bundles.Add(registerBundle);
        }

        protected virtual void RegisterCss(string virtualPath, string[] cdnPaths)
        {
            RegisterBundle(EBundleType.Style, $"{virtualPath}/css", cdnPaths);
        }
        protected virtual void RegisterJs(string virtualPath, string[] cdnPaths)
        {
            RegisterBundle(EBundleType.Script, $"{virtualPath}/js", cdnPaths);
        }
        public abstract void RegisterBundles();
        protected string ModuleName { get; set; }
        protected BundleCollection Bundles { get; set; }
    }

    public abstract class LibraryBaseBundleConfig : BaseBundleConfig
    {
        protected LibraryBaseBundleConfig(string libraryName)
        {
            LibraryName = libraryName;
        }

        public void LibraryCss(string[] cdnPaths)
        {
            RegisterCss($"~/Libraries/{LibraryName}", cdnPaths);
        }

        public void LibraryJs(string[] cdnPaths)
        {
            RegisterJs($"~/Libraries/{LibraryName}", cdnPaths);
        }

        protected string LibraryName { get; set; }
    }

    public abstract class ModuleBaseBundleConfig : BaseBundleConfig
    {
        protected void Controller(string name)
        {
            RegisterCss($"~/Modules/{name}", new[] { $"~/Modules/{ModuleName}/{name}/controller.css" });
            RegisterJs($"~/Modules/{name}", new[] { $"~/Modules/{ModuleName}/{name}/controller.js" });
        }
        protected void Action(string controller, string name)
        {
            RegisterCss($"~/Modules/{controller}/{name}", new[] { $"~/Modules/{ModuleName}/{controller}/{name}/action.css" });
            RegisterJs($"~/Modules/{controller}/{name}", new[] {$"~/Modules/{ModuleName}/{controller}/{name}/action.js"});
        }
    }

    public abstract class ComponentBaseBundleConfig : BaseBundleConfig
    {
        protected ComponentBaseBundleConfig(string baseComponentName)
        {
            BaseComponentName = baseComponentName;
        }

        protected override void RegisterCss(string componentName, string[] cdnPaths)
        {
            base.RegisterCss($"~/Libraries/Components/{componentName}", cdnPaths);
        }
        protected override void RegisterJs(string componentName, string[] cdnPaths)
        {
            base.RegisterJs($"~/Libraries/Components/{componentName}", cdnPaths);
        }

        protected string BaseComponentName { get; set; }
    }

    public abstract class ThemeBaseBundleConfig : BaseBundleConfig
    {
        protected ThemeBaseBundleConfig(string themeName)
        {
            ThemeName = themeName;
        }

        protected void RegisterCss(string[] cdnPaths)
        {
            RegisterCss($"~/Themes/{ThemeName}", cdnPaths);
        }
        protected void RegisterJs(string[] cdnPaths)
        {
            RegisterJs($"~/Themes/{ThemeName}", cdnPaths);
        }

        protected string ThemeName { get; set; }
    }
}