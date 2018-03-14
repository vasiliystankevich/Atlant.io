using Libraries.Core.Backend.Common;

namespace Themes.Core
{
    public class StartBootstrapSbAdmin2:ThemeBaseBundleConfig
    {
        public StartBootstrapSbAdmin2() : base("startbootstrap-sb-admin-2")
        {
        }

        public override void RegisterBundles()
        {
            RegisterCss(new []
            {
                $"~/Themes/{ModuleName}/{ThemeName}/vendor/bootstrap/css/bootstrap.min.css",
                $"~/Themes/{ModuleName}/{ThemeName}/vendor/font-awesome/font-awesome.min.css",
                $"~/Themes/{ModuleName}/{ThemeName}/vendor/metismenu/metismenu.min.css",
                $"~/Themes/{ModuleName}/{ThemeName}/dist/css/sb-admin-2.min.css",
                $"~/Themes/{ModuleName}/{ThemeName}/dist/css/preloader.css"
                ,
            });
            RegisterJs(new []
            {
                $"~/Themes/{ModuleName}/{ThemeName}/vendor/jquery/jquery.min.js",
                $"~/Themes/{ModuleName}/{ThemeName}/vendor/bootstrap/js/bootstrap.min.js",
                $"~/Themes/{ModuleName}/{ThemeName}/vendor/bootstrap/js/respond.min.js",
                $"~/Themes/{ModuleName}/{ThemeName}/vendor/metismenu/metisMenu.min.js",
                $"~/Themes/{ModuleName}/{ThemeName}/dist/js/sb-admin-2.min.js"
            });
        }
    }
}
