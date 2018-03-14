using Libraries.Core.Backend.Common;

namespace Libraries.Core.Frontend.Components.WebApi
{
    public class WebApiBundle:ComponentBaseBundleConfig
    {
        public WebApiBundle() : base("WebApi")
        {
        }

        protected void WebApi()
        {
            RegisterJs($"{BaseComponentName}", new[]
            {
                "~/Libraries/Libraries.Core.Frontend/Components/WebApi/WebApi.js",
            });

        }

        protected void Common()
        {
            RegisterJs($"{BaseComponentName}/Common", new[]
            {
                "~/Libraries/Libraries.Core.Frontend/Components/WebApi/Common.js",
            });
        }

        public override void RegisterBundles()
        {
            WebApi();
            Common();
        }
    }
}
