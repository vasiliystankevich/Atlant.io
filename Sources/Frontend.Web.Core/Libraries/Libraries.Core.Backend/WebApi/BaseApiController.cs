using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using log4net;
using Libraries.Core.Backend.Common;
using Libraries.Core.Backend.External;
using Libraries.Core.Backend.WebApi.Repositories;
using Modules.WebApi.Shared.Requests;
using Modules.WebApi.Shared.Responses;
using Newtonsoft.Json;
using Project.Kernel;

namespace Libraries.Core.Backend.WebApi
{
    public class BaseApiController<TRepository>:ApiController, IRepository<TRepository>
    {
        public BaseApiController(TRepository repository, IVersionRepository versionRepository, Wrapper<ILog> logger)
        {
            Repository = repository;
            Logger = logger;
            VersionRepository = versionRepository;
            IsVisibleExceptionRemoute = Convert.ToBoolean(ConfigurationManager.AppSettings["IsVisibleExceptionRemoute"]);
        }

        public async Task<HttpResponseMessage> ContentGenerator<T>(T model)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var content = JsonConvert.SerializeObject(model);
            response.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return await Task<HttpResponseMessage>.Factory.StartNew(() => response);
        }

        public async Task<HttpResponseMessage> ExecuteAction<TRequest, TResponse>(TRequest request, Func<TRequest, TResponse> functor, bool isCache=false, string key="") where TRequest:BaseRequest
        {
            Task<HttpResponseMessage> result;
            try
            {
                TResponse model;
                request.Verify(() => string.Compare(request.Version, VersionRepository.Version, StringComparison.OrdinalIgnoreCase) != 0, "invalid version of the request");
                Validate(request);
                if (!ModelState.IsValid) throw new ValidationException("not valid model");
                if (!isCache) model = functor(request);
                model = ValidateCache(key, request, functor);
                result = ContentGenerator(model);
            }
            catch (AuthenticationException exception)
            {
                LogException(request, exception);
                var model = GenerateExceptionResponse(401, exception);
                result = ContentGenerator(model);
            }
            catch (Exception exception)
            {
                LogException(request, exception);
                var model = GenerateExceptionResponse(500, exception);
                result = ContentGenerator(model);
            }
            return await result;
        }

        protected void LogException<TRequest>(TRequest request, Exception exception)
            where TRequest : BaseRequest
        {
            var badApiRequest = JsonConvert.SerializeObject(request);
            Logger.Instance.Error($"ip for bad request: {IP.GetIPAddress()}");
            Logger.Instance.Error($"bad api request: {badApiRequest}");
            Logger.Instance.Error(exception);
        }

        protected BaseResponse GenerateExceptionResponse(int httpCode, Exception exception)
        {
            var exceptionMessage = IsVisibleExceptionRemoute ? exception.Message : "bad request";
            return BaseResponse.Create(httpCode, exceptionMessage);
        }

        protected TResponse ValidateCache<TRequest, TResponse>(string key, TRequest request, Func<TRequest, TResponse> functor)
        {
            var response = (TResponse) HttpContext.Current.Cache[key];
            if (response!=null) return response;
            lock (LockObject)
            {
                response = (TResponse)HttpContext.Current.Cache[key];
                if (response != null) return response;
                response = functor(request);
                HttpContext.Current.Cache.Insert(key, response, null, DateTime.MaxValue, TimeSpan.FromSeconds(60));
            }
            return (TResponse)HttpContext.Current.Cache[key];
        }

        protected bool IsVisibleExceptionRemoute { get; set; }
        public TRepository Repository { get; set; }
        public Wrapper<ILog> Logger { get; set; }
        public IVersionRepository VersionRepository { get; set; }

        private static readonly object LockObject = new object();
    }
}
