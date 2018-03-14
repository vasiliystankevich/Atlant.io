using Libraries.Core.Backend.Common;

namespace Libraries.Core.Frontend.Components.Preloader
{
    public class PreloaderBundle:ComponentBaseBundleConfig
    {
        public PreloaderBundle() : base("Preloader")
        {
        }
        public override void RegisterBundles()
        {
            RegisterCss($"{BaseComponentName}", new[]
            {
                "~/Libraries/Libraries.Core.Frontend/Components/Preloader/preloader.css",
            });
            RegisterJs($"{BaseComponentName}", new[]
            {
                "~/Libraries/Libraries.Core.Frontend/Components/Preloader/preloader.js",
            });
        }
    }
}
