using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.WebApi;
using Libraries.Core.Backend.WebApi.Repositories;
using Modules.WebApi.Shared.Requests;
using Modules.WebApi.Shared.Responses;
using Project.Kernel;

namespace Modules.WebApi.BitcoinNet
{
    public interface IBitcoinNetApiController
    {
        Task<HttpResponseMessage> SendBtc(SendBtcRequest request);
        Task<HttpResponseMessage> GetLast(BaseRequest request);
        Task<HttpResponseMessage> GetAddressForAccount(GetAddressForAccountRequest request);
    }

    [RoutePrefix("bitcoinnet")]
    public class BitcoinNetApiController : BaseApiController<IBitcoinNetApiRepository>, IBitcoinNetApiController
    {
        public BitcoinNetApiController(IBitcoinNetApiRepository repository, IVersionRepository versionRepository, Wrapper<ILog> logger) : base(repository, versionRepository, logger)
        {
        }

        [Route("sendbtc")]
        [HttpPost]
        [Authorize(Roles = ERoles.AdministratorAndUser)]
        public Task<HttpResponseMessage> SendBtc([FromBody] SendBtcRequest request)
        {
            return ExecuteAction(request, Repository.SendBtc);
        }

        [Route("getlast")]
        [HttpPost]
        [Authorize(Roles = ERoles.AdministratorAndUser)]
        public Task<HttpResponseMessage> GetLast([FromBody] BaseRequest request)
        {
            return ExecuteAction(request, Repository.GetLast, true, "GetLastResponseResult");
        }

        [Route("getaddressforaccount")]
        [HttpPost]
        [Authorize(Roles = ERoles.AdministratorAndUser)]
        public Task<HttpResponseMessage> GetAddressForAccount(GetAddressForAccountRequest request)
        {
            return ExecuteAction(request, Repository.GetAddressForAccount);
        }
    }
}