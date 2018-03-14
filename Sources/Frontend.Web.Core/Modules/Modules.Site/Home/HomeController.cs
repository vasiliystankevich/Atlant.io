using System.Threading.Tasks;
using System.Web.Mvc;
using Dal;
using Libraries.Core.Backend.Common;

namespace Modules.Site.Home
{
    public interface IHomeController
    {
        Task<ActionResult> Index();
        IDalContext Context { get; set; }
    }

    [Authorize]
    public class HomeController:BaseController<IHomeRepository>, IHomeController
    {
        public HomeController(IHomeRepository homeRepository, IDalContext context):base(homeRepository)
        {
            Context = context;
        }

        public async Task<ActionResult> Index()
        {
            var model = Repository.GetLoadModules();
            return await GeneratorActionResult("~/Home/Index.cshtml", model);
        }
        public IDalContext Context { get; set; }
    }
}