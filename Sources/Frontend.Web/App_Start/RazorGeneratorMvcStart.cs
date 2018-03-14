[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Frontend.Web.RazorGeneratorMvcStart), "Start")]

namespace Frontend.Web
{
    public static class RazorGeneratorMvcStart {
        public static void Start() {
            ModulesActivator.RazorGeneratorMvcInit();
        }
    }
}
