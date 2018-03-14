using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.WebApi;
using Libraries.Core.Backend.WebApi.Repositories;
using Modules.WebApi.Shared.Requests;
using Project.Kernel;

namespace Modules.WebApi.Accounts
{
    public interface IAccountsApiController
    {
        Task<HttpResponseMessage> Login(LoginAccountRequest request);
        Task<HttpResponseMessage> Logout(BaseRequest request);
    }

    [RoutePrefix("accounts")]
    public class AccountsApiController : BaseApiController<IAccountsApiRepository>, IAccountsApiController
    {

        public AccountsApiController(IAccountsApiRepository repository, IVersionRepository versionRepository, Wrapper<ILog> logger) : base(repository, versionRepository, logger)
        {
        }

        [Route("login")]
        [HttpPost]
        public Task<HttpResponseMessage> Login([FromBody] LoginAccountRequest request)
        {
            return ExecuteAction(request, Repository.Login);
        }

        [Route("logout")]
        [HttpPost]
        [Authorize(Roles = ERoles.All)]
        public Task<HttpResponseMessage> Logout([FromBody] BaseRequest request)
        {
            return ExecuteAction(request, Repository.Logout);
        }
    }
}