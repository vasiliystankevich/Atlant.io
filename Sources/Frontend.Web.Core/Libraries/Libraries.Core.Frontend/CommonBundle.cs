using Libraries.Core.Backend.Common;

namespace Libraries.Core.Frontend
{
    public class CommonBundle:LibraryBaseBundleConfig
    {
        public CommonBundle() : base("Common")
        {
        }

        public override void RegisterBundles()
        {
            LibraryCss(new[]
            {
                "~/Libraries/Libraries.Core.Frontend/Common/css/jasny-bootstrap.css",
                "~/Libraries/Libraries.Core.Frontend/Common/css/aligns.css",
                "~/Libraries/Libraries.Core.Frontend/Common/css/margins.css",
                "~/Libraries/Libraries.Core.Frontend/Common/css/paddings.css",
                "~/Libraries/Libraries.Core.Frontend/Common/css/div-tables.css",
                "~/Libraries/Libraries.Core.Frontend/Common/css/menu.css",
                "~/Libraries/Libraries.Core.Frontend/Common/css/asp-net-mvc.css"
            });
            LibraryJs(new []
            {
                "~/Libraries/Libraries.Core.Frontend/Site.js",
                "~/Libraries/Libraries.Core.Frontend/Common/controller.js"
            });
        }
    }
}
